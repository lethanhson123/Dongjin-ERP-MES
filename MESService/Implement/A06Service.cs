namespace MESService.Implement
{
    public class A06Service : BaseService<torderlist, ItorderlistRepository>
    , IA06Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        

        public A06Service(ItorderlistRepository torderlistRepository, IWebHostEnvironment webHostEnvironment)
            : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _webHostEnvironment = webHostEnvironment;
            
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
                string sql = @"SELECT AUTH_IDX, AUTH_ID, AUTH_NM FROM TSAUTH WHERE NOT(AUTH_IDX = 1) ORDER BY AUTH_IDX";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView = SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]);
                result.Success = true;
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
                if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 3)
                {
                    string AA = "%" + BaseParameter.ListSearchString[0] + "%";
                    string BB = "%" + BaseParameter.ListSearchString[1] + "%";
                    string CC = "%" + BaseParameter.ListSearchString[2] + "%";

                    string sql = @"SELECT USER_IDX, USER_ID, USER_NM, PW, Dept, Note 
                                  FROM tsuser 
                                  WHERE USER_ID LIKE '" + AA + "' AND USER_NM LIKE '" + BB + "' AND Dept LIKE '" + CC + "' AND DESC_YN = 'Y'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView = SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]);
                    result.Success = true;
                }
                else
                {
                    result.Error = "Dữ liệu tìm kiếm không đầy đủ.";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            return Task.FromResult(new BaseResult { Success = true });
        }

        public async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();

            try
            {
                if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 7)
                {
                    string AAA = BaseParameter.ListSearchString[0]; // USER_IDX
                    string BBB = BaseParameter.ListSearchString[1]; // USER_ID
                    string CCC = BaseParameter.ListSearchString[2]; // USER_NM
                    string DDD = BaseParameter.ListSearchString[3]; // PW
                    string EEE = BaseParameter.ListSearchString[4]; // Dept
                    string FFF = BaseParameter.ListSearchString[5]; // Note
                    string ACC_LEV = BaseParameter.ListSearchString[6]; // AUTH_IDX

                    string userId = BaseParameter.USER_IDX ?? "SYSTEM";

                    // Insert or update tsuser
                    string sql = $@"
                INSERT INTO tsuser (USER_ID, USER_NM, PW, Dept, Note, DESC_YN, CREATE_DTM, CREATE_USER)
                VALUES ('{BBB}', '{CCC}', '{DDD}', '{EEE}', '{FFF}', 'Y', NOW(), '{userId}')
                ON DUPLICATE KEY UPDATE 
                    USER_NM = '{CCC}', 
                    PW = '{DDD}', 
                    Dept = '{EEE}', 
                    Note = '{FFF}', 
                    UPDATE_DTM = NOW(), 
                    UPDATE_USER = '{userId}'";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    sql = @"ALTER TABLE tsuser AUTO_INCREMENT = 1";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    // Insert or update tsurau if ACC_LEV exists
                    if (!string.IsNullOrEmpty(ACC_LEV))
                    {
                        sql = $@"
                    INSERT INTO tsurau (USER_IDX, AUTH_IDX, CREATE_DTM, CREATE_USER)
                    VALUES ((SELECT USER_IDX FROM tsuser WHERE USER_ID = '{BBB}'), {ACC_LEV}, NOW(), '{userId}')
                    ON DUPLICATE KEY UPDATE 
                        AUTH_IDX = {ACC_LEV}, 
                        UPDATE_DTM = NOW(), 
                        UPDATE_USER = '{userId}'";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE tsurau AUTO_INCREMENT = 1";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }

                    result.Success = true;
                    result.Message = "Lưu thành công.";
                }
                else
                {
                    result.Error = "Dữ liệu không đầy đủ.";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        public async Task<BaseResult> CheckUserID(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 1)
                {
                    string USER_ID = BaseParameter.ListSearchString[0];
                    string sql = @"SELECT USER_ID, USER_NM, IFNULL(UPDATE_DTM, CREATE_DTM) AS DT 
                                  FROM tsuser 
                                  WHERE USER_ID = '" + USER_ID + "'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView = SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]);
                    result.Success = true;
                }
                else
                {
                    result.Error = "User ID không được để trống.";
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
                if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 1)
                {
                    string AAA = BaseParameter.ListSearchString[0]; // USER_IDX
                    if (string.IsNullOrEmpty(AAA) || int.Parse(AAA) <= 0)
                    {
                        result.Error = "User ID không hợp lệ.";
                        return result;
                    }

                    string sql = @"UPDATE tsuser SET DESC_YN = 'N' WHERE USER_IDX = '" + AAA + "'";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Success = true;
                    result.Message = "Xóa thành công.";
                }
                else
                {
                    result.Error = "Dữ liệu không đầy đủ.";
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
        public Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
        {
            return Task.FromResult(new BaseResult
            {
                Success = true,
                Message = "Mở trang trợ giúp."
            });
        }


        public Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
        {
            return Task.FromResult(new BaseResult { Success = true });
        }
    }
}

