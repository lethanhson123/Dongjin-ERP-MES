namespace MESService.Implement
{
    public class Admin3Service : BaseService<tsmenu, ItsmenuRepository>
, Iadmin3Service
    {
        private readonly ItsmenuRepository _torderlistRepository;
        public Admin3Service(ItsmenuRepository torderlistRepository

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
                result = await LoadPermissionGroup();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> LoadPermissionGroup()
        {
            var result = new BaseResult();

            try
            {
                string DGV_DATA1 = @"SELECT  `AUTH_IDX`, `AUTH_ID`, `AUTH_NM`    FROM TSAUTH   ORDER BY `AUTH_IDX`";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1);
                result.ListtsuserTranfer = new List<tsuserTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.ListtsuserTranfer.AddRange(SQLHelper.ToList<tsuserTranfer>(dt));
                }

                result.ErrorNumber = 0;
                result.Message = "successfuly";
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
                result = await SearchUser(BaseParameter);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> SearchUser(BaseParameter baseParameter)
        {
            var result = new BaseResult();
            var user_id = baseParameter.USER_ID;
            var user_name = baseParameter.USER_NM;
            var dept = baseParameter.GroupName;
            var isusing = baseParameter.IsUsing;

            try
            {
                string sql = @"SELECT tsuser.`USER_IDX`, tsuser.`USER_ID`, tsuser.`USER_NM`, tsuser.`Dept`, 
                            tsuser.`Note`, tsurau.`AUTH_IDX`, tsauth.AUTH_ID, tsauth.AUTH_NM, tsuser.`DESC_YN`, 
                            tsuser.CREATE_DTM, tsuser.CREATE_USER, tsuser.UPDATE_DTM, tsuser.UPDATE_USER
                       FROM tsuser 
                       LEFT OUTER JOIN tsurau ON tsuser.`USER_IDX` = tsurau.`USER_IDX`
                       LEFT JOIN tsauth ON tsurau.`AUTH_IDX` = tsauth.AUTH_IDX
                       WHERE tsuser.`USER_ID` LIKE @user_id 
                         AND tsuser.`USER_NM` LIKE @user_name 
                         AND tsuser.`Dept` LIKE @dept 
                         AND tsuser.`DESC_YN` = @isusing";

                var parameters1 = new[] {
            new MySqlParameter("@user_id", MySqlDbType.VarChar) { Value = $"%{user_id.Trim()}%" },
            new MySqlParameter("@user_name", MySqlDbType.VarChar) { Value = $"%{user_name.Trim()}%" },
            new MySqlParameter("@dept", MySqlDbType.VarChar) { Value = $"%{dept.Trim()}%" },
            new MySqlParameter("@isusing", MySqlDbType.VarChar) { Value = isusing.Trim() },
        };

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters1);
                result.ListtsuserTranfer = new List<tsuserTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.ListtsuserTranfer.AddRange(SQLHelper.ToList<tsuserTranfer>(dt));
                }

                result.ErrorNumber = 0;
                result.Message = "successfuly";
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
                result = await RecoveryUsser(BaseParameter);
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
                result = await updatePermission(BaseParameter);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> updatePermission(BaseParameter baseParameter)
        {
            var result = new BaseResult();

            try
            {
                int userIdx = baseParameter.tsuserTranfer.USER_IDX ?? 0;
                int authIdx = baseParameter.tsuserTranfer.AUTH_IDX ?? 0;
                string pw = baseParameter.tsuserTranfer.PW?.Trim() ?? "";
                string note = baseParameter.tsuserTranfer.Note?.Trim() ?? "";
                string user = baseParameter.tsuserTranfer.UPDATE_USER?.Trim() ?? "";

                // Cập nhật mật khẩu nếu có
                if (!string.IsNullOrEmpty(pw))
                {
                    string updatePwSql = @"UPDATE tsuser SET PW = @PW, UPDATE_DTM = NOW(),UPDATE_USER = @UpdateUser WHERE USER_IDX = @USER_IDX";
                    var pwParams = new[]
                    {
                        new MySqlParameter("@PW", MySqlDbType.VarChar) { Value = pw },
                        new MySqlParameter("@USER_IDX", MySqlDbType.Int32) { Value = userIdx },
                        new MySqlParameter("@UpdateUser", MySqlDbType.VarChar) { Value = user }
                    };
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updatePwSql, pwParams);
                }

                // Cập nhật ghi chú nếu có
                if (!string.IsNullOrEmpty(note))
                {
                    string updateNoteSql = @"UPDATE tsuser SET Note = @Note, UPDATE_DTM = NOW(),UPDATE_USER = @UpdateUser WHERE USER_IDX = @USER_IDX";
                    var noteParams = new[]
                    {
                        new MySqlParameter("@Note", MySqlDbType.VarChar) { Value = note },
                        new MySqlParameter("@USER_IDX", MySqlDbType.Int32) { Value = userIdx },
                          new MySqlParameter("@UpdateUser", MySqlDbType.VarChar) { Value = user }
                    };
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateNoteSql, noteParams);
                }

                // Thêm mới hoặc cập nhật quyền
                string permissionSql = @"
                INSERT INTO tsurau (USER_IDX, AUTH_IDX, CREATE_DTM, CREATE_USER)
                VALUES (@USER_IDX, @AUTH_IDX, NOW(), @CREATE_USER)
                ON DUPLICATE KEY UPDATE
                    AUTH_IDX = @AUTH_IDX,
                    UPDATE_DTM = NOW(),
                    UPDATE_USER = @UPDATE_USER";

                var permissionParams = new[]
                {
                    new MySqlParameter("@USER_IDX", MySqlDbType.Int32) { Value = userIdx },
                    new MySqlParameter("@AUTH_IDX", MySqlDbType.Int32) { Value = authIdx },
                    new MySqlParameter("@CREATE_USER", MySqlDbType.VarChar) { Value = user },
                    new MySqlParameter("@UPDATE_USER", MySqlDbType.VarChar) { Value = user }
                };

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, permissionSql, permissionParams);

                result.ErrorNumber = 0;
                result.Error = "updated";
            }
            catch (Exception ex)
            {
                result.ErrorNumber = 1;
                result.Error = ex.Message;
            }

            return result;
        }


        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result = await DeleteUsser(BaseParameter);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> DeleteUsser(BaseParameter baseParameter)
        {
            var result = new BaseResult();
            foreach (var item in baseParameter.ListtsuserTranfer)
            {
                int userIdx = item.USER_IDX ?? 0;
                string user = item.UPDATE_USER?.Trim() ?? "";

                string permissionSql = @" UPDATE  tsuser  SET  `DESC_YN` = 'N', UPDATE_DTM = NOW(),UPDATE_USER = @UpdateUser WHERE  `USER_IDX` = @USER_IDX";

                var permissionParams = new[]
                    {
                    new MySqlParameter("@USER_IDX", MySqlDbType.Int32) { Value = userIdx },
                    new MySqlParameter("@UpdateUser", MySqlDbType.VarChar) { Value = user }
                };

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, permissionSql, permissionParams);

            }

            result.ErrorNumber = 0;
            result.Error = "updated";

            return result;
        }

        private async Task<BaseResult> RecoveryUsser(BaseParameter baseParameter)
        {
            var result = new BaseResult();
            foreach (var item in baseParameter.ListtsuserTranfer)
            {
                int userIdx = item.USER_IDX ?? 0;
                string user = item.UPDATE_USER?.Trim() ?? "";

                string permissionSql = @" UPDATE  tsuser  SET  `DESC_YN` = 'Y', UPDATE_DTM = NOW(),UPDATE_USER = @UpdateUser WHERE  `USER_IDX` = @USER_IDX";

                var permissionParams = new[]
                    {
                    new MySqlParameter("@USER_IDX", MySqlDbType.Int32) { Value = userIdx },
                    new MySqlParameter("@UpdateUser", MySqlDbType.VarChar) { Value = user }
                };

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, permissionSql, permissionParams);

            }

            result.ErrorNumber = 0;
            result.Error = "updated";

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

