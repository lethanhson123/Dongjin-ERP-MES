namespace MESService.Implement
{
    public class Z04_ADMIN_EXCELService : BaseService<torderlist, ItorderlistRepository>
    , IZ04_ADMIN_EXCELService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public Z04_ADMIN_EXCELService(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        if (BaseParameter.DataGridView1 != null)
                        {
                            var AA = BaseParameter.ListSearchString[0];
                            var BB = "";
                            var CC = "";
                            var DD = "";
                            var EE = USER_ID;
                            var FF = "";
                            var GG = "";
                            var TextBox15 = BaseParameter.ListSearchString[1];
                            var MAX_MES_NO = 0;

                            if (BaseParameter.RadioButton3 == true)
                            {
                                MAX_MES_NO = int.Parse(TextBox15);
                            }
                            else
                            {
                                string sql = @"SELECT MAX(tsyear_group_inv.`TSYEAR_MESNO`) +1 AS `MES_NO` FROM tsyear_group_inv 
                                             WHERE tsyear_group_inv.`TSYEAR_YEAR` = '" + AA + "'";

                                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                var DGV_Z04_ADMIN = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    DGV_Z04_ADMIN.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (DGV_Z04_ADMIN.Count > 0 && DGV_Z04_ADMIN[0].MES_NO.HasValue)
                                {
                                    MAX_MES_NO = DGV_Z04_ADMIN[0].MES_NO.Value;
                                }
                                else
                                {
                                    MAX_MES_NO = 1;
                                }
                            }

                            var VALTEXT = "";
                            var SUBTEXT = "";

                            foreach (var item in BaseParameter.DataGridView1)
                            {
                                BB = item.TSYEAR_MESNO.ToString();
                                CC = item.TSYEAR_DEPART;
                                DD = item.TSYEAR_PKILOC;
                                FF = item.TSYEAR_SERIAL_NO1.ToString();
                                GG = item.TSYEAR_SERIAL_NO2.ToString();

                                SUBTEXT = "('" + AA + "', '" + BB + "', '" + CC + "', '" + DD + "', '" + EE + "', '" + FF + "', '" + GG + "', NOW(), '" + USER_ID + "')";

                                if (string.IsNullOrEmpty(VALTEXT))
                                {
                                    VALTEXT = SUBTEXT;
                                }
                                else
                                {
                                    VALTEXT = VALTEXT + "," + SUBTEXT;
                                }
                            }

                            if (string.IsNullOrEmpty(VALTEXT))
                            {
                                result.Error = "[ERROR] VALTEXT is empty after loop";
                                return result;
                            }

                            string sqlInsert = @"INSERT INTO `tsyear_group_inv` (`TSYEAR_YEAR`, `TSYEAR_MESNO`, `TSYEAR_DEPART`, `TSYEAR_PKILOC`, `TSYEAR_INPUTER`, `TSYEAR_SERIAL_NO1`, `TSYEAR_SERIAL_NO2`, `CREATE_DTM`, `CREATE_USER`) 
                            VALUES  " + VALTEXT + " ON DUPLICATE KEY UPDATE `TSYEAR_DEPART` = VALUES(`TSYEAR_DEPART`), `TSYEAR_PKILOC` = VALUES(`TSYEAR_PKILOC`), `TSYEAR_INPUTER` = VALUES(`TSYEAR_INPUTER`), `TSYEAR_SERIAL_NO1` = VALUES(`TSYEAR_SERIAL_NO1`), `TSYEAR_SERIAL_NO2` = VALUES(`TSYEAR_SERIAL_NO2`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlInsert);

                            try
                            {
                                string sqlAlt = @"ALTER TABLE `tsyear_group_inv` AUTO_INCREMENT= 1";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlAlt);
                            }
                            catch { }

                            string sqlHist = @"INSERT INTO `tsyear_group_inv_HIST` (`TSYEAR_YEAR`, `TSYEAR_MESNO`, `TSYEAR_DEPART`, `TSYEAR_PKILOC`, `TSYEAR_INPUTER`, `TSYEAR_SERIAL_NO1`, `TSYEAR_SERIAL_NO2`, `CREATE_DTM`, `CREATE_USER`) 
                            VALUES " + VALTEXT;
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlHist);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message + " | InnerException: " + ex.InnerException?.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> MES_CDD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.RadioButton3 == true)
                    {
                        return result;
                    }

                    var S_Y_D = DateTime.Now.AddMonths(-3);
                    var MES_CODE = "";
                    var AA = S_Y_D.Year;
                    var BB = GlobalHelper.GetQuarterByDateTime(DateTime.Now);

                    if (BaseParameter.RadioButton1 == true)
                    {
                        MES_CODE = AA.ToString();
                    }
                    if (BaseParameter.RadioButton2 == true)
                    {
                        MES_CODE = AA + "-" + BB;
                    }

                    if (!string.IsNullOrEmpty(MES_CODE))
                    {
                        string sql = @"SELECT `TSYEAR_YEAR`, `TSYEAR_MESNO`, `TSYEAR_DEPART`, `TSYEAR_PKILOC`, `TSYEAR_INPUTER`, `TSYEAR_SERIAL_NO1`, `TSYEAR_SERIAL_NO2` 
                        FROM tsyear_group_inv WHERE `TSYEAR_YEAR` = '" + MES_CODE + "'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        var DGV_Z04_09 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            DGV_Z04_09.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        result.Error = MES_CODE;
                        result.ErrorNumber = DGV_Z04_09.Count + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = "";
                result.ErrorNumber = 0;
            }
            return result;
        }
    }
}