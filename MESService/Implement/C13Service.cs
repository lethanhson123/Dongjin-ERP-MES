
namespace MESService.Implement
{
    public class C13Service : BaseService<torderlist, ItorderlistRepository>, IC13Service
    {
        private readonly ItorderlistRepository _torderlistRepository;

        public C13Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
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

        public virtual async Task<BaseResult> COMLIST_LINE(BaseParameter BaseParameter)
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
                                WHERE `CDGR_IDX` = '14' AND RIGHT(`CD_SYS_NOTE`, 2) <= 60 
                                ORDER BY `CD_SYS_NOTE`";
                    }
                    else
                    {
                        sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_HAN`, `CD_NM_EN` 
                                FROM TSCODE 
                                WHERE `CD_SYS_NOTE` = 'MP GROUP 00' 
                                UNION 
                                SELECT `CD_IDX`, `CD_SYS_NOTE`, `CD_NM_HAN`, `CD_NM_EN` 
                                FROM TSCODE 
                                WHERE `CDGR_IDX` = '14' AND RIGHT(`CD_SYS_NOTE`, 2) < 99 AND RIGHT(`CD_SYS_NOTE`, 2) > 60 
                                ORDER BY `CD_SYS_NOTE`";
                    }

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.ComboBox1 = new List<SuperResultTranfer>();
                    result.T2_S1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        result.T2_S1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
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
                if (BaseParameter != null && BaseParameter.Action.HasValue)
                {
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 3)
                        {
                            string ComboBox1 = BaseParameter.ListSearchString[0]; 
                            string TextBox1 = BaseParameter.ListSearchString[1];  
                            string ComboBox3 = BaseParameter.ListSearchString[2]; 

                            if (string.IsNullOrEmpty(ComboBox1) || ComboBox1 == "Select Stage...")
                            {
                                return result; 
                            }

                            string CB_TEXT = ComboBox3 == "ALL" ? "" : ComboBox3;

                            string sql = @"SELECT 'ORDER' AS `ORDER`, 
                                          `LEAD_INDEX` AS `DJG_CODE`, 
                                          `LEAD_SCN` AS `TYPE`, 
                                          `LEAD_PN` AS `LEAD_NO`, 
                                          IFNULL(`BUNDLE_SIZE`, 0) AS `BUNDLE_SIZE`, 
                                          IFNULL((SELECT tiivtr_lead.QTY 
                                                  FROM tiivtr_lead 
                                                  WHERE tiivtr_lead.PART_IDX = torder_lead_bom.`LEAD_INDEX` 
                                                  AND tiivtr_lead.LOC_IDX='3'), 0) AS `STOCK`
                                          FROM torder_lead_bom 
                                          WHERE `DSCN_YN` = 'Y' 
                                          AND `LEAD_PN` LIKE '%" + TextBox1 + "%'  AND `LEAD_SCN` LIKE '%" + CB_TEXT + "%'";

                            sql += " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    else if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 5)
                        {
                            string ComboBox2 = BaseParameter.ListSearchString[0];
                            string ComboBox4 = BaseParameter.ListSearchString[1]; 
                            string T2_S1 = BaseParameter.ListSearchString[2];    
                            string T2_S2 = BaseParameter.ListSearchString[3];    
                            string T2_S3 = BaseParameter.ListSearchString[4];    

                            string CB_DATE = ComboBox2 switch
                            {
                                "ALL" => "",
                                "Y" => " AND `TMMTIN_DSCN_YN` = 'Y'",
                                "N" => " AND `TMMTIN_DSCN_YN` = 'N'",
                                _ => ""
                            };

                            string CB_TEXT = ComboBox4 == "ALL" ? "" : ComboBox4;

                            T2_S2 = DateTime.Parse(T2_S2).ToString("yyyy-MM-dd");

                            string sql = @"SELECT 
                                          `TMMTIN_CNF_YN`, 
                                          `TMMTIN_DSCN_YN`, 
                                          (SELECT `LEAD_SCN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`) AS `TYPE`, 
                                          (SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `TMMTIN_PART`) AS `LEAD_NO`, 
                                          `TMMTIN_PART_SNP` AS `BUNDLE_SIZE`, 
                                          `TMMTIN_QTY` AS `QTY`, 
                                          `CREATE_DTM`, 
                                          `TMMTIN_DATE` AS `DATE`, 
                                          `TMMTIN_DMM_IDX` AS `CODE`, 
                                          (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '14') AS `STAGE`, 
                                          `TMMTIN_PART` AS `DJG_CODE`
                                          FROM TMMTIN_DMM_LEAD 
                                          WHERE `TMMTIN_REC_YN` = 'Y' 
                                          AND `TMMTIN_DATE` = '" + T2_S2 + "' " + CB_DATE +
                                          "HAVING `STAGE` LIKE '%" + T2_S1 + "%'  AND `LEAD_NO` LIKE '%" + T2_S3 + "%' AND `TYPE` LIKE '%" + CB_TEXT + "%' ORDER BY `CREATE_DTM` DESC";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.T2_DGV1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
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
                if (BaseParameter != null && BaseParameter.Action == 1)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.DataGridView2 != null && BaseParameter.DataGridView2.Count > 0)
                    {
                        string VAL_DT = "";
                        string VAL_SUM = "";
                        foreach (var item in BaseParameter.DataGridView2)
                        {
                            string AAA = item.STAGE;   
                            string BBB = item.DATE?.ToString("yyyy-MM-dd"); 
                            string CCC = item.DJG_CODE;  
                            string DDD = item.BUNDLE_SIZE?.ToString(); 
                            double? EEE = item.QTY;    

                            if (EEE.HasValue && EEE > 0)
                            {
                                VAL_DT = $"((SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` = '{AAA}' AND `CDGR_IDX` = '14'), '{BBB}', '{CCC}', '{DDD}', '{EEE}', 'Y', 'N', 'N', NOW(), '{USER_IDX}')";
                                VAL_SUM = VAL_SUM.Length == 0 ? VAL_DT : VAL_SUM + ", " + VAL_DT;
                            }
                        }

                        if (VAL_SUM.Length > 0)
                        {
                            string sql = @"INSERT INTO `TMMTIN_DMM_LEAD` 
                                          (`TMMTIN_DMM_STGC`, `TMMTIN_DATE`, `TMMTIN_PART`, `TMMTIN_PART_SNP`, `TMMTIN_QTY`, 
                                           `TMMTIN_REC_YN`, `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`, `CREATE_DTM`, `CREATE_USER`) 
                                          VALUES " + VAL_SUM;
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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

        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.Action == 2)
                {
                    string D3_D01 = BaseParameter.SearchString; 
                    string sql = @"UPDATE `TMMTIN_DMM_LEAD` 
                                  SET `TMMTIN_REC_YN` = 'N' 
                                  WHERE `TMMTIN_DMM_IDX` = '" + D3_D01 + "'";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }
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