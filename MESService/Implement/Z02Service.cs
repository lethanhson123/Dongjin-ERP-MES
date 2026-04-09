namespace MESService.Implement
{
    public class Z02Service : BaseService<torderlist, ItorderlistRepository>
    , IZ02Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public Z02Service(ItorderlistRepository torderlistRepository

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
                // Load ComboBox1 - Department data (TSCODE với CDGR_IDX = 20)
                string DGV_DATA_CB1 = "SELECT TSCODE.CD_NM_HAN FROM TSCODE WHERE TSCODE.CDGR_IDX = 20 ORDER BY TSCODE.CD_NM_HAN";
                DataSet dsDGV_CB1 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA_CB1);

                result.ComboBox1 = new List<SuperResultTranfer>();
                if (dsDGV_CB1.Tables.Count > 0)
                {
                    DataTable dt = dsDGV_CB1.Tables[0];
                    for (int II_1 = 0; II_1 < dt.Rows.Count; II_1++)
                    {
                        result.ComboBox1.Add(new SuperResultTranfer
                        {
                            CD_NM_HAN = dt.Rows[II_1]["CD_NM_HAN"].ToString()
                        });
                    }
                }

                // Load ComboBox3 - Authority data (TSAUTH không phải AUTH_IDX = 1)
                string DGV_DATA_CB2 = "SELECT TSAUTH.AUTH_NM FROM TSAUTH WHERE NOT(TSAUTH.AUTH_IDX = 1) ORDER BY TSAUTH.AUTH_NM";
                DataSet dsDGV_CB2 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA_CB2);

                result.ComboBox3 = new List<SuperResultTranfer>();
                if (dsDGV_CB2.Tables.Count > 0)
                {
                    DataTable dt = dsDGV_CB2.Tables[0];
                    for (int II_2 = 0; II_2 < dt.Rows.Count; II_2++)
                    {
                        result.ComboBox3.Add(new SuperResultTranfer
                        {
                            AUTH_NM = dt.Rows[II_2]["AUTH_NM"].ToString()
                        });
                    }
                }

                // Load ComboBox6 - Authority với IDX cho TabPage3 (T3 콤보박스)
                string DGV_DATA_AM31 = "SELECT `AUTH_IDX`, `AUTH_ID`, `AUTH_NM` FROM TSAUTH WHERE NOT(`AUTH_IDX` = '1') ORDER BY `AUTH_IDX`";
                DataSet dsDGV_AM31 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA_AM31);

                result.ComboBox2 = new List<SuperResultTranfer>(); // Sử dụng ComboBox2 cho ComboBox6 data
                if (dsDGV_AM31.Tables.Count > 0)
                {
                    DataTable dt = dsDGV_AM31.Tables[0];
                    result.ComboBox2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1) // TabPage2 - Search Request
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string AA = BaseParameter.ListSearchString[0]; // DateTimePicker1 (yyyy-MM format)
                            string BB = BaseParameter.ListSearchString[1]; // ComboBox5 - Request Status
                            string CC = BaseParameter.ListSearchString[2]; // TextBox3 - New ID

                            // Build condition cho Request Status
                            string CB01 = "";
                            if (BB == "ALL")
                            {
                                CB01 = "";
                            }
                            else
                            {
                                CB01 = " AND `REQU_DES` = '" + BB + "' ";
                            }

                            // Build SQL query giống VB
                            string DGV_DATA1 = @"SELECT tsuser_REQU.`REQU_DES`, tsuser_REQU.`REQU_APP`, tsuser_REQU.`REQU_DATE`, tsuser_REQU.`REQU_NOTE`, tsuser_REQU.`REQU_NID`,
tsuser_REQU.`REQU_NAME`, tsuser_REQU.`REQU_DEP`, tsuser_REQU.`REQU_TSAUTH`, tsuser_REQU.`REQU_IDX`
FROM tsuser_REQU
WHERE DATE_FORMAT(tsuser_REQU.REQU_DATE, '%Y-%m') = '" + AA + "' AND tsuser_REQU.`REQU_ID` = '" + BaseParameter.USER_IDX + "' AND `REQU_NID` LIKE '%" + CC + "%' " + CB01;

                            DataSet dsDGV_01 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1);
                            result.DataGridView2 = new List<SuperResultTranfer>();
                            if (dsDGV_01.Tables.Count > 0)
                            {
                                DataTable dt = dsDGV_01.Tables[0];
                                result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    else if (BaseParameter.Action == 2) // TabPage3 - User Management
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string RadioButtonFilter = BaseParameter.ListSearchString[0]; // RadioButton1 or RadioButton2

                            // User Management logic
                            string Suck_CK = "";

                            // Kiểm tra RadioButton filter
                            if (RadioButtonFilter == "Pending") // RadioButton1
                            {
                                Suck_CK = "WHERE `REQU_DES` = 'Y' AND `REQU_APP` = 'N' ";
                            }
                            else if (RadioButtonFilter == "All") // RadioButton2
                            {
                                Suck_CK = "";
                            }
                            else
                            {
                                // Default là Pending nếu không có RadioButton nào được chọn
                                Suck_CK = "WHERE `REQU_DES` = 'Y' AND `REQU_APP` = 'N' ";
                            }

                            // Build SQL query cho TabPage3
                            string DGV_DATA3 = @"SELECT tsuser_REQU.`REQU_DES`, tsuser_REQU.`REQU_APP`, tsuser_REQU.`REQU_DATE`, tsuser_REQU.`REQU_NOTE`, tsuser_REQU.`REQU_NID`,
tsuser_REQU.`REQU_NAME`, tsuser_REQU.`REQU_DEP`, tsuser_REQU.`REQU_TSAUTH`, tsuser_REQU.`REQU_IDX`
FROM tsuser_REQU " + Suck_CK;

                            DataSet dsDGV_03 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA3);
                            result.DataGridView3 = new List<SuperResultTranfer>();
                            if (dsDGV_03.Tables.Count > 0)
                            {
                                DataTable dt = dsDGV_03.Tables[0];
                                result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
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
                if (BaseParameter != null)
                {
                    string TabName = BaseParameter.TabName ?? "TabPage1"; // Default tab

                    if (TabName == "TabPage1")
                    {
                        // Tạo item mới cho DataGridView1 giống VB code
                        string AA01 = BaseParameter.USER_IDX ?? ""; // Main.Tooluser.Text
                        string AA02 = DateTime.Now.ToString("yyyy-MM-dd"); // Format(Now(), "yyyy-MM-dd")
                        string AA03 = BaseParameter.TextBox1 ?? ""; // MES ID
                        string AA04 = BaseParameter.TextBox2 ?? ""; // NAME
                        string AA05 = BaseParameter.ComboBox1 ?? ""; // Department
                        string AA06 = BaseParameter.ComboBox2 ?? ""; // Usage Purpose
                        string AA07 = BaseParameter.ComboBox3 ?? ""; // Authority

                        // Tạo item mới để add vào DataGridView1
                        var newItem = new SuperResultTranfer
                        {
                            USER = AA01,        // DGV1_C01: User
                            DateRDCE = AA02,    // DGV1_C02: Date
                            REQU_NID = AA03,    // DGV1_C03: MES ID
                            REQU_NAME = AA04,   // DGV1_C04: Name
                            REQU_DEP = AA05,    // DGV1_C05: Department
                            REQU_NOTE = AA06,   // DGV1_C06: Usage Purpose
                            REQU_TSAUTH = AA07  // DGV1_C07: Authority
                        };

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
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1) // TabPage1 - Save User Request
                    {
                        // Save User Request - batch insert vào tsuser_REQU
                        if (BaseParameter.DataGridView1 != null && BaseParameter.DataGridView1.Count > 0)
                        {
                            string SQL_SUM = "";
                            string SQL_VAL = "";

                            // Build INSERT VALUES cho tất cả rows trong DataGridView1
                            for (int II = 0; II < BaseParameter.DataGridView1.Count; II++)
                            {
                                var item = BaseParameter.DataGridView1[II];

                                string A01 = item.REQU_NID ?? ""; // MES ID
                                string A02 = item.REQU_NAME ?? ""; // Name
                                string A03 = item.REQU_DEP ?? ""; // Department
                                string A04 = item.REQU_NOTE ?? ""; // Usage Purpose
                                string A05 = item.REQU_TSAUTH ?? ""; // Authority

                                SQL_VAL = $" ('{BaseParameter.USER_IDX}', DATE_FORMAT(NOW(), '%Y-%m-%d'), '{A04}', '{A01}', '{A02}', '{A03}', '{A05}', 'Y', 'N', NOW(), '{BaseParameter.USER_IDX}') ";

                                if (string.IsNullOrEmpty(SQL_SUM))
                                {
                                    SQL_SUM = SQL_VAL;
                                }
                                else
                                {
                                    SQL_SUM = SQL_SUM + "," + SQL_VAL;
                                }
                            }

                            // Execute batch INSERT
                            string insertSQL = $"INSERT INTO `tsuser_REQU` (`REQU_ID`, `REQU_DATE`, `REQU_NOTE`, `REQU_NID`, `REQU_NAME`, `REQU_DEP`, `REQU_TSAUTH`, `REQU_DES`, `REQU_APP`, `CREATE_DTM`, `CREATE_USER`) VALUES {SQL_SUM}";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSQL);
                            result.Success = true;
                        }
                    }
                    else if (BaseParameter.Action == 2) // TabPage3 - User Management
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            // User Management - Create user account và set quyền
                            string AAA = BaseParameter.ListSearchString[0]; // Label7 - REQU_IDX
                            string BBB = BaseParameter.ListSearchString[1]; // TextBox5 - User ID
                            string CCC = BaseParameter.ListSearchString[2]; // TextBox6 - User Name
                            string DDD = "1111"; // Default password
                            string EEE = BaseParameter.ListSearchString[3]; // TextBox8 - Department
                            string ACC_LEV = BaseParameter.ListSearchString[4]; // ComboBox6 - Authority Level

                            // 1. Insert/Update user account
                            string userSQL = $@"INSERT INTO tsuser (`USER_ID`, `USER_NM`, `PW`, `Dept`, `Note`, `DESC_YN`, `CREATE_DTM`, `CREATE_USER`) 
VALUES ('{BBB}', '{CCC}', '{DDD}', '{EEE}', '', 'Y', NOW(), '{BaseParameter.USER_IDX}') 
ON DUPLICATE KEY UPDATE `USER_NM` = VALUES(`USER_NM`), `PW` = VALUES(`PW`), `Dept` = VALUES(`Dept`), `Note` = VALUES(`Note`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, userSQL);

                            // 2. Insert/Update user authority
                            string authSQL = $@"INSERT INTO tsurau (`USER_IDX`, `AUTH_IDX`, `CREATE_DTM`, `CREATE_USER`) VALUES 
((SELECT tsuser.USER_IDX FROM tsuser WHERE tsuser.USER_ID = '{BBB}'), {ACC_LEV}, NOW(), '{BaseParameter.USER_IDX}') 
ON DUPLICATE KEY UPDATE `AUTH_IDX`={ACC_LEV}, `UPDATE_DTM`=NOW(), `UPDATE_USER`='{BaseParameter.USER_IDX}'";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, authSQL);

                            // 3. Log user creation (IP tracking)
                            string showup_IP = GetClientIPAddress(); // Helper method để lấy IP
                            string logSQL = $"INSERT INTO `TUSER_LOG_CHK_LIST` (`TS_USERID`, `TS_USER_IP`, `CREATE_DTM`, `CREATE_USER`) VALUES ('{BBB}', '{showup_IP}', NOW(), '{BaseParameter.USER_IDX}')";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, logSQL);

                            // 4. Update request status to approved
                            string updateRequestSQL = $"UPDATE `tsuser_REQU` SET `REQU_DES`= 'Y', `REQU_APP` = 'Y' WHERE `REQU_IDX`= '{AAA}'";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateRequestSQL);

                            // 5. Reset AUTO_INCREMENT (tương tự VB code)
                            try
                            {
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `tsuser` AUTO_INCREMENT= 1");
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, "ALTER TABLE `tsurau` AUTO_INCREMENT= 1");
                            }
                            catch (Exception)
                            {
                                // Ignore ALTER TABLE errors
                            }

                            result.Success = true;
                        }
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
        private string GetClientIPAddress()
        {
            try
            {
            
                return "127.0.0.1"; 
            }
            catch (Exception)
            {
                return "ERROR";
            }
        }
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1) // TabPage1 - Remove from DataGridView1
                    {
                        // Frontend sẽ handle remove row khỏi DataGridView1
                        result.Success = true;
                    }
                    else if (BaseParameter.Action == 2) // TabPage2 - Delete user request
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string CHK_IDX = BaseParameter.ListSearchString[0]; // REQU_DES status (Y/N)
                            string CD_IDX = BaseParameter.ListSearchString[1]; // REQU_IDX

                            if (CHK_IDX == "N")
                            {
                                // Chỉ update database, frontend sẽ handle message
                                string updateSQL = $"UPDATE `tsuser_REQU` SET `REQU_DES`= 'N' WHERE `REQU_IDX`= '{CD_IDX}'";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSQL);
                                result.Success = true;
                            }
                            else
                            {
                                // Return error để frontend handle message
                                result.Success = false;
                                result.Error = "ALREADY_APPROVED"; // Code để frontend biết loại lỗi
                            }
                        }
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1) // TabPage1 - Clear form fields
                    {
                        // Theo VB code: Clear TextBox1, TextBox2, reset ComboBox về index 0
                        // Frontend sẽ handle việc clear form, Service chỉ return success
                        result.Success = true;
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

