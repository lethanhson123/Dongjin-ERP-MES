
namespace MESService.Implement
{
    public class Admin4Service : BaseService<tsmenu, ItsmenuRepository>, Iadmin4Service
    {
        private readonly ItsmenuRepository _torderlistRepository;
        public Admin4Service(ItsmenuRepository torderlistRepository

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
                result.ListtsmenuTranfer = await GetListMenuGroup();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<List<tsmenuTranfer>?> GetListMenuGroup()
        {
            var resul = new BaseResult();

            var sql = "SELECT * FROM tsmenu WHERE menu_lvl = 1 ORDER BY MENU_NM_VIE, MENU_LVL,SCRN_PATH;";
            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            resul.ListtsmenuTranfer = new List<tsmenuTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                resul.ListtsmenuTranfer.AddRange(SQLHelper.ToList<tsmenuTranfer>(dt));
            }


            return resul.ListtsmenuTranfer;
        }

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result = await LoadMenu();
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> LoadMenu()
        {
            var result = new BaseResult();

            string sql = @"SELECT `MENU_IDX`, `MENU_NM_EN`, `MENU_LVL`, `MENU_NM_HAN`, `MENU_NM_VIE` AS `GroupCode`, `SCRN_PATH`, CREATE_DTM, CREATE_USER FROM tsmenu ORDER BY MENU_NM_VIE, MENU_LVL,SCRN_PATH ";

            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.ListtsmenuTranfer = new List<tsmenuTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.ListtsmenuTranfer.AddRange(SQLHelper.ToList<tsmenuTranfer>(dt));
            }

            result.Error = "success";

            return result;
        }

        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result = await AddNewMenu(BaseParameter);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<BaseResult> AddNewMenu(BaseParameter baseParameter)
        {
            var result = new BaseResult();

            if (baseParameter.tsmenuTranfer == null)
            {
                result.Message = ("empty data");
                result.ErrorNumber = -1;
                return result;             
            }

            var t = baseParameter.tsmenuTranfer;
            var userId = baseParameter.USER_ID;
            var createTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Lấy mã MENU_CD mới
            var menuCdObj = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString,
                "SELECT IFNULL(MAX(MENU_CD), 0) + 1 FROM tsmenu WHERE MENU_CD < 9999");
            int menuCd = Convert.ToInt32(menuCdObj);

            // Thêm tsmenu
            string insertMenu = @"INSERT INTO tsmenu 
        (MENU_CD, MENU_LVL, MENU_NM_EN, MENU_NM_HAN, MENU_NM_VIE, SCRN_PATH, CREATE_DTM, CREATE_USER) 
        VALUES (@MENU_CD, @MENU_LVL, @MENU_NM_EN, @MENU_NM_HAN, @MENU_NM_VIE, @SCRN_PATH, @CREATE_DTM, @CREATE_USER)";

            var param = new[]
            {
                new MySqlParameter("@MENU_CD", menuCd),
                new MySqlParameter("@MENU_LVL", t.MENU_LVL),
                new MySqlParameter("@MENU_NM_EN", t.MENU_NM_EN),
                new MySqlParameter("@MENU_NM_HAN", t.MENU_NM_HAN),
                new MySqlParameter("@MENU_NM_VIE", t.MENU_NM_VIE),
                new MySqlParameter("@SCRN_PATH", t.SCRN_PATH),
                new MySqlParameter("@CREATE_DTM", createTime),
                new MySqlParameter("@CREATE_USER", userId)
            };

            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertMenu, param);

            // Lấy MENU_IDX vừa insert
            var menuIdxObj = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, $"SELECT tsmenu.MENU_IDX FROM tsmenu WHERE tsmenu.CREATE_DTM = '{createTime}'");
            int menuIdx = Convert.ToInt32(menuIdxObj);

            // Lấy danh sách AUTH_IDX
            var dsAuth = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, "SELECT AUTH_IDX FROM TSAUTH");
            var authList = SQLHelper.ToList<tsauthTranfer>(dsAuth.Tables[0]);

            // Chuẩn bị insert tsmnau
            var values = authList.Select(a =>
                $"('{menuIdx}', '{a.AUTH_IDX}', 'False', 'True', 'True', 'True', 'True', 'True', 'True', 'True', 'True', 'False', 'False', 'False', NOW(), '{userId}')");

            string insertAuth = @"INSERT INTO tsmnau 
        (MENU_IDX, AUTH_IDX, MENU_AUTH_YN, IQ_AUTH_YN, RGST_AUTH_YN, MDFY_AUTH_YN, DEL_AUTH_YN, 
         CAN_AUTH_YN, EXCL_AUTH_YN, DNLD_AUTH_YN, PRNT_AUTH_YN, ETC1_AUTH_YN, ETC2_AUTH_YN, ETC3_AUTH_YN, CREATE_DTM, CREATE_USER) 
         VALUES " + string.Join(", ", values);

            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertAuth);

            result.ErrorNumber = 0;
            result.Message = "Added new menu";

            return result;
        }


        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
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
    }
}

