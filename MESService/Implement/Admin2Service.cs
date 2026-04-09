namespace MESService.Implement
{
    public class Admin2Service : BaseService<tsmenu, ItsmenuRepository>
  , Iadmin2Service
    {
        private readonly ItsmenuRepository _torderlistRepository;
        public Admin2Service(ItsmenuRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(tsmenu model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                result.ListtsmenuTranfer = await LoadMenuGroup();

                result.ListtsauthTranfer = await LoadGroupPermission();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<List<tsauthTranfer>?> LoadGroupPermission()
        {
            var result = new List<tsauthTranfer>();

            string sql = @"SELECT  * FROM TSAUTH ORDER BY `AUTH_IDX`";

            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.AddRange(SQLHelper.ToList<tsauthTranfer>(dt));
            }

            return result;
        }

        private async Task<List<tsmenuTranfer>> LoadMenuGroup()
        {
            var result = new List<tsmenuTranfer>();
            string sql = @"SELECT * FROM tsmenu  WHERE tsmenu.MENU_LVL = '1' ORDER BY tsmenu.MENU_NM_VIE";

            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.AddRange(SQLHelper.ToList<tsmenuTranfer>(dt));
            }

            return result;
        }

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result = await DGV_DATA_CHG(BaseParameter);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> DGV_DATA_CHG(BaseParameter baseParameter)
        {
            var result = new BaseResult();

            try
            {
                string CB_SQL = "";
                int AAA = baseParameter.tsauthTranfer.AUTH_IDX.Value;
                string AA_ComBO = baseParameter.ComboBox1.Trim();

                if (AA_ComBO == "ALL")
                    AA_ComBO = "";

                switch (AA_ComBO)
                {
                    case "0":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '0'  AND  `MENU_NM_VIE` <= '0'  ";
                        break;
                    case "1":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '1'  AND  `MENU_NM_VIE` <= '1'  ";
                        break;
                    case "2":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '2'  AND  `MENU_NM_VIE` <= '2'  ";
                        break;
                    case "3":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '3'  AND  `MENU_NM_VIE` <= '3'  ";
                        break;
                    case "4":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '4'  AND  `MENU_NM_VIE` <= '4'  ";
                        break;
                    case "5":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '5'  AND  `MENU_NM_VIE` <= '5'  ";
                        break;
                    case "6":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '6'  AND  `MENU_NM_VIE` <= '6'  ";
                        break;
                    case "7":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '7'  AND  `MENU_NM_VIE` <= '7'  ";
                        break;
                    case "8":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '8'  AND  `MENU_NM_VIE` <= '8'  ";
                        break;
                    case "9":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '9'  AND  `MENU_NM_VIE` <= '9'  ";
                        break;
                    case "10":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '10'  AND  `MENU_NM_VIE` <= '10'  ";
                        break;                     
                    case "50":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '50'  AND  `MENU_NM_VIE` <= '75'  ";
                        break;
                    case "76":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '76'  AND  `MENU_NM_VIE` <= '99'  ";
                        break;
                    case "100":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '100'  AND  `MENU_NM_VIE` <= '199'  ";
                        break;
                    case "200":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '200'  AND  `MENU_NM_VIE` <= '299'  ";
                        break;
                    case "300":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '300'  AND  `MENU_NM_VIE` <= '399'  ";
                        break;
                    case "400":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '400'  AND  `MENU_NM_VIE` <= '499'  ";
                        break;
                    case "500":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '500'  AND  `MENU_NM_VIE` <= '599'  ";
                        break;
                    case "600":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '600'  AND  `MENU_NM_VIE` <= '699'  ";
                        break;
                    case "700":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '700'  AND  `MENU_NM_VIE` <= '749'  ";
                        break;
                    case "820":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '820'  AND  `MENU_NM_VIE` <= '899'  ";
                        break;
                    case "900":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '900'  AND  `MENU_NM_VIE` <= '945'  ";
                        break;
                    case "950":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '950'  AND  `MENU_NM_VIE` <= '999'  ";
                        break;
                    case "1000":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '1000'  AND  `MENU_NM_VIE` <= '1099'  ";
                        break;
                    case "1100":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '1100'  AND  `MENU_NM_VIE` <= '1199'  ";
                        break;
                    case "1200":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '1200'  AND  `MENU_NM_VIE` <= '1299'  ";
                        break;
                    case "1300":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '1300'  AND  `MENU_NM_VIE` <= '1399'  ";
                        break;
                    case "1400":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '1400'  AND  `MENU_NM_VIE` <= '1499'  ";
                        break;
                    case "1500":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '1500'  AND  `MENU_NM_VIE` <= '1599'  ";
                        break;
                    case "1600":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '1600'  AND  `MENU_NM_VIE` <= '1699'  ";
                        break;
                    case "1700":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '1700'  AND  `MENU_NM_VIE` <= '1799'  ";
                        break;
                    case "1800":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '1800'  AND  `MENU_NM_VIE` <= '1899'  ";
                        break;
                    case "1900":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '1900'  AND  `MENU_NM_VIE` <= '1999'  ";
                        break;
                    case "2000":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '2000'  AND  `MENU_NM_VIE` <= '2099'  ";
                        break;
                    // =============== 한국 메뉴 관리
                    case "3000":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '3000'  AND  `MENU_NM_VIE` <= '3099'  ";
                        break;
                    case "3100":
                        CB_SQL = " 	And  `MENU_NM_VIE` >= '3100'  AND  `MENU_NM_VIE` <= '3199'  ";
                        break;
                    case "3200":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '3200'  AND  `MENU_NM_VIE` <= '3299'  ";
                        break;
                    case "3300":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '3300'  AND  `MENU_NM_VIE` <= '3399'  ";
                        break;
                    case "3400":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '3400'  AND  `MENU_NM_VIE` <= '3499'  ";
                        break;
                    case "3500":
                        CB_SQL = " 	AND  `MENU_NM_VIE` >= '3500'  AND  `MENU_NM_VIE` <= '3599'  ";
                        break;
                    default:
                        CB_SQL = "";
                        break;
                }

                try
                {

                    string sql = @"SELECT tsmenu.`MENU_NM_VIE`, tsmenu.`SCRN_PATH`, tsmenu.`MENU_LVL`, tsmenu.`MENU_NM_EN`, tsmenu.`MENU_NM_HAN`,
                                tsmnau.`MENU_AUTH_YN`, tsmnau.`IQ_AUTH_YN`, tsmnau.`RGST_AUTH_YN`, tsmnau.`MDFY_AUTH_YN`, tsmnau.`DEL_AUTH_YN`,
                                tsmnau.`CAN_AUTH_YN`, tsmnau.`EXCL_AUTH_YN`, tsmnau.`DNLD_AUTH_YN`, tsmnau.`PRNT_AUTH_YN`, tsmnau.`ETC1_AUTH_YN`,
                                tsmnau.`ETC2_AUTH_YN`, tsmnau.`ETC3_AUTH_YN`, tsmnau.`MENU_IDX`, tsmnau.`MENU_AUTH_IDX`, tsmenu.`MENU_CD`
                                FROM tsmnau, tsmenu
                                WHERE tsmnau.`MENU_IDX` = tsmenu.`MENU_IDX`
                                  AND (tsmnau.`AUTH_IDX` = '" + AAA + @"')
                                  AND tsmenu.`DECYN` = 'Y' " + CB_SQL + @" 
                                ORDER BY tsmenu.`MENU_NM_VIE`,MENU_LVL,SCRN_PATH ";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.ListSuperResultTranfer = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.ListSuperResultTranfer.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                }
                catch (Exception ex)
                {
                    result.Message = ex.Message;
                }

            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }

        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.Code.ToUpper() == "ADD")
                {
                    result = await AddNewPermissionGroup(BaseParameter);
                } 
                else if(BaseParameter.Code.ToUpper() == "EDIT")
                {
                    result = await EditPermissionGroup(BaseParameter);
                }             
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> EditPermissionGroup(BaseParameter baseParameter)
        {
            var result = new BaseResult();
            var sql = @"UPDATE `TSAUTH` SET `AUTH_ID`='" + baseParameter.GroupCode + "', `AUTH_NM`='" + baseParameter.GroupName + "' WHERE  `AUTH_IDX`=" + baseParameter.ID;
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql);
            result.tsauthTranfer = new tsauthTranfer();
            result.tsauthTranfer.AUTH_IDX = baseParameter.ID;
            result.tsauthTranfer.AUTH_ID = baseParameter.GroupCode;
            result.tsauthTranfer.AUTH_NM = baseParameter.GroupName;
            result.Message = "Edited";
            return result;

        }

        private async Task<BaseResult> DeletePermissionGroup(BaseParameter baseParameter)
        {
            var result = new BaseResult();
            var sql = "DELETE FROM tsmnau WHERE  `AUTH_IDX` = " + baseParameter.ID;
            var sql1 = "DELETE FROM tsurau WHERE  `AUTH_IDX` =   " + baseParameter.ID;
            var sql2 = "DELETE FROM TSAUTH WHERE  `AUTH_IDX` =  " + baseParameter.ID;
            //chạy thủ thục xóa trng database
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql);
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql1);
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql2);

            result.tsauthTranfer = new tsauthTranfer();
            result.tsauthTranfer.AUTH_IDX = baseParameter.ID;
            result.tsauthTranfer.AUTH_ID = baseParameter.GroupCode;
            result.tsauthTranfer.AUTH_NM = baseParameter.GroupName;
            result.Code = "deleted";

            return result;
        }

        private async Task<BaseResult> AddNewPermissionGroup(BaseParameter baseParameter)
        {
            var result = new BaseResult();

            var groupcode = baseParameter.GroupCode;
            var groupName = baseParameter.GroupName;
            var userID = baseParameter.USER_ID;

            var sql = @"INSERT INTO `TSAUTH` (`AUTH_ID`, `AUTH_NM`, `CREATE_DTM`, `CREATE_USER`) VALUES 
                        ( @groupcode, @groupName, NOW(), @userID)";

            var parameters1 = new[] { 
                new MySqlParameter("@groupcode", MySqlDbType.VarChar) { Value = groupcode.Trim() },
                new MySqlParameter("@groupName", MySqlDbType.VarChar) { Value = groupName.Trim() },
                new MySqlParameter("@userID", MySqlDbType.VarChar) { Value = userID.Trim() },
            };
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql, parameters1);         

            var sqlGetID = @"SELECT MAX(TSAUTH.AUTH_IDX) AS AUTH_IDX FROM tsauth";
                     
            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlGetID);
            result.tsauthTranfer = new tsauthTranfer();

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.tsauthTranfer.AUTH_IDX = int.Parse(dt.Rows[0][0].ToString()); // lưu thông tin AUTH_IDX mói thêm
            }

            var sql2 = @"INSERT INTO tsmnau (tsmnau.MENU_IDX, tsmnau.AUTH_IDX, tsmnau.CREATE_DTM, tsmnau.CREATE_USER)
                        (SELECT tsmenu.MENU_IDX, @AUTH_IDX , NOW(), '" + userID + "' FROM tsmenu)";

            var parameters3 = new[] {
                new MySqlParameter("@AUTH_IDX", MySqlDbType.Int16) { Value =  result.tsauthTranfer.AUTH_IDX.ToString() }          
            };
            MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, sql2, parameters3);
                  
            result.tsauthTranfer.AUTH_ID = baseParameter.GroupCode;
            result.tsauthTranfer.AUTH_NM = baseParameter.GroupName;
            result.Message = "Add completed";
            return result;

        }

        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
               if(baseParameter.DataGridView.Count > 0)
                {
                    var userID = baseParameter.USER_ID;
                    foreach (var item in baseParameter.DataGridView)
                    {
                        int MENU_AUTH_IDX = Convert.ToInt32(item.MENU_AUTH_IDX);
                        bool YNCHK0 = Convert.ToBoolean(item.MENU_AUTH_YN);
                        bool YNCHK1 = Convert.ToBoolean(item.IQ_AUTH_YN);
                        bool YNCHK2 = Convert.ToBoolean(item.RGST_AUTH_YN);
                        bool YNCHK3 = Convert.ToBoolean(item.MDFY_AUTH_YN);
                        bool YNCHK4 = Convert.ToBoolean(item.DEL_AUTH_YN);
                        bool YNCHK5 = Convert.ToBoolean(item.CAN_AUTH_YN);
                        bool YNCHK6 = Convert.ToBoolean(item.EXCL_AUTH_YN);
                        bool YNCHK7 = Convert.ToBoolean(item.DNLD_AUTH_YN);
                        bool YNCHK8 = Convert.ToBoolean(item.PRNT_AUTH_YN);
                        bool YNCHK9 = Convert.ToBoolean(item.ETC1_AUTH_YN);
                        bool YNCHK10 = Convert.ToBoolean(item.ETC2_AUTH_YN);
                        bool YNCHK11 = Convert.ToBoolean(item.ETC3_AUTH_YN);

                       var SQL1 =   "UPDATE `tsmnau` SET " +
                                   "`MENU_AUTH_YN` = '" + YNCHK0 + "', " +
                                   "`IQ_AUTH_YN` = '" + YNCHK1 + "', " +
                                   "`RGST_AUTH_YN` = '" + YNCHK2 + "', " +
                                   "`MDFY_AUTH_YN` = '" + YNCHK3 + "', " +
                                   "`DEL_AUTH_YN` = '" + YNCHK4 + "', " +
                                   "`CAN_AUTH_YN` = '" + YNCHK5 + "', " +
                                   "`EXCL_AUTH_YN` = '" + YNCHK6 + "', " +
                                   "`DNLD_AUTH_YN` = '" + YNCHK7 + "', " +
                                   "`PRNT_AUTH_YN` = '" + YNCHK8 + "', " +
                                   "`ETC1_AUTH_YN` = '" + YNCHK9 + "', " +
                                   "`ETC2_AUTH_YN` = '" + YNCHK10 + "', " +
                                   "`ETC3_AUTH_YN` = '" + YNCHK11 + "', " +
                                   "`UPDATE_DTM` = NOW(), " +
                                   "`UPDATE_USER` = '" + userID + "' " +
                                   "WHERE `MENU_AUTH_IDX` = " + MENU_AUTH_IDX;

                      MySQLHelper.ExecuteNonQuery(GlobalHelper.MariaDBConectionString, SQL1);

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
                if (BaseParameter.Code == "delete")
                {
                    result = await DeletePermissionGroup(BaseParameter);
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

