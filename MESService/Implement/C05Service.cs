using System.Numerics;

namespace MESService.Implement
{
    public class C05Service : BaseService<torderlist, ItorderlistRepository>
    , IC05Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public C05Service(ItorderlistRepository torderlistRepository

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
                        var AA = DateTime.Parse(BaseParameter.ListSearchString[0]).AddDays(-20).ToString("yyyy-MM-dd");
                        var BB = BaseParameter.ListSearchString[1];
                        var CC = "%" + BaseParameter.ListSearchString[2] + "%";
                        var DD = "%" + BaseParameter.ListSearchString[3] + "%";
                        var EEE = "%" + BaseParameter.ListSearchString[4] + "%";
                        var FFF = "%" + BaseParameter.ListSearchString[5] + "%";
                        var FAC = BaseParameter.ListSearchString[6];
                        if (DD == "%ALL%")
                        {
                            DD = "%%";
                        }

                        string sql = @"UPDATE TORDERLIST_LP SET `CONDITION` = 'Complete'  WHERE `TOT_QTY` <= `PERFORMN` AND NOT(`CONDITION`='Complete') AND DATE_FORMAT(`CREATE_DTM`, '%Y-%m-%d') > DATE_ADD(NOW(), INTERVAL -20 DAY)";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT FALSE AS `CHK`, IF(LEFT(`B`.`OR_NO`, 1)='N','','E') AS `OR_NO`, `B`.`WORK_WEEK`, `A`.`CONDITION`, `B`.`LEAD_NO`, `B`.`WIRE`, `A`.`TOT_QTY`, IF(IF(IF(`A`.`PERFORMN_L` = 0,1000000, 0) >= IF(`A`.`PERFORMN_R` =0 , 1000000,  0), IFNULL(`A`.`PERFORMN_R`, 0), IFNULL(`A`.`PERFORMN_L`, 0)) = 1000000, 0,IF(IF(`A`.`PERFORMN_L` = 0,1000000, 0) >= IF(`A`.`PERFORMN_R` =0 , 1000000,  0), IFNULL(`A`.`PERFORMN_R`, 0), IFNULL(`A`.`PERFORMN_L`, 0)))  AS `PERFORMN`, 
                        `A`.`MC`, `B`.`ADJ_AF_QTY`, `B`.`TERM1`, `B`.`SEAL1`, `B`.`TERM2`, `B`.`SEAL2`, `B`.`CCH_W1`, `B`.`ICH_W1`, `B`.`CCH_W2`, `B`.`ICH_W2`, `B`.`DT`, 
                        `C`.`LS_DATE`, `B`.`PROJECT`, `B`.`CUR_LEADS`, `B`.`CT_LEADS`, `B`.`CT_LEADS_PR`, `B`.`GRP`, `B`.`BUNDLE_SIZE`, `B`.`HOOK_RACK`, `B`.`T1_DIR`, `B`.`STRIP1`, `B`.`T2_DIR`,
                        `B`.`STRIP2`, `B`.`SP_ST`, `B`.`DSCN_YN`, `B`.`REP`, `A`.`ORDER_IDX` ,
                        (SELECT tiivtr_lead.QTY FROM tiivtr_lead WHERE tiivtr_lead.PART_IDX = (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = `B`.`LEAD_NO`) AND  tiivtr_lead.`LOC_IDX` = '3') AS `QTY_STOCK`

                        FROM  (TORDERLIST_LP `A` LEFT JOIN TORDERLIST `B` ON `B`.ORDER_IDX = `A`.ORDER_IDX) LEFT OUTER JOIN
                        (SELECT  `TORDER_IDX`, MAX(`CREATE_DTM`) AS `LS_DATE` FROM  TWWKAR_LP  GROUP BY `TORDER_IDX`) `C` ON `A`.ORDER_IDX = `C`.TORDER_IDX

                        WHERE  ((`B`.`TERM1` LIKE '" + EEE + "') OR (`B`.`TERM2` LIKE '" + EEE + "')) AND ((`B`.`SEAL1` LIKE '" + FFF + "') OR(`B`.`SEAL2` LIKE '" + FFF + "')) AND (`B`.`DSCN_YN` = 'Y') AND(`B`.`DT` >= '" + AA + " 00:00:00') AND(`B`.`LEAD_NO` LIKE '" + CC + "') AND(`A`.`CONDITION` LIKE '" + DD + "') AND(`A`.`MC`) = '" + BB + "' AND(NOT(`A`.`CONDITION` = 'Close')) AND `B`.`FCTRY_NM` = '" + FAC + "' HAVING(IF(NOT(`TERM1`) = '(899997)', IF(INSTR(`TERM1`, ')') > 0, 2, IF(NOT(`TERM2`) = '(899997)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), IF(NOT(`TERM2`) = '(899997)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), 0))), 0) + IF(NOT(`TERM1`) = '(899998)', IF(INSTR(`TERM1`, ')') > 0, 2, IF(NOT(`TERM2`) = '(899998)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), IF(NOT(`TERM2`) = '(899998)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), 0))), 0) + IF(NOT(`TERM1`) = '(899999)', IF(INSTR(`TERM1`, ')') > 0, 2, IF(NOT(`TERM2`) = '(899999)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), IF(NOT(`TERM2`) = '(899999)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), 0))), 1)) = 6 ORDER BY `A`.`CONDITION` DESC, `B`.`DT` DESC, `B`.`LEAD_NO`";

                        sql = sql + " LIMIT " + GlobalHelper.ListCount;

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
                                    var VAL = "";
                                    var AA = item.ORDER_IDX;
                                    var BB = item.MC;
                                    VAL = "UPDATE TORDERLIST_LP SET `MC`='" + BB + "' WHERE `ORDER_IDX` = " + AA;
                                    string sql = VAL;
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        var AA = DateTime.Parse(BaseParameter.ListSearchString[0]).AddDays(-20).ToString("yyyy-MM-dd");
                        var BB = BaseParameter.ListSearchString[1];
                        var CC = "%" + BaseParameter.ListSearchString[2] + "%";
                        var DD = "%" + BaseParameter.ListSearchString[3] + "%";
                        var EEE = "%" + BaseParameter.ListSearchString[4] + "%";
                        var FFF = "%" + BaseParameter.ListSearchString[5] + "%";
                        var FAC = BaseParameter.ListSearchString[6];
                        if (DD == "%ALL%")
                        {
                            DD = "%%";
                        }

                        string sql = @"UPDATE TORDERLIST_LP SET `CONDITION` = 'Complete'  WHERE `TOT_QTY` <= `PERFORMN` AND NOT(`CONDITION`='Complete') AND DATE_FORMAT(`CREATE_DTM`, '%Y-%m-%d') > DATE_ADD(NOW(), INTERVAL -20 DAY)";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT FALSE AS `CHK`, IF(LEFT(`B`.`OR_NO`, 1)='N','','E') AS `OR_NO`, `B`.`WORK_WEEK`, `A`.`CONDITION`, `B`.`LEAD_NO`, `B`.`WIRE`, `A`.`TOT_QTY`, IF(IF(IF(`A`.`PERFORMN_L` = 0,1000000, 0) >= IF(`A`.`PERFORMN_R` =0 , 1000000,  0), IFNULL(`A`.`PERFORMN_R`, 0), IFNULL(`A`.`PERFORMN_L`, 0)) = 1000000, 0,IF(IF(`A`.`PERFORMN_L` = 0,1000000, 0) >= IF(`A`.`PERFORMN_R` =0 , 1000000,  0), IFNULL(`A`.`PERFORMN_R`, 0), IFNULL(`A`.`PERFORMN_L`, 0)))  AS `PERFORMN`, 
                        `A`.`MC`, `B`.`ADJ_AF_QTY`, `B`.`TERM1`, `B`.`SEAL1`, `B`.`TERM2`, `B`.`SEAL2`, `B`.`CCH_W1`, `B`.`ICH_W1`, `B`.`CCH_W2`, `B`.`ICH_W2`, `B`.`DT`, 
                        `C`.`LS_DATE`, `B`.`PROJECT`, `B`.`CUR_LEADS`, `B`.`CT_LEADS`, `B`.`CT_LEADS_PR`, `B`.`GRP`, `B`.`BUNDLE_SIZE`, `B`.`HOOK_RACK`, `B`.`T1_DIR`, `B`.`STRIP1`, `B`.`T2_DIR`,
                        `B`.`STRIP2`, `B`.`SP_ST`, `B`.`DSCN_YN`, `B`.`REP`, `A`.`ORDER_IDX` ,
                        (SELECT tiivtr_lead.QTY FROM tiivtr_lead WHERE tiivtr_lead.PART_IDX = (SELECT torder_lead_bom.LEAD_INDEX FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = `B`.`LEAD_NO`) AND  tiivtr_lead.`LOC_IDX` = '3') AS `QTY_STOCK`

                        FROM  (TORDERLIST_LP `A` LEFT JOIN TORDERLIST `B` ON `B`.ORDER_IDX = `A`.ORDER_IDX) LEFT OUTER JOIN
                        (SELECT  `TORDER_IDX`, MAX(`CREATE_DTM`) AS `LS_DATE` FROM  TWWKAR_LP  GROUP BY `TORDER_IDX`) `C` ON `A`.ORDER_IDX = `C`.TORDER_IDX

                        WHERE  ((`B`.`TERM1` LIKE '" + EEE + "') OR (`B`.`TERM2` LIKE '" + EEE + "')) AND ((`B`.`SEAL1` LIKE '" + FFF + "') OR(`B`.`SEAL2` LIKE '" + FFF + "')) AND (`B`.`DSCN_YN` = 'Y') AND(`B`.`DT` >= '" + AA + " 00:00:00') AND(`B`.`LEAD_NO` LIKE '" + CC + "') AND(`A`.`CONDITION` LIKE '" + DD + "') AND(`A`.`MC`) = '" + BB + "' AND(NOT(`A`.`CONDITION` = 'Close')) AND `B`.`FCTRY_NM` = '" + FAC + "' HAVING(IF(NOT(`TERM1`) = '(899997)', IF(INSTR(`TERM1`, ')') > 0, 2, IF(NOT(`TERM2`) = '(899997)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), IF(NOT(`TERM2`) = '(899997)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), 0))), 0) + IF(NOT(`TERM1`) = '(899998)', IF(INSTR(`TERM1`, ')') > 0, 2, IF(NOT(`TERM2`) = '(899998)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), IF(NOT(`TERM2`) = '(899998)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), 0))), 0) + IF(NOT(`TERM1`) = '(899999)', IF(INSTR(`TERM1`, ')') > 0, 2, IF(NOT(`TERM2`) = '(899999)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), IF(NOT(`TERM2`) = '(899999)', IF(INSTR(`TERM2`, ')') > 0, 2, 0), 0))), 1)) = 6 ORDER BY `A`.`CONDITION` DESC, `B`.`DT` DESC, `B`.`LEAD_NO`";



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
                    var FAC = BaseParameter.SearchString;
                    string sql = @"SELECT DISTINCT `MC` FROM TORDERLIST_LP WHERE  (NOT (`CONDITION` = 'Close')) AND DATE_FORMAT(`CREATE_DTM`, '%Y-%m-%d') > DATE_SUB(NOW(),INTERVAL 20 DAY)
AND (SELECT TORDERLIST.`ORDER_IDX` FROM TORDERLIST WHERE TORDERLIST.FCTRY_NM = '" + FAC + "' AND TORDERLIST.`ORDER_IDX` = TORDERLIST_LP.ORDER_IDX)";
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


       /// <summary>
       /// đong đơn nếu vượt quá 20 ngày
       /// </summary>
       /// <param name="BaseParameter"></param>
       /// <returns></returns>
        public virtual async Task<BaseResult> DB_LISECHK(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"UPDATE TORDERLIST_LP SET `CONDITION`='Close' WHERE DATE_FORMAT(`CREATE_DTM`, '%Y-%m-%d') < DATE_ADD(NOW(), INTERVAL -20 DAY) AND NOT (TORDERLIST_LP.`CONDITION` = 'Complete')";
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
                    string C_USER = BaseParameter.USER_IDX;
                    var MCBOX = BaseParameter.SearchString;
                    string sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCBOX + "' AND `TS_TIME_ST` > DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d') AND  `TS_USER` = '" + C_USER + "'";
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
                        sql = @"INSERT INTO `TUSER_LOG` (`TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER`) VALUES ('" + MCBOX + "', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), NOW(), CONCAT(DATE_ADD(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), INTERVAL +1 DAY), ' 05:49:59'), '" + C_USER + "')";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCBOX + "' AND `TS_TIME_ST` > DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d') AND  `TS_USER` = '" + C_USER + "'";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, MIN(`TS_TIME_ST`) AS `DATEMIN`, MAX(`TS_TIME_END`) AS `DATEMAX` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCBOX + "' AND `TS_DATE` = DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d')";
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
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        bool IsCheck = true;
                        var BC_TEXT = BaseParameter.ListSearchString[0];        
                        
                        string sql = @"SELECT `ORDER_IDX` FROM TORDERLIST_LP WHERE `ORDER_IDX` = (SELECT `ORDER_IDX` FROM TORDER_BARCODE WHERE `TORDER_BARCODENM` = '" + BC_TEXT + "')";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        if (result.DataGridView2.Count > 0)
                        {
                            //sql = @"SELECT `CONDITION`, `ORDER_IDX`, 
                            //(SELECT `LEAD_NO` FROM TORDERLIST WHERE `ORDER_IDX` = TORDERLIST_LP.`ORDER_IDX`) AS `LEAD_NO`,
                            //IFNULL((SELECT `PROJECT` FROM TORDERLIST WHERE `ORDER_IDX` = TORDERLIST_LP.`ORDER_IDX`), '') AS `PROJECT`, 
                            //IFNULL(`TOT_QTY`, 0) AS `TOT_QTY`,  
                            //IFNULL((SELECT `ADJ_AF_QTY` FROM TORDERLIST WHERE `ORDER_IDX` = TORDERLIST_LP.`ORDER_IDX`), '') AS `ADJ_AF_QTY`, 
                            //(SELECT `DT` FROM TORDERLIST WHERE `ORDER_IDX` = TORDERLIST_LP.`ORDER_IDX`) AS `DT`,
                            //IFNULL((SELECT `BUNDLE_SIZE` FROM TORDERLIST WHERE `ORDER_IDX` = TORDERLIST_LP.`ORDER_IDX`), 0) AS `BUNDLE_SIZE`, 
                            //IF(IFNULL(`PERFORMN_L`, 0) >= IFNULL(`PERFORMN_R`, 0), IFNULL(`PERFORMN_R`, 0), IFNULL(`PERFORMN_L`, 0)) AS `PERFORMN`,  
                            //(SELECT `WIRE` FROM TORDERLIST WHERE `ORDER_IDX` = TORDERLIST_LP.`ORDER_IDX`) AS `WIRE`,
                            //IFNULL(LEFT((SELECT `SP_ST` FROM TORDERLIST WHERE `ORDER_IDX` = TORDERLIST_LP.`ORDER_IDX`), 256), '') AS `SP_ST`, 
                            //IFNULL((SELECT `REP` FROM TORDERLIST WHERE `ORDER_IDX` = TORDERLIST_LP.`ORDER_IDX`), '') AS `REP`, 
                            //IFNULL(`MC`, '') AS `MC`
                            //FROM TORDERLIST_LP  
                            //WHERE TORDERLIST_LP.`ORDER_IDX` = (SELECT `ORDER_IDX` FROM TORDER_BARCODE WHERE `TORDER_BARCODENM` = '" + BC_TEXT + "')";

                            sql = @"SELECT 
     	                                LP.`CONDITION`,
 		                                LP.`ORDER_IDX`,
		                                IFNULL(LP.`TOT_QTY`, 0) AS `TOT_QTY`,
		                                IFNULL(LP.`PERFORMN_L`, 0) AS PERFORMN_L, IFNULL(LP.`PERFORMN_R`, 0) AS PERFORMN_R,
 		                                O.`LEAD_NO`,
 		                                IFNULL(O.`PROJECT`, '') AS `PROJECT`,
 		                                IFNULL(O.`ADJ_AF_QTY`, '') AS `ADJ_AF_QTY`,
 		                                O.`DT`,
 		                                IFNULL(O.`BUNDLE_SIZE`, 0) AS `BUNDLE_SIZE`,
		                                 IF(
		                                     IFNULL(LP.`PERFORMN_L`, 0) >= IFNULL(LP.`PERFORMN_R`, 0),
		                                     IFNULL(LP.`PERFORMN_R`, 0),
		                                     IFNULL(LP.`PERFORMN_L`, 0)
		                                 ) AS `PERFORMN`,
 		                                O.`WIRE`,
 		                                IFNULL(LEFT(O.`SP_ST`, 256), '') AS `SP_ST`,
 		                                IFNULL(O.`REP`, '') AS `REP`,
 		                                IFNULL(LP.`MC`, '') AS `MC`,
 		                                O.TERM1, O.SEAL1, O.TERM2, O.SEAL2, O.CCH_W1, O.ICH_W1, O.CCH_W2, O.ICH_W2, O.FCTRY_NM
                                    FROM TORDERLIST_LP LP
                                    JOIN TORDERLIST O 
                                        ON LP.`ORDER_IDX` = O.`ORDER_IDX`
                                    WHERE LP.`ORDER_IDX` = (
                                        SELECT `ORDER_IDX` 
                                        FROM TORDER_BARCODE 
                                        WHERE `TORDER_BARCODENM` = '" + BC_TEXT + "' );";

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView3 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            if (result.DataGridView3.Count <= 0)
                            {
                                IsCheck = false;
                            }
                            if (IsCheck == true)
                            {
                                var CHKA = result.DataGridView3[0].CONDITION;
                                var TotalOrder = (int)(result.DataGridView3[0].TOT_QTY ?? 0);
                                var TotalL = (int)(result.DataGridView3[0].PERFORMN_L ?? 0);
                                var TotalR = (int)(result.DataGridView3[0].PERFORMN_R ?? 0);
                                var Term1LP = result.DataGridView3[0].TERM1 ?? "";
                                var Term2LP = result.DataGridView3[0].TERM2 ?? "";

                                CHKA = GetStatus(TotalOrder, TotalL, TotalR, Term1LP, Term2LP);

                                if (CHKA == "Complete")
                                {
                                    IsCheck = false;
                                }
                                if (IsCheck == true)
                                {
                                    result.DataGridView3[0].CONDITION = CHKA;

                                    var FFF = result.DataGridView3[0].ORDER_IDX;

                                    sql = @"SELECT `torder_bom_IDX`, `ORDER_IDX`, `CREATE_DTM`, `ERROR_CHK` FROM torder_bom_LP WHERE  (`ORDER_IDX` = '" + FFF + "')";
                                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    result.DataGridView4 = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }

                                    if (result.DataGridView4.Count <= 0)
                                    {
                                        sql = @"INSERT INTO torder_bom_LP (`ORDER_IDX`,  `ERROR_CHK`,  `CREATE_DTM`) SELECT `ORDER_IDX`,  'N', `CREATE_DTM`  FROM  TORDERLIST_LP  WHERE  `ORDER_IDX` = '" + FFF + "'";
                                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        sql = @"SELECT `torder_bom_IDX`, `ORDER_IDX`, `CREATE_DTM`, `ERROR_CHK` FROM torder_bom_LP WHERE  (`ORDER_IDX` = '" + FFF + "')";
                                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                        result.DataGridView4 = new List<SuperResultTranfer>();
                                        for (int i = 0; i < ds.Tables.Count; i++)
                                        {
                                            DataTable dt = ds.Tables[i];
                                            result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                        }
                                    }
                                    var BARNM = BC_TEXT;

                                    sql = @"SELECT  `TORDER_BARCODE_IDX`, TORDER_BARCODENM, ORDER_IDX FROM torder_barcode_lp WHERE `TORDER_BARCODENM` = '" + BARNM + "'";
                                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    result.DataGridView5 = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        result.DataGridView5.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }
                                    var BARCODE_CNT = result.DataGridView5.Count;
                                    if (BARCODE_CNT <= 0)
                                    {
                                        var ORDER_IDX = result.DataGridView3[0].ORDER_IDX;
                                        sql = @"INSERT INTO torder_barcode_lp (`TORDER_BARCODENM`, `ORDER_IDX`, `Barcode_SEQ`, `TORDER_BC_PRNT`, `TORDER_BC_WORK`, `DSCN_YN`,`CREATE_DTM`, `CREATE_USER`, `WORK_START`) SELECT `TORDER_BARCODENM`, `ORDER_IDX`, `Barcode_SEQ`, 'Y', 'Y', 'N', NOW(), '" + USER + "', NOW() FROM TORDER_BARCODE WHERE `ORDER_IDX` = '" + ORDER_IDX + "' AND TORDER_BARCODE.DSCN_YN = 'Y' ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        sql = @"ALTER TABLE     `torder_barcode_lp`     AUTO_INCREMENT= 1";
                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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

        /// <summary>
        /// thục hiện kiêm tra điều kiện mã dây có thực hiện LP hay không và theo số lượng thục tế đã àm để biết đơn đã hoàn thành hay không
        /// </summary>
        /// <param name="TotalOrder"></param>
        /// <param name="TotalL"></param>
        /// <param name="TotalR"></param>
        /// <param name="Term1"></param>
        /// <param name="Term2"></param>
        /// <returns></returns>
        private string GetStatus(int TotalOrder, int TotalL, int TotalR, string Term1, string Term2)
        {
            // true nếu cần làm đầu trái/phải
            bool Term1LP = Term1.Contains("(") && Term1.Contains(")");
            bool Term2LP = Term2.Contains("(") && Term2.Contains(")");

            // Nếu không cần làm đầu nào → Close
            if (!Term1LP && !Term2LP)
            {
                return "Close";
            }

            // Nếu cần làm cả 2 đầu
            if (Term1LP && Term2LP)
            {
                //nếu số lượng đã làm của 2 đầu đều đủ rồi => đơn hoàn thành, ngược lại đang working
                if (TotalL >= TotalOrder && TotalR >= TotalOrder)
                    return "Complete";
                else
                    return "Working";
            }

            // Nếu chỉ làm đầu trái
            if (Term1LP && !Term2LP)
            {
                //trả về kết quả nếu số lượng đầu bên trái đã đủ thì đơn hoàn thành, ngược lại là đang làm việc
                return (TotalL >= TotalOrder) ? "Complete" : "Working";
            }

            // Nếu chỉ làm đầu phải
            if (!Term1LP && Term2LP)
            {
                //trả về kết quả nếu số lượng đầu phải đã đủ thì đơn hoàn thành, ngược lại là đang làm việc
                return (TotalR >= TotalOrder) ? "Complete" : "Working";
            }

            return "Stay"; // fallback
        }

    }
}

