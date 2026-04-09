namespace MESService.Implement
{
    public class C16Service : BaseService<torderlist, ItorderlistRepository>, IC16Service
    {
        private readonly ItorderlistRepository _torderlistRepository;

        public C16Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }

        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }

        public async Task<BaseResult> Load()
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

        public async Task<BaseResult> COMLIST_LINE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string sql;
                    if (BaseParameter.RadioButton1 == true)
                    {
                        sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_HAN`, `CD_NM_EN` 
                                FROM TSCODE 
                                WHERE `CDGR_IDX` = '8' AND RIGHT(`CD_NM_HAN`, 3) <= 500 
                                ORDER BY `CD_NM_HAN`";
                    }
                    else
                    {
                        sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_HAN`, `CD_NM_EN` 
                                FROM TSCODE 
                                WHERE `CDGR_IDX` = '8' AND RIGHT(`CD_NM_HAN`, 3) > 500 
                                ORDER BY `CD_NM_HAN`";
                    }

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.cb_Stage1 = new List<SuperResultTranfer>();
                    foreach (DataTable dt in ds.Tables)
                    {
                        result.cb_Stage1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1) 
                    {
                        if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 2)
                        {
                            string TextBox1 = BaseParameter.ListSearchString[0]; 
                            string TextBox2 = BaseParameter.ListSearchString[1]; 
                            string sql = @"SELECT 'ORDER' AS `ORDER`, 
                                          `PART_IDX`, 
                                          'Material' AS `TMMTIN_CODE`, 
                                          `PART_NO`, 
                                          `PART_NM`, 
                                          `PART_FML`, 
                                          IFNULL(`PART_SNP`, 0) AS `PART_SNP`, 
                                          IFNULL((SELECT tiivtr.QTY 
                                                  FROM tiivtr 
                                                  WHERE tiivtr.PART_IDX = tspart.`PART_IDX` AND tiivtr.LOC_IDX='1'), 0) AS `STOCK`
                                          FROM tspart 
                                          WHERE `PART_SCN` = '5' AND `PART_USENY` = 'Y' 
                                          AND `PART_NO` LIKE '%" + TextBox1 + "%'  AND(`PART_NM` LIKE '%" + TextBox2 + "%' OR `PART_FML` LIKE '%" + TextBox2 + "%')";

                            sql += " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView1 = new List<SuperResultTranfer>();
                            foreach (DataTable dt in ds.Tables)
                            {
                                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    else if (BaseParameter.Action == 2) 
                    {
                        if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 5)
                        {
                            string ComboBox2 = BaseParameter.ListSearchString[0]; 
                            string T2_S2 = BaseParameter.ListSearchString[1];
                            string T2_S3 = BaseParameter.ListSearchString[2];
                            string T2_S4 = BaseParameter.ListSearchString[3];
                            string T2_S1 = BaseParameter.ListSearchString[4];

                            T2_S2 = DateTime.Parse(T2_S2).ToString("yyyy-MM-dd");
                            string CB_DATE = "";
                            if (ComboBox2 == "ALL") CB_DATE = "";
                            else if (ComboBox2 == "Y") CB_DATE = " AND `TMMTIN_DSCN_YN` = 'Y'";
                            else if (ComboBox2 == "N") CB_DATE = " AND `TMMTIN_DSCN_YN` = 'N'";

                            string sql = @"SELECT 
                                          `TMMTIN_CNF_YN`, 
                                          `TMMTIN_DSCN_YN`, 
                                          IF(`TMMTIN_CODE` = 'TUBE', 
                                             (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), 
                                             (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NO`, 
                                          `TMMTIN_CODE`, 
                                          `TMMTIN_PART_SNP` AS `SNP`, 
                                          `TMMTIN_QTY` AS `QTY`, 
                                          (SELECT `PART_NM` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`) AS `PART_NAME`, 
                                          `CREATE_DTM`, 
                                          (SELECT `PART_FML` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`) AS `FAMILY`, 
                                          `TMMTIN_DATE` AS `DATE`, 
                                          `TMMTIN_DMM_IDX` AS `CODE`, 
                                          (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '8') AS `STAGE`, 
                                          `TMMTIN_PART` AS `DJG_CODE`
                                          FROM TMMTIN_DMM_CUT 
                                          WHERE `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_DATE` = '" + T2_S2 + "' " + CB_DATE +
                                          "HAVING `STAGE` LIKE '%" + T2_S1 + "%' AND `PART_NO` LIKE '%" + T2_S3 + "%' " +
                                          "AND (`PART_NAME` LIKE '%" + T2_S4 + "%' OR `FAMILY` LIKE '%" + T2_S4 + "%') " +
                                          "ORDER BY `CREATE_DTM` DESC";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.T2_DGV1 = new List<SuperResultTranfer>();
                            foreach (DataTable dt in ds.Tables)
                            {
                                result.T2_DGV1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
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

        public async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
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

        public async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.Action == 1)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.DataGridView2 != null && BaseParameter.DataGridView2.Count > 0)
                    {
                        string VAL_DT = "";
                        string VAL_SUM = "";
                        foreach (var item in BaseParameter.DataGridView2)
                        {
                            var AAA = item.STAGE;
                            var BBB = item.DATE?.ToString("yyyy-MM-dd") ?? ""; 
                            var CCC = item.DJG_CODE; 
                            var DDD = item.PART_SNP; 
                            var EEE = item.QTY; 
                            var FFF = item.TMMTIN_CODE; 

                            if (EEE > 0)
                            {
                                VAL_DT = "((SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` ='" + AAA + "' AND `CDGR_IDX` = '8'), '" +
                                         BBB + "','" + CCC + "', '" + DDD + "', '" + EEE + "', '" + FFF + "', 'Y', 'N', 'N', NOW(), '" + USER_IDX + "')";
                                if (string.IsNullOrEmpty(VAL_SUM))
                                {
                                    VAL_SUM = VAL_DT;
                                }
                                else
                                {
                                    VAL_SUM += ", " + VAL_DT;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(VAL_SUM))
                        {
                            string sql = @"INSERT INTO `TMMTIN_DMM_CUT` (`TMMTIN_DMM_STGC`, `TMMTIN_DATE`, `TMMTIN_PART`, `TMMTIN_PART_SNP`, 
                                          `TMMTIN_QTY`, `TMMTIN_CODE`, `TMMTIN_REC_YN`, `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`, `CREATE_DTM`, `CREATE_USER`) 
                                          VALUES " + VAL_SUM;
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.Success = true;
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

        public async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.Action == 2)
                {
                    string D3_D01 = BaseParameter.SearchString; 
                    string sql = @"UPDATE `TMMTIN_DMM_CUT` SET `TMMTIN_REC_YN`='N' WHERE `TMMTIN_DMM_IDX` ='" + D3_D01 + "'";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
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

        public async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
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

        public async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
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

        public async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
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

        public async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
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

        public async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
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