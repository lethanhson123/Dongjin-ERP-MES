namespace MESService.Implement
{
    public class C10Service : BaseService<torderlist, ItorderlistRepository>
    , IC10Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public C10Service(ItorderlistRepository torderlistRepository

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
                // Kiểm tra kết nối (tương tự MES_Connect trong VB)
                string DGV_DATA1 = @"SELECT `TSNON_OPER_MCNM`, `TSNON_OPER_CODE` FROM TSNON_OPER WHERE `TSNON_OPER_ETIME` IS NULL AND `CREATE_DTM` > DATE_FORMAT(NOW(), '%Y-%m-%d 06:00:00') AND `TSNON_OPER_MCNM` LIKE 'A8%' GROUP BY `TSNON_OPER_MCNM`";
                DataSet dsDGV_01 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA1);
                int DB_II = dsDGV_01.Tables["DGV_C08_01"].Rows.Count;

                string DGV_DATA2 = @"SELECT `TSNON_OPER_MCNM`, `TSNON_OPER_CODE` FROM TSNON_OPER WHERE `TSNON_OPER_ETIME` IS NULL AND `CREATE_DTM` > DATE_FORMAT(NOW(), '%Y-%m-%d 06:00:00') AND `TSNON_OPER_MCNM` LIKE 'A8%' GROUP BY `TSNON_OPER_MCNM`";
                DataSet dsDGV_02 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, DGV_DATA2);
                int DB_JJ = dsDGV_02.Tables["DGV_C08_02"].Rows.Count;

                if (DB_II != DB_JJ)
                {
                    string SQL1 = @"UPDATE tsnon_oper_mitor SET `tsnon_oper_mitor_RUNYN` = 'N' WHERE NOT(`tsnon_oper_mitor_MCNM` = 
                            (SELECT TSNON_OPER.TSNON_OPER_MCNM FROM TSNON_OPER WHERE TSNON_OPER.TSNON_OPER_ETIME IS NULL AND TSNON_OPER.CREATE_DTM > DATE_FORMAT(NOW(), '%Y-%m-%d 06:00:00') GROUP BY TSNON_OPER.TSNON_OPER_MCNM))";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);
                }

                // Gọi Dgv_reload
                string DT_TEXT = DateTime.Now.ToString("yyyy-MM-dd");
                string SQL_TIME = @"SELECT 
                            '" + DT_TEXT + @"' AS `DATE`,
                            IFNULL(`AA`.`MC_NO`, '') AS `MC_NO`, 
                            `BB`.`TS_TIME_ST`,
                            `BB`.`TS_TIME_END`,
                            SUM(`AA`.`SUM`) AS `SUM`,
                            IFNULL(ROUND(AVG(`AA`.`STOP_TIME`/3600), 3), 0) AS `STOP_TIME`,
                            IFNULL(ROUND(AVG(`AA`.`T_STOP`/3600), 3), 0) AS `T_STOP`,
                            ROUND(AVG(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600), 6) AS `WORK_TIME`,
                            ROUND(AVG(((ROUND(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600, 6) 
                                - (IFNULL(ROUND(`AA`.`STOP_TIME`/3600, 3), 0) - IFNULL(ROUND(`AA`.`T_STOP`/3600, 3), 0))) 
                                / ROUND(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600, 6))), 2) AS `RUN_RAT`,
                            ROUND(AVG(`AA`.`SUM` / ((TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) - IFNULL(`AA`.`T_STOP`, 0)) / 3600)), 0) AS `UPH`
                            FROM
                            (SELECT * FROM (
                                SELECT `MC_NO`, SUM(`WK_QTY`) AS `SUM` 
                                FROM TWWKAR 
                                WHERE 
                                `CREATE_DTM` > '" + DT_TEXT + @" 06:00:00' AND 
                                `CREATE_DTM` < DATE_FORMAT(DATE_ADD('" + DT_TEXT + @"', INTERVAL +1 DAY), '%Y-%m-%d 06:00:00')  
                                GROUP BY `MC_NO`) `A`
                                LEFT JOIN (
                                SELECT `TSNON_OPER_MCNM`, 
                                SUM(TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) AS `STOP_TIME`,
                                SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'L' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
                                SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'T' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) AS `T_STOP`
                                FROM TSNON_OPER 
                                WHERE 
                                `CREATE_DTM` > '" + DT_TEXT + @" 06:00:00' AND 
                                `CREATE_DTM` < DATE_FORMAT(DATE_ADD('" + DT_TEXT + @"', INTERVAL +1 DAY), '%Y-%m-%d 06:00:00') 
                                GROUP BY `TSNON_OPER_MCNM`) `B`
                                ON `A`.`MC_NO` = `B`.`TSNON_OPER_MCNM`  
                                WHERE `A`.`MC_NO` LIKE 'A8%') `AA`
                            LEFT JOIN (
                                SELECT * FROM (
                                    SELECT  
                                    `BA`.`TS_MC_NM`,
                                    IFNULL(`BAA`.`START_TIME`, `BA`.`TS_TIME_ST`) AS `TS_TIME_ST`,
                                    IFNULL(`BAA`.`END_TIME`, `BA`.`TS_TIME_END`) AS `TS_TIME_END`,
                                    `BA`.`TS_DATE`
                                    FROM (
                                        SELECT `TS_MC_NM`, DATE_FORMAT(MIN(`TS_TIME_ST`), '%Y-%m-%d %T') AS `TS_TIME_ST`, 
                                        IFNULL(MAX(`TS_TIME_END`), '" + DT_TEXT + @" 06:00:00') AS `TS_TIME_END`,
                                        `TS_DATE` FROM TUSER_LOG  
                                        WHERE `TS_DATE` = '" + DT_TEXT + @"'
                                        AND `TS_MC_NM` LIKE 'A8%' 
                                        GROUP BY `TS_MC_NM`) `BA` 
                                        LEFT JOIN (
                                        SELECT (SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TSNON_MCIDX`) AS `MC_NAME`, 
                                        MIN(`TSNON_ST`) AS `START_TIME`, MAX(`TSNON_ET`) AS `END_TIME`
                                        FROM TSNON_WORKTIME 
                                        WHERE `TSNON_DATE` = '" + DT_TEXT + @"' AND `TSNON_MCIDX`
                                        GROUP BY `TSNON_MCIDX`) `BAA`
                                        ON `BA`.`TS_MC_NM` = `BAA`.`MC_NAME`) `BBA`) `BB`
                            ON `AA`.`MC_NO` = `BB`.`TS_MC_NM`
                            GROUP BY `AA`.`MC_NO`
                            WITH ROLLUP";

                DataSet dsSQL_TIME = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_TIME);
                result.DataGridView1 = new List<SuperResultTranfer>();
                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dsSQL_TIME.Tables["DB_TIME"]));

                // Gọi DB_STOP
                string SQL_STOP = @"SELECT `tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN` FROM tsnon_oper_mitor WHERE NOT(`tsnon_oper_mitor_RUNYN` = 'N') AND `tsnon_oper_mitor_MCNM` LIKE 'A8%'";
                DataSet dsSQL_STOP = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_STOP);
                result.DataGridView2 = new List<SuperResultTranfer>();
                result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dsSQL_STOP.Tables["DB_STOP"]));
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> NON_WORKER_AUTO()
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`)
                              (SELECT 
                              'NW', `TSNON_OPER_MCNM`, 'MES', DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d') AS `DT`, 'E', CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' 05:59:59'), NOW(), 'MES'
                              FROM (SELECT `TSNON_OPER_MCNM`,
                              COUNT(case when `TSNON_OPER_COL`='S' then `TSNON_OPER_COL` END) AS `S`,
                              COUNT(case when `TSNON_OPER_COL`='E' then `TSNON_OPER_COL` END) AS `E`,
                              COUNT(case when `TSNON_OPER_COL`='S' then `TSNON_OPER_COL` END) - COUNT(case when `TSNON_OPER_COL`='E' then `TSNON_OPER_COL` END) AS `SUM`
                              FROM TSNON_OPER_WORKER
                              WHERE TSNON_OPER_WORKER.`TSNON_OPER_TIME` >= CONCAT(DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d'), ' 06:00:00') AND 
                              TSNON_OPER_WORKER.`TSNON_OPER_TIME` < CONCAT(DATE_FORMAT(NOW(), '%Y-%m-%d'), ' 06:00:00')
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
              
                string MC_NAME = BaseParameter?.ListSearchString?[0] ?? string.Empty;

                string sql1 = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES 
                       ('" + MC_NAME + @"', '-----', 'N','-')  
                       ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = '-----', `tsnon_oper_mitor_RUNYN` = 'N',`StopCode`='-'";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql1);

                string sql2 = @"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`) VALUES 
                       ('NW', '" + MC_NAME + @"', 'MES', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), 'E', NOW(), NOW(), 'MES')";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql2);

                string sql3 = @"ALTER TABLE `tsnon_oper_mitor` AUTO_INCREMENT = 1";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql3);
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
              
                string MC_NAME = BaseParameter?.ListSearchString?[0] ?? string.Empty;

                string sql1 = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES 
                       ('" + MC_NAME + @"', 'No Worker', 'W','W')  
                       ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = 'No Worker', `tsnon_oper_mitor_RUNYN` = 'W',`StopCode`='W'";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql1);

                string sql2 = @"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`) VALUES 
                       ('NW', '" + MC_NAME + @"', 'MES', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), 'S', NOW(), NOW(), 'MES')";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql2);

                string sql3 = @"ALTER TABLE `tsnon_oper_mitor` AUTO_INCREMENT = 1";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql3);
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
                string DT_TEXT = DateTime.Parse(BaseParameter?.ListSearchString?[0] ?? DateTime.Now.ToString("yyyy-MM-dd")).ToString("yyyy-MM-dd");
                string SQL_TIME = @"
    SELECT 
        '" + DT_TEXT + @"' AS `DATE`,
        IFNULL(`AA`.`MC_NO`, '') AS `MC_NO`, 
        `BB`.`TS_TIME_ST`,
        `BB`.`TS_TIME_END`,
        SUM(`AA`.`SUM`) AS `SUM`,
        IFNULL(ROUND(AVG(`AA`.`STOP_TIME`/3600), 3), 0) AS `STOP_TIME`,
        IFNULL(ROUND(AVG(`AA`.`T_STOP`/3600), 3), 0) AS `T_STOP`,
        ROUND(AVG(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600), 6) AS `WORK_TIME`,
        ROUND(AVG(((ROUND(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600, 6) 
            - (IFNULL(ROUND(`AA`.`STOP_TIME`/3600, 3), 0) - IFNULL(ROUND(`AA`.`T_STOP`/3600, 3), 0))) 
            / ROUND(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600, 6))), 2) AS `RUN_RAT`,
        ROUND(AVG(`AA`.`SUM` / ((TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) - IFNULL(`AA`.`T_STOP`, 0)) / 3600)), 0) AS `UPH`
    FROM
        (SELECT * FROM (
            SELECT `MC_NO`, SUM(`WK_QTY`) AS `SUM` 
            FROM TWWKAR 
            WHERE 
                `CREATE_DTM` > '" + DT_TEXT + @" 06:00:00' AND 
                `CREATE_DTM` < DATE_FORMAT(DATE_ADD('" + DT_TEXT + @"', INTERVAL +1 DAY), '%Y-%m-%d 06:00:00')  
            GROUP BY `MC_NO`) `A`
            LEFT JOIN (
                SELECT `TSNON_OPER_MCNM`, 
                SUM(TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) AS `STOP_TIME`,
                SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'L' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
                SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'T' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) AS `T_STOP`
                FROM TSNON_OPER 
                WHERE 
                    `CREATE_DTM` > '" + DT_TEXT + @" 06:00:00' AND 
                    `CREATE_DTM` < DATE_FORMAT(DATE_ADD('" + DT_TEXT + @"', INTERVAL +1 DAY), '%Y-%m-%d 06:00:00') 
                GROUP BY `TSNON_OPER_MCNM`) `B`
            ON `A`.`MC_NO` = `B`.`TSNON_OPER_MCNM`  
            WHERE `A`.`MC_NO` LIKE 'A8%') `AA`
        LEFT JOIN (
            SELECT * FROM (
                SELECT  
                    `BA`.`TS_MC_NM`,
                    IFNULL(`BAA`.`START_TIME`, `BA`.`TS_TIME_ST`) AS `TS_TIME_ST`,
                    IFNULL(`BAA`.`END_TIME`, `BA`.`TS_TIME_END`) AS `TS_TIME_END`,
                    `BA`.`TS_DATE`
                FROM (
                    SELECT `TS_MC_NM`, DATE_FORMAT(MIN(`TS_TIME_ST`), '%Y-%m-%d %T') AS `TS_TIME_ST`, 
                    IFNULL(MAX(`TS_TIME_END`), '" + DT_TEXT + @" 06:00:00') AS `TS_TIME_END`,
                    `TS_DATE` FROM TUSER_LOG  
                    WHERE `TS_DATE` = '" + DT_TEXT + @"'
                    AND `TS_MC_NM` LIKE 'A8%' 
                    GROUP BY `TS_MC_NM`) `BA` 
                LEFT JOIN (
                    SELECT (SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TSNON_MCIDX`) AS `MC_NAME`, 
                    MIN(`TSNON_ST`) AS `START_TIME`, MAX(`TSNON_ET`) AS `END_TIME`
                    FROM TSNON_WORKTIME 
                    WHERE `TSNON_DATE` = '" + DT_TEXT + @"' AND `TSNON_MCIDX`
                    GROUP BY `TSNON_MCIDX`) `BAA`
                ON `BA`.`TS_MC_NM` = `BAA`.`MC_NAME`) `BBA`) `BB`
        ON `AA`.`MC_NO` = `BB`.`TS_MC_NM`
        GROUP BY `AA`.`MC_NO`
        WITH ROLLUP";

                DataSet dsSQL_TIME = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_TIME);
                result.DataGridView1 = new List<SuperResultTranfer>();
                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dsSQL_TIME.Tables["DB_TIME"]));

                string SQL_STOP = @"SELECT `tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN` FROM tsnon_oper_mitor WHERE NOT(`tsnon_oper_mitor_RUNYN` = 'N') AND `tsnon_oper_mitor_MCNM` LIKE 'A8%'";
                DataSet dsSQL_STOP = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_STOP);
                result.DataGridView2 = new List<SuperResultTranfer>();
                result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dsSQL_STOP.Tables["DB_STOP"]));
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
                // Gọi lại logic Dgv_reload (tương tự Buttonfind_Click)
                string DT_TEXT = DateTime.Parse(BaseParameter?.ListSearchString?[0] ?? DateTime.Now.ToString("yyyy-MM-dd")).ToString("yyyy-MM-dd");
                string SQL_TIME = @"SELECT 
                                    '" + DT_TEXT + @"' AS `DATE`,
                                    IFNULL(`AA`.`MC_NO`, '') AS `MC_NO`, 
                                    `BB`.`TS_TIME_ST`,
                                    `BB`.`TS_TIME_END`,
                                    SUM(`AA`.`SUM`) AS `SUM`,
                                    IFNULL(ROUND(AVG(`AA`.`STOP_TIME`/3600), 3), 0) AS `STOP_TIME`,
                                    IFNULL(ROUND(AVG(`AA`.`T_STOP`/3600), 3), 0) AS `T_STOP`,
                                    ROUND(AVG(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600), 6) AS `WORK_TIME`,
                                    ROUND(AVG(((ROUND(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600, 6) 
                                        - (IFNULL(ROUND(`AA`.`STOP_TIME`/3600, 3), 0) - IFNULL(ROUND(`AA`.`T_STOP`/3600, 3), 0))) 
                                        / ROUND(TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) / 3600, 6))), 2) AS `RUN_RAT`,
                                    ROUND(AVG(`AA`.`SUM` / ((TIMESTAMPDIFF(SECOND, `BB`.`TS_TIME_ST`, `BB`.`TS_TIME_END`) - IFNULL(`AA`.`T_STOP`, 0)) / 3600)), 0) AS `UPH`
                                    FROM
                                    (SELECT * FROM (
                                        SELECT `MC_NO`, SUM(`WK_QTY`) AS `SUM` 
                                        FROM TWWKAR 
                                        WHERE 
                                        `CREATE_DTM` > '" + DT_TEXT + @" 06:00:00' AND 
                                        `CREATE_DTM` < DATE_FORMAT(DATE_ADD('" + DT_TEXT + @"', INTERVAL +1 DAY), '%Y-%m-%d 06:00:00')  
                                        GROUP BY `MC_NO`) `A`
                                        LEFT JOIN (
                                        SELECT `TSNON_OPER_MCNM`, 
                                        SUM(TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) AS `STOP_TIME`,
                                        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'L' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
                                        SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'T' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) AS `T_STOP`
                                        FROM TSNON_OPER 
                                        WHERE 
                                        `CREATE_DTM` > '" + DT_TEXT + @" 06:00:00' AND 
                                        `CREATE_DTM` < DATE_FORMAT(DATE_ADD('" + DT_TEXT + @"', INTERVAL +1 DAY), '%Y-%m-%d 06:00:00') 
                                        GROUP BY `TSNON_OPER_MCNM`) `B`
                                        ON `A`.`MC_NO` = `B`.`TSNON_OPER_MCNM`  
                                        WHERE `A`.`MC_NO` LIKE 'A8%') `AA`
                                    LEFT JOIN (
                                        SELECT * FROM (
                                            SELECT  
                                            `BA`.`TS_MC_NM`,
                                            IFNULL(`BAA`.`START_TIME`, `BA`.`TS_TIME_ST`) AS `TS_TIME_ST`,
                                            IFNULL(`BAA`.`END_TIME`, `BA`.`TS_TIME_END`) AS `TS_TIME_END`,
                                            `BA`.`TS_DATE`
                                            FROM (
                                                SELECT `TS_MC_NM`, DATE_FORMAT(MIN(`TS_TIME_ST`), '%Y-%m-%d %T') AS `TS_TIME_ST`, 
                                                IFNULL(MAX(`TS_TIME_END`), '" + DT_TEXT + @" 06:00:00') AS `TS_TIME_END`,
                                                `TS_DATE` FROM TUSER_LOG  
                                                WHERE `TS_DATE` = '" + DT_TEXT + @"'
                                                AND `TS_MC_NM` LIKE 'A8%' 
                                                GROUP BY `TS_MC_NM`) `BA` 
                                                LEFT JOIN (
                                                SELECT (SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TSNON_MCIDX`) AS `MC_NAME`, 
                                                MIN(`TSNON_ST`) AS `START_TIME`, MAX(`TSNON_ET`) AS `END_TIME`
                                                FROM TSNON_WORKTIME 
                                                WHERE `TSNON_DATE` = '" + DT_TEXT + @"' AND `TSNON_MCIDX`
                                                GROUP BY `TSNON_MCIDX`) `BAA`
                                                ON `BA`.`TS_MC_NM` = `BAA`.`MC_NAME`) `BBA`) `BB`
                                    ON `AA`.`MC_NO` = `BB`.`TS_MC_NM`
                                    GROUP BY `AA`.`MC_NO`
                                    WITH ROLLUP";

                DataSet dsSQL_TIME = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_TIME);
                result.DataGridView1 = new List<SuperResultTranfer>();
                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dsSQL_TIME.Tables["DB_TIME"]));
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

