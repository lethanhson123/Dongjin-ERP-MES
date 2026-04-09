using OfficeOpenXml.Style;
using System.Security.AccessControl;
using ZXing;

namespace MESService.Implement
{
    public class C02_START_V2Service : BaseService<torderlist, ItorderlistRepository>
    , IC02_START_V2Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C02_START_V2Service(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> C02_start_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var SettingsMC_NM = BaseParameter.ListSearchString[0];
                        var Label8 = BaseParameter.ListSearchString[1];
                        string sql = @"INSERT INTO `torderlist_work` (`torderlist_work_MC`, `torderlist_work_ORDERIDX`, `torderlist_work_USER`) VALUES ('" + SettingsMC_NM + "', '" + Label8 + "', '" + USER_ID + "')  ON DUPLICATE KEY UPDATE  `torderlist_work_ORDERIDX` = '" + Label8 + "', `torderlist_work_USER` = '" + USER_ID + "'";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `torderlist_work`     AUTO_INCREMENT= 1";
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
        public virtual async Task<BaseResult> ORDER_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    bool IsCheck = true;
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        result.Search = new List<SuperResultTranfer>();
                        result.Search1 = new List<SuperResultTranfer>();

                        var Label8 = BaseParameter.ListSearchString[0];
                        string sqlResult = "";
                        string sql = @"SELECT TORDERLIST.`ORDER_IDX`, TORDERLIST.`TORDER_FG`, TORDERLIST.`OR_NO`, TORDERLIST.`WORK_WEEK`, TORDERLIST.`LEAD_NO`, TORDERLIST.`PROJECT`, TORDERLIST.`TOT_QTY`, TORDERLIST.`ADJ_AF_QTY`, TORDERLIST.`DT`,
IFNULL(TORDERLIST.`MC2`, TORDERLIST.`MC`) AS `MC`, 
IF(TORDERLIST.OR_NO= 'EVENT', 
   IF(TORDERLIST.TOT_QTY > TORDERLIST.BUNDLE_SIZE * TORDER_BARCODE.Barcode_SEQ, 
           TORDERLIST.BUNDLE_SIZE, TORDERLIST.TOT_QTY - TORDERLIST.BUNDLE_SIZE * ( TORDER_BARCODE.Barcode_SEQ -1)),
TORDERLIST.BUNDLE_SIZE) AS `BUNDLE_SIZE`,
TORDERLIST.`HOOK_RACK`, TORDERLIST.`WIRE`, TORDERLIST.`TERM1`, TORDERLIST.`STRIP1`, TORDERLIST.`SEAL1`,
TORDERLIST.`CCH_W1`, TORDERLIST.`ICH_W1`, TORDERLIST.`TERM2`, TORDERLIST.`STRIP2`, TORDERLIST.`SEAL2`, TORDERLIST.`CCH_W2`, TORDERLIST.`ICH_W2`, TORDERLIST.`SP_ST`, TORDERLIST.`REP`,
TORDERLIST.`DSCN_YN`, IFNULL(TORDERLIST.`PERFORMN`, 0) AS `PERFORMN`, TORDERLIST.`CONDITION`, torder_bom.`torder_bom_IDX`, IFNULL(torder_bom.`T1_TOOL_IDX`, '') AS `T1_TOO_IDX`, 
IFNULL((SELECT ttoolmaster2.`SEQ` FROM ttoolmaster2 WHERE ttoolmaster2.TOOLMASTER_IDX = torder_bom.T1_TOOL_IDX),'') AS `T1_NM`,
IFNULL((SELECT ttoolmaster2.`WK_CNT` FROM ttoolmaster2 WHERE ttoolmaster2.TOOLMASTER_IDX = torder_bom.T1_TOOL_IDX),0) AS `T1_CONT`, IFNULL(torder_bom.T2_TOOL_IDX,'') AS `T2_TOOL_IDX`, 
IFNULL((SELECT ttoolmaster2.`SEQ` FROM ttoolmaster2 WHERE ttoolmaster2.TOOLMASTER_IDX = torder_bom.T2_TOOL_IDX),'') AS `T2_NM`,
IFNULL((SELECT ttoolmaster2.`WK_CNT` FROM ttoolmaster2 WHERE ttoolmaster2.TOOLMASTER_IDX = torder_bom.T2_TOOL_IDX),0) AS `T2_CONT`,torder_bom.ERROR_CHK, 
TORDER_BARCODE.TORDER_BARCODE_IDX, TORDER_BARCODE.TORDER_BARCODENM, TORDER_BARCODE.Barcode_SEQ, TORDER_BARCODE.TORDER_BC_PRNT, TORDER_BARCODE.TORDER_BC_WORK,
TORDER_BARCODE.DSCN_YN, TORDER_BARCODE.WORK_START, TORDER_BARCODE.WORK_END, TORDER_BARCODE.CREATE_DTM, TORDER_BARCODE.CREATE_USER, TORDER_BARCODE.UPDATE_DTM,
TORDER_BARCODE.UPDATE_USER,torder_lead_bom.LEAD_INDEX, IFNULL(torder_lead_bom.W_PN_IDX,'') AS `W_PN_IDX`, IFNULL(torder_lead_bom.T1_PN_IDX,'') AS `T1_PN_IDX`, 
IFNULL(torder_lead_bom.S1_PN_IDX, '') AS `S1_PN_IDX`, IFNULL(torder_lead_bom.T2_PN_IDX,'') AS `T2_PN_IDX`, IFNULL(torder_lead_bom.S2_PN_IDX,'') AS `S2_PN_IDX`,
torder_lead_bom.STRIP1, torder_lead_bom.STRIP2, torder_lead_bom.CCH_W1, torder_lead_bom.ICH_W1, torder_lead_bom.CCH_W2, torder_lead_bom.ICH_W2, torder_lead_bom.T1NO,
torder_lead_bom.T2NO, torder_lead_bom.W_LINK, torder_lead_bom.WR_NO, torder_lead_bom.WIRE_NM, torder_lead_bom.W_Diameter, torder_lead_bom.W_Color, 
(SELECT `PART_NO` FROM tspart WHERE `PART_IDX`=(SELECT `W_PN_IDX`  FROM torder_lead_bom WHERE (`LEAD_PN` = TORDERLIST.`LEAD_NO`))) AS `WIRE_PARTNO`, 
IFNULL(torder_lead_bom.`W_Length`,0) AS `W_Length`, (SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.LEAD_NO = TORDERLIST.LEAD_NO) AS `HOOK_RACK`,
IFNULL((SELECT TTOOLMASTER.MAX_CNT FROM TTOOLMASTER WHERE TTOOLMASTER.`APPLICATOR` = TORDERLIST.TERM1), 1000000) AS `TOOL_MAX1`,
IFNULL((SELECT TTOOLMASTER.MAX_CNT FROM TTOOLMASTER WHERE TTOOLMASTER.`APPLICATOR` = TORDERLIST.TERM2), 1000000) AS `TOOL_MAX2`

FROM (TORDER_BARCODE INNER JOIN (torder_bom INNER JOIN TORDERLIST ON torder_bom.ORDER_IDX = TORDERLIST.ORDER_IDX) ON TORDER_BARCODE.ORDER_IDX = torder_bom.ORDER_IDX) INNER JOIN torder_lead_bom ON TRIM(TORDERLIST.LEAD_NO) = TRIM(torder_lead_bom.LEAD_PN)

WHERE TORDERLIST.ORDER_IDX = '" + Label8 + "' AND torder_bom.ERROR_CHK = 'Y' AND TORDER_BARCODE.DSCN_YN = 'N' ORDER BY torder_barcode.Barcode_SEQ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.Search.Count <= 0)
                        {
                            sql = @"SELECT TORDER_BARCODE.TORDER_BARCODENM, TORDER_BARCODE.WORK_END FROM TORDER_BARCODE WHERE TORDER_BARCODE.DSCN_YN = 'N' AND TORDER_BARCODE.ORDER_IDX = '" + Label8 + "' ORDER BY Barcode_SEQ";
                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.Search1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.Search1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            var COM_COUNT = result.Search1.Count;
                            if (COM_COUNT == 0)
                            {
                                IsCheck = false;
                                sql = @"Update TORDERLIST SET `CONDITION` = 'Complete', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE (`ORDER_IDX` = '" + Label8 + "')";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                            else
                            {
                                IsCheck = false;
                            }
                        }

                        result.DataGridView = await GetScrapList(Label8);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private async Task<List<SuperResultTranfer>?> GetScrapList(string label8)
        {
            var result = new List<SuperResultTranfer>();

            string sql = @"SELECT a.ORDER_IDX,a.WireNumber,a.Date as DT,a.MC,a.TERM1,a.PriceTerm1, a.SEAL1,a.SealPrice1, a.WireCode,a.WirePrice,a.WireLenght as WIRE_LENGTH,a.TERM2,a.TermPrice2,a.SEAL2,a.SealPrice2,a.CREATE_DTM, a.CREATE_USER, a.UPDATE_DTM, a.UPDATE_USER
                                FROM ScrapCutting a WHERE a.order_IDX = @OrderIdx ORDER BY a.WireNumber desc;";

            var parameters1 = new[] {
                            new MySqlParameter("@OrderIdx", MySqlDbType.Int32) { Value = label8 } ,
                        };

            var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters1);

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt)); // lưu thông tin hàng đã scan vào biến: ListtdpdmtimTranfer
            }

            return result;
        }

        public virtual async Task<BaseResult> DB_COUTN(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var MCbox = BaseParameter.ListSearchString[0];
                        string sqlResult = "";
                        string sql = @"SELECT `MC_NO`, SUM(`WK_QTY`) AS `SUM` FROM TWWKAR WHERE  `MC_NO` = '" + MCbox + "' AND `CREATE_DTM` > CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL - 1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00') AND `CREATE_DTM` < CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL + 1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00')  ";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> OPER_TIME(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var MCbox = BaseParameter.ListSearchString[0];
                        string sqlResult = "";
                        string sql = @"SELECT (SUM(TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))) AS `SUM_TIME` FROM TSNON_OPER WHERE `TSNON_OPER_MCNM` = '" + MCbox + "' AND NOT(`TSNON_OPER_CODE`= 'S') AND `CREATE_DTM` > CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL - 1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00') AND `CREATE_DTM` < CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL + 1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00')";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> SPC_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var Label8 = BaseParameter.ListSearchString[0];
                        string sqlResult = "";
                        string sql = @"SELECT `COLSIP`, (SELECT TORDERLIST.TOT_QTY FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = torderinspection.ORDER_IDX) AS `WORK_QTY` FROM torderinspection WHERE torderinspection.ORDER_IDX = '" + Label8 + "'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView3 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var USER_MC = BaseParameter.ListSearchString[0];
                        var USER_ORIDX = BaseParameter.ListSearchString[1];
                        var FormText = BaseParameter.ListSearchString[2];
                        var ST_TM = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string sqlResult = "";

                        string sql = @"INSERT INTO  `TORDERLIST_WTIME`  (`USER_ID`, `USER_MC`, `ORDER_IDX`, `S_TIME`, `CREATE_DTM`, `CREATE_USER`, `MENU_TEXT`) VALUES ('" + USER_ID + "', '" + USER_ID + "', '" + USER_ORIDX + "', NOW(), '" + ST_TM + "', '" + USER_ID + "', '" + FormText + "')";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = "SELECT  TORDERLIST_WTIME.TOWT_INDX    FROM   TORDERLIST_WTIME  WHERE TORDERLIST_WTIME.CREATE_DTM ='" + ST_TM + "'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView4 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> C02_FormClosed(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var SettingsMC_NM = BaseParameter.ListSearchString[0];
                        string sqlResult = "";

                        string sql = @"INSERT INTO `torderlist_work` (`torderlist_work_MC`, `torderlist_work_ORDERIDX`, `torderlist_work_USER`) VALUES ('" + SettingsMC_NM + "', '0', 'NOT_USER')  ON DUPLICATE KEY UPDATE  `torderlist_work_ORDERIDX` = '0', `torderlist_work_USER` = 'NOT_USER'";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `torderlist_work`     AUTO_INCREMENT= 1";
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
        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        bool IsCheck = true;
                        string sql = "";
                        string sqlResult = "";
                        var Label5 = BaseParameter.ListSearchString[0];
                        var Tcount = BaseParameter.ListSearchString[1];
                        var SHIELDWIRE_CHK = bool.Parse(BaseParameter.ListSearchString[2]);
                        var VLA1 = BaseParameter.ListSearchString[3];
                        var TOOL1_IDX = BaseParameter.ListSearchString[4];
                        var T1_CONT = int.Parse(BaseParameter.ListSearchString[5]);
                        var Label4 = BaseParameter.ListSearchString[6];
                        var VLA2 = BaseParameter.ListSearchString[7];
                        var TOOL2_IDX = BaseParameter.ListSearchString[8];
                        var T2_CONT = int.Parse(BaseParameter.ListSearchString[9]);
                        var Label8 = BaseParameter.ListSearchString[10];
                        var PERFORMN = int.Parse(BaseParameter.ListSearchString[11]);
                        var MCbox = BaseParameter.ListSearchString[12];

                        var TrolleyCode = BaseParameter.ListSearchString[14];
                        var safeTrolleyCode = TrolleyCode.Replace("'", "''");

                        var SAA = Label5;
                        var SQL_TEXT01 = "";
                        var SQL_TEXT02 = "";
                        var SQL_TEXT03 = "";
                        var SQL_TEXT04 = "";
                        var SQL_TEXT05 = "";
                        var SQL_TEXT06 = "";
                        var SQL_TEXT07 = "";
                        var SQL_TEXT08 = "";

                        //load dữ liệu tính số lượng đã có hay chưa, nếu có rồi bỏ qua không tính thêm
                        sql = @"SELECT TORDER_BARCODE.TORDER_BARCODE_IDX, TORDER_BARCODE.TORDER_BARCODENM FROM TORDER_BARCODE WHERE TORDER_BARCODE.TORDER_BARCODE_IDX = '" + Label5 + "' AND TORDER_BARCODE.DSCN_YN = 'N';";
                        DataSet ds_check = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        var listCheck = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds_check.Tables.Count; i++)
                        {
                            DataTable dt = ds_check.Tables[i];
                            listCheck.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        //nếu tem in là tem chưa cập nhật, thì thực hiện cập nhật và tính số lượng đã làm
                        if (listCheck.Count > 0)
                        {
                            var barcode = listCheck[0].TORDER_BARCODENM.Trim();
                            var WORK_QTY = int.Parse(barcode.Split("$$")[1]);
                            if (SHIELDWIRE_CHK == false)
                            {
                                if (VLA1 == "")
                                {
                                }
                                else
                                {
                                    SQL_TEXT02 = @"UPDATE ttoolmaster2 SET `TOT_WK_CNT`= `TOT_WK_CNT` + " + WORK_QTY + ", `WK_CNT` = `WK_CNT` + " + WORK_QTY + ", `UPDATE_DTM`=NOW(), `UPDATE_USER`='" + C_USER + "' WHERE `TOOLMASTER_IDX`=" + TOOL1_IDX;
                                    var A_CONT = T1_CONT + WORK_QTY;
                                    SQL_TEXT03 = @"INSERT INTO TWTOOL (`TOOL_IDX`, `TOOL_WORK`, `WK_QTY`, `TOT_WK_QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES (" + TOOL1_IDX + ", '" + Label4 + "', " + WORK_QTY + ", " + A_CONT + ", NOW(), '" + C_USER + "')";
                                }
                                if (VLA2 == "")
                                {
                                }
                                else
                                {
                                    SQL_TEXT04 = @"UPDATE ttoolmaster2 SET `TOT_WK_CNT`= `TOT_WK_CNT` + " + WORK_QTY + ", `WK_CNT` = `WK_CNT` + " + WORK_QTY + ", `UPDATE_DTM`=NOW(), `UPDATE_USER`='" + C_USER + "' WHERE `TOOLMASTER_IDX`=" + TOOL2_IDX;
                                    var B_CONT = T2_CONT + WORK_QTY;
                                    SQL_TEXT05 = "INSERT INTO TWTOOL (`TOOL_IDX`, `TOOL_WORK`, `WK_QTY`, `TOT_WK_QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES (" + TOOL2_IDX + ", '" + Label4 + "', " + WORK_QTY + ", " + B_CONT + ", NOW(), '" + C_USER + "')";
                                }
                            }
                            SQL_TEXT06 = "Update TORDERLIST SET `CONDITION` = 'Working' , `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + C_USER + "' WHERE (`ORDER_IDX` = " + Label8 + ")";



                            SQL_TEXT07 = "UPDATE `TORDERLIST` SET `PERFORMN`= `PERFORMN` + " + WORK_QTY + " WHERE `ORDER_IDX`=" + Label8;

                            SQL_TEXT08 = "INSERT INTO TWWKAR (`PART_IDX`, `WK_QTY`, `CREATE_DTM`, `CREATE_USER`, `MC_NO`, `TORDER_IDX`, `TORDER_BARCODE_IDX`) VALUES ('" + Label4 + "', " + WORK_QTY + ", NOW(), '" + C_USER + "', '" + MCbox + "', '" + Label8 + "','" + Label5 + "')";


                            if (SQL_TEXT02 == "")
                            {
                            }
                            else
                            {
                                sql = SQL_TEXT02;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                            if (SQL_TEXT03 == "")
                            {
                            }
                            else
                            {
                                sql = SQL_TEXT03;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                            if (SQL_TEXT04 == "")
                            {
                            }
                            else
                            {
                                sql = SQL_TEXT04;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                            if (SQL_TEXT05 == "")
                            {
                            }
                            else
                            {
                                sql = SQL_TEXT05;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                            if (SQL_TEXT06 == "")
                            {
                            }
                            else
                            {
                                sql = SQL_TEXT06;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                            if (SQL_TEXT07 == "")
                            {
                            }
                            else
                            {
                                sql = SQL_TEXT07;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                            if (SQL_TEXT08 == "")
                            {
                            }
                            else
                            {
                                sql = SQL_TEXT08;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }



                            sql = @"SELECT TORDER_BARCODE.TORDER_BARCODENM, TORDER_BARCODE.WORK_END FROM TORDER_BARCODE WHERE TORDER_BARCODE.DSCN_YN = 'N' AND TORDER_BARCODE.ORDER_IDX = '" + Label8 + "' ORDER BY Barcode_SEQ";
                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView5 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView5.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            var COM_COUNT = result.DataGridView5.Count;
                            if (COM_COUNT == 0)
                            {
                                sql = @"Update TORDERLIST SET `CONDITION` = 'Complete', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + C_USER + "' WHERE (`ORDER_IDX` = '" + Label8 + "')";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }

                            //update trạng thái đã in tem
                            SQL_TEXT01 = @"UPDATE `TORDER_BARCODE` SET `TORDER_BC_PRNT`='Y', `TORDER_BC_WORK`='Y', `DSCN_YN`='Y', `TrolleyCode`='" + safeTrolleyCode + @"', `WORK_END`= NOW(), `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + C_USER + "' WHERE `TORDER_BARCODE_IDX`= '" + Label5 + "'";

                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL_TEXT01);

                            //lấy danh sách tem đã in để tính số lượng đã làm việc      

                            sql = @"Select * from TORDER_BARCODE where TORDER_BARCODE.ORDER_IDX = '" + Label8 + "' and TORDER_BARCODE.DSCN_YN = 'Y' ORDER BY Barcode_SEQ";
                            DataSet dsCheck = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            var listBCCheck = new List<SuperResultTranfer>();
                            for (int i = 0; i < dsCheck.Tables.Count; i++)
                            {
                                DataTable dt = dsCheck.Tables[i];
                                listBCCheck.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            var totalWork = 0;

                            foreach (var item in listBCCheck)
                            {
                                var soluongTem = int.Parse(item.TORDER_BARCODENM.Split("$$")[1]);
                                totalWork = totalWork + soluongTem;
                            }

                            //cập nhật lại số lượng đã làm chính xác hơn
                            sql = "UPDATE TORDERLIST SET TORDERLIST.PERFORMN = '" + totalWork + "' WHERE TORDERLIST.ORDER_IDX = '" + Label8 + "';";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

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
        public virtual async Task<BaseResult> PrintDocument1_PrintPage(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string SheetName = this.GetType().Name;
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var BARCODE_DATE = GlobalHelper.InitializationDateTime;
                        var BARCODE_QR = BaseParameter.ListSearchString[0];
                        var PR1 = BaseParameter.ListSearchString[1];
                        var PR2 = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd");
                        var PR3 = C_USER;
                        var PR4 = BaseParameter.ListSearchString[2];
                        var PR5 = BaseParameter.ListSearchString[3];
                        var PR6 = BaseParameter.ListSearchString[4];
                        var PR7 = BaseParameter.ListSearchString[5];
                        var PR8 = BaseParameter.ListSearchString[6];
                        var PR9 = BaseParameter.ListSearchString[7];
                        var PR10 = BaseParameter.ListSearchString[8];
                        var PR11 = BaseParameter.ListSearchString[9];
                        var PR12 = BaseParameter.ListSearchString[10];
                        var PR13 = BaseParameter.ListSearchString[11];
                        var PR14 = BaseParameter.ListSearchString[12];
                        var PR15 = BaseParameter.ListSearchString[13];
                        var PR16 = BaseParameter.ListSearchString[14];
                        var PR17 = BaseParameter.ListSearchString[15];
                        var PR18 = "CCH.W";
                        var PR19 = "ICH.W";
                        var PR20 = BaseParameter.ListSearchString[16];
                        var PR21 = BaseParameter.ListSearchString[17];
                        var PR22 = BaseParameter.ListSearchString[18];
                        var PR23 = BaseParameter.ListSearchString[19];
                        var PR24 = "";
                        var PR25 = "-";                  
                        var OR_NO = BaseParameter.ListSearchString[20];
                        var productCode = BaseParameter.ListSearchString[21];
                        var MC = BaseParameter.ListSearchString[22];
                        if (PR4.Length > 10)
                        {
                            PR4 = PR4.Substring(0, 10);
                        }

                        string sql = @"SELECT TTENSILBNDLST.STRENGTH FROM TTENSILBNDLST WHERE TTENSILBNDLST.BNDLST_MIN <= (SELECT IFNULL(torder_lead_bom.W_Length, '-') AS `BUND` FROM  torder_lead_bom  WHERE torder_lead_bom.LEAD_PN = '" + PR5 + "') AND TTENSILBNDLST.BNDLST_MAX >= (SELECT IFNULL(torder_lead_bom.W_Length, '-') AS `BUND` FROM torder_lead_bom  WHERE torder_lead_bom.LEAD_PN = '" + PR5 + "') ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.DataGridView.Count > 0)
                        {
                            PR25 = result.DataGridView[0].STRENGTH.ToString();
                        }

                        if (OR_NO == "EVENT")
                        {
                            PR24 = "E";
                        }


                        //StringBuilder HTMLContent = new StringBuilder();
                        //HTMLContent.AppendLine(GlobalHelper.CreateHTMLC02(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, PR1, PR2, PR3, PR4, PR5, PR6, PR7.ToString(), PR8.ToString(), PR9, PR10, PR11, PR12, PR13, PR14, PR15, PR16, PR17, PR18, PR19, PR20.ToString(), PR21, PR22, PR23, PR24, PR25));
                        //result.Code = GlobalHelper.CreateHTMLClose(SheetName, _WebHostEnvironment.WebRootPath, HTMLContent.ToString());
                        result.DataGridView1 = new List<SuperResultTranfer>();

                        result.DataGridView1.Add(new SuperResultTranfer
                        {
                            DATE = BARCODE_DATE,
                            Barcode = BARCODE_QR,
                            D01 = PR1,
                            D02 = PR2,
                            D03 = PR3,
                            D04 = PR4,
                            D05 = PR5,
                            D06 = PR6,
                            D07 = PR7,
                            D08 = PR8,
                            D09 = PR9,
                            D10 = PR10,
                            D11 = PR11,
                            D12 = PR12,
                            D13 = PR13,
                            D14 = PR14,
                            D15 = PR15,
                            D16 = PR16,
                            D17 = PR17,
                            D18 = PR18,
                            D19 = PR19,
                            D20 = PR20,
                            D21 = PR21,
                            D22 = PR22,
                            D23 = PR23,
                            D24 = PR24,
                            D25 = PR25,
                            D26 = productCode,
                            D27 = MC,
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
        public virtual async Task<BaseResult> EW_TIME(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var WORING_IDX = BaseParameter.ListSearchString[0];
                        string sqlResult = "";

                        string sql = @"UPDATE  `TORDERLIST_WTIME`   SET `E_TIME`= NOW()    WHERE  `TOWT_INDX` = '" + WORING_IDX + "'";
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

        public virtual async Task<BaseResult> ScrapSave(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ImportData != null)
                    {
                        var WireCode = BaseParameter.ImportData[0].WIRE;
                        var WireLength = BaseParameter.ImportData[0].WIRE_LENGTH;
                        var Term1 = BaseParameter.ImportData[0].TERM1;
                        var Term2 = BaseParameter.ImportData[0].TERM2;
                        var Seal1 = BaseParameter.ImportData[0].SEAL1;
                        var Seal2 = BaseParameter.ImportData[0].SEAL2;
                        var Order_IDX = BaseParameter.ImportData[0].ORDER_IDX;
                        var MC = BaseParameter.ImportData[0].MC;
                        var wireCount = BaseParameter.ImportData[0].COUNT;

                        string sqlResult = "";
                        string sql = "";

                        var sb = new StringBuilder();

                        for (int i = 1; i <= wireCount; i++)
                        {
                            sb.Append(@"INSERT INTO ScrapCutting ( ORDER_IDX,   
                                                                    WireNumber,  
                                                                    Date,  
                                                                    MC,  
                                                                    Term1,  
                                                                    PriceTerm1, 
                                                                    Seal1,  
                                                                    SealPrice1, 
                                                                    WireCode,
                                                                    WireLenght,
                                                                    WirePrice,
                                                                    Term2,
                                                                    TermPrice2,
                                                                    Seal2,
                                                                    SealPrice2,
                                                                    CREATE_DTM,
                                                                    CREATE_USER,
                                                                    UPDATE_DTM,
                                                                    UPDATE_USER
                                                                )
                                                            SELECT
                                                                @OrderIdx AS ORDER_IDX,

                                                                IFNULL(
                                                                    (
                                                                        SELECT MAX(WireNumber) + 1
                                                                        FROM ScrapCutting
                                                                        WHERE ORDER_IDX = @OrderIdx                                                                        
                                                                    ),
                                                                    1
                                                                ) AS WireNumber,

                                                                @TheDate AS Date,
                                                                @MC AS MC,
                                                                @Term1 AS Term1,
                                                                @PriceTerm1 AS PriceTerm1,
                                                                @Seal1 AS Seal1,
                                                                @SealPrice1 AS SealPrice1,
                                                                @WireCode AS WireCode,
                                                                @WireLenght AS WireLenght,
                                                                @WirePrice AS WirePrice,
                                                                @Term2 AS Term2,
                                                                @TermPrice2 AS TermPrice2,
                                                                @Seal2 AS Seal2,
                                                                @SealPrice2 AS SealPrice2,
                                                                NOW() AS CREATE_DTM,
                                                                @User AS CREATE_USER,
                                                                NOW() AS UPDATE_DTM,
                                                                @User AS UPDATE_USER;
                                                            ");
                        }
                        sql = sb.ToString();

                        var parameters = new[] {
                            new MySqlParameter("@OrderIdx", MySqlDbType.Int32) { Value = Order_IDX } ,
                            new MySqlParameter("@TheDate", MySqlDbType.Date) { Value = DateTime.Now.ToString("yyyy-MM-dd") } ,
                            new MySqlParameter("@MC", MySqlDbType.VarChar) { Value = MC.Trim() } ,
                            new MySqlParameter("@Term1", MySqlDbType.VarChar) { Value = Term1.Trim() } ,
                            new MySqlParameter("@PriceTerm1", MySqlDbType.Double) { Value = 0 } ,
                            new MySqlParameter("@Seal1", MySqlDbType.VarChar) { Value = Seal1.Trim() } ,
                            new MySqlParameter("@SealPrice1", MySqlDbType.Double) { Value = 0 } ,
                            new MySqlParameter("@WireCode", MySqlDbType.VarChar) { Value = WireCode.Trim() } ,
                            new MySqlParameter("@WireLenght", MySqlDbType.Int32) { Value = WireLength } ,
                            new MySqlParameter("@WirePrice", MySqlDbType.Double) { Value = 0 } ,
                            new MySqlParameter("@Term2", MySqlDbType.VarChar) { Value = Term2.Trim() } ,
                            new MySqlParameter("@TermPrice2", MySqlDbType.Double) { Value = 0 } ,
                            new MySqlParameter("@Seal2", MySqlDbType.VarChar) { Value = Seal2.Trim() } ,
                            new MySqlParameter("@SealPrice2", MySqlDbType.Double) { Value = 0 } ,
                            new MySqlParameter("@User", MySqlDbType.VarChar) { Value = USER_ID } ,
                        };

                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql, parameters);
                                               
                        result.DataGridView = await GetScrapList(Order_IDX.ToString());                        

                        result.Message = "Successfull";
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


