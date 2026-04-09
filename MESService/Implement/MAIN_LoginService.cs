using MESData.Model;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System.Xml.Linq;
using ZXing;

namespace MESService.Implement
{
    public class MAIN_LoginService : BaseService<tsuser, ItsuserRepository>
    , IMAIN_LoginService
    {
        private readonly ItsuserRepository _tsuserRepository;
        private readonly Izz_mes_verRepository _zz_mes_verRepository;
        private readonly Ituser_log_chkRepository _tuser_log_chkRepository;
        private readonly Ituser_log_chk_listRepository _tuser_log_chk_listRepository;

        public MAIN_LoginService(ItsuserRepository tsuserRepository

            , Izz_mes_verRepository zz_mes_verRepository
            , Ituser_log_chkRepository tuser_log_chkRepository
            , Ituser_log_chk_listRepository tuser_log_chk_listRepository

            ) : base(tsuserRepository)
        {
            _tsuserRepository = tsuserRepository;
            _zz_mes_verRepository = zz_mes_verRepository;
            _tuser_log_chkRepository = tuser_log_chkRepository;
            _tuser_log_chk_listRepository = tuser_log_chk_listRepository;
        }


        public override void Initialization(tsuser model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> OK_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();

            try
            {
                string sql = @"SELECT `VER_MAJOR`, `VER_MINJOR`, `VER_BUILD`, `VER_REVISION` 
                       FROM `DongJin`.`ZZ_MES_VER`";

                result.Listzz_mes_ver = await _zz_mes_verRepository
                    .GetByMySQLToListAsync(GlobalHelper.MariaDBConectionString, sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                string sql = @"SELECT USER_IDX, USER_ID, USER_NM, PW, Dept, Note 
                       FROM tsuser 
                       WHERE USER_ID = @UserId AND DESC_YN = 'Y'";

                var param = new[]
                {
            new MySqlParameter("@UserId", BaseParameter.UserID?.Trim())
        };

                result.Listtsuser = await _tsuserRepository
                    .GetByMySQLToListAsync(GlobalHelper.MariaDBConectionString, sql, param);

                if (result.Listtsuser.Count > 0)
                {
                    var user = result.Listtsuser[0];

                    bool isHashed = !string.IsNullOrEmpty(user.PW) && user.PW.StartsWith("$2");

                    // PASSWORD CHECK
                    bool isValid = false;

                    if (!isHashed)
                    {
                        if (string.Equals(user.PW?.Trim(), BaseParameter.Password?.Trim(), StringComparison.Ordinal))
                            isValid = true;
                    }
                    else
                    {
                        if (SecurityHelper.VerifyPassword(BaseParameter.Password?.Trim(), user.PW))
                            isValid = true;
                    }

                    if (!isValid)
                    {
                        result.Message = "Wrong Password or User name";
                        return result;
                    }

                    result.tsuser = user;

                    // FORCE CHANGE PASSWORD
                    if (!isHashed)
                    {
                        result.RequireChangePassword = true;
                        result.Message = "FirstLogin";
                        return result;
                    }

                    // CHECK EXPIRE 90 DAYS
                    string checkSql = @"SELECT MAX(ChangePasswordTime) AS LastChange
                                FROM TSUSER_ChangePassHisstory
                                WHERE UserAccount = @UserId";

                    var dt = await MySQLHelperV2.FillDataTableAsync(
                        GlobalHelper.MariaDBConectionString,
                        checkSql,
                        new[] { new MySqlParameter("@UserId", BaseParameter.UserID?.Trim()) });

                    DateTime? lastChange = null;

                    if (dt.Rows.Count > 0 && dt.Rows[0]["LastChange"] != DBNull.Value)
                    {
                        lastChange = Convert.ToDateTime(dt.Rows[0]["LastChange"]);
                    }

                    if (lastChange == null)
                    {
                        result.RequireChangePassword = true;
                        result.Message = "FirstLogin";
                        return result;
                    }

                    if ((DateTime.Now - lastChange.Value).TotalDays > 90)
                    {
                        result.RequireChangePassword = true;
                        result.Message = "PasswordExpired";
                        return result;
                    }

    
                    sql = @"INSERT INTO TUSER_LOG_CHK_LIST 
                    (TS_USERID, TS_USER_IP, CREATE_DTM, CREATE_USER) 
                    VALUES (@UserId, @IP, NOW(), @UserId)";

                    await MySQLHelper.ExecuteNonQueryAsync(
                        GlobalHelper.MariaDBConectionString,
                        sql,
                        new[]
                        {
                    new MySqlParameter("@UserId", BaseParameter.UserID),
                    new MySqlParameter("@IP", BaseParameter.IPAddress ?? "Unknown")
                        });

                    sql = @"SELECT TS_USERID, TS_USER_IP, TS_USER_CNN 
                    FROM TUSER_LOG_CHK 
                    WHERE TS_USERID = @UserId AND TS_USER_CNN = 'Y'";

                    result.Listtuser_log_chk = await _tuser_log_chkRepository
                        .GetByMySQLToListAsync(GlobalHelper.MariaDBConectionString, sql,
                        new[] { new MySqlParameter("@UserId", BaseParameter.UserID) });

                    if (result.Listtuser_log_chk.Count <= 0)
                    {
                        sql = @"INSERT INTO TUSER_LOG_CHK 
                        (TS_USERID, TS_USER_IP, TS_USER_CNN, CREATE_DTM, CREATE_USER) 
                        VALUES (@UserId, @IP, 'Y', NOW(), @UserId)";

                        await MySQLHelper.ExecuteNonQueryAsync(
                            GlobalHelper.MariaDBConectionString,
                            sql,
                            new[]
                            {
                        new MySqlParameter("@UserId", BaseParameter.UserID),
                        new MySqlParameter("@IP", BaseParameter.IPAddress ?? "Unknown")
                            });
                    }

                    if (BaseParameter.UserID.ToUpper() != GlobalHelper.YSJ4947)
                    {
                        sql = @"SELECT TUSER_LOG_CHK_LIST.TS_USERID, TUSER_LOG_CHK_LIST.CREATE_DTM,
                               TIMESTAMPDIFF(DAY, TUSER_LOG_CHK_LIST.CREATE_DTM, NOW()) AS DAY
                        FROM TUSER_LOG_CHK_LIST
                        WHERE TIMESTAMPDIFF(DAY, CREATE_DTM, NOW()) < 30
                        AND TS_USERID = @UserId";

                        var ds = await MySQLHelper.FillDataSetBySQLAsync(
                            GlobalHelper.MariaDBConectionString,
                            sql.Replace("@UserId", $"'{BaseParameter.UserID}'"));

                        result.Listtuser_log_chk_listTranfer = new List<tuser_log_chk_listTranfer>();

                        foreach (DataTable table in ds.Tables)
                        {
                            result.Listtuser_log_chk_listTranfer.AddRange(
                                SQLHelper.ToList<tuser_log_chk_listTranfer>(table));
                        }

                        if (result.Listtuser_log_chk_listTranfer.Count <= 0)
                        {
                            sql = @"SELECT USER_IDX, USER_ID FROM tsuser WHERE USER_ID = @UserId";

                            result.Listtsuser = await _tsuserRepository.GetByMySQLToListAsync(
                                GlobalHelper.MariaDBConectionString,
                                sql,
                                new[] { new MySqlParameter("@UserId", BaseParameter.UserID) });
                        }

                        sql = @"UPDATE TSNON_OPER 
                        SET TSNON_OPER_ETIME = NOW(),
                            UPDATE_DTM = NOW(),
                            UPDATE_USER = 'SYSTEM',
                            TSNON_OPER_TIME = TIME_TO_SEC(TIMEDIFF(TSNON_OPER_ETIME, TSNON_OPER_STIME))
                        WHERE TSNON_OPER_IDX = @IDX AND TSNON_OPER_ETIME IS NULL";

                        await MySQLHelper.ExecuteNonQueryAsync(
                            GlobalHelper.MariaDBConectionString,
                            sql,
                            new[] { new MySqlParameter("@IDX", BaseParameter.NON_OPER_IDX) });
                    }

                    var STOP_MC = BaseParameter.MC_NAME?.Trim().ToUpper();
                    var C_USER = user.USER_ID;

                    sql = @"UPDATE TSNON_OPER 
                    SET TSNON_OPER_ETIME = NOW(),
                        UPDATE_DTM = NOW(),
                        UPDATE_USER = @User,
                        TSNON_OPER_TIME = TIME_TO_SEC(TIMEDIFF(TSNON_OPER_ETIME, TSNON_OPER_STIME))
                    WHERE TSNON_OPER_CODE = 'N' 
                    AND TSNON_OPER_MCNM = @MC 
                    AND TSNON_OPER_ETIME IS NULL";

                    await MySQLHelper.ExecuteNonQueryAsync(
                        GlobalHelper.MariaDBConectionString,
                        sql,
                        new[]
                        {
                    new MySqlParameter("@User", C_USER),
                    new MySqlParameter("@MC", STOP_MC)
                        });

                    sql = @"INSERT INTO tsnon_oper_mitor 
                    (tsnon_oper_mitor_MCNM, tsnon_oper_mitor_NOIC, tsnon_oper_mitor_RUNYN, StopCode)
                    VALUES (@MC, '-----', 'N','-')
                    ON DUPLICATE KEY UPDATE 
                        tsnon_oper_mitor_NOIC='-----',
                        tsnon_oper_mitor_RUNYN='N',
                        StopCode='-'";

                    await MySQLHelper.ExecuteNonQueryAsync(
                        GlobalHelper.MariaDBConectionString,
                        sql,
                        new[] { new MySqlParameter("@MC", STOP_MC) });

                    await MySQLHelper.ExecuteNonQueryAsync(
                        GlobalHelper.MariaDBConectionString,
                        "ALTER TABLE tsnon_oper_mitor AUTO_INCREMENT=1");

                    result.Message = "Success";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return result;
        }
        public async Task<BaseResult> ChangePassword_Click(ChangePasswordModel model)
        {
            BaseResult result = new BaseResult();

            try
            {
                string sql = @"SELECT USER_IDX, USER_ID, USER_NM, PW 
                       FROM tsuser 
                       WHERE USER_ID = @UserId AND DESC_YN = 'Y'";

                var users = await _tsuserRepository.GetByMySQLToListAsync(
                    GlobalHelper.MariaDBConectionString,
                    sql,
                    new[] { new MySqlParameter("@UserId", model.Username?.Trim()) });

                if (users.Count == 0)
                {
                    result.Message = "User not found";
                    return result;
                }

                var user = users[0];

                bool isHashed = !string.IsNullOrEmpty(user.PW) && user.PW.StartsWith("$2");

                bool isValid = false;

                if (!isHashed)
                {
                    if (string.Equals(user.PW?.Trim(), model.OldPassword?.Trim(), StringComparison.Ordinal))
                        isValid = true;
                }
                else
                {
                    if (SecurityHelper.VerifyPassword(model.OldPassword?.Trim(), user.PW))
                        isValid = true;
                }

                if (!isValid)
                {
                    result.Message = "Wrong Old Password";
                    return result;
                }

                // VALIDATE PASSWORD
                var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$");

                if (!regex.IsMatch(model.NewPassword ?? ""))
                {
                    result.Message = "Password must include upper, lower, number, special char";
                    return result;
                }

                string newHashedPassword = SecurityHelper.HashPassword(model.NewPassword.Trim());

                string updateSql = @"UPDATE tsuser 
                             SET PW = @Password 
                             WHERE USER_ID = @UserId AND DESC_YN = 'Y'";

                await MySQLHelper.ExecuteNonQueryAsync(
                    GlobalHelper.MariaDBConectionString,
                    updateSql,
                    new[]
                    {
                new MySqlParameter("@Password", newHashedPassword),
                new MySqlParameter("@UserId", model.Username?.Trim())
                    });

                // INSERT HISTORY
                string insertHistory = @"INSERT INTO TSUSER_ChangePassHisstory 
                                (UserAccount, ChangePasswordTime, IPConnected)
                                VALUES (@UserId, NOW(), @IP)";

                await MySQLHelper.ExecuteNonQueryAsync(
                    GlobalHelper.MariaDBConectionString,
                    insertHistory,
                    new[]
                    {
                new MySqlParameter("@UserId", model.Username?.Trim()),
                new MySqlParameter("@IP", model.IPAddress ?? "Unknown")
                    });

                result.Message = "Change password Successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                result.Message = "System Error";
            }

            return result;
        }
    }

}