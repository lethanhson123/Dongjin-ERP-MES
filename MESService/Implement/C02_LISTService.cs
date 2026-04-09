
namespace MESService.Implement
{
    public class C02_LISTService : BaseService<torderlist, ItorderlistRepository>
    , IC02_LISTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C02_LISTService(ItorderlistRepository torderlistRepository

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
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var LEAD_NO_SK = BaseParameter.ListSearchString[0];
                        var MCbox = BaseParameter.ListSearchString[1];

                        string updateOrrderTatus = @$"UPDATE TORDERLIST set TORDERLIST.`CONDITION` = 'Stay'

                                                        WHERE TORDERLIST.PERFORMN is NULL  AND TORDERLIST.LEAD_NO = '" + LEAD_NO_SK + "'  and TORDERLIST.`CONDITION` = 'Complete' AND `DT` >= DATE_ADD(NOW(), INTERVAL - 15 DAY) and TORDERLIST.`DSCN_YN` = 'Y' ";

                        var kq = await MySQLHelperV2.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateOrrderTatus);


                        string sql = @"SELECT 'FALSE' AS `CHK`, IF(LEFT(`OR_NO`, 1)='N','','E') AS `OR_NO`, `WORK_WEEK`, `CONDITION`, `TORDER_FG`, `LEAD_NO`, IF(`CONDITION` = 'Stay' , IFNULL(`MTRL_RQUST` ,'N'), 'F') AS `MTRL_RQUST`, 
                        `WIRE`, IFNULL(`TOEXCEL_QTY`, 0) AS `TOEXCEL_QTY`, IFNULL(`B`.`QTY_STOCK`, 0) AS `QTY_STOCK`, `TOT_QTY`, 0 AS `MES_QTY`, `PERFORMN`, IFNULL(`MC2`, `MC`) AS `MC`, `ADJ_AF_QTY`, `TERM1`, `SEAL1`, `TERM2`, `SEAL2`, `CCH_W1`, `ICH_W1`, `CCH_W2`, `ICH_W2`, `DT`, derivedtbl_1.`LS_DATE`, `PROJECT`,
                        `CUR_LEADS`, `CT_LEADS`, `CT_LEADS_PR`, `GRP`, `BUNDLE_SIZE`, `HOOK_RACK`, `T1_DIR`, `STRIP1`, `T2_DIR`, `STRIP2`, `SP_ST`, `DSCN_YN`, `REP`, `ORDER_IDX`

                        FROM  (TORDERLIST LEFT OUTER JOIN
                        (SELECT  TORDER_IDX, MAX(CREATE_DTM) AS LS_DATE  FROM   TWWKAR   GROUP BY TORDER_IDX) derivedtbl_1 ON TORDERLIST.ORDER_IDX = derivedtbl_1.TORDER_IDX)
                        LEFT OUTER JOIN   (SELECT `LEAD_PN`, `QTY` AS `QTY_STOCK` FROM  torder_lead_bom LEFT OUTER JOIN tiivtr_lead ON LEAD_INDEX = PART_IDX AND  tiivtr_lead.`LOC_IDX` = '3') AS `B` ON `B`.LEAD_PN = TORDERLIST.LEAD_NO

                        WHERE  TORDERLIST.`DSCN_YN` = 'Y' AND TORDERLIST.LEAD_NO = '" + LEAD_NO_SK + "'   AND  NOT(TORDERLIST.`CONDITION` = 'Close')    AND `DT` >= DATE_ADD(NOW(), INTERVAL - 15 DAY) ORDER BY TORDERLIST.`CONDITION` DESC, TORDERLIST.DT DESC, TORDERLIST.LEAD_NO   ";

                        //string sql = @"SELECT 'FALSE' AS `CHK`, IF(LEFT(`OR_NO`, 1)='N','','E') AS `OR_NO`, `WORK_WEEK`, `CONDITION`, `TORDER_FG`, `LEAD_NO`, IF(`CONDITION` = 'Stay' , IFNULL(`MTRL_RQUST` ,'N'), 'F') AS `MTRL_RQUST`, 
                        //`WIRE`, IFNULL(`TOEXCEL_QTY`, 0) AS `TOEXCEL_QTY`, IFNULL(`B`.`QTY_STOCK`, 0) AS `QTY_STOCK`, `TOT_QTY`, 0 AS `MES_QTY`, `PERFORMN`, IFNULL(`MC2`, `MC`) AS `MC`, `ADJ_AF_QTY`, `TERM1`, `SEAL1`, `TERM2`, `SEAL2`, `CCH_W1`, `ICH_W1`, `CCH_W2`, `ICH_W2`, `DT`, derivedtbl_1.`LS_DATE`, `PROJECT`,
                        //`CUR_LEADS`, `CT_LEADS`, `CT_LEADS_PR`, `GRP`, `BUNDLE_SIZE`, `HOOK_RACK`, `T1_DIR`, `STRIP1`, `T2_DIR`, `STRIP2`, `SP_ST`, `DSCN_YN`, `REP`, `ORDER_IDX`

                        //FROM  (TORDERLIST LEFT OUTER JOIN
                        //(SELECT  TORDER_IDX, MAX(CREATE_DTM) AS LS_DATE  FROM   TWWKAR   GROUP BY TORDER_IDX) derivedtbl_1 ON TORDERLIST.ORDER_IDX = derivedtbl_1.TORDER_IDX)
                        //LEFT OUTER JOIN   (SELECT `LEAD_PN`, `QTY` AS `QTY_STOCK` FROM  torder_lead_bom LEFT OUTER JOIN tiivtr_lead ON LEAD_INDEX = PART_IDX AND  tiivtr_lead.`LOC_IDX` = '3') AS `B` ON `B`.LEAD_PN = TORDERLIST.LEAD_NO

                        //WHERE  TORDERLIST.`DSCN_YN` = 'Y' AND TORDERLIST.LEAD_NO = '" + LEAD_NO_SK + "' AND (TORDERLIST.MC = '" + MCbox + "' OR TORDERLIST.MC2 = '" + MCbox + "')   AND  NOT(TORDERLIST.`CONDITION` = 'Close')    AND `DT` >= DATE_ADD(NOW(), INTERVAL - 15 DAY) ORDER BY TORDERLIST.`CONDITION` DESC, TORDERLIST.DT DESC, TORDERLIST.LEAD_NO   ";

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
                var C_USER = BaseParameter.USER_ID;
                if (BaseParameter.DataGridView1 != null)
                {
                    if (BaseParameter.DataGridView1.Count > 0)
                    {
                        foreach (var item in BaseParameter.DataGridView1)
                        {
                            if (item.CHK == true)
                            {
                                var AA = item.ORDER_IDX;
                                var BB = item.MC;
                                string sql = @"UPDATE TORDERLIST SET `MC2`='" + BB + "' WHERE `ORDER_IDX` = " + AA;

                                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
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
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.DataGridView1 != null)
                    {
                        if (BaseParameter.DataGridView1.Count > 0)
                        {
                            var CHK = new List<int>();
                            var CONT = 0;
                            foreach (var item in BaseParameter.DataGridView1)
                            {
                                if (item.CHK == true)
                                {
                                    CHK.Add(item.ORDER_IDX.Value);
                                    CONT = CONT + 1;
                                }
                            }
                            if (CHK.Count > 0)
                            {
                                for (int i = 0; i < CHK.Count; i++)
                                {
                                    string sql = @"UPDATE TORDERLIST SET `CONDITION`='Close' WHERE  `ORDER_IDX`= " + CHK[i];

                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                            }
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
        public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var IDX_CHK = BaseParameter.ListSearchString[0];

                        string sql = @"UPDATE `TORDERLIST` SET `MTRL_RQUST`='N' WHERE  `ORDER_IDX`= '" + IDX_CHK + "'  ";

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
        public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.DataGridView1 != null)
                    {
                        if (BaseParameter.DataGridView1.Count > 0)
                        {
                            foreach (var item in BaseParameter.DataGridView1)
                            {
                                if (item.CHK == true)
                                {
                                    var OO = item.ORDER_IDX;

                                    string sql = @"UPDATE TORDERLIST SET `PERFORMN` = (SELECT ((SELECT (COUNT(`DSCN_YN`)) FROM TORDER_BARCODE WHERE `ORDER_IDX` = '" + OO + "' AND `DSCN_YN`='Y') * TORDERLIST.`BUNDLE_SIZE`) FROM TORDERLIST WHERE((SELECT(COUNT(TORDER_BARCODE.`DSCN_YN`)) FROM TORDER_BARCODE WHERE `ORDER_IDX` = TORDERLIST.`ORDER_IDX` AND `DSCN_YN`= 'Y') * TORDERLIST.`BUNDLE_SIZE`) <> TORDERLIST.`PERFORMN`), `CONDITION`= 'Close' WHERE `ORDER_IDX` = '" + OO + "'";

                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                            }
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
        public virtual async Task<BaseResult> DataGridView1_CellClick3(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var AAA = BaseParameter.ListSearchString[0];
                        var BBB = int.Parse(BaseParameter.ListSearchString[1]);
                        var CCC = DateTime.Parse(BaseParameter.ListSearchString[2]).ToString("yyyy-MM-dd");
                        var DDD = int.Parse(BaseParameter.ListSearchString[3]);
                        var FFF = BaseParameter.ListSearchString[4];

                        // Thêm TrolleyCode (index 5)
                        var TrolleyCode = "";

                        // Escape để tránh SQL Injection
                        var safeTrolleyCode = TrolleyCode.Replace("'", "''");

                        string sql = @"SELECT * FROM torder_bom WHERE torder_bom.ORDER_IDX = '" + FFF + "'";
                        string sqlResult = "";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        if (result.Search.Count <= 0)
                        {
                            sql = @"INSERT INTO torder_bom (`ORDER_IDX`, `ERROR_CHK`, `CREATE_DTM`, `CREATE_USER`) VALUE ('" + FFF + "', 'N', NOW(), '" + C_USER + "')  ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"ALTER TABLE `torder_bom` AUTO_INCREMENT= 1";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }

                        sql = @"SELECT TORDER_BARCODE_IDX, TORDER_BARCODENM, ORDER_IDX FROM TORDER_BARCODE WHERE ORDER_IDX = '" + FFF + "'";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        var BARCODE_CNT = result.Search1.Count;
                        if (BARCODE_CNT <= 0)
                        {
                            var AA = DDD / BBB;
                            var AA1 = DDD % BBB;
                            if (AA1 > 0)
                            {
                                AA = AA + 1;
                            }
                            var II = 1;
                            var DCO = DDD;
                            var VALUES = "";
                            var VALUESSUM = "";
                            for (II = 1; II <= AA; II++)
                            {
                                var BARNM = "";
                                if (DCO > BBB)
                                {
                                    BARNM = AAA + "$$" + BBB + "$$" + CCC + "_" + FFF + "$$" + DDD + "$$" + II;
                                }
                                else
                                {
                                    BARNM = AAA + "$$" + DCO + "$$" + CCC + "_" + FFF + "$$" + DDD + "$$" + II;
                                }

                                // Thêm TrolleyCode vào INSERT
                                VALUES = "('" + BARNM + "', " + FFF + ", " + II + ", 'N', 'Y', 'N', '" + safeTrolleyCode + "', NOW(), '" + C_USER + "')";

                                if (VALUESSUM == "")
                                {
                                    VALUESSUM = VALUES;
                                }
                                else
                                {
                                    VALUESSUM = VALUESSUM + ", " + VALUES;
                                }
                                DCO = DCO - BBB;
                            }

                            // Thêm cột TrolleyCode vào INSERT statement
                            sql = @"INSERT INTO TORDER_BARCODE (`TORDER_BARCODENM`, `ORDER_IDX`, `Barcode_SEQ`, `TORDER_BC_PRNT`, `TORDER_BC_WORK`, `DSCN_YN`, `TrolleyCode`, `CREATE_DTM`, `CREATE_USER`) VALUES " + VALUESSUM;
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
        public virtual async Task<BaseResult> DB_LISECHK(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"UPDATE TORDERLIST SET `CONDITION`='Close' WHERE `DT` < DATE_ADD(NOW(), INTERVAL -11 DAY) AND NOT (TORDERLIST.`CONDITION` = 'Complete')";

                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> TS_USER(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    var MCbox = BaseParameter.SearchString;
                    string sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_TIME_ST` > DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d') AND  `TS_USER` = '" + C_USER + "'";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.USER_TIME = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.USER_TIME.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    var USER_COUNT = result.USER_TIME.Count;
                    if (USER_COUNT <= 0)
                    {
                        sql = @"INSERT INTO `TUSER_LOG` (`TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER`) VALUES ('" + MCbox + "', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), NOW(), CONCAT(DATE_ADD(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), INTERVAL +1 DAY), ' 05:49:59'), '" + C_USER + "')";

                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_TIME_ST` > DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d') AND  `TS_USER` = '" + C_USER + "'"; ;

                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.USER_TIME = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.USER_TIME.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }

                    sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, MIN(`TS_TIME_ST`) AS `DATEMIN`, MAX(`TS_TIME_END`) AS `DATEMAX` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_DATE` = DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d')";

                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.USER_TIME1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.USER_TIME1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    if (result.USER_TIME1.Count > 0)
                    {
                        result.USER_TIME1[0].Name = result.USER_TIME1[0].DATEMIN.Value.ToString("HH:mm:ss");
                        result.USER_TIME1[0].Description = result.USER_TIME1[0].DATEMIN.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> PageLoad(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var MCbox = BaseParameter.SearchString;

                    string sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES ('" + MCbox + "', '-----', 'N','-')  ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = '-----', `tsnon_oper_mitor_RUNYN` = 'N', `StopCode`='-'";

                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

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

        public virtual async Task<BaseResult> SetMCLIST_CHK(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var SettingsMC_NM = BaseParameter.SearchString;

                    string sql = @"SELECT `CD_IDX`,  `CD_SYS_NOTE`   FROM TSCODE  WHERE `CDGR_IDX` = '8' AND NOT(`CD_IDX` = '594') AND `CD_SYS_NOTE` = '" + SettingsMC_NM + "'  ";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DGV_C02_ML = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DGV_C02_ML.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> RE_TIME(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var USER_MC = BaseParameter.ListSearchString[0];

                        string sql = @"UPDATE  `TORDERLIST_WTIME`   SET `E_TIME`= NOW()    WHERE  TORDERLIST_WTIME.`TOWT_INDX` = (SELECT  `TOWT_INDX`  FROM   TORDERLIST_WTIME   WHERE   TORDERLIST_WTIME.`USER_ID` = '" + USER_ID + "' AND TORDERLIST_WTIME.USER_MC ='" + USER_MC + "' AND TORDERLIST_WTIME.`E_TIME` IS NULL)";
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
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {

                string sql = @"SELECT `LEAD_NO`, SUM(`TOT_QTY`) AS `TOTAL_QTY`  FROM   TORDERLIST   WHERE `CONDITION` = 'STAY' AND (IFNULL(`MC2`, `MC`) LIKE 'A8%' OR IFNULL(`MC2`, `MC`) LIKE 'PLAN') GROUP BY `LEAD_NO`";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                var DGV_C02_99 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    DGV_C02_99.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
                if (DGV_C02_99.Count > 0)
                {
                    string SheetName = this.GetType().Name;
                    string D1 = GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                    string fileName = @"C02_" + D1 + "_LIST.xlsx";
                    var streamExport = new MemoryStream();
                    InitializationToExcelAsync(DGV_C02_99, streamExport, SheetName);
                    string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                    Directory.CreateDirectory(physicalPathCreate);
                    GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                    string filePath = Path.Combine(physicalPathCreate, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        streamExport.CopyTo(stream);
                    }
                    result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        private void InitializationToExcelAsync(List<SuperResultTranfer> list, MemoryStream streamExport, string SheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(SheetName);
                int row = 1;
                int column = 1;
                workSheet.Cells[1, column].Value = "LEAD_NO";
                column = column + 1;
                workSheet.Cells[1, column].Value = "TOTAL_QTY";
                column = column + 1;

                foreach (var item in list)
                {
                    try
                    {
                        workSheet.Cells[row, 1].Value = item.LEAD_NO;
                        workSheet.Cells[row, 2].Value = item.TOTAL_QTY;
                        for (int i = 1; i < column; i++)
                        {
                            workSheet.Cells[row, i].Style.Font.Name = "Arial";
                            workSheet.Cells[row, i].Style.Font.Size = 14;
                            workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                        row = row + 1;
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
                for (int i = 1; i <= column; i++)
                {
                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;
        }
        public virtual async Task<BaseResult> ValidateTrolley(string TrolleyCode)
        {
            BaseResult result = new BaseResult();
            try
            {
                string safeTrolleyCode = TrolleyCode.Trim().Replace("'", "''");

                string sql = @"SELECT TrolleyCode, Location 
                       FROM Trolley 
                       WHERE TrolleyCode = '" + safeTrolleyCode + @"' 
                       AND Active = 1 
                       LIMIT 1";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    result.Success = true;
                    result.Location = ds.Tables[0].Rows[0]["Location"].ToString();
                    result.TrolleyCode = ds.Tables[0].Rows[0]["TrolleyCode"].ToString();
                }
                else
                {
                    result.Error = "Trolley không tồn tại!";
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

