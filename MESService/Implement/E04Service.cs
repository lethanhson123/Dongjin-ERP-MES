namespace MESService.Implement
{
    public class E04Service : BaseService<MaintenanceHistory, IMaintenanceHistoryRepository>, IE04Service
    {
        private readonly IMaintenanceHistoryRepository _maintenanceHistoryRepository;

        public E04Service(IMaintenanceHistoryRepository maintenanceHistoryRepository)
            : base(maintenanceHistoryRepository)
        {
            _maintenanceHistoryRepository = maintenanceHistoryRepository;
        }

        public override void Initialization(MaintenanceHistory model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                result.MaintenanceHistoryList = await _maintenanceHistoryRepository
                    .GetByCondition(x => x.IsActive == true)
                    .OrderByDescending(x => x.StartDate)
                    .ToListAsync();
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
                var query = _maintenanceHistoryRepository.GetByCondition(x => x.IsActive == true);
                if (!string.IsNullOrEmpty(BaseParameter.ToolShopSubCode))
                {
                    query = query.Where(x => x.ToolShopSubCode.Contains(BaseParameter.ToolShopSubCode));
                }
                if (!string.IsNullOrEmpty(BaseParameter.FilterFromDate))
                {
                    if (DateTime.TryParse(BaseParameter.FilterFromDate, out DateTime fromDate))
                    {
                        query = query.Where(x =>
                            x.StartDate.HasValue &&
                            x.StartDate.Value.Date >= fromDate.Date
                        );
                    }
                }

                if (!string.IsNullOrEmpty(BaseParameter.FilterToDate))
                {
                    if (DateTime.TryParse(BaseParameter.FilterToDate, out DateTime toDate))
                    {
                        query = query.Where(x =>
                            x.StartDate.HasValue &&
                            x.StartDate.Value.Date <= toDate.Date
                        );
                    }
                }
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString;
                    query = query.Where(x =>
                        x.ToolShopSubCode.Contains(search) ||
                        x.MaintenedBy.Contains(search) ||
                        x.Solution.Contains(search) ||
                        x.CurrentStatus.Contains(search)
                    );
                }

                result.MaintenanceHistoryList = await query
                    .OrderByDescending(x => x.StartDate)
                    .ToListAsync();

                result.Success = true;
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
                var maintenance = await _maintenanceHistoryRepository
                    .GetByCondition(x => x.ID == BaseParameter.ID)
                    .FirstOrDefaultAsync();

                if (maintenance == null)
                {
                    result.Error = "Không tìm thấy dữ liệu.";
                    return result;
                }

                // Chỉ cho phép sửa các trường này
                maintenance.CurrentStatus = BaseParameter.CurrentStatus;
                maintenance.Solution = BaseParameter.Solution;
                maintenance.SparePartsUsed = BaseParameter.SparePartsUsed;
                maintenance.MaintenedBy = BaseParameter.MaintenedBy;
                maintenance.Notes = BaseParameter.Notes;
                maintenance.UpdateUser = BaseParameter.USER_ID;  
                maintenance.UpdateDate = DateTime.Now;          

                await _maintenanceHistoryRepository.UpdateAsync(maintenance);
                result.Success = true;
                result.Message = "Cập nhật thành công.";
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
                var maintenance = await _maintenanceHistoryRepository
                    .GetByCondition(x => x.ID == BaseParameter.ID)
                    .FirstOrDefaultAsync();

                if (maintenance == null)
                {
                    result.Error = "Không tìm thấy dữ liệu.";
                    return result;
                }

                // Xóa mềm
                maintenance.IsActive = false;
                await _maintenanceHistoryRepository.UpdateAsync(maintenance);
                result.Success = true;
                result.Message = "Xóa thành công.";
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
                var maintenance = BaseParameter.MaintenanceHistory;
                maintenance.MaintenanceType = "M";
                maintenance.IsActive = true;
                if (maintenance.StartDate == null || maintenance.StartDate == DateTime.MinValue)
                {
                    maintenance.StartDate = DateTime.Now;
                }

                if (maintenance.EndDate == null || maintenance.EndDate == DateTime.MinValue)
                {
                    maintenance.EndDate = DateTime.Now;
                }
                maintenance.CreateDate = DateTime.Now;
                maintenance.CreateUser = BaseParameter.USER_ID;
                maintenance.UpdateDate = DateTime.Now;
                maintenance.UpdateUser = BaseParameter.USER_ID;

                Initialization(maintenance);

                await _maintenanceHistoryRepository.AddAsync(maintenance);

                result.MaintenanceHistoryList = new List<MaintenanceHistory> { maintenance };
                result.Success = true;
                result.Message = "Thêm mới thành công.";
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