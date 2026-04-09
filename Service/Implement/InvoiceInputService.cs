namespace Service.Implement
{
    public class InvoiceInputService : BaseService<InvoiceInput, IInvoiceInputRepository>
    , IInvoiceInputService
    {
        private readonly IInvoiceInputRepository _InvoiceInputRepository;
        private readonly ICompanyRepository _CompanyRepository;
        private readonly IInvoiceInputDetailRepository _InvoiceInputDetailRepository;
        private readonly IInvoiceInputFileRepository _InvoiceInputFileRepository;

        private readonly IInvoiceInputInventoryService _InvoiceInputInventoryService;
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        private readonly IMembershipCompanyRepository _MembershipCompanyRepository;
        private readonly IWarehouseInputService _WarehouseInputService;
        private readonly IWarehouseInputDetailService _WarehouseInputDetailService;
        public InvoiceInputService(IInvoiceInputRepository InvoiceInputRepository
            , ICompanyRepository CompanyRepository
            , IInvoiceInputDetailRepository InvoiceInputDetailRepository
            , IInvoiceInputFileRepository InvoiceInputFileRepository
            , IInvoiceInputInventoryService InvoiceInputInventoryService
            , IProductionOrderRepository ProductionOrderRepository
            , IMembershipCompanyRepository MembershipCompanyRepository
            , IWarehouseInputService WarehouseInputService
            , IWarehouseInputDetailService WarehouseInputDetailService
            ) : base(InvoiceInputRepository)
        {
            _InvoiceInputRepository = InvoiceInputRepository;
            _CompanyRepository = CompanyRepository;
            _InvoiceInputDetailRepository = InvoiceInputDetailRepository;
            _InvoiceInputFileRepository = InvoiceInputFileRepository;
            _InvoiceInputInventoryService = InvoiceInputInventoryService;
            _ProductionOrderRepository = ProductionOrderRepository;
            _MembershipCompanyRepository = MembershipCompanyRepository;
            _WarehouseInputService = WarehouseInputService;
            _WarehouseInputDetailService = WarehouseInputDetailService;
        }
        public override void InitializationSave(InvoiceInput model)
        {
            BaseInitialization(model);
            model.Code = model.Code ?? GlobalHelper.InitializationDateTimeCode;
            model.DateETD = model.DateETD ?? GlobalHelper.InitializationDateTime;
            model.DateETA = model.DateETA ?? GlobalHelper.InitializationDateTime;
            model.Year = model.DateETA.Value.Year;
            model.Month = model.DateETA.Value.Month;
            model.Day = model.DateETA.Value.Day;
            //model.SupplierID = model.SupplierID ?? GlobalHelper.CompanyID;
            //model.CustomerID = model.CustomerID ?? GlobalHelper.CompanyID;
            model.CompanyID = model.CustomerID ?? GlobalHelper.CompanyID;
            if (model.CompanyID > 0)
            {
                var Company = _CompanyRepository.GetByID(model.CompanyID.Value);
                model.CompanyName = Company.Name;
            }
            if (model.SupplierID > 0)
            {
                var Company = _CompanyRepository.GetByID(model.SupplierID.Value);
                model.SupplierName = Company.Name;
            }
            if (model.CustomerID > 0)
            {
                var Company = _CompanyRepository.GetByID(model.CustomerID.Value);
                model.CustomerName = Company.Name;
            }
            if (model.ProductionOrderID > 0)
            {
                var ProductionOrder = _ProductionOrderRepository.GetByID(model.ProductionOrderID.Value);
                model.ProductionOrderName = ProductionOrder.Code;
            }
            else
            {
                if (!string.IsNullOrEmpty(model.ProductionOrderName))
                {
                    var ProductionOrder = _ProductionOrderRepository.GetByCode(model.ProductionOrderName);
                    if (ProductionOrder.ID > 0)
                    {
                        model.ProductionOrderID = ProductionOrder.ID;
                    }
                }
            }
            model.Total = model.Total ?? GlobalHelper.InitializationNumber;
            model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
            model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
            model.TotalTax = model.Total * (model.Tax / 100);
            model.TotalDiscount = model.Total * (model.Discount / 100);
            model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;
        }
        public override async Task<BaseResult<InvoiceInput>> SaveAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.Code == BaseParameter.BaseModel.Code && o.SupplierID == BaseParameter.BaseModel.SupplierID && o.CustomerID == BaseParameter.BaseModel.CustomerID).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            bool IsSave = true;
            if (string.IsNullOrEmpty(BaseParameter.BaseModel.Code))
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.SupplierID == null)
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.CustomerID == null)
            {
                IsSave = false;
            }
            if (BaseParameter.BaseModel.CompanyID == null)
            {
                IsSave = false;
            }
            if (IsSave == true)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
                if (result.BaseModel.ID > 0)
                {
                    BaseParameter.BaseModel = result.BaseModel;
                    await SyncAsync(BaseParameter);
                }
            }
            return result;
        }
        public override async Task<BaseResult<InvoiceInput>> RemoveAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            result.Count = await _InvoiceInputRepository.RemoveAsync(BaseParameter.ID);
            if (result.Count > 0)
            {
                await SyncRemoveAsync(BaseParameter);
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInput>> SyncAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            await SyncInvoiceInputFileAsync(BaseParameter);
            //await SyncInvoiceInputInventoryAsync(BaseParameter);
            await SyncWarehouseInputAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInput>> SyncInvoiceInputFileAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            var List = await _InvoiceInputFileRepository.GetByCondition(item => item.Code == BaseParameter.BaseModel.Code).ToListAsync();
            if (List != null && List.Count > 0)
            {
                foreach (var item in List)
                {
                    item.ParentID = BaseParameter.BaseModel.ID;
                    await _InvoiceInputFileRepository.UpdateAsync(item);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInput>> SyncWarehouseInputAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.IsComplete == true)
                    {
                        var WarehouseInput = await _WarehouseInputService.GetByCondition(o => o.Active == true && o.IsSync == true && o.InvoiceInputID == BaseParameter.BaseModel.ID).FirstOrDefaultAsync();
                        if (WarehouseInput == null)
                        {
                            WarehouseInput = new WarehouseInput();
                            WarehouseInput.Active = true;
                            WarehouseInput.InvoiceInputID = BaseParameter.BaseModel.ID;
                            var BaseParameterWarehouseInput = new BaseParameter<WarehouseInput>();
                            BaseParameterWarehouseInput.BaseModel = WarehouseInput;
                            await _WarehouseInputService.SaveAsync(BaseParameterWarehouseInput);
                        }

                        //if (BaseParameterWarehouseInput.BaseModel != null && BaseParameterWarehouseInput.BaseModel.ID > 0)
                        //{
                        //    var BaseParameterWarehouseInputDetail = new BaseParameter<WarehouseInputDetail>();
                        //    BaseParameterWarehouseInputDetail.ParentID = BaseParameterWarehouseInput.BaseModel.ID;
                        //    BaseParameterWarehouseInputDetail.Active = true;
                        //    await _WarehouseInputDetailService.SaveListAndSyncWarehouseInputDetailBarcodeAsync(BaseParameterWarehouseInputDetail);
                        //}
                    }
                }
            }
            return result;
        }

        public virtual async Task<BaseResult<InvoiceInput>> SyncRemoveAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            //await SyncInvoiceInputInventoryAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInput>> SyncInvoiceInputDetailAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            var List = await _InvoiceInputDetailRepository.GetByParentIDToListAsync(BaseParameter.ID);
            if (List != null && List.Count > 0)
            {
                await _InvoiceInputDetailRepository.RemoveRangeAsync(List);
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInput>> SyncInvoiceInputInventoryAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            var BaseParameterInvoiceInputInventory = new BaseParameter<InvoiceInputInventory>();
            await _InvoiceInputInventoryService.CreateAutoAsync(BaseParameterInvoiceInputInventory);
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInput>> GetByActive_IsFutureToListAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            var DateNow = DateTime.Now;
            var DateAdd = DateNow.AddDays(1);
            result.List = await _InvoiceInputRepository.GetByCondition(o => o.Active == BaseParameter.Active && o.IsFuture == BaseParameter.IsComplete && o.DateETA != null && (DateNow.Date <= o.DateETA.Value.Date && o.DateETA.Value.Date <= DateAdd.Date)).OrderByDescending(o => o.DateETA).ToListAsync();
            return result;
        }

        public virtual async Task<BaseResult<InvoiceInput>> GetByMembershipIDToListAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipCompany = await _MembershipCompanyRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipCompany.Count > 0)
                {
                    var ListMembershipCompanyID = ListMembershipCompany.Select(o => o.CompanyID).ToList();
                    result.List = await GetByCondition(o => o.CustomerID != null && ListMembershipCompanyID.Contains(o.CustomerID)).OrderByDescending(o => o.DateETA).ThenBy(o => o.Code).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInput>> GetByMembershipID_ActiveToListAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            if (BaseParameter.MembershipID > 0)
            {
                var ListMembershipCompany = await _MembershipCompanyRepository.GetByParentIDAndActiveToListAsync(BaseParameter.MembershipID.Value, true);
                if (ListMembershipCompany.Count > 0)
                {
                    var ListMembershipCompanyID = ListMembershipCompany.Select(o => o.CompanyID).ToList();
                    result.List = await GetByCondition(o => o.CustomerID != null && ListMembershipCompanyID.Contains(o.CustomerID) && o.Active == BaseParameter.Active).OrderByDescending(o => o.DateETA).ThenBy(o => o.Code).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInput>> GetByCompanyID_SearchStringToListAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                result.List = await GetByCondition(o => !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
            }
            else
            {
                if (BaseParameter.CompanyID > 0)
                {
                    result.List = await GetByCondition(o => o.CustomerID == BaseParameter.CompanyID).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInput>> GetByCompanyID_Year_Month_SearchStringToListAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                else
                {
                    if (BaseParameter.Year == 0)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID).ToListAsync();
                    }
                    else
                    {
                        if (BaseParameter.Month == 0)
                        {
                            result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Year == BaseParameter.Year).ToListAsync();
                        }
                        else
                        {
                            result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month).ToListAsync();
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInput>> GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<InvoiceInput> BaseParameter)
        {
            var result = new BaseResult<InvoiceInput>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                else
                {
                    if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.DateETA != null && o.DateETA.Value.Date >= BaseParameter.DateBegin.Value.Date && o.DateETA.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                    }
                }
            }
            return result;
        }
    }
}

