using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MESService.Implement
{
    public class H06Service : BaseService<torderlist, ItorderlistRepository>, IH06Service
    {
        private readonly ItorderlistRepository _torderlistRepository;

        public H06Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
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
                result = await Dgv_reload();
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
                result = await Dgv_reload(
                    BaseParameter?.START_DATE,
                    BaseParameter?.END_DATE
                );
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Dgv_reload(string startDate = null, string endDate = null)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Set default dates nếu không truyền vào
                if (string.IsNullOrEmpty(startDate))
                {
                    startDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                if (string.IsNullOrEmpty(endDate))
                {
                    endDate = startDate;
                }

                // Tính toán thời gian ca làm việc (6h sáng)
                string startDateTime = $"{startDate} 06:00:00";
                string endDateTime = DateTime.Parse(endDate).AddDays(1).ToString("yyyy-MM-dd") + " 06:00:00";

                // Xác định TS_TIME_END dựa trên ngày được chọn
                DateTime selectedEndDate = DateTime.Parse(endDate);
                DateTime today = DateTime.Now.Date;

                string tsTimeEndFormula;
                if (selectedEndDate >= today)
                {
                    // Ngày hiện tại hoặc tương lai: dùng NOW()
                    tsTimeEndFormula = "DATE_FORMAT(NOW(), '%Y-%m-%d %T')";
                }
                else
                {
                    // Ngày quá khứ: dùng 6h sáng ngày endDate + 1
                    tsTimeEndFormula = $"'{endDateTime}'";
                }

                // Câu truy vấn SQL với date range
                string SQL_TIME = $@"SELECT 
            DATE_FORMAT(NOW(), '%Y-%m-%d') AS `DATE`,
            IFNULL(`AA`.`MC_NO`, '') AS `MC_NO`, 
            `BB`.`TS_TIME_ST`,
            `BB`.`TS_TIME_END`,
            SUM(`AA`.`SUM`) AS `SUM`,
            IFNULL(ROUND(AVG(`AA`.`STOP_TIME`/3600), 3), 0) AS `STOP_TIME`,
            IFNULL(ROUND(AVG(`AA`.`T_STOP`/3600), 3), 0) AS `T_STOP`,
            ROUND(AVG(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600), 3) AS `WORK_TIME`,
            ROUND(AVG(((ROUND(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600, 3) 
                - (IFNULL(ROUND(`AA`.`STOP_TIME`/3600, 3), 0) - IFNULL(ROUND(`AA`.`T_STOP`/3600, 3), 0))) 
                / ROUND(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600, 3))), 2) AS `RUN_RAT`,
            ROUND(AVG(`AA`.`SUM` / ((TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) - IFNULL(`AA`.`T_STOP`, 0)) / 3600)), 0) AS `UPH`

        FROM
        (SELECT  *  FROM
        (SELECT `MC_NO`, SUM(`WK_QTY`) AS `SUM` 
        FROM TWWKAR 
        WHERE 
        `CREATE_DTM` >= '{startDateTime}' AND 
        `CREATE_DTM` < '{endDateTime}'
        GROUP BY  `MC_NO`) `A`
        LEFT JOIN       
        (SELECT `TSNON_OPER_MCNM`, 
        SUM(TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) AS `STOP_TIME`,
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'S' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'Q' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'L' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'N' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'T' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'M' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) AS `T_STOP`
        FROM TSNON_OPER 
        WHERE 
        `CREATE_DTM` >= '{startDateTime}' AND 
        `CREATE_DTM` < '{endDateTime}'
        GROUP BY `TSNON_OPER_MCNM` ) `B`
        ON `A`.`MC_NO` = `B`.`TSNON_OPER_MCNM`  
        WHERE `A`.`MC_NO` LIKE 'ZA8%') `AA`

        LEFT JOIN
        (SELECT `TS_MC_NM`, DATE_FORMAT(MIN(`TS_TIME_ST`), '%Y-%m-%d %T') AS `TS_TIME_ST`, 
        {tsTimeEndFormula} AS `TS_TIME_END`,
        `TS_DATE` FROM TUSER_LOG  
        WHERE  `TS_DATE` >= '{startDate}' AND `TS_DATE` <= '{endDate}'
        AND `TS_MC_NM` LIKE 'ZA8%' 
        GROUP BY  `TS_MC_NM`) `BB`
        ON 
        `AA`.`MC_NO` = `BB`.`TS_MC_NM`
        GROUP BY `AA`.`MC_NO`
        WITH ROLLUP";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_TIME);
                result.DataGridView1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }

                // Lấy thông tin tổng
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int lastIndex = ds.Tables[0].Rows.Count - 1;

                    if (!Convert.IsDBNull(ds.Tables[0].Rows[lastIndex]["SUM"]))
                    {
                        result.Label2 = ds.Tables[0].Rows[lastIndex]["SUM"].ToString();
                    }

                    if (!Convert.IsDBNull(ds.Tables[0].Rows[lastIndex]["RUN_RAT"]))
                    {
                        double ratValue = Convert.ToDouble(ds.Tables[0].Rows[lastIndex]["RUN_RAT"]);
                        result.Label3 = ratValue.ToString("P0");
                    }

                    if (!Convert.IsDBNull(ds.Tables[0].Rows[lastIndex]["UPH"]))
                    {
                        result.Label4 = ds.Tables[0].Rows[lastIndex]["UPH"].ToString();
                    }
                }

                // Lấy thông tin dừng máy
                var stopResult = await DB_STOP();
                result.DataGridView2 = stopResult.DataGridView2;

                if (!string.IsNullOrEmpty(stopResult.Error))
                {
                    result.Error = stopResult.Error;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> DB_STOP()
        {
            BaseResult result = new BaseResult();
            try
            {
                string SQL_STOP = @"SELECT `tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN` 
            FROM tsnon_oper_mitor 
            WHERE NOT(`tsnon_oper_mitor_RUNYN` = 'N') AND `tsnon_oper_mitor_MCNM` LIKE 'ZA8%'";

                DataSet dsStop = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_STOP);
                result.DataGridView2 = new List<SuperResultTranfer>();
                for (int i = 0; i < dsStop.Tables.Count; i++)
                {
                    DataTable dt = dsStop.Tables[i];
                    result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> MC_STOP_RUN(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && !string.IsNullOrEmpty(BaseParameter.MC_NAME))
                {
                    string MC_NAME = BaseParameter.MC_NAME;
                    string USER_ID = !string.IsNullOrEmpty(BaseParameter.USER_ID) ? BaseParameter.USER_ID : "System";

                    string sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES 
                    ('" + MC_NAME + "', '-----', 'N','-') ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = '-----', `tsnon_oper_mitor_RUNYN` = 'N',`StopCode`='-'";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    sql = @"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`) VALUES 
                    ('NW', '" + MC_NAME + "', '" + USER_ID + "', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), 'E', NOW(), NOW(), '" + USER_ID + "')";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    try
                    {
                        sql = "ALTER TABLE `tsnon_oper_mitor` AUTO_INCREMENT= 1";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> MC_STOPN(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && !string.IsNullOrEmpty(BaseParameter.MC_NAME))
                {
                    string MC_NAME = BaseParameter.MC_NAME;
                    string USER_ID = !string.IsNullOrEmpty(BaseParameter.USER_ID) ? BaseParameter.USER_ID : "System";

                    string sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES 
                    ('" + MC_NAME + "', 'No Worker', 'W','W') ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = 'No Worker', `tsnon_oper_mitor_RUNYN` = 'W',`StopCode`='W'";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    sql = @"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`) VALUES 
                    ('NW', '" + MC_NAME + "', '" + USER_ID + "', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), 'S', NOW(), NOW(), '" + USER_ID + "')";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    try
                    {
                        sql = "ALTER TABLE `tsnon_oper_mitor` AUTO_INCREMENT= 1";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }
                    catch { }
                }
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
                if (BaseParameter != null && BaseParameter.Action == 1)
                {
                    result = await MC_STOP_RUN(BaseParameter);
                }
                else if (BaseParameter != null && BaseParameter.Action == 2)
                {
                    result = await MC_STOPN(BaseParameter);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        // Thêm vào H06Service
        public virtual async Task<BaseResult> GetMonthlyChart(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string month = BaseParameter?.MONTH ?? DateTime.Now.ToString("yyyy-MM");
                DateTime firstDay = DateTime.Parse(month + "-01");

                string startDateTime = firstDay.ToString("yyyy-MM-dd") + " 06:00:00";
                string endDateTime = firstDay.AddMonths(1).ToString("yyyy-MM-dd") + " 06:00:00";
                string startDate = firstDay.ToString("yyyy-MM-dd");
                string endDate = firstDay.AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");

                string SQL_CHART = $@"
SELECT 
    AA.WORK_DATE AS `DATE`,
    SUM(AA.SUM) AS `TOTAL_QTY`,
    ROUND(AVG(AA.SUM / ((TIMESTAMPDIFF(SECOND, BB.TS_TIME_ST, 
        CASE 
            WHEN AA.WORK_DATE = DATE_FORMAT(NOW(), '%Y-%m-%d') THEN NOW()
            ELSE DATE_ADD(STR_TO_DATE(CONCAT(AA.WORK_DATE, ' 06:00:00'), '%Y-%m-%d %H:%i:%s'), INTERVAL 1 DAY)
        END
    ) - IFNULL(AA.T_STOP, 0)) / 3600)), 0) AS `UPH`
FROM
(SELECT 
    A.WORK_DATE,
    A.MC_NO, 
    A.SUM,
    IFNULL(B.T_STOP, 0) AS T_STOP
FROM
(SELECT 
    DATE_FORMAT(
        IF(TIME(CREATE_DTM) < '06:00:00', 
           DATE_SUB(DATE(CREATE_DTM), INTERVAL 1 DAY), 
           DATE(CREATE_DTM)
        ), '%Y-%m-%d') AS WORK_DATE,
    MC_NO, 
    SUM(WK_QTY) AS `SUM` 
FROM TWWKAR 
WHERE CREATE_DTM >= '{startDateTime}' AND CREATE_DTM < '{endDateTime}'
GROUP BY WORK_DATE, MC_NO) A
LEFT JOIN       
(SELECT 
    TSNON_OPER_MCNM,
    DATE_FORMAT(
        IF(TIME(CREATE_DTM) < '06:00:00', 
           DATE_SUB(DATE(CREATE_DTM), INTERVAL 1 DAY), 
           DATE(CREATE_DTM)
        ), '%Y-%m-%d') AS STOP_DATE,
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'S' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'Q' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'L' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'N' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'T' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'M' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) AS `T_STOP`
FROM TSNON_OPER 
WHERE CREATE_DTM >= '{startDateTime}' AND CREATE_DTM < '{endDateTime}'
GROUP BY TSNON_OPER_MCNM, STOP_DATE) B
ON A.MC_NO = B.TSNON_OPER_MCNM AND A.WORK_DATE = B.STOP_DATE
WHERE A.MC_NO LIKE 'ZA8%') AA

LEFT JOIN
(SELECT 
    TS_MC_NM, 
    TS_DATE,
    MIN(TS_TIME_ST) AS TS_TIME_ST
FROM TUSER_LOG  
WHERE TS_DATE >= '{startDate}' AND TS_DATE <= '{endDate}'
  AND TS_MC_NM LIKE 'ZA8%' 
GROUP BY TS_MC_NM, TS_DATE) BB
ON AA.MC_NO = BB.TS_MC_NM AND AA.WORK_DATE = BB.TS_DATE

GROUP BY AA.WORK_DATE
ORDER BY AA.WORK_DATE";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_CHART);
                result.DataGridView1 = new List<SuperResultTranfer>();
                if (ds.Tables.Count > 0)
                {
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
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