namespace MESService.Implement
{
    public class FA_M4Service : BaseService<FAWorkOrder, IFAWorkOrderRepository>, IFA_M4Service
    {
        private readonly IFAWorkOrderRepository _faWorkOrderRepository;
        private readonly ILineListRepository _lineListRepository;
        private readonly IFAWorkOrderHistoryRepository _faWorkOrderHistoryRepository; 

        public FA_M4Service(
            IFAWorkOrderRepository faWorkOrderRepository,
            ILineListRepository lineListRepository,
            IFAWorkOrderHistoryRepository faWorkOrderHistoryRepository 
        ) : base(faWorkOrderRepository)
        {
            _faWorkOrderRepository = faWorkOrderRepository;
            _lineListRepository = lineListRepository;
            _faWorkOrderHistoryRepository = faWorkOrderHistoryRepository; 
        }

        public override void Initialization(FAWorkOrder model)
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
                var query = _faWorkOrderRepository.GetByCondition(x => x.Active == true);

                // Lọc theo Line
                if (BaseParameter.LineID != null && BaseParameter.LineID > 0)
                {
                    query = query.Where(x => x.LineID == BaseParameter.LineID);
                }

                // Lọc theo Date
                if (BaseParameter.Date != null)
                {
                    var filterDate = BaseParameter.Date.Value.Date;
                    query = query.Where(x => x.Date.Date == filterDate);
                }

                // Tìm kiếm text
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString;
                    query = query.Where(x =>
                        x.PartNumber.Contains(search) ||
                        (x.ECN != null && x.ECN.Contains(search)) ||
                        (x.New_ECN != null && x.New_ECN.Contains(search)) ||
                        (x.Remark != null && x.Remark.Contains(search))
                    );
                }

                result.FAWorkOrderList = await query
                    .OrderByDescending(x => x.Date)
                    .ThenByDescending(x => x.ID)
                    .ToListAsync();

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

        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                // Validate
                if (BaseParameter.FAWorkOrder == null)
                {
                    result.Error = "Dữ liệu không hợp lệ";
                    return result;
                }

                if (string.IsNullOrWhiteSpace(BaseParameter.FAWorkOrder.PartNumber))
                {
                    result.Error = "Part Number không được để trống";
                    return result;
                }

                if (BaseParameter.FAWorkOrder.LineID == null)
                {
                    result.Error = "Line không được để trống";
                    return result;
                }

                var workOrder = BaseParameter.FAWorkOrder;
                workOrder.Active = true;
                workOrder.CreatedDate = DateTime.Now;
                workOrder.CreatedBy = BaseParameter.USER_IDX;

                await _faWorkOrderRepository.AddAsync(workOrder);

                result.FAWorkOrderList = new List<FAWorkOrder> { workOrder };
                result.Success = true;
                result.Message = "Thêm mới thành công";
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
                var existingWorkOrder = await _faWorkOrderRepository
                    .GetByCondition(x => x.ID == BaseParameter.FAWorkOrder.ID && x.Active == true)
                    .FirstOrDefaultAsync();

                if (existingWorkOrder == null)
                {
                    result.Error = "Không tìm thấy Work Order";
                    return result;
                }

                // Lưu history trước khi update
                var history = new FAWorkOrderHistory
                {
                    WorkOrderID = existingWorkOrder.ID,
                    PartNumber = existingWorkOrder.PartNumber,
                    OldDate = existingWorkOrder.Date,
                    NewDate = BaseParameter.FAWorkOrder.Date,
                    OldQuantity = existingWorkOrder.Quantity,
                    NewQuantity = BaseParameter.FAWorkOrder.Quantity,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = BaseParameter.USER_IDX
                };
                await _faWorkOrderHistoryRepository.AddAsync(history);

                // Update work order
                existingWorkOrder.LineID = BaseParameter.FAWorkOrder.LineID;
                existingWorkOrder.PartNumber = BaseParameter.FAWorkOrder.PartNumber;
                existingWorkOrder.ECN = BaseParameter.FAWorkOrder.ECN;
                existingWorkOrder.New_ECN = BaseParameter.FAWorkOrder.New_ECN;
                existingWorkOrder.SNP = BaseParameter.FAWorkOrder.SNP;
                existingWorkOrder.Remark = BaseParameter.FAWorkOrder.Remark;
                existingWorkOrder.Date = BaseParameter.FAWorkOrder.Date;
                existingWorkOrder.Quantity = BaseParameter.FAWorkOrder.Quantity;
                existingWorkOrder.ModifiedDate = DateTime.Now;
                existingWorkOrder.ModifiedBy = BaseParameter.USER_IDX;

                await _faWorkOrderRepository.UpdateAsync(existingWorkOrder);

                result.FAWorkOrderList = new List<FAWorkOrder> { existingWorkOrder };
                result.Success = true;
                result.Message = "Cập nhật thành công";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> GetHistory(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                var histories = await _faWorkOrderHistoryRepository
                    .GetByCondition(x => x.WorkOrderID == BaseParameter.ID)
                    .OrderByDescending(x => x.ModifiedDate)
                    .ToListAsync();

                result.FAWorkOrderHistoryList = histories;
                result.Success = true;
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
                var workOrder = await _faWorkOrderRepository
                    .GetByCondition(x => x.ID == BaseParameter.ID && x.Active == true)
                    .FirstOrDefaultAsync();

                if (workOrder == null)
                {
                    result.Error = "Không tìm thấy Work Order";
                    return result;
                }

                workOrder.Active = false;
                workOrder.ModifiedDate = DateTime.Now;
                workOrder.ModifiedBy = BaseParameter.USER_IDX;

                await _faWorkOrderRepository.UpdateAsync(workOrder);

                result.Success = true;
                result.Message = "Xóa thành công";
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
                if (BaseParameter.FAWorkOrderList == null || BaseParameter.FAWorkOrderList.Count == 0)
                {
                    result.Error = "Không có dữ liệu để import";
                    return result;
                }

                int added = 0;
                int updated = 0;
                int skipped = 0;

                foreach (var item in BaseParameter.FAWorkOrderList)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(item.PartNumber) || item.LineID == null)
                        {
                            skipped++;
                            continue;
                        }

                        if (item.Date == default(DateTime))
                        {
                            skipped++;
                            continue;
                        }

                        var existing = await _faWorkOrderRepository
                            .GetByCondition(x =>
                                x.LineID == item.LineID &&
                                x.PartNumber == item.PartNumber &&
                                x.ECN == item.ECN &&
                                x.Date.Date == item.Date.Date &&
                                x.Active == true)
                            .FirstOrDefaultAsync();

                        if (existing != null)
                        {
                            var history = new FAWorkOrderHistory
                            {
                                WorkOrderID = existing.ID,
                                PartNumber = existing.PartNumber,
                                OldDate = existing.Date,
                                NewDate = item.Date,
                                OldQuantity = existing.Quantity,
                                NewQuantity = item.Quantity,
                                ModifiedDate = DateTime.Now,
                                ModifiedBy = BaseParameter.USER_IDX ?? "IMPORT"
                            };
                            await _faWorkOrderHistoryRepository.AddAsync(history);

                            existing.New_ECN = item.New_ECN;
                            existing.SNP = item.SNP;
                            existing.Quantity = item.Quantity;
                            existing.Remark = item.Remark ?? existing.Remark;
                            existing.ModifiedDate = DateTime.Now;
                            existing.ModifiedBy = BaseParameter.USER_IDX ?? "IMPORT";

                            await _faWorkOrderRepository.UpdateAsync(existing);
                            updated++;
                        }
                        else
                        {
                            item.Active = true;
                            item.Remark = item.Remark ?? "";
                            item.CreatedDate = DateTime.Now;
                            item.CreatedBy = BaseParameter.USER_IDX ?? "IMPORT";

                            await _faWorkOrderRepository.AddAsync(item);
                            added++;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error importing record: {ex.Message}");
                        skipped++;
                        continue;
                    }
                }

                result.Success = (added + updated) > 0;
                result.Message = $"Import hoàn tất!\n✓ Thêm mới: {added}\n✓ Cập nhật: {updated}\n✗ Bỏ qua: {skipped}";
            }
            catch (Exception ex)
            {
                result.Error = "Lỗi import: " + ex.Message;
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