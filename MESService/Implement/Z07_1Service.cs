namespace MESService.Implement
{
    public class Z07_1Service : BaseService<torderlist, ItorderlistRepository>
    , IZ07_1Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public Z07_1Service(ItorderlistRepository torderlistRepository

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
              
                string DGV_DATA1_1 = "SELECT '001' AS `SCRN_PATH`, 'New Menu'  AS `NAME`  UNION  " +
                    "SELECT tsmenu.SCRN_PATH, CONCAT(tsmenu.SCRN_PATH, '  ', tsmenu.MENU_NM_EN) AS `NAME`  FROM tsmenu   " +
                    "WHERE tsmenu.MENU_LVL = '2' AND tsmenu.DECYN ='Y'  ORDER BY `SCRN_PATH`  ";

                DataSet dsDGV_01_1 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1_1);

               
                result.ComboBox2 = new List<SuperResultTranfer>();
                if (dsDGV_01_1.Tables.Count > 0)
                {
                    DataTable dt = dsDGV_01_1.Tables[0];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        result.ComboBox2.Add(new SuperResultTranfer
                        {
                            SCRN_PATH = dt.Rows[i]["SCRN_PATH"].ToString(),
                            Name = dt.Rows[i]["NAME"].ToString()
                        });
                    }
                }
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
                if (BaseParameter != null && BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 0)
                {
                    string DEP_INDEX = BaseParameter.ListSearchString[0];

                    string DGV_DATA1_M = @"SELECT 
                ZT_DEVLPMNT_DB.`DELP_DATE`, ZT_DEVLPMNT_DB.`DELP_DEPT`, 
                (SELECT `A`.`NAME` FROM (
                SELECT 'New Menu' AS `SCRN_PATH`, 'New Menu' AS `NAME` UNION  
                SELECT tsmenu.`SCRN_PATH`, CONCAT(tsmenu.`SCRN_PATH`, '  ', tsmenu.MENU_NM_EN) AS `NAME` FROM tsmenu   
                WHERE tsmenu.`MENU_LVL` = '2' AND tsmenu.DECYN ='Y') `A`
                WHERE `A`.`SCRN_PATH` = ZT_DEVLPMNT_DB.MES_MENU) AS `MENU`,
                IFNULL(ZT_DEVLPMNT_DB.`DELP_NAME`, '') AS `DELP_NAME`,
                IFNULL(ZT_DEVLPMNT_DB.`DELP_DETIL`, '') AS `DELP_DETIL`,
                ZT_DEVLPMNT_DB.`FILE_DSYN`, IFNULL(ZT_DEVLPMNT_DB.`FILE_NM`, '') AS `FILE_NM`,
                IFNULL(ZT_DEVLPMNT_DB.`FILE_EX`, '') AS `FILE_EX`, IFNULL(ZT_DEVLPMNT_DB.`FILE_SIZE`, 0) AS `FILE_SIZE`,
                IFNULL(ZT_DEVLPMNT_DB.`DEP_PHOTO`, '') AS `DEP_PHOTO`, ZT_DEVLPMNT_DB.`CREATE_DTM`,
                (SELECT tsuser.USER_NM FROM tsuser WHERE tsuser.USER_ID = ZT_DEVLPMNT_DB.CREATE_USER) AS `USER`,
                ZT_DEVLPMNT_DB.`STATE`, IFNULL(ZT_DEVLPMNT_DB.`PROGRESS`, 'STAY') AS `PROGRESS`,
                IFNULL(ZT_DEVLPMNT_DB.`DONE_DATE`, NOW()) AS `DONE_DATE`,
                IFNULL(ZT_DEVLPMNT_DB.`MES_VER`, '-') AS `MES_VER`, ZT_DEVLPMNT_DB.`RESULT`,
                ZT_DEVLPMNT_DB.`UPDATE_DTM`
                FROM ZT_DEVLPMNT_DB
                WHERE ZT_DEVLPMNT_DB.DELP_IDX = '" + DEP_INDEX + "'";

                    DataSet dsDGV_01_M = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1_M);

                    if (dsDGV_01_M.Tables.Count > 0 && dsDGV_01_M.Tables[0].Rows.Count > 0)
                    {
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        DataTable dt = dsDGV_01_M.Tables[0];
                        result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
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
                await Task.Run(() => { });
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
                if (BaseParameter != null && BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 8)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    string DEP_INDEX = BaseParameter.ListSearchString[0];
                    string DELP_DEPT = BaseParameter.ListSearchString[1];
                    string MES_MENU = BaseParameter.ListSearchString[2];
                    string DELP_NAME = BaseParameter.ListSearchString[3];
                    string DELP_DETIL = BaseParameter.ListSearchString[4];
                    string FILE_DSYN = BaseParameter.ListSearchString[5];
                    string FILE_NM = BaseParameter.ListSearchString[6];
                    string FILE_EX = BaseParameter.ListSearchString[7];
                    string FILE_SIZE = BaseParameter.ListSearchString.Count > 8 ? BaseParameter.ListSearchString[8] : "0";
                    string DEP_PHOTO = BaseParameter.ListSearchString.Count > 9 ? BaseParameter.ListSearchString[9] : "";

                    // Xử lý dữ liệu DELP_DETIL
                    if (DELP_DETIL.Length > 1600)
                    {
                        DELP_DETIL = DELP_DETIL.Substring(0, 1600);
                    }

                    DELP_DETIL = DELP_DETIL.Replace("'", "");
                    DELP_DETIL = DELP_DETIL.Replace("?", "");
                    DELP_DETIL = DELP_DETIL.Replace("$", "");
                    DELP_DETIL = DELP_DETIL.Replace("#", "");
                    DELP_DETIL = DELP_DETIL.Replace("[", "");
                    DELP_DETIL = DELP_DETIL.Replace("]", "");
                    DELP_DETIL = DELP_DETIL.Replace("\\", "");
                    DELP_DETIL = DELP_DETIL.Replace("/", "");

                    string sql;

                    if (DEP_INDEX == "New")
                    {
                        // Insert new record
                        sql = @"INSERT INTO `ZT_DEVLPMNT_DB` 
                    (`DELP_DATE`, `DELP_DEPT`, `MES_MENU`, `DELP_NAME`, `DELP_DETIL`, 
                    `FILE_DSYN`, `FILE_NM`, `FILE_EX`, `FILE_SIZE`, `CREATE_DTM`, 
                    `CREATE_USER`, `STATE`, `PROGRESS`, `DONE_DATE`, `MES_VER`, 
                    `DELP_DELT`, `RESULT`, `UPDATE_DTM`, `UPDATE_USER`, `DEP_PHOTO`) 
                VALUES
                    (NOW(), '" + DELP_DEPT + "', '" + MES_MENU + "', '" + DELP_NAME + "', '" + DELP_DETIL + "', " +
                            "'" + FILE_DSYN + "', '" + FILE_NM + "', '" + FILE_EX + "', '" + FILE_SIZE + "', NOW(), " +
                            "'" + USER_IDX + "', 'N', '', NULL, NULL, " +
                            "NULL, 'N', NULL, NULL, '" + DEP_PHOTO + "')";
                    }
                    else
                    {
                        // Update existing record
                        sql = @"UPDATE `ZT_DEVLPMNT_DB` SET 
                    `DELP_DATE` = NOW(), 
                    `DELP_DEPT` = '" + DELP_DEPT + "',`MES_MENU` = '" + MES_MENU + "',`DELP_NAME` = '" + DELP_NAME + "',`DELP_DETIL` = '" + DELP_DETIL + "',";
        

                if (!string.IsNullOrEmpty(FILE_DSYN))
                        {
                            sql += @"`FILE_DSYN` = '" + FILE_DSYN + "',  `FILE_NM` = '" + FILE_NM + "',   `FILE_EX` = '" + FILE_EX + "',  `FILE_SIZE` = '" + FILE_SIZE + "',";
                }

                        if (!string.IsNullOrEmpty(DEP_PHOTO))
                        {
                            sql += @"`DEP_PHOTO` = '" + DEP_PHOTO + "',";
                        }

                        sql += @"`UPDATE_DTM` = NOW(), 
                    `UPDATE_USER` = '" + USER_IDX + "'  WHERE `DELP_IDX` = '" + DEP_INDEX + "'";
                    }

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                if (BaseParameter != null && BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 0)
                {
                    string DEP_INDEX = BaseParameter.ListSearchString[0];

                    string sql = "DELETE FROM ZT_DEVLPMNT_DB WHERE DELP_IDX = '" + DEP_INDEX + "'";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

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

