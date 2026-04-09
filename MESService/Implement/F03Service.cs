namespace MESService.Implement
{
    public class F03Service : BaseService<IQCNGCustomer2, IIQCNGCustomer2Repository>, IF03Service
    {
        private readonly IIQCNGCustomer2Repository _iqcNGCustomer2Repository;

        public F03Service(IIQCNGCustomer2Repository iqcNGCustomer2Repository) : base(iqcNGCustomer2Repository)
        {
            _iqcNGCustomer2Repository = iqcNGCustomer2Repository;
        }

        public override void Initialization(IQCNGCustomer2 model)
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
                var query = _iqcNGCustomer2Repository.GetByCondition(x => true);

                if (BaseParameter.FromDate.HasValue && BaseParameter.ToDate.HasValue)
                {
                    var fromDate = BaseParameter.FromDate.Value.Date;
                    var toDate = BaseParameter.ToDate.Value.Date.AddDays(1);
                    query = query.Where(x => x.IssueDate >= fromDate && x.IssueDate < toDate);
                }

                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var search = BaseParameter.SearchString;
                    query = query.Where(x =>
                        x.InvoiceName.Contains(search) ||
                        x.Barcode.Contains(search) ||
                        x.MaterialCode.Contains(search) ||
                        x.SupplierCode.Contains(search) ||
                        x.ErrorInfo.Contains(search)
                    );
                }

                result.IQCNGCustomer2List = await query
                    .OrderByDescending(x => x.IssueDate)
                    .ThenByDescending(x => x.ID)
                    .Take(1000)
                    .ToListAsync();
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
                var iqcNG = BaseParameter.IQCNGCustomer2;

                // ✅ SET GIÁ TRỊ MẶC ĐỊNH CHO TẤT CẢ - KHÔNG VALIDATE GÌ CẢ
                if (string.IsNullOrWhiteSpace(iqcNG.Barcode))
                    iqcNG.Barcode = "";

                if (iqcNG.NGQty <= 0)
                    iqcNG.NGQty = 0;

                if (string.IsNullOrWhiteSpace(iqcNG.ErrorInfo))
                    iqcNG.ErrorInfo = "";

                if (string.IsNullOrWhiteSpace(iqcNG.SupplierCode))
                    iqcNG.SupplierCode = "";

                if (string.IsNullOrWhiteSpace(iqcNG.MaterialCode))
                    iqcNG.MaterialCode = "";

                if (string.IsNullOrWhiteSpace(iqcNG.MaterialName))
                    iqcNG.MaterialName = "";

                if (string.IsNullOrWhiteSpace(iqcNG.InvoiceName))
                    iqcNG.InvoiceName = "";

                // ✅ Quantity: Nếu = 0 thì lấy = NGQty
                if (iqcNG.Quantity <= 0)
                    iqcNG.Quantity = iqcNG.NGQty;

                // ✅ TotalInvoice: Nếu = 0 thì lấy = Quantity
                if (iqcNG.TotalInvoice <= 0)
                    iqcNG.TotalInvoice = iqcNG.Quantity;

                // ✅ Unit: Default PC
                if (string.IsNullOrWhiteSpace(iqcNG.Unit))
                    iqcNG.Unit = "PC";
                else
                    iqcNG.Unit = iqcNG.Unit.ToUpper();

                // ✅ SET CÁC FIELD HỆ THỐNG
                iqcNG.InputDate = DateTime.Now;
                iqcNG.IssueDate = DateTime.Now;
                iqcNG.Week = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(
                    DateTime.Now,
                    CalendarWeekRule.FirstDay,
                    DayOfWeek.Monday
                );
                iqcNG.Month = DateTime.Now.Month;
                iqcNG.ImprovementPlans = "waiting feedback";
                iqcNG.Note = "On going";
                iqcNG.CreateUser = BaseParameter.USER_IDX;
                iqcNG.CreateDate = DateTime.Now;

                await _iqcNGCustomer2Repository.AddAsync(iqcNG);
                result.Error = null;
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
                var existing = await _iqcNGCustomer2Repository
                    .GetByCondition(x => x.ID == BaseParameter.IQCNGCustomer2.ID)
                    .FirstOrDefaultAsync();

                if (existing == null)
                {
                    result.Error = "Record not found";
                    return result;
                }

                existing.Quantity = BaseParameter.IQCNGCustomer2.Quantity;
                existing.NGQty = BaseParameter.IQCNGCustomer2.NGQty;

                // ✅ CẬP NHẬT UNIT
                if (!string.IsNullOrWhiteSpace(BaseParameter.IQCNGCustomer2.Unit))
                {
                    var unit = BaseParameter.IQCNGCustomer2.Unit.ToUpper();
                    if (unit == "PC" || unit == "MT")
                    {
                        existing.Unit = unit;
                    }
                }

                existing.ErrorInfo = BaseParameter.IQCNGCustomer2.ErrorInfo;
                existing.ImprovementPlans = BaseParameter.IQCNGCustomer2.ImprovementPlans;
                existing.Note = BaseParameter.IQCNGCustomer2.Note;
                existing.UpdateUser = BaseParameter.USER_IDX;
                existing.UpdateDate = DateTime.Now;

                await _iqcNGCustomer2Repository.UpdateAsync(existing);
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
                foreach (var id in BaseParameter.Ids)
                {
                    var item = await _iqcNGCustomer2Repository
                        .GetByCondition(x => x.ID == id)
                        .FirstOrDefaultAsync();

                    if (item != null)
                    {
                        _iqcNGCustomer2Repository.RemoveAsync(item);
                    }
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