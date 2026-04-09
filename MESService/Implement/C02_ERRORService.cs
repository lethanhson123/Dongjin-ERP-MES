using System.Collections.Generic;

namespace MESService.Implement
{
    public class C02_ERRORService : BaseService<torderlist, ItorderlistRepository>
    , IC02_ERRORService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C02_ERRORService(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> C02_ERROR_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        bool IsCheck = true;
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        result.DataGridView3 = new List<SuperResultTranfer>();
                        string sqlResult = "";

                        var Label1 = BaseParameter.ListSearchString[0];
                        var MCbox = BaseParameter.ListSearchString[1];

                        string sql = @"SELECT  ORDER_IDX, LEAD_NO, PROJECT, TOT_QTY, BUNDLE_SIZE, WIRE, UCASE(TERM1) AS TERM1, UCASE(SEAL1) AS SEAL1, UCASE(TERM2) AS TERM2, UCASE(SEAL2) AS SEAL2, DT FROM     TORDERLIST WHERE  DSCN_YN = 'Y' AND ORDER_IDX  ='" + Label1 + "'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        var PP = result.DataGridView1.Count - 1;
                        if (PP != 0)
                        {
                            IsCheck = false;
                        }
                        if (IsCheck == true)
                        {
                            sql = @"SELECT  
                            `ORDER_IDX`, 
                            `TERM1`,
                            (SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.`TERM1`)) AS `T1_IDX`,
                            `SEAL1`,
                            (SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.`SEAL1`)) AS `S1_IDX`, 
                            `TERM2`,
                            (SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.TERM2)) AS `T2_IDX`, 
                            `SEAL2`,
                            (SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = TORDERLIST.SEAL2)) AS `S2_IDX`, 
                            `WIRE`, 
                            (SELECT `PART_NO` FROM tspart WHERE `PART_IDX`=(SELECT `W_PN_IDX`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`))) AS `WIRE_NM`,
                            (SELECT `W_PN_IDX`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`))  AS `WIRE_IDX`
                            FROM  TORDERLIST WHERE `ORDER_IDX` = '" + Label1 + "'";

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView2 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            var ST_TM = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            sql = @"INSERT INTO `TSNON_OPER` (`TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_STIME`, `CREATE_DTM`, `CREATE_USER`, `TSNON_OPER_CODE`, `REMARK`) VALUES ('" + MCbox + "', '" + C_USER + "', DATE_FORMAT(NOW(), '%Y-%m-%d'), NOW(), '" + ST_TM + "', '" + C_USER + "', 'S','New MES C02 Error')";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"SELECT  TSNON_OPER.TSNON_OPER_IDX FROM TSNON_OPER  WHERE TSNON_OPER.CREATE_DTM ='" + ST_TM + "'";

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView3 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES ('" + MCbox + "', 'Chuẩn bị thiết bị(설비준비)(S)', 'Y', 'S')  ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC`= 'Chuẩn bị thiết bị(설비준비)(S)', `tsnon_oper_mitor_RUNYN` = 'Y', `StopCode` = 'S'";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"ALTER TABLE     `tsnon_oper_mitor`     AUTO_INCREMENT= 1";
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
        public virtual async Task<BaseResult> SW_TIME(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        bool IsCheck = true;

                        var USER_MC = BaseParameter.ListSearchString[0];
                        var USER_ORIDX = BaseParameter.ListSearchString[1];
                        var Text = BaseParameter.ListSearchString[2];
                        var ST_TM = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        string sql = @"INSERT INTO  `TORDERLIST_WTIME`  (`USER_ID`, `USER_MC`, `ORDER_IDX`, `S_TIME`, `CREATE_DTM`, `CREATE_USER`, `MENU_TEXT`) VALUES               ('" + USER_ID + "', '" + USER_MC + "', '" + USER_ORIDX + "', NOW(), '" + ST_TM + "', '" + USER_ID + "', '" + Text + "')";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT  TORDERLIST_WTIME.TOWT_INDX    FROM   TORDERLIST_WTIME  WHERE TORDERLIST_WTIME.CREATE_DTM ='" + ST_TM + "'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> EW_TIME(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        bool IsCheck = true;
                        var WORING_IDX = BaseParameter.ListSearchString[0];

                        string sql = @"UPDATE  `TORDERLIST_WTIME`   SET `E_TIME`= NOW()    WHERE  `TOWT_INDX` = '" + WORING_IDX + "'";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> C02_ERROR_FormClosed(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        bool IsCheck = true;
                        var LAST_ID = BaseParameter.ListSearchString[0];
                        var MCbox = BaseParameter.ListSearchString[1];

                        string sql = @"UPDATE `TSNON_OPER` SET `TSNON_OPER_ETIME`= NOW(), `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + USER_ID + "', `TSNON_OPER_TIME` = TIME_TO_SEC(TIMEDIFF(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`)) WHERE  `TSNON_OPER_IDX`= '" + LAST_ID + "' and `TSNON_OPER_ETIME` is null;";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`, `StopCode`) VALUES ('" + MCbox + "', '-----', 'N','-')  ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC`= '-----', `tsnon_oper_mitor_RUNYN` = 'N', `StopCode`='-'";
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
        public virtual async Task<BaseResult> Button2_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        bool IsCheck = true;
                        var Label1 = BaseParameter.ListSearchString[0];
                        var MCbox = BaseParameter.ListSearchString[1];

                        string sql = @"UPDATE `torder_bom` SET `ERROR_CHK`='Y', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `ORDER_IDX`=" + Label1;
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        //LeThanhSon 20260402 Begin

                        //sql = "SELECT * FROM torderlist where ORDER_IDX=" + Label1;

                        //DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        //var Listtorderlist01 = new List<torderlist>();
                        //for (int i = 0; i < ds.Tables.Count; i++)
                        //{
                        //    DataTable dt = ds.Tables[i];
                        //    Listtorderlist01.AddRange(SQLHelper.ToList<torderlist>(dt));
                        //}
                        //if (Listtorderlist01.Count > 0)
                        //{
                        //    var torderlist = Listtorderlist01[0];
                        //    var DT = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd 00:00:00");
                        //    var Listtorderlist02 = new List<torderlist>();
                        //    //sql = "SELECT * FROM TORDERLIST WHERE LEAD_NO='" + torderlist.LEAD_NO + "' AND MC='" + torderlist.MC + "' AND DSCN_YN='Y' AND NOT('CONDITION' = 'Close') AND DT >= DATE_ADD(NOW(), INTERVAL - 15 DAY)";
                        //    if (torderlist.MC == MCbox)
                        //    {
                        //        sql = "SELECT * FROM TORDERLIST WHERE LEAD_NO='" + torderlist.LEAD_NO + "' AND MC='" + torderlist.MC + "' AND DSCN_YN='Y' AND NOT('CONDITION' = 'Close') AND DT >= '" + DT + "'";
                        //        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);                                
                        //        for (int i = 0; i < ds.Tables.Count; i++)
                        //        {
                        //            DataTable dt = ds.Tables[i];
                        //            Listtorderlist02.AddRange(SQLHelper.ToList<torderlist>(dt));
                        //        }
                        //    }
                        //    if (torderlist.MC2 == MCbox)
                        //    {
                        //        sql = "SELECT * FROM TORDERLIST WHERE LEAD_NO='" + torderlist.LEAD_NO + "' AND MC2='" + torderlist.MC2 + "' AND DSCN_YN='Y' AND NOT('CONDITION' = 'Close') AND DT >= '" + DT + "'";
                        //        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        //        for (int i = 0; i < ds.Tables.Count; i++)
                        //        {
                        //            DataTable dt = ds.Tables[i];
                        //            Listtorderlist02.AddRange(SQLHelper.ToList<torderlist>(dt));
                        //        }
                        //    }
                        //    if (Listtorderlist02.Count > 0)
                        //    {
                        //        var ListtorderlistORDER_IDX = Listtorderlist02.Select(s => s.ORDER_IDX).Distinct().ToList();
                        //        var Parameter = string.Join(",", ListtorderlistORDER_IDX);
                        //        sql = @"UPDATE `torder_bom` SET `ERROR_CHK`='Y', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `ORDER_IDX` in (" + Parameter + ")";
                        //        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        //    }
                        //}

                        //LeThanhSon 20260402 End
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
                        var chuoiNhap = BaseParameter.ListSearchString[0];
                        string sqlResult = "";
                        string sql = @"SELECT `BARCD_ID`, `DSCN_YN`  FROM  TMBRCD   WHERE   `BARCD_ID` = '" + chuoiNhap + "'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> Button1_ClickSub001(BaseParameter BaseParameter)
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
                        result.DataGridView4 = new List<SuperResultTranfer>();
                        result.DataGridView5 = new List<SuperResultTranfer>();
                        result.DataGridView6 = new List<SuperResultTranfer>();


                        var A1 = BaseParameter.ListSearchString[0];
                        var LBA1SEQ = BaseParameter.ListSearchString[1];
                        var Label1 = BaseParameter.ListSearchString[2];


                        string sqlResult = "";
                        string sql = @"SELECT  ttoolmaster2.TOOLMASTER_IDX, ttoolmaster2.TOOL_IDX, ttoolmaster2.SEQ, ttoolmaster2.TOT_WK_CNT, ttoolmaster2.WK_CNT, TTOOLMASTER.APPLICATOR FROM     ttoolmaster2, TTOOLMASTER  WHERE  ttoolmaster2.TOOL_IDX = TTOOLMASTER.TOOL_IDX AND (TTOOLMASTER.APPLICATOR = '" + A1 + "') AND (ttoolmaster2.SEQ = '" + LBA1SEQ + "')";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView4 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.DataGridView4.Count <= 0)
                        {
                            sql = @"SELECT  TOOL_IDX, APPLICATOR FROM   TTOOLMASTER  WHERE  (APPLICATOR = '" + A1 + "')";
                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView5 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView5.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            if (result.DataGridView5.Count <= 0)
                            {
                                IsCheck = false;
                            }
                            else
                            {
                                var TIDX = result.DataGridView5[0].TOOLMASTER_IDX;
                                sql = @"INSERT INTO ttoolmaster2 (`TOOL_IDX`, `SEQ`, `TOT_WK_CNT`, `WK_CNT`, `CREATE_DTM`, `CREATE_USER`) VALUES  ('" + TIDX + "', '" + LBA1SEQ + "', 0, 0, NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"ALTER TABLE     `ttoolmaster2`     AUTO_INCREMENT= 1";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"SELECT  ttoolmaster2.TOOLMASTER_IDX, ttoolmaster2.TOOL_IDX, ttoolmaster2.SEQ, ttoolmaster2.TOT_WK_CNT, ttoolmaster2.WK_CNT, TTOOLMASTER.APPLICATOR                                    FROM     ttoolmaster2, TTOOLMASTER  WHERE  ttoolmaster2.TOOL_IDX = TTOOLMASTER.TOOL_IDX AND (TTOOLMASTER.APPLICATOR = '" + A1 + "') AND (ttoolmaster2.SEQ = '" + LBA1SEQ + "')";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView6 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView6.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }

                                sql = @"SELECT  ttoolmaster2.TOOLMASTER_IDX, ttoolmaster2.TOOL_IDX, ttoolmaster2.SEQ, ttoolmaster2.TOT_WK_CNT, ttoolmaster2.WK_CNT, TTOOLMASTER.APPLICATOR FROM     ttoolmaster2, TTOOLMASTER  WHERE  ttoolmaster2.TOOL_IDX = TTOOLMASTER.TOOL_IDX AND (TTOOLMASTER.APPLICATOR = '" + A1 + "') AND (ttoolmaster2.SEQ = '" + LBA1SEQ + "')";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView4 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                            }
                        }
                        else
                        {
                        }
                        if (IsCheck == true)
                        {
                            sql = @"UPDATE torder_bom  SET T1_TOOL_IDX = '" + result.DataGridView4[0].TOOLMASTER_IDX + "' WHERE  ORDER_IDX ='" + Label1 + "'";
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
        public virtual async Task<BaseResult> Button1_ClickSub002(BaseParameter BaseParameter)
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
                        result.DataGridView7 = new List<SuperResultTranfer>();
                        result.DataGridView8 = new List<SuperResultTranfer>();
                        result.DataGridView9 = new List<SuperResultTranfer>();


                        var A2 = BaseParameter.ListSearchString[0];
                        var LBA2SEQ = BaseParameter.ListSearchString[1];
                        var Label1 = BaseParameter.ListSearchString[2];


                        string sqlResult = "";
                        string sql = @"SELECT  ttoolmaster2.TOOLMASTER_IDX, ttoolmaster2.TOOL_IDX, ttoolmaster2.SEQ, ttoolmaster2.TOT_WK_CNT, ttoolmaster2.WK_CNT, TTOOLMASTER.APPLICATOR FROM     ttoolmaster2, TTOOLMASTER  WHERE  ttoolmaster2.TOOL_IDX = TTOOLMASTER.TOOL_IDX AND (TTOOLMASTER.APPLICATOR = '" + A2 + "') AND (ttoolmaster2.SEQ = '" + LBA2SEQ + "')";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView7 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView7.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.DataGridView7.Count <= 0)
                        {
                            sql = @"SELECT  TOOL_IDX, APPLICATOR FROM   TTOOLMASTER  WHERE  (APPLICATOR = '" + A2 + "')";
                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView8 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView8.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            if (result.DataGridView8.Count <= 0)
                            {
                                IsCheck = false;
                            }
                            else
                            {
                                var TIDX = result.DataGridView8[0].TOOLMASTER_IDX;
                                sql = @"INSERT INTO ttoolmaster2 (`TOOL_IDX`, `SEQ`, `TOT_WK_CNT`, `WK_CNT`, `CREATE_DTM`, `CREATE_USER`)  VALUES  ('" + TIDX + "', '" + LBA2SEQ + "', 0, 0, NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"ALTER TABLE     `ttoolmaster2`     AUTO_INCREMENT= 1";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"SELECT  ttoolmaster2.TOOLMASTER_IDX, ttoolmaster2.TOOL_IDX, ttoolmaster2.SEQ, ttoolmaster2.TOT_WK_CNT, ttoolmaster2.WK_CNT, TTOOLMASTER.APPLICATOR                                    FROM     ttoolmaster2, TTOOLMASTER  WHERE  ttoolmaster2.TOOL_IDX = TTOOLMASTER.TOOL_IDX AND (TTOOLMASTER.APPLICATOR = '" + A2 + "') AND (ttoolmaster2.SEQ = '" + LBA2SEQ + "')";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView9 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView9.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }

                                sql = @"SELECT  ttoolmaster2.TOOLMASTER_IDX, ttoolmaster2.TOOL_IDX, ttoolmaster2.SEQ, ttoolmaster2.TOT_WK_CNT, ttoolmaster2.WK_CNT, TTOOLMASTER.APPLICATOR FROM     ttoolmaster2, TTOOLMASTER  WHERE  ttoolmaster2.TOOL_IDX = TTOOLMASTER.TOOL_IDX AND (TTOOLMASTER.APPLICATOR = '" + A2 + "') AND (ttoolmaster2.SEQ = '" + LBA2SEQ + "')"; ;
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView7 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView7.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                            }
                        }
                        else
                        {
                        }
                        if (IsCheck == true)
                        {
                            sql = @"UPDATE torder_bom  SET T2_TOOL_IDX = '" + result.DataGridView7[0].TOOLMASTER_IDX + "' WHERE  ORDER_IDX ='" + Label1 + "'";
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
    }
}

