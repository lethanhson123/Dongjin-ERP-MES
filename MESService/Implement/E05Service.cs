namespace MESService.Implement
{
    public class E05Service : BaseService<torderlist, ItorderlistRepository>
    , IE05Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public E05Service(ItorderlistRepository torderlistRepository

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
              
                string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_HAN`, `CD_NM_EN` FROM TSCODE WHERE TSCODE.CDGR_IDX = '21'";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.CB_FCTRY1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.CB_FCTRY1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
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
                if (BaseParameter != null && BaseParameter.ListSearchString != null)
                {
                    string FAC_ST = BaseParameter.ListSearchString[0].ToString() == "Factory 1"
                        ? " AND RIGHT((SELECT `CD_NM_HAN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '8'), 2) <= 60"
                        : " AND RIGHT((SELECT `CD_NM_HAN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '8'), 2) > 60";

                    if (BaseParameter.Action == 1) // TabPage1: DataGridView2
                    {
                        string DateTimePicker2 = DateTime.Parse(BaseParameter.ListSearchString[1].ToString()).ToString("yyyy-MM-dd");
                        string sql = $@"SELECT 
                            (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '8') AS `STAGE`, 
                            `TMMTIN_DATE` AS `DATE`
                            FROM TMMTIN_DMM_APP 
                            WHERE `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_CNF_YN` = 'N' AND `TMMTIN_DATE` = '{DateTimePicker2}' {FAC_ST}
                            GROUP BY `TMMTIN_DATE`, `STAGE`";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    else if (BaseParameter.Action == 2) // TabPage2: T2_DGV1
                    {
                        string T2_S2 = DateTime.Parse(BaseParameter.ListSearchString[1].ToString()).ToString("yyyy-MM-dd");
                        string T2_S3 = BaseParameter.ListSearchString[2].ToString();
                        string T2_S4 = BaseParameter.ListSearchString[3].ToString();
                        string T2_S6 = BaseParameter.ListSearchString[4].ToString();
                        string CB_DATE = T2_S6 == "ALL" ? "" : $" AND `TMMTIN_DSCN_YN` = '{T2_S6}'";

                        string sql = $@"SELECT 
                            `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`, 
                            (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '8') AS `STAGE`, 
                            `TMMTIN_CODE` AS `FAMILY`, 
                            IF(`TMMTIN_CODE` = 'Material', (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`), (SELECT `APPLICATOR` FROM TTOOLMASTER WHERE `TOOL_IDX` = `TMMTIN_PART`)) AS `PART_NO`,
                            IF(`TMMTIN_CODE` = 'Material', (SELECT `PART_NM` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`), 'Applicator') AS `PART_NAME`,
                            `TMMTIN_QTY` AS `QTY`, `CREATE_DTM`, 
                            IF(`TMMTIN_REC_YN` = 'Y', 'ORDER', 'CANCEL') AS `ORDER`, `TMMTIN_SHEETNO`, `TMMTIN_DMM_IDX` AS `CODE`, 
                            `TMMTIN_DATE` AS `DATE`, `TMMTIN_PART` AS `DJG_CODE`
                            FROM TMMTIN_DMM_APP 
                            WHERE `TMMTIN_DATE` = '{T2_S2}' {CB_DATE} {FAC_ST}
                            HAVING `PART_NO` LIKE '%{T2_S3}%' AND (`PART_NAME` LIKE '%{T2_S4}%' OR `FAMILY` LIKE '%{T2_S4}%')
                            ORDER BY IF(`TMMTIN_DSCN_YN` = 'N', 1, IF(`TMMTIN_DSCN_YN` = 'C', 2, 3)) ASC";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.T2_DGV1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.T2_DGV1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    else if (BaseParameter.Action == 3) // TabPage3: DataGridView4
                    {
                        string T3_S2 = DateTime.Parse(BaseParameter.ListSearchString[1].ToString()).ToString("yyyy-MM-dd");
                        string T3_S3 = BaseParameter.ListSearchString[2].ToString();
                        string sql = $@"SELECT 
                            (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '8') AS `STAGE`,
                            `TMMTIN_DSCN_YN` AS `DSCN`, `TMMTIN_DATE` AS `DATE`, `TMMTIN_PART` AS `DJG_CODE`, 
                            IF(`TMMTIN_CODE` = 'Material', (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`), (SELECT `APPLICATOR` FROM TTOOLMASTER WHERE `TOOL_IDX` = `TMMTIN_PART`)) AS `PART_NO`, 
                            IF(`TMMTIN_CODE` = 'Material', (SELECT `PART_NM` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`), 'Applicator') AS `PART_NAME`,
                            `TMMTIN_CODE` AS `FAMILY`, `TMMTIN_PART_SNP` AS `SNP`, SUM(`TMMTIN_QTY`) AS `QTY`, `TMMTIN_SHEETNO`, `TMMTIN_DMM_IDX` AS `CODE`
                            FROM TMMTIN_DMM_APP 
                            WHERE `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_CNF_YN` = 'Y' AND NOT(`TMMTIN_DSCN_YN` = 'Y') AND `TMMTIN_DATE` = '{T3_S2}' {FAC_ST}
                            GROUP BY `STAGE`, `DATE`, `PART_NO`
                            HAVING (`PART_NO` LIKE '%{T3_S3}%' OR `PART_NAME` LIKE '%{T3_S3}%' OR `FAMILY` LIKE '%{T3_S3}%')";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView4 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1 && BaseParameter.DataGridView3 != null && BaseParameter.DataGridView3.Count > 0)
                    {
                      
                        string DGV2_D1 = BaseParameter.ListSearchString[0].ToString();
                        string DGV2_D2 = BaseParameter.ListSearchString[1].ToString();
                        string sql = $@"SELECT IFNULL(MAX(`TMMTIN_SHEETNO`), 0) + 1 AS `SHEET_NO` 
                            FROM `TMMTIN_DMM_APP` 
                            WHERE `TMMTIN_DATE` = '{DGV2_D1}'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        var DGV_NO = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            DGV_NO.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (DGV_NO.Count > 0)
                        {
                            string SHEET_NO = DGV_NO[0].SHEET_NO;
                            foreach (var item in BaseParameter.DataGridView3)
                            {
                                if (item.CHK == true)
                                {
                                    string DJG_CODEIDX = item.DJG_CODE;
                                    sql = $@"UPDATE `TMMTIN_DMM_APP` 
                                        SET `TMMTIN_CNF_YN` = 'Y', `TMMTIN_SHEETNO` = '{SHEET_NO}' 
                                        WHERE `TMMTIN_DMM_STGC` = (SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` = '{DGV2_D2}' AND `CDGR_IDX` = '8')
                                        AND `TMMTIN_DATE` = '{DGV2_D1}' AND `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_CNF_YN` = 'N' AND `TMMTIN_PART` = '{DJG_CODEIDX}'";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                            }
                        }
                    }
                    else if (BaseParameter.Action == 3 && BaseParameter.DataGridView4 != null && BaseParameter.DataGridView4.Count > 0)
                    {
                      
                        foreach (var item in BaseParameter.DataGridView4)
                        {
                            if (item.CHK == true)
                            {
                                string ORDER_CD = item.CODE?.ToString() ?? string.Empty;
                                string sql = $@"UPDATE `TMMTIN_DMM_APP` 
                                    SET `TMMTIN_DSCN_YN` = 'Y' 
                                    WHERE `TMMTIN_DMM_IDX` = '{ORDER_CD}'";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                        }
                    }
                }
                result.Success = true;
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
                if (BaseParameter != null && BaseParameter.DataGridView4 != null && BaseParameter.Action == 3)
                {
                 
                    foreach (var item in BaseParameter.DataGridView4)
                    {
                        if (item.CHK == true)
                        {
                            string ORDER_CD = item.CODE?.ToString() ?? string.Empty;
                            string sql = $@"UPDATE `TMMTIN_DMM_APP` 
                                SET `TMMTIN_DSCN_YN` = 'C' 
                                WHERE `TMMTIN_DMM_IDX` = '{ORDER_CD}'";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                    }
                }
                result.Success = true;
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
        public virtual async Task<BaseResult> DataT2DGV_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ListSearchString != null)
                {
                    string DGV_D1 = DateTime.Parse(BaseParameter.ListSearchString[0].ToString()).ToString("yyyy-MM-dd");
                    string DGV_D2 = BaseParameter.ListSearchString[1].ToString();
                    string sql = $@"SELECT FALSE AS `CHK`, `TMMTIN_DMM_IDX` AS `CODE`,
                        (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '8') AS `STAGE`, 
                        `TMMTIN_DATE` AS `DATE`, `TMMTIN_PART` AS `DJG_CODE`, 'ORDER' AS `ORDER`,
                        IFNULL(`TMMTIN_MTORDR`, 'N') AS `TMMTIN_MTORDR`,
                        IF(`TMMTIN_CODE` = 'Material', (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`), (SELECT `APPLICATOR` FROM TTOOLMASTER WHERE `TOOL_IDX` = `TMMTIN_PART`)) AS `PART_NO`,
                        IF(`TMMTIN_CODE` = 'Material', (SELECT `PART_NM` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`), 'Applicator') AS `PART_NAME`, 
                        `TMMTIN_CODE` AS `FAMILY`, `TMMTIN_PART_SNP` AS `SNP`, IFNULL(SUM(`TMMTIN_QTY`), 0) AS `QTY`,
                        IFNULL((SELECT tiivtr.QTY FROM tiivtr WHERE tiivtr.`PART_IDX` = TMMTIN_DMM_APP.`TMMTIN_PART` AND tiivtr.LOC_IDX='1'), 0) AS `STOCK`
                        FROM TMMTIN_DMM_APP 
                        WHERE `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_CNF_YN` = 'N' AND `TMMTIN_DATE` = '{DGV_D1}'
                        GROUP BY `STAGE`, `DATE`, `PART_NO`
                        HAVING `STAGE` = '{DGV_D2}'";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView3 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}

