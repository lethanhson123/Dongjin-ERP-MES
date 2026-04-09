namespace MESService.Implement
{
    public class H14Service : BaseService<torderlist, ItorderlistRepository>
    , IH14Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public H14Service(ItorderlistRepository torderlistRepository

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
                string sqlCheck = @"SELECT `TSNON_OPER_MCNM`, `TSNON_OPER_CODE` 
                  FROM TSNON_OPER 
                  WHERE `TSNON_OPER_ETIME` IS NULL 
                  AND `CREATE_DTM` > DATE_FORMAT(NOW(), '%Y-%m-%d 05:00:00') 
                  AND (`TSNON_OPER_MCNM` = 'B218' OR `TSNON_OPER_MCNM` = 'B219' OR `TSNON_OPER_MCNM` = 'B220' OR `TSNON_OPER_MCNM` = 'B221') 
                  GROUP BY `TSNON_OPER_MCNM`";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlCheck);
                result = await Button1_Click(new BaseParameter());
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
                string SQL_TIME = @"SELECT  IFNULL(`DATE`, DATE_FORMAT(NOW(), '%Y-%m-%d')) AS `DATE`,  
IFNULL(`MC_NO`, '') AS `MC_NO`,                           
IFNULL(`TS_TIME_ST`, DATE_FORMAT(NOW(), '%Y-%m-%d 05:00:00')) AS `TS_TIME_ST`,  
IFNULL(`TS_TIME_END`, DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s')) AS `TS_TIME_END`,
IFNULL(SUM(`SUM`), 0) AS `SUM`,                         
IFNULL(ROUND(AVG(`STOP_TIME`), 3), 0) AS `STOP_TIME`,   
IFNULL(ROUND(AVG(`T_STOP`), 3), 0) AS `T_STOP`,       
IFNULL(ROUND(AVG(`WORK_TIME`), 3), 0) AS `WORK_TIME`,  
IFNULL(ROUND(AVG(`RUN_RAT`), 2), 0) AS `RUN_RAT`,     
IFNULL(ROUND(AVG(`UPH`), 0), 0) AS `UPH`         
FROM (SELECT DATE_FORMAT(NOW(), '%Y-%m-%d') AS `DATE`,
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
FROM TWWKAR_LP 
WHERE 
`CREATE_DTM` > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 05:00:00'), '%Y-%m-%d %T') AND 
`CREATE_DTM` < DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL +1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), ' 05:00:00'), '%Y-%m-%d %T')  
GROUP BY  `MC_NO`) `A`
LEFT JOIN       
(SELECT `TSNON_OPER_MCNM`, 
SUM(TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) AS `STOP_TIME`,
SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'L' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'T' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0))+
SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'M' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) AS `T_STOP`
FROM TSNON_OPER 
WHERE 
`CREATE_DTM` > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 05:00:00'), '%Y-%m-%d %T') AND 
`CREATE_DTM` < DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL +1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), ' 05:00:00'), '%Y-%m-%d %T')  
GROUP BY `TSNON_OPER_MCNM` ) `B`
ON `A`.`MC_NO` = `B`.`TSNON_OPER_MCNM`  
WHERE `A`.`MC_NO` IN ('B218', 'B219', 'B220', 'B221') ) `AA`

LEFT JOIN
(SELECT `TS_MC_NM`, DATE_FORMAT(MIN(`TS_TIME_ST`), '%Y-%m-%d 05:00:00') AS `TS_TIME_ST`, 
DATE_FORMAT(NOW(), '%Y-%m-%d %T') AS `TS_TIME_END`,
`TS_DATE` FROM TUSER_LOG  
WHERE  `TS_DATE` = DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d')
AND (`TS_MC_NM` IN ('B218', 'B219', 'B220', 'B221'))
GROUP BY  `TS_MC_NM`) `BB`
ON 
`AA`.`MC_NO` = `BB`.`TS_MC_NM`
GROUP BY `AA`.`MC_NO`) AS `T`
GROUP BY `MC_NO` WITH ROLLUP";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_TIME);

                result.DataGridView1 = new List<SuperResultTranfer>();
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }

                await DB_STOP(result);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        private async Task DB_STOP(BaseResult result)
        {
            try
            {
                string SQL_STOP = @"SELECT `tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`   
        FROM tsnon_oper_mitor 
        WHERE NOT(`tsnon_oper_mitor_RUNYN` = 'N')  
        AND (`tsnon_oper_mitor_MCNM` IN ('B218', 'B219', 'B220', 'B221'))";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_STOP);

                result.DataGridView2 = new List<SuperResultTranfer>();
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
        }
        public virtual async Task<BaseResult> Button2_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Truy vấn để lấy thông tin chi tiết cho modal
                string SQL_DETAIL = @"SELECT  IFNULL(`DATE`, DATE_FORMAT(NOW(), '%Y-%m-%d')) AS `DATE`,  
IFNULL(`MC_NO`, '') AS `MC_NO`,                           
IFNULL(`TS_TIME_ST`, DATE_FORMAT(NOW(), '%Y-%m-%d 05:00:00')) AS `TS_TIME_ST`,  
IFNULL(`TS_TIME_END`, DATE_FORMAT(NOW(), '%Y-%m-%d %H:%i:%s')) AS `TS_TIME_END`,
IFNULL(SUM(`SUM`), 0) AS `SUM`,                         
IFNULL(ROUND(AVG(`STOP_TIME`), 3), 0) AS `STOP_TIME`,   
IFNULL(ROUND(AVG(`T_STOP`), 3), 0) AS `T_STOP`,       
IFNULL(ROUND(AVG(`WORK_TIME`), 3), 0) AS `WORK_TIME`,  
IFNULL(ROUND(AVG(`RUN_RAT`), 2), 0) AS `RUN_RAT`,     
IFNULL(ROUND(AVG(`UPH`), 0), 0) AS `UPH`         
FROM (SELECT DATE_FORMAT(NOW(), '%Y-%m-%d') AS `DATE`,
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
FROM TWWKAR_LP 
WHERE 
`CREATE_DTM` > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 05:00:00'), '%Y-%m-%d %T') AND 
`CREATE_DTM` < DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL +1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), ' 05:00:00'), '%Y-%m-%d %T')  
GROUP BY  `MC_NO`) `A`
LEFT JOIN       
(SELECT `TSNON_OPER_MCNM`, 
SUM(TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) AS `STOP_TIME`,
SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'L' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'T' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0))+
SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'M' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) AS `T_STOP`
FROM TSNON_OPER 
WHERE 
`CREATE_DTM` > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 05:00:00'), '%Y-%m-%d %T') AND 
`CREATE_DTM` < DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL +1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), ' 05:00:00'), '%Y-%m-%d %T')  
GROUP BY `TSNON_OPER_MCNM` ) `B`
ON `A`.`MC_NO` = `B`.`TSNON_OPER_MCNM`  
WHERE `A`.`MC_NO` IN ('B218', 'B219', 'B220' , 'B221') ) `AA`

LEFT JOIN
(SELECT `TS_MC_NM`, DATE_FORMAT(MIN(`TS_TIME_ST`), '%Y-%m-%d 05:00:00') AS `TS_TIME_ST`, 
DATE_FORMAT(NOW(), '%Y-%m-%d %T') AS `TS_TIME_END`,
`TS_DATE` FROM TUSER_LOG  
WHERE  `TS_DATE` = DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d')
AND (`TS_MC_NM` IN ('B218', 'B219', 'B220' , 'B221'))
GROUP BY  `TS_MC_NM`) `BB`
ON 
`AA`.`MC_NO` = `BB`.`TS_MC_NM`
GROUP BY `AA`.`MC_NO`) AS `T`
GROUP BY `MC_NO` WITH ROLLUP";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_DETAIL);

                result.DataGridView3 = new List<SuperResultTranfer>();
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> MachineStopRun(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string mcName = BaseParameter.ListSearchString[0];
                string currentUser = BaseParameter.ListSearchString.Count > 1 ? BaseParameter.ListSearchString[1] : "SYSTEM";

                string sql1 = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) 
                VALUES (@mcName, '-----', 'N','-') 
                ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = '-----', `tsnon_oper_mitor_RUNYN` = 'N',`StopCode`='-'";

                string sql2 = @"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`) 
                VALUES ('NW', @mcName, @user, DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(NOW(), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), 'E', NOW(), NOW(), @user)";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql1, new MySqlParameter("@mcName", mcName));
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql2,
                    new MySqlParameter("@mcName", mcName),
                    new MySqlParameter("@user", currentUser));
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> MachineStopNoWorker(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string mcName = BaseParameter.ListSearchString[0];
                string currentUser = BaseParameter.ListSearchString.Count > 1 ? BaseParameter.ListSearchString[1] : "SYSTEM";

                string sql1 = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) 
                VALUES (@mcName, 'No Worker', 'W','W') 
                ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = 'No Worker', `tsnon_oper_mitor_RUNYN` = 'W',`StopCode`='W'";

                string sql2 = @"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`) 
                VALUES ('NW', @mcName, @user, DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(NOW(), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), 'S', NOW(), NOW(), @user)";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql1, new MySqlParameter("@mcName", mcName));
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql2,
                    new MySqlParameter("@mcName", mcName),
                    new MySqlParameter("@user", currentUser));
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> NonWorkerAuto(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`)
               SELECT 
                   'NW', `TSNON_OPER_MCNM`, 'MES', DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY),'%Y-%m-%d') AS `DT`, 'E', CONCAT(DATE_FORMAT(NOW(),'%Y-%m-%d'), ' 05:59:59'), NOW(), 'MES'
               FROM (SELECT `TSNON_OPER_MCNM`,
                   COUNT(case when `TSNON_OPER_COL`='S' then `TSNON_OPER_COL` END) AS `S`,
                   COUNT(case when `TSNON_OPER_COL`='E' then `TSNON_OPER_COL` END) AS `E`,
                   COUNT(case when `TSNON_OPER_COL`='S' then `TSNON_OPER_COL` END) - COUNT(case when `TSNON_OPER_COL`='E' then `TSNON_OPER_COL` END) AS `SUM`
                   FROM TSNON_OPER_WORKER
                   WHERE TSNON_OPER_WORKER.`TSNON_OPER_TIME` >= CONCAT(DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY),'%Y-%m-%d'), ' 05:00:00') 
                   AND TSNON_OPER_WORKER.`TSNON_OPER_TIME` < CONCAT(DATE_FORMAT(NOW(),'%Y-%m-%d'), ' 05:00:00')
                   AND TSNON_OPER_WORKER.`TSNON_OPER_MCNM` IN ('B218', 'B219', 'B220', 'B221')
                   GROUP BY TSNON_OPER_MCNM
                   HAVING (COUNT(case when `TSNON_OPER_COL`='S' then `TSNON_OPER_COL` END) - COUNT(case when `TSNON_OPER_COL`='E' then `TSNON_OPER_COL` END)) > 0) `T_B`";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> GetChartData(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // SQL cho Production Trend
                string sqlProduction = @"SELECT 
            DATE_FORMAT(CREATE_DTM, '%H:00') as Hour,
            SUM(CASE WHEN MC_NO = 'B218' THEN WK_QTY ELSE 0 END) as B218,
            SUM(CASE WHEN MC_NO = 'B219' THEN WK_QTY ELSE 0 END) as B219,
            SUM(CASE WHEN MC_NO = 'B220' THEN WK_QTY ELSE 0 END) as B220,
            SUM(CASE WHEN MC_NO = 'B221' THEN WK_QTY ELSE 0 END) as B221
        FROM TWWKAR_LP 
        WHERE 
        CREATE_DTM > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 05:00:00'), '%Y-%m-%d %T') AND 
        CREATE_DTM < DATE_FORMAT(NOW(), '%Y-%m-%d %T')
        GROUP BY DATE_FORMAT(CREATE_DTM, '%H:00')
        ORDER BY Hour";

                // SQL cho Downtime Analysis
                string sqlDowntime = @"SELECT 
            TSNON_OPER_CODE as PART_CODE,
            CASE 
                WHEN TSNON_OPER_CODE = 'L' THEN '식사시간 (Giờ ăn)'
                WHEN TSNON_OPER_CODE = 'T' THEN '교육/회의 (Đào tạo/họp)'
                WHEN TSNON_OPER_CODE = 'M' THEN '설비고장 (Máy hư)'
                WHEN TSNON_OPER_CODE = 'S' THEN '설비준비 (Chuẩn bị thiết bị)'
                WHEN TSNON_OPER_CODE = 'I' THEN '자재결품 (Thiếu nguyên vật liệu)'
                WHEN TSNON_OPER_CODE = 'Q' THEN '품질문제 (Vấn đề chất lượng)'
                ELSE TSNON_OPER_CODE
            END as Reason,
            SUM(TIMESTAMPDIFF(MINUTE, TSNON_OPER_STIME, IFNULL(TSNON_OPER_ETIME, NOW()))) as DURATION
        FROM TSNON_OPER
        WHERE 
        CREATE_DTM > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 05:00:00'), '%Y-%m-%d %T') AND 
        CREATE_DTM < DATE_FORMAT(NOW(), '%Y-%m-%d %T')
        AND TSNON_OPER_MCNM IN ('B218', 'B219', 'B220', 'B221')
        GROUP BY TSNON_OPER_CODE
        ORDER BY DURATION DESC";

                DataSet dsProduction = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlProduction);
                DataSet dsDowntime = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlDowntime);

                result.ProductionTrend = new List<SuperResultTranfer>();
                result.DowntimeAnalysis = new List<SuperResultTranfer>();

                if (dsProduction.Tables.Count > 0)
                {
                    DataTable dt = dsProduction.Tables[0];
                    result.ProductionTrend.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }

                if (dsDowntime.Tables.Count > 0)
                {
                    DataTable dt = dsDowntime.Tables[0];
                    result.DowntimeAnalysis.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
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
                await Task.Run(() => { });
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

