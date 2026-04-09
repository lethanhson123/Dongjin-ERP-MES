

namespace MESService.Implement
{
    public class C11Service : BaseService<torderlist, ItorderlistRepository>
    , IC11Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public C11Service(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> TS_USER(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string C_USER = BaseParameter.USER_ID;
                    var MCbox = BaseParameter.SearchString;
                    string sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_TIME_ST` > DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d') AND  `TS_USER` = '" + C_USER + "'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    var USER_COUNT = result.Search.Count;
                    if (USER_COUNT <= 0)
                    {
                        sql = @"INSERT INTO `TUSER_LOG` (`TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER`) VALUES ('" + MCbox + "', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), NOW(), CONCAT(DATE_ADD(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), INTERVAL +1 DAY), ' 05:49:59'), '" + C_USER + "')";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_TIME_ST` > DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d') AND  `TS_USER` = '" + C_USER + "'";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, MIN(`TS_TIME_ST`) AS `DATEMIN`, MAX(`TS_TIME_END`) AS `MAX` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_DATE` = DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d')";
                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    if (result.Search1.Count > 0)
                    {
                        var S_DATE = result.Search1[0].DATEMIN;
                        result.Search1[0].Name = S_DATE.Value.ToString("HH:mm:ss");
                        result.Search1[0].Description = S_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss");
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
                    string C_USER = BaseParameter.USER_ID;
                    var MCbox = BaseParameter.SearchString;
                    string sql = @"SELECT `MC_NO`, SUM(`WK_QTY`) AS `SUM` FROM TWWKAR_LP WHERE `MC_NO` = '" + MCbox + "' AND `CREATE_DTM` > CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL - 1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00') AND `CREATE_DTM` < CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL + 1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00')  ";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    string C_USER = BaseParameter.USER_ID;
                    var MCbox = BaseParameter.SearchString;
                    string sql = @"SELECT (SUM(TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))) AS `SUM_TIME` FROM TSNON_OPER WHERE `TSNON_OPER_MCNM` = '" + MCbox + "' AND NOT(`TSNON_OPER_CODE`='S') AND `CREATE_DTM` > CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL - 1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00') AND `CREATE_DTM` < CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL + 1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00')";
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
        public virtual async Task<BaseResult> Barcod_read(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    bool IsCheck = true;
                    string C_USER = BaseParameter.USER_ID;
                    var Barcodebox = BaseParameter.SearchString;
                    string sqlResult = "";
                    string sql = @"SELECT 
                    `A`.`TORDER_BARCODE_IDX`, `A`.`TORDER_BARCODENM`, `A`.`ORDER_IDX`, `A`.`Barcode_SEQ`, `A`.`DSCN_YN`, `A`.`WORK_END`, 
                    (SELECT IFNULL(`MC2`, `MC`) FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = `A`.ORDER_IDX) AS `MC_CHK`,
                    (SELECT IFNULL(`TOT_QTY`, 0) FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = `A`.ORDER_IDX) AS `TOT_QTY`,
                    (SELECT IFNULL(`BUNDLE_SIZE`, 0) FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = `A`.ORDER_IDX) AS `BUND`,
                    (SELECT `WIRE` FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = `A`.ORDER_IDX) AS `WIRE`
                    FROM TORDER_BARCODE `A` WHERE `A`.TORDER_BARCODENM = '" + Barcodebox + "' AND(SELECT IFNULL(`MC2`, `MC`) FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = `A`.ORDER_IDX) = 'SHIELD WIRE' AND `A`.`DSCN_YN` = 'Y'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView12 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView12.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    try
                    {
                        result.ORDER_IDX = result.DataGridView12[0].ORDER_IDX;
                    }
                    catch (Exception ex)
                    {
                        IsCheck = false;
                        result.Error = ex.Message;
                    }

                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Barcod_readSub(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    bool IsCheck = true;
                    string C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var Barcodebox = BaseParameter.ListSearchString[0];
                        var BBB = BaseParameter.ListSearchString[1];
                        var ERROR_J = BaseParameter.ListSearchString[2];
                        var ERROR_L = BaseParameter.ListSearchString[3];
                        var ERROR_J2 = BaseParameter.ListSearchString[4];
                        var ERROR_R = BaseParameter.ListSearchString[5];                        
                        var Label48 = BaseParameter.ListSearchString[6];

                        string sqlResult = "";
                        string sql = @"SELECT 
                    `A`.`TORDER_BARCODE_IDX`, `A`.`TORDER_BARCODENM`, `A`.`ORDER_IDX`, `A`.`Barcode_SEQ`, `A`.`DSCN_YN`, `A`.`WORK_END`, 
                    (SELECT IFNULL(`MC2`, `MC`) FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = `A`.ORDER_IDX) AS `MC_CHK`,
                    (SELECT IFNULL(`TOT_QTY`, 0) FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = `A`.ORDER_IDX) AS `TOT_QTY`,
                    (SELECT IFNULL(`BUNDLE_SIZE`, 0) FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = `A`.ORDER_IDX) AS `BUND`,
                    (SELECT `WIRE` FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = `A`.ORDER_IDX) AS `WIRE`
                    FROM TORDER_BARCODE `A` WHERE `A`.TORDER_BARCODENM = '" + Barcodebox + "' AND(SELECT IFNULL(`MC2`, `MC`) FROM TORDERLIST WHERE TORDERLIST.ORDER_IDX = `A`.ORDER_IDX) = 'SHIELD WIRE' AND `A`.`DSCN_YN` = 'Y'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        try
                        {
                            result.ORDER_IDX = result.DataGridView2[0].ORDER_IDX;
                        }
                        catch (Exception ex)
                        {
                            IsCheck = false;
                            result.Error = ex.Message;
                        }
                        if (IsCheck == true)
                        {
                            if (result.DataGridView2.Count <= 0)
                            {
                                IsCheck = false;
                            }
                            if (IsCheck == true)
                            {
                                sql = @"SELECT IF(INSTR(`B`.`LEAD_PN`, '#') > 0, 'JOINT', 'LP') AS `Work`, `B`.`LEAD_PN`, `B`.`BUNDLE_SIZE`,
                                IFNULL((SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `B`.`W_PN_IDX`), '') AS `WIRE_PARTNO`,
                                IFNULL((SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `B`.`T1_PN_IDX`), '') AS `T1_PARTNO`,
                                IFNULL((SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `B`.`S1_PN_IDX`), '') AS `S1_PARTNO`,
                                IFNULL((SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `B`.`T2_PN_IDX`), '') AS `T2_PARTNO`,
                                IFNULL((SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `B`.`S2_PN_IDX`), '') AS `S2_PARTNO`,
                                `B`.`STRIP1`, `B`.`STRIP2`, `B`.`CCH_W1`, `B`.`ICH_W1`, `B`.`CCH_W2`, `B`.`ICH_W2`, `B`.`T1NO`, `B`.`T2NO`, `B`.`W_LINK`, `B`.`WR_NO`,
                                `B`.`WIRE_NM`, `B`.`W_Diameter`, `B`.`W_Color`, `B`.`W_Length`, `B`.`LEAD_INDEX`, 
                                IFNULL(`B`.`W_PN_IDX`, 0) AS `W_PN_IDX`, 
                                IFNULL(`B`.`T1_PN_IDX`, 0) AS `T1_PN_IDX`, 
                                IFNULL(`B`.`S1_PN_IDX`, 0) AS `S1_PN_IDX`,  
                                IFNULL(`B`.`T2_PN_IDX`, 0) AS `T2_PN_IDX`,  
                                IFNULL(`B`.`S2_PN_IDX`, 0) AS `S2_PN_IDX`

                                FROM torder_lead_bom_spst `A` JOIN torder_lead_bom `B` ON `B`.`LEAD_INDEX` = `A`.`S_PART_IDX`
                                WHERE `A`.`M_PART_IDX` = (SELECT `LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = '" + BBB + "')  ORDER BY `Work` DESC";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.SW_BOM = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.SW_BOM.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (result.SW_BOM.Count <= 0)
                                {
                                    IsCheck = false;
                                }
                                if (IsCheck == true)
                                {
                                    var TERM1_CONT = 0;
                                    var TERM2_CONT = 0;
                                    var SEAL1_CONT = 0;
                                    var SEAL2_CONT = 0;
                                    foreach (var item in result.SW_BOM)
                                    {
                                        if (item.T1_PARTNO == "")
                                        {
                                            TERM1_CONT = TERM1_CONT;
                                        }
                                        else
                                        {
                                            if (item.T1_PARTNO == "899997")
                                            {
                                                TERM1_CONT = TERM1_CONT;
                                            }
                                            else
                                            {
                                                if (item.T1_PARTNO == "899998")
                                                {
                                                    TERM1_CONT = TERM1_CONT;
                                                }
                                                else
                                                {
                                                    if (item.T1_PARTNO == "899999")
                                                    {
                                                        TERM1_CONT = TERM1_CONT;
                                                    }
                                                    else
                                                    {
                                                        if (item.Work == "JOINT")
                                                        {
                                                            if (ERROR_J == "A")
                                                            {
                                                                sql = @"INSERT INTO `torder_bom_sw` (`ORDER_IDX`, `LOC_LRJ`, `PERFORMN`, `DSCN_YN`, `ERROR_CHK`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + result.ORDER_IDX + "', 'J', '0', 'N', 'N', NOW(), '" + C_USER + "') ON DUPLICATE KEY UPDATE  `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                                                ERROR_J = "N";
                                                            }
                                                            TERM1_CONT = TERM1_CONT + 1;
                                                        }
                                                        else
                                                        {
                                                            if (ERROR_L == "A")
                                                            {
                                                                sql = @"INSERT INTO `torder_bom_sw` (`ORDER_IDX`, `LOC_LRJ`, `PERFORMN`, `DSCN_YN`, `ERROR_CHK`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + result.ORDER_IDX + "', 'L', '0', 'N', 'N', NOW(), '" + C_USER + "') ON DUPLICATE KEY UPDATE  `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                                                ERROR_L = "N";
                                                            }
                                                            TERM1_CONT = TERM1_CONT + 1;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (item.T2_PARTNO == "")
                                        {
                                            TERM2_CONT = TERM2_CONT;
                                        }
                                        else
                                        {
                                            if (item.T2_PARTNO == "899997")
                                            {
                                                TERM2_CONT = TERM2_CONT;
                                            }
                                            else
                                            {
                                                if (item.T2_PARTNO == "899998")
                                                {
                                                    TERM2_CONT = TERM2_CONT;
                                                }
                                                else
                                                {
                                                    if (item.T2_PARTNO == "899999")
                                                    {
                                                        TERM2_CONT = TERM2_CONT;
                                                    }
                                                    else
                                                    {
                                                        if (item.Work == "JOINT")
                                                        {
                                                            if (ERROR_J2 == "A")
                                                            {
                                                                sql = @"INSERT INTO `torder_bom_sw` (`ORDER_IDX`, `LOC_LRJ`, `PERFORMN`, `DSCN_YN`, `ERROR_CHK`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + result.ORDER_IDX + "', 'J2', '0', 'N', 'N', NOW(), '" + C_USER + "') ON DUPLICATE KEY UPDATE  `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                                                ERROR_J2 = "N";
                                                            }
                                                            TERM2_CONT = TERM2_CONT + 1;
                                                        }
                                                        else
                                                        {
                                                            if (ERROR_R == "A")
                                                            {
                                                                sql = @"INSERT INTO `torder_bom_sw` (`ORDER_IDX`, `LOC_LRJ`, `PERFORMN`, `DSCN_YN`, `ERROR_CHK`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + result.ORDER_IDX + "', 'R', '0', 'N', 'N', NOW(), '" + C_USER + "') ON DUPLICATE KEY UPDATE  `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                                                ERROR_R = "N";
                                                            }
                                                            TERM2_CONT = TERM2_CONT + 1;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    var MIN = 999999;
                                    for (var i = 0; i < 4; i++)
                                    {
                                        if (MIN > BaseParameter.C_MIN[i])
                                        {
                                            MIN = BaseParameter.C_MIN[i];
                                        }
                                    }
                                    sql = @"SELECT `CONDITION`   FROM   TORDERLIST_SW   WHERE   `ORDER_IDX`='" + result.ORDER_IDX + "'";
                                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    result.DataGridView3 = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }
                                    var OR_LIST = "";
                                    if (result.DataGridView3.Count <= 0)
                                    {
                                        sql = @"INSERT INTO `TORDERLIST_SW` (`ORDER_IDX`, `MC`, `TOT_QTY`, `PERFORMN`, `CONDITION`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + result.ORDER_IDX + "', 'SW', '" + Label48 + "', " + MIN + ", 'STAY', NOW(), '" + C_USER + "')  ON DUPLICATE KEY UPDATE  `CONDITION` = 'Working',  `PERFORMN` = VALUES(`PERFORMN`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                    else
                                    {
                                        OR_LIST = result.DataGridView3[0].CONDITION;
                                    }
                                    if (OR_LIST == "Complete")
                                    {
                                        IsCheck = false;
                                    }
                                    if (IsCheck == true)
                                    {
                                        var Label48Number = int.Parse(Label48);
                                        if (Label48Number > 0)
                                        {
                                            if (Label48Number > MIN)
                                            {
                                                sql = @"UPDATE `TORDERLIST_SW` SET `CONDITION`='Complete'   WHERE  `ORDER_IDX`= '" + result.ORDER_IDX + "'";
                                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                            }
                                        }
                                    }
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
        public virtual async Task<BaseResult> BOM_CHK(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    bool IsCheck = true;
                    string C_USER = BaseParameter.USER_ID;
                    var ORDER_IDX = BaseParameter.SearchString;
                    string sqlResult = "";
                    string sql = @"SELECT `ORDER_IDX`, `LOC_LRJ`, IFNULL(`PERFORMN`, 0) AS `PERFORMN` , `DSCN_YN`, IFNULL(`T1_TOOL_IDX`, 0) AS `T1_TOOL_IDX`, `ERROR_CHK`  
                        FROM torder_bom_sw `A` WHERE `ORDER_IDX` = '" + ORDER_IDX + "'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView4 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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

