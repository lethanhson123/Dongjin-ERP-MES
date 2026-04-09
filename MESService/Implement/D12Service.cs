

namespace MESService.Implement
{
    public class D12Service : BaseService<torderlist, ItorderlistRepository>
    , ID12Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D12Service(ItorderlistRepository torderlistRepository

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

                if (BaseParameter.Code == "LoadHistory")
                {
                    // thúc hiện tìm kiếm thông tin thông thường
                    result = await LoadFG(BaseParameter);
                }
                else if (BaseParameter.Code == "LoadAbility")
                {
                    result = await LoadAbility(BaseParameter);
                }
                else if (BaseParameter.Code == "LoadRework")
                {
                    result = await LoadRework(BaseParameter);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> LoadRework(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            string ADATE = baseParameter.FromDate.Value.ToString("yyyy-MM-dd");
            string BDATE = baseParameter.ToDate.Value.ToString("yyyy-MM-dd");

            string Selector = baseParameter.FilterType.Replace("'", "") ;
            string SERIALID = "%" + baseParameter.PackingLotCode.Replace("'", "") + "%";  
            string LPARKNO = "%" + baseParameter.PartNo.Replace("'", "") + "%";
            string LotCode = "%" + baseParameter.LotCode.Replace("'", "") + "%";
            string Stage = "%" + baseParameter.LeadData.ToString().Replace("'", "") + "%";
            string TBPART_NM = "%" + baseParameter.PartName.Replace("'", "") + "%";
        

            string query = "";

            if (Selector == "N") // recive
            {
                query = $@"SELECT  (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_NO`, 
                        (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_NM`, 
                        (SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_CAR`, 
                        (SELECT tspart.`PART_FML` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_FML`, 
                        tdpdmtim.`VLID_PART_SNP` AS SNP_QTY, tdpdmtim.`VLID_GRP` AS PKG_GRP, 
                        (SELECT tiivtr.`QTY` FROM tiivtr WHERE tiivtr.LOC_IDX = '2' AND tiivtr.`PART_IDX` = tdpdmtim.VLID_PART_IDX) AS `QTY`, 
                        tdpdmtim.`VLID_DTM` AS MTIN_DTM, tdpdmtim.`CREATE_DTM`, tdpdmtim.`VLID_BARCODE` AS LOT_CODE, tdpdmtim.`VLID_REMARK` AS REMARK, tdpdmtim.`CREATE_USER`, 
                        tdpdmtim.`VLID_DSCN_YN` as DSCN_YN , tdpdmtim.`PDMTIN_IDX`, tdpdmtim.`VLID_PART_IDX`
                        FROM tdpdmtim
                        WHERE   tdpdmtim.`VLID_DSCN_YN` = 'N' AND tdpdmtim.`VLID_DTM` <= '{ADATE}' AND  tdpdmtim.`VLID_DTM` >= '{BDATE}' AND   tdpdmtim.`VLID_GRP` LIKE '{SERIALID}'  
                        AND  tdpdmtim.`VLID_BARCODE` LIKE  '{LotCode}'  
                        HAVING `PART_NO` LIKE '{LPARKNO}' AND   `PART_CAR` LIKE '{Stage}' AND   `PART_NM` LIKE '{TBPART_NM}'    
                        ORDER BY `VLID_DTM` DESC, `PART_NO` ";
            }
            else if (Selector == "Y") //re_work
            {
                query = $@"SELECT  (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim_REWORK.`VLID_PART_IDX`) AS `PART_NO`, 
                            (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim_REWORK.`VLID_PART_IDX`) AS `PART_NM`, 
                            (SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim_REWORK.`VLID_PART_IDX`) AS `PART_CAR`, 
                            (SELECT tspart.`PART_FML` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim_REWORK.`VLID_PART_IDX`) AS `PART_FML`, 
                            tdpdmtim_REWORK.`VLID_PART_SNP` AS SNP_QTY, tdpdmtim_REWORK.`VLID_GRP`  AS PKG_GRP, 
                            (SELECT tiivtr.`QTY` FROM tiivtr WHERE tiivtr.LOC_IDX = '2' AND tiivtr.`PART_IDX` = tdpdmtim_REWORK.VLID_PART_IDX) AS `QTY`, 
                            tdpdmtim_REWORK.`VLID_DTM` AS MTIN_DTM , tdpdmtim_REWORK.`CREATE_DTM`, tdpdmtim_REWORK.`VLID_BARCODE`  AS LOT_CODE, tdpdmtim_REWORK.`VLID_REMARK` AS REMARK, tdpdmtim_REWORK.`CREATE_USER`, 
                            tdpdmtim_REWORK.`VLID_DSCN_YN` as DSCN_YN, tdpdmtim_REWORK.`PDMTIN_IDX`, tdpdmtim_REWORK.`VLID_PART_IDX`
                            FROM tdpdmtim_REWORK
                            WHERE    tdpdmtim_REWORK.`VLID_DTM` <= '{ADATE}' AND  tdpdmtim_REWORK.`VLID_DTM` >= '{BDATE}' AND   tdpdmtim_REWORK.`VLID_GRP` LIKE '{SERIALID}' 
                            AND  tdpdmtim_REWORK.`VLID_BARCODE` LIKE  '{LotCode}' 
                            HAVING `PART_NO` LIKE '{LPARKNO}' AND   `PART_CAR` LIKE '{Stage}' AND   `PART_NM` LIKE '{TBPART_NM}'    
                            ORDER BY `VLID_DTM` DESC, `PART_NO`  ";

            }

            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);
            result.ListtspartTranfer = new List<tspartTranfer>();

            foreach (DataTable dt in ds.Tables)
            {
                result.ListtspartTranfer.AddRange(SQLHelper.ToList<tspartTranfer>(dt));
            }

            return result;
        }

        private async Task<BaseResult> LoadAbility(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            string ADATE = baseParameter.FromDate.Value.ToString("yyyy-MM-dd");
            string BDATE = baseParameter.ToDate.Value.ToString("yyyy-MM-dd");
   
            string catalog = "%" + baseParameter.FilterType.Replace("'", "") + "%";
       
            string TBPART_NM = "%" + baseParameter.PartName.Replace("'", "") + "%";
            string TBPART_CLOG = "%" + baseParameter.LeadData.ToString().Replace("'", "") + "%";
  
            string query = $@"SELECT  
                            `TB_AA`.`CATALOG`,  `TB_AA`.`STAGE`,  `TB_AA`.`PART_NM`, COUNT(`TB_AA`.`PART_NO`) AS `PN_COUNT`, `TB_AA`.`PART_SNP`, SUM(`TB_AA`.`QTY`) AS `SUM_QTY`, SUM(`TB_AA`.`BOX`) AS `SUM_BOX`,
                            MIN(`TB_AA`.`MIN`) AS `MIN`, MAX(`TB_AA`.`MAX`) AS `MAX`, TIMESTAMPDIFF(HOUR, MIN(`TB_AA`.`MIN`), MAX(`TB_AA`.`MAX`))  AS `TIME`
                            FROM (SELECT (SELECT tspart.`PART_FML` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `CATALOG`, 
                            (SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `STAGE`, 
                            (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_NM`,
                            (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_NO`,
                            (SELECT tspart.`PART_SNP` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_SNP`,
                            COUNT(tdpdmtim.`VLID_BARCODE`) AS `QTY`,
                            CEILING(COUNT(tdpdmtim.`VLID_BARCODE`)/(SELECT tspart.`PART_SNP` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `BOX`,
                            MIN(tdpdmtim.CREATE_DTM) AS `MIN`,  MAX(tdpdmtim.CREATE_DTM) AS `MAX`  
                            FROM      tdpdmtim
                            WHERE    tdpdmtim.`VLID_DTM` <= '{BDATE}' AND  tdpdmtim.`VLID_DTM` >= '{ADATE}'
                            GROUP BY `PART_NO`) `TB_AA`
                            GROUP BY `TB_AA`.`CATALOG`, `TB_AA`.`STAGE`,  `TB_AA`.`PART_NM`
                            HAVING `TB_AA`.`CATALOG` LIKE '{catalog}' AND  `TB_AA`.`STAGE` LIKE '{TBPART_CLOG}' AND  `TB_AA`.`PART_NM` LIKE '{TBPART_NM}'
                            ORDER BY `TB_AA`.`CATALOG`, `TB_AA`.`STAGE`,  `TB_AA`.`PART_NM`";
           
            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);
            result.ListtspartTranfer = new List<tspartTranfer>();

            foreach (DataTable dt in ds.Tables)
            {
                result.ListtspartTranfer.AddRange(SQLHelper.ToList<tspartTranfer>(dt));
            }

            return result;


        }

        private async Task<BaseResult> LoadFG(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            string ADATE = baseParameter.FromDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            string BDATE = baseParameter.ToDate.Value.AddHours(23).AddMinutes(59).AddSeconds(59).ToString("yyyy-MM-dd HH:mm:ss");

            string LPARKNO = "%" + baseParameter.PartNo.Replace("'", "") + "%";
            string SERIALID = "%" + baseParameter.PackingLotCode.Replace("'", "") + "%";
            string TBPART_NM = "%" + baseParameter.PartName.Replace("'", "") + "%";
            string TBPART_CLOG = "%" + baseParameter.LeadData.ToString().Replace("'", "") + "%";      
            string query = "";

            if (baseParameter.FilterType == "Y")
            {
                query = $@"SELECT mainTB.* From
                           ( SELECT 
                            tspart.PART_NO,
                            tspart.PART_NM,
                            tspart.PART_CAR,
                            tspart.PART_FML,
                            tdpdmtim.VLID_PART_SNP AS SNP_QTY,
                            tdpdmtim.VLID_GRP AS PKG_GRP,
                            tiivtr.QTY,
                            tdpdmtim.VLID_DTM AS MTIN_DTM,
                            TRIM(tdpdmtim.VLID_BARCODE) AS LOT_CODE,
                            tdpdmtim.VLID_REMARK AS REMARK,
                            tdpdmtim.CREATE_USER,
                            tdpdmtim.VLID_DSCN_YN AS DSCN_YN,
                            tdpdmtim.PDMTIN_IDX,
                            tdpdmtim.VLID_PART_IDX,
                            tdpdmtim.CREATE_DTM,
                            null AS DeleteTime,
	                         null AS DeleteBy
                        FROM tdpdmtim
                        JOIN tspart ON tspart.PART_IDX = tdpdmtim.VLID_PART_IDX
                        LEFT JOIN tiivtr ON tiivtr.PART_IDX = tdpdmtim.VLID_PART_IDX AND tiivtr.LOC_IDX = '2'
                        WHERE 
                            tdpdmtim.VLID_DTM BETWEEN '{ADATE}' AND '{BDATE}' and tdpdmtim.VLID_DSCN_YN ='Y') as mainTB
                        WHERE mainTB.`PKG_GRP` LIKE '{SERIALID}' 
                              AND   mainTB.`PART_NO` LIKE '{LPARKNO}'  
                              AND   mainTB.`PART_CAR` LIKE '{TBPART_CLOG}' 
                              AND    mainTB.`PART_NM` LIKE '{TBPART_NM}'    
                        ORDER BY  mainTB.`MTIN_DTM` DESC, mainTB.`PART_NO`, mainTB.PKG_GRP ";
            }

            else if (baseParameter.FilterType == "N")
            {               
                query = $@"SELECT mainTB.* From
                           ( SELECT 
                            tspart.PART_NO,
                            tspart.PART_NM,
                            tspart.PART_CAR,
                            tspart.PART_FML,
                            tdpdmtim.VLID_PART_SNP AS SNP_QTY,
                            tdpdmtim.VLID_GRP AS PKG_GRP,
                            tiivtr.QTY,
                            tdpdmtim.VLID_DTM AS MTIN_DTM,
                            TRIM(tdpdmtim.VLID_BARCODE) AS LOT_CODE,
                            tdpdmtim.VLID_REMARK AS REMARK,
                            tdpdmtim.CREATE_USER,
                            tdpdmtim.VLID_DSCN_YN AS DSCN_YN,
                            tdpdmtim.PDMTIN_IDX,
                            tdpdmtim.VLID_PART_IDX,
                            tdpdmtim.CREATE_DTM,
                            null AS DeleteTime,
	                         null AS DeleteBy
                        FROM tdpdmtim
                        JOIN tspart ON tspart.PART_IDX = tdpdmtim.VLID_PART_IDX
                        LEFT JOIN tiivtr ON tiivtr.PART_IDX = tdpdmtim.VLID_PART_IDX AND tiivtr.LOC_IDX = '2'
                        WHERE 
                            tdpdmtim.VLID_DTM BETWEEN '{ADATE}' AND '{BDATE}') as mainTB
                        WHERE mainTB.`PKG_GRP` LIKE '{SERIALID}' 
                              AND   mainTB.`PART_NO` LIKE '{LPARKNO}'  
                              AND   mainTB.`PART_CAR` LIKE '{TBPART_CLOG}' 
                              AND    mainTB.`PART_NM` LIKE '{TBPART_NM}'    
                        ORDER BY  mainTB.`MTIN_DTM` DESC, mainTB.`PART_NO`, mainTB.PKG_GRP ";

            }

            else if (baseParameter.FilterType == "D")
            {

                query = $@"SELECT mainTB.* From
                   ( SELECT 
                    B.PART_NO,
                    B.PART_NM,
                    B.PART_CAR,
                    B.PART_FML,
                    A.VLID_PART_SNP AS SNP_QTY,
                    A.VLID_GRP AS PKG_GRP,
                    C.QTY,
                    D.VLID_DTM AS MTIN_DTM,
                    D.CREATE_DTM,
                    TRIM(A.VLID_BARCODE) AS LOT_CODE,
                    A.VLID_REMARK AS REMARK,
                    D.CREATE_USER,
                    'D' AS DSCN_YN,
                    A.PDMTIN_IDX,
                    A.VLID_PART_IDX,                  
                    A.CREATE_DTM AS DeleteTime,
                    A.CREATE_USER AS DeleteBy
                FROM tdpdmtim_del AS A
                JOIN tspart AS B ON A.VLID_PART_IDX = B.PART_IDX
                LEFT JOIN tiivtr AS C ON A.VLID_PART_IDX = C.PART_IDX AND C.LOC_IDX = '2'
                JOIN tdpdmtim_hist AS D ON A.VLID_PART_IDX = D.VLID_PART_IDX 
                                        AND A.VLID_GRP = D.VLID_GRP 
                                        AND A.VLID_BARCODE = D.VLID_BARCODE
                WHERE 
                    A.VLID_DTM BETWEEN '{ADATE}' AND '{BDATE}') as mainTB
                Where mainTB.PART_NO LIKE '{LPARKNO}'
                    AND mainTB.PART_CAR LIKE '{TBPART_CLOG}'
                    AND mainTB.PART_NM LIKE '{TBPART_NM}'
                    AND mainTB.PKG_GRP LIKE '{SERIALID}'
                ORDER BY mainTB.MTIN_DTM DESC, mainTB.PART_NO, mainTB.PKG_GRP ";
            }
            else
            {

                query = $@"SELECT mainTB.* FROM 
                        (SELECT 
                            tspart.PART_NO,
                            tspart.PART_NM,
                            tspart.PART_CAR,
                            tspart.PART_FML,
                            tdpdmtim.VLID_PART_SNP AS SNP_QTY,
                            tdpdmtim.VLID_GRP AS PKG_GRP,
                            tiivtr.QTY,
                            tdpdmtim.VLID_DTM AS MTIN_DTM,
                            TRIM(tdpdmtim.VLID_BARCODE) AS LOT_CODE,
                            tdpdmtim.VLID_REMARK AS REMARK,
                            tdpdmtim.CREATE_USER,
                            tdpdmtim.VLID_DSCN_YN AS DSCN_YN,
                            tdpdmtim.PDMTIN_IDX,
                            tdpdmtim.VLID_PART_IDX,
                            tdpdmtim.CREATE_DTM ,
                            null AS DeleteTime,
	                         null AS DeleteBy
                        FROM tdpdmtim
                        JOIN tspart ON tspart.PART_IDX = tdpdmtim.VLID_PART_IDX
                        LEFT JOIN tiivtr ON tiivtr.PART_IDX = tdpdmtim.VLID_PART_IDX AND tiivtr.LOC_IDX = '2'
                        WHERE 
                            tdpdmtim.VLID_DTM BETWEEN '{ADATE}' AND '{BDATE}'

                        UNION

                        SELECT 
                            B.PART_NO, B.PART_NM, B.PART_CAR, B.PART_FML,
                            A.VLID_PART_SNP AS SNP_QTY, A.VLID_GRP AS PKG_GRP, C.QTY,
                            D.VLID_DTM AS MTIN_DTM, TRIM(A.VLID_BARCODE) AS LOT_CODE, A.VLID_REMARK AS REMARK,
                            D.CREATE_USER, 'D' AS DSCN_YN, A.PDMTIN_IDX,  A.VLID_PART_IDX,
                            D.CREATE_DTM,  A.CREATE_DTM AS DeleteTime, A.CREATE_USER AS DeleteBy
                        FROM tdpdmtim_del AS A
                        JOIN tspart AS B ON A.VLID_PART_IDX = B.PART_IDX
                        LEFT JOIN tiivtr AS C ON A.VLID_PART_IDX = C.PART_IDX AND C.LOC_IDX = '2'
                        JOIN tdpdmtim_hist AS D ON A.VLID_PART_IDX = D.VLID_PART_IDX 
                                                AND A.VLID_GRP = D.VLID_GRP 
                                                AND A.VLID_BARCODE = D.VLID_BARCODE
                        WHERE 
                            A.VLID_DTM BETWEEN '{ADATE}' AND '{BDATE}'
                            ) AS mainTB
                        WHERE mainTB.PART_NO LIKE '{LPARKNO}'
                            AND mainTB.PART_CAR LIKE '{TBPART_CLOG}'
                            AND mainTB.PART_NM LIKE '{TBPART_NM}'
                            AND mainTB.PKG_GRP LIKE '{SERIALID}' 
                        ORDER BY mainTB.MTIN_DTM DESC, mainTB.PART_NO,mainTB.PKG_GRP";

            }


            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);
            result.ListtspartTranfer = new List<tspartTranfer>();

            foreach (DataTable dt in ds.Tables)
            {
                result.ListtspartTranfer.AddRange(SQLHelper.ToList<tspartTranfer>(dt));
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
                await Task.Run(() => { });
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

