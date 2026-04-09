namespace MESService.Implement
{
    public class C02Service : BaseService<torderlist, ItorderlistRepository>
    , IC02Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        
        public C02Service(ItorderlistRepository torderlistRepository
        

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
                    if (BaseParameter.ListSearchString != null)
                    {
                        string sql = @"UPDATE TORDERLIST SET `CONDITION` = 'Complete', `PERFORMN` = `TOT_QTY`  WHERE `TOT_QTY` <= `PERFORMN` AND NOT(`CONDITION`='Complete') AND `DT` > DATE_ADD(NOW(), INTERVAL -11 DAY)";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        var date = DateTime.Parse(BaseParameter.ListSearchString[0]).AddDays(-11).ToString("yyyy-MM-dd");
                        var MC = BaseParameter.ListSearchString[1];
                        var LedNo = "%" + BaseParameter.ListSearchString[2] + "%";
                        var CONDITION = "%" + BaseParameter.ListSearchString[3] + "%";
                        var term = "%" + BaseParameter.ListSearchString[4] + "%";
                        var seal = "%" + BaseParameter.ListSearchString[5] + "%";

                        var MCbox = BaseParameter.ListSearchString[6];

                        if (CONDITION == "%ALL%")
                        {
                            CONDITION = "%%";
                        }

                        var MC_WH_SQL = "";
                        var MC_CHKE = "";

                        MC_CHKE = MCbox.Substring(0, 1);
                        if (MCbox == "DJG")
                        {
                        }
                        else
                        {
                            switch (MC_CHKE)
                            {
                                case "_":
                                    MC_WH_SQL = "  AND TORDERLIST.FCTRY_NM = ''  ";
                                    break;
                                case "A":
                                    MC_WH_SQL = "  AND TORDERLIST.FCTRY_NM = 'Factory 1'  ";
                                    break;
                                case "Z":
                                    MC_WH_SQL = "  AND TORDERLIST.FCTRY_NM = 'Factory 2'  ";
                                    break;
                            }
                        }

                        //sql = @"SELECT 'FALSE' AS `CHK`, 
                        //IF(SUM(IF(LEFT(`OR_NO`, 1)= 'N', 0, 1)) > 0, 'E', '') AS `OR_NO`, 
                        //`WORK_WEEK`, 
                        //`CONDITION`, 
                        //MIN(`CREATE_DTM`) AS `CREATE_DTM`, 
                        //`LEAD_NO`, 
                        //`WIRE`,  
                        //IFNULL(SUM(`TOEXCEL_QTY`), 0) AS `TOEXCEL_QTY`, 
                        //IFNULL(`B`.`QTY_STOCK`, 0) AS `QTY_STOCK`, 
                        //SUM(`TOT_QTY`) AS `SUM_QTY`, 
                        //(TOEXCEL_QTY - QTY_STOCK ) AS REM_QTY,
                        //SUM(`PERFORMN`) AS `ACT`, 
                        //IFNULL(`MC2`, `MC`) AS `MC`, 
                        //`ADJ_AF_QTY`, 
                        //`TERM1`, 
                        //`SEAL1`, 
                        //`TERM2`, 
                        //`SEAL2`, 
                        //`CCH_W1`, 
                        //`ICH_W1`, 
                        //`CCH_W2`, 
                        //`ICH_W2`, 
                        //`DT`, 
                        //derivedtbl_1.`LS_DATE`, 
                        //`PROJECT`,
                        //`CUR_LEADS`, 
                        //`CT_LEADS`, 
                        //`CT_LEADS_PR`, 
                        //`GRP`, 
                        //`BUNDLE_SIZE`, 
                        //`HOOK_RACK`, 
                        //`T1_DIR`, 
                        //`STRIP1`, 
                        //`T2_DIR`, 
                        //`STRIP2`, 
                        //`SP_ST`, 
                        //`REP`
                        //FROM  (TORDERLIST LEFT OUTER JOIN
                        //(SELECT  TORDER_IDX, MAX(CREATE_DTM) AS LS_DATE  FROM   TWWKAR   GROUP BY TORDER_IDX) derivedtbl_1 ON TORDERLIST.ORDER_IDX = derivedtbl_1.TORDER_IDX)
                        //LEFT OUTER JOIN   (SELECT `LEAD_PN`, `QTY` AS `QTY_STOCK` FROM  torder_lead_bom LEFT OUTER JOIN tiivtr_lead ON LEAD_INDEX = PART_IDX  AND  tiivtr_lead.`LOC_IDX` = '3' ) AS `B` ON `B`.LEAD_PN = TORDERLIST.LEAD_NO                   
                        //WHERE  ((TORDERLIST.TERM1 LIKE '" + term + "') OR (TORDERLIST.TERM2 LIKE '" + term + "')) AND ((TORDERLIST.SEAL1 LIKE '" + seal + "') OR (TORDERLIST.SEAL2 LIKE '" + seal + "')) AND (TORDERLIST.DSCN_YN = 'Y') AND (TORDERLIST.DT >= '" + date + " 00:00:00') AND (TORDERLIST.LEAD_NO LIKE '" + LedNo + "') AND (TORDERLIST.`CONDITION` LIKE '" + CONDITION + "') AND (IFNULL(TORDERLIST.MC2, TORDERLIST.MC) = '" + MC + "') AND (NOT(TORDERLIST.`CONDITION` = 'Close')) " + MC_WH_SQL + " GROUP BY TORDERLIST.`LEAD_NO`, `OR_NO` ORDER BY  `CONDITION` DESC,  `LEAD_NO`,  `CREATE_DTM`   ";

                        sql = @"SELECT * FROM 
                        (SELECT 'FALSE' AS `CHK`, 
                        IF(SUM(IF(LEFT(`OR_NO`, 1)= 'N', 0, 1)) > 0, 'E', '') AS `OR_NO`, 
                        `WORK_WEEK`, 
                            CASE WHEN SUM( CASE   WHEN PERFORMN > 0  AND PERFORMN < TOT_QTY  THEN 1 ELSE 0   END  ) > 0 THEN 'Working'   
                              WHEN SUM( CASE  WHEN IFNULL(PERFORMN,0) = 0  THEN 1 ELSE 0  END ) > 0  THEN 'Stay'
                              WHEN SUM(CASE  WHEN PERFORMN >= TOT_QTY   THEN 1 ELSE 0  END  ) = COUNT(*)   THEN 'Complete'
                              ELSE 'Stay' 
                            END AS `CONDITION`, 
                        MIN(`CREATE_DTM`) AS `CREATE_DTM`, `LEAD_NO`, `WIRE`,  
                        IFNULL(SUM(`TOEXCEL_QTY`), 0) AS `TOEXCEL_QTY`, 
                        IFNULL(`B`.`QTY_STOCK`, 0) AS `QTY_STOCK`, 
                        SUM(`TOT_QTY`) AS `SUM_QTY`, 
                        (TOEXCEL_QTY - QTY_STOCK ) AS REM_QTY,
                        SUM(`PERFORMN`) AS `ACT`, 
                        IFNULL(`MC2`, `MC`) AS `MC`, `ADJ_AF_QTY`, `TERM1`, `SEAL1`, `TERM2`, `SEAL2`, `CCH_W1`, `ICH_W1`, `CCH_W2`, `ICH_W2`, `DT`, 
                        derivedtbl_1.`LS_DATE`, `PROJECT`,`CUR_LEADS`, `CT_LEADS`, `CT_LEADS_PR`, `GRP`, `BUNDLE_SIZE`, `HOOK_RACK`, `T1_DIR`, `STRIP1`, 
                        `T2_DIR`, `STRIP2`, `SP_ST`, `REP`
                        FROM  (TORDERLIST LEFT OUTER JOIN
                        (SELECT  TORDER_IDX, MAX(CREATE_DTM) AS LS_DATE FROM twwkar GROUP BY TORDER_IDX) derivedtbl_1 ON TORDERLIST.ORDER_IDX = derivedtbl_1.TORDER_IDX)
                        LEFT OUTER JOIN (SELECT `LEAD_PN`, `QTY` AS `QTY_STOCK` FROM  torder_lead_bom LEFT OUTER JOIN tiivtr_lead ON LEAD_INDEX = PART_IDX  AND  tiivtr_lead.`LOC_IDX` = '3' ) AS `B` ON `B`.LEAD_PN = TORDERLIST.LEAD_NO                   
                        WHERE  (TORDERLIST.DSCN_YN = 'Y') AND (TORDERLIST.DT >= '" + date + @" 00:00:00')  and ( TORDERLIST.PERFORMN < TORDERLIST.TOT_QTY or TORDERLIST.PERFORMN is null ) 
                        AND (IFNULL(TORDERLIST.MC2, TORDERLIST.MC) = '" + MC + @"') AND (NOT(TORDERLIST.`CONDITION` = 'Close')) " + MC_WH_SQL + @"   
                        GROUP BY TORDERLIST.`LEAD_NO`, `OR_NO` ORDER BY  `CONDITION` DESC,  `LEAD_NO`,  `CREATE_DTM` ) AS tb
                        WHERE ((tb.TERM1 LIKE '" + term + "') OR (tb.TERM2 LIKE '" + term + "')) AND ((tb.SEAL1 LIKE '" + seal + "') OR (tb.SEAL2 LIKE '" + seal + @"')) 
                        AND (tb.LEAD_NO LIKE '" + LedNo + "') AND (tb.`CONDITION` LIKE '" + CONDITION + "') ";

                        sql = sql + @"union SELECT * FROM 
                        (SELECT 'FALSE' AS `CHK`, 
                        IF(SUM(IF(LEFT(`OR_NO`, 1)= 'N', 0, 1)) > 0, 'E', '') AS `OR_NO`, 
                        `WORK_WEEK`, 
                            CASE WHEN SUM( CASE   WHEN PERFORMN > 0  AND PERFORMN < TOT_QTY  THEN 1 ELSE 0   END  ) > 0 THEN 'Working'   
                              WHEN SUM( CASE  WHEN IFNULL(PERFORMN,0) = 0  THEN 1 ELSE 0  END ) > 0  THEN 'Stay'
                              WHEN SUM(CASE  WHEN PERFORMN >= TOT_QTY   THEN 1 ELSE 0  END  ) = COUNT(*)   THEN 'Complete'
                              ELSE 'Stay' 
                            END AS `CONDITION`, 
                        MIN(`CREATE_DTM`) AS `CREATE_DTM`, `LEAD_NO`, `WIRE`,  
                        IFNULL(SUM(`TOEXCEL_QTY`), 0) AS `TOEXCEL_QTY`, 
                        IFNULL(`B`.`QTY_STOCK`, 0) AS `QTY_STOCK`, 
                        SUM(`TOT_QTY`) AS `SUM_QTY`, 
                        (TOEXCEL_QTY - QTY_STOCK ) AS REM_QTY,
                        SUM(`PERFORMN`) AS `ACT`, 
                        IFNULL(`MC2`, `MC`) AS `MC`, `ADJ_AF_QTY`, `TERM1`, `SEAL1`, `TERM2`, `SEAL2`, `CCH_W1`, `ICH_W1`, `CCH_W2`, `ICH_W2`, `DT`, 
                        derivedtbl_1.`LS_DATE`, `PROJECT`,`CUR_LEADS`, `CT_LEADS`, `CT_LEADS_PR`, `GRP`, `BUNDLE_SIZE`, `HOOK_RACK`, `T1_DIR`, `STRIP1`, 
                        `T2_DIR`, `STRIP2`, `SP_ST`, `REP`
                        FROM  (TORDERLIST LEFT OUTER JOIN
                        (SELECT  TORDER_IDX, MAX(CREATE_DTM) AS LS_DATE FROM twwkar GROUP BY TORDER_IDX) derivedtbl_1 ON TORDERLIST.ORDER_IDX = derivedtbl_1.TORDER_IDX)
                        LEFT OUTER JOIN (SELECT `LEAD_PN`, `QTY` AS `QTY_STOCK` FROM  torder_lead_bom LEFT OUTER JOIN tiivtr_lead ON LEAD_INDEX = PART_IDX  AND  tiivtr_lead.`LOC_IDX` = '3' ) AS `B` ON `B`.LEAD_PN = TORDERLIST.LEAD_NO                   
                        WHERE  (TORDERLIST.DSCN_YN = 'Y') AND (TORDERLIST.DT >= '" + date + @" 00:00:00')  and ( TORDERLIST.PERFORMN >= TORDERLIST.TOT_QTY ) 
                        AND (IFNULL(TORDERLIST.MC2, TORDERLIST.MC) = '" + MC + @"') AND (NOT(TORDERLIST.`CONDITION` = 'Close')) " + MC_WH_SQL + @"   
                        GROUP BY TORDERLIST.`LEAD_NO`, `OR_NO` ORDER BY  `CONDITION` DESC,  `LEAD_NO`,  `CREATE_DTM` ) AS tb
                        WHERE ((tb.TERM1 LIKE '" + term + "') OR (tb.TERM2 LIKE '" + term + "')) AND ((tb.SEAL1 LIKE '" + seal + "') OR (tb.SEAL2 LIKE '" + seal + @"')) 
                        AND (tb.LEAD_NO LIKE '" + LedNo + "') AND (tb.`CONDITION` LIKE '" + CONDITION + "') ";

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
                if (BaseParameter != null)
                {
                    if (BaseParameter.DataGridView1 != null)
                    {
                        if (BaseParameter.DataGridView1.Count > 0)
                        {
                            foreach (var item in BaseParameter.DataGridView1)
                            {
                                if (item.CHK == true)
                                {
                                    var AA = item.LEAD_NO;
                                    var AAA = item.CREATE_DTM.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                    var BB = item.MC;
                                    string sql = "UPDATE TORDERLIST SET `MC2`='" + BB + "' WHERE `LEAD_NO` = '" + AA + "' AND  `CREATE_DTM` = '" + AAA + "'  ";
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
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter?.DataGridView1 != null && BaseParameter.DataGridView1.Count > 0)
                {
                    var C_USER = BaseParameter.USER_ID;
                    var stayItems = BaseParameter.DataGridView1
                                        .Where(x => x.CONDITION == "Stay" || x.CONDITION == "Working")
                                        .ToList();

                    if (stayItems.Any())
                    {
                        // Gom danh sách thành (LEAD_NO, CREATE_DTM)
                        var conditions = stayItems
                            .Select(x => $"('{x.LEAD_NO}','{x.CREATE_DTM:yyyy-MM-dd HH:mm:ss}')");

                        string inClause = string.Join(",", conditions);

                        string sql = $@"
                    UPDATE TORDERLIST
                    SET `CONDITION`='Close',
                        `UPDATE_DTM`=NOW(),
                        `UPDATE_USER`=@User
                    WHERE NOT(`CONDITION`='Complete')
                    AND (LEAD_NO, CREATE_DTM) IN ({inClause});
                ";

                        var parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@User", C_USER)
                        };

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql, parameters);

                        result.Success = true;
                        result.Message = $"{stayItems.Count} order(s) updated.";
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
        public virtual async Task<BaseResult> MC_LIST(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var MC_WH_SQL = "";
                    var C_USER = BaseParameter.USER_ID;
                    var MCbox = BaseParameter.SearchString;
                    var MC_CHKE = MCbox.Substring(0, 1);
                    if (MCbox == "DJG")
                    {
                    }
                    else
                    {
                        switch (MC_CHKE)
                        {
                            case "-":
                                MC_WH_SQL = "  AND TORDERLIST.FCTRY_NM = ''  ";
                                break;
                            case "A":
                                MC_WH_SQL = "  AND TORDERLIST.FCTRY_NM = 'Factory 1'  ";
                                break;
                            case "Z":
                                MC_WH_SQL = "  AND TORDERLIST.FCTRY_NM = 'Factory 2'  ";
                                break;
                        }
                    }
                    string sql = @"SELECT DISTINCT IFNULL(MC2, MC) AS MC FROM TORDERLIST WHERE  (DSCN_YN = 'Y') AND (NOT (`CONDITION` = 'Close')) AND `DT` > DATE_SUB(NOW(),INTERVAL 12 DAY) " + MC_WH_SQL + "   GROUP BY IFNULL(MC2, MC)  ";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.ComboBox1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                string sql = @"UPDATE TORDERLIST SET `CONDITION`='Close',  `UPDATE_DTM` = NOW(),  `UPDATE_USER` = 'MES'  WHERE `DT` < DATE_ADD(NOW(), INTERVAL -11 DAY) AND NOT (TORDERLIST.`CONDITION` = 'Complete')  AND NOT (`CONDITION` = 'Close') ";

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
                    string sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER` FROM TUSER_LOG 
                    WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_TIME_ST` > DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d') AND  `TS_USER` = '" + C_USER + "'";

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
                        sql = @"INSERT INTO `TUSER_LOG` (`TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER`) VALUES 
                        ('" + MCbox + "', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), NOW(), CONCAT(DATE_ADD(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), INTERVAL +1 DAY), ' 05:49:59'), '" + C_USER + "')";

                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER` FROM TUSER_LOG 
                        WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_TIME_ST` > DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d') AND  `TS_USER` = '" + C_USER + "'";

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
                        var DATEMIN = result.USER_TIME1[0].DATEMIN;
                        result.USER_TIME1[0].Name = DATEMIN.Value.ToString("HH:mm:ss");
                        result.USER_TIME1[0].Description = DATEMIN.Value.ToString("yyyy-MM-dd HH:mm:ss");
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

                    //kiêm tra trạng thái downtime có hay không

                    string sql = @"SELECT * FROM tsnon_oper WHERE  tsnon_oper.TSNON_OPER_DATE = '" + DateTime.Now.ToString("yyyy-MM-dd") +
                                    "' AND tsnon_oper.TSNON_OPER_MCNM = '" + MCbox +
                                   "' AND tsnon_oper.TSNON_OPER_ETIME IS NULL";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Listtsnon_oper = new List<tsnon_oper>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Listtsnon_oper.AddRange(SQLHelper.ToList<tsnon_oper>(dt));
                    }

                    if (result.Listtsnon_oper.Count > 0)
                    {
                        //trẻ về cho client hiện giao diện báo downtime
                        return result;
                    }
                    else
                    {
                        sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`, `StopCode`) VALUES 
                                ('" + MCbox + "', '-----', 'N','-')  ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = '-----', `tsnon_oper_mitor_RUNYN` = 'N'";

                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

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
    }
}

