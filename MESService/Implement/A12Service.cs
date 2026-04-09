namespace MESService.Implement
{
    public class A12Service : BaseService<torderlist, ItorderlistRepository>
    , IA12Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public A12Service(ItorderlistRepository torderlistRepository

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
                await Task.Run(() => { });
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
                var searchText = BaseParameter.SearchString ?? "";
                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("p_searchText", searchText)
                };

                DataSet ds = await MySQLHelper.FillDataSetAsync(
                    GlobalHelper.MariaDBConectionString,
                    "sp_ShiftTime_Find",
                    parameters
                );

                if (ds != null && ds.Tables.Count > 0)
                {
                    result.ShiftTimes = SQLHelper.ToList<ShiftTime>(ds.Tables[0]);
                }
                else
                {
                    result.ShiftTimes = new List<ShiftTime>();
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ShiftTimes = new List<ShiftTime>();
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string currentUser = BaseParameter.USER_NM ??
                           (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 1 ?
                           BaseParameter.ListSearchString[1] : "SYSTEM");

                TimeSpan startTime = TimeSpan.Parse(BaseParameter.TextBox3 ?? "00:00:00");
                TimeSpan endTime = TimeSpan.Parse(BaseParameter.TextBox4 ?? "00:00:00");
                int breakTime = int.TryParse(BaseParameter.TextBox5, out int bt) ? bt : 0;

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("p_ShiftName", BaseParameter.TextBox2 ?? ""),
                    new MySqlParameter("p_StartTime", startTime.ToString()),
                    new MySqlParameter("p_EndTime", endTime.ToString()),
                    new MySqlParameter("p_BreakTime", breakTime),
                    new MySqlParameter("p_Description", BaseParameter.TextBox6 ?? ""),
                    new MySqlParameter("p_CreateUser", currentUser)
                };

                DataSet ds = await MySQLHelper.FillDataSetAsync(
                    GlobalHelper.MariaDBConectionString,
                    "sp_ShiftTime_Add",
                    parameters
                );

                if (ds != null && ds.Tables.Count > 0)
                {
                    result.ShiftTimes = SQLHelper.ToList<ShiftTime>(ds.Tables[0]);
                }
                else
                {
                    result.ShiftTimes = new List<ShiftTime>();
                }

                result.Success = true;
                result.Message = "Ca làm việc đã được thêm thành công.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ShiftTimes = new List<ShiftTime>();
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string currentUser = BaseParameter.USER_NM ??
                            (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 1 ?
                            BaseParameter.ListSearchString[1] : "SYSTEM");

                TimeSpan startTime = TimeSpan.Parse(BaseParameter.TextBox3 ?? "00:00:00");
                TimeSpan endTime = TimeSpan.Parse(BaseParameter.TextBox4 ?? "00:00:00");
                int breakTime = int.TryParse(BaseParameter.TextBox5, out int bt) ? bt : 0;
                bool active = bool.TryParse(BaseParameter.TextBox7, out bool act) ? act : true;

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("p_ID", Convert.ToInt64(BaseParameter.ID)),
                    new MySqlParameter("p_ShiftName", BaseParameter.TextBox2 ?? ""),
                    new MySqlParameter("p_StartTime", startTime.ToString()),
                    new MySqlParameter("p_EndTime", endTime.ToString()),
                    new MySqlParameter("p_BreakTime", breakTime),
                    new MySqlParameter("p_Description", BaseParameter.TextBox6 ?? ""),
                    new MySqlParameter("p_Active", active ? 1 : 0),
                    new MySqlParameter("p_UpdateUser", currentUser)
                };

                DataSet ds = await MySQLHelper.FillDataSetAsync(
                    GlobalHelper.MariaDBConectionString,
                    "sp_ShiftTime_Update",
                    parameters
                );

                if (ds != null && ds.Tables.Count > 0)
                {
                    result.ShiftTimes = SQLHelper.ToList<ShiftTime>(ds.Tables[0]);
                }
                else
                {
                    result.ShiftTimes = new List<ShiftTime>();
                }

                result.Success = true;
                result.Message = "Ca làm việc đã được cập nhật thành công.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ShiftTimes = new List<ShiftTime>();
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string currentUser = BaseParameter.USER_NM ??
                           (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 1 ?
                           BaseParameter.ListSearchString[1] : "SYSTEM");

                var parameters = new MySqlParameter[]
                {
                    new MySqlParameter("p_ID", Convert.ToInt64(BaseParameter.ID)),
                    new MySqlParameter("p_UpdateUser", currentUser)
                };

                DataSet ds = await MySQLHelper.FillDataSetAsync(
                    GlobalHelper.MariaDBConectionString,
                    "sp_ShiftTime_Delete",
                    parameters
                );

                if (ds != null && ds.Tables.Count > 0)
                {
                    result.ShiftTimes = SQLHelper.ToList<ShiftTime>(ds.Tables[0]);
                }
                else
                {
                    result.ShiftTimes = new List<ShiftTime>();
                }

                result.Success = true;
                result.Message = "Ca làm việc đã được vô hiệu hóa thành công.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ShiftTimes = new List<ShiftTime>();
            }
            return result;
        }
        public virtual async Task<BaseResult> GetAllActiveShifts()
        {
            BaseResult result = new BaseResult();
            try
            {
                var parameters = new MySqlParameter[] { };

                DataSet ds = await MySQLHelper.FillDataSetAsync(
                    GlobalHelper.MariaDBConectionString,
                    "sp_ShiftTime_GetActive",
                    parameters
                );

                if (ds != null && ds.Tables.Count > 0)
                {
                    result.ShiftTimes = SQLHelper.ToList<ShiftTime>(ds.Tables[0]);
                }
                else
                {
                    result.ShiftTimes = new List<ShiftTime>();
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
                result.ShiftTimes = new List<ShiftTime>();
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