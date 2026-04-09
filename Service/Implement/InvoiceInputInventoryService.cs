namespace Service.Implement
{
    public class InvoiceInputInventoryService : BaseService<InvoiceInputInventory, IInvoiceInputInventoryRepository>
    , IInvoiceInputInventoryService
    {
        private readonly IInvoiceInputInventoryRepository _InvoiceInputInventoryRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IInvoiceInputRepository _InvoiceInputRepository;
        private readonly IInvoiceInputDetailRepository _InvoiceInputDetailRepository;
        public InvoiceInputInventoryService(IInvoiceInputInventoryRepository InvoiceInputInventoryRepository

            , ICategoryDepartmentRepository categoryDepartmentRepository
            , IMaterialRepository materialRepository
            , IInvoiceInputRepository invoiceInputRepository
            , IInvoiceInputDetailRepository invoiceInputDetailRepository

            ) : base(InvoiceInputInventoryRepository)
        {
            _InvoiceInputInventoryRepository = InvoiceInputInventoryRepository;

            _CategoryDepartmentRepository = categoryDepartmentRepository;
            _MaterialRepository = materialRepository;
            _InvoiceInputRepository = invoiceInputRepository;
            _InvoiceInputDetailRepository = invoiceInputDetailRepository;
        }
        public override void Initialization(InvoiceInputInventory model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _MaterialRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Name;
                model.Code = Parent.Code;
            }
            if (model.CategoryDepartmentID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.CategoryDepartmentID.Value);
                model.CategoryDepartmentName = CategoryDepartment.Name;
            }

            model.QuantityInput01 = model.QuantityInput01 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput02 = model.QuantityInput02 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput03 = model.QuantityInput03 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput04 = model.QuantityInput04 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput05 = model.QuantityInput05 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput06 = model.QuantityInput06 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput07 = model.QuantityInput07 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput08 = model.QuantityInput08 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput09 = model.QuantityInput09 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput10 = model.QuantityInput10 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput11 = model.QuantityInput11 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput12 = model.QuantityInput12 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput13 = model.QuantityInput13 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput14 = model.QuantityInput14 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput15 = model.QuantityInput15 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput16 = model.QuantityInput16 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput17 = model.QuantityInput17 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput18 = model.QuantityInput18 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput19 = model.QuantityInput19 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput20 = model.QuantityInput20 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput21 = model.QuantityInput21 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput22 = model.QuantityInput22 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput23 = model.QuantityInput23 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput24 = model.QuantityInput24 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput25 = model.QuantityInput25 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput26 = model.QuantityInput26 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput27 = model.QuantityInput27 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput28 = model.QuantityInput28 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput29 = model.QuantityInput29 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput30 = model.QuantityInput30 ?? GlobalHelper.InitializationNumber;
            model.QuantityInput31 = model.QuantityInput31 ?? GlobalHelper.InitializationNumber;

            model.QuantityOutput01 = model.QuantityOutput01 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput02 = model.QuantityOutput02 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput03 = model.QuantityOutput03 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput04 = model.QuantityOutput04 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput05 = model.QuantityOutput05 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput06 = model.QuantityOutput06 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput07 = model.QuantityOutput07 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput08 = model.QuantityOutput08 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput09 = model.QuantityOutput09 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput10 = model.QuantityOutput10 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput11 = model.QuantityOutput11 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput12 = model.QuantityOutput12 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput13 = model.QuantityOutput13 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput14 = model.QuantityOutput14 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput15 = model.QuantityOutput15 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput16 = model.QuantityOutput16 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput17 = model.QuantityOutput17 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput18 = model.QuantityOutput18 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput19 = model.QuantityOutput19 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput20 = model.QuantityOutput20 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput21 = model.QuantityOutput21 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput22 = model.QuantityOutput22 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput23 = model.QuantityOutput23 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput24 = model.QuantityOutput24 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput25 = model.QuantityOutput25 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput26 = model.QuantityOutput26 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput27 = model.QuantityOutput27 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput28 = model.QuantityOutput28 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput29 = model.QuantityOutput29 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput30 = model.QuantityOutput30 ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput31 = model.QuantityOutput31 ?? GlobalHelper.InitializationNumber;

            model.Quantity01 = model.QuantityInput01 - model.QuantityOutput01;
            model.Quantity02 = model.QuantityInput02 - model.QuantityOutput02;
            model.Quantity03 = model.QuantityInput03 - model.QuantityOutput03;
            model.Quantity04 = model.QuantityInput04 - model.QuantityOutput04;
            model.Quantity05 = model.QuantityInput05 - model.QuantityOutput05;
            model.Quantity06 = model.QuantityInput06 - model.QuantityOutput06;
            model.Quantity07 = model.QuantityInput07 - model.QuantityOutput07;
            model.Quantity08 = model.QuantityInput08 - model.QuantityOutput08;
            model.Quantity09 = model.QuantityInput09 - model.QuantityOutput09;
            model.Quantity10 = model.QuantityInput10 - model.QuantityOutput10;
            model.Quantity11 = model.QuantityInput11 - model.QuantityOutput11;
            model.Quantity12 = model.QuantityInput12 - model.QuantityOutput12;
            model.Quantity13 = model.QuantityInput13 - model.QuantityOutput13;
            model.Quantity14 = model.QuantityInput14 - model.QuantityOutput14;
            model.Quantity15 = model.QuantityInput15 - model.QuantityOutput15;
            model.Quantity16 = model.QuantityInput16 - model.QuantityOutput16;
            model.Quantity17 = model.QuantityInput17 - model.QuantityOutput17;
            model.Quantity18 = model.QuantityInput18 - model.QuantityOutput18;
            model.Quantity19 = model.QuantityInput19 - model.QuantityOutput19;
            model.Quantity20 = model.QuantityInput20 - model.QuantityOutput20;
            model.Quantity21 = model.QuantityInput21 - model.QuantityOutput21;
            model.Quantity22 = model.QuantityInput22 - model.QuantityOutput22;
            model.Quantity23 = model.QuantityInput23 - model.QuantityOutput23;
            model.Quantity24 = model.QuantityInput24 - model.QuantityOutput24;
            model.Quantity25 = model.QuantityInput25 - model.QuantityOutput25;
            model.Quantity26 = model.QuantityInput26 - model.QuantityOutput26;
            model.Quantity27 = model.QuantityInput27 - model.QuantityOutput27;
            model.Quantity28 = model.QuantityInput28 - model.QuantityOutput28;
            model.Quantity29 = model.QuantityInput29 - model.QuantityOutput29;
            model.Quantity30 = model.QuantityInput30 - model.QuantityOutput30;
            model.Quantity31 = model.QuantityInput31 - model.QuantityOutput31;

            model.QuantityInput00 = model.QuantityInput01 + model.QuantityInput02 + model.QuantityInput03 + model.QuantityInput04
                + model.QuantityInput05 + model.QuantityInput06 + model.QuantityInput07 + model.QuantityInput08
                + model.QuantityInput09 + model.QuantityInput10 + model.QuantityInput11 + model.QuantityInput12
                + model.QuantityInput13 + model.QuantityInput14 + model.QuantityInput15 + model.QuantityInput16
                + model.QuantityInput17 + model.QuantityInput18 + model.QuantityInput19 + model.QuantityInput20
                + model.QuantityInput21 + model.QuantityInput22 + model.QuantityInput23 + model.QuantityInput24
                + model.QuantityInput25 + model.QuantityInput26 + model.QuantityInput27 + model.QuantityInput28
                + model.QuantityInput29 + model.QuantityInput30 + model.QuantityInput31;

            model.QuantityOutput00 = model.QuantityOutput01 + model.QuantityOutput02 + model.QuantityOutput03 + model.QuantityOutput04 +
                model.QuantityOutput05 + model.QuantityOutput06 + model.QuantityOutput07 + model.QuantityOutput08 +
                model.QuantityOutput09 + model.QuantityOutput10 + model.QuantityOutput11 + model.QuantityOutput12 +
                model.QuantityOutput13 + model.QuantityOutput14 + model.QuantityOutput15 + model.QuantityOutput16 +
                model.QuantityOutput17 + model.QuantityOutput18 + model.QuantityOutput19 + model.QuantityOutput20 +
                model.QuantityOutput21 + model.QuantityOutput22 + model.QuantityOutput23 + model.QuantityOutput24 +
                model.QuantityOutput25 + model.QuantityOutput26 + model.QuantityOutput27 + model.QuantityOutput28 +
                model.QuantityOutput29 + model.QuantityOutput30 + model.QuantityOutput31;

            model.Quantity00 = model.QuantityInput00 - model.QuantityOutput00;

        }
        public override async Task<BaseResult<InvoiceInputInventory>> SaveAsync(BaseParameter<InvoiceInputInventory> BaseParameter)
        {
            var result = new BaseResult<InvoiceInputInventory>();
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
                try
                {
                    BaseParameter.BaseModel = result.BaseModel;
                    result = await SyncAsync(BaseParameter);
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInputInventory>> SyncAsync(BaseParameter<InvoiceInputInventory> BaseParameter)
        {
            var result = new BaseResult<InvoiceInputInventory>();

            return result;
        }
        public virtual async Task<BaseResult<InvoiceInputInventory>> CreateAutoAsync(BaseParameter<InvoiceInputInventory> BaseParameter)
        {
            var result = new BaseResult<InvoiceInputInventory>();
            var List = await _InvoiceInputInventoryRepository.GetAllToListAsync();
            if (List.Count > 0)
            {
                foreach (var item in List)
                {
                    item.QuantityInput01 = GlobalHelper.InitializationNumber;
                    item.QuantityInput00 = GlobalHelper.InitializationNumber;
                    await _InvoiceInputInventoryRepository.UpdateAsync(item);
                }
            }
            var ListInvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.Active == true && o.IsFuture == true).ToListAsync();
            if (ListInvoiceInput.Count > 0)
            {
                var ListInvoiceInputID = ListInvoiceInput.Select(o => o.ID).ToList();
                var ListInvoiceInputDetail = await _InvoiceInputDetailRepository.GetByCondition(o => o.Active == true && o.ParentID != null && ListInvoiceInputID.Contains(o.ParentID.Value)).ToListAsync();
                if (ListInvoiceInputDetail.Count > 0)
                {
                    foreach (var InvoiceInputDetail in ListInvoiceInputDetail)
                    {
                        var InvoiceInputInventory = new InvoiceInputInventory();
                        InvoiceInputInventory = await GetByCondition(o => o.Action == 1 && o.CategoryDepartmentID == GlobalHelper.DepartmentID && o.ParentID == InvoiceInputDetail.MaterialID).FirstOrDefaultAsync();
                        if ((InvoiceInputInventory == null) || (InvoiceInputInventory.ID == 0))
                        {
                            InvoiceInputInventory = new InvoiceInputInventory();
                        }
                        InvoiceInputInventory.Active = true;
                        InvoiceInputInventory.Action = 3;
                        InvoiceInputInventory.Year = GlobalHelper.InitializationNumber;
                        InvoiceInputInventory.Month = GlobalHelper.InitializationNumber;
                        InvoiceInputInventory.Day = GlobalHelper.InitializationNumber;
                        InvoiceInputInventory.ParentID = InvoiceInputDetail.MaterialID;
                        InvoiceInputInventory.CategoryDepartmentID = GlobalHelper.DepartmentID;
                        var ListInvoiceInputDetailSum = ListInvoiceInputDetail.Where(o => o.MaterialID == InvoiceInputInventory.ParentID).ToList();
                        if (ListInvoiceInputDetailSum.Count > 0)
                        {
                            InvoiceInputInventory.QuantityInput01 = ListInvoiceInputDetailSum.Sum(x => x.Quantity);
                        }
                        BaseParameter.BaseModel = InvoiceInputInventory;
                        await SaveAsync(BaseParameter);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<InvoiceInputInventory>> GetByCategoryDepartmentIDAndYearAndMonthToListAsync(BaseParameter<InvoiceInputInventory> BaseParameter)
        {
            var result = new BaseResult<InvoiceInputInventory>();
            result.List = await GetByCondition(item => item.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && item.Year == BaseParameter.Year && item.Month == BaseParameter.Month).ToListAsync();
            return result;
        }
    }
}

