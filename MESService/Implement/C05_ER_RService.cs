namespace MESService.Implement
{
    public class C05_ER_RService : BaseService<torderlist, ItorderlistRepository>
    , IC05_ER_RService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C05_ER_RService(ItorderlistRepository torderlistRepository
            , IWebHostEnvironment webHostEnvironment

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> C11_ERROR_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    var MCbox = BaseParameter.SearchString;
                    var ST_TM = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string sql = @"INSERT INTO `TSNON_OPER` (`TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_STIME`, `CREATE_DTM`, `CREATE_USER`, `TSNON_OPER_CODE`,`REMARK`) VALUES ('" + MCbox + "', '" + USER_ID + "', DATE_FORMAT(NOW(), '%Y-%m-%d'), NOW(), '" + ST_TM + "', '" + USER_ID + "', 'S', 'New MES C05 ER R')";
                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    sql = @"SELECT  TSNON_OPER.TSNON_OPER_IDX FROM TSNON_OPER  WHERE TSNON_OPER.CREATE_DTM ='" + ST_TM + "'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                    sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES ('" + MCbox + "', 'Chuẩn bị thiết bị(설비준비)(S)', 'Y','S')  ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC`= 'Chuẩn bị thiết bị(설비준비)(S)', `tsnon_oper_mitor_RUNYN` = 'Y',`StopCode`='S'";
                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    sql = @"ALTER TABLE     `tsnon_oper_mitor`     AUTO_INCREMENT= 1";
                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> C11_ERROR_FormClosed(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var LAST_ID = BaseParameter.ListSearchString[0];
                        var MCbox = BaseParameter.ListSearchString[1];

                        string sql = @"UPDATE `TSNON_OPER` SET `TSNON_OPER_ETIME`= NOW(), `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + USER_ID + "', `TSNON_OPER_TIME` = TIME_TO_SEC(TIMEDIFF(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`)) WHERE  `TSNON_OPER_IDX`= '" + LAST_ID + "'  and `TSNON_OPER_ETIME` is null;";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES ('" + MCbox + "', '-----', 'N','-')  ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC`= '-----', `tsnon_oper_mitor_RUNYN` = 'N',`StopCode`='-'";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `tsnon_oper_mitor`     AUTO_INCREMENT= 1";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    }
                }
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
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    var IsCheck = true;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var A2 = BaseParameter.ListSearchString[0];
                        var LBA2SEQ = BaseParameter.ListSearchString[1];
                        var Label1 = BaseParameter.ListSearchString[2];
                        string sqlResult = "";
                        string sql = @"SELECT  ttoolmaster2.TOOLMASTER_IDX, ttoolmaster2.TOOL_IDX, ttoolmaster2.SEQ, ttoolmaster2.TOT_WK_CNT, ttoolmaster2.WK_CNT, TTOOLMASTER.APPLICATOR FROM     ttoolmaster2, TTOOLMASTER  WHERE  ttoolmaster2.TOOL_IDX = TTOOLMASTER.TOOL_IDX AND (TTOOLMASTER.APPLICATOR = '" + A2 + "') AND (ttoolmaster2.SEQ = '" + LBA2SEQ + "')";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        result.Search1 = new List<SuperResultTranfer>();
                        result.DataGridView = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.Search.Count <= 0)
                        {
                            sql = @"SELECT  TOOL_IDX, APPLICATOR FROM   TTOOLMASTER  WHERE  (APPLICATOR = '" + A2 + "')";
                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.Search1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.Search1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            if (result.Search1.Count <= 0)
                            {
                                IsCheck = false;
                            }
                            else
                            {
                                var TIDX = result.Search1[0].TOOLMASTER_IDX;
                                sql = @"INSERT INTO ttoolmaster2 (`TOOL_IDX`, `SEQ`, `TOT_WK_CNT`, `WK_CNT`, `CREATE_DTM`, `CREATE_USER`) VALUES  ('" + TIDX + "', '" + LBA2SEQ + "', 0, 0, NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"ALTER TABLE     `ttoolmaster2`     AUTO_INCREMENT= 1";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"SELECT  ttoolmaster2.TOOLMASTER_IDX, ttoolmaster2.TOOL_IDX, ttoolmaster2.SEQ, ttoolmaster2.TOT_WK_CNT, ttoolmaster2.WK_CNT, TTOOLMASTER.APPLICATOR  FROM     ttoolmaster2, TTOOLMASTER  WHERE  ttoolmaster2.TOOL_IDX = TTOOLMASTER.TOOL_IDX AND (TTOOLMASTER.APPLICATOR = '" + A2 + "') AND (ttoolmaster2.SEQ = '" + USER_ID + "')";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }

                                sql = @"SELECT  ttoolmaster2.TOOLMASTER_IDX, ttoolmaster2.TOOL_IDX, ttoolmaster2.SEQ, ttoolmaster2.TOT_WK_CNT, ttoolmaster2.WK_CNT, TTOOLMASTER.APPLICATOR FROM     ttoolmaster2, TTOOLMASTER  WHERE  ttoolmaster2.TOOL_IDX = TTOOLMASTER.TOOL_IDX AND (TTOOLMASTER.APPLICATOR = '" + A2 + "') AND (ttoolmaster2.SEQ = '" + LBA2SEQ + "')";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.Search = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                            }
                        }
                        else
                        {
                        }
                        if (IsCheck == true)
                        {
                            var TOOLMASTER_IDX = result.Search[0].TOOLMASTER_IDX;
                            sql = @"UPDATE torder_bom_LP  SET `T2_TOOL_IDX` = '" + TOOLMASTER_IDX + "' WHERE  `ORDER_IDX` ='" + Label1 + "'";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button2_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    var Label1 = BaseParameter.SearchString;
                    string sql = @"UPDATE `torder_bom_LP` SET `ERROR_CHK`='Y', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `ORDER_IDX`='" + Label1 + "'";
                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}

