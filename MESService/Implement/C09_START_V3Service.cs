namespace MESService.Implement
{
    public class C09_START_V3Service : BaseService<torderlist, ItorderlistRepository>
    , IC09_START_V3Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C09_START_V3Service(ItorderlistRepository torderlistRepository
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
        public virtual async Task<BaseResult> ORDER_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var ORDER_NO_TEXT = BaseParameter.SearchString;
                    var MCname = BaseParameter.MC_NAME;
                    string sql = @"SELECT 
                    `A`.`ORDER_IDX`, 
                    `A`.`OR_NO`, 
                    `A`.`WORK_WEEK`, 
                    `A`.`PO_DT`, 
                    `A`.`LEAD_NO`, 
                    `A`.`PO_QTY`, 
                    `A`.`SAFTY_QTY`, 
                    `A`.`MC`, 
                    `A`.`BUNDLE_SIZE`,
                    `A`.`PERFORMN`, 
                    `A`.`CONDITION`, 
                    `A`.`LEAD_COUNT`, 
                    `A`.`ERROR_YN`, 
                    '' AS `TORDER_BARCODENM`, 
                    '' AS `Barcode_SEQ`, 
                    (SELECT `LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = `A`.`LEAD_NO`) AS `LEAD_IDX`,
                    (SELECT `HOOK_RACK` FROM trackmaster WHERE trackmaster.`LEAD_NO` = `A`.`LEAD_NO`) AS `HOOK_RACK`,
                    (SELECT torder_lead_bom.`W_Length` FROM torder_lead_bom WHERE torder_lead_bom.LEAD_PN = `A`.`LEAD_NO`) AS `W_Length`
                    FROM TORDERLIST_SPST `A`
                    WHERE `A`.`DSCN_YN` ='Y'  AND `A`.`ORDER_IDX` ='" + ORDER_NO_TEXT + "'";


                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DGV_C09_ST_00 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DGV_C09_ST_00.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    var PERFORMN = result.DGV_C09_ST_00[0].PERFORMN;

                    //thục hiện truy vấn thông tin chi tiết của mã dây SPST
                    sql = @"SELECT   
                    `A`.`OR_NO`, 
                    `A`.`LEAD_NO` AS `M_LEAD`, 
                    `B`.LEAD_NO, 
                    `A`.`PO_QTY`, 
                    `B`.`B_SIZE`, 
                    (`A`.`PO_QTY` / `B`.`B_SIZE`) AS `SCAN_CNT`, 
                    `B`.`S_LR`,
                    IFNULL(`C`.`BARCODE_QTY`, 0) AS `B_QTY`, `C`.`B_COUNT`, `C`.`SUM`,
                    IFNULL(`C`.`USE_QTY`, 0) AS `US_QTY`, 
                    IFNULL(`C`.`DSCN_YN`, 'N') AS `DC_YN`, 
                    `A`.`ERROR_YN`, 
                    (IFNULL(`C`.`SUM`, 0) - IFNULL(`C`.`USE_QTY`, 0)) AS `S_SUM`,
                    IF(`A`.`BUNDLE_SIZE`<= (IFNULL(`C`.`SUM`, 0) - IFNULL(`C`.`USE_QTY`, 0)), 'Y', 'N') AS `E_CHK`
                    FROM (TORDERLIST_SPST `A` LEFT JOIN 
                    (SELECT  (SELECT `LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`M_PART_IDX`) AS `MLEAD_NO`,
                     (SELECT `LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`) AS `LEAD_NO`, `S_LR`, (SELECT `BUNDLE_SIZE` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`) AS `B_SIZE`
                    FROM   torder_lead_bom_spst)  `B` ON `A`.`LEAD_NO` = `B`.`MLEAD_NO`)  LEFT JOIN 
                    (SELECT  `TORDER_BARCODENM`,  COUNT(`TORDER_BARCODENM`) `B_COUNT`, 
                    `BARCODE_QTY`,  SUM(BARCODE_QTY) AS `SUM`,  SUM(`USE_QTY`) AS `USE_QTY`, `DSCN_YN`, `TORDER_SPSTORDER_IDX`
                    FROM torder_bom_spst2 JOIN torder_bom_spst1 ON torder_bom_spst2.`torder_bomSPST_IDX` = torder_bom_spst1.`torder_bomSPST_IDX`
                    WHERE torder_bom_spst2.`TORDER_SPSTORDER_IDX` = '" + ORDER_NO_TEXT + "' " +
                    " AND torder_bom_spst1.MC ='" + MCname + "' AND torder_bom_spst1.DSCN_YN = 'N'" +
                    " GROUP BY SUBSTRING_INDEX(`TORDER_BARCODENM`, '$$', 1)) `C` ON `A`.`ORDER_IDX` = `C`.`TORDER_SPSTORDER_IDX` AND `B`.`LEAD_NO` = SUBSTRING_INDEX(`C`.`TORDER_BARCODENM`, '$$', 1) WHERE `A`.`ORDER_IDX` = '" + ORDER_NO_TEXT + "'  ";


                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DGV_C09_ST_BOM = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DGV_C09_ST_BOM.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    var MCbox = BaseParameter.SearchString;
                    string sql = @"SELECT (SUM(TIMESTAMPDIFF(SECOND, TSNON_OPER_STIME, TSNON_OPER_ETIME))) AS `SUM_TIME` FROM TSNON_OPER WHERE `TSNON_OPER_MCNM` = '" + MCbox + "' AND NOT(`TSNON_OPER_CODE`='S') AND `CREATE_DTM` > CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL - 1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00') AND `CREATE_DTM` < CONCAT(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL + 1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), '%Y-%m-%d'), ' 06:00:00')";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    var ORDER_NO_TEXT = BaseParameter.SearchString;
                    string sql = @"SELECT `COLSIP` FROM torderinspection_spst    WHERE    `ORDER_IDX` = '" + ORDER_NO_TEXT + "'";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Barcodebox_KeyDown_1(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    var TEXT_BARCODE = BaseParameter.SearchString;
                    TEXT_BARCODE = TEXT_BARCODE.ToUpper();
                    string sql = @"SELECT * FROM torder_bom_spst1 WHERE torder_bom_spst1.TORDER_BARCODENM = '" + TEXT_BARCODE + "'";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DGV_C09_13 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DGV_C09_13.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Barcodebox_KeyDown_1Sub01(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    string MC_name = BaseParameter.MC_NAME;

                    if (BaseParameter.ListSearchString != null)
                    {
                        var TEXT_BARCODE = BaseParameter.ListSearchString[0];
                        var ORDER_NO_TEXT = BaseParameter.ListSearchString[1];
                        var BC_QTY = BaseParameter.ListSearchString[2];
                        var BAR_NM_CHK = bool.Parse(BaseParameter.ListSearchString[3]);
                        string sql = "";
                        string sqlRessult = "";

                        if (BAR_NM_CHK == false)
                        {
                            sql = @"INSERT INTO `torder_bom_spst1` (`TORDER_BARCODENM`,`MC`, `BARCODE_QTY`, `USE_QTY`, `DSCN_YN`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + TEXT_BARCODE + "','" + MC_name + "', '" + BC_QTY + "', '0', 'N', NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                            sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                        else
                        {
                            sql = @"UPDATE `torder_bom_spst1` SET `MC`='" + MC_name + "', `UPDATE_USER`='" + USER_ID + "', `UPDATE_DTM`=NOW()  WHERE `TORDER_BARCODENM`='" + TEXT_BARCODE + "'";
                            sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }

                        sql = @"INSERT INTO `torder_bom_spst2` (`torder_bomSPST_IDX`, `TORDER_SPSTORDER_IDX`, `CREATE_DTM`, `CREATE_USER`)  
                               VALUES ((SELECT torder_bomSPST_IDX FROM  torder_bom_spst1 WHERE  TORDER_BARCODENM = '" + TEXT_BARCODE + "'), '" + ORDER_NO_TEXT + "', NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                        sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `torder_bom_spst1`     AUTO_INCREMENT= 1";
                        sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `torder_bom_spst2`     AUTO_INCREMENT= 1";
                        sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Barcodebox_KeyDown_1Sub02(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    string MC_name = BaseParameter.MC_NAME;
                    if (BaseParameter.ListSearchString != null)
                    {

                        var TEXT_BARCODE = BaseParameter.ListSearchString[0];
                        var ORDER_NO_TEXT = BaseParameter.ListSearchString[1];
                        var BC_QTY = BaseParameter.ListSearchString[2];
                        var BAR_NM_CHK = bool.Parse(BaseParameter.ListSearchString[3]);
                        string sql = "";
                        string sqlRessult = "";

                        if (BAR_NM_CHK == false)
                        {
                            sql = @"INSERT INTO `torder_bom_spst1` (`TORDER_BARCODENM`,`MC`,  `BARCODE_QTY`, `USE_QTY`, `DSCN_YN`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + TEXT_BARCODE + "', '" + MC_name + "', '" + BC_QTY + "', '0', 'N', NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                            sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }

                        sql = @"INSERT INTO `torder_bom_spst2` (`torder_bomSPST_IDX`, `TORDER_SPSTORDER_IDX`, `CREATE_DTM`, `CREATE_USER`)   VALUES ((SELECT torder_bomSPST_IDX FROM  torder_bom_spst1 WHERE  TORDER_BARCODENM = '" + TEXT_BARCODE + "'), '" + ORDER_NO_TEXT + "', NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                        sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `torder_bom_spst1`     AUTO_INCREMENT= 1";
                        sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `torder_bom_spst2`     AUTO_INCREMENT= 1";
                        sqlRessult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                    string USER_ID = BaseParameter.USER_ID;
                    string MC_name = BaseParameter.MC_NAME;

                    if (BaseParameter.ListSearchString != null)
                    {
                        if (BaseParameter.DGV_C09_ST_00 != null)
                        {
                            if (BaseParameter.DGV_C09_ST_00.Count > 0)
                            {
                                if (BaseParameter.DGV_C09_ST_BOM != null)
                                {
                                    if (BaseParameter.DGV_C09_ST_BOM.Count > 0)
                                    {
                                        var Label7 = BaseParameter.ListSearchString[0] == null ? "0" : BaseParameter.ListSearchString[0];
                                        var Label59 = BaseParameter.ListSearchString[1] == null ? "0" : BaseParameter.ListSearchString[1];
                                        var ORDER_NO_TEXT = BaseParameter.ListSearchString[2];
                                        var partbox = BaseParameter.ListSearchString[3];
                                        var MCbox = BaseParameter.ListSearchString[4];
                                        var Label48 = BaseParameter.ListSearchString[5] == null ? "0" : BaseParameter.ListSearchString[5];

                                        var WORK_QTY = int.Parse(Label7) + int.Parse(Label59);

                                        string sql = @"UPDATE   torder_bom_spst1 JOIN (SELECT  `A1`.`TORDER_BARCODENM`, `A1`.`BARCODE_QTY`, `A1`.`USE_QTY`, `A1`.`torder_bomSPST_IDX`, `A2`.`TORDER_SPSTORDER_IDX`,  
                                                    IF((SUM(`A1`.`BARCODE_QTY`) OVER(PARTITION  BY SUBSTRING_INDEX(`A1`.`TORDER_BARCODENM`, '$$', 1) ORDER BY `A1`.`torder_bomSPST_IDX`) -  '" + WORK_QTY + "') <=0 ,`BARCODE_QTY`, " +
                                                    " `A1`.`BARCODE_QTY` -(SUM(`A1`.`BARCODE_QTY`) OVER(PARTITION  BY SUBSTRING_INDEX(`A1`.`TORDER_BARCODENM`, '$$', 1) ORDER BY `A1`.`torder_bomSPST_IDX`) - '" + WORK_QTY + "'))  AS `CM_SUM`, " +
                                                    " IF((SUM(`A1`.`BARCODE_QTY`) OVER(PARTITION  BY SUBSTRING_INDEX(`A1`.`TORDER_BARCODENM`, '$$', 1) ORDER BY `A1`.`torder_bomSPST_IDX`) - '" + WORK_QTY + "') <= 0 ,'Y', 'N') AS `DSCN_YN` " +
                                                    " FROM torder_bom_spst2 `A2`   JOIN torder_bom_spst1 `A1`  ON   `A1`.`torder_bomSPST_IDX` = `A2`.`torder_bomSPST_IDX` " +
                                                    " WHERE `A2`.`TORDER_SPSTORDER_IDX` = '" + ORDER_NO_TEXT + "'  AND A1.MC = '" + MC_name + "'    ORDER BY `A1`.`torder_bomSPST_IDX`) `M2` " +
                                                    " ON torder_bom_spst1.`torder_bomSPST_IDX` = `M2`.`torder_bomSPST_IDX` " +
                                                    " SET torder_bom_spst1.`USE_QTY` = `M2`.`CM_SUM`, torder_bom_spst1.DSCN_YN = `M2`.`DSCN_YN`    WHERE  `M2`.`TORDER_SPSTORDER_IDX` = '" + ORDER_NO_TEXT + "'  ";
                                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        sql = @"INSERT INTO TWWKAR_SPST (`PART_IDX`, `WK_QTY`, `CREATE_DTM`, `CREATE_USER`, `MC_NO`, `TORDER_IDX`) VALUES ('" + partbox + "', " + Label7 + ", NOW(), '" + USER_ID + "', '" + MCbox + "', '" + ORDER_NO_TEXT + "')";
                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        var BARCODE_QR_SEQ = (decimal.Parse(Label7) + decimal.Parse(Label59)) / (decimal)BaseParameter.DGV_C09_ST_00[0].BUNDLE_SIZE;
                                        var BARCODE_QR = partbox + "$$" + BaseParameter.DGV_C09_ST_00[0].BUNDLE_SIZE + "$$" + DateTime.Now.ToString("yyyyMMdd") + "_" + ORDER_NO_TEXT.Trim() + MC_name.Trim() + "$$" + BaseParameter.DGV_C09_ST_00[0].PO_QTY + "$$" + BARCODE_QR_SEQ;
                                        BaseParameter.Code = BARCODE_QR;

                                        sql = @"INSERT INTO TORDER_BARCODE_SP (`TORDER_BARCODENM`, `ORDER_IDX`, `Barcode_SEQ`, `TORDER_BC_PRNT`, `TORDER_BC_WORK`, `DSCN_YN`,`CREATE_DTM`, `CREATE_USER`, `WORK_END`)   
                                                VALUES  ('" + BARCODE_QR + "', " + ORDER_NO_TEXT + ", " + BARCODE_QR_SEQ + ", 'Y', 'Y', 'Y',  NOW(), '" + USER_ID + "', NOW())";
                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        //chạy cập nhật đung số lượng đã làm cho đơn spst
                                        sql = @"UPDATE TORDERLIST_SPST AS a JOIN (
                                                    SELECT COUNT(torder_barcode_sp.TORDER_BARCODE_IDX) AS TotalLotCode 
                                                    FROM torder_barcode_sp 
                                                    WHERE torder_barcode_sp.ORDER_IDX = '" + ORDER_NO_TEXT + "' ) b " +
                                                    @"SET 
                                                        a.PERFORMN = a.BUNDLE_SIZE * b.TotalLotCode,
                                                        a.`CONDITION` = IF((a.BUNDLE_SIZE * b.TotalLotCode) >= a.PO_QTY, 'Complete', 'Working'),
                                                         a.UPDATE_DTM = NOW(), a.UPDATE_USER = '" + USER_ID + "' " +
                                                    "WHERE a.ORDER_IDX = '" + ORDER_NO_TEXT + "';";

                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        //if (WORK_QTY >= int.Parse(Label48))
                                        //{
                                        //    sql = @"Update  TORDERLIST_SPST   SET `CONDITION` = 'Complete',  `PERFORMN`='" + WORK_QTY + "', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "'  WHERE (`ORDER_IDX` = '" + ORDER_NO_TEXT + "')";
                                        // sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                        //}
                                        //else
                                        //{
                                        //    sql = @"Update  TORDERLIST_SPST   SET `CONDITION` = 'Working',  `PERFORMN`='" + WORK_QTY + "', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "'  WHERE (`ORDER_IDX` = '" + ORDER_NO_TEXT + "')";
                                        //    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                        //}

                                        result.Code = await PrintDocument1_PrintPage(BaseParameter);
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
        public virtual async Task<string> PrintDocument1_PrintPage(BaseParameter BaseParameter)
        {
            string SheetName = this.GetType().Name;
            string result = "";
            var BARCODE_DATE = GlobalHelper.InitializationDateTime;
            var PR1 = "";
            var PR2 = BARCODE_DATE.ToString("yyyy-MM-dd");
            var PR3 = BaseParameter.USER_ID;
            var PR4 = BaseParameter.DGV_C09_ST_00[0].HOOK_RACK;
            if (PR4.Length > 10)
            {
                PR4 = PR4.Substring(0, 10);
            }
            var PR5 = BaseParameter.DGV_C09_ST_00[0].LEAD_NO;
            var PR7 = BaseParameter.DGV_C09_ST_00[0].PO_QTY;
            var PR8 = BaseParameter.DGV_C09_ST_00[0].BUNDLE_SIZE;
            var Label7 = BaseParameter.ListSearchString[0] == null ? "0" : BaseParameter.ListSearchString[0];
            var Label59 = BaseParameter.ListSearchString[1] == null ? "0" : BaseParameter.ListSearchString[1];
            var PR20 = (decimal.Parse(Label7) + decimal.Parse(Label59)) / (decimal)BaseParameter.DGV_C09_ST_00[0].BUNDLE_SIZE;
            var PR23 = BaseParameter.DGV_C09_ST_00[0].SAFTY_QTY;
            var PR24 = "";
            if (BaseParameter.DGV_C09_ST_00[0].OR_NO == "OR_NO")
            {
                PR24 = "E";
            }
            var LEAD_BOM = new List<string>();
            for (int i = 0; i < 30; i++)
            {
                if (i < BaseParameter.DGV_C09_ST_BOM.Count)
                {
                    LEAD_BOM.Add(BaseParameter.DGV_C09_ST_BOM[i].LEAD_NO);
                }
                else
                {
                    LEAD_BOM.Add("");
                }
            }

            string sql = @"SELECT 
            'After Length' AS `M_NAME`,
            (SELECT torder_lead_bom.`LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`M_PART_IDX`) AS `M_PART`,
            MAX((SELECT torder_lead_bom.`W_Length` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`M_PART_IDX`)) AS `M_W_Length`,
            'Before Length' AS `S_NAME`,
            (SELECT torder_lead_bom.`LEAD_PN` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`) AS `S_PART`,
            MAX((SELECT torder_lead_bom.`W_Length` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_INDEX` = torder_lead_bom_spst.`S_PART_IDX`)) AS `S_W_Length`
 
            FROM  torder_lead_bom_spst
            WHERE  
            torder_lead_bom_spst.`M_PART_IDX` = (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = '" + PR5 + "')   ";
            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            var DGV_C09_88 = new List<SuperResultTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                DGV_C09_88.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }
            var M_LEN = "";
            var S_LEN = "";
            if (DGV_C09_88.Count > 0)
            {
                M_LEN = DGV_C09_88[0].M_NAME + " :" + DGV_C09_88[0].M_W_Length;
                S_LEN = DGV_C09_88[0].S_NAME + " :" + DGV_C09_88[0].S_W_Length;
            }
            StringBuilder HTMLContent = new StringBuilder();
            HTMLContent.AppendLine(GlobalHelper.CreateHTMLC09_REPRINT(SheetName, _WebHostEnvironment.WebRootPath, BaseParameter.Code, PR1, PR2, PR3, PR4, PR5, PR7.ToString(), PR8.ToString(), PR20.ToString(), PR23.ToString(), PR24, M_LEN, S_LEN, LEAD_BOM));
            result = GlobalHelper.CreateHTMLClose(SheetName, _WebHostEnvironment.WebRootPath, HTMLContent.ToString());
            return result;
        }
    }
}

