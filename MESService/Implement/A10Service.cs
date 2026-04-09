namespace MESService.Implement
{
    public class A10Service : BaseService<torderlist, ItorderlistRepository>, IA10Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly ILineListRepository _lineListRepository;

        public A10Service(
            ItorderlistRepository torderlistRepository,
            ILineListRepository lineListRepository
        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _lineListRepository = lineListRepository;
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
                // Không cần thực hiện gì trong Load()
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
                var query = _lineListRepository.GetByCondition(x => x.Active == true);

                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString;
                    query = query.Where(x =>
                        (x.LineGroup != null && x.LineGroup.Contains(search)) ||
                        (x.LineName != null && x.LineName.Contains(search)) ||
                        (x.LineType != null && x.LineType.Contains(search)) ||
                        (x.Family != null && x.Family.Contains(search)) ||
                        (x.Description != null && x.Description.Contains(search))
                    );
                }

                result.LineList = await query.OrderByDescending(x => x.ID).ToListAsync();
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
                var line = new LineList
                {
                    LineGroup = BaseParameter.TextBox0,
                    LineName = BaseParameter.TextBox1,
                    LineType = BaseParameter.TextBox2,
                    Family = BaseParameter.TextBox3,
                    LineCapa = string.IsNullOrEmpty(BaseParameter.TextBox4) ? null : Convert.ToDecimal(BaseParameter.TextBox4),
                    WorkerNumber = string.IsNullOrEmpty(BaseParameter.ComboBox1) ? null : Convert.ToInt32(BaseParameter.ComboBox1),
                    SUB_Worker = string.IsNullOrEmpty(BaseParameter.ComboBox2) ? null : Convert.ToInt32(BaseParameter.ComboBox2),
                    FA_Worker = string.IsNullOrEmpty(BaseParameter.ComboBox3) ? null : Convert.ToInt32(BaseParameter.ComboBox3),
                    RO_Worker = string.IsNullOrEmpty(BaseParameter.ComboBox4) ? null : Convert.ToInt32(BaseParameter.ComboBox4),
                    CLIP_Worker = string.IsNullOrEmpty(BaseParameter.ComboBox5) ? null : Convert.ToInt32(BaseParameter.ComboBox5),
                    Channel = string.IsNullOrEmpty(BaseParameter.TextBox10) ? null : Convert.ToInt32(BaseParameter.TextBox10),
                    Vision_LeakTest = string.IsNullOrEmpty(BaseParameter.TextBox11) ? null : Convert.ToInt32(BaseParameter.TextBox11),
                    Description = BaseParameter.TextBox12,
                    Active = true,
                    CreateDate = DateTime.Now,
                    CreateUser = BaseParameter.USER_IDX // Lấy thông tin người dùng từ BaseParameter
                };

                // Thêm vào database
                await _lineListRepository.AddAsync(line);

                // Trả về bản ghi vừa thêm
                result.LineList = new List<LineList> { line };
                result.Success = true;
                result.Message = "Line added successfully.";
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
                // Tìm bản ghi cần cập nhật
                var existingLine = await _lineListRepository
                    .GetByCondition(x => x.ID == Convert.ToInt64(BaseParameter.ID))
                    .FirstOrDefaultAsync();

                if (existingLine == null)
                {
                    result.Error = "Line không tồn tại.";
                    return result;
                }

                // Cập nhật thông tin
                existingLine.LineGroup = BaseParameter.TextBox0;
                existingLine.LineName = BaseParameter.TextBox1;
                existingLine.LineType = BaseParameter.TextBox2;
                existingLine.Family = BaseParameter.TextBox3;
                existingLine.LineCapa = string.IsNullOrEmpty(BaseParameter.TextBox4) ? null : Convert.ToDecimal(BaseParameter.TextBox4);
                existingLine.WorkerNumber = string.IsNullOrEmpty(BaseParameter.ComboBox1) ? null : Convert.ToInt32(BaseParameter.ComboBox1);
                existingLine.SUB_Worker = string.IsNullOrEmpty(BaseParameter.ComboBox2) ? null : Convert.ToInt32(BaseParameter.ComboBox2);
                existingLine.FA_Worker = string.IsNullOrEmpty(BaseParameter.ComboBox3) ? null : Convert.ToInt32(BaseParameter.ComboBox3);
                existingLine.RO_Worker = string.IsNullOrEmpty(BaseParameter.ComboBox4) ? null : Convert.ToInt32(BaseParameter.ComboBox4);
                existingLine.CLIP_Worker = string.IsNullOrEmpty(BaseParameter.ComboBox5) ? null : Convert.ToInt32(BaseParameter.ComboBox5);
                existingLine.Channel = string.IsNullOrEmpty(BaseParameter.TextBox10) ? null : Convert.ToInt32(BaseParameter.TextBox10);
                existingLine.Vision_LeakTest = string.IsNullOrEmpty(BaseParameter.TextBox11) ? null : Convert.ToInt32(BaseParameter.TextBox11);
                existingLine.Description = BaseParameter.TextBox12;
                existingLine.UpdateDate = DateTime.Now;
                existingLine.UpdateUser = BaseParameter.USER_IDX; // Lấy thông tin người dùng từ BaseParameter

                // Cập nhật vào database
                await _lineListRepository.UpdateAsync(existingLine);

                // Trả về bản ghi sau khi cập nhật
                result.LineList = new List<LineList> { existingLine };
                result.Success = true;
                result.Message = "Line updated successfully.";
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
                // Tìm bản ghi cần xóa
                var existingLine = await _lineListRepository
                    .GetByCondition(x => x.ID == Convert.ToInt64(BaseParameter.ID))
                    .FirstOrDefaultAsync();

                if (existingLine == null)
                {
                    result.Error = "Line không tồn tại.";
                    return result;
                }

                // Soft delete - đặt Active = false
                existingLine.Active = false;
                existingLine.UpdateDate = DateTime.Now;
                existingLine.UpdateUser = BaseParameter.USER_IDX; // Lấy thông tin người dùng từ BaseParameter

                // Cập nhật vào database
                await _lineListRepository.UpdateAsync(existingLine);

                // Lấy danh sách sau khi xóa
                var lines = await _lineListRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderByDescending(x => x.ID)
                    .ToListAsync();

                result.LineList = lines;
                result.Success = true;
                result.Message = "Line deleted successfully.";
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

        public virtual async Task<BaseResult> Buttoninport_Click(List<LineList> importList)
        {
            BaseResult result = new BaseResult();
            try
            {
                int successCount = 0;

                foreach (var item in importList)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.LineName))
                            continue;

                        // Thêm thông tin khác
                        item.Active = true;

                        // Đảm bảo có thông tin CreateDate và CreateUser
                        if (item.CreateDate == null)
                            item.CreateDate = DateTime.Now;

                        if (string.IsNullOrEmpty(item.CreateUser))
                            item.CreateUser = "IMPORT";

                        // Thêm vào database
                        await _lineListRepository.AddAsync(item);
                        successCount++;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                // Lấy lại danh sách sau khi import
                var lines = await _lineListRepository
                    .GetByCondition(x => x.Active == true)
                    .OrderByDescending(x => x.ID)
                    .ToListAsync();

                result.LineList = lines;
                result.Success = successCount > 0;
                result.Message = $"Import thành công {successCount} bản ghi.";
            }
            catch (Exception ex)
            {
                result.Success = false;
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