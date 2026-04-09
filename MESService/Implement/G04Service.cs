namespace MESService.Implement
{
    public class G04Service : BaseService<torderlist, ItorderlistRepository>
    , IG04Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public G04Service(ItorderlistRepository torderlistRepository

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
                string sql = "SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.PART_SCN = '6' GROUP BY tspart.`PART_CAR`";


                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);


                result.DGV_G04_CB1 = SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]);
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
                    string selectedTab = BaseParameter.TabSelected ?? "TabPage1";

                    if (selectedTab == "TabPage1")
                    {
                        string PART_AA = BaseParameter.TextBox1 ?? string.Empty;
                        string LINE_AA = BaseParameter.ComboBox1 ?? "Select Line";
                        DateTime N_DT_BACK = DateTime.Parse(BaseParameter.DateTimePicker1 ?? DateTime.Now.ToString("yyyy-MM-dd"));

                        string N_DT = N_DT_BACK.ToString("yyyy-MM") + "-" +
                                     N_DT_BACK.AddMonths(1).AddDays(-N_DT_BACK.Day).ToString("dd");

                        string AUTO_DT;
                        if (DateTime.Now.Month > N_DT_BACK.Month)
                        {
                            AUTO_DT = N_DT;
                        }
                        else
                        {
                            AUTO_DT = DateTime.Now.ToString("yyyy-MM-dd");
                        }

                        DateTime CHK_WK = new DateTime(N_DT_BACK.Year, N_DT_BACK.Month, 1);
                        int DB_WEEK = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                            CHK_WK, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

                        int[] WEEK_NM = new int[7];
                        int II = 0;
                        for (int W_II = DB_WEEK; W_II <= DB_WEEK + 5; W_II++)
                        {
                            WEEK_NM[II] = W_II;
                            II++;
                        }

                        int WW1 = WEEK_NM[0];
                        int WW2 = WEEK_NM[1];
                        int WW3 = WEEK_NM[2];
                        int WW4 = WEEK_NM[3];
                        int WW5 = WEEK_NM[4];
                        int WW6 = WEEK_NM[5];

                        string LINE_SQL = "";
                        if (LINE_AA != "Select Line")
                        {
                            LINE_SQL = " AND (`MAIN`.`PART_CAR` = '" + LINE_AA + "' OR `MAIN`.`PART_CAR` = 'STAGE')";
                        }

                        string DGV_DATA0 = @"SELECT * FROM (

SELECT  
CONVERT(CONCAT('A', ROW_NUMBER() OVER (ORDER BY tspart.`PART_CAR`)), CHAR)  AS `ROW_NO`,  
0 AS `PART_IDX`, tspart.`PART_CAR` AS `PART_CAR`, '' AS  `PART_NO`, '' AS `PART_NM`, '' AS `PART_FML`, tspart.`PART_CAR` AS `PART_SNP1`,
IFNULL(`M_2`.`01`, 0) AS `D01`,
IFNULL(`M_2`.`02`, 0) AS `D02`,
IFNULL(`M_2`.`03`, 0) AS `D03`,
IFNULL(`M_2`.`04`, 0) AS `D04`,
IFNULL(`M_2`.`05`, 0) AS `D05`,
IFNULL(`M_2`.`06`, 0) AS `D06`,
IFNULL(`M_2`.`07`, 0) AS `D07`,
IFNULL(`M_2`.`08`, 0) AS `D08`,
IFNULL(`M_2`.`09`, 0) AS `D09`,
IFNULL(`M_2`.`10`, 0) AS `D10`,
IFNULL(`M_2`.`11`, 0) AS `D11`,
IFNULL(`M_2`.`12`, 0) AS `D12`,
IFNULL(`M_2`.`13`, 0) AS `D13`,
IFNULL(`M_2`.`14`, 0) AS `D14`,
IFNULL(`M_2`.`15`, 0) AS `D15`,
IFNULL(`M_2`.`16`, 0) AS `D16`,
IFNULL(`M_2`.`17`, 0) AS `D17`,
IFNULL(`M_2`.`18`, 0) AS `D18`,
IFNULL(`M_2`.`19`, 0) AS `D19`,
IFNULL(`M_2`.`20`, 0) AS `D20`,
IFNULL(`M_2`.`21`, 0) AS `D21`,
IFNULL(`M_2`.`22`, 0) AS `D22`,
IFNULL(`M_2`.`23`, 0) AS `D23`,
IFNULL(`M_2`.`24`, 0) AS `D24`,
IFNULL(`M_2`.`25`, 0) AS `D25`,
IFNULL(`M_2`.`26`, 0) AS `D26`,
IFNULL(`M_2`.`27`, 0) AS `D27`,
IFNULL(`M_2`.`28`, 0) AS `D28`,
IFNULL(`M_2`.`29`, 0) AS `D29`,
IFNULL(`M_2`.`30`, 0) AS `D30`,
IFNULL(`M_2`.`31`, 0) AS `D31`,
IFNULL(`M_2`.`SUM`, 0) AS `P_SUM`,

''  AS `DATE_STOCK`,

''  AS `NOW_QTY`,
IFNULL(`M_4`.`SHIP_SUM`, 0) AS `SHIP_SUM`,
IFNULL(`M_4`.`W1`, 0) AS `W1`,
IFNULL(`M_4`.`W2`, 0) AS `W2`,
IFNULL(`M_4`.`W3`, 0) AS `W3`,
IFNULL(`M_4`.`W4`, 0) AS `W4`,
IFNULL(`M_4`.`W5`, 0) AS `W5`,
IFNULL(`M_4`.`W6`, 0) AS `W6`
FROM
tspart LEFT JOIN
/* 생산 실적 관리 시트 */
(
SELECT  
(SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = `A`.`VLID_PART_IDX`) AS `PART_GP`,
SUM(`A`.`01`) AS `01`,
SUM(`A`.`02`) AS `02`,
SUM(`A`.`03`) AS `03`,
SUM(`A`.`04`) AS `04`,
SUM(`A`.`05`) AS `05`,
SUM(`A`.`06`) AS `06`,
SUM(`A`.`07`) AS `07`,
SUM(`A`.`08`) AS `08`,
SUM(`A`.`09`) AS `09`,
SUM(`A`.`10`) AS `10`,
SUM(`A`.`11`) AS `11`,
SUM(`A`.`12`) AS `12`,
SUM(`A`.`13`) AS `13`,
SUM(`A`.`14`) AS `14`,
SUM(`A`.`15`) AS `15`,
SUM(`A`.`16`) AS `16`,
SUM(`A`.`17`) AS `17`,
SUM(`A`.`18`) AS `18`,
SUM(`A`.`19`) AS `19`,
SUM(`A`.`20`) AS `20`,
SUM(`A`.`21`) AS `21`,
SUM(`A`.`22`) AS `22`,
SUM(`A`.`23`) AS `23`,
SUM(`A`.`24`) AS `24`,
SUM(`A`.`25`) AS `25`,
SUM(`A`.`26`) AS `26`,
SUM(`A`.`27`) AS `27`,
SUM(`A`.`28`) AS `28`,
SUM(`A`.`29`) AS `29`,
SUM(`A`.`30`) AS `30`,
SUM(`A`.`31`) AS `31`,
IFNULL(SUM(`A`.`QTY`), 0) AS `SUM`
 
FROM(
SELECT  tdpdmtim.VLID_PART_IDX,
(SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tdpdmtim.VLID_PART_IDX) AS `PART_NO`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '01' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `01`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '02' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `02`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '03' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `03`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '04' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `04`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '05' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `05`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '06' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `06`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '07' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `07`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '08' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `08`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '09' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `09`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '10' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `10`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '11' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `11`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '12' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `12`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '13' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `13`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '14' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `14`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '15' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `15`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '16' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `16`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '17' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `17`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '18' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `18`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '19' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `19`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '20' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `20`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '21' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `21`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '22' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `22`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '23' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `23`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '24' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `24`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '25' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `25`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '26' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `26`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '27' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `27`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '28' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `28`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '29' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `29`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '30' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `30`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '31' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `31`,

COUNT(tdpdmtim.VLID_BARCODE) AS `QTY`
FROM tdpdmtim 
WHERE tdpdmtim.VLID_DTM >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY) AND  tdpdmtim.VLID_DTM <= LAST_DAY('" + N_DT + @"') 
GROUP BY tdpdmtim.VLID_PART_IDX,  tdpdmtim.VLID_DTM) `A`
GROUP BY `PART_GP`
ORDER BY `PART_GP`, `PART_NO`) `M_2` 

ON tspart.`PART_CAR` = `M_2`.`PART_GP`

LEFT JOIN 
/* 출하 실적 관리 시트 */
(
SELECT 
`A_M`.`PART_GP`,
IFNULL(SUM(`A_M`.`QTY`), 0) AS `SHIP_SUM`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW1 + @" THEN (`A_M`.`QTY`) END), 0) AS `W1`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW2 + @" THEN (`A_M`.`QTY`) END), 0) AS `W2`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW3 + @" THEN (`A_M`.`QTY`) END), 0) AS `W3`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW4 + @" THEN (`A_M`.`QTY`) END), 0) AS `W4`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW5 + @" THEN (`A_M`.`QTY`) END), 0) AS `W5`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW6 + @" THEN (`A_M`.`QTY`) END), 0) AS `W6`
FROM (
SELECT  
`M_1`.`PDOTPL_IDX`, 
`M_1`.`PO_CODE`,
WEEK(DATE_FORMAT(`M_1`.`DATE`, '%Y-%m-%d'), 7) +1  AS `WEEK`,
DATE_FORMAT(`M_1`.`DATE`, '%Y-%m-%d')  AS `DATE`,
(SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_GP`,
COUNT(tdpdmtim.`VLID_BARCODE`) AS `QTY`

FROM 
(SELECT `CODE_A`.`PO_CODE`, tdpdotpl.PDOTPL_IDX, IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) AS `DATE`
FROM (  SELECT  tdpdotpl.`PO_CODE` FROM tdpdotpl
WHERE IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
 AND  IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) <= LAST_DAY('" + N_DT + @"') 
GROUP BY tdpdotpl.PO_CODE) `CODE_A` JOIN tdpdotpl
ON `CODE_A`.`PO_CODE` = tdpdotpl.`PO_CODE`) `M_1`  JOIN tdpdmtim
ON `M_1`.PDOTPL_IDX = tdpdmtim.PDOTPL_IDX
WHERE `DATE` >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
 AND  `DATE` <= LAST_DAY('" + N_DT + @"') 
GROUP BY `PO_CODE`, `VLID_PART_IDX`

 /* SCAN 없는 출하 실적 관리 시트 */
UNION
SELECT 
0 AS `PDOTPL_IDX`,
tdpdotpl_ETC.ETC_PO_CODE,
WEEK(DATE_FORMAT(tdpdotpl_ETC.`DATE`, '%Y-%m-%d'), 7) +1  AS `WEEK`,
tdpdotpl_ETC.`DATE`,
(SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = tdpdotpl_ETC.`PART_IDX`) AS `PART_GP`,
tdpdotpl_ETC.`QTY`

 FROM tdpdotpl_ETC
WHERE tdpdotpl_ETC.`DATE` >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
 AND  tdpdotpl_ETC.`DATE` <= LAST_DAY('" + N_DT + @"') 

) `A_M`
GROUP BY `A_M`.`PART_GP`) `M_4`

ON tspart.`PART_CAR` = `M_4`.`PART_GP`

WHERE tspart.`PART_SCN` = '6' AND  tspart.`PART_USENY` = 'Y'
AND (IFNULL(`M_2`.`SUM`, 0) + IFNULL(`M_4`.`SHIP_SUM`, 0)) > 0
GROUP BY tspart.`PART_CAR`

UNION

SELECT 
'B1' AS `ROW_NO`, 
0 AS `PART_IDX`, 'STAGE' AS `PART_CAR`, 'PART_NO' AS  `PART_NO`, 'PART_NAME' AS `PART_NM`, 
'PART_FML' AS `PART_FML`, 'PART_SNP' AS `PART_SNP1`,
'01' AS `D01`,
'02' AS `D02`,
'03' AS `D03`,
'04' AS `D04`,
'05' AS `D05`,
'06' AS `D06`,
'07' AS `D07`,
'08' AS `D08`,
'09' AS `D09`,
'10' AS `D10`,
'11' AS `D11`,
'12' AS `D12`,
'13' AS `D13`,
'14' AS `D14`,
'15' AS `D15`,
'16' AS `D16`,
'17' AS `D17`,
'18' AS `D18`,
'19' AS `D19`,
'20' AS `D20`,
'21' AS `D21`,
'22' AS `D22`,
'23' AS `D23`,
'24' AS `D24`,
'25' AS `D25`,
'26' AS `D26`,
'27' AS `D27`,
'28' AS `D28`,
'29' AS `D29`,
'30' AS `D30`,
'31' AS `D31`,
'INPUT(NHẬP KHO)' AS `P_SUM`,
'" + AUTO_DT + @"'  AS `DATE_STOCK`,
'MES_STOCK'  AS `NOW_QTY`,
'Shipping' AS `SHIP_SUM`,
'" + WW1 + @"WEEK" + @"' AS `W1`,
'" + WW2 + @"WEEK" + @"' AS `W2`,
'" + WW3 + @"WEEK" + @"' AS `W3`,
'" + WW4 + @"WEEK" + @"' AS `W4`,
'" + WW5 + @"WEEK" + @"' AS `W5`,
'" + WW6 + @"WEEK" + @"' AS `W6`

UNION

(SELECT  
CONVERT(CONCAT('C', ROW_NUMBER() OVER (ORDER BY tspart.`PART_CAR`)), CHAR) AS `ROW_NO`,  
tspart.`PART_IDX`,
tspart.`PART_CAR`,
tspart.`PART_NO`,
tspart.`PART_NM`,
tspart.`PART_FML`,
tspart.`PART_SNP`,
IFNULL(`M_2`.`01`, 0) AS `D01`,
IFNULL(`M_2`.`02`, 0) AS `D02`,
IFNULL(`M_2`.`03`, 0) AS `D03`,
IFNULL(`M_2`.`04`, 0) AS `D04`,
IFNULL(`M_2`.`05`, 0) AS `D05`,
IFNULL(`M_2`.`06`, 0) AS `D06`,
IFNULL(`M_2`.`07`, 0) AS `D07`,
IFNULL(`M_2`.`08`, 0) AS `D08`,
IFNULL(`M_2`.`09`, 0) AS `D09`,
IFNULL(`M_2`.`10`, 0) AS `D10`,
IFNULL(`M_2`.`11`, 0) AS `D11`,
IFNULL(`M_2`.`12`, 0) AS `D12`,
IFNULL(`M_2`.`13`, 0) AS `D13`,
IFNULL(`M_2`.`14`, 0) AS `D14`,
IFNULL(`M_2`.`15`, 0) AS `D15`,
IFNULL(`M_2`.`16`, 0) AS `D16`,
IFNULL(`M_2`.`17`, 0) AS `D17`,
IFNULL(`M_2`.`18`, 0) AS `D18`,
IFNULL(`M_2`.`19`, 0) AS `D19`,
IFNULL(`M_2`.`20`, 0) AS `D20`,
IFNULL(`M_2`.`21`, 0) AS `D21`,
IFNULL(`M_2`.`22`, 0) AS `D22`,
IFNULL(`M_2`.`23`, 0) AS `D23`,
IFNULL(`M_2`.`24`, 0) AS `D24`,
IFNULL(`M_2`.`25`, 0) AS `D25`,
IFNULL(`M_2`.`26`, 0) AS `D26`,
IFNULL(`M_2`.`27`, 0) AS `D27`,
IFNULL(`M_2`.`28`, 0) AS `D28`,
IFNULL(`M_2`.`29`, 0) AS `D29`,
IFNULL(`M_2`.`30`, 0) AS `D30`,
IFNULL(`M_2`.`31`, 0) AS `D31`,
IFNULL(`M_2`.`SUM`, 0) AS `P_SUM`,

IFNULL(`OLD_ST`.`QTY`, 0) AS `DATE_STOCK`,

(SELECT IFNULL(tiivtr.`QTY` , 0) FROM tiivtr WHERE tiivtr.LOC_IDX = '2' AND tiivtr.`PART_IDX` = tspart.`PART_IDX`) AS `NOW_QTY`,
IFNULL(`M_4`.`SHIP_SUM`, 0) AS `SHIP_SUM`,
IFNULL(`M_4`.`W1`, 0) AS `W1`,
IFNULL(`M_4`.`W2`, 0) AS `W2`,
IFNULL(`M_4`.`W3`, 0) AS `W3`,
IFNULL(`M_4`.`W4`, 0) AS `W4`,
IFNULL(`M_4`.`W5`, 0) AS `W5`,
IFNULL(`M_4`.`W6`, 0) AS `W6`
FROM
tspart LEFT JOIN
/* 생산 실적 관리 시트 */
(
SELECT  
`A`.`VLID_PART_IDX`, 
`A`.`PART_NO`, (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = `A`.`VLID_PART_IDX`) AS `PART_NM`,
(SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = `A`.`VLID_PART_IDX`) AS `PART_GP`,
SUM(`A`.`01`) AS `01`,
SUM(`A`.`02`) AS `02`,
SUM(`A`.`03`) AS `03`,
SUM(`A`.`04`) AS `04`,
SUM(`A`.`05`) AS `05`,
SUM(`A`.`06`) AS `06`,
SUM(`A`.`07`) AS `07`,
SUM(`A`.`08`) AS `08`,
SUM(`A`.`09`) AS `09`,
SUM(`A`.`10`) AS `10`,
SUM(`A`.`11`) AS `11`,
SUM(`A`.`12`) AS `12`,
SUM(`A`.`13`) AS `13`,
SUM(`A`.`14`) AS `14`,
SUM(`A`.`15`) AS `15`,
SUM(`A`.`16`) AS `16`,
SUM(`A`.`17`) AS `17`,
SUM(`A`.`18`) AS `18`,
SUM(`A`.`19`) AS `19`,
SUM(`A`.`20`) AS `20`,
SUM(`A`.`21`) AS `21`,
SUM(`A`.`22`) AS `22`,
SUM(`A`.`23`) AS `23`,
SUM(`A`.`24`) AS `24`,
SUM(`A`.`25`) AS `25`,
SUM(`A`.`26`) AS `26`,
SUM(`A`.`27`) AS `27`,
SUM(`A`.`28`) AS `28`,
SUM(`A`.`29`) AS `29`,
SUM(`A`.`30`) AS `30`,
SUM(`A`.`31`) AS `31`,
IFNULL(SUM(`A`.`QTY`), 0) AS `SUM`
 
FROM(
SELECT  tdpdmtim.VLID_PART_IDX,
(SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tdpdmtim.VLID_PART_IDX) AS `PART_NO`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '01' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `01`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '02' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `02`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '03' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `03`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '04' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `04`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '05' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `05`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '06' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `06`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '07' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `07`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '08' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `08`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '09' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `09`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '10' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `10`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '11' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `11`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '12' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `12`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '13' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `13`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '14' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `14`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '15' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `15`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '16' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `16`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '17' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `17`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '18' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `18`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '19' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `19`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '20' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `20`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '21' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `21`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '22' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `22`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '23' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `23`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '24' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `24`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '25' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `25`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '26' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `26`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '27' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `27`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '28' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `28`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '29' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `29`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '30' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `30`,
IFNULL((CASE DATE_FORMAT(tdpdmtim.VLID_DTM, '%d') WHEN '31' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `31`,

COUNT(tdpdmtim.VLID_BARCODE) AS `QTY`
FROM tdpdmtim 
WHERE tdpdmtim.VLID_DTM >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY) AND  tdpdmtim.VLID_DTM <= LAST_DAY('" + N_DT + @"') 
GROUP BY tdpdmtim.VLID_PART_IDX,  tdpdmtim.VLID_DTM) `A`
GROUP BY `PART_NO`
ORDER BY `PART_GP`, `PART_NO`) `M_2` 

ON tspart.`PART_IDX` = `M_2`.`VLID_PART_IDX`

LEFT JOIN 
/* 출하 실적 관리 시트 */
(
SELECT 
`A_M`.`VLID_PART_IDX`, `A_M`.`PART_NO`,
IFNULL(SUM(`A_M`.`QTY`), 0) AS `SHIP_SUM`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW1 + @" THEN (`A_M`.`QTY`) END), 0) AS `W1`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW2 + @" THEN (`A_M`.`QTY`) END), 0) AS `W2`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW3 + @" THEN (`A_M`.`QTY`) END), 0) AS `W3`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW4 + @" THEN (`A_M`.`QTY`) END), 0) AS `W4`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW5 + @" THEN (`A_M`.`QTY`) END), 0) AS `W5`,
IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW6 + @" THEN (`A_M`.`QTY`) END), 0) AS `W6`
FROM (
SELECT  
`M_1`.`PDOTPL_IDX`, 
`M_1`.`PO_CODE`,
WEEK(DATE_FORMAT(`M_1`.`DATE`, '%Y-%m-%d'), 7) +1  AS `WEEK`,
DATE_FORMAT(`M_1`.`DATE`, '%Y-%m-%d')  AS `DATE`,
tdpdmtim.`VLID_PART_IDX`,
(SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_NO`,
COUNT(tdpdmtim.`VLID_BARCODE`) AS `QTY`

FROM 
(SELECT `CODE_A`.`PO_CODE`, tdpdotpl.PDOTPL_IDX, IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) AS `DATE`
FROM (  SELECT  tdpdotpl.`PO_CODE` FROM tdpdotpl
WHERE IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
 AND  IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) <= LAST_DAY('" + N_DT + @"') 
GROUP BY tdpdotpl.PO_CODE) `CODE_A` JOIN tdpdotpl
ON `CODE_A`.`PO_CODE` = tdpdotpl.`PO_CODE`) `M_1`  JOIN tdpdmtim
ON `M_1`.PDOTPL_IDX = tdpdmtim.PDOTPL_IDX
WHERE `DATE` >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
 AND  `DATE` <= LAST_DAY('" + N_DT + @"') 
GROUP BY `PO_CODE`, `VLID_PART_IDX`

 /* SCAN 없는 출하 실적 관리 시트 */
UNION
SELECT 
0 AS `PDOTPL_IDX`,
tdpdotpl_ETC.ETC_PO_CODE,
WEEK(DATE_FORMAT(tdpdotpl_ETC.`DATE`, '%Y-%m-%d'), 7) +1  AS `WEEK`,
tdpdotpl_ETC.`DATE`,
tdpdotpl_ETC.`PART_IDX`,
(SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdotpl_ETC.`PART_IDX`) AS `PART_NO`,
tdpdotpl_ETC.`QTY`

 FROM tdpdotpl_ETC
WHERE tdpdotpl_ETC.`DATE` >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
 AND  tdpdotpl_ETC.`DATE` <= LAST_DAY('" + N_DT + @"') 

) `A_M`
GROUP BY `A_M`.`VLID_PART_IDX`) `M_4`

ON tspart.`PART_IDX` = `M_4`.`VLID_PART_IDX`

LEFT JOIN

(SELECT tiivtr_HISTORY.`PART_IDX`, tiivtr_HISTORY.`QTY` 
FROM tiivtr_HISTORY
WHERE tiivtr_HISTORY.LOC_IDX ='2' AND 
tiivtr_HISTORY.STOCK_DATE = IF(NOW() <= LAST_DAY('" + N_DT + @"'), DATE_FORMAT(NOW(), '%Y-%m-%d'), LAST_DAY('" + N_DT + @"')) ) `OLD_ST`

ON `OLD_ST`.`PART_IDX` =  tspart.`PART_IDX`

WHERE tspart.`PART_SCN` = '6' AND  tspart.`PART_USENY` = 'Y'
AND (IFNULL(`M_2`.`SUM`, 0) + IFNULL(`M_4`.`SHIP_SUM`, 0)) > 0
ORDER BY  `PART_NO`, `PART_CAR`)  

) `MAIN`    

WHERE (`MAIN`.`PART_NO` LIKE '%" + PART_AA + @"%' OR `MAIN`.`PART_NO` = 'PART_NO' OR `MAIN`.`PART_NO` = '')" + LINE_SQL;

                        DataSet dsDGV_00 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA0);
                        result.DataGridView1 = SQLHelper.ToList<SuperResultTranfer>(dsDGV_00.Tables[0]);
                        result.AUTO_DT = AUTO_DT;
                        result.WW1 = WW1;
                        result.WW2 = WW2;
                        result.WW3 = WW3;
                        result.WW4 = WW4;
                        result.WW5 = WW5;
                        result.WW6 = WW6;
                    }
                    else if (selectedTab == "TabPage2")
                    {
                        string PART_AA = BaseParameter.TextBox2 ?? string.Empty;
                        string LINE_AA = BaseParameter.ComboBox2 ?? "Select Line";
                        DateTime N_DT_BACK = DateTime.Parse(BaseParameter.DateTimePicker2 ?? DateTime.Now.ToString("yyyy-MM-dd"));

                        string N_DT = N_DT_BACK.ToString("yyyy-MM") + "-" +
                                     N_DT_BACK.AddMonths(1).AddDays(-N_DT_BACK.Day).ToString("dd");

                        string AUTO_DT;
                        if (DateTime.Now.Month > N_DT_BACK.Month)
                        {
                            AUTO_DT = N_DT;
                        }
                        else
                        {
                            AUTO_DT = DateTime.Now.ToString("yyyy-MM-dd");
                        }

                        DateTime CHK_WK = new DateTime(N_DT_BACK.Year, N_DT_BACK.Month, 1);
                        int DB_WEEK = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                            CHK_WK, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);

                        int[] WEEK_NM = new int[7];
                        int II = 0;
                        for (int W_II = DB_WEEK; W_II <= DB_WEEK + 5; W_II++)
                        {
                            WEEK_NM[II] = W_II;
                            II++;
                        }

                        int WW1 = WEEK_NM[0];
                        int WW2 = WEEK_NM[1];
                        int WW3 = WEEK_NM[2];
                        int WW4 = WEEK_NM[3];
                        int WW5 = WEEK_NM[4];
                        int WW6 = WEEK_NM[5];

                        string LINE_SQL = "";
                        if (LINE_AA != "Select Line")
                        {
                            LINE_SQL = " AND (`MAIN`.`PART_CAR` = '" + LINE_AA + "' OR `MAIN`.`PART_CAR` = 'STAGE')";
                        }

                        string DGV_DATA0 = @"SELECT * FROM (
            SELECT  
            CONVERT(CONCAT('A', ROW_NUMBER() OVER (ORDER BY tspart.`PART_CAR`)), CHAR) AS `ROW_NO`,  
            0 AS `PART_IDX`, tspart.`PART_CAR` AS `PART_CAR`, '' AS `PART_NO`, '' AS `PART_NM`, '' AS `PART_FML`, tspart.`PART_CAR` AS `PART_SNP1`,
            IFNULL(`M_2`.`W1`, 0) AS `PW1`,
            IFNULL(`M_2`.`W2`, 0) AS `PW2`,
            IFNULL(`M_2`.`W3`, 0) AS `PW3`,
            IFNULL(`M_2`.`W4`, 0) AS `PW4`,
            IFNULL(`M_2`.`W5`, 0) AS `PW5`,
            IFNULL(`M_2`.`W6`, 0) AS `PW6`,
            IFNULL(`M_2`.`SUM`, 0) AS `P_SUM`,
            '' AS `DATE_STOCK`,
            '' AS `NOW_QTY`,
            IFNULL(`M_4`.`SHIP_SUM`, 0) AS `SHIP_SUM`,
            IFNULL(`M_4`.`W1`, 0) AS `W1`,
            IFNULL(`M_4`.`W2`, 0) AS `W2`,
            IFNULL(`M_4`.`W3`, 0) AS `W3`,
            IFNULL(`M_4`.`W4`, 0) AS `W4`,
            IFNULL(`M_4`.`W5`, 0) AS `W5`,
            IFNULL(`M_4`.`W6`, 0) AS `W6`
            FROM
            tspart LEFT JOIN
            (
            SELECT  
            (SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = `A`.`VLID_PART_IDX`) AS `PART_GP`,
            SUM(`A`.`W1`) AS `W1`,
            SUM(`A`.`W2`) AS `W2`,
            SUM(`A`.`W3`) AS `W3`,
            SUM(`A`.`W4`) AS `W4`,
            SUM(`A`.`W5`) AS `W5`,
            SUM(`A`.`W6`) AS `W6`,
            IFNULL(SUM(`A`.`QTY`), 0) AS `SUM`
            
            FROM(
            SELECT tdpdmtim.VLID_PART_IDX,
            (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tdpdmtim.VLID_PART_IDX) AS `PART_NO`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW1 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W1`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW2 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W2`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW3 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W3`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW4 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W4`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW5 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W5`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW6 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W6`,
            COUNT(tdpdmtim.VLID_BARCODE) AS `QTY`
            FROM tdpdmtim 
            WHERE tdpdmtim.VLID_DTM >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY) AND tdpdmtim.VLID_DTM <= LAST_DAY('" + N_DT + @"') 
            GROUP BY tdpdmtim.VLID_PART_IDX, tdpdmtim.VLID_DTM) `A`
            GROUP BY `PART_GP`
            ORDER BY `PART_GP`, `PART_NO`) `M_2` 

            ON tspart.`PART_CAR` = `M_2`.`PART_GP`

            LEFT JOIN 
            (
            SELECT 
            `A_M`.`PART_GP`,
            IFNULL(SUM(`A_M`.`QTY`), 0) AS `SHIP_SUM`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW1 + @" THEN (`A_M`.`QTY`) END), 0) AS `W1`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW2 + @" THEN (`A_M`.`QTY`) END), 0) AS `W2`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW3 + @" THEN (`A_M`.`QTY`) END), 0) AS `W3`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW4 + @" THEN (`A_M`.`QTY`) END), 0) AS `W4`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW5 + @" THEN (`A_M`.`QTY`) END), 0) AS `W5`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW6 + @" THEN (`A_M`.`QTY`) END), 0) AS `W6`
            FROM (
            SELECT  
            `M_1`.`PDOTPL_IDX`, 
            `M_1`.`PO_CODE`,
            WEEK(DATE_FORMAT(`M_1`.`DATE`, '%Y-%m-%d'), 7) +1 AS `WEEK`,
            DATE_FORMAT(`M_1`.`DATE`, '%Y-%m-%d') AS `DATE`,
            (SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_GP`,
            COUNT(tdpdmtim.`VLID_BARCODE`) AS `QTY`

            FROM 
            (SELECT `CODE_A`.`PO_CODE`, tdpdotpl.PDOTPL_IDX, IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) AS `DATE`
            FROM (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl
            WHERE IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
            AND IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) <= LAST_DAY('" + N_DT + @"') 
            GROUP BY tdpdotpl.PO_CODE) `CODE_A` JOIN tdpdotpl
            ON `CODE_A`.`PO_CODE` = tdpdotpl.`PO_CODE`) `M_1` JOIN tdpdmtim
            ON `M_1`.PDOTPL_IDX = tdpdmtim.PDOTPL_IDX
            WHERE `DATE` >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
            AND `DATE` <= LAST_DAY('" + N_DT + @"') 
            GROUP BY `PO_CODE`, `VLID_PART_IDX`

            UNION
            SELECT 
            0 AS `PDOTPL_IDX`,
            tdpdotpl_ETC.ETC_PO_CODE,
            WEEK(DATE_FORMAT(tdpdotpl_ETC.`DATE`, '%Y-%m-%d'), 7) +1 AS `WEEK`,
            tdpdotpl_ETC.`DATE`,
            (SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = tdpdotpl_ETC.`PART_IDX`) AS `PART_GP`,
            tdpdotpl_ETC.`QTY`

            FROM tdpdotpl_ETC
            WHERE tdpdotpl_ETC.`DATE` >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
            AND tdpdotpl_ETC.`DATE` <= LAST_DAY('" + N_DT + @"') 

            ) `A_M`
            GROUP BY `A_M`.`PART_GP`) `M_4`

            ON tspart.`PART_CAR` = `M_4`.`PART_GP`

            WHERE tspart.`PART_SCN` = '6' AND tspart.`PART_USENY` = 'Y'
            AND (IFNULL(`M_2`.`SUM`, 0) + IFNULL(`M_4`.`SHIP_SUM`, 0)) > 0
            GROUP BY tspart.`PART_CAR`

            UNION

            SELECT 
            'B1' AS `ROW_NO`, 
            0 AS `PART_IDX`, 'STAGE' AS `PART_CAR`, 'PART_NO' AS `PART_NO`, 'PART_NAME' AS `PART_NM`, 
            'PART_FML' AS `PART_FML`, 'PART_SNP' AS `PART_SNP1`,
            '" + WW1 + @"WEEK' AS `PW1`,
            '" + WW2 + @"WEEK' AS `PW2`,
            '" + WW3 + @"WEEK' AS `PW3`,
            '" + WW4 + @"WEEK' AS `PW4`,
            '" + WW5 + @"WEEK' AS `PW5`,
            '" + WW6 + @"WEEK' AS `PW6`,
            'INPUT(NHẬP KHO)' AS `P_SUM`,
            '" + AUTO_DT + @"' AS `DATE_STOCK`,
            'MES_STOCK' AS `NOW_QTY`,
            'Shipping' AS `SHIP_SUM`,
            '" + WW1 + @"WEEK' AS `W1`,
            '" + WW2 + @"WEEK' AS `W2`,
            '" + WW3 + @"WEEK' AS `W3`,
            '" + WW4 + @"WEEK' AS `W4`,
            '" + WW5 + @"WEEK' AS `W5`,
            '" + WW6 + @"WEEK' AS `W6`

            UNION

            (SELECT  
            CONVERT(CONCAT('C', ROW_NUMBER() OVER (ORDER BY tspart.`PART_CAR`)), CHAR) AS `ROW_NO`,  
            tspart.`PART_IDX`,
            tspart.`PART_CAR`,
            tspart.`PART_NO`,
            tspart.`PART_NM`,
            tspart.`PART_FML`,
            tspart.`PART_SNP`,
            IFNULL(`M_2`.`W1`, 0) AS `PW1`,
            IFNULL(`M_2`.`W2`, 0) AS `PW2`,
            IFNULL(`M_2`.`W3`, 0) AS `PW3`,
            IFNULL(`M_2`.`W4`, 0) AS `PW4`,
            IFNULL(`M_2`.`W5`, 0) AS `PW5`,
            IFNULL(`M_2`.`W6`, 0) AS `PW6`,
            IFNULL(`M_2`.`SUM`, 0) AS `P_SUM`,

            IFNULL(`OLD_ST`.`QTY`, 0) AS `DATE_STOCK`,

            (SELECT IFNULL(tiivtr.`QTY` , 0) FROM tiivtr WHERE tiivtr.LOC_IDX = '2' AND tiivtr.`PART_IDX` = tspart.`PART_IDX`) AS `NOW_QTY`,
            IFNULL(`M_4`.`SHIP_SUM`, 0) AS `SHIP_SUM`,
            IFNULL(`M_4`.`W1`, 0) AS `W1`,
            IFNULL(`M_4`.`W2`, 0) AS `W2`,
            IFNULL(`M_4`.`W3`, 0) AS `W3`,
            IFNULL(`M_4`.`W4`, 0) AS `W4`,
            IFNULL(`M_4`.`W5`, 0) AS `W5`,
            IFNULL(`M_4`.`W6`, 0) AS `W6`
            FROM
            tspart LEFT JOIN
            (
            SELECT  
            `A`.`VLID_PART_IDX`, 
            `A`.`PART_NO`, (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = `A`.`VLID_PART_IDX`) AS `PART_NM`,
            (SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = `A`.`VLID_PART_IDX`) AS `PART_GP`,
            SUM(`A`.`W1`) AS `W1`,
            SUM(`A`.`W2`) AS `W2`,
            SUM(`A`.`W3`) AS `W3`,
            SUM(`A`.`W4`) AS `W4`,
            SUM(`A`.`W5`) AS `W5`,
            SUM(`A`.`W6`) AS `W6`,
            IFNULL(SUM(`A`.`QTY`), 0) AS `SUM`
            
            FROM(
            SELECT tdpdmtim.VLID_PART_IDX,
            (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tdpdmtim.VLID_PART_IDX) AS `PART_NO`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW1 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W1`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW2 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W2`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW3 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W3`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW4 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W4`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW5 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W5`,
            IFNULL((CASE WEEK(DATE_FORMAT(tdpdmtim.VLID_DTM, '%Y-%m-%d'), 7) + 1 WHEN '" + WW6 + @"' THEN COUNT(tdpdmtim.VLID_BARCODE) END), 0) AS `W6`,
            COUNT(tdpdmtim.VLID_BARCODE) AS `QTY`
            FROM tdpdmtim 
            WHERE tdpdmtim.VLID_DTM >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY) AND tdpdmtim.VLID_DTM <= LAST_DAY('" + N_DT + @"') 
            GROUP BY tdpdmtim.VLID_PART_IDX, tdpdmtim.VLID_DTM) `A`
            GROUP BY `PART_NO`
            ORDER BY `PART_GP`, `PART_NO`) `M_2` 

            ON tspart.`PART_IDX` = `M_2`.`VLID_PART_IDX`

            LEFT JOIN 
            (
            SELECT 
            `A_M`.`VLID_PART_IDX`, `A_M`.`PART_NO`,
            IFNULL(SUM(`A_M`.`QTY`), 0) AS `SHIP_SUM`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW1 + @" THEN (`A_M`.`QTY`) END), 0) AS `W1`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW2 + @" THEN (`A_M`.`QTY`) END), 0) AS `W2`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW3 + @" THEN (`A_M`.`QTY`) END), 0) AS `W3`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW4 + @" THEN (`A_M`.`QTY`) END), 0) AS `W4`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW5 + @" THEN (`A_M`.`QTY`) END), 0) AS `W5`,
            IFNULL(SUM(CASE `A_M`.`WEEK` WHEN " + WW6 + @" THEN (`A_M`.`QTY`) END), 0) AS `W6`
            FROM (
            SELECT  
            `M_1`.`PDOTPL_IDX`, 
            `M_1`.`PO_CODE`,
            WEEK(DATE_FORMAT(`M_1`.`DATE`, '%Y-%m-%d'), 7) +1 AS `WEEK`,
            DATE_FORMAT(`M_1`.`DATE`, '%Y-%m-%d') AS `DATE`,
            tdpdmtim.`VLID_PART_IDX`,
            (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdmtim.`VLID_PART_IDX`) AS `PART_NO`,
            COUNT(tdpdmtim.`VLID_BARCODE`) AS `QTY`

            FROM 
            (SELECT `CODE_A`.`PO_CODE`, tdpdotpl.PDOTPL_IDX, IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) AS `DATE`
            FROM (SELECT tdpdotpl.`PO_CODE` FROM tdpdotpl
            WHERE IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
            AND IFNULL(tdpdotpl.UPDATE_DTM, tdpdotpl.CREATE_DTM) <= LAST_DAY('" + N_DT + @"') 
            GROUP BY tdpdotpl.PO_CODE) `CODE_A` JOIN tdpdotpl
            ON `CODE_A`.`PO_CODE` = tdpdotpl.`PO_CODE`) `M_1` JOIN tdpdmtim
            ON `M_1`.PDOTPL_IDX = tdpdmtim.PDOTPL_IDX
            WHERE `DATE` >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
            AND `DATE` <= LAST_DAY('" + N_DT + @"') 
            GROUP BY `PO_CODE`, `VLID_PART_IDX`

            UNION
            SELECT 
            0 AS `PDOTPL_IDX`,
            tdpdotpl_ETC.ETC_PO_CODE,
            WEEK(DATE_FORMAT(tdpdotpl_ETC.`DATE`, '%Y-%m-%d'), 7) +1 AS `WEEK`,
            tdpdotpl_ETC.`DATE`,
            tdpdotpl_ETC.`PART_IDX`,
            (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tdpdotpl_ETC.`PART_IDX`) AS `PART_NO`,
            tdpdotpl_ETC.`QTY`

            FROM tdpdotpl_ETC
            WHERE tdpdotpl_ETC.`DATE` >= (LAST_DAY('" + N_DT + @"' - INTERVAL 1 MONTH) + INTERVAL 1 DAY)
            AND tdpdotpl_ETC.`DATE` <= LAST_DAY('" + N_DT + @"') 

            ) `A_M`
            GROUP BY `A_M`.`VLID_PART_IDX`) `M_4`

            ON tspart.`PART_IDX` = `M_4`.`VLID_PART_IDX`

            LEFT JOIN

            (SELECT tiivtr_HISTORY.`PART_IDX`, tiivtr_HISTORY.`QTY` 
            FROM tiivtr_HISTORY
            WHERE tiivtr_HISTORY.LOC_IDX ='2' AND 
            tiivtr_HISTORY.STOCK_DATE = IF(NOW() <= LAST_DAY('" + N_DT + @"'), DATE_FORMAT(NOW(), '%Y-%m-%d'), LAST_DAY('" + N_DT + @"')) ) `OLD_ST`

            ON `OLD_ST`.`PART_IDX` = tspart.`PART_IDX`

            WHERE tspart.`PART_SCN` = '6' AND tspart.`PART_USENY` = 'Y'
            AND (IFNULL(`M_2`.`SUM`, 0) + IFNULL(`M_4`.`SHIP_SUM`, 0)) > 0
            ORDER BY `PART_NO`, `PART_CAR`)  

            ) `MAIN`    

            WHERE (`MAIN`.`PART_NO` LIKE '%" + PART_AA + @"%' OR `MAIN`.`PART_NO` = 'PART_NO' OR `MAIN`.`PART_NO` = '')" + LINE_SQL;

                        DataSet dsDGV_00 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA0);
                        result.DataGridView2 = SQLHelper.ToList<SuperResultTranfer>(dsDGV_00.Tables[0]);
                        result.AUTO_DT = AUTO_DT;
                        result.WW1 = WW1;
                        result.WW2 = WW2;
                        result.WW3 = WW3;
                        result.WW4 = WW4;
                        result.WW5 = WW5;
                        result.WW6 = WW6;
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

    }
}

