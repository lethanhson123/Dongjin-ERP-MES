namespace MESService.Implement
{
    public class X01Service : BaseService<PartSpare, IPartSpareRepository>, IX01Service
    {
        private readonly IPartSpareRepository _partSpareRepository;
        private readonly IPartSpareScanInRepository _scanInRepository;
        private readonly IPartSpareScanOutRepository _scanOutRepository;

        public X01Service(
            IPartSpareRepository partSpareRepository,
            IPartSpareScanInRepository scanInRepository,
            IPartSpareScanOutRepository scanOutRepository
        ) : base(partSpareRepository)
        {
            _partSpareRepository = partSpareRepository;
            _scanInRepository = scanInRepository;
            _scanOutRepository = scanOutRepository;
        }

        public override void Initialization(PartSpare model)
        {
            BaseInitialization(model);
        }

        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                var query = _partSpareRepository.GetByCondition(x => x.Active == true);
                result.PartSpareList = await query
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();

                CalculateComputedFields(result.PartSpareList);
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
                var query = _partSpareRepository.GetByCondition(x => x.Active == true);

                // ✅ Xử lý tìm kiếm từ Frontend
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    string search = BaseParameter.SearchString.Trim();
                    query = query.Where(x =>
                        x.Code.Contains(search) ||
                        x.Name.Contains(search) ||
                        x.Display.Contains(search) ||
                        x.Description.Contains(search)
                    );
                }

                result.PartSpareList = await query
                    .OrderByDescending(x => x.CreateDate)
                    .ToListAsync();

                CalculateComputedFields(result.PartSpareList);
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
                await Task.Run(() => { });
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
                var partData = BaseParameter.PartSpare;

                var existing = await _partSpareRepository
                    .GetByCondition(x => x.Code == partData.Code && x.Active == true)
                    .FirstOrDefaultAsync();

                if (existing != null)
                {
                    existing.Name = partData.Name;
                    existing.Price = partData.Price;
                    existing.Display = partData.Display;
                    existing.QuantityRequired = partData.QuantityRequired;
                    existing.SafetyStock = partData.SafetyStock;
                    existing.Description = partData.Description;
                    existing.UpdateDate = DateTime.Now;
                    existing.UpdateUserName = BaseParameter.USER_IDX;
                    existing.InventoryWarning = existing.Inventory - existing.SafetyStock;
                    existing.TotalAmount = existing.Inventory * existing.Price;

                    await _partSpareRepository.UpdateAsync(existing);
                    result.Message = "Updated successfully";
                }
                else
                {
                    partData.Active = true;
                    partData.CreateDate = DateTime.Now;
                    partData.CreateUserName = BaseParameter.USER_IDX;
                    partData.Inventory = 0;
                    partData.InventoryWarning = -partData.SafetyStock;
                    partData.TotalAmount = 0;

                    await _partSpareRepository.AddAsync(partData);
                    result.Message = "Created successfully";
                }

                result.PartSpareList = new List<PartSpare> { partData };
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        // =====================================================
        // DELETE - Soft Delete Spare Part
        // =====================================================
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var partSpare = await _partSpareRepository
                    .GetByCondition(x => x.ID == BaseParameter.ID)
                    .FirstOrDefaultAsync();

                var hasScans = await _scanInRepository
                    .GetByCondition(x => x.ParentID == BaseParameter.ID)
                    .AnyAsync()
                    || await _scanOutRepository
                    .GetByCondition(x => x.ParentID == BaseParameter.ID)
                    .AnyAsync();

                if (hasScans)
                {
                    partSpare.Active = false;
                    partSpare.UpdateDate = DateTime.Now;
                    partSpare.UpdateUserName = BaseParameter.USER_IDX;
                    await _partSpareRepository.UpdateAsync(partSpare);
                }
                else
                {
                    await _partSpareRepository.RemoveAsync(partSpare);
                }

                result.Success = true;
                result.Message = "Deleted successfully";
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
                int updateCount = 0;
                int skipCount = 0;

                foreach (var item in BaseParameter.PartSpareList)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.Code))
                        {
                            skipCount++;
                            continue;
                        }

                        var existing = await _partSpareRepository
                            .GetByCondition(x => x.Code == item.Code && x.Active == true)
                            .FirstOrDefaultAsync();

                        if (existing != null)
                        {
                            existing.Name = item.Name;
                            existing.Price = item.Price;
                            existing.Display = item.Display;
                            existing.QuantityRequired = item.QuantityRequired;
                            existing.SafetyStock = item.SafetyStock;
                            existing.Description = item.Description;
                            existing.UpdateDate = DateTime.Now;
                            existing.UpdateUserName = BaseParameter.USER_IDX ?? "IMPORT";
                            existing.InventoryWarning = existing.Inventory - existing.SafetyStock;
                            existing.TotalAmount = existing.Inventory * existing.Price;

                            await _partSpareRepository.UpdateAsync(existing);
                            updateCount++;
                        }
                        else
                        {
                            item.Active = true;
                            item.CreateDate = DateTime.Now;
                            item.CreateUserName = BaseParameter.USER_IDX ?? "IMPORT";
                            item.Inventory = 0;
                            item.InventoryWarning = -item.SafetyStock;
                            item.TotalAmount = 0;

                            await _partSpareRepository.AddAsync(item);
                            successCount++;
                        }
                    }
                    catch
                    {
                        skipCount++;
                        continue;
                    }
                }

                result.Success = (successCount + updateCount) > 0;
                result.Message = $"Import complete! Created: {successCount}, Updated: {updateCount}, Skipped: {skipCount}";
                result.TotalCount = successCount;
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
                var code = BaseParameter.SearchString;
                var bytes = GenerateQRCodeBytes(code);

                result.QRCodeData = Convert.ToBase64String(bytes);
                result.QRCodeText = code;
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return await Task.FromResult(result);
        }

        private byte[] GenerateQRCodeBytes(string data)
        {
            using var generator = new QRCodeGenerator();
            using var qrData = generator.CreateQrCode(data, QRCodeGenerator.ECCLevel.L);
            using var qr = new QRCode(qrData);
            using var bmp = qr.GetGraphic(20);
            using var ms = new MemoryStream();
            bmp.Save(ms, ImageFormat.Png);
            return ms.ToArray();
        }

        public virtual async Task<BaseResult> UploadPartImage(IFormFile file, BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "PartImages");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                var partSpare = await _partSpareRepository
                    .GetByCondition(x => x.ID == BaseParameter.ID)
                    .FirstOrDefaultAsync();

                if (partSpare != null)
                {
                    partSpare.FileName = fileName;
                    partSpare.UpdateDate = DateTime.Now;
                    partSpare.UpdateUserName = BaseParameter.USER_IDX;
                    await _partSpareRepository.UpdateAsync(partSpare);
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> ButtonScanIn_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var scanData = BaseParameter.PartSpareScanIn;

                var partSpare = await _partSpareRepository
                    .GetByCondition(x => x.Code == scanData.Code && x.Active == true)
                    .FirstOrDefaultAsync();

                if (partSpare == null)
                {
                    result.Error = "NotFound";
                    return result;
                }

                partSpare.Inventory = (partSpare.Inventory ?? 0) + scanData.Quantity.Value;
                partSpare.SafetyStock = scanData.SafetyQty ?? partSpare.SafetyStock;
                partSpare.InventoryWarning = partSpare.Inventory - partSpare.SafetyStock;
                partSpare.TotalAmount = partSpare.Inventory * partSpare.Price;
                partSpare.UpdateDate = DateTime.Now;
                partSpare.UpdateUserName = BaseParameter.USER_IDX;

                await _partSpareRepository.UpdateAsync(partSpare);

                var scanIn = new PartSpareScanIn
                {
                    ParentID = partSpare.ID,
                    Code = partSpare.Code,
                    Name = partSpare.Name,
                    Quantity = scanData.Quantity,
                    SafetyQty = scanData.SafetyQty,
                    InventoryQty = partSpare.Inventory,
                    Note = scanData.Note,
                    Description = scanData.Description,
                    Active = true,
                    CreateDate = DateTime.Now,
                    CreateUserName = BaseParameter.USER_IDX
                };

                await _scanInRepository.AddAsync(scanIn);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> ButtonScanOut_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var scanData = BaseParameter.PartSpareScanOut;

                var partSpare = await _partSpareRepository
                    .GetByCondition(x => x.Code == scanData.Code && x.Active == true)
                    .FirstOrDefaultAsync();

                if (partSpare == null)
                {
                    result.Error = "NotFound";
                    return result;
                }

                if ((partSpare.Inventory ?? 0) < scanData.Quantity.Value)
                {
                    result.Error = "NotEnoughInventory";
                    return result;
                }

                partSpare.Inventory = (partSpare.Inventory ?? 0) - scanData.Quantity.Value;
                partSpare.SafetyStock = scanData.SafetyQty ?? partSpare.SafetyStock;
                partSpare.InventoryWarning = partSpare.Inventory - partSpare.SafetyStock;
                partSpare.TotalAmount = partSpare.Inventory * partSpare.Price;
                partSpare.UpdateDate = DateTime.Now;
                partSpare.UpdateUserName = BaseParameter.USER_IDX;

                await _partSpareRepository.UpdateAsync(partSpare);

                var scanOut = new PartSpareScanOut
                {
                    ParentID = partSpare.ID,
                    Code = partSpare.Code,
                    Name = partSpare.Name,
                    Quantity = scanData.Quantity,
                    SafetyQty = scanData.SafetyQty,
                    InventoryQty = partSpare.Inventory,
                    Note = scanData.Note,
                    Description = scanData.Description,
                    Active = true,
                    CreateDate = DateTime.Now,
                    CreateUserName = BaseParameter.USER_IDX
                };

                await _scanOutRepository.AddAsync(scanOut);

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> LoadScanInData(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var query = _scanInRepository.GetByCondition(x => x.Active == true);

                if (BaseParameter?.FromDate != null && BaseParameter?.ToDate != null)
                {
                    var toDateEnd = BaseParameter.ToDate.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreateDate >= BaseParameter.FromDate && x.CreateDate <= toDateEnd);
                }

                if (!string.IsNullOrEmpty(BaseParameter?.SearchString))
                {
                    query = query.Where(x => x.Code.Contains(BaseParameter.SearchString) || x.Name.Contains(BaseParameter.SearchString));
                }

                result.PartSpareScanInList = await query.OrderByDescending(x => x.CreateDate).ToListAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> LoadScanOutData(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var query = _scanOutRepository.GetByCondition(x => x.Active == true);

                if (BaseParameter?.FromDate != null && BaseParameter?.ToDate != null)
                {
                    var toDateEnd = BaseParameter.ToDate.Value.Date.AddDays(1).AddSeconds(-1);
                    query = query.Where(x => x.CreateDate >= BaseParameter.FromDate && x.CreateDate <= toDateEnd);
                }

                if (!string.IsNullOrEmpty(BaseParameter?.SearchString))
                {
                    query = query.Where(x => x.Code.Contains(BaseParameter.SearchString) || x.Name.Contains(BaseParameter.SearchString));
                }

                result.PartSpareScanOutList = await query.OrderByDescending(x => x.CreateDate).ToListAsync();
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> GetPartInfoByCode(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var partSpare = await _partSpareRepository
                    .GetByCondition(x => x.Code == BaseParameter.SearchString && x.Active == true)
                    .FirstOrDefaultAsync();

                if (partSpare != null)
                {
                    result.QuantityRequired = partSpare.QuantityRequired;
                    result.SafetyStock = partSpare.SafetyStock;
                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
        {
            return await Task.FromResult(new BaseResult { Success = true });
        }

        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
        {
            return await Task.FromResult(new BaseResult { Success = true });
        }

        public virtual async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
        {
            return await Task.FromResult(new BaseResult { Success = true });
        }

        public virtual async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
        {
            return await Task.FromResult(new BaseResult { Success = true });
        }
        private void CalculateComputedFields(List<PartSpare> parts)
        {
            if (parts == null || parts.Count == 0) return;

            foreach (var part in parts)
            {
                part.InventoryWarning = (part.Inventory ?? 0) - (part.SafetyStock ?? 0);
                part.TotalAmount = (part.Inventory ?? 0) * (part.Price ?? 0);
            }
        }
    }
}