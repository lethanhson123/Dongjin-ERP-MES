namespace MESService.Implement
{
    public class C04Service : BaseService<torderlist, ItorderlistRepository>
    , IC04Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C04Service(ItorderlistRepository torderlistRepository
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
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = "%" + BaseParameter.ListSearchString[0] + "%";
                            var BBB = "%" + BaseParameter.ListSearchString[1] + "%";
                            var CCC = "%" + BaseParameter.ListSearchString[2] + "%";
                            var DDD = DateTime.Parse(BaseParameter.ListSearchString[3]).ToString("yyyy-MM-dd");
                            var EEE = "%" + BaseParameter.ListSearchString[4] + "%";
                            if (EEE == "%ALL%")
                            {
                                EEE = "%%";
                            }
                            string sql = @"SELECT IFNULL(c.torderlist_work_MC, 'NOT') AS `STATUS`, a.ORDER_IDX AS `CODE`, a.OR_NO AS ORDER_NO, a.WORK_WEEK, a.`CONDITION`, a.LEAD_NO, a.PROJECT,a.TOT_QTY AS PO_QTY, a.PERFORMN AS WORK_QTY,
                            a.DT AS PO_DATE, IFNULL(a.`MC2`, a.`MC`) AS `MC`, a.BUNDLE_SIZE, b.HOOK_RACK, a.WIRE, a.`T1_DIR`, a.`TERM1`, a.`STRIP1`, a.`SEAL1`, a.`CCH_W1`, a.`ICH_W1`, 
                            a.`T2_DIR`, a.`TERM2`, a.`STRIP2`, a.`SEAL2`, a.`CCH_W2`, a.`ICH_W2`, a.`SP_ST`, a.`REP`
		                       FROM TORDERLIST a JOIN trackmaster b ON a.`LEAD_NO` = b.`LEAD_NO` LEFT JOIN torderlist_work c ON a.ORDER_IDX = c.TORDERLIST_WORK_ORDERIDX
		                      WHERE (IFNULL(`MC2`, `MC`) LIKE '" + AAA + "') AND (a.`LEAD_NO` LIKE '" + BBB + "') AND (`PROJECT` LIKE '" + CCC + "') AND (`DT` >= '" + DDD + "') AND (`CONDITION` LIKE '" + EEE + "') AND (c.TORDERLIST_WORK_MC IS NULL OR c.TORDERLIST_WORK_MC LIKE '%A%')  ORDER BY a.DT, a.LEAD_NO ";

                            //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var TextBox5 = BaseParameter.ListSearchString[0];
                            var FFF = "";
                            if (BaseParameter.RadioButton1 == true)
                            {
                                FFF = "WHERE LENGTH(`DB_AA`.`LEAD_NM`) > 0 AND `DB_AA`.`LEAD_NM` = '" + TextBox5 + "' GROUP BY `DB_AA`.`LEAD_NM` ORDER BY `Stock_status` ASC,  `CHK_QTY` ASC , `DB_AA`.`LEAD_NM`";
                            }
                            if (BaseParameter.RadioButton2 == true)
                            {
                                FFF = "WHERE LENGTH(`DB_AA`.`LEAD_NM`) > 0 AND `DB_AA`.`HOOK_RACK`= '" + TextBox5 + "' GROUP BY `DB_AA`.`LEAD_NM` ORDER BY `Stock_status` ASC,  `CHK_QTY` ASC , `DB_AA`.`LEAD_NM`";
                            }
                            string sql = @"SELECT 
                            `DB_AA`.`HOOK_RACK`,  
                            `DB_AA`.`LEAD_NM`,  
                            IFNULL(`STOCKDB`.`STOCK_QTY`, 0) AS `STOCK_QTY`,
                            `DB_AA`.`IN_QTY`,  
                            `DB_AA`.`OUT_QTY`,
                            IF(((`DB_AA`.`IN_QTY` - `DB_AA`.`OUT_QTY`) - (IFNULL(`STOCKDB`.`STOCK_QTY`, 0) - IFNULL(`STOCKDB`.`ADJ_QTY`, 0))) =0, 'Good', 'Bad') AS `Stock_status`, 
                            ((`DB_AA`.`IN_QTY` - `DB_AA`.`OUT_QTY`) - (IFNULL(`STOCKDB`.`STOCK_QTY`, 0) - IFNULL(`STOCKDB`.`ADJ_QTY`, 0))) AS `CHK_QTY`
                            FROM (SELECT 
                            (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `HOOK_RACK`, 
                            trackmtim.`LEAD_NM`, (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = trackmtim.`LEAD_NM`) AS `LD_IDX`,
                            SUM(IF(trackmtim.`RACKIN_YN`='Y', trackmtim.QTY, 0)) AS `IN_QTY`, 
                            SUM(IF(trackmtim.`RACKOUT_YN`='Y', trackmtim.QTY, 0)) AS `OUT_QTY`
                            FROM trackmtim 
                            GROUP BY trackmtim.`LEAD_NM`) `DB_AA` 
                            LEFT JOIN  (SELECT  
                            tiivtr_lead.`PART_IDX`, IFNULL(tiivtr_lead.`QTY`, 0) AS `STOCK_QTY`, IFNULL(tiivaj_LEAD.`ADJ_QTY`, 0) AS `ADJ_QTY`
                            FROM tiivtr_lead LEFT JOIN tiivaj_LEAD
                            ON tiivtr_lead.`PART_IDX` = tiivaj_LEAD.`PART_IDX` AND tiivtr_lead.`LOC_IDX` = tiivaj_LEAD.`ADJ_SCN`
                            WHERE tiivtr_lead.`LOC_IDX` = '3') `STOCKDB`
                            ON  `DB_AA`.`LD_IDX` = `STOCKDB`.`PART_IDX`  " + FFF;

                            // sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView3 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            result.DataGridView4 = new List<SuperResultTranfer>();
                            if (result.DataGridView3.Count > 0)
                            {
                                var DataGridView3 = result.DataGridView3[0].LEAD_NM;
                                sql = @"SELECT 
                                (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `HOOK_RACK`, 
                                trackmtim.`LEAD_NM` AS `LEAD_NO`,
                                trackmtim.`BARCODE_NM` AS `BARCODE_NAME`, 
                                SUBSTRING_INDEX (trackmtim.`BARCODE_NM`, '$$',-1) AS `SEQ` ,
                                trackmtim.`QTY`, 
                                trackmtim.`RACKCODE` AS `STATUS`,
                                trackmtim.`RACKDTM` AS `IN_DATE`, 
                                trackmtim.`RACKOUT_DTM` AS `OUT_DATE`
                                 FROM trackmtim WHERE trackmtim.`LEAD_NM` = '" + DataGridView3 + "' ORDER BY trackmtim.`TRACK_IDX` DESC LIMIT 300";

                                //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView4 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = "%" + BaseParameter.ListSearchString[0] + "%";
                            var BBB = "%" + BaseParameter.ListSearchString[1] + "%";
                            var CCC = "%" + BaseParameter.ListSearchString[2] + "%";
                            var DDD = DateTime.Parse(BaseParameter.ListSearchString[3]).ToString("yyyy-MM-dd");
                            var EEE = "%" + BaseParameter.ListSearchString[4] + "%";
                            if (EEE == "%ALL%")
                            {
                                EEE = "%%";
                            }
                            string sql = @"SELECT 
                            `B`. `ORDER_IDX` AS `CODE`, `A`.`OR_NO` AS `ORDER_NO`, `A`.`WORK_WEEK`, `B`.`CONDITION`, `A`.`LEAD_NO`, `A`.`PROJECT`, `B`.`TOT_QTY` AS `PO_QTY`,
                            `A`.`ADJ_AF_QTY`, `A`.`DT` AS `PO_DATE`, IFNULL(`A`.`MC`, '-') AS `MC`, 
                            `A`.`TERM1`, `B`.`PERFORMN_L` AS `T1_W_QTY`, `A`.`TERM2`, `B`.`PERFORMN_R` AS `T2_W_QTY`, 
                            `A`.`BUNDLE_SIZE`, (SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.`LEAD_NO` = `A`.`LEAD_NO`) AS `HOOK_RACK`, `A`.`WIRE`, 
                            `A`.`T1_DIR`, `A`.`TERM1`, `A`.`STRIP1`, `A`.`SEAL1`, `A`.`CCH_W1`, `A`.`ICH_W1`, 
                            `A`.`T2_DIR`, `A`.`TERM2`, `A`.`STRIP2`, `A`.`SEAL2`, `A`.`CCH_W2`, `A`.`ICH_W2`, `A`.`SP_ST`, `A`.`REP`
                            FROM  TORDERLIST `A` JOIN TORDERLIST_LP `B` ON `A`.`ORDER_IDX` = `B`.`ORDER_IDX`
                            WHERE (IFNULL(`A`.`MC`, '-') LIKE '" + AAA + "') AND (`A`.`LEAD_NO` LIKE '" + BBB + "') AND (`A`.`PROJECT` LIKE '" + CCC + "') AND (`A`.`DT` >= '" + DDD + "') AND (`B`.`CONDITION` LIKE '" + EEE + "')";

                            // sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView5 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView5.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 4)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = "%" + BaseParameter.ListSearchString[0] + "%";
                            var BBB = "%" + BaseParameter.ListSearchString[1] + "%";
                            var DDD = DateTime.Parse(BaseParameter.ListSearchString[2]).ToString("yyyy-MM-dd");
                            var EEE = "%" + BaseParameter.ListSearchString[3] + "%";
                            if (EEE == "%ALL%")
                            {
                                EEE = "%%";
                            }
                            string sql = @"SELECT
                            `A`.ORDER_IDX AS `CODE`, `A`.`OR_NO`, `A`.`WORK_WEEK`, `A`.`CONDITION`, `A`.`LEAD_NO`, `A`.`PO_QTY`,  `A`.`SAFTY_QTY`, `A`.`PERFORMN`, `A`.`PO_DT`, `A`.`MC`,  `A`.`LEAD_COUNT`, 
                            `A`.`BUNDLE_SIZE`, (SELECT trackmaster.HOOK_RACK FROM trackmaster WHERE trackmaster.`LEAD_NO` = `A`.`LEAD_NO`) AS `HOOK_RACK`
                            FROM TORDERLIST_SPST `A`
                            WHERE `A`.`PO_YN` = 'Y' AND (IFNULL(`A`.`MC`, '') LIKE '" + AAA + "') AND (`A`.`LEAD_NO` LIKE '" + BBB + "') AND (`A`.`PO_DT` >= '" + DDD + "') AND (`A`.`CONDITION` LIKE '" + EEE + "')";

                            // sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView7 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView7.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 6)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var DateBegin = DateTime.Parse(BaseParameter.ListSearchString[0]);
                            var DateEnd = DateTime.Parse(BaseParameter.ListSearchString[1]);
                            var DateBeginSub = DateEnd.AddDays(-15);
                            if (DateBegin < DateBeginSub)
                            {
                                DateBegin = DateBeginSub;
                            }
                            var DT1 = DateBegin.ToString("yyyy-MM-dd") + " 06:00:00";
                            var DT2 = DateEnd.AddDays(1).ToString("yyyy-MM-dd") + " 05:59:59";
                            var AAAA = "%" + BaseParameter.ListSearchString[2] + "%";
                            var BBBB = "%" + BaseParameter.ListSearchString[3] + "%";
                            var CCCC = "%" + BaseParameter.ListSearchString[4] + "%";

                            string sql = @"SELECT * FROM (
                                SELECT 'KOMAX' AS `STAGE`,A.TORDER_IDX, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, `A`.`PART_IDX` AS `LEAD_NO`, `A`.`MC_NO`, 
                                '' AS `TERM`, SUM(`A`.`WK_QTY`) AS `WK_QTY`,MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`, `A`.`CREATE_USER`,
                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `Name`, `A`.`CREATE_DTM` 
                                FROM TWWKAR `A` Where A.CREATE_DTM Between '" + DT1 + "' AND '" + DT2 + "' GROUP BY `A`.`PART_IDX`, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d') UNION SELECT 'LP' AS `STAGE`,B.TORDER_IDX, DATE_FORMAT(`B`.`CREATE_DTM`, '%Y-%m-%d')  AS `DATE`, `B`.`PART_IDX`, `B`.`MC_NO`, IFNULL(`B`.`WK_TERM`, '') AS `TERM`, SUM(`B`.`WK_QTY`) AS `WK_QTY`, MIN(`B`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`B`.`CREATE_DTM`) AS `END_TIME`, `B`.`CREATE_USER`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `B`.`CREATE_USER`) AS `Name`, `B`.`CREATE_DTM` FROM TWWKAR_LP `B`  Where B.CREATE_DTM Between '" + DT1 + "' AND '" + DT2 + "' GROUP BY `B`.`PART_IDX`, DATE_FORMAT(`B`.`CREATE_DTM`, '%Y-%m-%d') UNION SELECT 'SP/ST' AS `STAGE`, C.TORDER_IDX, DATE_FORMAT(`C`.`CREATE_DTM`, '%Y-%m-%d') AS `DATE`, `C`.`PART_IDX`, `C`.`MC_NO`, '' AS `TERM`, SUM(`C`.`WK_QTY`) AS `WK_QTY`, MIN(`C`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`C`.`CREATE_DTM`) AS `END_TIME`, `C`.`CREATE_USER`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `C`.`CREATE_USER`) AS `Name`, `C`.`CREATE_DTM` FROM TWWKAR_SPST `C`   Where C.CREATE_DTM Between '" + DT1 + "' AND '" + DT2 + "' GROUP BY `C`.`PART_IDX`, DATE_FORMAT(`C`.`CREATE_DTM`, '%Y-%m-%d')) AS `MAIN` WHERE  `MAIN`.`LEAD_NO` LIKE '" + AAAA + "'  AND  `MAIN`.`MC_NO` LIKE '" + BBBB + "' AND   `MAIN`.`STAGE` LIKE '" + CCCC + "' ORDER BY  `MAIN`.`DATE`, `MAIN`.`LEAD_NO`";

                            // sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView9 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView9.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }


                            sql = @"SELECT COUNT(DISTINCT `MAIN`.`STAGE`) AS `COL1` , COUNT(DISTINCT  `MAIN`.`PART_IDX`) AS `COL2`, 
                                                 COUNT(DISTINCT `MAIN`.`MC_NO`) AS `COL3`, SUM(`MAIN`.`WK_QTY`) AS `COL4`  FROM (
                                SELECT 'KOMAX' AS `STAGE`, `A`.`PART_IDX`, `A`.`MC_NO`, 
                                '' AS `TERM`, SUM(`A`.`WK_QTY`) AS `WK_QTY`,MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`, `A`.`CREATE_USER`,
                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `Name`, `A`.`CREATE_DTM` 
                                FROM TWWKAR `A` WHERE `A`.CREATE_DTM BETWEEN '" + DT1 + "' AND '" + DT2 + "' GROUP BY `A`.`PART_IDX` UNION SELECT 'LP' AS `STAGE`,  `B`.`PART_IDX`, `B`.`MC_NO`, IFNULL(`B`.`WK_TERM`, '') AS `TERM`, SUM(`B`.`WK_QTY`) AS `WK_QTY`, MIN(`B`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`B`.`CREATE_DTM`) AS `END_TIME`, `B`.`CREATE_USER`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `B`.`CREATE_USER`) AS `Name`, `B`.`CREATE_DTM` FROM TWWKAR_LP `B`  WHERE `B`.CREATE_DTM BETWEEN '" + DT1 + "' AND '" + DT2 + "' GROUP BY `B`.`PART_IDX` UNION SELECT 'SP/ST' AS `STAGE`, `C`.`PART_IDX`, `C`.`MC_NO`, '' AS `TERM`, SUM(`C`.`WK_QTY`) AS `WK_QTY`, MIN(`C`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`C`.`CREATE_DTM`) AS `END_TIME`, `C`.`CREATE_USER`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `C`.`CREATE_USER`) AS `Name`, `C`.`CREATE_DTM` FROM TWWKAR_SPST `C`  WHERE `C`.CREATE_DTM BETWEEN '" + DT1 + "' AND '" + DT2 + "' GROUP BY `C`.`PART_IDX`) AS `MAIN` WHERE  `MAIN`.`PART_IDX` LIKE '%" + AAAA + "%'  AND  `MAIN`.`MC_NO` LIKE '" + BBBB + "%' AND   `MAIN`.`STAGE` LIKE '%" + CCCC + "%' ORDER BY  `MAIN`.`PART_IDX`";

                            //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.Search = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                        }
                    }
                    if (BaseParameter.Action == 7)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var DT1 = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd") + " 06:00:00";
                            var DT2 = DateTime.Parse(BaseParameter.ListSearchString[1]).AddDays(1).ToString("yyyy-MM-dd") + " 06:00:00";
                            var AAAA = "%" + BaseParameter.ListSearchString[2] + "%";
                            var BBBB = "%" + BaseParameter.ListSearchString[3] + "%";


                            string sql = @"SELECT * FROM (
                                SELECT 'KOMAX' AS `STAGE`, `A`.`MC_NO`, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, COUNT(`A`.`PART_IDX`) AS `LEAD_COUNT`, 
                                SUM(`A`.`WK_QTY`) AS `WK_QTY`, MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR `A`
                                GROUP BY `A`.`MC_NO`, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d')

                                UNION
                                SELECT 'LP' AS `STAGE`, `B`.`MC_NO`,  DATE_FORMAT(`B`.`CREATE_DTM`,'%Y-%m-%d')  AS `DATE`, COUNT(`B`.`PART_IDX`) AS `LEAD_COUNT`, 
                                SUM(`B`.`WK_QTY`) AS `WK_QTY`, MIN(`B`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`B`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR_LP `B`
                                GROUP BY `B`.`MC_NO`, DATE_FORMAT(`B`.`CREATE_DTM`,'%Y-%m-%d')

                                UNION
                                SELECT 'SP/ST' AS `STAGE`, `C`.`MC_NO`, DATE_FORMAT(`C`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, COUNT(`C`.`PART_IDX`) AS `LEAD_COUNT`,  
                                SUM(`C`.`WK_QTY`) AS `WK_QTY`, MIN(`C`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`C`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR_SPST `C`
                                GROUP BY `C`.`MC_NO`, DATE_FORMAT(`C`.`CREATE_DTM`,'%Y-%m-%d')) AS `MAIN`

                                WHERE  `MAIN`.`MC_NO` LIKE '" + AAAA + "' AND `MAIN`.`END_TIME` >= '" + DT1 + "' AND `MAIN`.`END_TIME` <= '" + DT2 + "' AND   `MAIN`.`STAGE` LIKE '" + BBBB + "' ORDER BY  `MAIN`.`DATE`, `MAIN`.`MC_NO`";

                            //  sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView10 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView10.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }


                            sql = @"SELECT COUNT(DISTINCT `MAIN`.`STAGE`) AS `COL1` , COUNT(DISTINCT  `MAIN`.`MC_NO`) AS `COL2`, 
                                                 SUM(`MAIN`.`LEAD_COUNT`) AS `COL3`, SUM(`MAIN`.`WK_QTY`) AS `COL4`  FROM (
                                SELECT 'KOMAX' AS `STAGE`, `A`.`MC_NO`, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, COUNT(`A`.`PART_IDX`) AS `LEAD_COUNT`, 
                                SUM(`A`.`WK_QTY`) AS `WK_QTY`, MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR `A`
                                GROUP BY `A`.`MC_NO`, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d')

                                UNION
                                SELECT 'LP' AS `STAGE`, `B`.`MC_NO`,  DATE_FORMAT(`B`.`CREATE_DTM`,'%Y-%m-%d')  AS `DATE`, COUNT(`B`.`PART_IDX`) AS `LEAD_COUNT`, 
                                SUM(`B`.`WK_QTY`) AS `WK_QTY`, MIN(`B`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`B`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR_LP `B`
                                GROUP BY `B`.`MC_NO`, DATE_FORMAT(`B`.`CREATE_DTM`,'%Y-%m-%d')

                                UNION
                                SELECT 'SP/ST' AS `STAGE`, `C`.`MC_NO`, DATE_FORMAT(`C`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, COUNT(`C`.`PART_IDX`) AS `LEAD_COUNT`,  
                                SUM(`C`.`WK_QTY`) AS `WK_QTY`, MIN(`C`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`C`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR_SPST `C`
                                GROUP BY `C`.`MC_NO`, DATE_FORMAT(`C`.`CREATE_DTM`,'%Y-%m-%d')) AS `MAIN`

                                WHERE  `MAIN`.`MC_NO` LIKE '" + AAAA + "' AND `MAIN`.`END_TIME` >= '" + DT1 + "' AND `MAIN`.`END_TIME` <= '" + DT2 + "' AND   `MAIN`.`STAGE` LIKE '" + BBBB + "' ORDER BY  `MAIN`.`DATE`, `MAIN`.`MC_NO`";

                            //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.Search = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                        }
                    }
                    if (BaseParameter.Action == 8)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var DT1 = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd") + " 06:00:00";
                            var DT2 = DateTime.Parse(BaseParameter.ListSearchString[1]).AddDays(1).ToString("yyyy-MM-dd") + " 06:00:00";
                            var BBBB = "%" + BaseParameter.ListSearchString[2] + "%";
                            var CCCC = "%" + BaseParameter.ListSearchString[3] + "%";


                            string sql = @"SELECT * FROM (
                                SELECT 'KOMAX' AS `STAGE`, `A`.`CREATE_USER`, 
                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `Name`, 
                                DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, COUNT(`A`.`PART_IDX`) AS `LEAD_COUNT`, 
                                SUM(`A`.`WK_QTY`) AS `WK_QTY`,MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR `A`
                                GROUP BY `A`.`CREATE_USER`, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d')

                                UNION
                                SELECT 'LP' AS `STAGE`, `B`.`CREATE_USER`,  
                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `B`.`CREATE_USER`) AS `Name`,
                                DATE_FORMAT(`B`.`CREATE_DTM`,'%Y-%m-%d')  AS `DATE`, COUNT(`B`.`PART_IDX`) AS `LEAD_COUNT`, 
                                SUM(`B`.`WK_QTY`) AS `WK_QTY`, MIN(`B`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`B`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR_LP `B`
                                GROUP BY `B`.`CREATE_USER`, DATE_FORMAT(`B`.`CREATE_DTM`,'%Y-%m-%d')

                                UNION
                                SELECT 'SP/ST' AS `STAGE`, `C`.`CREATE_USER`, 
                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `C`.`CREATE_USER`) AS `Name`,
                                DATE_FORMAT(`C`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, COUNT(`C`.`PART_IDX`) AS `LEAD_COUNT`,  
                                SUM(`C`.`WK_QTY`) AS `WK_QTY`, MIN(`C`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`C`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR_SPST `C`
                                GROUP BY `C`.`CREATE_USER`, DATE_FORMAT(`C`.`CREATE_DTM`,'%Y-%m-%d')) AS `MAIN`

                                WHERE   `MAIN`.`END_TIME` >= '" + DT1 + "' AND `MAIN`.`END_TIME` <= '" + DT2 + "' AND   `MAIN`.`STAGE` LIKE '" + BBBB + "' HAVING  `MAIN`.`NAME` LIKE '" + CCCC + "' ORDER BY  `MAIN`.`DATE`, `MAIN`.`CREATE_USER`";

                            // sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView11 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView11.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }


                            sql = @"SELECT COUNT(DISTINCT `MAIN`.`STAGE`) AS `COL1` , COUNT(DISTINCT  `MAIN`.`CREATE_USER`) AS `COL2`, 
                                                 SUM(`MAIN`.`LEAD_COUNT`) AS `COL3`, SUM(`MAIN`.`WK_QTY`) AS `COL4`   FROM (
                                SELECT 'KOMAX' AS `STAGE`, `A`.`CREATE_USER`, 
                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `Name`, 
                                DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, COUNT(`A`.`PART_IDX`) AS `LEAD_COUNT`, 
                                SUM(`A`.`WK_QTY`) AS `WK_QTY`,MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR `A`
                                GROUP BY `A`.`CREATE_USER`, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d')

                                UNION
                                SELECT 'LP' AS `STAGE`, `B`.`CREATE_USER`,  
                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `B`.`CREATE_USER`) AS `Name`,
                                DATE_FORMAT(`B`.`CREATE_DTM`,'%Y-%m-%d')  AS `DATE`, COUNT(`B`.`PART_IDX`) AS `LEAD_COUNT`, 
                                SUM(`B`.`WK_QTY`) AS `WK_QTY`, MIN(`B`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`B`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR_LP `B`
                                GROUP BY `B`.`CREATE_USER`, DATE_FORMAT(`B`.`CREATE_DTM`,'%Y-%m-%d')

                                UNION
                                SELECT 'SP/ST' AS `STAGE`, `C`.`CREATE_USER`, 
                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `C`.`CREATE_USER`) AS `Name`,
                                DATE_FORMAT(`C`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, COUNT(`C`.`PART_IDX`) AS `LEAD_COUNT`,  
                                SUM(`C`.`WK_QTY`) AS `WK_QTY`, MIN(`C`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`C`.`CREATE_DTM`) AS `END_TIME`
                                FROM TWWKAR_SPST `C`
                                GROUP BY `C`.`CREATE_USER`, DATE_FORMAT(`C`.`CREATE_DTM`,'%Y-%m-%d')) AS `MAIN`

                                WHERE   `MAIN`.`END_TIME` >= '" + DT1 + "' AND `MAIN`.`END_TIME` <= '" + DT2 + "' AND   `MAIN`.`STAGE` LIKE '" + BBBB + "' AND (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `MAIN`.`CREATE_USER`) LIKE '" + CCCC + "' ORDER BY  `MAIN`.`DATE`, `MAIN`.`CREATE_USER`";

                            //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.Search = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                        }
                    }
                    if (BaseParameter.Action == 9)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var DT1 = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd") + " 06:00:00";
                            var DT2 = DateTime.Parse(BaseParameter.ListSearchString[1]).AddDays(1).ToString("yyyy-MM-dd") + " 05:50:50";
                            var AAA = "%" + BaseParameter.ListSearchString[2] + "%";
                            var BBB = "%" + BaseParameter.ListSearchString[3] + "%";
                            var CCC = "%" + BaseParameter.ListSearchString[4] + "%";


                            string sql = @"SELECT * FROM (
                            SELECT 'KOMAX' AS `STAGE`, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, `A`.`PART_IDX` AS `LEAD_NO`, `A`.`MC_NO`, 
                            HOUR(`A`.`CREATE_DTM`) AS `TIME`, (`A`.`WK_QTY`) AS `WK_QTY`, (`A`.`CREATE_DTM`) AS `FIRST_TIME`, 
                            (`A`.`CREATE_DTM`) AS `END_TIME`, `A`.`CREATE_USER`,
                            (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `Name`
                            FROM TWWKAR `A`

                            UNION
                            SELECT 'LP' AS `STAGE`, DATE_FORMAT(`B`.`CREATE_DTM`,'%Y-%m-%d')  AS `DATE`, `B`.`PART_IDX` AS `LEAD_NO`, `B`.`MC_NO`, 
                            HOUR(`B`.`CREATE_DTM`) AS `TIME`, (`B`.`WK_QTY`) AS `WK_QTY`, (`B`.`CREATE_DTM`) AS `FIRST_TIME`, 
                            (`B`.`CREATE_DTM`) AS `END_TIME`, `B`.`CREATE_USER`,
                            (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `B`.`CREATE_USER`) AS `Name`
                            FROM TWWKAR_LP `B`

                            UNION
                            SELECT 'SP/ST' AS `STAGE`, DATE_FORMAT(`C`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, `C`.`PART_IDX` AS `LEAD_NO`, `C`.`MC_NO`, 
                            HOUR(`C`.`CREATE_DTM`) AS `TIME`, (`C`.`WK_QTY`) AS `WK_QTY`, (`C`.`CREATE_DTM`) AS `FIRST_TIME`, 
                            (`C`.`CREATE_DTM`) AS `END_TIME`, `C`.`CREATE_USER`,
                            (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `C`.`CREATE_USER`) AS `Name`
                            FROM TWWKAR_SPST `C`  ) AS `MAIN`

                            WHERE  `MAIN`.`LEAD_NO` LIKE '%" + AAA + "%'  AND  `MAIN`.`MC_NO` LIKE '%" + BBB + "%' AND `MAIN`.`END_TIME` >= '" + DT1 + "' AND `MAIN`.`END_TIME` <= '" + DT2 + "' AND   `MAIN`.`STAGE` LIKE '%" + CCC + "%'   ";

                            //  sql = sql + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView12 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView12.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                await Task.Run(() => { });
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
                string SheetName = this.GetType().Name;
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 6)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var DateBegin = DateTime.Parse(BaseParameter.ListSearchString[0]);
                            var DateEnd = DateTime.Parse(BaseParameter.ListSearchString[1]);
                            var DateBeginSub = DateEnd.AddDays(-15);
                            if (DateBegin < DateBeginSub)
                            {
                            }
                            var DT1 = DateBegin.ToString("yyyy-MM-dd") + " 06:00:00";
                            var DT2 = DateEnd.AddDays(1).ToString("yyyy-MM-dd") + " 05:59:59";
                            var AAAA = "%" + BaseParameter.ListSearchString[2] + "%";
                            var BBBB = "%" + BaseParameter.ListSearchString[3] + "%";
                            var CCCC = "%" + BaseParameter.ListSearchString[4] + "%";

                            string sql = @"SELECT * FROM (
                                SELECT 'KOMAX' AS `STAGE`,A.TORDER_IDX, DATE_FORMAT(`A`.`CREATE_DTM`,'%Y-%m-%d') AS `DATE`, `A`.`PART_IDX` AS `LEAD_NO`, `A`.`MC_NO`, 
                                '' AS `TERM`, SUM(`A`.`WK_QTY`) AS `WK_QTY`,MIN(`A`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`A`.`CREATE_DTM`) AS `END_TIME`, `A`.`CREATE_USER`,
                                (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `A`.`CREATE_USER`) AS `Name`, `A`.`CREATE_DTM` 
                                FROM TWWKAR `A` Where A.CREATE_DTM Between '" + DT1 + "' AND '" + DT2 + "' GROUP BY `A`.`PART_IDX`, DATE_FORMAT(`A`.`CREATE_DTM`, '%Y-%m-%d') UNION SELECT 'LP' AS `STAGE`,B.TORDER_IDX, DATE_FORMAT(`B`.`CREATE_DTM`, '%Y-%m-%d')  AS `DATE`, `B`.`PART_IDX`, `B`.`MC_NO`, IFNULL(`B`.`WK_TERM`, '') AS `TERM`, SUM(`B`.`WK_QTY`) AS `WK_QTY`, MIN(`B`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`B`.`CREATE_DTM`) AS `END_TIME`, `B`.`CREATE_USER`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `B`.`CREATE_USER`) AS `Name`, `B`.`CREATE_DTM` FROM TWWKAR_LP `B`  Where B.CREATE_DTM Between '" + DT1 + "' AND '" + DT2 + "' GROUP BY `B`.`PART_IDX`, DATE_FORMAT(`B`.`CREATE_DTM`, '%Y-%m-%d') UNION SELECT 'SP/ST' AS `STAGE`, C.TORDER_IDX, DATE_FORMAT(`C`.`CREATE_DTM`, '%Y-%m-%d') AS `DATE`, `C`.`PART_IDX`, `C`.`MC_NO`, '' AS `TERM`, SUM(`C`.`WK_QTY`) AS `WK_QTY`, MIN(`C`.`CREATE_DTM`) AS `FIRST_TIME`, MAX(`C`.`CREATE_DTM`) AS `END_TIME`, `C`.`CREATE_USER`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = `C`.`CREATE_USER`) AS `Name`, `C`.`CREATE_DTM` FROM TWWKAR_SPST `C`   Where C.CREATE_DTM Between '" + DT1 + "' AND '" + DT2 + "' GROUP BY `C`.`PART_IDX`, DATE_FORMAT(`C`.`CREATE_DTM`, '%Y-%m-%d')) AS `MAIN` WHERE  `MAIN`.`LEAD_NO` LIKE '" + AAAA + "'  AND  `MAIN`.`MC_NO` LIKE '" + BBBB + "' AND   `MAIN`.`STAGE` LIKE '" + CCCC + "' ORDER BY  `MAIN`.`DATE`, `MAIN`.`LEAD_NO`";

                            // sql = sql + " LIMIT " + GlobalHelper.ListCount;
                            string fileName = SheetName + "-GroupLead-" + DateBegin.ToString("yyyyMMdd") + "-" + DateEnd.ToString("yyyyMMdd") + ".xlsx";
                            string downloadPath = Path.Combine(GlobalHelper.Download, SheetName, fileName).Replace("\\", "/");
                            result.Code = $"{GlobalHelper.URLSite}/{downloadPath}";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView9 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView9.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            result.Code = GlobalHelper.InitializationString;
                            if (result.DataGridView9.Count > 0)
                            {
                                using (var streamExport = new MemoryStream())
                                {
                                    InitializationToExcelAsync(result.DataGridView9, streamExport, SheetName);
                                    string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                                    Directory.CreateDirectory(physicalPath);
                                    GlobalHelper.DeleteFilesByPath(physicalPath);
                                    string filePath = Path.Combine(physicalPath, fileName);
                                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                                    {
                                        streamExport.Position = 0;
                                        await streamExport.CopyToAsync(fileStream);
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
        public virtual async Task<BaseResult> DataGridView1_SelectionChanged(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var AAA = BaseParameter.SearchString;
                    string sql = @"SELECT  `TORDER_BARCODE_IDX` AS `CODE`, `TORDER_BARCODENM` AS `BARCODE_NAME`, `Barcode_SEQ`,TrolleyCode,`WORK_END`,`UPDATE_USER`, 
                    (SELECT tsuser.USER_NM FROM tsuser WHERE tsuser.USER_ID = TORDER_BARCODE.UPDATE_USER) AS `USER_NAME` 
                    FROM  TORDER_BARCODE   WHERE  `ORDER_IDX` = '" + AAA + "' ORDER BY `Barcode_SEQ`   ";

                    //sql = sql + " LIMIT " + GlobalHelper.ListCount;

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
        public virtual async Task<BaseResult> DataGridView5_SelectionChanged(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var AAA = BaseParameter.SearchString;
                    string sql = @"SELECT  `TORDER_BARCODE_IDX` AS `CODE`, `TORDER_BARCODENM` AS `BARCODE_NAME`, `Barcode_SEQ`,  `WORK_END`,`UPDATE_USER`, 
                    (SELECT tsuser.USER_NM FROM tsuser WHERE tsuser.`USER_ID` = torder_barcode_lp.`UPDATE_USER`) AS `USER_NAME` 
                    FROM  torder_barcode_lp   WHERE  `ORDER_IDX` = '" + AAA + "'";

                    //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView6 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView6.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> DataGridView8_SelectionChanged(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var AAA = BaseParameter.SearchString;
                    string sql = @"SELECT  `TORDER_BARCODE_IDX` AS `CODE`, `TORDER_BARCODENM` AS `BARCODE_NAME`, `Barcode_SEQ`,  `WORK_END`,`UPDATE_USER`, 
                    (SELECT tsuser.USER_NM FROM tsuser WHERE tsuser.USER_ID = TORDER_BARCODE_SP.`UPDATE_USER`) AS `USER_NAME` 
                    FROM  TORDER_BARCODE_SP   WHERE  `ORDER_IDX` = '" + AAA + "'";

                    //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView8 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView8.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttonexport_ClickSub(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string SheetName = this.GetType().Name;
                string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                Directory.CreateDirectory(physicalPath);
                GlobalHelper.DeleteFilesByPath(physicalPath);

                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 6)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var DateBegin = DateTime.Parse(BaseParameter.ListSearchString[0]);
                            var DateEnd = DateTime.Parse(BaseParameter.ListSearchString[1]);

                            string fileName = SheetName + "-GroupLead-" + DateBegin.ToString("yyyyMMdd") + "-" + DateEnd.ToString("yyyyMMdd") + ".xlsx";
                            string downloadPath = Path.Combine(GlobalHelper.Download, SheetName, fileName).Replace("\\", "/");
                            result.Code = $"{GlobalHelper.URLSite}/{downloadPath}";
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
        private void InitializationToExcelAsync(List<SuperResultTranfer> list, MemoryStream streamExport, string SheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(SheetName);
                int row = 1;
                int column = 1;
                workSheet.Cells[1, column].Value = "Stage";
                column = column + 1;
                workSheet.Cells[1, column].Value = "Order IDX";
                column = column + 1;
                workSheet.Cells[1, column].Value = "DATE";
                column = column + 1;
                workSheet.Cells[1, column].Value = "LEAD NO";
                column = column + 1;
                workSheet.Cells[1, column].Value = "MC_NO";
                column = column + 1;
                workSheet.Cells[1, column].Value = "TERM";
                column = column + 1;
                workSheet.Cells[1, column].Value = "WORK_QTY";
                column = column + 1;
                workSheet.Cells[1, column].Value = "FIRST_END_TIME";
                column = column + 1;
                workSheet.Cells[1, column].Value = "END_TIME";
                column = column + 1;
                workSheet.Cells[1, column].Value = "CREATE_USER";
                column = column + 1;
                workSheet.Cells[1, column].Value = "NAME";
                column = column + 1;
                workSheet.Cells[1, column].Value = "CREATE_DTM";
                column = column + 1;



                for (int i = 1; i <= column; i++)
                {
                    workSheet.Cells[row, i].Style.Font.Bold = true;
                    workSheet.Cells[row, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[row, i].Style.Font.Size = 14;
                    workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                row = row + 1;

                foreach (var item in list)
                {
                    try
                    {
                        workSheet.Cells[row, 1].Value = item.STAGE;
                        workSheet.Cells[row, 2].Value = item.TORDER_IDX;
                        try
                        {
                            workSheet.Cells[row, 3].Value = item.DATE.Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            workSheet.Cells[row, 3].Value = "";
                        }
                        workSheet.Cells[row, 4].Value = item.LEAD_NO;
                        workSheet.Cells[row, 5].Value = item.MC_NO;
                        workSheet.Cells[row, 6].Value = item.TERM;
                        workSheet.Cells[row, 7].Value = item.WK_QTY;
                        try
                        {
                            workSheet.Cells[row, 8].Value = item.FIRST_TIME.Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            workSheet.Cells[row, 8].Value = "";
                        }
                        try
                        {
                            workSheet.Cells[row, 9].Value = item.END_TIME.Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            workSheet.Cells[row, 9].Value = "";
                        }                        
                        workSheet.Cells[row, 10].Value = item.CREATE_USER;
                        workSheet.Cells[row, 11].Value = item.Name;                       
                        try
                        {
                            workSheet.Cells[row, 12].Value = item.CREATE_DTM.Value.ToString();
                        }
                        catch (Exception ex)
                        {
                            workSheet.Cells[row, 12].Value = "";
                        }

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
    }
}

