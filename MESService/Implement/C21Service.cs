namespace MESService.Implement
{
    public class C21Service : BaseService<Trolley, ITrolleyRepository>, IC21Service
    {
        private readonly ITrolleyRepository _trolleyRepository;

        public C21Service(ITrolleyRepository trolleyRepository) : base(trolleyRepository)
        {
            _trolleyRepository = trolleyRepository;
        }

        public override void Initialization(Trolley model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                result.TrolleyList = await _trolleyRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderByDescending(x => x.ID)
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
                var query = _trolleyRepository.GetByCondition(x => x.Active == true);

                // Tìm kiếm theo TrolleyCode hoặc Location
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString.Trim();
                    query = query.Where(x =>
                        x.TrolleyCode.Contains(search) ||
                        x.Location.Contains(search)
                    );
                }

                result.TrolleyList = await query
                    .OrderByDescending(x => x.ID)
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
                // Kiểm tra TrolleyCode đã tồn tại chưa
                var exists = await _trolleyRepository
                    .GetByCondition(x => x.TrolleyCode == BaseParameter.Trolley.TrolleyCode && x.Active == true)
                    .AnyAsync();

                if (exists)
                {
                    result.Error = "Trolley Code đã tồn tại!";
                    return result;
                }

                var trolley = BaseParameter.Trolley;
                trolley.Active = true;
                trolley.CreatedDate = DateTime.Now;
                trolley.CreatedBy = BaseParameter.USER_IDX ?? "SYSTEM";

                await _trolleyRepository.AddAsync(trolley);

                result.TrolleyList = new List<Trolley> { trolley };
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
                var existingTrolley = await _trolleyRepository
                    .GetByCondition(x => x.ID == BaseParameter.Trolley.ID)
                    .FirstOrDefaultAsync();

                if (existingTrolley == null)
                {
                    result.Error = "Không tìm thấy Trolley.";
                    return result;
                }

                // Kiểm tra TrolleyCode trùng (trừ chính nó)
                var duplicateCode = await _trolleyRepository
                    .GetByCondition(x =>
                        x.TrolleyCode == BaseParameter.Trolley.TrolleyCode &&
                        x.ID != BaseParameter.Trolley.ID &&
                        x.Active == true)
                    .AnyAsync();

                if (duplicateCode)
                {
                    result.Error = "Trolley Code đã tồn tại!";
                    return result;
                }

                existingTrolley.TrolleyCode = BaseParameter.Trolley.TrolleyCode;
                existingTrolley.Location = BaseParameter.Trolley.Location;
                existingTrolley.UpdateDate = DateTime.Now;
                existingTrolley.UpdateBy = BaseParameter.USER_IDX ?? "SYSTEM";

                await _trolleyRepository.UpdateAsync(existingTrolley);

                result.TrolleyList = new List<Trolley> { existingTrolley };
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
                var trolley = await _trolleyRepository
                    .GetByCondition(x => x.ID == BaseParameter.ID)
                    .FirstOrDefaultAsync();

                if (trolley == null)
                {
                    result.Error = "Không tìm thấy Trolley.";
                    return result;
                }

                // Soft delete
                trolley.Active = false;
                trolley.UpdateDate = DateTime.Now;
                trolley.UpdateBy = BaseParameter.USER_IDX ?? "SYSTEM";

                await _trolleyRepository.UpdateAsync(trolley);

                result.TrolleyList = await _trolleyRepository
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
                result.Success = true;
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

                foreach (var item in BaseParameter.TrolleyList)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.TrolleyCode) ||
                            string.IsNullOrWhiteSpace(item.Location))
                        {
                            skipCount++;
                            continue;
                        }

                        // Kiểm tra đã tồn tại chưa
                        var existing = await _trolleyRepository
                            .GetByCondition(x =>
                                x.TrolleyCode == item.TrolleyCode &&
                                x.Active == true)
                            .FirstOrDefaultAsync();

                        if (existing != null)
                        {
                            // Update nếu đã tồn tại
                            existing.Location = item.Location;
                            existing.UpdateDate = DateTime.Now;
                            existing.UpdateBy = BaseParameter.USER_IDX ?? "IMPORT";

                            await _trolleyRepository.UpdateAsync(existing);
                            updateCount++;
                        }
                        else
                        {
                            // Thêm mới nếu chưa tồn tại
                            item.Active = true;
                            item.CreatedDate = DateTime.Now;
                            item.CreatedBy = BaseParameter.USER_IDX ?? "IMPORT";

                            await _trolleyRepository.AddAsync(item);
                            successCount++;
                        }
                    }
                    catch (Exception)
                    {
                        skipCount++;
                        continue;
                    }
                }

                result.TrolleyList = await _trolleyRepository
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
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}