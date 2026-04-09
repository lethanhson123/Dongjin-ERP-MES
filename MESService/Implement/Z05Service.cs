namespace MESService.Implement
{
    public class Z05Service : BaseService<torderlist, ItorderlistRepository>
    , IZ05Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public Z05Service(ItorderlistRepository torderlistRepository

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
                string DGV_DATA_CB1 = "SELECT TSCODE.CD_IDX, TSCODE.CD_SYS_NOTE FROM TSCODE WHERE TSCODE.CDGR_IDX = 20 ORDER BY TSCODE.CD_SYS_NOTE";
                DataSet dsDGV_CB1 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA_CB1);

                // Khởi tạo ComboBox1 để chứa dữ liệu Department
                result.ComboBox1 = new List<SuperResultTranfer>();
                if (dsDGV_CB1.Tables.Count > 0)
                {
                    DataTable dt = dsDGV_CB1.Tables[0];
                    for (int II_1 = 0; II_1 < dt.Rows.Count; II_1++)
                    {
                        int cdIdx;
                        int.TryParse(dt.Rows[II_1]["CD_IDX"].ToString(), out cdIdx);

                        result.ComboBox1.Add(new SuperResultTranfer
                        {
                            CD_IDX = cdIdx,
                            CD_SYS_NOTE = dt.Rows[II_1]["CD_SYS_NOTE"].ToString()
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
                if (BaseParameter != null && BaseParameter.ListSearchString != null)
                {
                    string A01 = BaseParameter.ListSearchString[0] ?? ""; // TextBox5 - Asset ID
                    string A02 = BaseParameter.ListSearchString[1] ?? ""; // TextBox1 - Asset Name
                    string A03 = BaseParameter.ListSearchString[2] ?? ""; // TextBox2 - Specification
                    string A04 = BaseParameter.ListSearchString[3] ?? ""; // ComboBox1 - Department
                    string A05 = BaseParameter.ListSearchString[4] ?? ""; // TextBox3 - User
                    string A06 = BaseParameter.ListSearchString[5] ?? "ALL"; // ComboBox4 - Status

                    // Build condition for Status
                    string CB01 = "";
                    if (A06 != "ALL")
                    {
                        CB01 = " AND tasset.STATUS = '" + A06 + "' ";
                    }

                    // Build condition for Department
                    string CB02 = "";
                    if (A04 != "ALL")
                    {
                        CB02 = " AND tasset.DEPARTMENT = '" + A04 + "' ";
                    }

                    // Build SQL query
                    string DGV_DATA = @"SELECT 
                              tasset.ASSET_NAME, 
                              tasset.SPECIFICATION, 
                              tasset.PURCHASE_DATE, 
                              tasset.DEPARTMENT, 
                              tasset.USER_NAME, 
                              tasset.USE_DATE, 
                              tasset.ASSET_ID, 
                              tasset.STATUS
                            FROM tasset
                            WHERE tasset.ASSET_ID LIKE '%" + A01 + @"%' 
                            AND tasset.ASSET_NAME LIKE '%" + A02 + @"%' 
                            AND tasset.SPECIFICATION LIKE '%" + A03 + @"%' 
                            AND tasset.USER_NAME LIKE '%" + A05 + @"%'" + CB01 + CB02;

                    DataSet dsDGV = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA);
                    result.DataGridView1 = new List<SuperResultTranfer>();
                    if (dsDGV.Tables.Count > 0)
                    {
                        DataTable dt = dsDGV.Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            result.DataGridView1.Add(new SuperResultTranfer
                            {
                                ASSET_NAME = dt.Rows[i]["ASSET_NAME"].ToString(),
                                SPECIFICATION = dt.Rows[i]["SPECIFICATION"].ToString(),
                                PURCHASE_DATE = dt.Rows[i]["PURCHASE_DATE"].ToString(),
                                DEPARTMENT = dt.Rows[i]["DEPARTMENT"].ToString(),
                                USER_NAME = dt.Rows[i]["USER_NAME"].ToString(),
                                USE_DATE = dt.Rows[i]["USE_DATE"].ToString(),
                                ASSET_ID = dt.Rows[i]["ASSET_ID"].ToString(),
                                STATUS = dt.Rows[i]["STATUS"].ToString()
                            });
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
                // Generate new Asset ID using the current timestamp
                string NewAssetID = "ASSET" + DateTime.Now.ToString("yyyyMMddHHmmss");

                // Create a new asset object and set default values
                result.SuperResultTranfer = new SuperResultTranfer
                {
                    ASSET_ID = NewAssetID,
                    ASSET_NAME = "",
                    SPECIFICATION = "",
                    PURCHASE_DATE = DateTime.Now.ToString("yyyy-MM-dd"),
                    DEPARTMENT = "",
                    USER_NAME = "",
                    USE_DATE = DateTime.Now.ToString("yyyy-MM-dd"),
                    STATUS = "Y"
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
                    string A01 = BaseParameter.ListSearchString[0] ?? ""; // TextBox4 - Asset ID
                    string A02 = BaseParameter.ListSearchString[1] ?? ""; // TextBox8 - Asset Name
                    string A03 = BaseParameter.ListSearchString[2] ?? ""; // TextBox7 - Specification
                    string A04 = BaseParameter.ListSearchString[3] ?? ""; // DateTimePicker4 - Purchase Date
                    string A05 = BaseParameter.ListSearchString[4] ?? ""; // ComboBox2 - Department
                    string A06 = BaseParameter.ListSearchString[5] ?? ""; // TextBox6 - User
                    string A07 = BaseParameter.ListSearchString[6] ?? ""; // DateTimePicker3 - Use Start Date
                    string A08 = BaseParameter.ListSearchString[7] ?? "Y"; // ComboBox3 - Status

                    // Check if asset ID already exists
                    string CHK_SQL = "SELECT COUNT(*) FROM tasset WHERE ASSET_ID = '" + A01 + "'";
                    object count = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, CHK_SQL);
                    bool recordExists = Convert.ToInt32(count) > 0;

                    if (recordExists)
                    {
                        // Update existing asset
                        string UPD_SQL = @"UPDATE tasset SET 
                        ASSET_NAME = '" + A02 + @"', 
                        SPECIFICATION = '" + A03 + @"', 
                        PURCHASE_DATE = '" + A04 + @"', 
                        DEPARTMENT = '" + A05 + @"', 
                        USER_NAME = '" + A06 + @"', 
                        USE_DATE = '" + A07 + @"', 
                        STATUS = '" + A08 + @"',
                        UPDATE_DTM = NOW(), 
                        UPDATE_USER = '" + BaseParameter.USER_IDX + @"' 
                      WHERE ASSET_ID = '" + A01 + "'";

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, UPD_SQL);
                    }
                    else
                    {
                        // Insert new asset
                        string INS_SQL = @"INSERT INTO tasset (
                        ASSET_ID, ASSET_NAME, SPECIFICATION, PURCHASE_DATE, 
                        DEPARTMENT, USER_NAME, USE_DATE, STATUS, 
                        CREATE_DTM, CREATE_USER
                      ) VALUES (
                        '" + A01 + "', '" + A02 + "', '" + A03 + "', '" + A04 + "', " +
                                "'" + A05 + "', '" + A06 + "', '" + A07 + "', '" + A08 + "', " +
                                "NOW(), '" + BaseParameter.USER_IDX + "')";

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
                    string assetID = BaseParameter.ListSearchString[0];

                    if (!string.IsNullOrEmpty(assetID))
                    {
                        // Delete the asset record
                        string DEL_SQL = "DELETE FROM tasset WHERE ASSET_ID = '" + assetID + "'";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, DEL_SQL);
                        result.Success = true;
                    }
                    else
                    {
                        result.Success = false;
                        result.Error = "Invalid asset ID.";
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

