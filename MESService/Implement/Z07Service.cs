namespace MESService.Implement
{
    public class Z07Service : BaseService<torderlist, ItorderlistRepository>
    , IZ07Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public Z07Service(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
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
                // Load ComboBox2 - MES Menu
                string DGV_DATA1_1 = @"SELECT '001' AS `SCRN_PATH`, 'New Menu' AS `NAME` UNION 
                    SELECT tsmenu.SCRN_PATH, CONCAT(tsmenu.SCRN_PATH, '  ', tsmenu.MENU_NM_EN) AS `NAME` 
                    FROM tsmenu WHERE tsmenu.MENU_LVL = '2' AND tsmenu.DECYN ='Y' ORDER BY `SCRN_PATH`";
                DataSet dsDGV_01_1 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1_1);

                result.ComboBox2 = new List<SuperResultTranfer>();
                if (dsDGV_01_1.Tables.Count > 0)
                {
                    foreach (DataRow row in dsDGV_01_1.Tables[0].Rows)
                    {
                        result.ComboBox2.Add(new SuperResultTranfer
                        {
                            SCRN_PATH = row["SCRN_PATH"].ToString(),
                            Name = row["NAME"].ToString()
                        });
                    }
                }

                // Load ComboBox4 - Department
                string DGV_DATA_CB4 = @"SELECT CD_SYS_NOTE FROM TSCODE WHERE CDGR_IDX = 20 ORDER BY CD_SYS_NOTE";
                DataSet dsDGV_CB4 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA_CB4);

                result.ComboBox4 = new List<SuperResultTranfer>();
                if (dsDGV_CB4.Tables.Count > 0)
                {
                    foreach (DataRow row in dsDGV_CB4.Tables[0].Rows)
                    {
                        result.ComboBox4.Add(new SuperResultTranfer
                        {
                            CD_SYS_NOTE = row["CD_SYS_NOTE"].ToString()
                        });
                    }
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
              
                bool isRequestMode = BaseParameter.RadioButton1 ?? true; 
                string DATE_MIN = BaseParameter.DateTimePicker1 ?? ""; 
                string DATE_MAX = BaseParameter.DateTimePicker2 ?? ""; 
                string MES_MENU = BaseParameter.ComboBox2 ?? "ALL"; 
                string SUCH1 = BaseParameter.ComboBox4 ?? "ALL";
                string SUCH2 = BaseParameter.TextBox2 ?? ""; 
                string STAY_RUL = BaseParameter.ComboBox3 ?? "ALL"; 
                string SUCH_RUL = BaseParameter.ComboBox1 ?? "ALL"; 
                string SUCH_VER = BaseParameter.TextBox1 ?? ""; 

              
                if (MES_MENU == "ALL") MES_MENU = "%%";
                if (STAY_RUL == "ALL") STAY_RUL = "%%";
                if (SUCH_RUL == "ALL") SUCH_RUL = "%%";

             
                string VALUSE = "";

                if (isRequestMode)
                {
                   
                    if (SUCH1 == "ALL") SUCH1 = "";

                    VALUSE = "WHERE  DATE_FORMAT(ZT_DEVLPMNT_DB.`DELP_DATE`, '%Y-%m-%d') >= '" + DATE_MIN + "' " +
                             "AND DATE_FORMAT(ZT_DEVLPMNT_DB.`DELP_DATE`, '%Y-%m-%d') <= '" + DATE_MAX + "' " +
                             "AND ZT_DEVLPMNT_DB.`DELP_DEPT` LIKE '%" + SUCH1 + "%' " +
                             "AND ZT_DEVLPMNT_DB.`MES_MENU` LIKE '" + MES_MENU + "' " +
                             "AND ZT_DEVLPMNT_DB.`CREATE_USER` LIKE '%" + SUCH2 + "%' " +
                             "AND ZT_DEVLPMNT_DB.`STATE` LIKE '" + STAY_RUL + "' " +
                             "AND ZT_DEVLPMNT_DB.`RESULT` LIKE '" + SUCH_RUL + "'";
                }
                else
                {
                   
                    VALUSE = "WHERE  DATE_FORMAT(ZT_DEVLPMNT_DB.`DONE_DATE`, '%Y-%m-%d') >= '" + DATE_MIN + "' " +
                             "AND DATE_FORMAT(ZT_DEVLPMNT_DB.`DONE_DATE`, '%Y-%m-%d') <= '" + DATE_MAX + "' " +
                             "AND ZT_DEVLPMNT_DB.`MES_VER` LIKE '%" + SUCH_VER + "%' " +
                             "AND ZT_DEVLPMNT_DB.`MES_MENU` LIKE '" + MES_MENU + "' " +
                             "AND ZT_DEVLPMNT_DB.`RESULT` = 'Done'";
                }

              
                string DGV_DATA1 = "SELECT 'Detail' AS `BUTT`, " +
                                   "ZT_DEVLPMNT_DB.`DELP_DATE`, ZT_DEVLPMNT_DB.`DELP_DEPT`, ZT_DEVLPMNT_DB.`MES_MENU`, ZT_DEVLPMNT_DB.`DELP_NAME`, " +
                                   "ZT_DEVLPMNT_DB.`DELP_DETIL`, ZT_DEVLPMNT_DB.`FILE_DSYN`, ZT_DEVLPMNT_DB.`FILE_NM`, " +
                                   "IFNULL(ZT_DEVLPMNT_DB.`UPDATE_DTM`, ZT_DEVLPMNT_DB.`CREATE_DTM`) AS `DATE`, " +
                                   "IFNULL(ZT_DEVLPMNT_DB.`UPDATE_USER`, ZT_DEVLPMNT_DB.`CREATE_USER`) AS `USER`, " +
                                   "ZT_DEVLPMNT_DB.`STATE`, ZT_DEVLPMNT_DB.`PROGRESS`, ZT_DEVLPMNT_DB.`RESULT`, " +
                                   "ZT_DEVLPMNT_DB.`DONE_DATE`, ZT_DEVLPMNT_DB.`MES_VER`, " +
                                   "ZT_DEVLPMNT_DB.`DELP_IDX` " +
                                   "FROM ZT_DEVLPMNT_DB " + VALUSE;

                DataSet dsDGV_01 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1);

                result.DataGridView1 = new List<SuperResultTranfer>();
                if (dsDGV_01.Tables.Count > 0)
                {
                    DataTable dt = dsDGV_01.Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                       
                        DateTime? delpDate = null;
                        if (DateTime.TryParse(dt.Rows[i]["DELP_DATE"].ToString(), out DateTime parsedDelpDate))
                        {
                            delpDate = parsedDelpDate;
                        }

                        DateTime? date = null;
                        if (DateTime.TryParse(dt.Rows[i]["DATE"].ToString(), out DateTime parsedDate))
                        {
                            date = parsedDate;
                        }

                        DateTime? doneDate = null;
                        if (DateTime.TryParse(dt.Rows[i]["DONE_DATE"].ToString(), out DateTime parsedDoneDate))
                        {
                            doneDate = parsedDoneDate;
                        }

                        int? delpIdx = null;
                        if (int.TryParse(dt.Rows[i]["DELP_IDX"].ToString(), out int parsedDelpIdx))
                        {
                            delpIdx = parsedDelpIdx;
                        }

                      
                        SuperResultTranfer item = new SuperResultTranfer
                        {
                          
                            CD_SYS_NOTE = dt.Rows[i]["BUTT"].ToString(),  
                            DELP_DATE = delpDate,
                            DELP_DEPT = dt.Rows[i]["DELP_DEPT"].ToString(),
                            MES_MENU = dt.Rows[i]["MES_MENU"].ToString(),
                            DELP_NAME = dt.Rows[i]["DELP_NAME"].ToString(),
                            DELP_DETIL = dt.Rows[i]["DELP_DETIL"].ToString(),
                            FILE_DSYN = dt.Rows[i]["FILE_DSYN"].ToString(),
                            FILE_NM = dt.Rows[i]["FILE_NM"].ToString(),
                            DATE = date,
                          
                            CREATE_USER = dt.Rows[i]["USER"].ToString(),
                            STATE = dt.Rows[i]["STATE"].ToString(),
                            PROGRESS = dt.Rows[i]["PROGRESS"].ToString(),
                            RESULT = dt.Rows[i]["RESULT"].ToString(),
                            DONE_DATE = doneDate,
                            MES_VER = dt.Rows[i]["MES_VER"].ToString(),
                            DELP_IDX = delpIdx
                        };

                        result.DataGridView1.Add(item);
                    }
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
               
                DateTime now = DateTime.Now;

                result.SuperResultTranfer = new SuperResultTranfer
                {
                    DELP_DATE = now,
                    DELP_DEPT = "",
                    MES_MENU = "",
                    DELP_NAME = "",
                    DELP_DETIL = "",
                    FILE_DSYN = "N",
                    FILE_NM = "",
                    STATE = "Waiting",
                    PROGRESS = "0",
                    RESULT = "Waiting",
                    CREATE_USER = BaseParameter?.USER_IDX ?? ""
                };

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ListSearchString != null)
                {
                    string A01 = BaseParameter.ListSearchString[0] ?? "";
                    string A02 = BaseParameter.ListSearchString[1] ?? ""; 
                    string A03 = BaseParameter.ListSearchString[2] ?? "";
                    string A04 = BaseParameter.ListSearchString[3] ?? ""; 
                    string A05 = BaseParameter.ListSearchString[4] ?? "";
                    string A06 = BaseParameter.ListSearchString[5] ?? ""; 
                    string A07 = BaseParameter.ListSearchString[6] ?? "N";
                    string A08 = BaseParameter.ListSearchString[7] ?? ""; 
                    string A09 = BaseParameter.ListSearchString[8] ?? ""; 
                    string A10 = BaseParameter.ListSearchString[9] ?? "Waiting"; 
                    string A11 = BaseParameter.ListSearchString[10] ?? "0"; 
                    string A12 = BaseParameter.ListSearchString[11] ?? "Waiting"; 
                    string A13 = BaseParameter.ListSearchString[12] ?? ""; 
                    string A14 = BaseParameter.ListSearchString[13] ?? ""; 

                   
                    string CHK_SQL = "SELECT COUNT(*) FROM ZT_DEVLPMNT_DB WHERE DELP_IDX = " + A01;
                    object count = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, CHK_SQL);
                    bool recordExists = Convert.ToInt32(count) > 0;

                    if (recordExists)
                    {
                      
                        string UPD_SQL = @"UPDATE ZT_DEVLPMNT_DB SET 
                    DELP_DATE = '" + A02 + @"', 
                    DELP_DEPT = '" + A03 + @"', 
                    MES_MENU = '" + A04 + @"', 
                    DELP_NAME = '" + A05 + @"', 
                    DELP_DETIL = '" + A06 + @"', 
                    FILE_DSYN = '" + A07 + @"', 
                    FILE_NM = '" + A08 + @"', 
                    STATE = '" + A10 + @"', 
                    PROGRESS = '" + A11 + @"', 
                    RESULT = '" + A12 + @"',";

                     
                        if (!string.IsNullOrEmpty(A13))
                        {
                            UPD_SQL += " DONE_DATE = '" + A13 + "', ";
                        }

                        if (!string.IsNullOrEmpty(A14))
                        {
                            UPD_SQL += " MES_VER = '" + A14 + "', ";
                        }

                        UPD_SQL += @" UPDATE_DTM = NOW(), 
                    UPDATE_USER = '" + BaseParameter.USER_IDX + @"' 
                WHERE DELP_IDX = " + A01;

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, UPD_SQL);
                    }
                    else
                    {
                       
                        string INS_SQL = @"INSERT INTO ZT_DEVLPMNT_DB (
                    DELP_DATE, DELP_DEPT, MES_MENU, DELP_NAME, 
                    DELP_DETIL, FILE_DSYN, FILE_NM, 
                    STATE, PROGRESS, RESULT,";

                 
                        if (!string.IsNullOrEmpty(A13))
                        {
                            INS_SQL += " DONE_DATE,";
                        }

                        if (!string.IsNullOrEmpty(A14))
                        {
                            INS_SQL += " MES_VER,";
                        }

                        INS_SQL += @" CREATE_DTM, CREATE_USER
                ) VALUES (
                    '" + A02 + "', '" + A03 + "', '" + A04 + "', '" + A05 + "', " +
                            "'" + A06 + "', '" + A07 + "', '" + A08 + "', " +
                            "'" + A10 + "', '" + A11 + "', '" + A12 + "', ";

                      
                        if (!string.IsNullOrEmpty(A13))
                        {
                            INS_SQL += "'" + A13 + "', ";
                        }

                        if (!string.IsNullOrEmpty(A14))
                        {
                            INS_SQL += "'" + A14 + "', ";
                        }

                        INS_SQL += "NOW(), '" + (string.IsNullOrEmpty(A09) ? BaseParameter.USER_IDX : A09) + "')";

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, INS_SQL);
                    }

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ListSearchString != null)
                {
                    string delpIdx = BaseParameter.ListSearchString[0]; 

                    if (!string.IsNullOrEmpty(delpIdx))
                    {
                      
                        string DEL_SQL = "DELETE FROM ZT_DEVLPMNT_DB WHERE DELP_IDX = " + delpIdx;
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, DEL_SQL);
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.Error = "DELP_IDX is empty";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.Success = false;
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

