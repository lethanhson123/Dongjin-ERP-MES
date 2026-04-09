namespace MESService.Implement
{
    public class D06Service : BaseService<torderlist, ItorderlistRepository>
    , ID06Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D06Service(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result = await SearchShippingByPage(BaseParameter);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> ExportToExcel(BaseParameter baseParameter)
        {       
            var fromDate = baseParameter.FromDate?.ToString("yyyy-MM-dd") + " 00:00:00";
            var toDate = baseParameter.ToDate?.ToString("yyyy-MM-dd") + " 23:59:59";

            string POShippCode = !string.IsNullOrWhiteSpace(baseParameter.ShippNo) ? $" AND tdpdotpl.PO_CODE LIKE '%{baseParameter.ShippNo.Trim()}%' " : "";
            string PartNo = !string.IsNullOrWhiteSpace(baseParameter.PartNo) ? $" AND tspart.PART_NO LIKE '%{baseParameter.PartNo.Trim()}%' " : "";
            string PartName = !string.IsNullOrWhiteSpace(baseParameter.PartName) ? $" AND tspart.PART_NM LIKE '%{baseParameter.PartName.Trim()}%' " : "";
            string PalletNo = !string.IsNullOrWhiteSpace(baseParameter.PalletNo) ? $" AND tdpdotplmu.PLET_NO LIKE '%{baseParameter.PalletNo.Trim()}%' " : "";
            string PackingLot = !string.IsNullOrWhiteSpace(baseParameter.PackingLotCode) ? $" AND tdpdmtim.VLID_GRP LIKE '%{baseParameter.PackingLotCode.Trim()}%' " : "";

            string sql = $@"
        SELECT 
            tdpdmtim.PDOTPL_IDX AS CODE,
            tdpdotpl.PO_CODE,
            IFNULL(tdpdotplmu.PLET_NO, '') AS PLET_NO,
            IFNULL(tdpdotplmu.PLET_COMS, '') AS CM_PALLET_NO,
            tspart.PART_NO,
            tspart.PART_CAR AS PART_GRP,
            tspart.PART_NM,
            tdpdmtim.VLID_PART_SNP AS PART_SNP,
            tdpdmtim.VLID_GRP,   
            tdpdotpl.PO_QTY,
            COUNT(tdpdmtim.VLID_GRP) AS QTY,
            CEILING(COUNT(tdpdmtim.VLID_GRP) / tdpdmtim.VLID_PART_SNP) AS BOX_QTY,
            (tdpdotpl.PO_QTY - IFNULL(CD.CONT, 0)) AS Not_yet_packing,
            COALESCE(tiivtr.QTY, 0) AS Inventory,
           tdpdmtim.CREATE_DTM,
           tdpdmtim.CREATE_USER,
           tdpdmtim.UPDATE_DTM,
           tdpdmtim.UPDATE_USER
        FROM tdpdmtim
        LEFT JOIN tdpdotpl ON tdpdotpl.PDOTPL_IDX = tdpdmtim.PDOTPL_IDX
        LEFT JOIN tdpdotplmu ON tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX
        LEFT JOIN tspart ON tspart.PART_IDX = tdpdmtim.VLID_PART_IDX
        LEFT JOIN tiivtr ON tiivtr.PART_IDX = tdpdmtim.VLID_PART_IDX AND tiivtr.LOC_IDX = '2'
        LEFT JOIN (
            SELECT PDOTPL_IDX, COUNT(*) AS CONT
            FROM tdpdmtim
            GROUP BY PDOTPL_IDX
        ) AS CD ON tdpdmtim.PDOTPL_IDX = CD.PDOTPL_IDX
        WHERE 
            tdpdmtim.TDPDOTPLMU_IDX IS NOT NULL
            AND tdpdmtim.PDOTPL_IDX IS NOT NULL
            AND tdpdmtim.CREATE_DTM BETWEEN '{fromDate}' AND '{toDate}'
            {POShippCode}
            {PartNo}
            {PartName}
            {PalletNo}
            {PackingLot}
        GROUP BY tdpdmtim.VLID_GRP
        ORDER BY PO_CODE, CREATE_DTM DESC, PLET_NO, PART_NO, CODE DESC;
        ";

            var result = new BaseResult();
            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.DataGridView8 = new List<SuperResultTranfer>();

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.DataGridView8.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }  
          
            return result;
        }

        private async Task<BaseResult> SearchShippingByPage(BaseParameter baseParameter)
        {
            var fromDate = baseParameter.FromDate?.ToString("yyyy-MM-dd") + " 00:00:00";
            var toDate = baseParameter.ToDate?.ToString("yyyy-MM-dd") + " 23:59:59";

            string POShippCode = !string.IsNullOrWhiteSpace(baseParameter.ShippNo) ? $" AND tdpdotpl.PO_CODE LIKE '%{baseParameter.ShippNo.Trim()}%' " : "";
            string PartNo = !string.IsNullOrWhiteSpace(baseParameter.PartNo) ? $" AND tspart.PART_NO LIKE '%{baseParameter.PartNo.Trim()}%' " : "";
            string PartName = !string.IsNullOrWhiteSpace(baseParameter.PartName) ? $" AND tspart.PART_NM LIKE '%{baseParameter.PartName.Trim()}%' " : "";
            string PalletNo = !string.IsNullOrWhiteSpace(baseParameter.PalletNo) ? $" AND tdpdotplmu.PLET_NO LIKE '%{baseParameter.PalletNo.Trim()}%' " : "";
            string PackingLot = !string.IsNullOrWhiteSpace(baseParameter.PackingLotCode) ? $" AND tdpdmtim.VLID_GRP LIKE '%{baseParameter.PackingLotCode.Trim()}%' " : "";

            // Câu truy vấn lấy dữ liệu trang hiện tại
            string sql = $@"
    SELECT 
        tdpdmtim.PDOTPL_IDX AS CODE,
        tdpdotpl.PO_CODE,
        IFNULL(tdpdotplmu.PLET_NO, '') AS PLET_NO,
        IFNULL(tdpdotplmu.PLET_COMS, '') AS CM_PALLET_NO,
        tspart.PART_NO,
        tspart.PART_CAR AS PART_GRP,
        tspart.PART_NM,
        tdpdmtim.VLID_PART_SNP AS PART_SNP,
        tdpdmtim.VLID_GRP,
        tdpdmtim.CREATE_DTM,
        tdpdotpl.PO_QTY,
        COUNT(tdpdmtim.VLID_GRP) AS QTY,
        CEILING(COUNT(tdpdmtim.VLID_GRP) / tdpdmtim.VLID_PART_SNP) AS BOX_QTY,
        (tdpdotpl.PO_QTY - IFNULL(CD.CONT, 0)) AS Not_yet_packing,
        COALESCE(tiivtr.QTY, 0) AS Inventory,
       tdpdmtim.CREATE_DTM,
       tdpdmtim.CREATE_USER,
       tdpdmtim.UPDATE_DTM,
       tdpdmtim.UPDATE_USER
    FROM tdpdmtim
    LEFT JOIN tdpdotpl ON tdpdotpl.PDOTPL_IDX = tdpdmtim.PDOTPL_IDX
    LEFT JOIN tdpdotplmu ON tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX
    LEFT JOIN tspart ON tspart.PART_IDX = tdpdmtim.VLID_PART_IDX
    LEFT JOIN tiivtr ON tiivtr.PART_IDX = tdpdmtim.VLID_PART_IDX AND tiivtr.LOC_IDX = '2'
    LEFT JOIN (
        SELECT PDOTPL_IDX, COUNT(*) AS CONT
        FROM tdpdmtim
        GROUP BY PDOTPL_IDX
    ) AS CD ON tdpdmtim.PDOTPL_IDX = CD.PDOTPL_IDX
    WHERE 
        tdpdmtim.TDPDOTPLMU_IDX IS NOT NULL
        AND tdpdmtim.PDOTPL_IDX IS NOT NULL
        AND tdpdmtim.CREATE_DTM BETWEEN '{fromDate}' AND '{toDate}'
        {POShippCode}
        {PartNo}
        {PartName}
        {PalletNo}
        {PackingLot}
    GROUP BY tdpdmtim.VLID_GRP
    ORDER BY PO_CODE, CREATE_DTM DESC, PLET_NO, PART_NO, CODE DESC
    LIMIT {baseParameter.StartNumber}, {baseParameter.Length};
    ";

            // Câu truy vấn để đếm tổng số dòng (phục vụ phân trang)
            string countSql = $@"
    SELECT COUNT(*) AS TotalCount FROM (
        SELECT tdpdmtim.VLID_GRP
        FROM tdpdmtim
        LEFT JOIN tdpdotpl ON tdpdotpl.PDOTPL_IDX = tdpdmtim.PDOTPL_IDX
        LEFT JOIN tdpdotplmu ON tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX
        LEFT JOIN tspart ON tspart.PART_IDX = tdpdmtim.VLID_PART_IDX
        WHERE 
            tdpdmtim.TDPDOTPLMU_IDX IS NOT NULL
            AND tdpdmtim.PDOTPL_IDX IS NOT NULL
            AND tdpdmtim.CREATE_DTM BETWEEN '{fromDate}' AND '{toDate}'
            {POShippCode}
            {PartNo}
            {PartName}
            {PalletNo}
            {PackingLot}
        GROUP BY tdpdmtim.VLID_GRP
    ) AS countTable;
    ";

            var result = new BaseResult();
            result.DataGridView8 = new List<SuperResultTranfer>();

            // Lấy dữ liệu chính
            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.DataGridView8.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }

            // Lấy tổng số dòng
            DataSet countDs = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, countSql);
            if (countDs.Tables[0].Rows.Count > 0)
            {
                result.TotalCount = Convert.ToInt32(countDs.Tables[0].Rows[0]["TotalCount"]);
            }

            return result;
        }


        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {             
                result = await ExportToExcel(BaseParameter);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}

