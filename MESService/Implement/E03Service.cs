namespace MESService.Implement
{
    public class E03Service : BaseService<ToolShop, IToolShopRepository>, IE03Service
    {
        private readonly IToolShopRepository _toolShopRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public E03Service(
            IToolShopRepository toolShopRepository,
            IWebHostEnvironment webHostEnvironment
        ) : base(toolShopRepository)
        {
            _toolShopRepository = toolShopRepository;
            _webHostEnvironment = webHostEnvironment;
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
                var query = _toolShopRepository.GetByCondition(x => x.Active == true);

                if (!string.IsNullOrEmpty(BaseParameter.ProductionLine))
                {
                    query = query.Where(x => x.ProductionLine == BaseParameter.ProductionLine);
                }

                if (!string.IsNullOrEmpty(BaseParameter.Dept))
                {
                    query = query.Where(x => x.Dept == BaseParameter.Dept);
                }

                if (!string.IsNullOrEmpty(BaseParameter.Status))
                {
                    query = query.Where(x => x.Status == BaseParameter.Status);
                }

                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString.ToLower();
                    query = query.Where(x =>
                        x.Code.ToLower().Contains(search) ||
                        x.Sub_Code.ToLower().Contains(search) ||
                        x.Serial.ToLower().Contains(search) ||
                        x.NameVN.ToLower().Contains(search) ||
                        x.NameEN.ToLower().Contains(search)
                    );
                }

                result.ToolShopList = await query
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
                var toolShop = BaseParameter.ToolShop;
                toolShop.Active = true;
                toolShop.CreateDate = DateTime.Now;
                toolShop.CreateUser = BaseParameter.USER_IDX;

                await _toolShopRepository.AddAsync(toolShop);
                result.ToolShopList = new List<ToolShop> { toolShop };
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
                var existingToolShop = await _toolShopRepository
                    .GetByCondition(x => x.ID == BaseParameter.ToolShop.ID)
                    .FirstOrDefaultAsync();

                if (existingToolShop == null)
                {
                    result.Error = "Không tìm thấy Tool Shop.";
                    return result;
                }

                existingToolShop.Code = BaseParameter.ToolShop.Code;
                existingToolShop.Sub_Code = BaseParameter.ToolShop.Sub_Code;
                existingToolShop.NameVN = BaseParameter.ToolShop.NameVN;
                existingToolShop.NameEN = BaseParameter.ToolShop.NameEN;
                existingToolShop.NameKO = BaseParameter.ToolShop.NameKO;
                existingToolShop.Serial = BaseParameter.ToolShop.Serial;
                existingToolShop.Quantity = BaseParameter.ToolShop.Quantity;
                existingToolShop.Unit = BaseParameter.ToolShop.Unit;
                existingToolShop.UsingDate = BaseParameter.ToolShop.UsingDate;
                existingToolShop.InvNumber = BaseParameter.ToolShop.InvNumber;
                existingToolShop.CustomDeclaration = BaseParameter.ToolShop.CustomDeclaration;
                existingToolShop.UnitPrice = BaseParameter.ToolShop.UnitPrice;
                existingToolShop.Currency = BaseParameter.ToolShop.Currency;
                existingToolShop.Type = BaseParameter.ToolShop.Type;
                existingToolShop.Owner = BaseParameter.ToolShop.Owner;
                existingToolShop.Supplier = BaseParameter.ToolShop.Supplier;
                existingToolShop.Dept = BaseParameter.ToolShop.Dept;
                existingToolShop.ProductionLine = BaseParameter.ToolShop.ProductionLine;
                existingToolShop.Note = BaseParameter.ToolShop.Note;
                existingToolShop.ExpiryDate = BaseParameter.ToolShop.ExpiryDate;
                existingToolShop.Status = BaseParameter.ToolShop.Status;
                existingToolShop.UpdateDate = DateTime.Now;
                existingToolShop.UpdateUser = BaseParameter.USER_IDX;

                await _toolShopRepository.UpdateAsync(existingToolShop);

                result.ToolShopList = new List<ToolShop> { existingToolShop };
                result.Success = true;
                result.Message = "Cập nhật thành công.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // ============================================
        // NEW METHOD: Upload Image
        // ============================================
        public virtual async Task<BaseResult> UploadImage(IFormFile file, int toolShopId)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (file == null || file.Length == 0)
                {
                    result.Error = "Không có file được chọn.";
                    return result;
                }

                // Validate file type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    result.Error = "Chỉ chấp nhận file ảnh (jpg, jpeg, png, gif, bmp).";
                    return result;
                }

                // Validate file size (max 5MB)
                if (file.Length > 5 * 1024 * 1024)
                {
                    result.Error = "Kích thước file không được vượt quá 5MB.";
                    return result;
                }

                // Create upload directory if not exists
                var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "toolshop");
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                // Generate unique filename
                var fileName = $"{toolShopId}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                var filePath = Path.Combine(uploadFolder, fileName);

                // Save file
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Update database
                var toolShop = await _toolShopRepository
                    .GetByCondition(x => x.ID == toolShopId)
                    .FirstOrDefaultAsync();

                if (toolShop != null)
                {
                    // Delete old image if exists
                    if (!string.IsNullOrEmpty(toolShop.Image))
                    {
                        var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, toolShop.Image.TrimStart('/'));
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }

                    toolShop.Image = $"/uploads/toolshop/{fileName}";
                    await _toolShopRepository.UpdateAsync(toolShop);

                    result.Success = true;
                    result.Message = toolShop.Image;
                }
                else
                {
                    result.Error = "Không tìm thấy Tool Shop.";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // ============================================
        // NEW METHOD: Delete Image
        // ============================================
        public virtual async Task<BaseResult> DeleteImage(int toolShopId)
        {
            BaseResult result = new BaseResult();
            try
            {
                var toolShop = await _toolShopRepository
                    .GetByCondition(x => x.ID == toolShopId)
                    .FirstOrDefaultAsync();

                if (toolShop == null)
                {
                    result.Error = "Không tìm thấy Tool Shop.";
                    return result;
                }

                if (!string.IsNullOrEmpty(toolShop.Image))
                {
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, toolShop.Image.TrimStart('/'));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    toolShop.Image = null;
                    await _toolShopRepository.UpdateAsync(toolShop);
                }

                result.Success = true;
                result.Message = "Xóa ảnh thành công.";
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
                var toolShop = await _toolShopRepository
                    .GetByCondition(x => x.ID == BaseParameter.ID)
                    .FirstOrDefaultAsync();

                if (toolShop == null)
                {
                    result.Error = "Không tìm thấy Tool Shop.";
                    return result;
                }

                toolShop.Active = false;
                toolShop.UpdateDate = DateTime.Now;
                toolShop.UpdateUser = BaseParameter.USER_IDX;

                await _toolShopRepository.UpdateAsync(toolShop);

                result.ToolShopList = await _toolShopRepository
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

                foreach (var item in BaseParameter.ToolShopList)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.Code))
                        {
                            skipCount++;
                            continue;
                        }

                        var existing = await _toolShopRepository
                            .GetByCondition(x =>
                                x.Code == item.Code &&
                                x.Serial == item.Serial &&
                                x.Active == true)
                            .FirstOrDefaultAsync();

                        if (existing != null)
                        {
                            existing.Sub_Code = item.Sub_Code;
                            existing.NameVN = item.NameVN;
                            existing.NameEN = item.NameEN;
                            existing.NameKO = item.NameKO;
                            existing.Quantity = item.Quantity;
                            existing.Unit = item.Unit;
                            existing.UsingDate = item.UsingDate;
                            existing.InvNumber = item.InvNumber;
                            existing.CustomDeclaration = item.CustomDeclaration;
                            existing.UnitPrice = item.UnitPrice;
                            existing.Currency = item.Currency;
                            existing.Type = item.Type;
                            existing.Owner = item.Owner;
                            existing.Supplier = item.Supplier;
                            existing.Dept = item.Dept;
                            existing.ProductionLine = item.ProductionLine;
                            existing.Note = item.Note;
                            existing.ExpiryDate = item.ExpiryDate;
                            existing.Status = item.Status;
                            existing.UpdateDate = DateTime.Now;
                            existing.UpdateUser = BaseParameter.USER_IDX;

                            await _toolShopRepository.UpdateAsync(existing);
                            updateCount++;
                        }
                        else
                        {
                            item.Active = true;
                            item.CreateDate = DateTime.Now;
                            item.CreateUser = BaseParameter.USER_IDX ?? "IMPORT";

                            await _toolShopRepository.AddAsync(item);
                            successCount++;
                        }
                    }
                    catch (Exception)
                    {
                        skipCount++;
                        continue;
                    }
                }

                result.ToolShopList = await _toolShopRepository
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