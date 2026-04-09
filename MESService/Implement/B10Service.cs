namespace MESService.Implement
{
    public class B10Service : BaseService<torderlist, ItorderlistRepository>
    , IB10Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B10Service(ItorderlistRepository torderlistRepository
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
                            string CB_FCTRY1 = BaseParameter.ListSearchString[0].ToString();
                            string ComboBox1 = BaseParameter.ListSearchString[1].ToString();
                            string CB2_TEXT = BaseParameter.ListSearchString[2].ToString();
                            string DateTimePicker2 = BaseParameter.ListSearchString[3].ToString();
                            DateTimePicker2 = DateTime.Parse(DateTimePicker2).ToString("yyyy-MM-dd");
                            string FAC_ST_1 = "";
                            if (CB_FCTRY1 == "Factory 1")
                            {
                                FAC_ST_1 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17'), 2) <= 60";
                            }
                            else
                            {
                                FAC_ST_1 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17'), 2) > 60";
                            }
                            if (CB2_TEXT == "ALL")
                            {
                                CB2_TEXT = "%%";
                            }
                            string sql = @"SELECT (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17') AS `STAGE`, `TMMTIN_DATE` AS `DATE` FROM   TMMTIN_DMM   WHERE   `TMMTIN_REC_YN` = 'Y' AND `TMMTIN_CNF_YN` = 'N' AND `TMMTIN_DATE` = '" + DateTimePicker2 + "' AND `TMMTIN_CODE` = '" + ComboBox1 + "'   " + FAC_ST_1 + " GROUP BY `TMMTIN_DATE`, `STAGE` HAVING `STAGE` LIKE '" + CB2_TEXT + "'    ";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView2 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            if (result.DataGridView2.Count > 0)
                            {
                                for (int i = 0; i < result.DataGridView2.Count; i++)
                                {
                                    result.DataGridView2[i].Name = result.DataGridView2[i].DATE.Value.ToString("yyyy-MM-dd");
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string CB_FCTRY3 = BaseParameter.ListSearchString[0].ToString();
                            string T3_S1 = BaseParameter.ListSearchString[1].ToString();
                            string T3_S2 = BaseParameter.ListSearchString[2].ToString();
                            string T3_S3 = BaseParameter.ListSearchString[3].ToString();
                            string T3_S4 = BaseParameter.ListSearchString[4].ToString();
                            string ComboBox4 = BaseParameter.ListSearchString[5].ToString();

                            string CB3_TEXT = T3_S1;
                            string DGV_D1 = DateTime.Parse(T3_S2).ToString("yyyy-MM-dd");
                            string FAC_ST_3 = "";
                            if (CB_FCTRY3 == "Factory 1")
                            {
                                FAC_ST_3 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17'), 2) <= 60";
                            }
                            else
                            {
                                FAC_ST_3 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17'), 2) > 60";
                            }
                            string DGV_D2 = "";
                            if (CB3_TEXT == "ALL")
                            {
                                CB3_TEXT = "%%";
                                DGV_D2 = "ALL";
                            }
                            string sql = @"SELECT  `TMMTIN_DSCN_YN` AS  `DSCN`,
(SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17') AS `STAGE`, `TMMTIN_DATE` AS `DATE`, 
`TMMTIN_PART` AS `DJG_CODE`, 
IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NO`,
IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_DESC` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_NM` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NAME`,
IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_SIZE` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_FML` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `FAMILY`,
`TMMTIN_PART_SNP` AS `SNP`, SUM(`TMMTIN_QTY`) AS `QTY`, 

IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_LOC` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `TMPB_A`.`LOC` FROM (SELECT 
ROW_NUMBER() OVER (PARTITION BY (SELECT TMMTIN.PART_IDX FROM TMMTIN WHERE TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX) ORDER BY (SELECT TMMTIN.MTIN_DTM FROM TMMTIN WHERE TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX) DESC ) AS `RN`,
(SELECT TMMTIN.PART_IDX FROM TMMTIN WHERE TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX) AS `PART_IDX`,
IFNULL(TMBRCD.BARCD_LOC, 
(SELECT tspart.PART_LOC FROM tspart WHERE tspart.PART_IDX = (SELECT TMMTIN.PART_IDX FROM TMMTIN WHERE TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX))) AS `LOC`
FROM TMBRCD  WHERE  NOT(TMBRCD.DSCN_YN = 'Y') AND TMBRCD.BBCO ='Y') `TMPB_A` WHERE `TMPB_A`.`RN` = '1' AND  `TMPB_A`.`PART_IDX` = TMMTIN_DMM.`TMMTIN_PART` )  ) AS `LOC`, 

IFNULL(SUM(`TMMTIN_QTY`) / `TMMTIN_PART_SNP`, 0) AS `BOX`,
`TMMTIN_DMM_IDX` AS `CODE`, `TMMTIN_SHEETNO`
FROM TMMTIN_DMM WHERE `TMMTIN_REC_YN` = 'Y'  AND `TMMTIN_CNF_YN` = 'Y'  AND NOT(`TMMTIN_DSCN_YN` = 'Y')   AND  `TMMTIN_DATE` = '" + DGV_D1 + "'   AND `TMMTIN_CODE` = '" + ComboBox4 + "' " + FAC_ST_3 + " GROUP BY  `STAGE` , `DATE` , `PART_NO` HAVING  `STAGE` LIKE '" + CB3_TEXT + "' AND  `TMMTIN_SHEETNO` LIKE '%" + T3_S4 + "%' AND(`PART_NO`  LIKE '%" + T3_S3 + "%'  OR  `PART_NAME`  LIKE '%" + T3_S3 + "%'  OR `FAMILY` LIKE '%" + T3_S3 + "%')";
                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView4 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            if (result.DataGridView4.Count > 0)
                            {
                                for (int i = 0; i < result.DataGridView4.Count; i++)
                                {
                                    result.DataGridView4[i].Name = result.DataGridView4[i].DATE.Value.ToString("yyyy-MM-dd");
                                }
                            }
                        }

                    }
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string CB_FCTRY2 = BaseParameter.ListSearchString[0].ToString();
                            string T2_S1 = BaseParameter.ListSearchString[1].ToString();
                            string T2_S2 = BaseParameter.ListSearchString[2].ToString();
                            string T2_S3 = BaseParameter.ListSearchString[3].ToString();
                            string T2_S4 = BaseParameter.ListSearchString[4].ToString();
                            string T2_S5 = BaseParameter.ListSearchString[5].ToString();
                            string T2_S6 = BaseParameter.ListSearchString[6].ToString();
                            string ComboBox3 = BaseParameter.ListSearchString[7].ToString();



                            string FAC_ST_2 = "";
                            if (CB_FCTRY2 == "Factory 1")
                            {
                                FAC_ST_2 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17'), 2) <= 60";
                            }
                            else
                            {
                                FAC_ST_2 = " AND RIGHT((SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17'), 2) > 60";
                            }

                            string T2_CB_TEXT = "";

                            if (T2_S1 == "ALL")
                            {
                                T2_CB_TEXT = "%%";
                            }
                            else
                            {
                                T2_CB_TEXT = T2_S1;
                            }

                            string CB_DATE = "";

                            if (T2_S6 == "ALL")
                            {
                                CB_DATE = "";
                            }

                            if (T2_S6 == "Y")
                            {
                                CB_DATE = " AND  `TMMTIN_DSCN_YN` = 'Y'";
                            }
                            if (T2_S6 == "N")
                            {
                                CB_DATE = " AND  `TMMTIN_DSCN_YN` = 'N'";
                            }
                            string sql = "";
                            if (BaseParameter.CheckBox1 == false)
                            {
                                T2_S2 = DateTime.Parse(T2_S2).ToString("yyyy-MM-dd");

                                sql = @"SELECT 
                                    TMMTIN_DMM_IDX, `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`, `TMMTIN_PART` AS `DJG_CODE`, `TMMTIN_PART` AS `PART_IDX`, 
                                    IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NO`,
                                    `TMMTIN_PART_SNP` AS `SNP`, `TMMTIN_QTY` AS `QTY`, 
                                    IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_DESC` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_NM` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NAME`,
                                    `CREATE_DTM`, IF(`TMMTIN_REC_YN` = 'Y', 'ORDER', 'CANCEL') AS `ORDER`, 
                                    `TMMTIN_DMM_IDX` AS `CODE`, (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17') AS `STAGE`, `TMMTIN_DATE` AS `DATE`, 
                                    IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_SIZE` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_FML` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `FAMILY`,
                                    `TMMTIN_SHEETNO`,
                                    IF(`TMMTIN_DSCN_YN` = 'N', 0 , CEIL(IFNULL((`TMMTIN_QTY` / `TMMTIN_PART_SNP`), 1))) AS `BOX_COUNT`
                                    FROM TMMTIN_DMM 
                                    WHERE `TMMTIN_DATE` = '" + T2_S2 + "' AND  `TMMTIN_CODE` = '" + ComboBox3 + "'   " + CB_DATE + FAC_ST_2 + "  HAVING  `STAGE` LIKE '" + T2_CB_TEXT + "'  AND  `TMMTIN_SHEETNO` LIKE '%" + T2_S5 + "%'  AND `PART_NO` LIKE '%" + T2_S3 + "%' AND (`PART_NAME` LIKE '%" + T2_S4 + "%' OR `FAMILY` LIKE '%" + T2_S4 + "%') ORDER BY IF(`TMMTIN_DSCN_YN` = 'N', 1, IF(`TMMTIN_DSCN_YN` = 'C', 2, 3)) ASC";
                            }
                            else
                            {
                                T2_S2 = DateTime.Parse(T2_S2).ToString("yyyy-MM");

                                sql = @"SELECT 
                                   TMMTIN_DMM_IDX, `TMMTIN_CNF_YN`, `TMMTIN_DSCN_YN`, `TMMTIN_PART` AS `DJG_CODE`, `TMMTIN_PART` AS `PART_IDX`, 
                                    IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NO`,
                                    `TMMTIN_PART_SNP` AS `SNP`, `TMMTIN_QTY` AS `QTY`, 
                                    IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_DESC` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_NM` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NAME`,
                                    `CREATE_DTM`, IF(`TMMTIN_REC_YN` = 'Y', 'ORDER', 'CANCEL') AS `ORDER`, 
                                    `TMMTIN_DMM_IDX` AS `CODE`, (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17') AS `STAGE`, `TMMTIN_DATE` AS `DATE`, 
                                    IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_SIZE` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_FML` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `FAMILY`,
                                    `TMMTIN_SHEETNO`,
                                    IF(`TMMTIN_DSCN_YN` = 'N', 0 , CEIL(IFNULL((`TMMTIN_QTY` / `TMMTIN_PART_SNP`), 1))) AS `BOX_COUNT`
                                    FROM TMMTIN_DMM 
                                    WHERE `TMMTIN_DATE` >= '" + T2_S2 + "-01' AND `TMMTIN_DATE` <= '" + T2_S2 + "-31'  AND  `TMMTIN_CODE` = '" + ComboBox3 + "' " + CB_DATE + FAC_ST_2 + " HAVING   `STAGE` LIKE '" + T2_CB_TEXT + "' AND  `TMMTIN_SHEETNO` LIKE '%" + T2_S5 + "%'  AND `PART_NO` LIKE '%" + T2_S3 + "%' AND(`PART_NAME` LIKE '%" + T2_S4 + "%' OR `FAMILY` LIKE '%" + T2_S4 + "%') ORDER BY IF(`TMMTIN_DSCN_YN` = 'N', 1, IF(`TMMTIN_DSCN_YN` = 'C', 2, 3)) ASC";
                            }

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.T2_DGV1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.T2_DGV1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            if (result.T2_DGV1.Count > 0)
                            {
                                var ListTMMTIN_DMM_IDX = result.T2_DGV1.Select(x => x.TMMTIN_DMM_IDX).ToList();
                                var ListTMMTIN_DMM_IDX_String = string.Join(",", ListTMMTIN_DMM_IDX);
                                ListTMMTIN_DMM_IDX_String = ListTMMTIN_DMM_IDX_String + ",0";
                                //sql = @"SELECT a.TMMTIN_DMM_IDX, a.PART_IDX, SUM(a.PKG_OUTQTY) AS PKG_OUTQTY FROM tmbrcd_his a WHERE a.TMMTIN_DMM_IDX IN (" + ListTMMTIN_DMM_IDX_String + ") GROUP BY a.TMMTIN_DMM_IDX, a.PART_IDX";
                                sql = @"SELECT * FROM tmbrcd_his a WHERE a.OUT_DTM LIKE '%" + T2_S2 + "%' ORDER BY PART_IDX ASC";
                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                result.DataGridView0 = new List<SuperResultTranfer>();
                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.DataGridView0.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                                if (result.DataGridView0.Count > 0)
                                {
                                    for (int i = 0; i < result.T2_DGV1.Count; i++)
                                    {
                                        if (result.T2_DGV1[i].PART_IDX > 0)
                                        {
                                            var DataGridView0Sub = result.DataGridView0.Where(o => o.PART_IDX == result.T2_DGV1[i].PART_IDX && o.TMMTIN_SHEETNO == result.T2_DGV1[i].TMMTIN_SHEETNO && o.TMMTIN_DMM_IDX > 0).ToList();
                                            //if (DataGridView0Sub.Count == 0)
                                            //{
                                            //    DataGridView0Sub = result.DataGridView0.Where(o => o.PART_IDX == result.T2_DGV1[i].PART_IDX && o.TMMTIN_DMM_IDX == result.T2_DGV1[i].TMMTIN_DMM_IDX).ToList();
                                            //}
                                            //if (DataGridView0Sub.Count == 0)
                                            //{
                                            //    DataGridView0Sub = result.DataGridView0.Where(o => o.PART_IDX == result.T2_DGV1[i].PART_IDX).ToList();
                                            //}                                            
                                            var PKG_OUTQTY = DataGridView0Sub.Sum(o => o.PKG_OUTQTY);
                                            result.T2_DGV1[i].QuantityActual = PKG_OUTQTY;
                                            result.T2_DGV1[i].QuantityGAP = result.T2_DGV1[i].QuantityActual - result.T2_DGV1[i].QTY;
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
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            if (BaseParameter.DataGridView3 != null)
                            {
                                if (BaseParameter.DataGridView3.Count > 0)
                                {
                                    var DGV2_D1 = BaseParameter.ListSearchString[0].ToString();
                                    var DGV2_D2 = BaseParameter.ListSearchString[1].ToString();
                                    var T1_TYPE = BaseParameter.ListSearchString[2].ToString();

                                    string sql = @"SELECT IFNULL(MAX(`TMMTIN_SHEETNO`),0) + 1 AS `SHEET_NO` FROM  `TMMTIN_DMM`   WHERE   `TMMTIN_CODE` = '" + T1_TYPE + "'  AND   `TMMTIN_DATE` = '" + DGV2_D1 + "'";

                                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    var DGV_B10_NO = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        DGV_B10_NO.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }
                                    if (DGV_B10_NO != null)
                                    {
                                        if (DGV_B10_NO.Count > 0)
                                        {
                                            var SHEET_NO = DGV_B10_NO[0].SHEET_NO;

                                            SuperResultTranfer DataGridView3Item = new SuperResultTranfer();

                                            StringBuilder Detail = new StringBuilder();
                                            int no = 0;
                                            BaseParameter.DataGridView3 = BaseParameter.DataGridView3.OrderByDescending(x => x.DATE).ToList();
                                            foreach (var item in BaseParameter.DataGridView3)
                                            {
                                                if (item.CHK == true)
                                                {
                                                    no = no + 1;
                                                    if (no == 1)
                                                    {
                                                        DataGridView3Item = item;
                                                    }
                                                    var DJG_CODEIDX = item.DJG_CODE;
                                                    sql = @"UPDATE `TMMTIN_DMM`     SET  `TMMTIN_CNF_YN`='Y', `TMMTIN_SHEETNO` = '" + SHEET_NO + "' WHERE  `TMMTIN_DMM_STGC` = (SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` = '" + DGV2_D2 + "'  AND `CDGR_IDX` = '17') AND  `TMMTIN_DATE` = '" + DGV2_D1 + "' AND `TMMTIN_REC_YN`= 'Y' AND `TMMTIN_CNF_YN` = 'N' AND `TMMTIN_PART` = '" + DJG_CODEIDX + "'";
                                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                                    Detail.AppendLine(@"<tr>");
                                                    Detail.AppendLine(@"<td>" + no + "</td>");
                                                    Detail.AppendLine(@"<td><b style='font-size: 16px;'>" + item.PART_NO + "</b></td>");
                                                    Detail.AppendLine(@"<td>" + item.SNP + "</td>");
                                                    Detail.AppendLine(@"<td>" + item.QTY + "</td>");
                                                    Detail.AppendLine(@"<td></td>");
                                                    Detail.AppendLine(@"<td><b style='font-size: 16px;'>" + item.LOC + "</b></td>");
                                                    Detail.AppendLine(@"<td></td>");
                                                    try
                                                    {
                                                        var BOX = decimal.Parse(item.BOX);
                                                        Detail.AppendLine(@"<td>" + BOX.ToString("N2") + " Boxes</td>");
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Detail.AppendLine(@"<td></td>");
                                                    }

                                                    try
                                                    {
                                                        var tmmtin = new List<SuperResultTranfer>();
                                                        //sql = @"SELECT * FROM tmmtin WHERE PART_IDX IN (SELECT PART_IDX FROM tspart WHERE PART_NO ='" + item.PART_NO + "') ORDER BY MTIN_DTM DESC LIMIT 1";
                                                        //for (int i = 0; i < ds.Tables.Count; i++)
                                                        //{
                                                        //    DataTable dt = ds.Tables[i];
                                                        //    tmmtin.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                                        //}
                                                        if (tmmtin.Count > 1)
                                                        {
                                                            Detail.AppendLine(@"<td>" + tmmtin[0].MTIN_DTM.Value.ToString("yyyy-MM-dd") + "</td>");
                                                        }
                                                        else
                                                        {
                                                            Detail.AppendLine(@"<td>" + item.DATE.Value.ToString("yyyy-MM-dd") + "</td>");
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Detail.AppendLine(@"<td></td>");
                                                    }
                                                    Detail.AppendLine(@"</tr>");

                                                }
                                            }

                                            string SheetName = this.GetType().Name;
                                            string contentHTML = GlobalHelper.InitializationString;
                                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "B10.html");
                                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                            {
                                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                                {
                                                    contentHTML = r.ReadToEnd();
                                                }
                                            }

                                            contentHTML = contentHTML.Replace(@"[UserCode]", BaseParameter.USER_ID);
                                            contentHTML = contentHTML.Replace(@"[UserName]", BaseParameter.USER_NM);
                                            contentHTML = contentHTML.Replace(@"[STAGE]", DataGridView3Item.STAGE);
                                            contentHTML = contentHTML.Replace(@"[TMMTIN_SHEETNO]", SHEET_NO);
                                            contentHTML = contentHTML.Replace(@"[DATE]", DataGridView3Item.DATE.Value.ToString("yyyy-MM-dd"));
                                            contentHTML = contentHTML.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));


                                            contentHTML = contentHTML.Replace(@"[Detail]", Detail.ToString());
                                            string fileName = "B10_" + GlobalHelper.InitializationDateTimeCode + ".html";
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
                    }
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.DataGridView4 != null)
                        {
                            foreach (var item in BaseParameter.DataGridView4)
                            {
                                if (item.CHK == true)
                                {
                                    var ORDER_CD = item.CODE;
                                    string sql = @"UPDATE `TMMTIN_DMM`   SET  `TMMTIN_DSCN_YN` = 'Y'  WHERE  `TMMTIN_DMM_IDX` = '" + ORDER_CD + "'";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.DataGridView4 != null)
                    {
                        foreach (var item in BaseParameter.DataGridView4)
                        {
                            if (item.CHK == true)
                            {
                                var ORDER_CD = item.CODE;
                                string sql = @"UPDATE `TMMTIN_DMM`   SET  `TMMTIN_DSCN_YN` = 'C'  WHERE  `TMMTIN_DMM_IDX` = '" + ORDER_CD + "'";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.DataGridView4 != null)
                        {
                            string SheetName = this.GetType().Name;
                            string contentHTML = GlobalHelper.InitializationString;
                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "B10.html");
                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    contentHTML = r.ReadToEnd();
                                }
                            }
                            SuperResultTranfer DataGridView4Item = BaseParameter.DataGridView4[0];
                            contentHTML = contentHTML.Replace(@"[UserCode]", BaseParameter.USER_ID);
                            contentHTML = contentHTML.Replace(@"[UserName]", BaseParameter.USER_NM);
                            contentHTML = contentHTML.Replace(@"[STAGE]", DataGridView4Item.STAGE);
                            contentHTML = contentHTML.Replace(@"[TMMTIN_SHEETNO]", DataGridView4Item.TMMTIN_SHEETNO.ToString());
                            contentHTML = contentHTML.Replace(@"[DATE]", DataGridView4Item.DATE.Value.ToString("yyyy-MM-dd"));
                            contentHTML = contentHTML.Replace(@"[Day]", GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

                            StringBuilder Detail = new StringBuilder();
                            int no = 0;
                            foreach (SuperResultTranfer item in BaseParameter.DataGridView4)
                            {
                                no = no + 1;
                                Detail.AppendLine(@"<tr>");
                                Detail.AppendLine(@"<td>" + no + "</td>");
                                Detail.AppendLine(@"<td>" + item.PART_NO + "</td>");
                                Detail.AppendLine(@"<td>" + item.SNP + "</td>");
                                Detail.AppendLine(@"<td>" + item.QTY + "</td>");
                                Detail.AppendLine(@"<td></td>");
                                Detail.AppendLine(@"<td>" + item.LOC + "</td>");
                                Detail.AppendLine(@"<td></td>");
                                Detail.AppendLine(@"<td>" + item.BOX + " Boxes</td>");
                                SuperResultTranfer SuperResultTranferB04T_DGV_02 = await B04T_DGV_02(item.PART_NO);
                                if (SuperResultTranferB04T_DGV_02.Receipt_Data != null)
                                {
                                    Detail.AppendLine(@"<td>" + DateTime.Parse(SuperResultTranferB04T_DGV_02.Receipt_Data).ToString("yyyy-MM-dd") + "</td>");
                                }
                                else
                                {
                                    Detail.AppendLine(@"<td></td>");
                                }
                                Detail.AppendLine(@"</tr>");
                            }
                            contentHTML = contentHTML.Replace(@"[Detail]", Detail.ToString());
                            string fileName = "B10_" + GlobalHelper.InitializationDateTimeCode + ".html";
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
        public virtual async Task<BaseResult> CB_FCTRY_LINE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE` , `CD_NM_HAN`, `CD_NM_EN` FROM TSCODE WHERE TSCODE.CDGR_IDX = '21' ";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.CB_FCTRY1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.CB_FCTRY1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> COMLIST_LINE_1(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string FAC_ST = BaseParameter.SearchString;
                    string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE` , `CD_NM_HAN`, `CD_NM_EN` FROM TSCODE WHERE `CDGR_IDX` = '17' AND NOT(`CD_SYS_NOTE` = 'Material Payment 01') " + FAC_ST + " ORDER BY `CD_SYS_NOTE`";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.T2_S1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.T2_S1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> COMLIST_LINE_2(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string FAC_ST = BaseParameter.SearchString;
                    string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE` , `CD_NM_HAN`, `CD_NM_EN` FROM TSCODE WHERE `CDGR_IDX` = '17' AND NOT(`CD_SYS_NOTE` = 'Material Payment 01') " + FAC_ST + " ORDER BY `CD_SYS_NOTE`";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.ComboBox2 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.ComboBox2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> COMLIST_LINE_3(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string FAC_ST = BaseParameter.SearchString;
                    string sql = @"SELECT `CD_IDX`, `CD_SYS_NOTE` , `CD_NM_HAN`, `CD_NM_EN` FROM TSCODE WHERE `CDGR_IDX` = '17' AND NOT(`CD_SYS_NOTE` = 'Material Payment 01') " + FAC_ST + " ORDER BY `CD_SYS_NOTE`";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.T3_S1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.T3_S1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> DataT2DGV_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        string DGV_D1 = BaseParameter.ListSearchString[0].ToString();
                        string DGV_D2 = BaseParameter.ListSearchString[1].ToString();
                        string ComboBox1 = BaseParameter.ListSearchString[2].ToString();
                        string sql = @"SELECT  FALSE AS `CHK`, `TMMTIN_DMM_IDX` AS `CODE`, 
                            (SELECT `CD_NM_EN` FROM TSCODE WHERE `CD_IDX` = `TMMTIN_DMM_STGC` AND `CDGR_IDX` = '17') AS `STAGE`, `TMMTIN_DATE` AS `DATE`, 
                            `TMMTIN_PART` AS `DJG_CODE`,  `TMMTIN_CODE`, 
                            IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NO`,
                            IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_DESC` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_NM` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `PART_NAME`,
                            IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_SIZE` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`), (SELECT `PART_FML` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)) AS `FAMILY`,
                            `TMMTIN_PART_SNP` AS `SNP`, SUM(`TMMTIN_QTY`) AS `QTY`,
                            IFNULL((SELECT tiivtr.QTY FROM tiivtr WHERE tiivtr.`PART_IDX` = TMMTIN_DMM.`TMMTIN_PART` AND tiivtr.LOC_IDX='1'), 0) AS `STOCK`,

                            IF(`TMMTIN_CODE` = 'TUBE', (SELECT `TC_LOC` FROM TTC_PART WHERE `TTC_PART_IDX` = `TMMTIN_PART`),  (SELECT tspart.`PART_LOC` FROM tspart WHERE `PART_IDX` = `TMMTIN_PART`)   ) AS `LOC`, 

                            IFNULL(SUM(`TMMTIN_QTY`) / `TMMTIN_PART_SNP`, 0) AS `BOX`
                            FROM TMMTIN_DMM  
                            WHERE `TMMTIN_REC_YN` = 'Y'  AND `TMMTIN_CNF_YN` = 'N'  AND  `TMMTIN_DATE` = '" + DGV_D1 + "' AND `TMMTIN_CODE` = '" + ComboBox1 + "' GROUP BY  `STAGE` , `DATE` , `PART_NO` HAVING `STAGE` = '" + DGV_D2 + "'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView3 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        if (result.DataGridView3.Count > 0)
                        {
                            for (int i = 0; i < result.DataGridView3.Count; i++)
                            {
                                result.DataGridView3[i].Name = result.DataGridView3[i].DATE.Value.ToString("yyyy-MM-dd");
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

        public virtual async Task<SuperResultTranfer> B04T_DGV_02(string PART_NO)
        {
            SuperResultTranfer result = new SuperResultTranfer();
            try
            {
                string AAA = PART_NO;
                string CCC = DateTime.Now.ToString("yyyy-MM-dd");

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
                                DATE_FORMAT(IFNULL(`TMBRCD_SUM`.`UPDATE_DTM`, IFNULL(`TMBRCD_SUM`.`CREATE_DTM`, TMMTIN.`MTIN_DTM`)), '%Y-%m-%d') AS `OR_DT`

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
                                     TMBRCD.`UPDATE_USER`, TMBRCD.`BBCO`
                                     FROM TMBRCD LEFT JOIN TMBRCD_HIS
                                     ON TMBRCD.BARCD_IDX = TMBRCD_HIS.BARCD_IDX
                                     WHERE TMBRCD.BARCD_ID LIKE '" + AAA + "$$%' ) `TMBRCD_SUM` WHERE  `TMBRCD_SUM`.`MTIN_IDX` = TMMTIN.`MTIN_IDX`  AND  `TMBRCD_SUM`.`BBCO` = 'Y' AND (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = (SELECT TMMTIN.`PART_IDX` FROM TMMTIN WHERE TMMTIN.`MTIN_IDX` = `TMBRCD_SUM`.`MTIN_IDX`)) = '" + AAA + "' AND TMMTIN.`MTIN_DTM` >= '" + CCC + "' ORDER BY  `Receipt_Data` ASC    ";

                sql = sql + " LIMIT 1";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                var T_DGV_02 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    T_DGV_02.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
                if (T_DGV_02.Count > 0)
                {
                    result = T_DGV_02[0];
                }
            }
            catch (Exception ex)
            {
                var Message = ex.Message;
            }
            return result;
        }
    }
}

