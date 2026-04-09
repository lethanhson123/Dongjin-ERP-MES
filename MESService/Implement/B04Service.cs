namespace MESService.Implement
{
    public class B04Service : BaseService<torderlist, ItorderlistRepository>
    , IB04Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly Itmbrcd_hisRepository _tmbrcd_hisRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B04Service(ItorderlistRepository torderlistRepository
            , IWebHostEnvironment webHostEnvironment
            , Itmbrcd_hisRepository tmbrcd_hisRepository



        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
            _tmbrcd_hisRepository = tmbrcd_hisRepository;
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
                result.DataGridView4 = new List<SuperResultTranfer>();
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
                        string ADATE = BaseParameter.ListSearchString[0];
                        string BDATE = BaseParameter.ListSearchString[1];
                        string BC_ID = BaseParameter.ListSearchString[2];
                        string BC_DSCN_YN = BaseParameter.ListSearchString[3];

                        DateTime ADATEDate = DateTime.Parse(ADATE);
                        ADATE = ADATEDate.ToString("yyyy-MM-dd");
                        DateTime BDATEDate = DateTime.Parse(BDATE);
                        BDATE = BDATEDate.ToString("yyyy-MM-dd");

                        string sql = @"SELECT  FALSE AS `CHK`,
                        TMBRCD.BARCD_ID, tspart.PART_NO,  tspart.PART_NM, tspart.PART_CAR, tspart.PART_FML, (SELECT TSCODE.CD_SYS_NOTE FROM TSCODE WHERE  TSCODE.CD_IDX = tspart.`PART_SCN`) AS `PART_SCN`,
                         tiivtr.QTY AS Stock, TMMTIN.`QTY`,TMMTIN.`PLET_NO`, TMMTIN.SHPD_NO, TMBRCD.PKG_GRP, TMBRCD.PKG_QTY,  IF(TMBRCD.PKG_QTY = TMBRCD.PKG_OUTQTY, TMBRCD.PKG_QTY, TMBRCD.PKG_OUTQTY) AS `OUT_QTY`,
                        TMMTIN.MTIN_RMK, TMMTIN.MTIN_DTM,   TMMTIN.DSCN_YN,  TMBRCD.DSCN_YN AS BC_DSCN_YN,
                        TMBRCD.UPDATE_DTM, TMBRCD.UPDATE_USER, tiivtr.IV_IDX   FROM     tspart, tiivtr, TMMTIN, TMBRCD    ";
                        string DGV_DATA0 = GlobalHelper.InitializationString;
                        if (BC_DSCN_YN == "Y")
                        {
                            DGV_DATA0 = @"WHERE  tspart.PART_IDX = tiivtr.PART_IDX AND tspart.PART_IDX = TMMTIN.PART_IDX AND TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX AND 
                            (TMMTIN.DSCN_YN = 'Y') AND NOT(TMBRCD.DSCN_YN = 'Y') AND(TMMTIN.MTIN_DTM <= '" + ADATE + "') AND(TMMTIN.MTIN_DTM >= '" + BDATE + "') AND(TMBRCD.BARCD_ID LIKE '" + BC_ID + "') AND(TMBRCD.BBCO = 'Y') AND (tiivtr.LOC_IDX = 1) ORDER BY TMMTIN.MTIN_DTM, tspart.PART_NO, TMBRCD.PKG_GRP, TMBRCD.BARCD_IDX";
                        }
                        else
                        {
                            DGV_DATA0 = @"WHERE  tspart.PART_IDX = tiivtr.PART_IDX AND tspart.PART_IDX = TMMTIN.PART_IDX AND TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX AND 
                            (TMMTIN.DSCN_YN = 'Y') AND NOT(TMBRCD.DSCN_YN = 'N') AND (TMMTIN.MTIN_DTM <= '" + ADATE + "') AND (TMMTIN.MTIN_DTM >= '" + BDATE + "') AND (TMBRCD.BARCD_ID LIKE '" + BC_ID + "') AND (TMBRCD.BBCO = 'Y') AND (tiivtr.LOC_IDX = 1) AND(TMBRCD.BBCO = 'Y') ORDER BY TMMTIN.MTIN_DTM, tspart.PART_NO, TMBRCD.PKG_GRP, TMBRCD.BARCD_IDX";
                        }
                        sql = sql + DGV_DATA0;

                        sql = sql + " LIMIT " + GlobalHelper.ListCount;

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_LIST = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_LIST.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        string AAA = BaseParameter.ListSearchString[0];
                        string CCC = BaseParameter.ListSearchString[1];

                        DateTime CCCDate = DateTime.Parse(CCC);
                        CCC = CCCDate.ToString("yyyy-MM-dd");

                        string sql = @"SELECT  
                                (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = `TMBRCD_SUM`.`MTIN_IDX`)) AS `PART_NO`,
                                (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = `TMBRCD_SUM`.`MTIN_IDX`)) AS `PART_NAME`,
                                TMMTIN.`SNP_QTY` AS `PART_SNP`,  
                                `TMBRCD_SUM`.`PKG_QTY` AS `PKG_QTY`, 
                                `TMBRCD_SUM`.`PKG_OUTQTY`, 
                                TMMTIN.`PLET_NO` AS `Pallet_NO`, TMMTIN.`SHPD_NO` AS `Shipping_NO`, 
                                IFNULL(TMMTIN.`MTIN_DTM`, DATE_FORMAT(`TMBRCD_SUM`.`CREATE_DTM`,'%Y-%m-%d')) AS `Receipt_Data`,
                                (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `TMBRCD_SUM`.`CREATE_USER`) AS `Receipt_USER`, 
                                (`TMBRCD_SUM`.`OUT_DTM`)  AS `Release_Date`, 
                                IFNULL((SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `TMBRCD_SUM`.`UPDATE_USER`), '') AS `Release_USER`,
                                `TMBRCD_SUM`.`DSCN_YN` AS `BR_DSCN`, `TMBRCD_SUM`.`PKG_GRP`, TMMTIN.`MTIN_IDX`, TMMTIN.`PART_IDX`, `TMBRCD_SUM`.`BARCD_IDX`, `TMBRCD_SUM`.`BARCD_ID`,
                                IFNULL(`TMBRCD_SUM`.`BARCD_LOC`, (SELECT tspart.`PART_LOC` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = `TMBRCD_SUM`.`MTIN_IDX`))) AS `LOC`,
                                DATE_FORMAT(IFNULL(`TMBRCD_SUM`.`UPDATE_DTM`, IFNULL(`TMBRCD_SUM`.`CREATE_DTM`, TMMTIN.`MTIN_DTM`)), '%Y-%m-%d') AS `OR_DT`, `TMBRCD_SUM`.`TMMTIN_DMM_IDX`, `TMBRCD_SUM`.`Name`, `TMBRCD_SUM`.`ReasonID`, `TMBRCD_SUM`.`Reason`

                                FROM    TMMTIN, 

                                (SELECT 
	                                  TMBRCD.`BARCD_IDX`, TMBRCD.`BARCD_ID`,	TMBRCD.`PKG_GRP_IDX`,	TMBRCD.`PKG_GRP`, 
	                                  IFNULL(TMBRCD_HIS.`PKG_QTY`, TMBRCD.`PKG_QTY`) AS `PKG_QTY`,
	                                  IF(TMBRCD.`DSCN_YN` ='Y', TMBRCD.`PKG_OUTQTY`, IFNULL(TMBRCD_HIS.`PKG_OUTQTY`, TMBRCD.`PKG_OUTQTY`)) AS `PKG_OUTQTY`,
	                                  TMBRCD.`DSCN_YN`, 
	                                  IFNULL(IF(TMBRCD_HIS.`OUT_DTM` > TMBRCD.`OUT_DTM`, TMBRCD_HIS.`OUT_DTM`, TMBRCD.`OUT_DTM`), '') AS `OUT_DTM`,
                                     TMBRCD.`MTIN_IDX`, TMBRCD.`BARCD_LOC`, 
  	                                 IFNULL(IF(TMBRCD_HIS.`CREATE_DTM` > TMBRCD.`CREATE_DTM`, TMBRCD_HIS.`CREATE_DTM`, TMBRCD.`CREATE_DTM`), '')  AS `CREATE_DTM`,
	                                  TMBRCD.`CREATE_USER`, 
	                                  IFNULL(IFNULL(TMBRCD_HIS.`UPDATE_DTM`, TMBRCD.`UPDATE_DTM`), '') AS `UPDATE_DTM`,
                                     TMBRCD.`UPDATE_USER`, TMBRCD.`BBCO`, TMBRCD_HIS.`TMMTIN_DMM_IDX`, TMBRCD_HIS.`Name`, TMBRCD_HIS.`ReasonID`, TMBRCD_HIS.`Reason` 
                                     FROM TMBRCD LEFT JOIN TMBRCD_HIS
                                     ON TMBRCD.BARCD_IDX = TMBRCD_HIS.BARCD_IDX
                                     WHERE TMBRCD.BARCD_ID LIKE '" + AAA + "$$%' ) `TMBRCD_SUM` WHERE  `TMBRCD_SUM`.`MTIN_IDX` = TMMTIN.`MTIN_IDX`  AND  `TMBRCD_SUM`.`BBCO` = 'Y' AND (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = `TMBRCD_SUM`.`MTIN_IDX`)) = '" + AAA + "' AND TMMTIN.`MTIN_DTM` >= '" + CCC + "' ORDER BY  `Receipt_Data` ASC    ";

                        //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.T_DGV_02 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.T_DGV_02.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.T_DGV_02.Count > 0)
                        {
                            for (int i = 0; i < result.T_DGV_02.Count; i++)
                            {
                                result.T_DGV_02[i].QTY = result.T_DGV_02[i].PKG_QTY - result.T_DGV_02[i].PKG_OUTQTY;
                                result.T_DGV_02[i].CODE = 0;
                                if (result.T_DGV_02[i].QTY > 0)
                                {
                                    result.T_DGV_02[i].CODE = 1;
                                    if ((result.T_DGV_02[i].Release_Date == null) || (result.T_DGV_02[i].Release_Date == ""))
                                    {
                                        //result.T_DGV_02[i].CODE = 2;
                                    }
                                }
                            }
                            result.T_DGV_02 = result.T_DGV_02.OrderByDescending(item => item.CODE).ThenBy(item => item.Receipt_Data).ThenBy(item => item.Release_Date).ToList();
                        }

                        result.CALL_STOCK = await CALL_STOCK(BaseParameter);

                        CCC = "yyyy-MM-dd";
                        sql = @"SELECT
                            `TB_M`.`NO`, `TB_M`.`DateRDCE`, `TB_M`.`TYPE`, `TB_M`.`SUM_QTY`, 
                            IF(`NO` ='1', `TB_M`.`STOCK_QTY`, (`TB_M`.`STOCK_QTY` + `TB_M`.`SUM_QTY`) - SUM(`TB_M`.`SUM_QTY`) OVER(ORDER BY `TB_M`.`NO`)) AS `STOCK_QTY`, 
                            `TB_M`.`DATE`, `TB_M`.`SHPD_NO`, `TB_M`.`PLET_NO`, `TB_M`.`USER`
                            FROM(
                            SELECT 
                            ROW_NUMBER() OVER(ORDER BY `SUM_LIST`.`DateRDCE` DESC, `SUM_LIST`.`IN_DATE` DESC, `SUM_LIST`.`OUT_DATE` DESC) AS `NO`, 
                            `SUM_LIST`.`DateRDCE`,
                            `SUM_LIST`.`TYPE`,
                            SUM(`SUM_LIST`.`QTY`) AS `SUM_QTY`,
                            `SUM_LIST`.`STOCK_QTY`,
                            MAX(IF(`SUM_LIST`.`OUT_DATE` = '', `SUM_LIST`.`IN_DATE`, `SUM_LIST`.`OUT_DATE`)) AS `DATE`,
                            `SUM_LIST`.`SHPD_NO`,
                            `SUM_LIST`.`PLET_NO`,
                            `SUM_LIST`.`USER`

                            FROM 
                            (SELECT 
                            'IN' AS `TYPE`,
                            DATE_FORMAT(TMBRCD.`CREATE_DTM`, '%Y-%m-%d') AS `DateRDCE`, 
                            TMMTIN.`QTY` AS `QTY`, 
                            (SELECT tiivtr.`QTY` FROM tiivtr WHERE tiivtr.`PART_IDX` = TMMTIN.`PART_IDX` AND tiivtr.`LOC_IDX`= '1') AS `STOCK_QTY`, 
                            (TMBRCD.`CREATE_DTM`) AS `IN_DATE`,
                            '' AS `OUT_DATE`,
                            TMMTIN.`SHPD_NO`, 
                            TMMTIN.`PLET_NO`, (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = TMMTIN.`UPDATE_USER`) AS `USER`
                            FROM TMMTIN, TMBRCD
                            WHERE TMBRCD.`MTIN_IDX` = TMMTIN.`MTIN_IDX` AND TMMTIN.`PART_IDX` = (SELECT tspart.`PART_IDX` FROM tspart WHERE `PART_NO` = '" + AAA + "') AND(TMMTIN.MTIN_DTM >= '" + CCC + "') AND TMBRCD.BBCO = 'Y' GROUP BY TMMTIN.`MTIN_IDX` UNION SELECT 'OUT' AS `TYPE`, DATE_FORMAT(IF(`TMBRCD_SUM`.`PKG_OUTQTY` = 0, '', `TMBRCD_SUM`.`UPDATE_DTM`), '%Y-%m-%d') AS `DateRDCE`,  (`TMBRCD_SUM`.`PKG_OUTQTY` *-1) AS `QTY`,   (SELECT tiivtr.`QTY` FROM tiivtr WHERE tiivtr.`LOC_IDX`= '1' AND tiivtr.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = `TMBRCD_SUM`.`MTIN_IDX`)) AS `STOCK_QTY`, '' AS `IN_DATE`, IF(`TMBRCD_SUM`.`PKG_OUTQTY` = 0, '', `TMBRCD_SUM`.`UPDATE_DTM`)  AS `OUT_DATE`,  TMMTIN.`SHPD_NO`, TMMTIN.`PLET_NO`, IFNULL((SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `TMBRCD_SUM`.`UPDATE_USER`), '') AS `USER` FROM TMMTIN, (SELECT  TMBRCD.`BARCD_IDX`, TMBRCD.`BARCD_ID`, TMBRCD.`PKG_GRP_IDX`, TMBRCD.`PKG_GRP`,      IFNULL(TMBRCD_HIS.`PKG_QTY`, TMBRCD.`PKG_QTY`) AS `PKG_QTY`,      IF(TMBRCD.`DSCN_YN` = 'Y', TMBRCD.`PKG_OUTQTY`, IFNULL(TMBRCD_HIS.`PKG_OUTQTY`, TMBRCD.`PKG_OUTQTY`)) AS `PKG_OUTQTY`,      TMBRCD.`DSCN_YN`,      IFNULL(TMBRCD_HIS.`OUT_DTM`, TMBRCD.`OUT_DTM`) AS `OUT_DTM`,     TMBRCD.`MTIN_IDX`, TMBRCD.`BARCD_LOC`,        IFNULL(TMBRCD_HIS.`CREATE_DTM`, TMBRCD.`CREATE_DTM`) AS `CREATE_DTM`,      TMBRCD.`CREATE_USER`,      IFNULL(TMBRCD_HIS.`UPDATE_DTM`, TMBRCD.`UPDATE_DTM`) AS `UPDATE_DTM`,     TMBRCD.`UPDATE_USER`, TMBRCD.`BBCO`     FROM TMBRCD LEFT JOIN TMBRCD_HIS     ON TMBRCD.BARCD_IDX = TMBRCD_HIS.BARCD_IDX     WHERE TMBRCD.BARCD_ID LIKE '" + AAA + "$$%' ) `TMBRCD_SUM`  WHERE  `TMBRCD_SUM`.`MTIN_IDX` = TMMTIN.`MTIN_IDX`  AND  `TMBRCD_SUM`.`BBCO` = 'Y' AND (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = `TMBRCD_SUM`.`MTIN_IDX`)) = '" + AAA + "' AND TMMTIN.`MTIN_DTM` >= '" + CCC + "' AND `PKG_OUTQTY` > 0 ORDER BY `DateRDCE` DESC, `IN_DATE` DESC, `OUT_DATE` DESC ) `SUM_LIST` GROUP BY `DateRDCE`, `TYPE` ORDER BY `NO`, `DateRDCE` DESC) `TB_M`    ";

                        sql = sql + " LIMIT " + GlobalHelper.ListCount;

                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.T_DGV_01 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.T_DGV_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    if (BaseParameter.Action == 4)
                    {
                        string D4_YEAR = BaseParameter.ListSearchString[0];
                        string D4_MONTH = BaseParameter.ListSearchString[1];



                        string sql = @"SELECT  `MAIN`.`YEAR`, `MAIN`.`MONTH`, IFNULL(`MAIN`.`WEEK`, 'SUM') AS `WEEK`, COUNT(`MAIN`.`PART_IDX`) AS `P_COUNT`,
                            SUM(`MAIN`.`COUNT`) AS `BC_COUNT`, SUM(`MAIN`.`QTY`) AS `QTY`, 
                            SUM(`MAIN`.`IN_QTY`) AS `IN_QTY`, SUM(`MAIN`.`OUT_QTY`) AS `OUT_QTY`,
                            AVG(`MAIN`.`PO_PLAN`) AS `PO_PLAN`, SUM(`MAIN`.`STAY_QTY`) AS `STAY_QTY`, AVG(`MAIN`.`STAY_RATIO`) AS `STAY_RATIO`
                            FROM ( SELECT 
                            TMMTIN.`MTIN_IDX`, TMMTIN.`PART_IDX`, TMMTIN.`UTM`, TMMTIN.`DESC`, YEAR(TMMTIN.`MTIN_DTM`) AS `YEAR`, MONTH(TMMTIN.`MTIN_DTM`) AS `MONTH`,
                            WEEK(TMMTIN.`MTIN_DTM`) AS `WEEK`, TMMTIN.`MTIN_DTM`, TMMTIN.`QTY`, TMMTIN.`BRCD_PRNT`,
                            `TB_B`.`PKG_GRP_IDX`, `TB_B`.`PKG_GRP`, `TB_B`.`COUNT`, `TB_B`.`IN_QTY`, `TB_B`.`OUT_QTY`,
                            (TMMTIN.`QTY` / `TB_B`.`IN_QTY`) * 100 AS `PO_PLAN`, 
                            (`TB_B`.`IN_QTY` - `TB_B`.`OUT_QTY`) AS `STAY_QTY`,
                            (`TB_B`.`OUT_QTY` / `TB_B`.`IN_QTY`) * 100 AS `STAY_RATIO`
                            FROM TMMTIN LEFT JOIN (SELECT  TMBRCD.`MTIN_IDX`, TMBRCD.`PKG_GRP_IDX`, TMBRCD.`PKG_GRP`, COUNT(TMBRCD.`PKG_GRP`) AS `COUNT`,
                            SUM(TMBRCD.`PKG_QTY`) AS `IN_QTY`, SUM(TMBRCD.`PKG_OUTQTY`) AS `OUT_QTY`
                            FROM TMBRCD   WHERE TMBRCD.`BBCO` = 'Y'  GROUP BY TMBRCD.`MTIN_IDX`) `TB_B`
                            ON TMMTIN.MTIN_IDX = `TB_B`.`MTIN_IDX`
                            WHERE  TMMTIN.`DSCN_YN` = 'Y' AND YEAR(TMMTIN.`MTIN_DTM`) = '" + D4_YEAR + "' AND MONTH(TMMTIN.`MTIN_DTM`) =  '" + D4_MONTH + "'  ) `MAIN` GROUP BY `MAIN`.`WEEK` WITH ROLLUP     ";

                        sql = sql + " LIMIT " + GlobalHelper.ListCount;

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        string AAA = BaseParameter.ListSearchString[2];
                        string BBB = BaseParameter.ListSearchString[3];
                        string CCC = GlobalHelper.InitializationString;
                        if (string.IsNullOrEmpty(BBB))
                        {
                            CCC = "";
                        }
                        else
                        {
                            CCC = " AND `WEEK` = '" + BBB + "'";
                        }
                        sql = @"SELECT  (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = `MAIN`.`PART_IDX`) AS `PART_NO`, 
                            `MAIN`.`UTM`, `MAIN`.`DESC`, `MAIN`.`YEAR`, `MAIN`.`MONTH`, `MAIN`.`WEEK`, 
                            SUM(`MAIN`.`QTY`) AS `QTY`, SUM(`MAIN`.`COUNT`) AS `BC_COUNT`, 
                            SUM(`MAIN`.`IN_QTY`) AS `IN_QTY`, SUM(`MAIN`.`OUT_QTY`) AS `OUT_QTY`,
                            AVG(`MAIN`.`PO_PLAN`) AS `PO_PLAN`, SUM(`MAIN`.`STAY_QTY`) AS `STAY_QTY`, AVG(`MAIN`.`STAY_RATIO`) AS `STAY_RATIO`
                            FROM ( SELECT 
                            TMMTIN.`MTIN_IDX`, TMMTIN.`PART_IDX`, TMMTIN.`UTM`, TMMTIN.`DESC`, YEAR(TMMTIN.`MTIN_DTM`) AS `YEAR`, MONTH(TMMTIN.`MTIN_DTM`) AS `MONTH`,
                            WEEK(TMMTIN.`MTIN_DTM`) AS `WEEK`, TMMTIN.`MTIN_DTM`, TMMTIN.`QTY`, TMMTIN.`BRCD_PRNT`,
                            `TB_B`.`PKG_GRP_IDX`, `TB_B`.`PKG_GRP`, `TB_B`.`COUNT`, `TB_B`.`IN_QTY`, `TB_B`.`OUT_QTY`,
                            (TMMTIN.`QTY` / `TB_B`.`IN_QTY`) * 100 AS `PO_PLAN`, 
                            (`TB_B`.`IN_QTY` - `TB_B`.`OUT_QTY`) AS `STAY_QTY`,
                            (`TB_B`.`OUT_QTY` / `TB_B`.`IN_QTY`) * 100 AS `STAY_RATIO`
                            FROM TMMTIN LEFT JOIN (SELECT  TMBRCD.`MTIN_IDX`, TMBRCD.`PKG_GRP_IDX`, TMBRCD.`PKG_GRP`, COUNT(TMBRCD.`PKG_GRP`) AS `COUNT`,
                            SUM(TMBRCD.`PKG_QTY`) AS `IN_QTY`, SUM(TMBRCD.`PKG_OUTQTY`) AS `OUT_QTY`
                            FROM TMBRCD   WHERE TMBRCD.`BBCO` = 'Y'  GROUP BY TMBRCD.`MTIN_IDX`) `TB_B`
                            ON TMMTIN.MTIN_IDX = `TB_B`.`MTIN_IDX`
                            WHERE TMMTIN.`DSCN_YN` = 'Y' AND  YEAR(TMMTIN.`MTIN_DTM`) = '" + D4_YEAR + "' AND MONTH(TMMTIN.`MTIN_DTM`) =  '" + D4_MONTH + "'  ) `MAIN` GROUP BY `MAIN`.`WEEK`, `MAIN`.`PART_IDX` HAVING `PART_NO` LIKE '%" + AAA + "%'   " + CCC + "ORDER BY `WEEK`, `PART_IDX`     ";


                        sql = sql + " LIMIT " + GlobalHelper.ListCount;

                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    if (BaseParameter.Action == 5)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string SearchDate = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd");
                            string Search = BaseParameter.ListSearchString[1];
                            Search = Search.Trim();
                            string sql = @"SELECT a.* FROM tmbrcd_his a WHERE a.ReasonID>0 AND a.OUT_DTM LIKE '%" + SearchDate + "%'";
                            if (!string.IsNullOrEmpty(Search))
                            {
                                sql = @"SELECT a.* FROM tmbrcd_his a WHERE a.ReasonID>0 AND a.BARCD_ID LIKE '%" + Search + "%'";
                            }
                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView7 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView7.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }

                            sql = @"SELECT a.* FROM tmbrcd_his a WHERE a.TMMTIN_DMM_IDX=0 AND a.OUT_DTM LIKE '%" + SearchDate + "%'";
                            if (!string.IsNullOrEmpty(Search))
                            {
                                sql = @"SELECT a.* FROM tmbrcd_his a WHERE a.TMMTIN_DMM_IDX=0 AND a.BARCD_ID LIKE '%" + Search + "%'";
                            }
                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView8 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView8.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 6)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string SearchString = BaseParameter.ListSearchString[0];
                            string Year = BaseParameter.ListSearchString[1];
                            string Month = BaseParameter.ListSearchString[2];

                            string sql = "";

                            if (!string.IsNullOrEmpty(SearchString))
                            {
                                sql = @"SELECT 
                                * 

                                , (a.OUT_QTY - a.PKG_OUTQTY) AS QuantityGAP

                                FROM (

                                SELECT 

                                SUBSTRING_INDEX(a.BARCD_ID, '$$', 1) AS PART_NO

                                , a.BARCD_IDX

                                , a.BARCD_ID

                                , a.PKG_QTY

                                , a.PKG_OUTQTY

                                , SUM(b.PKG_OUTQTY) AS OUT_QTY

                                FROM tmbrcd a LEFT JOIN tmbrcd_his b ON a.BARCD_IDX=b.BARCD_IDX

                                WHERE a.BARCD_ID LIKE '%" + SearchString + "%' GROUP BY a.BARCD_IDX, a.BARCD_ID, a.PKG_QTY, a.PKG_OUTQTY) a WHERE a.PKG_OUTQTY <> a.OUT_QTY ORDER BY a.OUT_QTY DESC ";
                            }
                            else
                            {
                                if (Month == "00")
                                {
                                    SearchString = Year;
                                }
                                else
                                {
                                    SearchString = Year + "-" + Month;
                                }
                                sql = @"SELECT 
                                * 

                                , (a.OUT_QTY - a.PKG_OUTQTY) AS QuantityGAP

                                FROM (

                                SELECT 

                                SUBSTRING_INDEX(a.BARCD_ID, '$$', 1) AS PART_NO

                                , a.BARCD_IDX

                                , a.BARCD_ID

                                , a.PKG_QTY

                                , a.PKG_OUTQTY

                                , SUM(b.PKG_OUTQTY) AS OUT_QTY

                                FROM tmbrcd a LEFT JOIN tmbrcd_his b ON a.BARCD_IDX=b.BARCD_IDX

                                WHERE a.OUT_DTM LIKE '%" + SearchString + "%' GROUP BY a.BARCD_IDX, a.BARCD_ID, a.PKG_QTY, a.PKG_OUTQTY, a.OUT_DTM) a WHERE a.PKG_OUTQTY <> a.OUT_QTY ORDER BY a.OUT_QTY DESC ";
                            }
                            if (BaseParameter.CheckBox1 == true)
                            {
                                var OUT_DTM = "";
                                if (Month == "00")
                                {
                                    OUT_DTM = Year;
                                }
                                else
                                {
                                    OUT_DTM = Year + "-" + Month;
                                }
                                sql = @"SELECT * FROM tmbrcd WHERE BARCD_ID LIKE '%" + SearchString + "%' ORDER BY OUT_DTM DESC";
                            }

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DGV_B07_31 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DGV_B07_31.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                string SheetName = this.GetType().Name;
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.DataGridView4 != null)
                        {
                            result.DataGridView4 = BaseParameter.DataGridView4;
                            string DGV_result1 = "ERROR : ";
                            string DGV_result2 = "";
                            result.DGV_result3 = true;

                            int JJJ = BaseParameter.DataGridView4.Count;
                            StringBuilder HTMLContent = new StringBuilder();
                            for (int III = 0; III < JJJ; III++)
                            {
                                result.DataGridView4[III].DGV_result3 = true;
                                var V_PART_NO = BaseParameter.DataGridView4[III].PART_NO;
                                var V_MTIN_DTM = BaseParameter.DataGridView4[III].MTIN_DTM.Value.ToString("yyyy-MM-dd");
                                var V_USE_YN = BaseParameter.DataGridView4[III].BC_DSCN_YN;
                                var V_PART_IDX = BaseParameter.DataGridView4[III].PART_IDX;
                                var V_OUT_QTY = BaseParameter.DataGridView4[III].PKG_QTY;
                                var V_BARCD_IDX = BaseParameter.DataGridView4[III].BARCD_IDX;
                                var V_PKG_GRP_IDX = BaseParameter.DataGridView4[III].PKG_GRP_IDX;
                                var V_PKG_GRP = BaseParameter.DataGridView4[III].PKG_GRP;
                                var V_BARID_TEXT = BaseParameter.DataGridView4[III].BARCD_ID;
                                var MAX_SNP = double.Parse(BaseParameter.DataGridView4[III].MAX_PKG);
                                var TMMTIN_DMM_IDX = BaseParameter.DataGridView4[III].TMMTIN_DMM_IDX;
                                var Name = BaseParameter.DataGridView4[III].Name;
                                var ReasonID = BaseParameter.DataGridView4[III].ReasonID;
                                var Reason = BaseParameter.DataGridView4[III].Reason;
                                var PART_IDX = BaseParameter.DataGridView4[III].PART_IDX;
                                var TMMTIN_SHEETNO = BaseParameter.DataGridView4[III].TMMTIN_SHEETNO == null ? 0 : BaseParameter.DataGridView4[III].TMMTIN_SHEETNO;

                                if (TMMTIN_DMM_IDX == null)
                                {
                                    TMMTIN_DMM_IDX = 0;
                                }
                                if (ReasonID == null)
                                {
                                    ReasonID = 0;
                                }
                                string sql = @"SELECT  TMBRCD.`BARCD_ID`, TMBRCD.`DSCN_YN` AS `BC_DSCN_YN`, TMBRCD.`BARCD_IDX` FROM TMBRCD  WHERE NOT(TMBRCD.`DSCN_YN` = 'Y') AND TMBRCD.`BBCO` = 'Y'  AND TMBRCD.`BARCD_ID` = '" + V_BARID_TEXT + "'   ";


                                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DGV_B04_BCTXT = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DGV_B04_BCTXT.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (result.DGV_B04_BCTXT.Count < 0)
                                {
                                    DGV_result2 = DGV_result2 + " U";
                                    result.DataGridView4[III].RESULT = DGV_result1 + DGV_result2;
                                    result.DGV_result3 = false;
                                }
                                if (V_USE_YN == "Y")
                                {
                                    DGV_result2 = DGV_result2 + " U";
                                    result.DataGridView4[III].RESULT = DGV_result1 + DGV_result2;
                                    result.DGV_result3 = false;
                                }

                                sql = @"SELECT  `QTY`, `IV_IDX`  FROM     tiivtr   WHERE  `PART_IDX` = '" + V_PART_IDX + "'   AND   `LOC_IDX` = '1'  ";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DGV_B09_91 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DGV_B09_91.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (result.DGV_B09_91.Count < 0)
                                {
                                    DGV_result2 = DGV_result2 + " D";
                                    result.DataGridView4[III].RESULT = DGV_result1 + DGV_result2;
                                    result.DGV_result3 = false;
                                }
                                var V_IV_QTY = result.DGV_B09_91[0].QTY;
                                var V_IV_IDX = result.DGV_B09_91[0].IV_IDX;
                                if (V_IV_QTY < V_OUT_QTY)
                                {
                                    DGV_result2 = DGV_result2 + " C";
                                    result.DataGridView4[III].RESULT = DGV_result1 + DGV_result2;
                                    result.DGV_result3 = false;
                                }

                                sql = @"SELECT `A`.BARCD_IDX, left(`A`.`BARCD_ID`, POSITION('$$' IN `A`.`BARCD_ID`)-1) AS `PART_NO`, `B`.`MTIN_DTM` FROM TMBRCD `A`, TMMTIN `B` WHERE `A`.MTIN_IDX = `B`.MTIN_IDX AND  left(`A`.`BARCD_ID`, POSITION('$$' IN `A`.`BARCD_ID`)-1) = '" + V_PART_NO + "' AND `A`.`DSCN_YN`= 'N' AND ((WEEK(`B`.`MTIN_DTM`) + YEAR(`B`.`MTIN_DTM`)*100) < (WEEK('" + V_MTIN_DTM + "')+YEAR('" + V_MTIN_DTM + "')*100)) AND `A`.`BBCO` = 'Y' ORDER BY `BARCD_IDX`";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DGV_B04_02 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DGV_B04_02.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (result.DGV_B04_02.Count < 0)
                                {
                                    DGV_result2 = DGV_result2 + " FIFO";
                                    result.DataGridView4[III].RESULT = DGV_result1 + DGV_result2;
                                    result.DGV_result3 = false;
                                }

                                if (result.DGV_result3 == true)
                                {
                                    string UPDATE_USER = BaseParameter.USER_ID;
                                    sql = @"UPDATE  TMBRCD   SET  `DSCN_YN` = IF(`PKG_QTY` = `PKG_OUTQTY` + " + V_OUT_QTY + ", 'Y' , 'S'), `PKG_OUTQTY` = IF(`PKG_QTY` <= `PKG_OUTQTY` + " + V_OUT_QTY + ", `PKG_QTY`, `PKG_OUTQTY` + " + V_OUT_QTY + "), `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + UPDATE_USER + "',  `OUT_DTM` = NOW() WHERE  `BARCD_IDX` = '" + V_BARCD_IDX + "'   ";
                                    string sqlResult01 = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    if (V_OUT_QTY < MAX_SNP)
                                    {
                                        var BARCODE_QR = V_BARID_TEXT;
                                        var BARCODE_AA = V_PKG_GRP;
                                        var BARCODE_BB = (MAX_SNP - V_OUT_QTY) + " (S)";
                                        var BARCODE_CC = result.DataGridView4[III].QTY.ToString();
                                        var BARCODE_DD = V_PART_NO;
                                        var BARCODE_EE = result.DataGridView4[III].PART_NM;
                                        var BARCODE_FF = result.DataGridView4[III].SHPD_NO;
                                        var BARCODE_HH = result.DataGridView4[III].LOC;
                                        var BARCODE_ZZ = result.DataGridView4[III].MTIN_DTM.Value.ToString("yyyy-MM-dd");
                                        var BARCODE_GG = GlobalHelper.InitializationString;
                                        //var BARCODE_CC1 = V_OUT_QTY.ToString();
                                        var BARCODE_CC1 = BARCODE_BB;
                                        HTMLContent.AppendLine(GlobalHelper.CreateHTMLB04(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, BARCODE_AA, BARCODE_BB, BARCODE_CC, BARCODE_DD, BARCODE_EE, BARCODE_FF, BARCODE_GG, BARCODE_HH, BARCODE_ZZ, BARCODE_CC1));
                                        //sql = @"INSERT INTO `TMBRCD_HIS` (`BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, `PKG_OUTQTY`, `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, `TMMTIN_DMM_IDX`, `PART_IDX`, `Name`, `ReasonID`, `Reason`) 
                                        //    SELECT `BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, " + V_OUT_QTY + ", `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, " + TMMTIN_DMM_IDX + ", " + PART_IDX + ", '" + Name + "', " + ReasonID + ", '" + Reason + "' FROM TMBRCD   WHERE TMBRCD.BARCD_ID = '" + V_BARID_TEXT + "'";
                                        //sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                    else
                                    {
                                        //if (V_USE_YN == "S")
                                        //{
                                        //    sql = @"INSERT INTO `TMBRCD_HIS` (`BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, `PKG_OUTQTY`, `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, `TMMTIN_DMM_IDX`, `Name`, `ReasonID`, `Reason`) 
                                        //    SELECT `BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, " + V_OUT_QTY + ", `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, " + TMMTIN_DMM_IDX + ", '" + Name + "', " + ReasonID + ", '" + Reason + "' FROM TMBRCD   WHERE TMBRCD.BARCD_ID = '" + V_BARID_TEXT + "'";
                                        //    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                        //}                                      
                                    }

                                    sql = @"INSERT INTO `TMBRCD_HIS` (`BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, `PKG_OUTQTY`, `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, `TMMTIN_DMM_IDX`, `PART_IDX`, `Name`, `TMMTIN_SHEETNO`, `ReasonID`, `Reason`) 
                                            SELECT `BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, " + V_OUT_QTY + ", `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, " + TMMTIN_DMM_IDX + ", " + PART_IDX + ", '" + Name + "', " + TMMTIN_SHEETNO + ", " + ReasonID + ", '" + Reason + "' FROM TMBRCD   WHERE TMBRCD.BARCD_ID = '" + V_BARID_TEXT + "'";
                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);


                                    sql = @"UPDATE tiivtr   SET  `QTY` = (`QTY` - " + V_OUT_QTY + "),  `UPDATE_DTM` = NOW(),  `UPDATE_USER` = '" + UPDATE_USER + "'  WHERE  `IV_IDX` = '" + V_IV_IDX + "'  ";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES (" + V_PART_IDX + ", 7, " + V_OUT_QTY + ", NOW(), '" + UPDATE_USER + "' ) ON DUPLICATE KEY UPDATE  `LOC_IDX`= 7, `QTY` = (`QTY` +" + V_OUT_QTY + "), `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + UPDATE_USER + "'";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"ALTER TABLE     `tiivtr`     AUTO_INCREMENT= 1";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    result.DataGridView4[III].RESULT = "Complete";
                                }
                            }

                            if (!string.IsNullOrEmpty(HTMLContent.ToString()))
                            {
                                string contentHTML = GlobalHelper.InitializationString;
                                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
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
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.DGV_LIST != null)
                        {
                            int II = GlobalHelper.InitializationNumber;
                            int JJ = BaseParameter.DGV_LIST.Count;
                            if (JJ > 0)
                            {
                                for (II = 0; II < JJ; II++)
                                {
                                    bool ACK = BaseParameter.DGV_LIST[II].CHK.Value;
                                    string BCID = BaseParameter.DGV_LIST[II].BARCD_ID;
                                    double PKGQUT_LIST = BaseParameter.DGV_LIST[II].PKG_QTY.Value;
                                    int IV_IDX_LIST = BaseParameter.DGV_LIST[II].IV_IDX.Value;
                                    double Stock_LIST = BaseParameter.DGV_LIST[II].Stock.Value;
                                    Stock_LIST = Stock_LIST + PKGQUT_LIST;
                                    if (ACK == true)
                                    {
                                        string UPDATE_USER = BaseParameter.USER_ID;
                                        string sql = @"UPDATE  TMBRCD  SET   `DSCN_YN` = 'N', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + UPDATE_USER + "'  WHERE  `BARCD_ID` = '" + BCID + "'";
                                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        sql = @"UPDATE   tiivtr  SET   `QTY` = '" + Stock_LIST + "', UPDATE_DTM = NOW(), UPDATE_USER = '" + UPDATE_USER + "' WHERE  `IV_IDX` = '" + IV_IDX_LIST + "'";
                                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                    if (BaseParameter.Action == 3)
                    {
                        if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                        {
                            string CCC = BaseParameter.SearchString;
                            DateTime CCCDate = DateTime.Parse(CCC);
                            var List = await _tmbrcd_hisRepository.GetByCondition(o => o.OUT_DTM != null && o.OUT_DTM.Value.Year == CCCDate.Year).OrderBy(o => o.OUT_DTM).ToListAsync();
                            for (int i = 0; i < List.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(List[i].BARCD_ID))
                                {
                                    List[i].Name = List[i].BARCD_ID.Split('$')[0];
                                }
                            }
                            string SheetName = this.GetType().Name;
                            string fileName = SheetName + @"_" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                            var streamExport = new MemoryStream();
                            InitializationToExcelAsync(List, streamExport, SheetName);
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
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        private void InitializationToExcelAsync(List<tmbrcd_his> list, MemoryStream streamExport, string SheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(SheetName);
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "No";
                column = column + 1;
                workSheet.Cells[row, column].Value = "PART NO";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Barcode";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Quantity";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Date Out";
                column = column + 1;
                workSheet.Cells[row, column].Value = "User";

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

                int stt = 1;
                foreach (var item in list)
                {
                    try
                    {
                        workSheet.Cells[row, 1].Value = stt;
                        workSheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        workSheet.Cells[row, 2].Value = item.Name;
                        workSheet.Cells[row, 3].Value = item.BARCD_ID;
                        workSheet.Cells[row, 4].Value = item.PKG_OUTQTY;
                        try
                        {
                            workSheet.Cells[row, 5].Value = item.OUT_DTM.Value.ToString("dd/MM/yyyy HH:mm:ss");
                        }
                        catch (Exception ex)
                        {

                        }

                        workSheet.Cells[row, 6].Value = item.UPDATE_USER;

                        for (int i = 1; i <= column; i++)
                        {
                            workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                            workSheet.Cells[row, i].Style.Font.Size = 14;
                            workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                        stt = stt + 1;
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
        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string SheetName = this.GetType().Name;
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.DataGridView4 != null)
                        {
                            result.DataGridView4 = BaseParameter.DataGridView4;
                            string DGV_result1 = "ERROR : ";
                            string DGV_result2 = "";
                            result.DGV_result3 = true;

                            int JJJ = BaseParameter.DataGridView4.Count;
                            StringBuilder HTMLContent = new StringBuilder();
                            for (int III = 0; III < JJJ; III++)
                            {
                                result.DataGridView4[III].DGV_result3 = true;
                                var V_PART_NO = BaseParameter.DataGridView4[III].PART_NO;
                                var V_MTIN_DTM = BaseParameter.DataGridView4[III].MTIN_DTM.Value.ToString("yyyy-MM-dd");
                                var V_USE_YN = BaseParameter.DataGridView4[III].BC_DSCN_YN;
                                var V_PART_IDX = BaseParameter.DataGridView4[III].PART_IDX;
                                var V_OUT_QTY = BaseParameter.DataGridView4[III].PKG_QTY;
                                var V_BARCD_IDX = BaseParameter.DataGridView4[III].BARCD_IDX;
                                var V_PKG_GRP_IDX = BaseParameter.DataGridView4[III].PKG_GRP_IDX;
                                var V_PKG_GRP = BaseParameter.DataGridView4[III].PKG_GRP;
                                var V_BARID_TEXT = BaseParameter.DataGridView4[III].BARCD_ID;
                                var MAX_SNP = double.Parse(BaseParameter.DataGridView4[III].MAX_PKG);
                                var TMMTIN_DMM_IDX = BaseParameter.DataGridView4[III].TMMTIN_DMM_IDX;
                                var Name = BaseParameter.DataGridView4[III].Name;
                                var ReasonID = BaseParameter.DataGridView4[III].ReasonID;
                                var Reason = BaseParameter.DataGridView4[III].Reason;
                                var PART_IDX = BaseParameter.DataGridView4[III].PART_IDX;
                                var TMMTIN_SHEETNO = BaseParameter.DataGridView4[III].TMMTIN_SHEETNO == null ? 0 : BaseParameter.DataGridView4[III].TMMTIN_SHEETNO;

                                if (TMMTIN_DMM_IDX == null)
                                {
                                    TMMTIN_DMM_IDX = 0;
                                }
                                if (ReasonID == null)
                                {
                                    ReasonID = 0;
                                }

                                string sql = @"SELECT  TMBRCD.`BARCD_ID`, TMBRCD.`DSCN_YN` AS `BC_DSCN_YN`, TMBRCD.`BARCD_IDX` FROM TMBRCD  WHERE NOT(TMBRCD.`DSCN_YN` = 'Y') AND TMBRCD.`BBCO` = 'Y'  AND TMBRCD.`BARCD_ID` = '" + V_BARID_TEXT + "'   ";

                                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DGV_B04_BCTXT = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DGV_B04_BCTXT.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (result.DGV_B04_BCTXT.Count < 0)
                                {
                                    DGV_result2 = DGV_result2 + " U";
                                    result.DataGridView4[III].RESULT = DGV_result1 + DGV_result2;
                                    result.DGV_result3 = false;
                                }
                                if (V_USE_YN == "Y")
                                {
                                    DGV_result2 = DGV_result2 + " U";
                                    result.DataGridView4[III].RESULT = DGV_result1 + DGV_result2;
                                    result.DGV_result3 = false;
                                }

                                sql = @"SELECT  `QTY`, `IV_IDX`  FROM     tiivtr   WHERE  `PART_IDX` = '" + V_PART_IDX + "'   AND   `LOC_IDX` = '1'  ";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DGV_B09_91 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DGV_B09_91.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (result.DGV_B09_91.Count < 0)
                                {
                                    DGV_result2 = DGV_result2 + " D";
                                    result.DataGridView4[III].RESULT = DGV_result1 + DGV_result2;
                                    result.DGV_result3 = false;
                                }
                                var V_IV_QTY = result.DGV_B09_91[0].QTY;
                                var V_IV_IDX = result.DGV_B09_91[0].IV_IDX;
                                if (V_IV_QTY < V_OUT_QTY)
                                {
                                    DGV_result2 = DGV_result2 + " C";
                                    result.DataGridView4[III].RESULT = DGV_result1 + DGV_result2;
                                    result.DGV_result3 = false;
                                }

                                sql = @"SELECT `A`.BARCD_IDX, left(`A`.`BARCD_ID`, POSITION('$$' IN `A`.`BARCD_ID`)-1) AS `PART_NO`, `B`.`MTIN_DTM` FROM TMBRCD `A`, TMMTIN `B` WHERE `A`.MTIN_IDX = `B`.MTIN_IDX AND  left(`A`.`BARCD_ID`, POSITION('$$' IN `A`.`BARCD_ID`)-1) = '" + V_PART_NO + "' AND `A`.`DSCN_YN`= 'N' AND ((WEEK(`B`.`MTIN_DTM`) + YEAR(`B`.`MTIN_DTM`)*100) < (WEEK('" + V_MTIN_DTM + "')+YEAR('" + V_MTIN_DTM + "')*100)) AND `A`.`BBCO` = 'Y' ORDER BY `BARCD_IDX`";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DGV_B04_02 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DGV_B04_02.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (result.DGV_B04_02.Count < 0)
                                {
                                    DGV_result2 = DGV_result2 + " FIFO";
                                    result.DataGridView4[III].RESULT = DGV_result1 + DGV_result2;
                                    result.DGV_result3 = false;
                                }

                                if (result.DGV_result3 == true)
                                {
                                    string UPDATE_USER = BaseParameter.USER_ID;
                                    sql = @"UPDATE  TMBRCD   SET  `DSCN_YN` = IF(`PKG_QTY` = `PKG_OUTQTY` + " + V_OUT_QTY + ", 'Y' , 'S'), `PKG_OUTQTY` = IF(`PKG_QTY` <= `PKG_OUTQTY` +" + V_OUT_QTY + ", `PKG_QTY`, `PKG_OUTQTY` +" + V_OUT_QTY + "), `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + UPDATE_USER + "',  `OUT_DTM` = NOW()  WHERE  `BARCD_IDX` = '" + V_BARCD_IDX + "'   ";
                                    string sqlResult01 = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    if (V_OUT_QTY < MAX_SNP)
                                    {
                                        var BARCODE_QR = V_BARID_TEXT;
                                        var BARCODE_AA = V_PKG_GRP;
                                        var BARCODE_BB = (MAX_SNP - V_OUT_QTY) + " (S)";
                                        var BARCODE_CC = result.DataGridView4[III].QTY.Value.ToString();
                                        var BARCODE_DD = V_PART_NO;
                                        var BARCODE_EE = result.DataGridView4[III].PART_NM;
                                        var BARCODE_FF = result.DataGridView4[III].SHPD_NO;
                                        var BARCODE_HH = result.DataGridView4[III].LOC;
                                        var BARCODE_ZZ = result.DataGridView4[III].MTIN_DTM.Value.ToString("yyyy-MM-dd");
                                        var BARCODE_GG = GlobalHelper.InitializationString;
                                        //var BARCODE_CC1 = V_OUT_QTY.ToString();
                                        var BARCODE_CC1 = BARCODE_BB;
                                        HTMLContent.AppendLine(GlobalHelper.CreateHTMLB04(SheetName, _WebHostEnvironment.WebRootPath, BARCODE_QR, BARCODE_AA, BARCODE_BB, BARCODE_CC, BARCODE_DD, BARCODE_EE, BARCODE_FF, BARCODE_GG, BARCODE_HH, BARCODE_ZZ, BARCODE_CC1));
                                        //sql = @"INSERT INTO `TMBRCD_HIS` (`BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, `PKG_OUTQTY`, `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, `TMMTIN_DMM_IDX`, `PART_IDX`, `Name`, `ReasonID`, `Reason`) 
                                        //SELECT `BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, " + V_OUT_QTY + ", `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, " + TMMTIN_DMM_IDX + ", " + PART_IDX + ", '" + Name + "', " + ReasonID + ", '" + Reason + "'  FROM TMBRCD   WHERE TMBRCD.BARCD_ID = '" + V_BARID_TEXT + "'";
                                        //sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                    else
                                    {
                                        //if (V_USE_YN == "S")
                                        //{
                                        //    sql = @"INSERT INTO `TMBRCD_HIS` (`BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, `PKG_OUTQTY`, `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, `TMMTIN_DMM_IDX`, `PART_IDX`, `Name`, `ReasonID`, `Reason`) 
                                        //    SELECT `BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, " + V_OUT_QTY + ", `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, " + TMMTIN_DMM_IDX + ", " + PART_IDX + ", '" + Name + "', " + ReasonID + ", '" + Reason + "'  FROM TMBRCD   WHERE TMBRCD.BARCD_ID = '" + V_BARID_TEXT + "'";
                                        //    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                        //}

                                    }

                                    sql = @"INSERT INTO `TMBRCD_HIS` (`BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, `PKG_OUTQTY`, `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, `TMMTIN_DMM_IDX`, `PART_IDX`, `Name`, `TMMTIN_SHEETNO`, `ReasonID`, `Reason`) 
                                            SELECT `BARCD_IDX`, `BARCD_ID`, `PKG_GRP_IDX`, `PKG_GRP`, `PKG_QTY`, " + V_OUT_QTY + ", `DSCN_YN`, `OUT_DTM`, `MTIN_IDX`, `BARCD_LOC`, `CREATE_DTM`, `CREATE_USER`, `UPDATE_DTM`, `UPDATE_USER`, `BBCO`, " + TMMTIN_DMM_IDX + ", " + PART_IDX + ", '" + Name + "', " + TMMTIN_SHEETNO + ", " + ReasonID + ", '" + Reason + "' FROM TMBRCD   WHERE TMBRCD.BARCD_ID = '" + V_BARID_TEXT + "'";
                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);


                                    sql = @"UPDATE tiivtr   SET  `QTY` = (`QTY` - " + V_OUT_QTY + "),  `UPDATE_DTM` = NOW(),  `UPDATE_USER` = '" + UPDATE_USER + "'  WHERE  `IV_IDX` = '" + V_IV_IDX + "'  ";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES (" + V_PART_IDX + ", 7, " + V_OUT_QTY + ", NOW(), '" + UPDATE_USER + "' ) ON DUPLICATE KEY UPDATE  `LOC_IDX`= 7, `QTY` = (`QTY` +" + V_OUT_QTY + "), `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + UPDATE_USER + "'";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    sql = @"ALTER TABLE     `tiivtr`     AUTO_INCREMENT= 1";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                    result.DataGridView4[III].RESULT = "Complete";
                                }
                            }
                            if (!string.IsNullOrEmpty(HTMLContent.ToString()))
                            {
                                string contentHTML = GlobalHelper.InitializationString;
                                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
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
        public virtual async Task<BaseResult> BTBCCHK_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string pi_barcode = BaseParameter.SearchString.Trim();
                    string sql = @"SELECT  TMMTIN.`PART_IDX`, (SELECT `PART_NO` FROM tspart WHERE TMMTIN.`PART_IDX` = tspart.`PART_IDX`) AS `PART_NO`,
                (SELECT `PART_NM` FROM tspart WHERE TMMTIN.`PART_IDX` = tspart.`PART_IDX`) AS `PART_NM`, 
                IF(TMBRCD.`PKG_OUTQTY` = 0, TMBRCD.`PKG_QTY` , TMBRCD.`PKG_QTY`- TMBRCD.`PKG_OUTQTY`) AS `PKG_QTY`,
                TMMTIN.`MTIN_DTM`, TMMTIN.`QTY`, TMBRCD.`PKG_GRP`,  TMMTIN.`PLET_NO`, TMMTIN.`SHPD_NO`, 
                TMBRCD.`BARCD_ID`, TMBRCD.`DSCN_YN` AS `BC_DSCN_YN`, TMMTIN.`PART_IDX`, TMBRCD.`BARCD_IDX`, TMBRCD.`PKG_GRP_IDX`, 
                (TMBRCD.`PKG_QTY`- TMBRCD.`PKG_OUTQTY`) AS `MAX_PKG`, (SELECT `PART_LOC` FROM tspart WHERE TMMTIN.`PART_IDX` = tspart.`PART_IDX`) AS `LOC`
                FROM      TMMTIN, TMBRCD   WHERE  TMMTIN.`MTIN_IDX` = TMBRCD.`MTIN_IDX`
                AND TMMTIN.`DSCN_YN` = 'Y' AND NOT(TMBRCD.`DSCN_YN` = 'Y') AND   TMBRCD.`BBCO` = 'Y'  AND TMBRCD.`BARCD_ID` = '" + pi_barcode + "'";


                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DGV_B04_03 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DGV_B04_03.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }

                    result.DataGridView9 = new List<SuperResultTranfer>();

                    if (result.DGV_B04_03.Count > 0)
                    {
                        var ListPART_IDX = result.DGV_B04_03.Select(x => x.PART_IDX).Distinct().ToList();
                        string V_PART_IDX = string.Join(",", ListPART_IDX);
                        string End = DateTime.Now.ToString("yyyy-MM-dd");
                        string Begin = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");

                        sql = @"    SELECT * FROM (

                                    SELECT 

                                    distinct

                                    case when RIGHT(b.CD_SYS_NOTE, 2) <= 60 then 'Factory 1' ELSE 'Factory 2' END AS 'Name'

                                    , a.TMMTIN_SHEETNO      

                                    , a.TMMTIN_DMM_IDX

                                    , a.TMMTIN_DATE

                                    FROM tmmtin_dmm a JOIN tscode b ON a.TMMTIN_DMM_STGC=b.CD_IDX

                                    WHERE a.TMMTIN_CODE='Material' AND a.TMMTIN_REC_YN='Y' AND a.TMMTIN_CNF_YN='Y' AND NOT(a.TMMTIN_DSCN_YN='Y') AND a.TMMTIN_PART IN (" + V_PART_IDX + ") AND (a.TMMTIN_DATE BETWEEN '" + Begin + "' AND '" + End + "')) a ORDER BY a.Name asc, a.TMMTIN_DATE desc, a.TMMTIN_SHEETNO asc";

                        //sql = @"    SELECT * FROM (

                        //            SELECT 

                        //            distinct

                        //            case when RIGHT(b.CD_SYS_NOTE, 2) <= 60 then 'Factory 1' ELSE 'Factory 2' END AS 'Name'

                        //            , a.TMMTIN_SHEETNO     

                        //            , a.TMMTIN_SHEETNO AS `TMMTIN_DMM_IDX`

                        //            , a.TMMTIN_DATE

                        //            FROM tmmtin_dmm a JOIN tscode b ON a.TMMTIN_DMM_STGC=b.CD_IDX

                        //            WHERE a.TMMTIN_CODE='Material' AND a.TMMTIN_REC_YN='Y' AND a.TMMTIN_CNF_YN='Y' AND NOT(a.TMMTIN_DSCN_YN='Y') AND (a.TMMTIN_DATE BETWEEN '" + Begin + "' AND '" + End + "')) a ORDER BY a.Name asc, a.TMMTIN_DATE desc, a.TMMTIN_SHEETNO asc";

                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView9 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView9.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        if (result.DataGridView9.Count > 0)
                        {
                            for (int i = 0; i < result.DataGridView9.Count; i++)
                            {
                                result.DataGridView9[i].Name = result.DataGridView9[i].TMMTIN_SHEETNO + " [" + result.DataGridView9[i].TMMTIN_DATE.Value.ToString("yyyy-MM-dd") + " | " + result.DataGridView9[i].Name + "]";
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
        public virtual async Task<BaseResult> BTBCCHK_ClickSub(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        string R1 = BaseParameter.ListSearchString[0];
                        string R4 = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd");
                        string sql = @"SELECT `A`.BARCD_IDX, left(`A`.`BARCD_ID`, POSITION('$$' IN `A`.`BARCD_ID`)-1) AS `PART_NO`, `B`.`MTIN_DTM` FROM TMBRCD `A`, TMMTIN `B` WHERE `A`.MTIN_IDX = `B`.MTIN_IDX AND  left(`A`.`BARCD_ID`, POSITION('$$' IN `A`.`BARCD_ID`) - 1) = '" + R1 + "'       AND(`A`.`DSCN_YN`= 'N'  OR  `A`.`DSCN_YN`= 'S') AND NOT(`A`.`PKG_QTY` = `A`.`PKG_OUTQTY`)       AND((WEEK(`B`.`MTIN_DTM`) + YEAR(`B`.`MTIN_DTM`) * 100) < (WEEK('" + R4 + "') + YEAR('" + R4 + "') * 100)) AND `A`.`BBCO` = 'Y' ORDER BY `BARCD_IDX`";


                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_B04_02 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_B04_02.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        private async Task<List<SuperResultTranfer>> CALL_STOCK(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            string TBTEXT01 = BaseParameter.ListSearchString[0];
            string sql = @"SELECT (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) AS `PART_NO`,
            (SELECT tspart.PART_NM FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) AS `PART_NM`, tiivtr.QTY,
            IFNULL((SELECT tiivtr_EXCEL.QTY FROM tiivtr_EXCEL WHERE tiivtr_EXCEL.`PART_NO` = '" + TBTEXT01 + "'), 0) AS `MT_EXCEL` FROM tiivtr WHERE tiivtr.LOC_IDX = '1' AND tiivtr.PART_IDX = (SELECT tspart.PART_IDX FROM tspart WHERE  tspart.PART_NO = '" + TBTEXT01 + "')";

            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.ListSuperResultTranfer = new List<SuperResultTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.ListSuperResultTranfer.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }
            return result.ListSuperResultTranfer;
        }
        public virtual async Task<BaseResult> GetSHEETNOToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        string Now = DateTime.Now.ToString("yyyy-MM-dd");
                        string sql = @"SELECT 

                                    distinct

                                    case when RIGHT(b.CD_SYS_NOTE, 2) <= 60 then 'Factory 1' ELSE 'Factory 2' END AS 'Name'

                                    , a.TMMTIN_SHEETNO 

                                    FROM tmmtin_dmm a JOIN tscode b ON a.TMMTIN_DMM_STGC=b.CD_IDX

                                    WHERE a.TMMTIN_CODE='Material' AND a.TMMTIN_REC_YN='Y' AND a.TMMTIN_CNF_YN='Y' AND NOT(a.TMMTIN_DSCN_YN='Y') AND a.TMMTIN_DATE='" + Now + "'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView9 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView9.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> GetFIFOReasonToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string SearchDate = DateTime.Now.ToString("yyyy-MM-dd");

                string sql = @"SELECT a.* FROM tmbrcd_his a WHERE (a.ReasonID>0 OR a.TMMTIN_DMM_IDX=0) AND a.OUT_DTM LIKE '%" + SearchDate + "%' ORDER BY a.OUT_DTM DESC";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView6 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DataGridView6.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> GettmbrcdAndtmbrcd_hisToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        string SearchString = BaseParameter.ListSearchString[0];
                        string Year = BaseParameter.ListSearchString[1];
                        string Month = BaseParameter.ListSearchString[2];
                        string sql = "";
                        if (!string.IsNullOrEmpty(SearchString))
                        {
                            sql = @"SELECT 
                                * 

                                , (a.OUTQTY - a.PKG_OUTQTY) AS GAP

                                FROM (

                                SELECT 

                                SUBSTRING_INDEX(a.BARCD_ID, '$$', 1) AS PART_NO

                                , a.BARCD_IDX

                                , a.BARCD_ID

                                , a.PKG_QTY

                                , a.PKG_OUTQTY

                                , SUM(b.PKG_OUTQTY) AS OUTQTY

                                FROM tmbrcd a JOIN tmbrcd_his b ON a.BARCD_IDX=b.BARCD_IDX

                                WHERE b.BARCD_ID LIKE '%" + SearchString + "%' GROUP BY a.BARCD_IDX, a.BARCD_ID, a.PKG_QTY, a.PKG_OUTQTY) a WHERE a.PKG_OUTQTY <> a.OUTQTY ORDER BY a.PART_NO ASC, a.OUTQTY DESC ";
                        }
                        else
                        {
                            if (Month == "00")
                            {
                                SearchString = Year;
                            }
                            else
                            {
                                SearchString = Year + "-" + Month;
                            }
                            sql = @"SELECT 
                                * 

                                , (a.OUT_QTY - a.PKG_OUTQTY) AS GAP

                                FROM (

                                SELECT 

                                SUBSTRING_INDEX(a.BARCD_ID, '$$', 1) AS PART_NO

                                , a.BARCD_IDX

                                , a.BARCD_ID

                                , a.PKG_QTY

                                , a.PKG_OUTQTY

                                , SUM(b.PKG_OUTQTY) AS OUT_QTY

                                FROM tmbrcd a JOIN tmbrcd_his b ON a.BARCD_IDX=b.BARCD_IDX

                                WHERE b.OUT_DTM LIKE '%" + SearchString + "%' GROUP BY a.BARCD_IDX, a.BARCD_ID, a.PKG_QTY, a.PKG_OUTQTY) a WHERE a.PKG_OUTQTY <> a.OUTQTY ORDER BY a.PART_NO ASC, a.OUTQTY DESC ";
                        }

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_B07_31 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_B07_31.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> Gettmbrcd_hisByBARCD_IDXToListAsync(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        string SearchString = BaseParameter.ListSearchString[0];

                        string sql = @"SELECT 

                            a.*

                            , b.USER_NM

                            FROM tmbrcd_his a JOIN tsuser b ON a.UPDATE_USER=b.USER_ID

                            WHERE a.BARCD_IDX =" + SearchString + " ORDER BY a.OUT_DTM DESC";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_B07_32 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_B07_32.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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


