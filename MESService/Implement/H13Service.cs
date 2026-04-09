namespace MESService.Implement
{
    public class H13Service : BaseService<torderlist, ItorderlistRepository>
    , IH13Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public H13Service(ItorderlistRepository torderlistRepository)
            : base(torderlistRepository)
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
                          AND `CREATE_DTM` > DATE_FORMAT(NOW(), '%Y-%m-%d 06:00:00') 
                          AND (`TSNON_OPER_MCNM` LIKE 'ZZ1%' OR `TSNON_OPER_MCNM` LIKE 'ZS1%') 
                          GROUP BY `TSNON_OPER_MCNM`";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlCheck);

                // Cập nhật trạng thái các máy nếu trạng thái không khớp với dữ liệu trong TSNON_OPER
                string sqlUpdate = @"UPDATE tsnon_oper_mitor 
                             SET `tsnon_oper_mitor_RUNYN` = 'N' 
                             WHERE NOT(`tsnon_oper_mitor_MCNM` = 
                             (SELECT TSNON_OPER.TSNON_OPER_MCNM 
                              FROM TSNON_OPER 
                              WHERE TSNON_OPER.TSNON_OPER_ETIME IS NULL 
                              AND TSNON_OPER.CREATE_DTM > DATE_FORMAT(NOW(), '%Y-%m-%d 06:00:00') 
                              GROUP BY TSNON_OPER.TSNON_OPER_MCNM))";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sqlUpdate);

                // Tự động cập nhật trạng thái các máy không có người vận hành
                await NonWorkerAuto(new BaseParameter());

                // Tải dữ liệu sản xuất và trạng thái máy
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
                // Câu SQL lấy dữ liệu sản xuất từ các máy TWIST và WELDING
                string SQL_TIME = @"SELECT 
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
FROM TWWKAR_SPST 
WHERE 
`CREATE_DTM` > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T') AND 
`CREATE_DTM` < DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL +1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T')  
GROUP BY  `MC_NO`) `A`
LEFT JOIN       
(SELECT `TSNON_OPER_MCNM`, 
SUM(TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) AS `STOP_TIME`,
SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'L' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'T' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) AS `T_STOP`
FROM TSNON_OPER 
WHERE 
`CREATE_DTM` > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T') AND 
`CREATE_DTM` < DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL +1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T')  
GROUP BY `TSNON_OPER_MCNM` ) `B`
ON `A`.`MC_NO` = `B`.`TSNON_OPER_MCNM`  
WHERE `A`.`MC_NO` LIKE 'ZZ1%' OR `A`.`MC_NO` LIKE 'ZS1%') `AA`

LEFT JOIN
(SELECT `TS_MC_NM`, DATE_FORMAT(MIN(`TS_TIME_ST`), '%Y-%m-%d %T') AS `TS_TIME_ST`, 
DATE_FORMAT(NOW(), '%Y-%m-%d %T') AS `TS_TIME_END`,
`TS_DATE` FROM TUSER_LOG  
WHERE  `TS_DATE` = DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d')
AND (`TS_MC_NM` LIKE 'ZZ1%' OR `TS_MC_NM` LIKE 'ZS1%')
GROUP BY  `TS_MC_NM`) `BB`
ON 
`AA`.`MC_NO` = `BB`.`TS_MC_NM`
GROUP BY `AA`.`MC_NO`
WITH ROLLUP";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_TIME);

                // Lưu dữ liệu sản xuất vào result
                result.DataGridView1 = new List<SuperResultTranfer>();
                if (ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }

                // Lấy dữ liệu trạng thái dừng máy
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
                // Lấy thông tin các máy đang dừng
                string SQL_STOP = @"SELECT `tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`   
                            FROM tsnon_oper_mitor 
                            WHERE NOT(`tsnon_oper_mitor_RUNYN` = 'N')  
                            AND (`tsnon_oper_mitor_MCNM` LIKE 'ZZ1%' OR `tsnon_oper_mitor_MCNM` LIKE 'ZS1%')";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_STOP);

                // Lưu thông tin dừng máy vào result
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
                // Lấy dữ liệu chi tiết cho modal hiển thị
                string SQL_DETAIL = @"SELECT 
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
FROM TWWKAR_SPST 
WHERE 
`CREATE_DTM` > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T') AND 
`CREATE_DTM` < DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL +1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T')  
GROUP BY  `MC_NO`) `A`
LEFT JOIN       
(SELECT `TSNON_OPER_MCNM`, 
SUM(TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) AS `STOP_TIME`,
SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'L' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) +
SUM(IFNULL(CASE TSNON_OPER.TSNON_OPER_CODE WHEN 'T' THEN (TIMESTAMPDIFF(SECOND, `TSNON_OPER_STIME`, IFNULL(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))) END, 0)) AS `T_STOP`
FROM TSNON_OPER 
WHERE 
`CREATE_DTM` > DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T') AND 
`CREATE_DTM` < DATE_FORMAT(CONCAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL +1 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d')), ' 06:00:00'), '%Y-%m-%d %T')  
GROUP BY `TSNON_OPER_MCNM` ) `B`
ON `A`.`MC_NO` = `B`.`TSNON_OPER_MCNM`  
WHERE `A`.`MC_NO` LIKE 'ZZ1%' OR `A`.`MC_NO` LIKE 'ZS1%') `AA`

LEFT JOIN
(SELECT `TS_MC_NM`, DATE_FORMAT(MIN(`TS_TIME_ST`), '%Y-%m-%d %T') AS `TS_TIME_ST`, 
DATE_FORMAT(NOW(), '%Y-%m-%d %T') AS `TS_TIME_END`,
`TS_DATE` FROM TUSER_LOG  
WHERE  `TS_DATE` = DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d')
AND (`TS_MC_NM` LIKE 'ZZ1%' OR `TS_MC_NM` LIKE 'ZS1%')
GROUP BY  `TS_MC_NM`) `BB`
ON 
`AA`.`MC_NO` = `BB`.`TS_MC_NM`
GROUP BY `AA`.`MC_NO`
WITH ROLLUP";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, SQL_DETAIL);

                // Lưu dữ liệu chi tiết vào result
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
                // Lấy thông tin máy và người dùng từ tham số
                string mcName = BaseParameter.ListSearchString[0];
                string currentUser = BaseParameter.ListSearchString.Count > 1 ? BaseParameter.ListSearchString[1] : "SYSTEM";

                // Cập nhật trạng thái máy thành "Run"
                string sql1 = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) 
                        VALUES (@mcName, '-----', 'N','-') 
                        ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = '-----', `tsnon_oper_mitor_RUNYN` = 'N',`StopCode`='-'";

                // Ghi log người vận hành (END)
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
                // Lấy thông tin máy và người dùng từ tham số
                string mcName = BaseParameter.ListSearchString[0];
                string currentUser = BaseParameter.ListSearchString.Count > 1 ? BaseParameter.ListSearchString[1] : "SYSTEM";

                // Cập nhật trạng thái máy thành "Stop - No Worker"
                string sql1 = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) 
                        VALUES (@mcName, 'No Worker', 'W','W') 
                        ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = 'No Worker', `tsnon_oper_mitor_RUNYN` = 'W',`StopCode`='W'";

                // Ghi log người vận hành (START)
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
                // Tự động đóng các trạng thái "No Worker" vào cuối ngày
                string sql = @"INSERT INTO `TSNON_OPER_WORKER` (`TSNON_OPER_CODE`, `TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_COL`, `TSNON_OPER_TIME`, `CREATE_DTM`, `CREATE_USER`)
                       SELECT 
                           'NW', `TSNON_OPER_MCNM`, 'MES', DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY),'%Y-%m-%d') AS `DT`, 'E', CONCAT(DATE_FORMAT(NOW(),'%Y-%m-%d'), ' 05:59:59'), NOW(), 'MES'
                       FROM (SELECT `TSNON_OPER_MCNM`,
                           COUNT(case when `TSNON_OPER_COL`='S' then `TSNON_OPER_COL` END) AS `S`,
                           COUNT(case when `TSNON_OPER_COL`='E' then `TSNON_OPER_COL` END) AS `E`,
                           COUNT(case when `TSNON_OPER_COL`='S' then `TSNON_OPER_COL` END) - COUNT(case when `TSNON_OPER_COL`='E' then `TSNON_OPER_COL` END) AS `SUM`
                           FROM TSNON_OPER_WORKER
                           WHERE TSNON_OPER_WORKER.`TSNON_OPER_TIME` >= CONCAT(DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY),'%Y-%m-%d'), ' 06:00:00') 
                           AND TSNON_OPER_WORKER.`TSNON_OPER_TIME` < CONCAT(DATE_FORMAT(NOW(),'%Y-%m-%d'), ' 06:00:00')
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

        // Các phương thức CRUD tiêu chuẩn không sử dụng cho ứng dụng này
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