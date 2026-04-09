namespace MESService.Implement
{
    public class P04Service : BaseService<TaskTimeFA, ITaskTimeFARepository>, IP04Service
    {
        private readonly ITaskTimeFARepository _taskTimeFARepository;
        private readonly ILineListRepository _lineListRepository;

        public P04Service(
            ITaskTimeFARepository taskTimeFARepository,
            ILineListRepository lineListRepository
        ) : base(taskTimeFARepository)
        {
            _taskTimeFARepository = taskTimeFARepository;
            _lineListRepository = lineListRepository;
        }

        public override void Initialization(TaskTimeFA model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                result.LineList = await _lineListRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderBy(x => x.LineName)
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
                var query = _taskTimeFARepository.GetByCondition(x => x.Active == true);
                if (BaseParameter.ID.HasValue && BaseParameter.ID.Value > 0)
                {
                    query = query.Where(x => x.LineID == BaseParameter.ID.Value);
                }
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString;
                    query = query.Where(x =>
                        x.PartNo.Contains(search) ||
                        x.ECN.Contains(search)
                    );
                }
                result.TaskTimeFAList = await query
                    .OrderByDescending(x => x.ID)
                    .ToListAsync();

                result.LineList = await _lineListRepository
                    .GetByCondition(x => x.Active == true)
                    .ToListAsync();

                result.Success = true;
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
                var taskTime = BaseParameter.TaskTimeFA;
                taskTime.Active = true;
                taskTime.CreateDate = DateTime.Now;
                taskTime.CreateUserName = BaseParameter.USER_IDX;

                await _taskTimeFARepository.AddAsync(taskTime);

                result.TaskTimeFAList = new List<TaskTimeFA> { taskTime };
                result.Success = true;
                result.Message = "Thêm mới thành công.";
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
                var existingTaskTime = await _taskTimeFARepository
                    .GetByCondition(x => x.ID == BaseParameter.TaskTimeFA.ID)
                    .FirstOrDefaultAsync();

                if (existingTaskTime == null)
                {
                    result.Error = "Không tìm thấy Task Time.";
                    return result;
                }

                existingTaskTime.LineID = BaseParameter.TaskTimeFA.LineID;
                existingTaskTime.PartNo = BaseParameter.TaskTimeFA.PartNo;
                existingTaskTime.ECN = BaseParameter.TaskTimeFA.ECN;
                existingTaskTime.TaskTimeIE = BaseParameter.TaskTimeFA.TaskTimeIE;
                existingTaskTime.TaskTimeIE2 = BaseParameter.TaskTimeFA.TaskTimeIE2;  // ✅ Thêm
                existingTaskTime.TaskTimeIE3 = BaseParameter.TaskTimeFA.TaskTimeIE3;  // ✅ Thêm
                existingTaskTime.TaskTimeIE4 = BaseParameter.TaskTimeFA.TaskTimeIE4;  // ✅ Thêm
                existingTaskTime.TaskTimeCus = BaseParameter.TaskTimeFA.TaskTimeCus;
                existingTaskTime.UpdateDate = DateTime.Now;
                existingTaskTime.UpdateUserName = BaseParameter.USER_IDX;

                await _taskTimeFARepository.UpdateAsync(existingTaskTime);

                result.TaskTimeFAList = new List<TaskTimeFA> { existingTaskTime };
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
                var taskTime = await _taskTimeFARepository
                    .GetByCondition(x => x.ID == BaseParameter.ID)
                    .FirstOrDefaultAsync();

                if (taskTime == null)
                {
                    result.Error = "Không tìm thấy Task Time.";
                    return result;
                }

                taskTime.Active = false;
                taskTime.UpdateDate = DateTime.Now;
                taskTime.UpdateUserName = BaseParameter.USER_IDX;

                await _taskTimeFARepository.UpdateAsync(taskTime);

                result.TaskTimeFAList = await _taskTimeFARepository
                    .GetByCondition(x => x.Active == true)
                    .OrderByDescending(x => x.ID)
                    .ToListAsync();

                result.Success = true;
                result.Message = "Xóa thành công.";
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
                int successCount = 0;
                int skipCount = 0;
                int updateCount = 0;

                foreach (var item in BaseParameter.TaskTimeFAList)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.PartNo) || item.LineID == null)
                        {
                            skipCount++;
                            continue;
                        }

                        var existing = await _taskTimeFARepository
                            .GetByCondition(x =>
                                x.LineID == item.LineID &&
                                x.PartNo == item.PartNo &&
                                x.ECN == item.ECN &&
                                x.Active == true)
                            .FirstOrDefaultAsync();

                        if (existing != null)
                        {
                            existing.TaskTimeIE = item.TaskTimeIE;
                            existing.TaskTimeIE2 = item.TaskTimeIE2;  // ✅ Thêm
                            existing.TaskTimeIE3 = item.TaskTimeIE3;  // ✅ Thêm
                            existing.TaskTimeIE4 = item.TaskTimeIE4;  // ✅ Thêm
                            existing.TaskTimeCus = item.TaskTimeCus;
                            existing.UpdateDate = DateTime.Now;
                            existing.UpdateUserName = BaseParameter.USER_IDX;

                            await _taskTimeFARepository.UpdateAsync(existing);
                            updateCount++;
                        }
                        else
                        {
                            item.Active = true;
                            item.CreateDate = DateTime.Now;
                            item.CreateUserName = BaseParameter.USER_IDX ?? "IMPORT";

                            await _taskTimeFARepository.AddAsync(item);
                            successCount++;
                        }
                    }
                    catch (Exception)
                    {
                        skipCount++;
                        continue;
                    }
                }

                result.TaskTimeFAList = await _taskTimeFARepository
                    .GetByCondition(x => x.Active == true)
                    .OrderByDescending(x => x.ID)
                    .ToListAsync();

                result.Success = (successCount + updateCount) > 0;
                result.Message = $"Import thành công! Thêm mới: {successCount}, Cập nhật: {updateCount}, Bỏ qua: {skipCount}";
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