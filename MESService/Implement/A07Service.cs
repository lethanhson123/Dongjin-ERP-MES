
namespace MESService.Implement
{
    public class A07Service : BaseService<torderlist, ItorderlistRepository>, IA07Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private List<SuperResultTranfer> _dgvA07_01;

        public A07Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _dgvA07_01 = new List<SuperResultTranfer>();
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
                string sql = @"SELECT CDGR_IDX, CDGR_SYSNOTE, CDGR_NM_HAN, CDGR_NM_EN FROM TSCDGR";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DGV_A07_01 = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DGV_A07_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    _dgvA07_01 = result.DGV_A07_01; // Lưu vào biến instance
                }
                else
                {
                    result.Error = "Không có dữ liệu trả về từ cơ sở dữ liệu.";
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Mã lỗi: 99990, Không thể kết nối tới cơ sở dữ liệu MES: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter?.SearchString == null)
                {
                    result.Error = "Yêu cầu phải có SearchString.";
                    return result;
                }

                string cdgr_idx = BaseParameter.SearchString;
                // Truy vấn TSCODE với tham số hóa
                string sql = @"SELECT CD_IDX, CD_SYS_NOTE, CD_NM_HAN, CD_NM_EN FROM TSCODE WHERE CDGR_IDX = @CDGR_IDX";
                var parameters = new List<MySqlParameter> { new MySqlParameter("@CDGR_IDX", cdgr_idx) };
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters.ToArray());

                result.DataGridView1 = new List<SuperResultTranfer>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
                }

                if (_dgvA07_01 == null || !_dgvA07_01.Any())
                {
                    // Nếu _dgvA07_01 rỗng, truy vấn trực tiếp TSCDGR
                    sql = @"SELECT CDGR_IDX, CDGR_SYSNOTE, CDGR_NM_HAN, CDGR_NM_EN FROM TSCDGR WHERE CDGR_IDX = @CDGR_IDX";
                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters.ToArray());
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        var row = ds.Tables[0].Rows[0];
                        result.Data = new
                        {
                            CDGR_IDX = row["CDGR_IDX"],
                            CDGR_SYSNOTE = row["CDGR_SYSNOTE"],
                            CDGR_NM_HAN = row["CDGR_NM_HAN"],
                            CDGR_NM_EN = row["CDGR_NM_EN"]
                        };
                    }
                    else
                    {
                        result.Error = $"Không tìm thấy TSCDGR với CDGR_IDX: {cdgr_idx}";
                    }
                }
                else
                {
                    var tscdgr = _dgvA07_01.Find(item => item.CDGR_IDX.ToString() == cdgr_idx);
                    if (tscdgr != null)
                    {
                        result.Data = new
                        {
                            CDGR_IDX = tscdgr.CDGR_IDX,
                            CDGR_SYSNOTE = tscdgr.CDGR_SYSNOTE,
                            CDGR_NM_HAN = tscdgr.CDGR_NM_HAN,
                            CDGR_NM_EN = tscdgr.CDGR_NM_EN
                        };
                    }
                    else
                    {
                        result.Error = $"Không tìm thấy TSCDGR với CDGR_IDX: {cdgr_idx} trong cache.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Mã lỗi: 99990, Không thể kết nối tới cơ sở dữ liệu MES: {ex.Message}";
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result.Data = "Ready to add new record";
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
                if (BaseParameter != null && BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 4)
                {
                    string cdgr_idx = BaseParameter.ListSearchString[0];
                    string cd_idx = BaseParameter.ListSearchString[1];
                    string cd_nm_han = BaseParameter.ListSearchString[2];
                    string cd_nm_en = BaseParameter.ListSearchString[3];
                    string create_user = BaseParameter.USER_IDX;

                    if (cd_idx == "ERROR" || cd_idx == "Code")
                    {
                        result.Error = "Please check again.";
                        return result;
                    }

                    string sql = @"
                INSERT INTO TSCODE (CDGR_IDX, CD_NM_HAN, CD_NM_EN, CD_SYS_NOTE, CREATE_DTM, CREATE_USER)
                VALUES (@CDGR_IDX, @CD_NM_HAN, @CD_NM_EN, @CD_SYS_NOTE, NOW(), @CREATE_USER)
                ON DUPLICATE KEY UPDATE 
                    CD_NM_HAN = @CD_NM_HAN,
                    CD_NM_EN = @CD_NM_EN,
                    CD_SYS_NOTE = @CD_SYS_NOTE,
                    UPDATE_DTM = NOW(),
                    UPDATE_USER = @CREATE_USER;
            ";

                    var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@CDGR_IDX", cdgr_idx),
                new MySqlParameter("@CD_NM_HAN", cd_nm_han),
                new MySqlParameter("@CD_NM_EN", cd_nm_en),
                new MySqlParameter("@CD_SYS_NOTE", cd_nm_en),
                new MySqlParameter("@CREATE_USER", create_user)
            };

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql, parameters.ToArray());

                    result.Data = "Save database complete";
                }
                else
                {
                    result.Error = "Invalid input parameters.";
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Error Code: 99990, Unable to connect to MES database: {ex.Message}";
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
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();

            try
            {
                if (BaseParameter?.ListSearchString != null && BaseParameter.ListSearchString.Count >= 3)
                {
                    string cdgr_idx = BaseParameter.ListSearchString[0];
                    string cdgr_nm_han = BaseParameter.ListSearchString[1];
                    string cdgr_nm_en = BaseParameter.ListSearchString[2];
                    string create_user = BaseParameter.USER_IDX;

                    if (cdgr_idx == "ERROR" || cdgr_idx == "Code" ||
                        string.IsNullOrWhiteSpace(cdgr_nm_han) || string.IsNullOrWhiteSpace(cdgr_nm_en))
                    {
                        result.Error = "Please check again.";
                        return result;
                    }

                    string sql = @"
                INSERT INTO TSCDGR (CDGR_NM_HAN, CDGR_NM_EN, CREATE_DTM, CREATE_USER)
                VALUES (@CDGR_NM_HAN, @CDGR_NM_EN, NOW(), @CREATE_USER)
                ON DUPLICATE KEY UPDATE
                    CDGR_NM_HAN = @CDGR_NM_HAN,
                    CDGR_NM_EN = @CDGR_NM_EN,
                    UPDATE_DTM = NOW(),
                    UPDATE_USER = @CREATE_USER;
            ";

                    var parameters = new List<MySqlParameter>
            {
                new MySqlParameter("@CDGR_NM_HAN", cdgr_nm_han),
                new MySqlParameter("@CDGR_NM_EN", cdgr_nm_en),
                new MySqlParameter("@CREATE_USER", create_user)
            };

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql, parameters.ToArray());

                    result.Data = "Save database complete";
                }
                else
                {
                    result.Error = "Invalid parameters provided.";
                }
            }
            catch (Exception ex)
            {
                result.Error = $"Error Code: 99990, Unable to connect to MES database: {ex.Message}";
            }

            return result;
        }

    }
}