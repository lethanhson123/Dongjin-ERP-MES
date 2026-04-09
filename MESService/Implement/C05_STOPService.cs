

namespace MESService.Implement
{
    public class C05_STOPService : BaseService<torderlist, ItorderlistRepository>
     , IC05_STOPService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IMaintenanceHistoryRepository _maintenanceHistoryRepository; 
        private readonly IToolShopRepository _toolShopRepository; 

        public C05_STOPService(
            ItorderlistRepository torderlistRepository,
            IMaintenanceHistoryRepository maintenanceHistoryRepository, 
            IToolShopRepository toolShopRepository 
        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _maintenanceHistoryRepository = maintenanceHistoryRepository;
            _toolShopRepository = toolShopRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> PageLoad(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var ST_TM = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        var STOP_MC = BaseParameter.ListSearchString[0];
                        var Label5 = BaseParameter.ListSearchString[1];
                        var Label2 = BaseParameter.ListSearchString[2];

                        // CODE CŨ - GIỮ NGUYÊN
                        string sql = @"INSERT INTO `TSNON_OPER` (`TSNON_OPER_MCNM`, `TSNON_OPER_USERNM`, `TSNON_OPER_DATE`, `TSNON_OPER_STIME`, `CREATE_DTM`, `CREATE_USER`, `TSNON_OPER_CODE`, `REMARK`) VALUES ('" + STOP_MC + "', '" + C_USER + "', DATE_FORMAT(NOW(), '%Y-%m-%d'), NOW(), '" + ST_TM + "', '" + C_USER + "', '" + Label5 + "','New MES C05 STOP')";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT  TSNON_OPER.TSNON_OPER_IDX FROM TSNON_OPER  WHERE TSNON_OPER.CREATE_DTM ='" + ST_TM + "'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES ('" + STOP_MC + "', '" + Label2 + "', 'Y','" + Label5 + "')  ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC`= '" + Label2 + "', `tsnon_oper_mitor_RUNYN` = 'Y',`StopCode`='" + Label5 + "'";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `tsnon_oper_mitor`     AUTO_INCREMENT= 1";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        if (Label5 == "M")
                        {
                            sql = @"INSERT INTO  `tsnon_oper_andon` (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + STOP_MC + "', 'MC STOP', 'Y', NOW(), '" + C_USER + "') ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = VALUES(`tsnon_oper_mitor_NOIC`), `tsnon_oper_mitor_RUNYN` = VALUES(`tsnon_oper_mitor_RUNYN`),                            `UPDATE_DTM` = VALUES(`CREATE_DTM`),  `UPDATE_USER` = VALUES(`CREATE_USER`)     ";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"ALTER TABLE     `tsnon_oper_andon`     AUTO_INCREMENT= 1";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"INSERT INTO  `tsnon_oper_andon_LIST` (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + STOP_MC + "', 'MC STOP', 'Y', NOW(), '" + C_USER + "')     ";
                            sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);                       
                            // CODE MỚI: Insert MaintenanceHistory khi máy hư
                            try
                            {
                                // Tìm ToolShop theo Sub_Code = STOP_MC
                                var toolShop = await _toolShopRepository
                                    .GetByCondition(x => x.Sub_Code == STOP_MC && x.Active == true)
                                    .FirstOrDefaultAsync();

                                if (toolShop != null)
                                {
                                    var maintenanceHistory = new MaintenanceHistory
                                    {
                                        ToolShopID = toolShop.ID,
                                        ToolShopSubCode = toolShop.Sub_Code,
                                        MaintenanceType = "M",
                                        StartDate = DateTime.Now,
                                        EndDate = null,
                                        DurationMinutes = null,
                                        Cost = null,
                                        CreateDate = DateTime.Now,
                                        CreateUser = C_USER,
                                        IsActive = true
                                    };

                                    await _maintenanceHistoryRepository.AddAsync(maintenanceHistory);
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error inserting MaintenanceHistory: {ex.Message}");
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
        public virtual async Task<BaseResult> SW_TIME(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var ST_TM = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        var USER_MC = BaseParameter.ListSearchString[0];
                        var USER_ORIDX = BaseParameter.ListSearchString[1];
                        var Label5 = BaseParameter.ListSearchString[2];

                        string sql = @"INSERT INTO  `TORDERLIST_WTIME`  (`USER_ID`, `USER_MC`, `ORDER_IDX`, `S_TIME`, `CREATE_DTM`, `CREATE_USER`, `MENU_TEXT`) VALUES                 ('" + USER_ID + "', '" + USER_MC + "', '" + USER_ORIDX + "', NOW(), '" + ST_TM + "', '" + USER_ID + "', '" + Label5 + "')";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT  TORDERLIST_WTIME.TOWT_INDX    FROM   TORDERLIST_WTIME  WHERE TORDERLIST_WTIME.CREATE_DTM ='" + ST_TM + "'";
                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_WT = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_WT.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> EW_TIME(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var STOPW_ORING_IDX = BaseParameter.SearchString;
                    string sql = @"UPDATE  `TORDERLIST_WTIME`   SET `E_TIME`= NOW()    WHERE  `TOWT_INDX` = '" + STOPW_ORING_IDX + "'";
                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> C05_STOP_FormClosed(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.ListSearchString != null)
                    {
                        var RunTime = DateTime.Now;
                        var StartTime = DateTime.Parse(BaseParameter.ListSearchString[0]);
                        var Label6 = BaseParameter.ListSearchString[1];
                        var STOP_MC = BaseParameter.ListSearchString[2];
                        var Label5 = BaseParameter.ListSearchString.Count > 3 ? BaseParameter.ListSearchString[3] : ""; 

                        // CODE CŨ - GIỮ NGUYÊN
                        string sql = @"UPDATE `TSNON_OPER` SET `TSNON_OPER_ETIME`= NOW(), `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + C_USER + "', `TSNON_OPER_TIME` = TIME_TO_SEC(TIMEDIFF(`TSNON_OPER_ETIME`, `TSNON_OPER_STIME`)) WHERE  `TSNON_OPER_IDX`= '" + Label6 + "' and `TSNON_OPER_ETIME` is null;";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        // CODE MỚI: Auto-close MaintenanceHistory khi restart máy
                        if (Label5 == "M")
                        {
                            try
                            {
                                var openMaintenance = await _maintenanceHistoryRepository
                                    .GetByCondition(x =>
                                        x.ToolShopSubCode == STOP_MC &&
                                        x.MaintenanceType == "M" &&
                                        x.EndDate == null &&
                                        x.IsActive == true &&
                                        x.StartDate.HasValue &&
                                        x.StartDate.Value.Date == DateTime.Today)
                                    .OrderByDescending(x => x.StartDate)
                                    .FirstOrDefaultAsync();

                                if (openMaintenance != null)
                                {
                                    openMaintenance.EndDate = DateTime.Now;
                                    if (openMaintenance.StartDate.HasValue)
                                    {
                                        openMaintenance.DurationMinutes = Convert.ToDecimal(
                                            (openMaintenance.EndDate.Value - openMaintenance.StartDate.Value).TotalMinutes
                                        );
                                    }

                                    // Nếu user chưa nhập chi tiết, thêm ghi chú cảnh báo
                                    if (string.IsNullOrEmpty(openMaintenance.CurrentStatus))
                                    {
                                        openMaintenance.Notes = string.IsNullOrEmpty(openMaintenance.Notes)
                                            ? "May khoi dong lai ma chua cap nhat chi tiet bao tri"
                                            : openMaintenance.Notes + " | May khoi dong lai ma chua cap nhat chi tiet bao tri";
                                    }

                                    await _maintenanceHistoryRepository.UpdateAsync(openMaintenance);

                                    System.Diagnostics.Debug.WriteLine($"Auto-closed MaintenanceHistory ID={openMaintenance.ID} for machine {STOP_MC}");
                                }
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error closing MaintenanceHistory: {ex.Message}");
                            }
                        }

                        sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES ('" + STOP_MC + "', '-----', 'N','-')  ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC`= '-----', `tsnon_oper_mitor_RUNYN` = 'N',`StopCode`='-'";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"ALTER TABLE     `tsnon_oper_mitor`     AUTO_INCREMENT= 1";
                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> OPER_TIME(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var STOP_MC = BaseParameter.SearchString;
                    string sql = @"SELECT SUM(`TSNON_OPER_TIME`) AS `SUM_TIME` FROM TSNON_OPER WHERE `TSNON_OPER_MCNM` = '" + STOP_MC + "' AND NOT(`TSNON_OPER_CODE`='S')";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
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
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    var STOP_MC = BaseParameter.SearchString;

                    string sql = @"INSERT INTO  `tsnon_oper_andon` (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + STOP_MC + "', 'MC STOP', 'I', NOW(), '" + C_USER + "') ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = VALUES(`tsnon_oper_mitor_NOIC`), `tsnon_oper_mitor_RUNYN` = VALUES(`tsnon_oper_mitor_RUNYN`),                            `UPDATE_DTM` = VALUES(`CREATE_DTM`),  `UPDATE_USER` = VALUES(`CREATE_USER`)     ";
                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    sql = @"ALTER TABLE     `tsnon_oper_andon`     AUTO_INCREMENT= 1";
                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    sql = @"INSERT INTO  `tsnon_oper_andon_LIST` (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + STOP_MC + "', 'MC STOP', 'I', NOW(), '" + C_USER + "')     ";
                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    try
                    {
                        var openMaintenance = await _maintenanceHistoryRepository
                            .GetByCondition(x =>
                                x.ToolShopSubCode == STOP_MC &&
                                x.MaintenanceType == "M" &&
                                x.EndDate == null &&
                                x.IsActive == true &&
                                x.StartDate.HasValue &&
                                x.StartDate.Value.Date == DateTime.Today)
                            .OrderByDescending(x => x.StartDate)
                            .FirstOrDefaultAsync();

                        if (openMaintenance != null && string.IsNullOrEmpty(openMaintenance.MaintenedBy))
                        {
                            openMaintenance.MaintenedBy = "Đang kiểm tra";
                            await _maintenanceHistoryRepository.UpdateAsync(openMaintenance);
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error updating MaintenanceHistory: {ex.Message}");
                    }

                    result.Success = true;
                }
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
                if (BaseParameter != null)
                {
                    var C_USER = BaseParameter.USER_ID;
                    var STOP_MC = BaseParameter.SearchString;

                    string sql = @"INSERT INTO  `tsnon_oper_andon` (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + STOP_MC + "', 'MC STOP', 'N', NOW(), '" + C_USER + "') ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = VALUES(`tsnon_oper_mitor_NOIC`), `tsnon_oper_mitor_RUNYN` = VALUES(`tsnon_oper_mitor_RUNYN`),                            `UPDATE_DTM` = VALUES(`CREATE_DTM`),  `UPDATE_USER` = VALUES(`CREATE_USER`)     ";
                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    sql = @"ALTER TABLE     `tsnon_oper_andon`     AUTO_INCREMENT= 1";
                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    sql = @"INSERT INTO  `tsnon_oper_andon_LIST` (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + STOP_MC + "', 'MC STOP', 'N', NOW(), '" + C_USER + "')     ";
                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> SaveMaintenanceHistory(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string STOP_MC = BaseParameter.STOP_MC;
                    string currentStatus = BaseParameter.CurrentStatus;
                    string solution = BaseParameter.Solution;
                    string sparePartsUsed = BaseParameter.SparePartsUsed;
                    string maintenedBy = BaseParameter.MaintenedBy;
                    string notes = BaseParameter.Notes;
                    string userId = BaseParameter.USER_ID;

                    // Tìm ToolShop
                    var toolShop = await _toolShopRepository
                        .GetByCondition(x => x.Sub_Code == STOP_MC && x.Active == true)
                        .FirstOrDefaultAsync();

                    if (toolShop == null)
                    {
                        result.Error = "Khong tim thay ToolShop voi Sub_Code: " + STOP_MC;
                        return result;
                    }

                    // Tìm MaintenanceHistory đang mở
                    var maintenanceHistory = await _maintenanceHistoryRepository
                        .GetByCondition(x =>
                            x.ToolShopID == toolShop.ID &&
                            x.ToolShopSubCode == STOP_MC &&
                            x.MaintenanceType == "M" &&
                            x.IsActive == true &&
                            x.EndDate == null &&
                            x.StartDate.HasValue &&
                            x.StartDate.Value.Date == DateTime.Today)
                        .OrderByDescending(x => x.StartDate)
                        .FirstOrDefaultAsync();

                    if (maintenanceHistory == null)
                    {
                        result.Error = "Khong tim thay ban ghi MaintenanceHistory dang mo de cap nhat";
                        return result;
                    }

                    // Update MaintenanceHistory với chi tiết
                    maintenanceHistory.CurrentStatus = currentStatus;
                    maintenanceHistory.Solution = solution;
                    maintenanceHistory.SparePartsUsed = sparePartsUsed;
                    maintenanceHistory.MaintenedBy = maintenedBy;

                    if (!string.IsNullOrEmpty(notes))
                    {
                        maintenanceHistory.Notes = string.IsNullOrEmpty(maintenanceHistory.Notes)
                            ? notes
                            : maintenanceHistory.Notes + " | " + notes;
                    }

                    await _maintenanceHistoryRepository.UpdateAsync(maintenanceHistory);

                    result.Success = true;
                    result.Message = "Luu chi tiet bao tri thanh cong.";
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

