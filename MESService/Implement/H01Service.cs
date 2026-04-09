namespace MESService.Implement
{
    public class H01Service : BaseService<torderlist, ItorderlistRepository>, IH01Service
    {
        private readonly ItorderlistRepository _torderlistRepository;

        public H01Service(ItorderlistRepository torderlistRepository) : base(torderlistRepository)
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
                string DGV_DATA1 = "SELECT `TSNON_OPER_MCNM`, `TSNON_OPER_CODE` FROM TSNON_OPER WHERE `TSNON_OPER_ETIME` IS NULL AND `CREATE_DTM` > DATE_FORMAT(NOW(), '%Y-%m-%d 06:00:00') AND `TSNON_OPER_MCNM` LIKE 'A%' GROUP BY `TSNON_OPER_MCNM`";
                DataSet dsDGV_01 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1);
                int DB_II = dsDGV_01.Tables[0].Rows.Count;

                string DGV_DATA2 = "SELECT `TSNON_OPER_MCNM`, `TSNON_OPER_CODE` FROM TSNON_OPER WHERE `TSNON_OPER_ETIME` IS NULL AND `CREATE_DTM` > DATE_FORMAT(NOW(), '%Y-%m-%d 06:00:00') AND `TSNON_OPER_MCNM` LIKE 'A%' GROUP BY `TSNON_OPER_MCNM`";
                DataSet dsDGV_02 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA2);
                int DB_JJ = dsDGV_02.Tables[0].Rows.Count;

                if (DB_II != DB_JJ)
                {
                    string SQL1 = @"UPDATE tsnon_oper_mitor SET `tsnon_oper_mitor_RUNYN` = 'N' WHERE NOT(`tsnon_oper_mitor_MCNM` = 
(SELECT TSNON_OPER.TSNON_OPER_MCNM FROM TSNON_OPER WHERE TSNON_OPER.TSNON_OPER_ETIME IS NULL AND TSNON_OPER.CREATE_DTM > DATE_FORMAT(NOW(), '%Y-%m-%d 06:00:00') GROUP BY TSNON_OPER.TSNON_OPER_MCNM))";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);
                }

                await Dgv_reload(result);
                await DB_STOP(result);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> RELOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string startDate = BaseParameter.START_DATE ?? DateTime.Now.ToString("yyyy-MM-dd");
                string endDate = BaseParameter.END_DATE ?? startDate;
                string machineList = BaseParameter.MACHINE_LIST ?? "A801,A802,A803,A804,A805,A806,A807,A808,A809,A810,A811,A812,A813,A814,A815,A816,A501,A502,A830,A831,A832,A833";

                string SQL_CALL = $"CALL sp_GetCutingUPH_DateRange('{machineList}', '{startDate}', '{endDate}')";
                DataSet dsSQL_TIME = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_CALL);
                result.DataGridView1 = new List<SuperResultTranfer>();

                for (int i = 0; i < dsSQL_TIME.Tables.Count; i++)
                {
                    DataTable dt = dsSQL_TIME.Tables[i];
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

        private async Task Dgv_reload(BaseResult result)
        {
            try
            {
                string SQL_TIME = @"SELECT 
DATE_FORMAT(NOW(), '%Y-%m-%d') AS `DATE`,
IFNULL(`AA`.`MC_NO`, 'TOTAL') AS `MC_NO`, 
    SUM(`AA`.`SUM`) AS `SUM`,
    ROUND(SUM(IFNULL(`AA`.`STOP_TIME`,0)) / 3600,3) AS `STOP_TIME`,
    ROUND(SUM(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`)) / 3600,3) AS `WORK_TIME`,
    ROUND(
        SUM(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) - IFNULL(`AA`.`STOP_TIME`, 0))
        / SUM(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`))
    , 3) AS `RUN_RAT`,
    ROUND(
        SUM(`AA`.`SUM`) 
        / (SUM(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) - IFNULL(`AA`.`STOP_TIME`, 0)) / 3600)
    , 0) AS `UPH`
FROM
(SELECT  *  FROM
(SELECT `MC_NO`, SUM(`WK_QTY`) AS `SUM` 
FROM TWWKAR 
WHERE 
`CREATE_DTM` > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T') AND 
`CREATE_DTM` < DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL +1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T')  
GROUP BY  `MC_NO`) `A`
LEFT JOIN       
(SELECT `TSNON_OPER_MCNM`, 
SUM(TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) AS `STOP_TIME`
FROM TSNON_OPER 
WHERE 
`CREATE_DTM` > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T') AND 
`CREATE_DTM` < DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL +1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T')  
GROUP BY `TSNON_OPER_MCNM` ) `B`
ON `A`.`MC_NO` = `B`.`TSNON_OPER_MCNM`  
WHERE `A`.`MC_NO` IN ('A801' ,'A802','A803', 'A804', 'A805', 'A806', 'A807', 'A808', 'A809' , 'A810', 'A811', 'A812', 'A813', 'A814', 'A815','A816', 'A501', 'A502','A830','A831','A832','A833')) `AA`
LEFT JOIN
(SELECT `TS_MC_NM`, DATE_FORMAT(MIN(`TS_TIME_ST`), '%Y-%m-%d %T') AS `TS_TIME_ST`, 
DATE_FORMAT(NOW(), '%Y-%m-%d %T') AS `TS_TIME_END`,
`TS_DATE` FROM TUSER_LOG  
WHERE  `TS_DATE` = DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d')
AND `TS_MC_NM` IN ('A801' ,'A802','A803', 'A804', 'A805', 'A806', 'A807', 'A808', 'A809' , 'A810', 'A811', 'A812', 'A813', 'A814', 'A815','A816', 'A501', 'A502','A830','A831','A832','A833') 
GROUP BY  `TS_MC_NM`) `BB`
ON 
`AA`.`MC_NO` = `BB`.`TS_MC_NM`
GROUP BY `AA`.`MC_NO`
WITH ROLLUP";

                DataSet dsSQL_TIME = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_TIME);
                result.DataGridView1 = new List<SuperResultTranfer>();

                for (int i = 0; i < dsSQL_TIME.Tables.Count; i++)
                {
                    DataTable dt = dsSQL_TIME.Tables[i];
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task DB_STOP(BaseResult result)
        {
            try
            {
                string SQL_STOP = @"SELECT `tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN` 
                      FROM tsnon_oper_mitor 
                      WHERE NOT(`tsnon_oper_mitor_RUNYN` = 'N')  
                      AND `tsnon_oper_mitor_MCNM` IN ('A801','A802','A803','A804','A805','A806','A807','A808','A809','A810','A811','A812','A813','A814','A815','A816','A501','A502','A830','A831','A832','A833')";

                DataSet dsSQL_STOP = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_STOP);

                result.DataGridView2 = new List<SuperResultTranfer>();
                if (dsSQL_STOP.Tables.Count > 0)
                {
                    DataTable dt = dsSQL_STOP.Tables[0];
                    result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public virtual async Task<BaseResult> NON_WORKER_AUTO(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`)
(SELECT 
'NW', `TSNON_OPER_MCNM`, 'MES', DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY),'%Y-%m-%d') AS `DT`, 'E', CONCAT(DATE_FORMAT(NOW(),'%Y-%m-%d'), ' 05:59:59'), NOW(), 'MES'
FROM (SELECT `TSNON_OPER_MCNM`,
COUNT(case when `TSNON_OPER_COL`='S' then `TSNON_OPER_COL` END) AS `S`,
COUNT(case when `TSNON_OPER_COL`='E' then `TSNON_OPER_COL` END) AS `E`,
COUNT(case when `TSNON_OPER_COL`='S' then `TSNON_OPER_COL` END) - COUNT(case when `TSNON_OPER_COL`='E' then `TSNON_OPER_COL` END) AS `SUM`
FROM TSNON_OPER_WORKER
WHERE TSNON_OPER_WORKER.`TSNON_OPER_TIME` >= CONCAT(DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY),'%Y-%m-%d'), ' 06:00:00') AND 
TSNON_OPER_WORKER.`TSNON_OPER_TIME` < CONCAT(DATE_FORMAT(NOW(),'%Y-%m-%d'), ' 06:00:00')
GROUP BY TSNON_OPER_MCNM
HAVING (COUNT(case when `TSNON_OPER_COL`='S' then `TSNON_OPER_COL` END) - COUNT(case when `TSNON_OPER_COL`='E' then `TSNON_OPER_COL` END)) > 0) `T_B`)";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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

                    string sql1 = $@"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES 
('{MC_NAME}', '-----', 'N','-')  
ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = '-----', `tsnon_oper_mitor_RUNYN` = 'N',`StopCode`='-'";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql1);

                    string sql2 = $@"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`) VALUES 
   ('NW', '{MC_NAME}', '{BaseParameter.USER_NM}', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), 'E', NOW(), NOW(), '{BaseParameter.USER_NM}')";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql2);

                    try
                    {
                        string sql3 = "ALTER TABLE `tsnon_oper_mitor` AUTO_INCREMENT= 1";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql3);
                    }
                    catch { /* Bỏ qua lỗi nếu có */ }
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

                    string sql1 = $@"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES 
('{MC_NAME}', 'No Worker', 'W','W')  
ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = 'No Worker', `tsnon_oper_mitor_RUNYN` = 'W',`StopCode`='W'";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql1);

                    string sql2 = $@"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`) VALUES 
   ('NW', '{MC_NAME}', '{BaseParameter.USER_NM}', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), 'S', NOW(), NOW(), '{BaseParameter.USER_NM}')";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql2);

                    try
                    {
                        string sql3 = "ALTER TABLE `tsnon_oper_mitor` AUTO_INCREMENT= 1";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql3);
                    }
                    catch { /* Bỏ qua lỗi nếu có */ }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        // Thêm vào H01Service.cs

        public virtual async Task<BaseResult> GetMonthlyProduction(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string monthStr = BaseParameter.MONTH ?? DateTime.Now.ToString("yyyy-MM");
                DateTime firstDay = DateTime.Parse($"{monthStr}-01");
                DateTime lastDay = firstDay.AddMonths(1).AddDays(-1);

                if (firstDay.Year == DateTime.Now.Year && firstDay.Month == DateTime.Now.Month)
                {
                    lastDay = DateTime.Now.Date;
                }

                string machineList = BaseParameter.MACHINE_LIST ?? "A801,A802,A803,A804,A805,A806,A807,A808,A809,A810,A811,A812,A813,A814,A815,A816,A501,A502,A830,A831,A832,A833";

                // Query theo ca làm việc 06:00 -> 06:00
                string SQL_MONTHLY = $@"
SELECT 
    DATE_FORMAT(
        CASE 
            WHEN TIME(TWWKAR.CREATE_DTM) >= '06:00:00' THEN DATE(TWWKAR.CREATE_DTM)
            ELSE DATE(DATE_SUB(TWWKAR.CREATE_DTM, INTERVAL 1 DAY))
        END, 
        '%Y-%m-%d'
    ) AS `DATE`,
    SUM(TWWKAR.WK_QTY) AS `TOTAL_QTY`
FROM TWWKAR
WHERE TWWKAR.CREATE_DTM >= '{firstDay:yyyy-MM-dd} 06:00:00'
  AND TWWKAR.CREATE_DTM < '{lastDay.AddDays(1):yyyy-MM-dd} 06:00:00'
  AND FIND_IN_SET(TWWKAR.MC_NO, '{machineList}') > 0
GROUP BY DATE
ORDER BY DATE";

                DataSet dsMonthly = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_MONTHLY);

                result.DataGridView1 = new List<SuperResultTranfer>();
                if (dsMonthly.Tables.Count > 0)
                {
                    DataTable dt = dsMonthly.Tables[0];
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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