namespace MESService.Implement
{
    public class F04Service : BaseService<NGList, INGListRepository>, IF04Service
    {
        private readonly INGListRepository _ngListRepository;

        public F04Service(INGListRepository ngListRepository) : base(ngListRepository)
        {
            _ngListRepository = ngListRepository;
        }

        public override void Initialization(NGList model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
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

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var query = _ngListRepository.GetByCondition(x => x.Active == true);

                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString;
                    query = query.Where(x =>
                        (x.ErrorCode != null && x.ErrorCode.Contains(search)) ||
                        (x.ErrorType != null && x.ErrorType.Contains(search)) ||
                        (x.ErrorDescription != null && x.ErrorDescription.Contains(search)) ||
                        (x.KoreanDescription != null && x.KoreanDescription.Contains(search))
                    );
                }

                result.NGLists = await query.OrderByDescending(x => x.ID).ToListAsync();
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
                if (BaseParameter.NGList == null)
                {
                    result.Error = "Dữ liệu không hợp lệ.";
                    return result;
                }

                var ngItem = BaseParameter.NGList;
                ngItem.Active = true;
                ngItem.CreateDate = DateTime.Now;
                ngItem.CreateUser = BaseParameter.USER_IDX;

                await _ngListRepository.AddAsync(ngItem);

                result.NGLists = new List<NGList> { ngItem };
                result.Success = true;
                result.Message = "Thêm mã lỗi thành công.";
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
                if (BaseParameter.NGList == null)
                {
                    result.Error = "Dữ liệu không hợp lệ.";
                    return result;
                }

                var ngItem = BaseParameter.NGList;
                var existingItem = await _ngListRepository
                    .GetByCondition(x => x.ID == ngItem.ID)
                    .FirstOrDefaultAsync();

                if (existingItem == null)
                {
                    result.Error = "Mã lỗi không tồn tại.";
                    return result;
                }

                existingItem.ErrorCode = ngItem.ErrorCode;
                existingItem.ErrorType = ngItem.ErrorType;
                existingItem.ErrorDescription = ngItem.ErrorDescription;
                existingItem.KoreanDescription = ngItem.KoreanDescription;
                existingItem.UpdateDate = DateTime.Now;
                existingItem.UpdateUser = BaseParameter.USER_IDX;

                await _ngListRepository.UpdateAsync(existingItem);

                result.NGLists = new List<NGList> { existingItem };
                result.Success = true;
                result.Message = "Cập nhật mã lỗi thành công.";
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
                long id = Convert.ToInt64(BaseParameter.ID);
                var existingItem = await _ngListRepository
                    .GetByCondition(x => x.ID == id)
                    .FirstOrDefaultAsync();

                if (existingItem == null)
                {
                    result.Error = "Mã lỗi không tồn tại.";
                    return result;
                }

                existingItem.Active = false;
                existingItem.UpdateDate = DateTime.Now;
                existingItem.UpdateUser = BaseParameter.USER_IDX;

                await _ngListRepository.UpdateAsync(existingItem);

                var items = await _ngListRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderByDescending(x => x.ID)
                    .ToListAsync();

                result.NGLists = items;
                result.Success = true;
                result.Message = "Xóa mã lỗi thành công.";
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
                result.Success = true;
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
                if (BaseParameter.NGLists == null || !BaseParameter.NGLists.Any())
                {
                    result.Error = "Không có dữ liệu import.";
                    return result;
                }

                int successCount = 0;
                foreach (var item in BaseParameter.NGLists)
                {
                    var existingCode = await _ngListRepository
                        .GetByCondition(x => x.ErrorCode == item.ErrorCode && x.Active == true)
                        .FirstOrDefaultAsync();

                    if (existingCode == null)
                    {
                        item.Active = true;
                        item.CreateDate = DateTime.Now;
                        item.CreateUser = BaseParameter.USER_IDX ?? "IMPORT";

                        await _ngListRepository.AddAsync(item);
                        successCount++;
                    }
                }

                var items = await _ngListRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderByDescending(x => x.ID)
                    .ToListAsync();

                result.NGLists = items;
                result.Success = true;
                result.Message = $"Import thành công {successCount} mã lỗi.";
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
                var items = await _ngListRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderBy(x => x.ErrorCode)
                    .ToListAsync();

                result.NGLists = items;
                result.Success = true;
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
                result.Success = true;
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
                result.Success = true;
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
                result.Success = true;
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public List<NGList> GetNGListsByErrorCodes(List<string> errorCodes)
        {
            return _ngListRepository
                .GetByCondition(x => errorCodes.Contains(x.ErrorCode) && x.Active == true)
                .ToList();
        }
    }
}