

namespace MESService.Implement
{
    public class C05_STARTService : BaseService<torderlist, ItorderlistRepository>
    , IC05_STARTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public C05_STARTService(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> C05_FormClosed(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var SettingsMC_NM = BaseParameter.SearchString;
                    string sql = @"INSERT INTO `torderlist_work` (`torderlist_work_MC`, `torderlist_work_ORDERIDX`, `torderlist_work_USER`) VALUES ('" + SettingsMC_NM + "', '0', 'NOT_USER')  ON DUPLICATE KEY UPDATE  `torderlist_work_ORDERIDX` = '0', `torderlist_work_USER` = 'NOT_USER'";
                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> C05_start_Load(BaseParameter BaseParameter)
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

                        sql = @"UPDATE torder_barcode_lp SET  `TORDER_BC_PRNT` ='L' WHERE  `ORDER_IDX` = '" + Label8 + "' AND(SELECT IF(`TERM1` = '(899997)', 'NG', IF(`TERM1` = '(899998)', 'NG', IF(`TERM1` = '(899999)', 'NG', IF(POSITION('(' IN `TERM1`) > 0, 'OK', 'NG')))) FROM TORDERLIST WHERE `ORDER_IDX` = '" + Label8 + "') = 'NG'";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"UPDATE torder_barcode_lp SET  `TORDER_BC_WORK` ='R' WHERE  `ORDER_IDX` = '" + Label8 + "' AND(SELECT IF(`TERM2` = '(899997)', 'NG', IF(`TERM2` = '(899998)', 'NG', IF(`TERM2` = '(899999)', 'NG', IF(POSITION('(' IN `TERM2`) > 0, 'OK', 'NG')))) FROM TORDERLIST WHERE `ORDER_IDX` = '" + Label8 + "') = 'NG'";
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
                    string C_USER = BaseParameter.USER_ID;
                    var Label8 = BaseParameter.SearchString;

                    string sqlResult = "";
                    string sql = @"SELECT `CLIMP_TERM`   FROM   torder_bom_NOT_CLIMP";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                    sql = @"SELECT `DD`.`ORDER_IDX`, `CC`.`OR_NO`, `CC`.`WORK_WEEK`, `CC`.`LEAD_NO`, `CC`.`PROJECT`, `DD`.`TOT_QTY`, `CC`.ADJ_AF_QTY, `CC`.DT,
`DD`.`MC`, `CC`.`BUNDLE_SIZE`, `CC`.`HOOK_RACK`, `CC`.`WIRE`, `CC`.`TERM1`, `CC`.`STRIP1`, `CC`.`SEAL1`, `CC`.`CCH_W1`, 
`CC`.`ICH_W1`, `CC`.`TERM2`, `CC`.`STRIP2`, `CC`.`SEAL2`, `CC`.`CCH_W2`, `CC`.`ICH_W2`, `CC`.`SP_ST`, `CC`.`REP`, `CC`.`DSCN_YN`,
IF(IF(IF(`DD`.`PERFORMN_L` = 0,1000000, 0) >= IF(`DD`.`PERFORMN_R` =0 , 1000000,  0), IFNULL(`DD`.`PERFORMN_R`, 0), IFNULL(`DD`.`PERFORMN_L`, 0)) = 1000000, 0,IF(IF(`DD`.`PERFORMN_L` = 0,1000000, 0) >= IF(`DD`.`PERFORMN_R` =0 , 1000000,  0), IFNULL(`DD`.`PERFORMN_R`, 0), IFNULL(`DD`.`PERFORMN_L`, 0)))  AS `PERFORMN`,
`CC`.`CONDITION`, `BB`.`torder_bom_IDX`, IFNULL(`BB`.T1_TOOL_IDX, '') AS `T1_TOO_IDX`,
 
IFNULL((SELECT `SEQ` FROM ttoolmaster2 WHERE `TOOLMASTER_IDX` = `BB`.`T1_TOOL_IDX`),'') AS `T1_NM`,
IFNULL((SELECT `WK_CNT` FROM ttoolmaster2 WHERE `TOOLMASTER_IDX` = `BB`.`T1_TOOL_IDX`),0) AS `T1_CONT`, 
IFNULL(`BB`.`T2_TOOL_IDX`,'') AS `T2_TOOL_IDX`, 
IFNULL((SELECT `SEQ` FROM ttoolmaster2 WHERE `TOOLMASTER_IDX` = `BB`.`T2_TOOL_IDX`),'') AS `T2_NM`,
IFNULL((SELECT `WK_CNT` FROM ttoolmaster2 WHERE `TOOLMASTER_IDX` = `BB`.`T2_TOOL_IDX`),0) AS `T2_CONT`,`BB`.ERROR_CHK, 

`AA`.`TORDER_BARCODE_IDX`, `AA`.`TORDER_BARCODENM`, `AA`.`Barcode_SEQ`, `AA`.`TORDER_BC_PRNT`, `AA`.`TORDER_BC_WORK`,
`AA`.`DSCN_YN`, `AA`.`WORK_START`, `AA`.`WORK_END`, `AA`.`CREATE_DTM`, `AA`.`CREATE_USER`, `AA`.`UPDATE_DTM`,
`AA`.`UPDATE_USER`,`EE`.`LEAD_INDEX`, IFNULL(`EE`.`W_PN_IDX`,'') AS `W_PN_IDX`, IFNULL(`EE`.`T1_PN_IDX`,'') AS `T1_PN_IDX`,
 
IFNULL(`EE`.`S1_PN_IDX`, '') AS `S1_PN_IDX`, IFNULL(`EE`.`T2_PN_IDX`,'') AS `T2_PN_IDX`, IFNULL(`EE`.S2_PN_IDX,'') AS `S2_PN_IDX`,
`EE`.`STRIP1`, `EE`.`STRIP2`, `EE`.`CCH_W1`, `EE`.`ICH_W1`, `EE`.`CCH_W2`, `EE`.`ICH_W2`, `EE`.T1NO,
`EE`.`T2NO`, `EE`.`W_LINK`, `EE`.`WR_NO`, `EE`.`WIRE_NM`, `EE`.`W_Diameter`, `EE`.`W_Color`, 
IFNULL(`EE`.`W_Length`, 0) AS `W_Length`, (SELECT `HOOK_RACK` FROM trackmaster WHERE `LEAD_NO` = `CC`.`LEAD_NO`) AS `HOOK_RACK`,
IFNULL((SELECT `MAX_CNT` FROM TTOOLMASTER WHERE `APPLICATOR` = IF(INSTR(`CC`.`TERM1`, ')')> 0 , substring(`CC`.`TERM1`, INSTR(`CC`.`TERM1`, '(') + 1, INSTR(`CC`.`TERM1`, ')') -2 ), `CC`.`TERM1`)), 1000000)  AS `TOOL_MAX1`,
IFNULL((SELECT `MAX_CNT` FROM TTOOLMASTER WHERE `APPLICATOR` = IF(INSTR(`CC`.`TERM2`, ')')> 0 , substring(`CC`.`TERM2`, INSTR(`CC`.`TERM2`, '(') + 1, INSTR(`CC`.`TERM2`, ')') -2 ), `CC`.`TERM2`)), 1000000)  AS `TOOL_MAX2`,
`DD`.`PERFORMN_L`, `DD`.`PERFORMN_R`, 
IF(`DD`.`PERFORMN_L` > 0 , IF(`DD`.`PERFORMN_L` >= `DD`.`TOT_QTY`, 'L', 'N') , 'N') AS `LH`,
IF(`DD`.`PERFORMN_R` > 0 , IF(`DD`.`PERFORMN_R` >= `DD`.`TOT_QTY`, 'L', 'N') , 'N') AS `RH`

FROM (torder_barcode_lp `AA` INNER JOIN (torder_bom_LP `BB` INNER JOIN (TORDERLIST `CC` INNER JOIN TORDERLIST_LP `DD` ON  
`CC`.`ORDER_IDX` = `DD`.`ORDER_IDX`) ON `BB`.`ORDER_IDX` = `DD`.`ORDER_IDX`) ON `AA`.`ORDER_IDX` = `BB`.`ORDER_IDX`) INNER JOIN torder_lead_bom `EE` ON `CC`.`LEAD_NO` = `EE`.`LEAD_PN`

    WHERE `DD`.`ORDER_IDX` = '" + Label8 + "' AND `AA`.`DSCN_YN` = 'N'";
                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search = new List<SuperResultTranfer>();
                    result.Search1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                    if (result.Search.Count <= 0)
                    {
                        sql = @"SELECT `TORDER_BARCODENM`, `WORK_END` FROM torder_barcode_lp WHERE `DSCN_YN` = 'N' AND `ORDER_IDX` = '" + Label8 + "'";
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
                            sql = @"Update TORDERLIST_LP SET `CONDITION` = 'Complete', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + C_USER + "' WHERE (`ORDER_IDX` = '" + Label8 + "')";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                        else
                        {
                            IsCheck = false;
                        }
                    }
                    if (IsCheck == true)
                    {
                        var T1_CHK = result.Search[0].TERM1.IndexOf("(");
                        var T2_CHK = result.Search[0].TERM2.IndexOf("(");
                        if (T1_CHK > -1)
                        {
                            var NOT_TERM1 = result.Search[0].TERM1.Replace("(", "").Replace(")", "");
                            foreach (var item in result.DataGridView)
                            {
                                if (item.CLIMP_TERM == NOT_TERM1)
                                {
                                    sql = @"UPDATE torder_barcode_lp SET  `TORDER_BC_PRNT` ='L' WHERE  `ORDER_IDX` = '" + Label8 + "' ";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                            }
                        }

                        if (T2_CHK > -1)
                        {
                            var NOT_TERM2 = result.Search[0].TERM2.Replace("(", "").Replace(")", "");
                            foreach (var item in result.DataGridView)
                            {
                                if (item.CLIMP_TERM == NOT_TERM2)
                                {
                                    sql = @"UPDATE torder_barcode_lp SET  `TORDER_BC_WORK` ='R' WHERE  `ORDER_IDX` = '" + Label8 + "'";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> DB_COUTN(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    var MCBOX = BaseParameter.SearchString;

                    string sqlResult = "";
                    string sql = @"SELECT `MC_NO`, SUM(`WK_QTY`) AS `SUM` FROM TWWKAR_LP WHERE `MC_NO` = '" + MCBOX + "' AND `CREATE_DTM` > CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL - 1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00') AND `CREATE_DTM` < CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL + 1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00')  ";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
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
        public virtual async Task<BaseResult> OPER_TIME(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    var MCBOX = BaseParameter.SearchString;

                    string sqlResult = "";
                    string sql = @"SELECT (SUM(TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))) AS `SUM_TIME` FROM TSNON_OPER WHERE `TSNON_OPER_MCNM` = '" + MCBOX + "' AND NOT(`TSNON_OPER_CODE`= 'S') AND `CREATE_DTM` > CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL - 1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00') AND `CREATE_DTM` < CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL + 1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00')  ";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView2 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    var Label8 = BaseParameter.SearchString;
                    var CHK_LR = "";

                    string sqlResult = "";
                    if (BaseParameter.RB_LH == true)
                    {
                        CHK_LR = "L";
                    }
                    if (BaseParameter.RB_RH == true)
                    {
                        CHK_LR = "R";
                    }
                    string sql = @"SELECT `COLSIP` FROM torderinspection_lp WHERE `ORDER_IDX` = '" + Label8 + "'  AND `CHK_LR` = '" + CHK_LR + "'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView3 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> BARCODE_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        bool IsCheck = true;
                        //bool LH = BaseParameter.RB_LH ?? false;
                        //bool RH = BaseParameter.RB_RH ?? false;
                        var TB_BARCODE = BaseParameter.ListSearchString[0];
                        var DC_STR = BaseParameter.ListSearchString[1];
                        var Label8 = BaseParameter.ListSearchString[2];
                        string sqlResult = "";
                        string sql = @"SELECT `TORDER_BARCODE_IDX`, `ORDER_IDX`, `DSCN_YN`, `TORDER_BC_PRNT` as LH, `TORDER_BC_WORK` as RH FROM torder_barcode_lp 
                        WHERE `TORDER_BARCODENM` = '" + TB_BARCODE + "'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView4 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        if (result.DataGridView4.Count <= 0)
                        {
                            IsCheck = false;
                        }
                        if (IsCheck == true)
                        {
                            var DSCN_YN = result.DataGridView4[0].DSCN_YN;                     
                            if (DSCN_YN == "Y")
                            {
                                IsCheck = false;
                            }
                        }
                        if (IsCheck == true)
                        {
                            var BARCODE_QR = TB_BARCODE;
                            sql = @"Update TORDERLIST_LP SET  `PROJECT` = '" + DC_STR + "', `CONDITION` = 'Working' , `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE (`ORDER_IDX` = " + Label8 + ")";
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
        public virtual async Task<BaseResult> BARCODE_LOADSub001(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        if (BaseParameter.Search != null)
                        {
                            if (BaseParameter.Search.Count > 0)
                            {
                                var Tcount = BaseParameter.ListSearchString[0];
                                var VLA1 = BaseParameter.ListSearchString[1];
                                var TOOL1_IDX = BaseParameter.ListSearchString[2];
                                var Label4 = BaseParameter.ListSearchString[3];
                                var Label8 = BaseParameter.ListSearchString[4];
                                var MCBOX = BaseParameter.ListSearchString[5];
                                var VLT1 = BaseParameter.ListSearchString[6];
                                var DC_STR = BaseParameter.ListSearchString[7];
                                string sql = @"UPDATE `torder_barcode_lp` SET `TORDER_BC_PRNT`='L', `WORK_END`= NOW(), `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + USER_ID + "' WHERE  `TORDER_BARCODENM`= '" + Tcount + "'";
                                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                //var AAT = Tcount.Substring(Tcount.IndexOf("$$") + 2, 12);
                                //var WORK_QTY = int.Parse(AAT.Substring(0, AAT.IndexOf("$$") - 1));
                                var WORK_QTY = int.Parse(Tcount.Split("$$")[1]);
                                if (VLA1 == "")
                                {
                                }
                                else
                                {
                                    sql = @"UPDATE ttoolmaster2 SET `TOT_WK_CNT`= `TOT_WK_CNT` + " + WORK_QTY + ", `WK_CNT` = `WK_CNT` + " + WORK_QTY + ", `UPDATE_DTM`=NOW(), `UPDATE_USER`='" + USER_ID + "' WHERE `TOOLMASTER_IDX`=" + TOOL1_IDX;
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    var A_CONT = BaseParameter.Search[0].T1_CONT + WORK_QTY;
                                    sql = @"INSERT INTO TWTOOL (`TOOL_IDX`, `TOOL_WORK`, `WK_QTY`, `TOT_WK_QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES (" + TOOL1_IDX + ", '" + Label4 + "', " + WORK_QTY + ", " + A_CONT + ", NOW(), '" + USER_ID + "')";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                                var bbt_sum = BaseParameter.Search[0].PERFORMN_L + WORK_QTY;
                                sql = @"UPDATE `TORDERLIST_LP` SET `PERFORMN_L`='" + bbt_sum + "' WHERE `ORDER_IDX`=" + Label8;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"INSERT INTO TWWKAR_LP (`PART_IDX`, `WK_QTY`, `CREATE_DTM`, `CREATE_USER`, `MC_NO`, `TORDER_IDX`, `WK_TERM`, `WK_CM`)  VALUES ('" + Label4 + "', " + WORK_QTY + ", NOW(), '" + USER_ID + "', '" + MCBOX + "', '" + Label8 + "', '" + VLT1 + "', '" + DC_STR + "')";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> BARCODE_LOADSub002(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        if (BaseParameter.Search != null)
                        {
                            if (BaseParameter.Search.Count > 0)
                            {
                                var Tcount = BaseParameter.ListSearchString[0];
                                var VLA2 = BaseParameter.ListSearchString[1];
                                var TOOL2_IDX = BaseParameter.ListSearchString[2];
                                var Label4 = BaseParameter.ListSearchString[3];
                                var Label8 = BaseParameter.ListSearchString[4];
                                var MCBOX = BaseParameter.ListSearchString[5];
                                var VLT2 = BaseParameter.ListSearchString[6];
                                var DC_STR = BaseParameter.ListSearchString[7];
                                string sql = @"UPDATE `torder_barcode_lp` SET `TORDER_BC_WORK`='R',`WORK_END`= NOW(), `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + USER_ID + "' WHERE  `TORDER_BARCODENM`= '" + Tcount + "'";
                                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                //var AAT = Tcount.Substring(Tcount.IndexOf("$$") + 2, 12);
                                //var WORK_QTY = int.Parse(AAT.Substring(0, AAT.IndexOf("$$") - 1));
                                var WORK_QTY = int.Parse(Tcount.Split("$$")[1]);
                                if (VLA2 == "")
                                {
                                }
                                else
                                {
                                    sql = @"UPDATE ttoolmaster2 SET `TOT_WK_CNT`= `TOT_WK_CNT` + " + WORK_QTY + ", `WK_CNT` = `WK_CNT` + " + WORK_QTY + ", `UPDATE_DTM`=NOW(), `UPDATE_USER`='" + USER_ID + "' WHERE `TOOLMASTER_IDX`=" + TOOL2_IDX;
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    var B_CONT = BaseParameter.Search[0].T2_CONT + WORK_QTY;
                                    sql = @"INSERT INTO TWTOOL (`TOOL_IDX`, `TOOL_WORK`, `WK_QTY`, `TOT_WK_QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES (" + TOOL2_IDX + ", '" + Label4 + "', " + WORK_QTY + ", " + B_CONT + ", NOW(), '" + USER_ID + "')";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                                var bbt_sum = BaseParameter.Search[0].PERFORMN_R + WORK_QTY;
                                sql = @"UPDATE `TORDERLIST_LP` SET `PERFORMN_R`='" + bbt_sum + "' WHERE `ORDER_IDX`=" + Label8;
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"INSERT INTO TWWKAR_LP (`PART_IDX`, `WK_QTY`, `CREATE_DTM`, `CREATE_USER`, `MC_NO`, `TORDER_IDX`, `WK_TERM`, `WK_CM`)  VALUES ('" + Label4 + "', " + WORK_QTY + ", NOW(), '" + USER_ID + "', '" + MCBOX + "', '" + Label8 + "', '" + VLT2 + "', '" + DC_STR + "')";
                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"SELECT `TORDER_BARCODENM`, `WORK_END` FROM torder_barcode_lp WHERE `DSCN_YN` = 'N' AND `ORDER_IDX` = '" + Label8 + "'";
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
                                    sql = @"Update TORDERLIST_LP SET `CONDITION` = 'Complete', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE (`ORDER_IDX` = '" + Label8 + "')";
                                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> BARCODE_LOADSub003(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var Tcount = BaseParameter.ListSearchString[0];
                        var Label8 = BaseParameter.ListSearchString[1];
                        string sql = @"UPDATE `torder_barcode_lp` SET `DSCN_YN`= IF(`TORDER_BC_PRNT` ='L' AND `TORDER_BC_WORK`= 'R', 'Y', 'N'), `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `TORDER_BARCODENM`= '" + Tcount + "'";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT `TORDER_BARCODENM`, `WORK_END` FROM torder_barcode_lp WHERE `DSCN_YN` = 'N' AND `ORDER_IDX` = '" + Label8 + "'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView6 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView6.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        var COM_COUNT = result.DataGridView6.Count;
                        if (COM_COUNT == 0)
                        {
                            sql = @"Update TORDERLIST_LP SET `CONDITION` = 'Complete', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE (`ORDER_IDX` = '" + Label8 + "')";
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

