namespace MESService.Implement
{
    public class H08Service : BaseService<ToolShop, IToolShopRepository>, IH08Service
    {
        private readonly IToolShopRepository _toolShopRepository;
        private readonly Itsnon_operRepository _tsnonoperRepository;
        private readonly IMaintenanceHistoryRepository _maintenanceHistoryRepository;

        public H08Service(
            IToolShopRepository toolShopRepository,
            Itsnon_operRepository tsnonoperRepository,
            IMaintenanceHistoryRepository maintenanceHistoryRepository
        ) : base(toolShopRepository)
        {
            _toolShopRepository = toolShopRepository;
            _tsnonoperRepository = tsnonoperRepository;
            _maintenanceHistoryRepository = maintenanceHistoryRepository;
        }

        public override void Initialization(ToolShop model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                result = await GetMachineStatus();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetMachineStatus()
        {
            BaseResult result = new BaseResult();
            try
            {
                var today = DateTime.Now.Date;

                var allMachines = await _toolShopRepository
                    .GetByCondition(x => x.Active == true
                        && (x.ProductionLine == "CUTTING"
                            || x.ProductionLine == "CRIMPING"
                            || x.ProductionLine == "WELDING"
                            || x.ProductionLine == "TWIST"))
                    .OrderBy(x => x.ProductionLine)
                    .ThenBy(x => x.Sub_Code)
                    .ToListAsync();

                var openDowntimes = await _tsnonoperRepository
                    .GetByCondition(x => x.TSNON_OPER_ETIME == null
                        && x.TSNON_OPER_CODE == "M"
                        && x.TSNON_OPER_STIME.HasValue
                        && x.TSNON_OPER_STIME.Value.Date == today)
                    .ToListAsync();

                var maintenances = await _maintenanceHistoryRepository
                    .GetByCondition(x => x.IsActive == true
                        && x.MaintenanceType == "M"
                        && x.EndDate == null
                        && x.StartDate.HasValue
                        && x.StartDate.Value.Date == today)
                    .ToListAsync();

                var machineStatusList = new List<dynamic>();

                foreach (var machine in allMachines)
                {
                    var downtime = openDowntimes.FirstOrDefault(x => x.TSNON_OPER_MCNM == machine.Sub_Code);

                    int status = 0;

                    if (downtime != null)
                    {
                        var maintenance = maintenances.FirstOrDefault(x =>x.ToolShopSubCode == machine.Sub_Code 
                        && !string.IsNullOrEmpty(x.MaintenedBy) 
                        && x.StartDate.HasValue
                        && Math.Abs((x.StartDate.Value - downtime.TSNON_OPER_STIME.Value).TotalSeconds) <= 300);

                        if (maintenance != null) 
                        {
                            status = 1;
                        }
                        else
                        {
                            status = 2;
                        }
                    }

                    int downtimeSeconds = 0;
                    if (downtime != null && downtime.TSNON_OPER_STIME.HasValue)
                    {
                        downtimeSeconds = (int)DateTime.Now.Subtract(downtime.TSNON_OPER_STIME.Value).TotalSeconds;
                    }

                    var currentMaintenance = maintenances.FirstOrDefault(x =>
                        x.ToolShopSubCode == machine.Sub_Code
                        && x.EndDate == null);

                    machineStatusList.Add(new
                    {
                        machine.ID,
                        machine.Code,
                        machine.Sub_Code,
                        machine.NameVN,
                        machine.ProductionLine,
                        Status = status,
                        DowntimeSeconds = downtimeSeconds,
                        DowntimeID = downtime?.TSNON_OPER_IDX,
                        MaintenedBy = currentMaintenance?.MaintenedBy
                    });
                }

                result.MachineStatusList = machineStatusList.Cast<object>().ToList();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetMaintenanceDetail(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var subCode = BaseParameter.Sub_Code;
                var machine = await _toolShopRepository
                    .GetByCondition(x => x.Sub_Code == subCode && x.Active == true)
                    .FirstOrDefaultAsync();
                var maintenance = await _maintenanceHistoryRepository
                    .GetByCondition(x => x.ToolShopSubCode == subCode
                        && x.EndDate == null
                        && x.IsActive == true)
                    .OrderByDescending(x => x.StartDate)
                    .FirstOrDefaultAsync();
                int minutesWorked = 0;
                if (maintenance.StartDate.HasValue)
                {
                    minutesWorked = (int)DateTime.Now.Subtract(maintenance.StartDate.Value).TotalMinutes;
                }

                result.MaintenanceDetail = new
                {
                    // Machine Info
                    Sub_Code = machine.Sub_Code,
                    NameVN = machine.NameVN,
                    ProductionLine = machine.ProductionLine,

                    // Maintenance Info
                    MaintenedBy = maintenance.MaintenedBy ?? "Chưa rõ",
                    StartDate = maintenance.StartDate,
                    CurrentStatus = maintenance.CurrentStatus ?? "Đang xử lý",
                    Reason = maintenance.Reason ?? "Đang kiểm tra",
                    Solution = maintenance.Solution ?? "Đang thực hiện",
                    MinutesWorked = minutesWorked
                };

                result.Success = true;
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