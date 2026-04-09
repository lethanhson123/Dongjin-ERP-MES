namespace MESService.Implement
{
    public class D04Service : BaseService<torderlist, ItorderlistRepository>
    , ID04Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public D04Service(ItorderlistRepository torderlistRepository
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
                    if (BaseParameter.Action == 1)
                    {

                    }
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var ADATE = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd");
                            var BDATE = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd");
                            var LPARKNO = "%" + BaseParameter.ListSearchString[2] + "%";
                            var SERIERID = "%" + BaseParameter.ListSearchString[3] + "%";
                            var LPOCODE = "%" + BaseParameter.ListSearchString[4] + "%";
                            var LPNTNO = "%" + BaseParameter.ListSearchString[5] + "%";
                            var PART_NM = "%" + BaseParameter.ListSearchString[6] + "%";
                            var RO_BC = "%" + BaseParameter.ListSearchString[7] + "%";
                            var BC_DSCN_YN = BaseParameter.ListSearchString[8];

                            string sql = @"SELECT FALSE AS `CHK`, 
                            (SELECT `PO_CODE` FROM tdpdotpl WHERE PDOTPL_IDX= tdpdmtim.PDOTPL_IDX) AS `PO_CODE`,
                            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_NO`,
                            (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_NAME`,
                            (SELECT `PART_FML` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_FML`,
                            IFNULL((SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)), '') AS `PART_STAGE`,
                            (`VLID_PART_SNP`) AS `PART_SNP`,
                            (SELECT `PO_QTY` FROM tdpdotpl WHERE PDOTPL_IDX = tdpdmtim.`PDOTPL_IDX`) AS PO_QTY,
                            IFNULL((SELECT `PLET_NO` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), '') AS `Pallet_NO`,
                            IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), '') AS `PALLET_CODE`,
                            `VLID_GRP` AS `SERIAL_ID`, 
                            VLID_DTM AS `INPUT_DATE`,
                            VLID_BARCODE AS `Good_BRCODE`, 
                            VLID_REMARK, 
                            UPDATE_DTM AS `SHIPPING_DATE` ,
                            VLID_DSCN_YN,
                            (SELECT `QTY` FROM tiivtr WHERE (tiivtr.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AND  LOC_IDX = '2') AS `STOCK`, 
                            `PDMTIN_IDX`,
                            IFNULL((SELECT `DONE_YN` FROM tdpdotpl WHERE PDOTPL_IDX= tdpdmtim.PDOTPL_IDX), 'N') AS `DONE_YN`
                            FROM tdpdmtim  ";

                            //string sql = "";
                            string CHK_SQLTEXT = "";
                            if (BC_DSCN_YN == "Y")
                            {
                                //Nguyên mẫu
                                CHK_SQLTEXT = @"WHERE (SELECT `PO_CODE` FROM tdpdotpl WHERE PDOTPL_IDX= tdpdmtim.PDOTPL_IDX) LIKE '" + LPOCODE + "' AND (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) LIKE  '" + LPARKNO + "' AND(SELECT `PLET_NO` FROM tdpdotplmu WHERE(`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)) LIKE '" + LPNTNO + "' AND VLID_DSCN_YN = 'Y' AND(tdpdmtim.UPDATE_DTM >= '" + ADATE + "') AND(tdpdmtim.UPDATE_DTM <= '" + BDATE + "') AND `VLID_GRP` LIKE '" + SERIERID + "' HAVING `PART_NAME` LIKE '" + PART_NM + "' AND `Good_BRCODE` LIKE '" + RO_BC + "' ORDER BY tdpdmtim.VLID_DTM DESC, (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AND NOT(`TDPDOTPLMU_IDX` IS NULL) AND NOT(`PDOTPL_IDX` IS NULL)";

                                //Cải tiến
                                //CHK_SQLTEXT = @"WHERE (SELECT `PO_CODE` FROM tdpdotpl WHERE PDOTPL_IDX= tdpdmtim.PDOTPL_IDX) LIKE '" + LPOCODE + "' AND (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) LIKE  '" + LPARKNO + "' AND(SELECT `PLET_NO` FROM tdpdotplmu WHERE(`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)) LIKE '" + LPNTNO + "' AND VLID_DSCN_YN = 'Y' AND(tdpdmtim.UPDATE_DTM >= '" + ADATE + "') AND(tdpdmtim.UPDATE_DTM <= '" + BDATE + "') AND `VLID_GRP` LIKE '" + SERIERID + "' HAVING `PART_NAME` LIKE '" + PART_NM + "' AND `Good_BRCODE` LIKE '" + RO_BC + "' ORDER BY tdpdmtim.VLID_DTM DESC, (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`))";

                                //Cải tiến
                                //sql = @"SELECT mtb.* from
                                //(SELECT 
                                //    FALSE AS `CHK`,
                                //    dp.PO_CODE AS `PO_CODE`,
                                //    sp.PART_NO AS `PART_NO`,
                                //    sp.PART_NM AS `PART_NAME`,
                                //    sp.PART_FML AS `PART_FML`,
                                //    IFNULL(sp.PART_CAR, '') AS `PART_STAGE`,
                                //    tm.VLID_PART_SNP AS `PART_SNP`,
                                //    dp.PO_QTY AS `PO_QTY`,
                                //    IFNULL(pm.PLET_NO, '') AS `Pallet_NO`,
                                //    IFNULL(pm.PLET_COMS, '') AS `PALLET_CODE`,
                                //    tm.VLID_GRP AS `SERIAL_ID`,
                                //    tm.VLID_DTM AS `INPUT_DATE`,
                                //    tm.VLID_BARCODE AS `Good_BRCODE`,
                                //    tm.VLID_REMARK,
                                //    tm.UPDATE_DTM AS `SHIPPING_DATE`,
                                //    tm.VLID_DSCN_YN,
                                //    iv.QTY AS `STOCK`,
                                //    tm.PDMTIN_IDX,
                                //    IFNULL(dp.DONE_YN, 'N') AS `DONE_YN`
                                //FROM tdpdmtim tm
                                //LEFT JOIN tdpdotpl dp ON dp.PDOTPL_IDX = tm.PDOTPL_IDX
                                //LEFT JOIN tspart sp ON sp.PART_IDX = tm.VLID_PART_IDX
                                //LEFT JOIN tdpdotplmu pm ON pm.TDPDOTPLMU_IDX = tm.TDPDOTPLMU_IDX
                                //LEFT JOIN tiivtr iv ON iv.PART_IDX = tm.VLID_PART_IDX AND iv.LOC_IDX = '2'
                                //WHERE tm.UPDATE_DTM BETWEEN '" + ADATE + "' AND '" + BDATE + "' AND tm.VLID_GRP LIKE '" + SERIERID + @"') AS mtb WHERE mtb.PO_CODE LIKE '" + LPOCODE + @"' AND mtb.PART_NO LIKE '" + LPARKNO + @"' AND mtb.Pallet_NO LIKE '" + LPNTNO + @"' AND mtb.VLID_DSCN_YN = 'Y' AND mtb.PART_NAME LIKE '" + PART_NM + @"' AND mtb.Good_BRCODE LIKE '" + RO_BC + @"' ORDER BY mtb.INPUT_DATE DESC, mtb.PART_NO";
                            }
                            else
                            {
                                //Nguyên mẫu
                                CHK_SQLTEXT = @"WHERE VLID_DSCN_YN='N' AND (tdpdmtim.VLID_DTM >= '" + ADATE + "') AND (tdpdmtim.VLID_DTM <= '" + BDATE + "') HAVING `PART_NO` LIKE '" + LPARKNO + "' AND `SERIAL_ID` LIKE '" + SERIERID + "' AND `PART_NAME` LIKE '" + PART_NM + "'  AND `Good_BRCODE` LIKE '" + RO_BC + "' ORDER BY tdpdmtim.VLID_DTM, (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`))";

                                //Cải tiến
                                //sql = @"SELECT mtb.* from
                                //(SELECT 
                                //    FALSE AS `CHK`,
                                //    dp.PO_CODE AS `PO_CODE`,
                                //    sp.PART_NO AS `PART_NO`,
                                //    sp.PART_NM AS `PART_NAME`,
                                //    sp.PART_FML AS `PART_FML`,
                                //    IFNULL(sp.PART_CAR, '') AS `PART_STAGE`,
                                //    tm.VLID_PART_SNP AS `PART_SNP`,
                                //    dp.PO_QTY AS `PO_QTY`,
                                //    IFNULL(pm.PLET_NO, '') AS `Pallet_NO`,
                                //    IFNULL(pm.PLET_COMS, '') AS `PALLET_CODE`,
                                //    tm.VLID_GRP AS `SERIAL_ID`,
                                //    tm.VLID_DTM AS `INPUT_DATE`,
                                //    tm.VLID_BARCODE AS `Good_BRCODE`,
                                //    tm.VLID_REMARK,
                                //    tm.UPDATE_DTM AS `SHIPPING_DATE`,
                                //    tm.VLID_DSCN_YN,
                                //    iv.QTY AS `STOCK`,
                                //    tm.PDMTIN_IDX,
                                //    IFNULL(dp.DONE_YN, 'N') AS `DONE_YN`
                                //FROM tdpdmtim tm
                                //LEFT JOIN tdpdotpl dp ON dp.PDOTPL_IDX = tm.PDOTPL_IDX
                                //LEFT JOIN tspart sp ON sp.PART_IDX = tm.VLID_PART_IDX
                                //LEFT JOIN tdpdotplmu pm ON pm.TDPDOTPLMU_IDX = tm.TDPDOTPLMU_IDX
                                //LEFT JOIN tiivtr iv ON iv.PART_IDX = tm.VLID_PART_IDX AND iv.LOC_IDX = '2'
                                //WHERE tm.UPDATE_DTM BETWEEN '" + ADATE + "' AND '" + BDATE + "' AND tm.VLID_GRP LIKE '" + SERIERID + @"') AS mtb WHERE mtb.PO_CODE LIKE '" + LPOCODE + @"' AND mtb.PART_NO LIKE '" + LPARKNO + @"' AND mtb.Pallet_NO LIKE '" + LPNTNO + @"' AND mtb.VLID_DSCN_YN = 'N' AND mtb.PART_NAME LIKE '" + PART_NM + @"' AND mtb.Good_BRCODE LIKE '" + RO_BC + @"' ORDER BY mtb.INPUT_DATE DESC, mtb.PART_NO";
                            }
                            sql = sql + CHK_SQLTEXT;

                            //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView2 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            sql = @"SELECT count(PDMTIN_IDX) AS `PDMTIN_IDX`,
                            (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_NAME`,
                            VLID_BARCODE AS `Good_BRCODE`
                            FROM tdpdmtim  ";

                            CHK_SQLTEXT = "";
                            if (BC_DSCN_YN == "Y")
                            {
                                CHK_SQLTEXT = @"WHERE (SELECT `PO_CODE` FROM tdpdotpl WHERE PDOTPL_IDX= tdpdmtim.PDOTPL_IDX) LIKE '" + LPOCODE + "' AND (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) LIKE  '" + LPARKNO + "' AND(SELECT `PLET_NO` FROM tdpdotplmu WHERE(`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)) LIKE '" + LPNTNO + "' AND VLID_DSCN_YN = 'Y' AND(tdpdmtim.UPDATE_DTM >= '" + ADATE + "') AND(tdpdmtim.UPDATE_DTM <= '" + BDATE + "') AND `VLID_GRP` LIKE '" + SERIERID + "' HAVING `PART_NAME` LIKE '" + PART_NM + "' AND `Good_BRCODE` LIKE '" + RO_BC + "' ORDER BY tdpdmtim.VLID_DTM DESC, (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AND NOT(`TDPDOTPLMU_IDX` IS NULL) AND NOT(`PDOTPL_IDX` IS NULL)";
                            }
                            else
                            {
                                CHK_SQLTEXT = @"WHERE VLID_DSCN_YN='N' AND (tdpdmtim.VLID_DTM >= '" + ADATE + "') AND (tdpdmtim.VLID_DTM <= '" + BDATE + "') HAVING `PART_NO` LIKE '" + LPARKNO + "' AND `SERIAL_ID` LIKE '" + SERIERID + "' AND `PART_NAME` LIKE '" + PART_NM + "'  AND `Good_BRCODE` LIKE '" + RO_BC + "' ORDER BY tdpdmtim.VLID_DTM, (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`))";
                            }
                            sql = sql + CHK_SQLTEXT;

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            result.ErrorNumber = result.DataGridView[0].PDMTIN_IDX;

                            //result.DataGridView2= result.DataGridView2.Take(GlobalHelper.ListCount.Value).ToList();
                        }
                    }
                    if (BaseParameter.Action == 5)
                    {
                        result = await COMB_LIST(BaseParameter);
                    }
                    if (BaseParameter.Action == 6)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = BaseParameter.ListSearchString[0];
                            var BBB = BaseParameter.ListSearchString[1];
                            var CCC = BaseParameter.ListSearchString[2];
                            var DDD = BaseParameter.ListSearchString[3];
                            string sql = @"SELECT  tdpdmtim.`PDOTPL_IDX` AS `CODE`, (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) AS `PO_CODE`,
                            IFNULL((SELECT tdpdotplmu.PLET_NO FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX), '') AS `PLET_NO`,
                            IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), '') AS `CM_PALLET_NO`,
                            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NO`,
                            (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_GRP`,
                            (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NM`,
                            tdpdmtim.`VLID_PART_SNP` AS `PART_SNP`, `VLID_GRP`, tdpdmtim.`CREATE_DTM`,
                            (SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) AS `PO_QTY`, 
                            COUNT(tdpdmtim.`VLID_GRP`) AS `QTY`, CEILING(COUNT(tdpdmtim.`VLID_GRP`) / tdpdmtim.`VLID_PART_SNP`) AS `BOX_QTY`,
                            ((SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) - `CD`.`CONT`) AS `Not_yet_packing`,
                            COALESCE((SELECT `QTY` FROM tiivtr WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AND `LOC_IDX` = '2'), 0) AS `Inventory`,
                            (IF(COUNT(tdpdmtim.`VLID_GRP`)=(SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`), 'OK', 'Waiting')) AS  `Ship_Status`, `TDPDOTPLMU_IDX`,
                            IFNULL((SELECT `DONE_YN` FROM tdpdotpl WHERE PDOTPL_IDX= tdpdmtim.PDOTPL_IDX), 'N') AS `DONE_YN`

                            FROM tdpdmtim

                            LEFT JOIN (SELECT tdpdmtim.PDOTPL_IDX, COUNT(PDOTPL_IDX) AS `CONT` FROM tdpdmtim  GROUP BY PDOTPL_IDX) AS `CD`
                            ON tdpdmtim.`PDOTPL_IDX` = `CD`.PDOTPL_IDX

                            WHERE NOT(tdpdmtim.`TDPDOTPLMU_IDX` IS NULL) AND NOT(tdpdmtim.`PDOTPL_IDX` IS NULL) AND 
                            (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) = '" + AAA + "' AND (SELECT `PART_NM` FROM tspart WHERE(`PART_IDX` = tdpdmtim.VLID_PART_IDX)) LIKE '%" + BBB + "%' AND (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.VLID_PART_IDX)) LIKE '%" + CCC + "%' AND (SELECT tdpdotplmu.PLET_NO FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX)  LIKE '%" + DDD + "%' GROUP BY tdpdmtim.`VLID_GRP` ORDER BY `CODE` DESC, `PLET_NO`, `PART_NO`      ";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView8 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView8.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            sql = @"SELECT  (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) AS `PO_CODE`,
                            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NO`,
                            (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_GRP`,
                            (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NM`,
                            tdpdmtim.`VLID_PART_SNP` AS `PART_SNP`,
                            (SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) AS `PO_QTY`, 
                            COUNT(tdpdmtim.`VLID_GRP`) AS `QTY`, CEILING(COUNT(tdpdmtim.`VLID_GRP`) / tdpdmtim.`VLID_PART_SNP`) AS `BOX_QTY`,
                            ((SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) - `CD`.`CONT`) AS `Not_yet_packing`,
                            COALESCE((SELECT `QTY` FROM tiivtr WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AND `LOC_IDX` = '2'), 0) AS `Inventory`,
                            (IF(COUNT(tdpdmtim.`VLID_GRP`)=(SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`), 'OK', 'Waiting')) AS  `Ship_Status`,
                            tdpdmtim.`PDOTPL_IDX` AS `CODE`
                            FROM tdpdmtim

                            LEFT JOIN (SELECT tdpdmtim.PDOTPL_IDX, COUNT(PDOTPL_IDX) AS `CONT` FROM tdpdmtim  GROUP BY PDOTPL_IDX) AS `CD`
                            ON tdpdmtim.`PDOTPL_IDX` = `CD`.PDOTPL_IDX

                            WHERE NOT(tdpdmtim.`TDPDOTPLMU_IDX` IS NULL) AND NOT(tdpdmtim.`PDOTPL_IDX` IS NULL) AND 
                            (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) LIKE '%" + AAA + "%' AND (SELECT `PART_NM` FROM tspart WHERE(`PART_IDX` = tdpdmtim.VLID_PART_IDX)) LIKE '%" + BBB + "%' AND (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.VLID_PART_IDX)) LIKE '%" + CCC + "%' AND (SELECT tdpdotplmu.PLET_NO FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX)  LIKE '%" + DDD + "%' GROUP BY `PART_NO` ORDER BY `CODE` DESC      ";

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView10 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView10.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            sql = @"SELECT 
                            COUNT(DISTINCT (`MAIN_A`.`PLET_NO`)) AS `PLT`,   
                            COUNT(DISTINCT(`MAIN_A`.`CM_PALLET_NO`)) AS `CM_PLT`,   
                            CEILING(SUM(`MAIN_A`.`BOX_QTY`)) AS `BOX`,  
                            SUM(`MAIN_A`.`QTY`) AS `QTY`
                            FROM (
                            SELECT  
                            IFNULL((SELECT tdpdotplmu.PLET_NO FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX), '') AS `PLET_NO`,
                            IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), '') AS `CM_PALLET_NO`,
                            (SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) AS `PO_QTY`, 
                            COUNT(tdpdmtim.`VLID_GRP`) AS `QTY`, 
                            CEILING(COUNT(tdpdmtim.`VLID_GRP`) / (tdpdmtim.`VLID_PART_SNP`)) AS `BOX_QTY`
                            FROM tdpdmtim
                            WHERE NOT(tdpdmtim.`TDPDOTPLMU_IDX` IS NULL) AND NOT(tdpdmtim.`PDOTPL_IDX` IS NULL) AND 
                            (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) LIKE '%" + AAA + "%' AND (SELECT tdpdotplmu.PLET_NO FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX)  LIKE '%" + DDD + "%' GROUP BY tdpdmtim.`VLID_GRP`  ) `MAIN_A`      ";

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.Search = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 4)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var DATE_A = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd");
                            var DATE_B = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd");
                            var AAA = BaseParameter.ListSearchString[2];
                            var BBB = BaseParameter.ListSearchString[3];
                            var CCC = BaseParameter.ListSearchString[4];
                            var DDD = BaseParameter.ListSearchString[5];
                            string sql = @"SELECT
                            IF(IFNULL(`AA`.`VLID_PART_IDX`, '') = '' , '', `AA`.`PART_NO`) AS `PART_NO`,
                            IF(IFNULL(`AA`.`VLID_PART_IDX`, '') = '' , '', `AA`.`PART_NAME`) AS `PART_NAME`,
                            IF(IFNULL(`AA`.`VLID_PART_IDX`, '') = '' , '', `AA`.`PART_FML`) AS `PART_FML`,
                            IF(IFNULL(`AA`.`VLID_PART_IDX`, '') = '' , 'SUM', `AA`.`PART_STAGE`) AS `PART_STAGE`,
                            IF(IFNULL(`AA`.`SHIPPING DATE`, '') = '' , 'TOTAL', `AA`.`SHIPPING DATE`) AS `SHIPPING_DATE`,
                            `AA`.`QTY`

                            FROM(
                            SELECT 
                            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_NO`,
                            (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_NAME`,
                            (SELECT `PART_FML` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_FML`,
                            (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_STAGE`,
                            DATE_FORMAT(`UPDATE_DTM`, '%Y-%m-%d') AS `SHIPPING DATE`,
                            COUNT(tdpdmtim.`VLID_PART_IDX`) AS `QTY`, `VLID_PART_IDX`
                            FROM tdpdmtim  
                            WHERE (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) LIKE  '%" + AAA + "%' AND VLID_DSCN_YN = 'Y' AND(DATE_FORMAT(`UPDATE_DTM`, '%Y-%m-%d') >= '" + DATE_A + "') AND(DATE_FORMAT(`UPDATE_DTM`, '%Y-%m-%d') <= '" + DATE_B + "') AND (SELECT `PART_NM` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) LIKE  '%" + BBB + "%' AND (SELECT `PART_CAR` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) LIKE  '%" + CCC + "%' AND (SELECT `PART_FML` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) LIKE   '%" + DDD + "%' GROUP BY DATE_FORMAT(`UPDATE_DTM`, '%Y-%m-%d'), tdpdmtim.`VLID_PART_IDX` WITH ROLLUP   ) `AA` ORDER BY `PART_NO`, `SHIPPING_DATE` DESC   ";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView6 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView6.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 7)
                    {
                        var BC_TEXT = BaseParameter.SearchString;
                        string sql = @"SELECT 
                        (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_NO`,
                        (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `Name`,
                        IFNULL(tdpdmtim.`BARCD_LOC`, IFNULL((SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`), '')) AS `LOC`,
                        COUNT(tdpdmtim.`VLID_PART_IDX`) AS `QTY`, tdpdmtim.`VLID_GRP`
                        FROM tdpdmtim
                        WHERE NOT(tdpdmtim.`VLID_DSCN_YN` = 'Y') AND tdpdmtim.`VLID_GRP` = '" + BC_TEXT + "' GROUP BY tdpdmtim.`VLID_GRP`, tdpdmtim.`VLID_PART_IDX`   ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_D04_07 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_D04_07.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    if (BaseParameter.Action == 8)
                    {
                        var AAA = BaseParameter.SearchString;
                        string WHERE_SQL = "";
                        if (BaseParameter.RadioButton12 == true)
                        {
                            WHERE_SQL = "AND (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) LIKE  '%" + AAA + "%'   ";
                        }
                        if (BaseParameter.RadioButton11 == true)
                        {
                            WHERE_SQL = "AND  IFNULL(tdpdmtim.`BARCD_LOC`, IFNULL((SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`), '')) LIKE '%" + AAA + "%'   ";
                        }
                        if (BaseParameter.RadioButton13 == true)
                        {
                            WHERE_SQL = "AND  tdpdmtim.`VLID_GRP` LIKE '%" + AAA + "%'   ";
                        }
                        //string sql = @"SELECT 
                        //(SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_NO`,
                        //(SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `Name`,
                        //tdpdmtim.`VLID_DTM`,
                        //IFNULL(tdpdmtim.`BARCD_LOC`, IFNULL((SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`), '')) AS `LOC`,
                        //COUNT(tdpdmtim.`VLID_PART_IDX`) AS `QTY`,
                        //tdpdmtim.`VLID_GRP`
                        //FROM tdpdmtim
                        //WHERE NOT(tdpdmtim.`UPDATE_USER` IS NULL) AND NOT(tdpdmtim.`VLID_DSCN_YN` = 'Y')  " + WHERE_SQL + "  GROUP BY tdpdmtim.`VLID_GRP`, tdpdmtim.`VLID_PART_IDX`, tdpdmtim.`VLID_DTM`  ORDER BY tdpdmtim.`VLID_DTM` DESC  ";

                        string sql = @"SELECT 
                        (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_NO`,
                        (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `Name`,
                        tdpdmtim.`VLID_DTM`,
                        IFNULL(tdpdmtim.`BARCD_LOC`, IFNULL((SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`), '')) AS `LOC`,
                        COUNT(tdpdmtim.`VLID_PART_IDX`) AS `QTY`,
                        tdpdmtim.`VLID_GRP`
                        FROM tdpdmtim
                        WHERE NOT(tdpdmtim.`VLID_DSCN_YN` = 'Y')  " + WHERE_SQL + "  GROUP BY tdpdmtim.`VLID_GRP`, tdpdmtim.`VLID_PART_IDX`, tdpdmtim.`VLID_DTM`  ORDER BY tdpdmtim.`VLID_DTM` DESC  ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView9 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView9.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    if (BaseParameter.Action == 9)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = BaseParameter.ListSearchString[0];
                            var AAA1 = BaseParameter.ListSearchString[1];
                            if (!string.IsNullOrEmpty(AAA1))
                            {
                                AAA = AAA1;
                            }



                            //Nguyên mẫu
                            string sql = @"SELECT  
                            (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) AS `PO_CODE`,
                            IFNULL((SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX), '') AS `PLET_NO`,
                            IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), '') AS `CM_PALLET_NO`,
                            IF(LAG((SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX), 1) OVER (partition by `PLET_NO` order by `PART_NO`) = 
                            IFNULL((SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX), '') , '' , IFNULL((SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX), ''))
                            AS `PLET_NO1`,
                            (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NM`,
                            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NO`,
                            tdpdmtim.`VLID_PART_SNP` AS `PART_SNP`, 
                            CEILING(COUNT(tdpdmtim.`VLID_GRP`) / tdpdmtim.`VLID_PART_SNP`) AS `BOX_QTY`,

                            IF(LAG((SELECT tdpdotplmu.`PLET_COMS` FROM tdpdotplmu WHERE `TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`), 1) OVER (partition by `PLET_NO` order by `PART_NO`) = 
                            IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), ''), '', IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), ''))
                            AS `PALLET_SERIAL_NO`,

                            IF((MID(IF(LAG((SELECT tdpdotplmu.`PLET_COMS` FROM tdpdotplmu WHERE `TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`), 1) OVER (partition by `PLET_NO` order by `PART_NO`) = 
                            IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), ''), '', IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), '')), 1, 2)) 
                            = 'GZ', 'Gray box', '') AS `REMARK`,

                            (COUNT(tdpdmtim.`VLID_GRP`)) AS `QTY`
                            FROM tdpdmtim

                            LEFT JOIN (SELECT tdpdmtim.PDOTPL_IDX, COUNT(PDOTPL_IDX) AS `CONT` FROM tdpdmtim  GROUP BY PDOTPL_IDX) AS `CD`
                            ON tdpdmtim.`PDOTPL_IDX` = `CD`.PDOTPL_IDX

                            WHERE NOT(tdpdmtim.`TDPDOTPLMU_IDX` IS NULL) AND NOT(tdpdmtim.`PDOTPL_IDX` IS NULL) AND NOT(tdpdmtim.`UPDATE_USER` IS NULL) AND 
                            (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) = '" + AAA + "' GROUP BY `PART_NO`, `PLET_NO` ORDER BY `PLET_NO`, `PART_NO`      ";

                            //Cải tiến
                            //    string sql = @"SELECT  
                            //(SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) AS `PO_CODE`,
                            //IFNULL((SELECT tdpdotplmu.PLET_NO FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX), '') AS `PLET_NO`,
                            //IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), '') AS `CM_PALLET_NO`,
                            //IF(LAG((SELECT tdpdotplmu.PLET_NO FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX), 1) OVER (partition by `PLET_NO` order by `PART_NO`) = 
                            //IFNULL((SELECT tdpdotplmu.PLET_NO FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX), '') , '' , IFNULL((SELECT tdpdotplmu.PLET_NO FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX), ''))
                            //AS `PLET_NO1`,
                            //(SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NM`,
                            //(SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NO`,
                            //tdpdmtim.`VLID_PART_SNP` AS `PART_SNP`, 
                            //CEILING(COUNT(tdpdmtim.`VLID_GRP`) / tdpdmtim.`VLID_PART_SNP`) AS `BOX_QTY`,

                            //IF(LAG((SELECT tdpdotplmu.`PLET_COMS` FROM tdpdotplmu WHERE `TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`), 1) OVER (partition by `PLET_NO` order by `PART_NO`) = 
                            //IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), ''), '', IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), ''))
                            //AS `PALLET_SERIAL_NO`,

                            //IF((MID(IF(LAG((SELECT tdpdotplmu.`PLET_COMS` FROM tdpdotplmu WHERE `TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`), 1) OVER (partition by `PLET_NO` order by `PART_NO`) = 
                            //IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), ''), '', IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), '')), 1, 2)) 
                            //= 'GZ', 'Gray box', '') AS `REMARK`,

                            //(COUNT(tdpdmtim.`VLID_GRP`)) AS `QTY`
                            //FROM tdpdmtim

                            //LEFT JOIN (SELECT tdpdmtim.PDOTPL_IDX, COUNT(PDOTPL_IDX) AS `CONT` FROM tdpdmtim  GROUP BY PDOTPL_IDX) AS `CD`
                            //ON tdpdmtim.`PDOTPL_IDX` = `CD`.PDOTPL_IDX

                            //WHERE (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) = '" + AAA + "' GROUP BY `PART_NO`, `PLET_NO` ORDER BY `PLET_NO`, `PART_NO`      ";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView11 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView11.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    //string USER_IDX = BaseParameter.USER_IDX;
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var MaskedTextBox5 = BaseParameter.ListSearchString[0];
                            var MaskedTextBox6 = BaseParameter.ListSearchString[1];
                            if (BaseParameter.DataGridView3 != null)
                            {
                                if (BaseParameter.DataGridView3.Count > 0)
                                {
                                    string sql = "";
                                    foreach (var item in BaseParameter.DataGridView3)
                                    {
                                        var AA1 = item.Serial_Barcode;
                                        var AA2 = item.PO_IDX;
                                        var P_CODE = item.PART_CODE;
                                        var D3_SP_CHK = item.Split;

                                        sql = @"INSERT INTO `tdpdotplmu` (`PDOTPL_IDX`, `PLET_NO`, `PLET_COMS`, `PDOTPL_DTM`, `CREATE_DTM`, `CREATE_USER`)
                        VALUES ('" + AA2 + "', '" + MaskedTextBox6 + "', '" + MaskedTextBox5 + @"', NOW(), NOW(), '" + USER_ID + @"')
                        ON DUPLICATE KEY UPDATE
                            `UPDATE_DTM` = VALUES(`CREATE_DTM`),
                            `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        if (D3_SP_CHK == "N")
                                        {
                                            sql = @"UPDATE `tdpdmtim` SET
                            `VLID_DSCN_YN`='Y',
                            `TSCOST_IDX` = IFNULL((
                                SELECT `T`.`TSCOST_IDX`
                                FROM (
                                    SELECT `TSCOST_IDX`, `TSPSRT_IDX`, `TSCOST_DT`, `TSCOST_VAL`
                                    FROM TSCOST
                                    WHERE (`TSPSRT_IDX`, `TSCOST_DT`) IN (
                                        SELECT `TSPSRT_IDX`, MAX(`TSCOST_DT`)
                                        FROM TSCOST
                                        GROUP BY `TSPSRT_IDX`
                                    )
                                    ORDER BY `TSCOST_DT` DESC
                                ) `T`
                                WHERE `T`.`TSPSRT_IDX` = '" + P_CODE + @"'
                                GROUP BY `T`.`TSPSRT_IDX`
                            ), 0),
                            `PDOTPL_IDX`= " + AA2 + @",
                            `TDPDOTPLMU_IDX`= (
                                SELECT `TDPDOTPLMU_IDX`
                                FROM tdpdotplmu
                                WHERE `PLET_NO` = '" + MaskedTextBox6 + @"'
                                  AND `PDOTPL_IDX` = '" + AA2 + @"'
                            ),
                            `UPDATE_DTM` = NOW(),
                            `UPDATE_USER`= '" + USER_ID + @"'
                        WHERE `VLID_GRP`= '" + AA1 + @"'
                          AND `VLID_DSCN_YN`= 'N'";
                                        }
                                        else
                                        {
                                            sql = @"UPDATE `tdpdmtim` SET
                            `VLID_DSCN_YN`='Y',
                            `TSCOST_IDX` = IFNULL((
                                SELECT `T`.`TSCOST_IDX`
                                FROM (
                                    SELECT `TSCOST_IDX`, `TSPSRT_IDX`, `TSCOST_DT`, `TSCOST_VAL`
                                    FROM TSCOST
                                    WHERE (`TSPSRT_IDX`, `TSCOST_DT`) IN (
                                        SELECT `TSPSRT_IDX`, MAX(`TSCOST_DT`)
                                        FROM TSCOST
                                        GROUP BY `TSPSRT_IDX`
                                    )
                                    ORDER BY `TSCOST_DT` DESC
                                ) `T`
                                WHERE `T`.`TSPSRT_IDX` = '" + P_CODE + @"'
                                GROUP BY `T`.`TSPSRT_IDX`
                            ), 0),
                            `PDOTPL_IDX`= " + AA2 + @",
                            `TDPDOTPLMU_IDX`= (
                                SELECT `TDPDOTPLMU_IDX`
                                FROM tdpdotplmu
                                WHERE `PLET_NO` = '" + MaskedTextBox6 + @"'
                                  AND `PDOTPL_IDX` = '" + AA2 + @"'
                            ),
                            `UPDATE_DTM` = NOW(),
                            `UPDATE_USER`= '" + USER_ID + @"'
                        WHERE `VLID_BARCODE`= '" + AA1 + "'";
                                        }
                                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                    sql = @"ALTER TABLE `tdpdotplmu` AUTO_INCREMENT= 1";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                            }

                            if (BaseParameter.DGV_OUT != null && BaseParameter.DGV_OUT.Count > 0)
                            {
                                string sql = "";

                                // ════════════════════════════════════════════════════════════
                                // BƯỚC 1: Update PO PACK_QTY và Stock (tiivtr)
                                // ════════════════════════════════════════════════════════════
                                foreach (var item in BaseParameter.DGV_OUT)
                                {
                                    var AA1 = item.PDOTPL_IDX;
                                    var AA2 = item.QTY1;
                                    var AA3 = item.PART_IDX;

                                    if (AA2 > 0)
                                    {
                                        try
                                        {
                                            // Update PO pack quantity
                                            sql = @"UPDATE `tdpdotpl`
                        SET `PACK_QTY` = '" + AA2 + @"',
                            `UPDATE_DTM` = NOW(),
                            `UPDATE_USER` = '" + USER_ID + @"'
                        WHERE `PDOTPL_IDX` = " + AA1;
                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                            // Trừ Stock (LOC_IDX=2)
                                            sql = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`)
                        VALUES (" + AA3 + @", 2, " + -AA2 + @", NOW(), '" + USER_ID + @"')
                        ON DUPLICATE KEY UPDATE
                            `LOC_IDX` = 2,
                            `QTY` = `QTY` - " + AA2 + @",
                            `UPDATE_DTM` = NOW(),
                            `UPDATE_USER` = '" + USER_ID + "'";
                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception($"Error updating PO {AA1}: {ex.Message}");
                                        }
                                    }
                                }

                                // ════════════════════════════════════════════════════════════
                                // BƯỚC 2: Trừ FG Inventory theo từng Packing Lot
                                // ════════════════════════════════════════════════════════════
                                if (BaseParameter.DataGridView3 != null && BaseParameter.DataGridView3.Count > 0)
                                {
                                    foreach (var box in BaseParameter.DataGridView3)
                                    {
                                        string packingLot = box.Serial_Barcode ?? "";
                                        int qtyToShip = Convert.ToInt32(box.QTY ?? 0);
                                        int partIdx = Convert.ToInt32(box.PART_CODE);
                                        bool isSplit = (box.Split ?? "N") == "Y";

                                        if (qtyToShip <= 0 || string.IsNullOrEmpty(packingLot))
                                        {
                                            continue;
                                        }

                                        try
                                        {
                                            string checkSQL = @"
                                            SELECT 
                                                PACKING_LOT,
                                                PART_IDX,
                                                PART_NO,
                                                PART_NM,
                                                SNP_QTY,
                                                ACTUAL_QTY,
                                                REWORK_QTY,
                                                CUSTOMER_CODE,
                                                STATUS
                                            FROM tfg_inventory
                                            WHERE PACKING_LOT = @PackingLot
                                              AND PART_IDX = @PartIdx
                                              AND STATUS = 'IN'
                                            LIMIT 1";

                                            var parameters = new[] {
                                                new MySqlParameter("@PackingLot", MySqlDbType.VarChar) { Value = packingLot },
                                                new MySqlParameter("@PartIdx", MySqlDbType.Int32) { Value = partIdx }
                                            };

                                            var reworkCheck = await MySQLHelper.FillDataSetBySQLAsync(
                                                GlobalHelper.MariaDBConectionString, checkSQL, parameters);

                                            if (reworkCheck == null || reworkCheck.Tables.Count == 0 ||
                                                reworkCheck.Tables[0].Rows.Count == 0)
                                            {
                                                throw new Exception(
                                                    $"Packing lot {packingLot} not found in FG inventory or already shipped");
                                            }

                                            var row = reworkCheck.Tables[0].Rows[0];
                                            string partNo = row["PART_NO"].ToString();
                                            string partNm = row["PART_NM"].ToString();
                                            int snpQty = Convert.ToInt32(row["SNP_QTY"]);
                                            int currentActualQty = Convert.ToInt32(row["ACTUAL_QTY"]);
                                            int currentReworkQty = Convert.ToInt32(row["REWORK_QTY"]);
                                            string customerCode = row["CUSTOMER_CODE"]?.ToString() ?? "";

                                            if (qtyToShip > currentActualQty)
                                            {
                                                throw new Exception(
                                                    $"Insufficient stock in packing lot {packingLot}. Available: {currentActualQty}, Requested: {qtyToShip}");
                                            }

                                            bool isReworkLot = currentReworkQty > 0;
                                            int reworkToDeduct = isReworkLot ? qtyToShip : 0;

                                            if (isReworkLot)
                                            {
                                                sql = @"
                                                UPDATE tfg_inventory
                                                SET
                                                    REWORK_QTY = REWORK_QTY - " + qtyToShip + @",
                                                    ACTUAL_QTY = ACTUAL_QTY - " + qtyToShip + @",
                                                    STATUS = CASE 
                                                        WHEN (ACTUAL_QTY - " + qtyToShip + @") <= 0 THEN 'OUT'
                                                        ELSE 'in'
                                                    END,
                                                    FG_OUT_DATE = CASE 
                                                        WHEN (ACTUAL_QTY - " + qtyToShip + @") <= 0 THEN NOW()
                                                        ELSE FG_OUT_DATE
                                                    END,
                                                    FG_OUT_USER = CASE 
                                                        WHEN (ACTUAL_QTY - " + qtyToShip + @") <= 0 THEN '" + USER_ID + @"'
                                                        ELSE FG_OUT_USER
                                                    END,
                                                    UPDATE_DTM = NOW()
                                                WHERE PACKING_LOT = '" + packingLot + @"'
                                                  AND PART_IDX = " + partIdx + @"
                                                  AND STATUS = 'IN'";
                                            }
                                            else
                                            {
                                                sql = @"
                                                UPDATE tfg_inventory
                                                SET
                                                    ACTUAL_QTY = ACTUAL_QTY - " + qtyToShip + @",
                                                    STATUS = CASE 
                                                        WHEN (ACTUAL_QTY - " + qtyToShip + @") <= 0 THEN 'OUT'
                                                        ELSE 'in'
                                                    END,
                                                    FG_OUT_DATE = CASE 
                                                        WHEN (ACTUAL_QTY - " + qtyToShip + @") <= 0 THEN NOW()
                                                        ELSE FG_OUT_DATE
                                                    END,
                                                    FG_OUT_USER = CASE 
                                                        WHEN (ACTUAL_QTY - " + qtyToShip + @") <= 0 THEN '" + USER_ID + @"'
                                                        ELSE FG_OUT_USER
                                                    END,
                                                    UPDATE_DTM = NOW()
                                                WHERE PACKING_LOT = '" + packingLot + @"'
                                                  AND PART_IDX = " + partIdx + @"
                                                  AND STATUS = 'IN'";
                                            }

                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                            string remark = isReworkLot ? "REWORK shipment" : "Normal shipment";
                                            remark += isSplit ? " [PARTIAL]" : " [FULL BOX]";

                                            sql = @"
                                            INSERT INTO tfg_history (
                                                PACKING_LOT, PART_IDX, PART_NO, PART_NM,
                                                SNP_QTY, ACTUAL_QTY, REWORK_QTY,
                                                TRANS_TYPE, TRANS_DATE, TRANS_USER,
                                                CUSTOMER_CODE, REMARK, CREATE_DTM
                                            ) VALUES (
                                                '" + packingLot + @"',
                                                " + partIdx + @",
                                                '" + partNo + @"',
                                                '" + partNm + @"',
                                                " + snpQty + @",
                                                " + qtyToShip + @",
                                                " + reworkToDeduct + @",
                                                'OUT',
                                                NOW(),
                                                '" + USER_ID + @"',
                                                '" + customerCode + @"',
                                                '" + remark.Replace("'", "''") + @"',
                                                NOW()
                                            )";

                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception(
                                                $"Error processing packing lot {packingLot}: {ex.Message}");
                                        }
                                    }
                                }


                                sql = @"ALTER TABLE `tiivtr` AUTO_INCREMENT = 1";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                        }
                    }
                    if (BaseParameter.Action == 5)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = BaseParameter.ListSearchString[0];
                            var BBB = BaseParameter.ListSearchString[1];
                            var CCC = BaseParameter.ListSearchString[2];
                            var PO_CODE = BaseParameter.ListSearchString[3];
                            var MaskedTextBox4 = BaseParameter.ListSearchString[4];

                            if (BaseParameter.RadioButton5 == true)
                            {
                                string sql = @"UPDATE `tdpdotplmu` SET `PLET_NO`='" + AAA + "', `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + USER_ID + "' WHERE  `TDPDOTPLMU_IDX`=" + CCC;
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }
                            if (BaseParameter.RadioButton6 == true)
                            {
                                string sql = @"INSERT INTO `tdpdotplmu` (`PDOTPL_IDX`, `PLET_NO`, `PLET_COMS`, `PDOTPL_DTM`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + PO_CODE + "', '" + MaskedTextBox4 + "', '', NOW(), NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"UPDATE `tdpdmtim` SET `TDPDOTPLMU_IDX` = (SELECT tdpdotplmu.TDPDOTPLMU_IDX FROM tdpdotplmu WHERE tdpdotplmu.PLET_NO = '" + AAA + "' AND tdpdotplmu.PDOTPL_IDX = " + PO_CODE + "), `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + USER_ID + "'  WHERE `VLID_GRP` = '" + BBB + "'";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                sql = @"ALTER TABLE     `tdpdotplmu`     AUTO_INCREMENT= 1";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            }

                        }
                    }
                    if (BaseParameter.Action == 7)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = BaseParameter.ListSearchString[0];
                            var BBB = BaseParameter.ListSearchString[1];

                            string sql = @"UPDATE `tdpdmtim` SET `BARCD_LOC`='" + BBB + "' WHERE  `VLID_GRP`= '" + AAA + "'";
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
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.DataGridView2 != null)
                        {
                            if (BaseParameter.DataGridView2.Count > 0)
                            {
                                foreach (var item in BaseParameter.DataGridView2)
                                {
                                    var LISTCHK = item.CHK;
                                    var COMP_CHK = item.DONE_YN;
                                    if (COMP_CHK == "N")
                                    {
                                        if (LISTCHK == true)
                                        {
                                            var AAA = item.PDMTIN_IDX;
                                            var BBB = item.VLID_DSCN_YN;
                                            var DDD = item.PART_NO;
                                            var EEE = item.Good_BRCODE;

                                            string sql = @"SELECT  IV_IDX, QTY FROM tiivtr WHERE (SELECT tspart.PART_NO FROM tspart WHERE  tspart.PART_IDX  = tiivtr.PART_IDX) = '" + DDD + "' AND tiivtr.LOC_IDX = '2'";

                                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                            var DGV_D03_021 = new List<SuperResultTranfer>();
                                            for (int i = 0; i < ds.Tables.Count; i++)
                                            {
                                                DataTable dt = ds.Tables[i];
                                                DGV_D03_021.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                            }

                                            if (DGV_D03_021.Count > 0)
                                            {
                                                var CCC = DGV_D03_021[0].QTY;
                                                if (BBB == "Y")
                                                {
                                                    CCC = CCC + 1;

                                                    sql = @"UPDATE tdpdmtim SET `PDOTPL_IDX`= NULL, `TDPDOTPLMU_IDX`= NULL, VLID_DSCN_YN = 'N', UPDATE_DTM = NOW(), UPDATE_USER = '" + USER_ID + "' WHERE  (PDMTIN_IDX = '" + AAA + "')";
                                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                                    sql = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES ((SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = '" + DDD + "'), '2', '" + CCC + "', NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `QTY` = '" + CCC + "', `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                                    sql = @"ALTER TABLE     `tiivtr`     AUTO_INCREMENT= 1";
                                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 6)
                    {
                        if (BaseParameter.DataGridView8 != null)
                        {
                            if (BaseParameter.DataGridView8.Count > 0)
                            {
                                foreach (var item in BaseParameter.DataGridView8)
                                {
                                    if (item.DONE_YN == "N")
                                    {
                                        if (item.CHK == true)
                                        {
                                            var D8PART = item.PART_NO;
                                            var D8SOTCK = item.QTY;
                                            string sql = @"SELECT  IV_IDX, QTY   FROM     tiivtr   
                                        WHERE  (SELECT tspart.PART_NO FROM tspart WHERE  tspart.PART_IDX  = tiivtr.PART_IDX) = '" + D8PART + "' AND tiivtr.LOC_IDX = '2'";

                                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                            var DGV_D03_024 = new List<SuperResultTranfer>();
                                            for (int i = 0; i < ds.Tables.Count; i++)
                                            {
                                                DataTable dt = ds.Tables[i];
                                                DGV_D03_024.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                            }
                                            if (DGV_D03_024.Count > 0)
                                            {
                                                var CCC = DGV_D03_024[0].QTY;
                                                CCC = CCC + D8SOTCK;
                                                sql = @"UPDATE `tdpdmtim` SET  `VLID_DSCN_YN`='N', `TDPDOTPLMU_IDX` = NULL , `PDOTPL_IDX` = NULL , `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `TDPDOTPLMU_IDX`= '" + item.TDPDOTPLMU_IDX + "' AND `PDOTPL_IDX` = '" + item.CODE + "' AND `VLID_GRP` = '" + item.VLID_GRP + "'";
                                                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                                sql = @"UPDATE `tdpdotpl` SET  `PACK_QTY`= (`PACK_QTY` - " + D8SOTCK + "),  `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `PDOTPL_IDX`= '" + item.CODE + "'";
                                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                                sql = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES ((SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = '" + D8PART + "'), '2', '" + CCC + "', NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `QTY` = '" + CCC + "', `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                                                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                                sql = @"ALTER TABLE     `tiivtr`     AUTO_INCREMENT= 1";
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
                    if (BaseParameter.Action == 1)
                    {

                    }
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var ADATE = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd");
                            var BDATE = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd");
                            var LPARKNO = "%" + BaseParameter.ListSearchString[2] + "%";
                            var SERIERID = "%" + BaseParameter.ListSearchString[3] + "%";
                            var LPOCODE = "%" + BaseParameter.ListSearchString[4] + "%";
                            var LPNTNO = "%" + BaseParameter.ListSearchString[5] + "%";
                            var PART_NM = "%" + BaseParameter.ListSearchString[6] + "%";
                            var RO_BC = "%" + BaseParameter.ListSearchString[7] + "%";
                            var BC_DSCN_YN = BaseParameter.ListSearchString[8];

                            string sql = @"SELECT FALSE AS `CHK`, 
                            (SELECT `PO_CODE` FROM tdpdotpl WHERE PDOTPL_IDX= tdpdmtim.PDOTPL_IDX) AS `PO_CODE`,
                            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_NO`,
                            (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_NAME`,
                            (SELECT `PART_FML` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AS `PART_FML`,
                            IFNULL((SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)), '') AS `PART_STAGE`,
                            (`VLID_PART_SNP`) AS `PART_SNP`,
                            (SELECT `PO_QTY` FROM tdpdotpl WHERE PDOTPL_IDX = tdpdmtim.`PDOTPL_IDX`) AS PO_QTY,
                            IFNULL((SELECT `PLET_NO` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), '') AS `Pallet_NO`,
                            IFNULL((SELECT `PLET_COMS` FROM tdpdotplmu WHERE (`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)), '') AS `PALLET_CODE`,
                            `VLID_GRP` AS `SERIAL_ID`, 
                            VLID_DTM AS `INPUT_DATE`,
                            VLID_BARCODE AS `Good_BRCODE`, 
                            VLID_REMARK, 
                            UPDATE_DTM AS `SHIPPING_DATE` ,
                            VLID_DSCN_YN,
                            (SELECT `QTY` FROM tiivtr WHERE (tiivtr.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AND  LOC_IDX = '2') AS `STOCK`, 
                            `PDMTIN_IDX`,
                            IFNULL((SELECT `DONE_YN` FROM tdpdotpl WHERE PDOTPL_IDX= tdpdmtim.PDOTPL_IDX), 'N') AS `DONE_YN`
                            FROM tdpdmtim  ";

                            string CHK_SQLTEXT = "";
                            if (BC_DSCN_YN == "Y")
                            {
                                CHK_SQLTEXT = @"WHERE (SELECT `PO_CODE` FROM tdpdotpl WHERE PDOTPL_IDX= tdpdmtim.PDOTPL_IDX) LIKE '" + LPOCODE + "' AND (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) LIKE  '" + LPARKNO + "' AND(SELECT `PLET_NO` FROM tdpdotplmu WHERE(`TDPDOTPLMU_IDX` = tdpdmtim.`TDPDOTPLMU_IDX`)) LIKE '" + LPNTNO + "' AND VLID_DSCN_YN = 'Y' AND(tdpdmtim.UPDATE_DTM >= '" + ADATE + "') AND(tdpdmtim.UPDATE_DTM <= '" + BDATE + "') AND `VLID_GRP` LIKE '" + SERIERID + "' HAVING `PART_NAME` LIKE '" + PART_NM + "' AND `Good_BRCODE` LIKE '" + RO_BC + "' ORDER BY tdpdmtim.VLID_DTM DESC, (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`)) AND NOT(`TDPDOTPLMU_IDX` IS NULL) AND NOT(`PDOTPL_IDX` IS NULL)";
                            }
                            else
                            {
                                CHK_SQLTEXT = @"WHERE VLID_DSCN_YN='N' AND (tdpdmtim.VLID_DTM >= '" + ADATE + "') AND (tdpdmtim.VLID_DTM <= '" + BDATE + "') HAVING `PART_NO` LIKE '" + LPARKNO + "' AND `SERIAL_ID` LIKE '" + SERIERID + "' AND `PART_NAME` LIKE '" + PART_NM + "'  AND `Good_BRCODE` LIKE '" + RO_BC + "' ORDER BY tdpdmtim.VLID_DTM, (SELECT `PART_NO` FROM tspart WHERE(`PART_IDX` = tdpdmtim.`VLID_PART_IDX`))";
                            }
                            sql = sql + CHK_SQLTEXT;

                            //sql = sql + " LIMIT " + GlobalHelper.ListCount2000;

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
                string SheetName = this.GetType().Name;
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 5)
                    {
                        if (BaseParameter.T_DGV_02 != null && BaseParameter.T_DGV_02.Count > 0)
                        {
                            if (BaseParameter.ListSearchString != null)
                            {
                                var SU01 = BaseParameter.ListSearchString[0];
                                var SU02 = BaseParameter.ListSearchString[1];

                                var SU03 = GlobalHelper.InitializationString;
                                //List<string> ListSU03 = new List<string>();
                                //for (int i = 0; i < BaseParameter.T_DGV_02.Count; i++)
                                //{
                                //    if (BaseParameter.T_DGV_02[i].CHK == true)
                                //    {
                                //        if (!string.IsNullOrEmpty(BaseParameter.T_DGV_02[i].PLET_NO))
                                //        {
                                //            ListSU03.Add(BaseParameter.T_DGV_02[i].PLET_NO);
                                //        }
                                //    }
                                //}
                                //SU03 = string.Join(",", ListSU03.Select(x => $"'{x}'"));


                                var SU11 = SU01.Substring(0, 12);

                                var DGV_D04_PLT = new List<SuperResultTranfer>();

                                for (int j = 0; j < BaseParameter.T_DGV_02.Count; j++)
                                {
                                    if (BaseParameter.T_DGV_02[j].CHK == true)
                                    {
                                        SU03 = BaseParameter.T_DGV_02[j].PLET_NO;
                                        string sql = @"SELECT 
                                    tdpdmtim.`PDOTPL_IDX` AS `CODE`, 
                                    (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) AS `PO_CODE`,
                                    (SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX) AS `PLET_NO`, 
                                    (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NO`,
                                    (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_GRP`,
                                    (SELECT `PART_FML` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NAME`,
                                    tdpdmtim.`VLID_PART_SNP` AS `PART_SNP`,
                                    `VLID_GRP` AS `Serial_ID`,
                                    (SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.PDOTPL_IDX) AS `PO_QTY`, 
                                    COUNT(tdpdmtim.`VLID_GRP`) AS `QTY`, 
                                    IFNULL(CEILING(COUNT(tdpdmtim.`VLID_GRP`) / tdpdmtim.`VLID_PART_SNP`), 1) AS `BOX_QTY`,
                                    (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NM`,
                                    IFNULL((SELECT tdd_poplan.TDD_REMARK  FROM tdd_poplan WHERE tdd_poplan.TDD_POCODE  LIKE '" + SU11 + "%' LIMIT 1), '') AS `SHIP_NO` FROM tdpdmtim WHERE NOT(`TDPDOTPLMU_IDX` IS NULL) AND NOT(`PDOTPL_IDX` IS NULL) AND ((SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`)  LIKE '" + SU01 + "') AND(SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX) = '" + SU03 + "' AND `VLID_GRP` LIKE '%" + SU02 + "%' GROUP BY `PART_NO`, `PART_SNP` ORDER BY tdpdmtim.`PDMTIN_IDX` DESC  ";

                                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);

                                        for (int i = 0; i < ds.Tables.Count; i++)
                                        {
                                            DataTable dt = ds.Tables[i];
                                            DGV_D04_PLT.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                        }
                                    }
                                }
                                result.DGV_01 = new List<SuperResultTranfer>();
                                if (DGV_D04_PLT.Count > 0)
                                {
                                    var II = 0;
                                    var JJ = DGV_D04_PLT.Count - 1;
                                    if (BaseParameter.RadioButton9 == true)
                                    {
                                        if (JJ >= 20)
                                        {
                                            JJ = 19;
                                        }
                                    }
                                    if (BaseParameter.RadioButton10 == true)
                                    {
                                        if (JJ >= 6)
                                        {
                                            JJ = 6;
                                        }
                                    }


                                    for (II = 0; II <= JJ; II++)
                                    {
                                        StringBuilder HTMLContent = new StringBuilder();
                                        StringBuilder HTMLD04Report = new StringBuilder();
                                        StringBuilder D04Report_AIR = new StringBuilder();
                                        StringBuilder D04Report_H = new StringBuilder();

                                        var PARTGRP = "";
                                        var PALLLETNO = "";
                                        var PARTNAME = "";
                                        var PART_NM = "";
                                        var SHIP_NO = "";
                                        var PrintDate = DateTime.Now.ToString("dd/MM/yyyy");
                                        var PrintTime = DateTime.Now.ToString("HH:mm:ss");

                                        var AAA = DGV_D04_PLT[II].PART_GRP;
                                        var BBB = DGV_D04_PLT[II].PO_CODE;
                                        var CCC = DGV_D04_PLT[II].QTY;
                                        var DDD = DGV_D04_PLT[II].BOX_QTY;
                                        var EEE = DGV_D04_PLT[II].PLET_NO;
                                        var FFF = DGV_D04_PLT[II].PART_NO;
                                        var GGG = DGV_D04_PLT[II].PART_SNP;
                                        var HHH = DGV_D04_PLT[II].PART_NAME;
                                        var KKK = DGV_D04_PLT[II].PART_NM;
                                        var LLL = DGV_D04_PLT[II].SHIP_NO;

                                        PARTGRP = AAA;
                                        PALLLETNO = EEE;
                                        PARTNAME = HHH;
                                        PART_NM = KKK;
                                        SHIP_NO = LLL;

                                        if (KKK.Length > 7)
                                        {
                                            KKK = KKK.Substring(0, 7);
                                        }
                                        if (PART_NM.Length > 7)
                                        {
                                            PART_NM = PART_NM.Substring(0, 7);
                                        }

                                        int NO = II + 1;
                                        HTMLD04Report.AppendLine(@"<tr>");
                                        HTMLD04Report.AppendLine(@"<td style='text-align: center; font-size: 30px; font-weight: bold;'>" + NO + "</td>");
                                        HTMLD04Report.AppendLine(@"<td style='text-align: center; font-size: 30px; font-weight: bold;'>" + HHH + "</td>");
                                        HTMLD04Report.AppendLine(@"<td style='text-align: center; font-size: 30px; font-weight: bold;'>" + FFF + "</td>");
                                        HTMLD04Report.AppendLine(@"<td style='text-align: right; font-size: 30px; font-weight: bold;'>" + GGG + "</td>");
                                        HTMLD04Report.AppendLine(@"<td style='text-align: right; font-size: 30px; font-weight: bold;'>" + DDD + "</td>");
                                        HTMLD04Report.AppendLine(@"<td style='text-align: right; font-size: 30px; font-weight: bold;'>" + CCC + "</td>");
                                        HTMLD04Report.AppendLine(@"<td style='text-align: center; font-size: 30px; font-weight: bold;'>" + BBB + "</td>");
                                        HTMLD04Report.AppendLine(@"</tr>");

                                        D04Report_AIR.AppendLine(@"<tr>");
                                        D04Report_AIR.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + NO + "</td>");
                                        D04Report_AIR.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + FFF + "</td>");
                                        D04Report_AIR.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + KKK + "</td>");
                                        D04Report_AIR.AppendLine(@"<td style='text-align: right; font-size: 100px; font-weight: bold;'>" + CCC + "</td>");
                                        D04Report_AIR.AppendLine(@"<td style='text-align: right; font-size: 100px; font-weight: bold;'>" + DDD + "</td>");
                                        D04Report_AIR.AppendLine(@"</tr>");

                                        D04Report_H.AppendLine(@"<tr>");
                                        D04Report_H.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;' rowspan='2'>" + NO + "</td>");
                                        D04Report_H.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + HHH + "</td>");
                                        D04Report_H.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + CCC + "</td>");
                                        D04Report_H.AppendLine(@"</tr>");
                                        D04Report_H.AppendLine(@"<tr>");
                                        D04Report_H.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + FFF + "</td>");
                                        D04Report_H.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + DDD + "</td>");
                                        D04Report_H.AppendLine(@"</tr>");

                                        if (BaseParameter.RadioButton9 == true)
                                        {
                                            HTMLContent.AppendLine(GlobalHelper.CreateHTMLD04Report(SheetName, _WebHostEnvironment.WebRootPath, PARTGRP, PALLLETNO, HTMLD04Report.ToString(), PrintDate, PrintTime));
                                            if (!string.IsNullOrEmpty(HTMLD04Report.ToString()))
                                            {
                                                string contentHTML = GlobalHelper.InitializationString;
                                                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "EmptyLandscape2026.html");
                                                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                                {
                                                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                                    {
                                                        contentHTML = r.ReadToEnd();
                                                    }
                                                }
                                                contentHTML = contentHTML.Replace(@"[Content]", HTMLContent.ToString());
                                                string fileName = SheetName + GlobalHelper.InitializationDateTimeCode + ".html";
                                                string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                                                Directory.CreateDirectory(physicalPathCreate);
                                                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                                                string filePath = Path.Combine(physicalPathCreate, fileName);
                                                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                                                {
                                                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                                    {
                                                        await w.WriteLineAsync(contentHTML);
                                                    }
                                                }
                                                result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;

                                                SuperResultTranfer SuperResultTranfer = new SuperResultTranfer();
                                                SuperResultTranfer.CONTENT = result.Code;
                                                SuperResultTranfer.PalletNo = DGV_D04_PLT[II].PLET_NO;
                                                result.DGV_01.Add(SuperResultTranfer);
                                            }
                                        }
                                        if (BaseParameter.RadioButton10 == true)
                                        {
                                            HTMLContent.AppendLine(GlobalHelper.CreateHTMLD04Report_H(SheetName, _WebHostEnvironment.WebRootPath, PART_NM, SHIP_NO, PALLLETNO, D04Report_H.ToString(), PrintDate, PrintTime));
                                            if (!string.IsNullOrEmpty(HTMLD04Report.ToString()))
                                            {
                                                string contentHTML = GlobalHelper.InitializationString;
                                                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "EmptyPortrait2026.html");
                                                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                                {
                                                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                                    {
                                                        contentHTML = r.ReadToEnd();
                                                    }
                                                }
                                                contentHTML = contentHTML.Replace(@"[Content]", HTMLContent.ToString());
                                                string fileName = SheetName + GlobalHelper.InitializationDateTimeCode + ".html";
                                                string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                                                Directory.CreateDirectory(physicalPathCreate);
                                                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                                                string filePath = Path.Combine(physicalPathCreate, fileName);
                                                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                                                {
                                                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                                    {
                                                        await w.WriteLineAsync(contentHTML);
                                                    }
                                                }
                                                result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;

                                                SuperResultTranfer SuperResultTranfer = new SuperResultTranfer();
                                                SuperResultTranfer.CONTENT = result.Code;
                                                SuperResultTranfer.PalletNo = DGV_D04_PLT[II].PLET_NO;
                                                result.DGV_01.Add(SuperResultTranfer);
                                            }
                                        }
                                        if (BaseParameter.RadioButton14 == true)
                                        {
                                            HTMLContent.AppendLine(GlobalHelper.CreateHTMLD04Report_AIR(SheetName, _WebHostEnvironment.WebRootPath, PARTGRP, PARTNAME, PALLLETNO, D04Report_AIR.ToString(), PrintDate, PrintTime));
                                            if (!string.IsNullOrEmpty(HTMLD04Report.ToString()))
                                            {
                                                string contentHTML = GlobalHelper.InitializationString;
                                                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "EmptyPortrait2026.html");
                                                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                                {
                                                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                                    {
                                                        contentHTML = r.ReadToEnd();
                                                    }
                                                }
                                                contentHTML = contentHTML.Replace(@"[Content]", HTMLContent.ToString());
                                                string fileName = SheetName + GlobalHelper.InitializationDateTimeCode + ".html";
                                                string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                                                Directory.CreateDirectory(physicalPathCreate);
                                                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                                                string filePath = Path.Combine(physicalPathCreate, fileName);
                                                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                                                {
                                                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                                    {
                                                        await w.WriteLineAsync(contentHTML);
                                                    }
                                                }
                                                result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;

                                                SuperResultTranfer SuperResultTranfer = new SuperResultTranfer();
                                                SuperResultTranfer.CONTENT = result.Code;
                                                SuperResultTranfer.PalletNo = DGV_D04_PLT[II].PLET_NO;
                                                result.DGV_01.Add(SuperResultTranfer);
                                            }
                                        }

                                    }

                                    StringBuilder HTMLContentList = new StringBuilder();

                                    foreach (var SuperResultTranfer in result.DGV_01)
                                    {
                                        HTMLContentList.AppendLine(@"<a target='_blank' href='" + SuperResultTranfer.CONTENT + "' title='" + SuperResultTranfer.CONTENT + "'><h4>" + SuperResultTranfer.PalletNo + "</h4></a>");
                                    }

                                    string contentHTMLList = GlobalHelper.InitializationString;
                                    string physicalPathOpenList = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "EmptyLandscape2026List.html");
                                    using (FileStream fs = new FileStream(physicalPathOpenList, FileMode.Open))
                                    {
                                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                        {
                                            contentHTMLList = r.ReadToEnd();
                                        }
                                    }
                                    contentHTMLList = contentHTMLList.Replace(@"[Content]", HTMLContentList.ToString());
                                    string fileNameList = SheetName + GlobalHelper.InitializationDateTimeCode + ".html";
                                    string physicalPathCreateList = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                                    Directory.CreateDirectory(physicalPathCreateList);
                                    GlobalHelper.DeleteFilesByPath(physicalPathCreateList);
                                    string filePathList = Path.Combine(physicalPathCreateList, fileNameList);
                                    using (FileStream fs = new FileStream(filePathList, FileMode.Create))
                                    {
                                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                        {
                                            await w.WriteLineAsync(contentHTMLList);
                                        }
                                    }
                                    result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileNameList;
                                }
                            }
                        }

                        //if (BaseParameter.ListSearchString != null)
                        //{
                        //    var SU01 = BaseParameter.ListSearchString[0];
                        //    var SU02 = BaseParameter.ListSearchString[1];
                        //    var SU03 = BaseParameter.ListSearchString[2];
                        //    var SU22 = SU02.Substring(0, 12);

                        //    string sql = @"SELECT 
                        //    tdpdmtim.`PDOTPL_IDX` AS `CODE`, 
                        //    (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) AS `PO_CODE`,
                        //    (SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX) AS `PLET_NO`, 
                        //    (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NO`,
                        //    (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_GRP`,
                        //    (SELECT `PART_FML` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NAME`,
                        //    tdpdmtim.`VLID_PART_SNP` AS `PART_SNP`,
                        //    `VLID_GRP` AS `Serial_ID`,
                        //    (SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.PDOTPL_IDX) AS `PO_QTY`, 
                        //    COUNT(tdpdmtim.`VLID_GRP`) AS `QTY`, 
                        //    IFNULL(CEILING(COUNT(tdpdmtim.`VLID_GRP`) / tdpdmtim.`VLID_PART_SNP`), 1) AS `BOX_QTY`,
                        //    (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NM`,
                        //    IFNULL((SELECT tdd_poplan.TDD_REMARK  FROM tdd_poplan WHERE tdd_poplan.TDD_POCODE  LIKE '" + SU22 + "%' LIMIT 1), '') AS `SHIP_NO` FROM tdpdmtim WHERE NOT(`TDPDOTPLMU_IDX` IS NULL) AND NOT(`PDOTPL_IDX` IS NULL) AND ((SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`)  LIKE '" + SU02 + "') AND(SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX) = '" + SU01 + "' AND `VLID_GRP` LIKE '%" + SU03 + "%' GROUP BY `PART_NO`, `PART_SNP` ORDER BY tdpdmtim.`PDMTIN_IDX` DESC  ";

                        //    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        //    var DGV_D04_PLT = new List<SuperResultTranfer>();
                        //    for (int i = 0; i < ds.Tables.Count; i++)
                        //    {
                        //        DataTable dt = ds.Tables[i];
                        //        DGV_D04_PLT.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        //    }

                        //    if (DGV_D04_PLT.Count > 0)
                        //    {
                        //        var II = 0;
                        //        var JJ = DGV_D04_PLT.Count - 1;
                        //        if (BaseParameter.RadioButton9 == true)
                        //        {
                        //            if (JJ >= 20)
                        //            {
                        //                JJ = 19;
                        //            }
                        //        }
                        //        if (BaseParameter.RadioButton10 == true)
                        //        {
                        //            if (JJ >= 6)
                        //            {
                        //                JJ = 6;
                        //            }
                        //        }
                        //        StringBuilder HTMLContent = new StringBuilder();
                        //        StringBuilder HTMLD04Report = new StringBuilder();
                        //        StringBuilder D04Report_AIR = new StringBuilder();
                        //        StringBuilder D04Report_H = new StringBuilder();
                        //        var PARTGRP = "";
                        //        var PALLLETNO = "";
                        //        var PARTNAME = "";
                        //        var PART_NM = "";
                        //        var SHIP_NO = "";
                        //        var PrintDate = DateTime.Now.ToString("dd/MM/yyyy");
                        //        var PrintTime = DateTime.Now.ToString("HH:mm:ss");
                        //        for (II = 0; II <= JJ; II++)
                        //        {
                        //            var AAA = DGV_D04_PLT[II].PART_GRP;
                        //            var BBB = DGV_D04_PLT[II].PO_CODE;
                        //            var CCC = DGV_D04_PLT[II].QTY;
                        //            var DDD = DGV_D04_PLT[II].BOX_QTY;
                        //            var EEE = DGV_D04_PLT[II].PLET_NO;
                        //            var FFF = DGV_D04_PLT[II].PART_NO;
                        //            var GGG = DGV_D04_PLT[II].PART_SNP;
                        //            var HHH = DGV_D04_PLT[II].PART_NAME;
                        //            var KKK = DGV_D04_PLT[II].PART_NM;
                        //            var LLL = DGV_D04_PLT[II].SHIP_NO;

                        //            PARTGRP = AAA;
                        //            PALLLETNO = EEE;
                        //            PARTNAME = HHH;
                        //            PART_NM = KKK;
                        //            SHIP_NO = LLL;

                        //            if (KKK.Length > 7)
                        //            {
                        //                KKK = KKK.Substring(0, 7);
                        //            }
                        //            if (PART_NM.Length > 7)
                        //            {
                        //                PART_NM = PART_NM.Substring(0, 7);
                        //            }

                        //            int NO = II + 1;
                        //            HTMLD04Report.AppendLine(@"<tr>");
                        //            HTMLD04Report.AppendLine(@"<td style='text-align: center; font-size: 30px; font-weight: bold;'>" + NO + "</td>");
                        //            HTMLD04Report.AppendLine(@"<td style='text-align: center; font-size: 30px; font-weight: bold;'>" + HHH + "</td>");
                        //            HTMLD04Report.AppendLine(@"<td style='text-align: center; font-size: 30px; font-weight: bold;'>" + FFF + "</td>");
                        //            HTMLD04Report.AppendLine(@"<td style='text-align: right; font-size: 30px; font-weight: bold;'>" + GGG + "</td>");
                        //            HTMLD04Report.AppendLine(@"<td style='text-align: right; font-size: 30px; font-weight: bold;'>" + DDD + "</td>");
                        //            HTMLD04Report.AppendLine(@"<td style='text-align: right; font-size: 30px; font-weight: bold;'>" + CCC + "</td>");
                        //            HTMLD04Report.AppendLine(@"<td style='text-align: center; font-size: 30px; font-weight: bold;'>" + BBB + "</td>");
                        //            HTMLD04Report.AppendLine(@"</tr>");

                        //            D04Report_AIR.AppendLine(@"<tr>");
                        //            D04Report_AIR.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + NO + "</td>");
                        //            D04Report_AIR.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + FFF + "</td>");
                        //            D04Report_AIR.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + KKK + "</td>");
                        //            D04Report_AIR.AppendLine(@"<td style='text-align: right; font-size: 100px; font-weight: bold;'>" + CCC + "</td>");
                        //            D04Report_AIR.AppendLine(@"<td style='text-align: right; font-size: 100px; font-weight: bold;'>" + DDD + "</td>");
                        //            D04Report_AIR.AppendLine(@"</tr>");

                        //            D04Report_H.AppendLine(@"<tr>");
                        //            D04Report_H.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;' rowspan='2'>" + NO + "</td>");
                        //            D04Report_H.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + HHH + "</td>");
                        //            D04Report_H.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + CCC + "</td>");
                        //            D04Report_H.AppendLine(@"</tr>");
                        //            D04Report_H.AppendLine(@"<tr>");
                        //            D04Report_H.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + FFF + "</td>");
                        //            D04Report_H.AppendLine(@"<td style='text-align: center; font-size: 100px; font-weight: bold;'>" + DDD + "</td>");
                        //            D04Report_H.AppendLine(@"</tr>");

                        //        }
                        //        if (BaseParameter.RadioButton9 == true)
                        //        {
                        //            HTMLContent.AppendLine(GlobalHelper.CreateHTMLD04Report(SheetName, _WebHostEnvironment.WebRootPath, PARTGRP, PALLLETNO, HTMLD04Report.ToString(), PrintDate, PrintTime));
                        //            if (!string.IsNullOrEmpty(HTMLD04Report.ToString()))
                        //            {
                        //                string contentHTML = GlobalHelper.InitializationString;
                        //                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "EmptyLandscape.html");
                        //                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                        //                {
                        //                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        //                    {
                        //                        contentHTML = r.ReadToEnd();
                        //                    }
                        //                }
                        //                contentHTML = contentHTML.Replace(@"[Content]", HTMLContent.ToString());
                        //                string fileName = SheetName + GlobalHelper.InitializationDateTimeCode + ".html";
                        //                string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                        //                Directory.CreateDirectory(physicalPathCreate);
                        //                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                        //                string filePath = Path.Combine(physicalPathCreate, fileName);
                        //                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        //                {
                        //                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        //                    {
                        //                        await w.WriteLineAsync(contentHTML);
                        //                    }
                        //                }
                        //                result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        //            }
                        //        }
                        //        if (BaseParameter.RadioButton10 == true)
                        //        {
                        //            HTMLContent.AppendLine(GlobalHelper.CreateHTMLD04Report_H(SheetName, _WebHostEnvironment.WebRootPath, PART_NM, SHIP_NO, PALLLETNO, D04Report_H.ToString(), PrintDate, PrintTime));
                        //            if (!string.IsNullOrEmpty(HTMLD04Report.ToString()))
                        //            {
                        //                string contentHTML = GlobalHelper.InitializationString;
                        //                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "EmptyPortrait.html");
                        //                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                        //                {
                        //                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        //                    {
                        //                        contentHTML = r.ReadToEnd();
                        //                    }
                        //                }
                        //                contentHTML = contentHTML.Replace(@"[Content]", HTMLContent.ToString());
                        //                string fileName = SheetName + GlobalHelper.InitializationDateTimeCode + ".html";
                        //                string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                        //                Directory.CreateDirectory(physicalPathCreate);
                        //                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                        //                string filePath = Path.Combine(physicalPathCreate, fileName);
                        //                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        //                {
                        //                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        //                    {
                        //                        await w.WriteLineAsync(contentHTML);
                        //                    }
                        //                }
                        //                result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        //            }
                        //        }
                        //        if (BaseParameter.RadioButton14 == true)
                        //        {
                        //            HTMLContent.AppendLine(GlobalHelper.CreateHTMLD04Report_AIR(SheetName, _WebHostEnvironment.WebRootPath, PARTGRP, PARTNAME, PALLLETNO, D04Report_AIR.ToString(), PrintDate, PrintTime));
                        //            if (!string.IsNullOrEmpty(HTMLD04Report.ToString()))
                        //            {
                        //                string contentHTML = GlobalHelper.InitializationString;
                        //                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "EmptyPortrait.html");
                        //                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                        //                {
                        //                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        //                    {
                        //                        contentHTML = r.ReadToEnd();
                        //                    }
                        //                }
                        //                contentHTML = contentHTML.Replace(@"[Content]", HTMLContent.ToString());
                        //                string fileName = SheetName + GlobalHelper.InitializationDateTimeCode + ".html";
                        //                string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                        //                Directory.CreateDirectory(physicalPathCreate);
                        //                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                        //                string filePath = Path.Combine(physicalPathCreate, fileName);
                        //                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        //                {
                        //                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        //                    {
                        //                        await w.WriteLineAsync(contentHTML);
                        //                    }
                        //                }
                        //                result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
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
        public virtual async Task<BaseResult> PO_LIST_CB(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT tdpdotpl.PO_CODE, tdpdotpl.CREATE_DTM
                FROM tdpdotpl
                GROUP BY tdpdotpl.PO_CODE
                ORDER BY  tdpdotpl.PDOTPL_IDX DESC
                LIMIT " + BaseParameter.PageSize;

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.ComboBox2 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.ComboBox2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button3_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {

                if (BaseParameter != null)
                {
                    var CREATE_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var TextBoxA0 = BaseParameter.ListSearchString[0];
                        var TextBox6 = BaseParameter.ListSearchString[1];
                        var TextBoxA3 = BaseParameter.ListSearchString[2];
                        var TextBoxA2 = BaseParameter.ListSearchString[3];
                        var TextBoxA1 = BaseParameter.ListSearchString[4];
                        var TextBox22 = BaseParameter.ListSearchString[5];

                        if (string.IsNullOrEmpty(TextBoxA3))
                        {
                            TextBoxA3 = "0";
                        }
                        if (string.IsNullOrEmpty(TextBox22))
                        {
                            TextBox22 = "0";
                        }
                        var chk_text = TextBoxA0;
                        var PART_IDX = TextBoxA2;

                        if (chk_text == "NEW")
                        {
                            string sql = @"SELECT `PART_IDX`, `PART_SNP` FROM tspart WHERE PART_NO ='" + PART_IDX + "'";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            var Search = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            if (Search.Count > 0)
                            {
                                var PART_IDXK = Search[0].PART_IDX;
                                var PO_CODE = TextBoxA1.Trim();
                                var PO_QTY = int.Parse(TextBoxA3);
                                var NEXT_QTY = int.Parse(TextBox22);
                                var PART_SNP = Search[0].PART_SNP;

                                sql = @"SELECT  tdpdotpl.PDOTPL_IDX, tdpdotpl.PO_CODE, tdpdotpl.PART_IDX, tdpdotpl.PO_QTY, tdpdotpl.PACK_QTY 
                                FROM tdpdotpl WHERE tdpdotpl.PO_CODE = '" + PO_CODE + "'  AND  tdpdotpl.PART_IDX = '" + PART_IDXK + "'";

                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                var DGV_D04_PO01 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    DGV_D04_PO01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }

                                if (DGV_D04_PO01.Count > 0)
                                {
                                    sql = @"UPDATE tdpdotpl SET tdpdotpl.`PO_QTY` = '" + PO_QTY + "', tdpdotpl.`NT_QTY` = '" + NEXT_QTY + "'  WHERE tdpdotpl.`PO_CODE` = '" + PO_CODE + "' AND tdpdotpl.`PART_IDX` = '" + PART_IDXK + " '";
                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }
                                else
                                {
                                    sql = @"INSERT `tdpdotpl` (`PO_CODE`, `PART_IDX`, `PO_QTY`, `NT_QTY`, `PART_IDX_SNP`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + PO_CODE + "', '" + PART_IDXK + "', '" + PO_QTY + "', '" + NEXT_QTY + "', '" + PART_SNP + "', NOW(), '" + CREATE_USER + "') " +
                                              "  ON DUPLICATE KEY UPDATE `PO_QTY`= VALUES(`PO_QTY`), `NT_QTY`= VALUES(`NT_QTY`), `UPDATE_DTM` = NOW(),  `UPDATE_USER` = '" + CREATE_USER + "'";
                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                }

                            }
                        }
                        var chk_textNumber = int.Parse(chk_text);
                        if (chk_textNumber > 0)
                        {
                            var PO_CODE = TextBoxA0;
                            var PO_QTY = int.Parse(TextBoxA3);
                            var NEXT_QTY = int.Parse(TextBox22);
                            string sql = @"UPDATE `tdpdotpl` SET `PO_QTY`='" + PO_QTY + "', tdpdotpl.`NT_QTY` = '" + NEXT_QTY + "', `UPDATE_DTM`=NOW(), `UPDATE_USER`='" + CREATE_USER + "' WHERE  `PDOTPL_IDX`=" + PO_CODE;
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                    bool IsCheck = true;
                    bool PO_QTY_CHK = false;
                    var CREATE_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var AAA = BaseParameter.ListSearchString[0];
                        var MaskedTextBox1 = BaseParameter.ListSearchString[1];
                        string SQL9 = @"";
                        string SP_CHK = "N";
                        if (BaseParameter.CheckBox1 == true)
                        {
                            SQL9 = "SELECT IFNULL(`VLID_PART_IDX`, 0) AS `VLID_PART_IDX` , COUNT(`VLID_GRP`) AS `CONT`, `VLID_DTM`, `VLID_GRP`, `VLID_BARCODE` FROM  `tdpdmtim`  WHERE `VLID_DSCN_YN`= 'N'   AND `VLID_BARCODE` = '" + AAA + "'  ";
                            result.SP_CHK = "Y";
                        }
                        if (BaseParameter.CheckBox1 == false)
                        {
                            SQL9 = "SELECT IFNULL(`VLID_PART_IDX`, 0) AS `VLID_PART_IDX` , COUNT(`VLID_GRP`) AS `CONT`, `VLID_DTM`   FROM  `tdpdmtim`  WHERE `VLID_DSCN_YN`='N' AND `VLID_GRP` = '" + AAA + "'";
                            result.SP_CHK = "N";
                        }
                        string sql = SQL9;

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.COUNT_DGV = new List<SuperResultTranfer>();
                        result.COUNT_DGV_CHK = new List<SuperResultTranfer>();
                        result.DGV_D04_ALOC = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.COUNT_DGV.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.COUNT_DGV.Count > 0)
                        {
                            if (result.COUNT_DGV[0].VLID_PART_IDX == 0)
                            {
                                sql = @"SELECT IFNULL(`VLID_PART_IDX`, 0) AS `VLID_PART_IDX` , COUNT(`VLID_GRP`) AS `CONT`, `VLID_DTM`   FROM  `tdpdmtim`  WHERE `VLID_DSCN_YN`='Y' AND `VLID_GRP` = '" + AAA + "'";

                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.COUNT_DGV_CHK = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.COUNT_DGV_CHK.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                            }
                            var PART_IDX_LOC = result.COUNT_DGV[0].VLID_PART_IDX;
                            sql = @"SELECT tdpdotpl_ALOC.`D_ARRVL`   FROM  tdpdotpl_ALOC   WHERE tdpdotpl_ALOC.`PART_IDX` = '" + PART_IDX_LOC + "'   ";

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DGV_D04_ALOC = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DGV_D04_ALOC.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> Button2_Click01(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var CREATE_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var MaskedTextBox1 = BaseParameter.ListSearchString[0];
                        var ck_PNNO = int.Parse(BaseParameter.ListSearchString[1]);
                        string sql = @"SELECT `A`.PART_IDX , `A`.`PO_CODE`,(SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_NO`,
                        (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_GRP`,
                        (SELECT `PART_FML` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_NM`,
                        (`A`.`PART_IDX_SNP`) AS `PART_SNP`,
                        IFNULL(`PO_QTY`, 0) AS `PO_QTY`, 
                        IFNULL(`MZ`.`PN_QYT`, 0) AS `QTY`, 
                        IFNULL((SELECT `QTY` FROM tiivtr WHERE tiivtr.`PART_IDX` = `A`.`PART_IDX` AND tiivtr.LOC_IDX = '2'), 0) AS `STOCK`,
                        CEILING(IFNULL(`PO_QTY`, 0) / (`A`.`PART_IDX_SNP`)) AS `BOX_QTY`,
                        IFNULL(`MZ`.`PN_QYT`, 0 ) AS `PACK_QTY`, 
                        (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )) AS `Not_yet_packing`,
                        IFNULL((SELECT `QTY` FROM tiivtr WHERE tiivtr.`PART_IDX` = `A`.`PART_IDX` AND tiivtr.LOC_IDX = '2') - (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )), 0) AS `STATUS`,
                        `A`.`PDOTPL_IDX`,  '' AS `QTY1`
                        FROM tdpdotpl `A` LEFT JOIN

                        (SELECT COUNT(`ZZ`.`VLID_PART_IDX`) AS `PN_QYT`,
                        `ZZ`.`VLID_DSCN_YN`, `ZZ`.`PDOTPL_IDX`
                        FROM tdpdmtim `ZZ`
                        WHERE `ZZ`.`VLID_DSCN_YN` ='Y'
                        GROUP BY `ZZ`.`PDOTPL_IDX`, `ZZ`.`VLID_PART_IDX`) `MZ`

                        ON `A`.PDOTPL_IDX = `MZ`.`PDOTPL_IDX`

                        WHERE  (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )) <= 0 AND `A`.`PO_CODE` = '" + MaskedTextBox1 + "'  ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        var OUTLIST_99 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            OUTLIST_99.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        if (OUTLIST_99.Count > 0)
                        {
                            result.QTY_CHK = false;
                            for (int C_II = 0; C_II < OUTLIST_99.Count; C_II++)
                            {
                                var item = OUTLIST_99[C_II];
                                if (ck_PNNO == item.PART_IDX)
                                {
                                    result.QTY_CHK = true;
                                    C_II = OUTLIST_99.Count;
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
        public virtual async Task<BaseResult> DGV_OUT_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var CREATE_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var MaskedTextBox1 = BaseParameter.ListSearchString[0];
                        var TextBox10 = BaseParameter.ListSearchString[1];


                        var SQL_WHR = @" AND (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )) > 0 ";
                        if (BaseParameter.CheckBox3 == true)
                        {
                            SQL_WHR = "";
                        }
                        else
                        {
                            SQL_WHR = " AND (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )) > 0 ";
                        }


                        string sql = @"SELECT `A`.`PO_CODE`,(SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_NO`,
                        (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_GRP`,
                        (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_NM`,
                        (`A`.`PART_IDX_SNP`) AS `PART_SNP`,
                        IFNULL(`PO_QTY`, 0) AS `PO_QTY`, 
                        IFNULL(`NT_QTY`, 0) AS `NT_QTY`, 
                        IFNULL(`MZ`.`PN_QYT`, 0) AS `QTY`, 
                        IFNULL((SELECT `QTY` FROM tiivtr WHERE tiivtr.`PART_IDX` = `A`.`PART_IDX` AND tiivtr.LOC_IDX = '2'), 0) AS `STOCK`,
                        IFNULL(CEILING(IFNULL(`PO_QTY`, 0) / (`A`.`PART_IDX_SNP`)), 0) AS `BOX_QTY`,
                        IFNULL(`MZ`.`PN_QYT`, 0 ) AS `PACK_QTY`, 
                        (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )) AS `Not_yet_packing`,
                        IFNULL((SELECT `QTY` FROM tiivtr WHERE tiivtr.`PART_IDX` = `A`.`PART_IDX` AND tiivtr.LOC_IDX = '2') - (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )), 0) AS `STATUS`,
                        `A`.`PDOTPL_IDX`, `A`.PART_IDX , '' AS `QTY1`, IFNULL(`SORTNO`, 9) AS `SORTNO`
                        FROM tdpdotpl `A` LEFT JOIN

                        (SELECT COUNT(`ZZ`.`VLID_PART_IDX`) AS `PN_QYT`,
                        `ZZ`.`VLID_DSCN_YN`, `ZZ`.`PDOTPL_IDX`
                        FROM tdpdmtim `ZZ`
                        WHERE `ZZ`.`VLID_DSCN_YN` ='Y' 
                        GROUP BY `ZZ`.`PDOTPL_IDX`, `ZZ`.`VLID_PART_IDX`) `MZ`

                        ON `A`.PDOTPL_IDX = `MZ`.`PDOTPL_IDX`

                        WHERE `A`.`PO_CODE` = '" + MaskedTextBox1 + "' " + SQL_WHR + " HAVING `PART_NO` LIKE '%" + TextBox10 + "%'     ORDER BY `SORTNO` ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_OUT = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_OUT.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> DGV7_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var CREATE_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var SU01 = BaseParameter.ListSearchString[0];
                        var SU02 = BaseParameter.ListSearchString[1];
                        var SU03 = BaseParameter.ListSearchString[2];


                        string sql = @"SELECT 
                            tdpdmtim.`PDOTPL_IDX` AS `CODE`,
                            (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) AS `PO_CODE`,
                            (SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX) AS `PLET_NO`, 
                            `VLID_GRP` AS `SERIAL_ID`,
                            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NO`,
                            (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_GRP`,
                            (SELECT `PART_FML` FROM tspart WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX)) AS `PART_NAME`,
                            tdpdmtim.`VLID_PART_SNP` AS `PART_SNP`,
                            (SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.PDOTPL_IDX) AS `PO_QTY`, 
                            COUNT(tdpdmtim.`VLID_GRP`) AS `QTY`, 
                            IFNULL(CEILING(COUNT(tdpdmtim.`VLID_GRP`) / tdpdmtim.`VLID_PART_SNP`), 1) AS `BOX_QTY`,
                            ((SELECT tdpdotpl.`PO_QTY` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`) - COUNT(tdpdmtim.`VLID_GRP`)) AS `Not_yet_packing`,
                             COALESCE((SELECT `QTY` FROM tiivtr WHERE (`PART_IDX` = tdpdmtim.VLID_PART_IDX) AND `LOC_IDX` = '2'), 0) AS `Inventory`,
                            `TDPDOTPLMU_IDX` AS `TDPDOTPLMU_IDX`, 
                            `PDOTPL_IDX`,
                            (SELECT tsuser.USER_NM FROM tsuser WHERE tsuser.USER_ID = IFNULL(tdpdmtim.UPDATE_USER, tdpdmtim.CREATE_USER)) AS `Name`
                            FROM tdpdmtim

                            WHERE NOT(`TDPDOTPLMU_IDX` IS NULL) AND NOT(`PDOTPL_IDX` IS NULL) AND  
                            ((SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`)  LIKE '%" + SU02 + "%') AND(SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX) = '" + SU01 + "' AND `VLID_GRP` LIKE '%" + SU03 + "%' GROUP BY tdpdmtim.`VLID_GRP` ORDER BY tdpdmtim.`PDMTIN_IDX` DESC    ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView7 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView7.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> COMB_LIST(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var TextBox2 = BaseParameter.SearchString;
                    string sql = @"SELECT DISTINCT(SELECT trim(tdpdotplmu.PLET_NO) FROM tdpdotplmu WHERE tdpdotplmu.TDPDOTPLMU_IDX = tdpdmtim.TDPDOTPLMU_IDX) AS `PLET_NO`
                    FROM tdpdmtim WHERE NOT(`TDPDOTPLMU_IDX` IS NULL) AND NOT(`PDOTPL_IDX` IS NULL) AND  ((SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl WHERE tdpdotpl.`PDOTPL_IDX` = tdpdmtim.`PDOTPL_IDX`)  LIKE '%" + TextBox2 + "%') GROUP BY tdpdmtim.`VLID_GRP` ORDER BY `PLET_NO`";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DGV_D04_CB1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DGV_D04_CB1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button2_ClickOld(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    bool IsCheck = true;
                    bool PO_QTY_CHK = false;
                    var CREATE_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        if (BaseParameter.DGV_OUT != null)
                        {
                            if (BaseParameter.DGV_OUT.Count > 0)
                            {
                                if (BaseParameter.DataGridView1 != null)
                                {
                                    result.DGV_OUT = BaseParameter.DGV_OUT;
                                    result.DataGridView1 = BaseParameter.DataGridView1;
                                    var AAA = BaseParameter.ListSearchString[0];
                                    var MaskedTextBox1 = BaseParameter.ListSearchString[1];
                                    string SQL9 = @"";
                                    string SP_CHK = "N";
                                    if (BaseParameter.CheckBox1 == true)
                                    {
                                        SQL9 = "SELECT IFNULL(`VLID_PART_IDX`, 0) AS `VLID_PART_IDX` , COUNT(`VLID_GRP`) AS `CONT`, `VLID_DTM`, `VLID_GRP`, `VLID_BARCODE` FROM  `tdpdmtim`  WHERE `VLID_DSCN_YN`= 'N'   AND `VLID_BARCODE` = '" + AAA + "'  ";
                                        result.SP_CHK = "Y";
                                    }
                                    if (BaseParameter.CheckBox1 == false)
                                    {
                                        SQL9 = "SELECT IFNULL(`VLID_PART_IDX`, 0) AS `VLID_PART_IDX` , COUNT(`VLID_GRP`) AS `CONT`, `VLID_DTM`   FROM  `tdpdmtim`  WHERE `VLID_DSCN_YN`='N' AND `VLID_GRP` = '" + AAA + "'";
                                        result.SP_CHK = "N";
                                    }
                                    string sql = SQL9;

                                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    result.COUNT_DGV = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        result.COUNT_DGV.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }

                                    if (result.COUNT_DGV.Count > 0)
                                    {
                                        if (result.COUNT_DGV[0].VLID_PART_IDX == 0)
                                        {
                                            sql = @"SELECT IFNULL(`VLID_PART_IDX`, 0) AS `VLID_PART_IDX` , COUNT(`VLID_GRP`) AS `CONT`, `VLID_DTM`   FROM  `tdpdmtim`  WHERE `VLID_DSCN_YN`='Y' AND `VLID_GRP` = '" + AAA + "'";

                                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                            result.COUNT_DGV_CHK = new List<SuperResultTranfer>();
                                            for (int i = 0; i < ds.Tables.Count; i++)
                                            {
                                                DataTable dt = ds.Tables[i];
                                                result.COUNT_DGV_CHK.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                            }
                                            IsCheck = false;
                                        }
                                    }
                                    if (IsCheck == true)
                                    {
                                        var PART_IDX_LOC = 0;
                                        PART_IDX_LOC = result.COUNT_DGV[0].VLID_PART_IDX.Value;

                                        sql = @"SELECT tdpdotpl_ALOC.`D_ARRVL`   FROM  tdpdotpl_ALOC   WHERE tdpdotpl_ALOC.`PART_IDX` = '" + PART_IDX_LOC + "'   ";

                                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                        result.DGV_D04_ALOC = new List<SuperResultTranfer>();
                                        for (int i = 0; i < ds.Tables.Count; i++)
                                        {
                                            DataTable dt = ds.Tables[i];
                                            result.DGV_D04_ALOC.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                        }
                                        var ALOC = "";
                                        if (result.DGV_D04_ALOC.Count <= 0)
                                        {
                                            ALOC = "ALL";
                                        }
                                        else
                                        {
                                            ALOC = result.DGV_D04_ALOC[0].D_ARRVL.Substring(0, 2);
                                        }
                                        result.PO_LOC = MaskedTextBox1.Substring(0, 2);
                                        result.LOC_CHK = false;
                                        switch (result.PO_LOC)
                                        {
                                            case "BU":
                                                if (ALOC == result.PO_LOC)
                                                {
                                                    result.LOC_CHK = true;
                                                }
                                                else
                                                {
                                                    result.LOC_CHK = false;
                                                }
                                                break;
                                            case "IN":
                                                if (ALOC == result.PO_LOC)
                                                {
                                                    result.LOC_CHK = true;
                                                }
                                                else
                                                {
                                                    result.LOC_CHK = false;
                                                }
                                                break;
                                        }
                                        if (result.LOC_CHK == false)
                                        {
                                            var PO_LOC_CHK = false;
                                            if (PO_LOC_CHK == false)
                                            {
                                                IsCheck = false;
                                            }
                                        }
                                    }
                                    if (IsCheck == true)
                                    {
                                        var ck_PNNO = result.COUNT_DGV[0].VLID_PART_IDX;
                                        var Part_no = false;
                                        for (int ii = 0; ii < result.DGV_OUT.Count; ii++)
                                        {
                                            var item = result.DGV_OUT[ii];
                                            var ckCODE = item.PDOTPL_IDX;
                                            var ckPART = item.PART_NO;
                                            var ckLINE = item.PART_GRP;
                                            result.ckAA = item.PART_IDX;
                                            result.qtyAA = item.PO_QTY - (int)item.QTY;
                                            var qtyBB = item.QTY;
                                            result.ckBB = result.COUNT_DGV[0].VLID_PART_IDX;
                                            var QTY = result.COUNT_DGV[0].CONT;
                                            var QTY_ch = item.QTY1;

                                            result.TOT_QTY = QTY_ch + QTY;
                                            if (result.ckAA == result.ckBB)
                                            {
                                                PO_QTY_CHK = false;
                                                if (result.TOT_QTY <= result.qtyAA)
                                                {
                                                    PO_QTY_CHK = true;
                                                }
                                                else
                                                {
                                                    PO_QTY_CHK = false;
                                                }
                                                if (PO_QTY_CHK == true)
                                                {
                                                    result.DGV_OUT[ii].QTY1 = result.TOT_QTY;
                                                    result.S_CHKPART = false;
                                                    var S_QTY = 1;
                                                    for (int S_II = 0; S_II < result.DataGridView1.Count; S_II++)
                                                    {
                                                        var S_AA = result.DataGridView1[S_II].DGV1_QTY;
                                                        var S_BB = result.DataGridView1[S_II].DGV1_BOXC;
                                                        var DGV1_PARTNO = result.DataGridView1[S_II].DGV1_PARTNO;
                                                        if (ckPART == DGV1_PARTNO)
                                                        {
                                                            result.S_CHKPART = true;
                                                            result.DataGridView1[S_II].DGV1_QTY = S_AA + QTY;
                                                            result.DataGridView1[S_II].DGV1_BOXC = S_BB + 1;
                                                            S_II = result.DataGridView1.Count;
                                                        }
                                                    }
                                                    if (result.S_CHKPART == false)
                                                    {
                                                        SuperResultTranfer DataGridView1Item = new SuperResultTranfer();
                                                        DataGridView1Item.PART_NO = ckPART;
                                                        DataGridView1Item.PART_LINE = ckLINE;
                                                        DataGridView1Item.QTY = QTY;
                                                        DataGridView1Item.BOX_COUNT = S_QTY;
                                                        result.DataGridView1.Add(DataGridView1Item);
                                                    }
                                                    Part_no = true;
                                                    ii = result.DGV_OUT.Count;
                                                }
                                                else
                                                {
                                                    IsCheck = false;
                                                }
                                            }
                                        }

                                        if (IsCheck == true)
                                        {
                                            if (Part_no == false)
                                            {
                                                sql = @"SELECT `A`.PART_IDX , `A`.`PO_CODE`,(SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_NO`,
                                                    (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_GRP`,
                                                    (SELECT `PART_FML` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_NM`,
                                                    (`A`.`PART_IDX_SNP`) AS `PART_SNP`,
                                                    IFNULL(`PO_QTY`, 0) AS `PO_QTY`, 
                                                    IFNULL(`MZ`.`PN_QYT`, 0) AS `QTY`, 
                                                    IFNULL((SELECT `QTY` FROM tiivtr WHERE tiivtr.`PART_IDX` = `A`.`PART_IDX` AND tiivtr.LOC_IDX = '2'), 0) AS `STOCK`,
                                                    CEILING(IFNULL(`PO_QTY`, 0) / (`A`.`PART_IDX_SNP`)) AS `BOX_QTY`,
                                                    IFNULL(`MZ`.`PN_QYT`, 0 ) AS `PACK_QTY`, 
                                                    (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )) AS `Not_yet_packing`,
                                                    IFNULL((SELECT `QTY` FROM tiivtr WHERE tiivtr.`PART_IDX` = `A`.`PART_IDX` AND tiivtr.LOC_IDX = '2') - (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )), 0) AS `STATUS`,
                                                    `A`.`PDOTPL_IDX`,  '' AS `QTY1`
                                                    FROM tdpdotpl `A` LEFT JOIN

                                                    (SELECT COUNT(`ZZ`.`VLID_PART_IDX`) AS `PN_QYT`,
                                                    `ZZ`.`VLID_DSCN_YN`, `ZZ`.`PDOTPL_IDX`
                                                    FROM tdpdmtim `ZZ`
                                                    WHERE `ZZ`.`VLID_DSCN_YN` ='Y'
                                                    GROUP BY `ZZ`.`PDOTPL_IDX`, `ZZ`.`VLID_PART_IDX`) `MZ`

                                                    ON `A`.PDOTPL_IDX = `MZ`.`PDOTPL_IDX`

                                                    WHERE  (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )) <= 0 AND `A`.`PO_CODE` = '" + MaskedTextBox1 + "'  ";

                                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                                var OUTLIST_99 = new List<SuperResultTranfer>();
                                                for (int i = 0; i < ds.Tables.Count; i++)
                                                {
                                                    DataTable dt = ds.Tables[i];
                                                    OUTLIST_99.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                                }
                                                if (OUTLIST_99.Count > 0)
                                                {
                                                    result.QTY_CHK = false;
                                                    for (int C_II = 0; C_II < OUTLIST_99.Count; C_II++)
                                                    {
                                                        var item = OUTLIST_99[C_II];
                                                        if (ck_PNNO == item.PART_IDX)
                                                        {
                                                            result.QTY_CHK = true;
                                                            C_II = OUTLIST_99.Count;
                                                        }
                                                    }
                                                }
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
    }
}

