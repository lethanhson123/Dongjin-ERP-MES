using HelperMySQL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class CleanupDownloadService : BackgroundService
{
    private readonly string _downloadPath;
    private readonly ILogger<CleanupDownloadService> _logger;

    public CleanupDownloadService(ILogger<CleanupDownloadService> logger, IWebHostEnvironment env)
    {
        _logger = logger;
        _downloadPath = Path.Combine(env.WebRootPath, "Download");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("CleanupDownloadService đã khởi động.");

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRunTime = DateTime.Today.AddHours(6); // 06:00 hôm nay

            if (now > nextRunTime)
                nextRunTime = nextRunTime.AddDays(1);      // nếu đã qua 6h thì chạy ngày mai

            var delay = nextRunTime - now;

            _logger.LogInformation($"Chờ đến {nextRunTime} để chạy job.");

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch { break; }

            // ============================================================
            // 1. Update Etime No Worker
            // ============================================================
            try
            {
                string sql = @"UPDATE `TSNON_OPER` 
                           SET `TSNON_OPER_ETIME` = NOW(),
                               `UPDATE_DTM` = NOW(),
                               `UPDATE_USER` = 'MES Auto',
                               `TSNON_OPER_TIME` = TIME_TO_SEC(TIMEDIFF(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`))
                           WHERE `TSNON_OPER_CODE`= 'N' AND `TSNON_OPER_ETIME` IS NULL";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                _logger.LogInformation("Đã cập nhật ETIME TSNON_OPER.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi cập nhật ETIME TSNON_OPER.");
            }

            // chạy lệnh đóng đơn cho đơn LP hàng ngày
            try
            {
                string sql = @"UPDATE TORDERLIST_LP SET `CONDITION`='Close' WHERE DATE_FORMAT(`CREATE_DTM`, '%Y-%m-%d') < DATE_ADD(NOW(), INTERVAL -20 DAY) AND NOT (TORDERLIST_LP.`CONDITION` = 'Complete')";
                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                _logger.LogInformation($"Đã đong đơn TORDERLIST_LP quá 20 ngày");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đóng đơn TORDERLIST_LP quá 20 ngày .");
            }

            //chạy lệnh đóng đơn Cutting
            try
            {
                string sql = @"UPDATE TORDERLIST SET `CONDITION`='Close',  `UPDATE_DTM` = NOW(),  `UPDATE_USER` = 'MES'  WHERE `DT` < DATE_ADD(NOW(), INTERVAL -11 DAY) AND NOT (TORDERLIST.`CONDITION` = 'Complete')  AND NOT (`CONDITION` = 'Close') ";
                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                _logger.LogInformation($"Đã đóng đơn Cutting (TORDERLIST) quá 11 ngày");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đóng đơn Cutting (TORDERLIST) quá 11 ngày");
            }

            //chạy lệnh đóng đơn tài khoản không đang nhập quá 15 ngày
            try
            {
                string sql = @"
                                UPDATE TSUSER u
                                             SET DESC_YN = 'N'
                                            WHERE u.DESC_YN <> 'N'
                                            AND NOT EXISTS (
                                                SELECT 1
                                                FROM TUSER_LOG l
                                                WHERE l.TS_DATE >= CURDATE() - INTERVAL 15 DAY
                                                  AND u.USER_ID = l.TS_USER
                                            )
                                            AND NOT EXISTS (
                                                SELECT 1
                                                FROM TUSER_LOG l
                                                WHERE l.TS_DATE >= CURDATE() - INTERVAL 15 DAY
                                                  AND u.USER_IDX = l.TS_USER
                                            );
                                ";
                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                _logger.LogInformation($"Block user not login after 15 day");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error Block user not login after 15 day");
            }

            //chạy lệnh đóng đơn SPST
            try
            {
                string sql = @"UPDATE   TORDERLIST_SPST    SET `CONDITION` = 'Close', `UPDATE_DTM` = NOW(),  `UPDATE_USER` = 'MES'   WHERE `PO_DT` < DATE_ADD(NOW(), INTERVAL -11 DAY) AND NOT (`CONDITION` = 'Complete') AND NOT (`CONDITION` = 'Close') ";
                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                sql = @"UPDATE   TORDERLIST_SPST   SET `CONDITION` = 'Close', `DSCN_YN`='N', `UPDATE_DTM` = NOW(),  `UPDATE_USER` = 'MES'     WHERE    `PO_QTY`  <= 0  AND NOT (`CONDITION` = 'Complete') AND NOT (`CONDITION` = 'Close') ";
                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);


                _logger.LogInformation($"Đã đóng đơn TORDERLIST_SPST quá 11 ngày");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đóng đơn SPST (TORDERLIST_SPST) quá 11 ngày");
            }

            //thục hiện xóa file tạm trên server
            try
            {
                if (Directory.Exists(_downloadPath))
                {
                    var files = Directory.GetFiles(_downloadPath, "*", SearchOption.AllDirectories);
                    var dirs = Directory.GetDirectories(_downloadPath, "*", SearchOption.AllDirectories);

                    foreach (var file in files)
                    {
                        try
                        {
                            File.Delete(file);
                            _logger.LogInformation($"Đã xóa file: {file}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Không thể xóa file: {file}");
                        }
                    }

                    foreach (var dir in dirs)
                    {
                        try
                        {
                            Directory.Delete(dir, true);
                            _logger.LogInformation($"Đã xóa thư mục: {dir}");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, $"Không thể xóa thư mục: {dir}");
                        }
                    }

                    _logger.LogInformation("Dọn dẹp thư mục Download hoàn tất.");
                }
                else
                {
                    _logger.LogWarning($"Thư mục không tồn tại: {_downloadPath}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi dọn dẹp thư mục Download.");
            }

        }

        _logger.LogInformation("CleanupDownloadService đã dừng.");
    }
}
