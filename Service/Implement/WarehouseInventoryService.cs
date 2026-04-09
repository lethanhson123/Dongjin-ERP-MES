namespace Service.Implement
{
    public class WarehouseInventoryService : BaseService<WarehouseInventory, IWarehouseInventoryRepository>
    , IWarehouseInventoryService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IWarehouseInventoryRepository _WarehouseInventoryRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly IWarehouseInputDetailRepository _WarehouseInputDetailRepository;
        private readonly IWarehouseOutputDetailRepository _WarehouseOutputDetailRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseOutputDetailBarcodeRepository _WarehouseOutputDetailBarcodeRepository;
        private readonly IInvoiceInputRepository _InvoiceInputRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IProductionOrderRepository _ProductionOrderRepository;
        public WarehouseInventoryService(IWarehouseInventoryRepository WarehouseInventoryRepository
            , IWebHostEnvironment WebHostEnvironment
            , IWarehouseInputRepository warehouseInputRepository
            , IWarehouseOutputRepository warehouseOutputRepository
            , IWarehouseInputDetailRepository warehouseInputDetailRepository
            , IWarehouseOutputDetailRepository warehouseOutputDetailRepository
            , IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
            , IWarehouseOutputDetailBarcodeRepository WarehouseOutputDetailBarcodeRepository
            , IInvoiceInputRepository InvoiceInputRepository
            , ICategoryDepartmentRepository categoryDepartmentRepository
            , IMaterialRepository materialRepository
            , IProductionOrderRepository productionOrderRepository

            ) : base(WarehouseInventoryRepository)
        {
            _WebHostEnvironment = WebHostEnvironment;
            _WarehouseInventoryRepository = WarehouseInventoryRepository;
            _WarehouseInputRepository = warehouseInputRepository;
            _WarehouseOutputRepository = warehouseOutputRepository;
            _WarehouseInputDetailRepository = warehouseInputDetailRepository;
            _WarehouseOutputDetailRepository = warehouseOutputDetailRepository;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseOutputDetailBarcodeRepository = WarehouseOutputDetailBarcodeRepository;
            _InvoiceInputRepository = InvoiceInputRepository;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
            _MaterialRepository = materialRepository;
            _ProductionOrderRepository = productionOrderRepository;
        }
        public override void Initialization(WarehouseInventory model)
        {
            BaseInitialization(model);

            if (model.ParentID > 0)
            {
                var Parent = _MaterialRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Name;
                model.Code = Parent.Code;
                model.FileName = model.FileName ?? Parent.CategoryLineName;
            }
            //if (!string.IsNullOrEmpty(model.Code))
            //{
            //    var Material = _MaterialRepository.GetByDescription(model.Code, model.CompanyID);
            //    model.ParentID = Material.ID;
            //}
            if (model.CategoryDepartmentID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.CategoryDepartmentID.Value);
                model.CategoryDepartmentName = CategoryDepartment.Name;
                model.CompanyID = CategoryDepartment.CompanyID;
            }
            model.CategoryDepartmentID = model.CategoryDepartmentID ?? 0;
            if (model.CompanyID == null || model.CompanyID == 0)
            {
                model.CompanyID = GlobalHelper.CompanyID;
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

            model.QuantityBegin = model.QuantityBegin ?? 0;
            var Month = model.Month;
            var Year = model.Year;
            var WarehouseInventory = new WarehouseInventory();
            var ListWarehouseInventory = new List<WarehouseInventory>();
            bool IsQuantityBeginCheck = true;
            switch (model.Action)
            {
                case 2:
                    if (model.QuantityBegin == 0)
                    {
                        WarehouseInventory = _WarehouseInventoryRepository.GetByCondition(o => o.Year < Year && o.Action == model.Action && o.CategoryDepartmentID == model.CategoryDepartmentID && o.ParentID == model.ParentID).OrderByDescending(o => o.Year).FirstOrDefault();
                        if (WarehouseInventory != null && WarehouseInventory.ID > 0)
                        {
                            model.QuantityBegin = WarehouseInventory.QuantityEnd;
                        }
                        if (WarehouseInventory == null)
                        {
                            IsQuantityBeginCheck = false;
                        }
                    }
                    break;
                case 1:
                    if (model.QuantityBegin == 0)
                    {
                        WarehouseInventory = _WarehouseInventoryRepository.GetByCondition(o => ((o.Year < Year) || (o.Year == Year && o.Month < Month)) && o.Action == model.Action && o.CategoryDepartmentID == model.CategoryDepartmentID && o.ParentID == model.ParentID).OrderByDescending(o => o.Year).OrderByDescending(o => o.Month).FirstOrDefault();
                        if (WarehouseInventory != null && WarehouseInventory.ID > 0)
                        {
                            model.QuantityBegin = WarehouseInventory.QuantityEnd;
                        }
                        if (WarehouseInventory == null)
                        {
                            IsQuantityBeginCheck = false;
                        }
                    }
                    break;
                case 20:
                    if (model.QuantityBegin == 0)
                    {
                        WarehouseInventory = _WarehouseInventoryRepository.GetByCondition(o => o.Year < Year && o.Action == model.Action && o.CompanyID == model.CompanyID && o.ParentID == model.ParentID).OrderByDescending(o => o.Year).FirstOrDefault();
                        if (WarehouseInventory != null && WarehouseInventory.ID > 0)
                        {
                            model.QuantityBegin = WarehouseInventory.QuantityEnd;
                        }
                        if (WarehouseInventory == null)
                        {
                            IsQuantityBeginCheck = false;
                        }
                    }
                    break;
                case 10:
                    if (model.QuantityBegin == 0)
                    {
                        WarehouseInventory = _WarehouseInventoryRepository.GetByCondition(o => ((o.Year < Year) || (o.Year == Year && o.Month < Month)) && o.Month == Month && o.Action == model.Action && o.CompanyID == model.CompanyID && o.ParentID == model.ParentID).OrderByDescending(o => o.Year).OrderByDescending(o => o.Month).FirstOrDefault();
                        if (WarehouseInventory != null && WarehouseInventory.ID > 0)
                        {
                            model.QuantityBegin = WarehouseInventory.QuantityEnd;
                        }
                        if (WarehouseInventory == null)
                        {
                            IsQuantityBeginCheck = false;
                        }
                    }
                    break;
            }
            if (IsQuantityBeginCheck == false || model.Action == 3)
            {
                var ListWarehouseInput = _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.IsStock == true && o.CustomerID == model.CategoryDepartmentID).OrderBy(o => o.Date).ToList();
                if (ListWarehouseInput.Count > 0)
                {
                    var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).Distinct().ToList();
                    var ListWarehouseInputDetailBarcodeStock = _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.MaterialID == model.ParentID && o.Active == true && o.IsStock == true && ListWarehouseInputID.Contains(o.ParentID ?? 0)).OrderBy(o => o.MaterialID).ToList();
                    if (ListWarehouseInputDetailBarcodeStock.Count > 0)
                    {
                        model.QuantityBegin = ListWarehouseInputDetailBarcodeStock.Sum(o => o.QuantitySNP);
                    }
                }
            }

            model.QuantityBegin01 = model.QuantityBegin;

            model.QuantityEnd01 = model.QuantityBegin01 + model.Quantity01;
            model.QuantityBegin02 = model.QuantityEnd01;
            model.QuantityEnd02 = model.QuantityBegin02 + model.Quantity02;
            model.QuantityBegin03 = model.QuantityEnd02;
            model.QuantityEnd03 = model.QuantityBegin03 + model.Quantity03;
            model.QuantityBegin04 = model.QuantityEnd03;
            model.QuantityEnd04 = model.QuantityBegin04 + model.Quantity04;
            model.QuantityBegin05 = model.QuantityEnd04;
            model.QuantityEnd05 = model.QuantityBegin05 + model.Quantity05;
            model.QuantityBegin06 = model.QuantityEnd05;
            model.QuantityEnd06 = model.QuantityBegin06 + model.Quantity06;
            model.QuantityBegin07 = model.QuantityEnd06;
            model.QuantityEnd07 = model.QuantityBegin07 + model.Quantity07;
            model.QuantityBegin08 = model.QuantityEnd07;
            model.QuantityEnd08 = model.QuantityBegin08 + model.Quantity08;
            model.QuantityBegin09 = model.QuantityEnd08;
            model.QuantityEnd09 = model.QuantityBegin09 + model.Quantity09;
            model.QuantityBegin10 = model.QuantityEnd09;
            model.QuantityEnd10 = model.QuantityBegin10 + model.Quantity10;
            model.QuantityBegin11 = model.QuantityEnd10;
            model.QuantityEnd11 = model.QuantityBegin11 + model.Quantity11;
            model.QuantityBegin12 = model.QuantityEnd11;
            model.QuantityEnd12 = model.QuantityBegin12 + model.Quantity12;
            model.QuantityBegin13 = model.QuantityEnd12;
            model.QuantityEnd13 = model.QuantityBegin13 + model.Quantity13;
            model.QuantityBegin14 = model.QuantityEnd13;
            model.QuantityEnd14 = model.QuantityBegin14 + model.Quantity14;
            model.QuantityBegin15 = model.QuantityEnd14;
            model.QuantityEnd15 = model.QuantityBegin15 + model.Quantity15;
            model.QuantityBegin16 = model.QuantityEnd15;
            model.QuantityEnd16 = model.QuantityBegin16 + model.Quantity16;
            model.QuantityBegin17 = model.QuantityEnd16;
            model.QuantityEnd17 = model.QuantityBegin17 + model.Quantity17;
            model.QuantityBegin18 = model.QuantityEnd17;
            model.QuantityEnd18 = model.QuantityBegin18 + model.Quantity18;
            model.QuantityBegin19 = model.QuantityEnd18;
            model.QuantityEnd19 = model.QuantityBegin19 + model.Quantity19;
            model.QuantityBegin20 = model.QuantityEnd19;
            model.QuantityEnd20 = model.QuantityBegin20 + model.Quantity20;
            model.QuantityBegin21 = model.QuantityEnd20;
            model.QuantityEnd21 = model.QuantityBegin21 + model.Quantity21;
            model.QuantityBegin22 = model.QuantityEnd21;
            model.QuantityEnd22 = model.QuantityBegin22 + model.Quantity22;
            model.QuantityBegin23 = model.QuantityEnd22;
            model.QuantityEnd23 = model.QuantityBegin23 + model.Quantity23;
            model.QuantityBegin24 = model.QuantityEnd23;
            model.QuantityEnd24 = model.QuantityBegin24 + model.Quantity24;
            model.QuantityBegin25 = model.QuantityEnd24;
            model.QuantityEnd25 = model.QuantityBegin25 + model.Quantity25;
            model.QuantityBegin26 = model.QuantityEnd25;
            model.QuantityEnd26 = model.QuantityBegin26 + model.Quantity26;
            model.QuantityBegin27 = model.QuantityEnd26;
            model.QuantityEnd27 = model.QuantityBegin27 + model.Quantity27;
            model.QuantityBegin28 = model.QuantityEnd27;
            model.QuantityEnd28 = model.QuantityBegin28 + model.Quantity28;
            model.QuantityBegin29 = model.QuantityEnd28;
            model.QuantityEnd29 = model.QuantityBegin29 + model.Quantity29;
            model.QuantityBegin30 = model.QuantityEnd29;
            model.QuantityEnd30 = model.QuantityBegin30 + model.Quantity30;
            model.QuantityBegin31 = model.QuantityEnd30;
            model.QuantityEnd31 = model.QuantityBegin31 + model.Quantity31;

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

            model.QuantityStock = model.QuantityStock ?? 0;
            if (model.QuantityStock > 0)
            {
                model.QuantityEnd = model.QuantityStock + model.Quantity00;
            }
            else
            {
                model.QuantityEnd = model.QuantityBegin + model.Quantity00;
            }
            model.Action = model.Action ?? 1;
        }
        public override async Task<BaseResult<WarehouseInventory>> SaveAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            //Initialization(BaseParameter.BaseModel);
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                var ModelCheck = new WarehouseInventory();
                var List = await GetByCondition(o => o.Action == BaseParameter.BaseModel.Action && o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID && o.ParentID == BaseParameter.BaseModel.ParentID && o.Year == BaseParameter.BaseModel.Year && o.Month == BaseParameter.BaseModel.Month).OrderByDescending(o => o.UpdateDate).ToListAsync();
                if (List.Count > 0)
                {
                    ModelCheck = List[0];
                    for (int i = 1; i < List.Count; i++)
                    {
                        await _WarehouseInventoryRepository.RemoveAsync(List[i].ID);
                    }
                }
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
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
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            //await SyncYear_MonthAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncYear_MonthAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter != null)
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.Active == true)
                        {
                            var WarehouseInventory = new WarehouseInventory();
                            var List = new List<WarehouseInventory>();
                            switch (BaseParameter.BaseModel.Action)
                            {
                                case 1:
                                    WarehouseInventory = await GetByCondition(o => o.Action == 2 && o.ParentID == BaseParameter.BaseModel.ParentID && o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID && o.Year == BaseParameter.BaseModel.Year).FirstOrDefaultAsync();
                                    if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                    {
                                        WarehouseInventory = new WarehouseInventory();
                                    }
                                    WarehouseInventory = new WarehouseInventory();
                                    WarehouseInventory.ParentID = BaseParameter.BaseModel.ParentID;
                                    WarehouseInventory.CompanyID = BaseParameter.BaseModel.CompanyID;
                                    WarehouseInventory.CategoryDepartmentID = BaseParameter.BaseModel.CategoryDepartmentID;
                                    WarehouseInventory.Action = 2;
                                    WarehouseInventory.Active = true;
                                    WarehouseInventory.Year = BaseParameter.BaseModel.Year;
                                    WarehouseInventory.Month = GlobalHelper.InitializationNumber;
                                    WarehouseInventory.Day = GlobalHelper.InitializationNumber;

                                    List = await GetByCondition(o => o.Action == 1 && o.ParentID == BaseParameter.BaseModel.ParentID && o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID && o.Year == BaseParameter.BaseModel.Year && o.Month == BaseParameter.BaseModel.Month).ToListAsync();
                                    WarehouseInventory.QuantityInput00 = List.Sum(x => x.QuantityInput00);
                                    WarehouseInventory.QuantityOutput00 = List.Sum(x => x.QuantityOutput00);
                                    WarehouseInventory.FileName = string.Join(" | ", List.Select(o => o.FileName).Distinct().ToList());
                                    switch (BaseParameter.BaseModel.Month)
                                    {
                                        case 1:
                                            WarehouseInventory.QuantityInput01 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput01 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 2:
                                            WarehouseInventory.QuantityInput02 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput02 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 3:
                                            WarehouseInventory.QuantityInput03 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput03 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 4:
                                            WarehouseInventory.QuantityInput04 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput04 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 5:
                                            WarehouseInventory.QuantityInput05 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput05 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 6:
                                            WarehouseInventory.QuantityInput06 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput06 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 7:
                                            WarehouseInventory.QuantityInput07 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput07 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 8:
                                            WarehouseInventory.QuantityInput08 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput08 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 9:
                                            WarehouseInventory.QuantityInput09 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput09 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 10:
                                            WarehouseInventory.QuantityInput10 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput10 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 11:
                                            WarehouseInventory.QuantityInput11 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput11 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 12:
                                            WarehouseInventory.QuantityInput12 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput12 = WarehouseInventory.QuantityOutput00;
                                            break;
                                    }
                                    BaseParameter.BaseModel = WarehouseInventory;
                                    await SaveAsync(BaseParameter);
                                    break;
                                case 2:
                                    WarehouseInventory = await GetByCondition(o => o.Action == 3 && o.ParentID == BaseParameter.BaseModel.ParentID && o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID).FirstOrDefaultAsync();
                                    if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                    {
                                        WarehouseInventory = new WarehouseInventory();
                                    }
                                    WarehouseInventory = new WarehouseInventory();
                                    WarehouseInventory.ParentID = BaseParameter.BaseModel.ParentID;
                                    WarehouseInventory.CompanyID = BaseParameter.BaseModel.CompanyID;
                                    WarehouseInventory.CategoryDepartmentID = BaseParameter.BaseModel.CategoryDepartmentID;
                                    WarehouseInventory.Action = 3;
                                    WarehouseInventory.Active = true;
                                    WarehouseInventory.Year = GlobalHelper.InitializationNumber;
                                    WarehouseInventory.Month = GlobalHelper.InitializationNumber;
                                    WarehouseInventory.Day = GlobalHelper.InitializationNumber;

                                    List = await GetByCondition(o => o.Action == 2 && o.ParentID == BaseParameter.BaseModel.ParentID && o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID && o.Year == BaseParameter.BaseModel.Year).ToListAsync();
                                    WarehouseInventory.QuantityInput00 = List.Sum(x => x.QuantityInput00);
                                    WarehouseInventory.QuantityOutput00 = List.Sum(x => x.QuantityOutput00);
                                    WarehouseInventory.FileName = string.Join(" | ", List.Select(o => o.FileName).Distinct().ToList());
                                    switch (BaseParameter.BaseModel.Year)
                                    {
                                        case 2026:
                                            WarehouseInventory.QuantityInput01 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput01 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 2027:
                                            WarehouseInventory.QuantityInput02 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput02 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 2028:
                                            WarehouseInventory.QuantityInput03 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput03 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 2029:
                                            WarehouseInventory.QuantityInput04 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput04 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 2030:
                                            WarehouseInventory.QuantityInput05 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput05 = WarehouseInventory.QuantityOutput00;
                                            break;
                                    }
                                    BaseParameter.BaseModel = WarehouseInventory;
                                    await SaveAsync(BaseParameter);
                                    break;
                                case 10:
                                    WarehouseInventory = await GetByCondition(o => o.Action == 20 && o.ParentID == BaseParameter.BaseModel.ParentID && o.CompanyID == BaseParameter.BaseModel.CompanyID && o.Year == BaseParameter.BaseModel.Year).FirstOrDefaultAsync();
                                    if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                    {
                                        WarehouseInventory = new WarehouseInventory();
                                    }
                                    WarehouseInventory = new WarehouseInventory();
                                    WarehouseInventory.ParentID = BaseParameter.BaseModel.ParentID;
                                    WarehouseInventory.CompanyID = BaseParameter.BaseModel.CompanyID;
                                    WarehouseInventory.Action = 2;
                                    WarehouseInventory.Active = true;
                                    WarehouseInventory.Year = BaseParameter.BaseModel.Year;
                                    WarehouseInventory.Month = GlobalHelper.InitializationNumber;
                                    WarehouseInventory.Day = GlobalHelper.InitializationNumber;

                                    List = await GetByCondition(o => o.Action == 10 && o.ParentID == BaseParameter.BaseModel.ParentID && o.CompanyID == BaseParameter.BaseModel.CompanyID && o.Year == BaseParameter.BaseModel.Year && o.Month == BaseParameter.BaseModel.Month).ToListAsync();
                                    WarehouseInventory.QuantityInput00 = List.Sum(x => x.QuantityInput00);
                                    WarehouseInventory.QuantityOutput00 = List.Sum(x => x.QuantityOutput00);
                                    WarehouseInventory.FileName = string.Join(" | ", List.Select(o => o.FileName).Distinct().ToList());
                                    switch (BaseParameter.BaseModel.Month)
                                    {
                                        case 1:
                                            WarehouseInventory.QuantityInput01 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput01 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 2:
                                            WarehouseInventory.QuantityInput02 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput02 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 3:
                                            WarehouseInventory.QuantityInput03 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput03 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 4:
                                            WarehouseInventory.QuantityInput04 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput04 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 5:
                                            WarehouseInventory.QuantityInput05 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput05 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 6:
                                            WarehouseInventory.QuantityInput06 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput06 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 7:
                                            WarehouseInventory.QuantityInput07 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput07 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 8:
                                            WarehouseInventory.QuantityInput08 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput08 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 9:
                                            WarehouseInventory.QuantityInput09 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput09 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 10:
                                            WarehouseInventory.QuantityInput10 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput10 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 11:
                                            WarehouseInventory.QuantityInput11 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput11 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 12:
                                            WarehouseInventory.QuantityInput12 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput12 = WarehouseInventory.QuantityOutput00;
                                            break;
                                    }
                                    BaseParameter.BaseModel = WarehouseInventory;
                                    await SaveAsync(BaseParameter);
                                    break;
                                case 20:
                                    WarehouseInventory = await GetByCondition(o => o.Action == 30 && o.ParentID == BaseParameter.BaseModel.ParentID && o.CompanyID == BaseParameter.BaseModel.CompanyID).FirstOrDefaultAsync();
                                    if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                    {
                                        WarehouseInventory = new WarehouseInventory();
                                    }
                                    WarehouseInventory = new WarehouseInventory();
                                    WarehouseInventory.ParentID = BaseParameter.BaseModel.ParentID;
                                    WarehouseInventory.CompanyID = BaseParameter.BaseModel.CompanyID;
                                    WarehouseInventory.Action = 30;
                                    WarehouseInventory.Active = true;
                                    WarehouseInventory.Year = GlobalHelper.InitializationNumber;
                                    WarehouseInventory.Month = GlobalHelper.InitializationNumber;
                                    WarehouseInventory.Day = GlobalHelper.InitializationNumber;

                                    List = await GetByCondition(o => o.Action == 20 && o.ParentID == BaseParameter.BaseModel.ParentID && o.CompanyID == BaseParameter.BaseModel.CompanyID && o.Year == BaseParameter.BaseModel.Year).ToListAsync();
                                    WarehouseInventory.QuantityInput00 = List.Sum(x => x.QuantityInput00);
                                    WarehouseInventory.QuantityOutput00 = List.Sum(x => x.QuantityOutput00);
                                    WarehouseInventory.FileName = string.Join(" | ", List.Select(o => o.FileName).Distinct().ToList());
                                    switch (BaseParameter.BaseModel.Year)
                                    {
                                        case 2026:
                                            WarehouseInventory.QuantityInput01 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput01 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 2027:
                                            WarehouseInventory.QuantityInput02 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput02 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 2028:
                                            WarehouseInventory.QuantityInput03 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput03 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 2029:
                                            WarehouseInventory.QuantityInput04 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput04 = WarehouseInventory.QuantityOutput00;
                                            break;
                                        case 2030:
                                            WarehouseInventory.QuantityInput05 = WarehouseInventory.QuantityInput00;
                                            WarehouseInventory.QuantityOutput05 = WarehouseInventory.QuantityOutput00;
                                            break;
                                    }
                                    BaseParameter.BaseModel = WarehouseInventory;
                                    await SaveAsync(BaseParameter);
                                    break;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncByWarehouseInputDetailBarcodeAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByIDAsync(BaseParameter.ID);
                    if (WarehouseInputDetailBarcode != null)
                    {
                        if (WarehouseInputDetailBarcode.ID > 0)
                        {
                            if (WarehouseInputDetailBarcode.ParentID > 0)
                            {
                                if (WarehouseInputDetailBarcode.MaterialID > 0)
                                {
                                    if (WarehouseInputDetailBarcode.DateScan != null)
                                    {
                                        var WarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.ID == WarehouseInputDetailBarcode.ParentID.Value && o.Active == true && o.CustomerID > 0).FirstOrDefaultAsync();
                                        if (WarehouseInput != null && WarehouseInput.ID > 0 && WarehouseInput.Active == true && WarehouseInput.CustomerID > 0)
                                        {
                                            var WarehouseInventory = new WarehouseInventory();
                                            WarehouseInventory.ParentID = WarehouseInputDetailBarcode.MaterialID;
                                            WarehouseInventory.CategoryDepartmentID = WarehouseInput.CustomerID;
                                            WarehouseInventory.CompanyID = WarehouseInput.CompanyID;
                                            WarehouseInventory.Year = WarehouseInputDetailBarcode.DateScan.Value.Year;
                                            WarehouseInventory.Month = WarehouseInputDetailBarcode.DateScan.Value.Month;
                                            WarehouseInventory.Day = WarehouseInputDetailBarcode.DateScan.Value.Day;
                                            WarehouseInventory.QuantityInput00 = 0;
                                            WarehouseInventory.Active = true;
                                            WarehouseInventory.Action = 1;

                                            WarehouseInventory = await GetByCondition(o => o.Action == WarehouseInventory.Action && o.CategoryDepartmentID == WarehouseInventory.CategoryDepartmentID && o.ParentID == WarehouseInventory.ParentID && o.Year == WarehouseInventory.Year && o.Month == WarehouseInventory.Month).FirstOrDefaultAsync();
                                            if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                            {
                                                WarehouseInventory = new WarehouseInventory();
                                            }

                                            WarehouseInventory.ParentID = WarehouseInputDetailBarcode.MaterialID;
                                            WarehouseInventory.CategoryDepartmentID = WarehouseInput.CustomerID;
                                            WarehouseInventory.CompanyID = WarehouseInput.CompanyID;
                                            WarehouseInventory.Year = WarehouseInputDetailBarcode.DateScan.Value.Year;
                                            WarehouseInventory.Month = WarehouseInputDetailBarcode.DateScan.Value.Month;
                                            WarehouseInventory.Day = WarehouseInputDetailBarcode.DateScan.Value.Day;
                                            WarehouseInventory.QuantityInput00 = 0;
                                            WarehouseInventory.Active = true;
                                            WarehouseInventory.Action = 1;

                                            var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == WarehouseInventory.CategoryDepartmentID && o.MaterialID == WarehouseInventory.ParentID && o.Active == true && o.DateScan != null && o.DateScan.Value.Month == WarehouseInputDetailBarcode.DateScan.Value.Month).ToListAsync();
                                            if (ListWarehouseInputDetailBarcode.Count > 0)
                                            {
                                                WarehouseInventory.QuantityInput01 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 1).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput02 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 2).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput03 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 3).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput04 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 4).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput05 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 5).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput06 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 6).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput07 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 7).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput08 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 8).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput09 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 9).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput10 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 10).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput11 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 11).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput12 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 12).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput13 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 13).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput14 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 14).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput15 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 15).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput16 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 16).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput17 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 17).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput18 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 18).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput19 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 19).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput20 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 20).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput21 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 21).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput22 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 22).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput23 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 23).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput24 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 24).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput25 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 25).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput26 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 26).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput27 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 27).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput28 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 28).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput29 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 29).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput30 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 30).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput31 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 31).ToList().Sum(o => o.Quantity);
                                            }

                                            var BaseParameterWarehouseInventory = new BaseParameter<WarehouseInventory>();
                                            BaseParameterWarehouseInventory.BaseModel = WarehouseInventory;
                                            await SaveAsync(BaseParameterWarehouseInventory);

                                            //await SaveFromSyncAsync(WarehouseInventory);

                                            //if (WarehouseInput.InvoiceInputID > 0 && WarehouseInput.CustomerID > 0)
                                            //{
                                            //    var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.ID == WarehouseInput.CustomerID.Value && o.IsSync == true).FirstOrDefaultAsync();
                                            //    if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                                            //    {
                                            //        var InvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.ID == WarehouseInput.InvoiceInputID.Value && o.Active == true).FirstOrDefaultAsync();
                                            //        if (InvoiceInput != null && InvoiceInput.ID > 0 && InvoiceInput.Active == true)
                                            //        {
                                            //            WarehouseInventory = new WarehouseInventory();
                                            //            WarehouseInventory.ParentID = WarehouseInputDetailBarcode.MaterialID;
                                            //            WarehouseInventory.CompanyID = InvoiceInput.SupplierID;
                                            //            WarehouseInventory.Year = WarehouseInputDetailBarcode.DateScan.Value.Year;
                                            //            WarehouseInventory.Month = WarehouseInputDetailBarcode.DateScan.Value.Month;
                                            //            WarehouseInventory.Day = WarehouseInputDetailBarcode.DateScan.Value.Day;
                                            //            WarehouseInventory.QuantityInput00 = 0;
                                            //            WarehouseInventory.Active = true;
                                            //            WarehouseInventory.Action = 10;

                                            //            WarehouseInventory = await GetByCondition(o => o.Action == WarehouseInventory.Action && o.CompanyID == WarehouseInventory.CompanyID && o.ParentID == WarehouseInventory.ParentID && o.Year == WarehouseInventory.Year && o.Month == WarehouseInventory.Month).FirstOrDefaultAsync();
                                            //            if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                            //            {
                                            //                WarehouseInventory = new WarehouseInventory();
                                            //            }

                                            //            WarehouseInventory.ParentID = WarehouseInputDetailBarcode.MaterialID;
                                            //            WarehouseInventory.CompanyID = InvoiceInput.SupplierID;
                                            //            WarehouseInventory.Year = WarehouseInputDetailBarcode.DateScan.Value.Year;
                                            //            WarehouseInventory.Month = WarehouseInputDetailBarcode.DateScan.Value.Month;
                                            //            WarehouseInventory.Day = WarehouseInputDetailBarcode.DateScan.Value.Day;
                                            //            WarehouseInventory.QuantityInput00 = 0;
                                            //            WarehouseInventory.Active = true;
                                            //            WarehouseInventory.Action = 10;

                                            //            var ListInvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.Active == true && o.SupplierID == WarehouseInventory.CompanyID).ToListAsync();
                                            //            if (ListInvoiceInput.Count > 0)
                                            //            {
                                            //                var ListInvoiceInputID = ListInvoiceInput.Select(o => o.ID).ToList();
                                            //                ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.InvoiceInputID > 0 && ListInvoiceInputID.Contains(o.InvoiceInputID.Value)).ToListAsync();
                                            //                if (ListWarehouseInput.Count > 0)
                                            //                {
                                            //                    var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                            //                    var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.MaterialID == WarehouseInventory.ParentID && o.Active == true && o.DateScan != null && o.DateScan.Value.Date == WarehouseInputDetailBarcode.DateScan.Value.Date).ToListAsync();
                                            //                    if (ListWarehouseInputDetailBarcode.Count > 0)
                                            //                    {
                                            //                        WarehouseInventory.QuantityInput00 = ListWarehouseInputDetailBarcode.Sum(x => x.Quantity);
                                            //                    }
                                            //                }
                                            //            }
                                            //            await SaveFromSyncAsync(WarehouseInventory);
                                            //        }
                                            //    }
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncByWarehouseInputDetailBarcodeFromBaseParameterAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter != null)
            {
                var WarehouseInputDetailBarcode = BaseParameter.BaseModel;
                if (WarehouseInputDetailBarcode != null)
                {
                    if (WarehouseInputDetailBarcode.ID > 0)
                    {
                        if (WarehouseInputDetailBarcode.ParentID > 0)
                        {
                            if (WarehouseInputDetailBarcode.MaterialID > 0)
                            {
                                if (WarehouseInputDetailBarcode.DateScan != null)
                                {
                                    var WarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.ID == WarehouseInputDetailBarcode.ParentID.Value && o.Active == true && o.CustomerID > 0).FirstOrDefaultAsync();
                                    if (WarehouseInput != null && WarehouseInput.ID > 0 && WarehouseInput.Active == true && WarehouseInput.CustomerID > 0)
                                    {
                                        var WarehouseInventory = new WarehouseInventory();
                                        WarehouseInventory.ParentID = WarehouseInputDetailBarcode.MaterialID;
                                        WarehouseInventory.CategoryDepartmentID = WarehouseInput.CustomerID;
                                        WarehouseInventory.CompanyID = WarehouseInput.CompanyID;
                                        WarehouseInventory.Year = WarehouseInputDetailBarcode.DateScan.Value.Year;
                                        WarehouseInventory.Month = WarehouseInputDetailBarcode.DateScan.Value.Month;
                                        WarehouseInventory.Day = WarehouseInputDetailBarcode.DateScan.Value.Day;
                                        WarehouseInventory.QuantityInput00 = 0;
                                        WarehouseInventory.Active = true;
                                        WarehouseInventory.Action = 1;

                                        WarehouseInventory = await GetByCondition(o => o.Action == WarehouseInventory.Action && o.CategoryDepartmentID == WarehouseInventory.CategoryDepartmentID && o.ParentID == WarehouseInventory.ParentID && o.Year == WarehouseInventory.Year && o.Month == WarehouseInventory.Month).FirstOrDefaultAsync();
                                        if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                        {
                                            WarehouseInventory = new WarehouseInventory();
                                        }

                                        WarehouseInventory.ParentID = WarehouseInputDetailBarcode.MaterialID;
                                        WarehouseInventory.CategoryDepartmentID = WarehouseInput.CustomerID;
                                        WarehouseInventory.CompanyID = WarehouseInput.CompanyID;
                                        WarehouseInventory.Year = WarehouseInputDetailBarcode.DateScan.Value.Year;
                                        WarehouseInventory.Month = WarehouseInputDetailBarcode.DateScan.Value.Month;
                                        WarehouseInventory.Day = WarehouseInputDetailBarcode.DateScan.Value.Day;
                                        WarehouseInventory.QuantityInput00 = 0;
                                        WarehouseInventory.Active = true;
                                        WarehouseInventory.Action = 1;

                                        var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == WarehouseInventory.CategoryDepartmentID).ToListAsync();
                                        if (ListWarehouseInput.Count > 0)
                                        {
                                            var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                            var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.MaterialID == WarehouseInventory.ParentID && o.Active == true && o.DateScan != null && o.DateScan.Value.Date == WarehouseInputDetailBarcode.DateScan.Value.Date).ToListAsync();
                                            if (ListWarehouseInputDetailBarcode.Count > 0)
                                            {
                                                WarehouseInventory.QuantityInput01 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 1).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput02 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 2).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput03 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 3).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput04 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 4).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput05 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 5).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput06 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 6).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput07 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 7).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput08 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 8).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput09 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 9).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput10 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 10).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput11 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 11).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput12 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 12).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput13 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 13).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput14 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 14).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput15 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 15).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput16 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 16).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput17 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 17).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput18 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 18).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput19 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 19).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput20 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 20).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput21 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 21).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput22 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 22).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput23 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 23).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput24 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 24).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput25 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 25).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput26 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 26).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput27 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 27).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput28 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 28).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput29 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 29).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput30 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 30).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityInput31 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 31).ToList().Sum(o => o.Quantity);
                                            }
                                        }
                                        var BaseParameterWarehouseInventory = new BaseParameter<WarehouseInventory>();
                                        BaseParameterWarehouseInventory.BaseModel = WarehouseInventory;
                                        await SaveAsync(BaseParameterWarehouseInventory);

                                        //if (WarehouseInput.InvoiceInputID > 0 && WarehouseInput.CustomerID > 0)
                                        //{
                                        //    var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.ID == WarehouseInput.CustomerID.Value && o.IsSync == true).FirstOrDefaultAsync();
                                        //    if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                                        //    {
                                        //        var InvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.ID == WarehouseInput.InvoiceInputID.Value && o.Active == true).FirstOrDefaultAsync();
                                        //        if (InvoiceInput != null && InvoiceInput.ID > 0 && InvoiceInput.Active == true)
                                        //        {
                                        //            WarehouseInventory = new WarehouseInventory();
                                        //            WarehouseInventory.ParentID = WarehouseInputDetailBarcode.MaterialID;
                                        //            WarehouseInventory.CompanyID = InvoiceInput.SupplierID;
                                        //            WarehouseInventory.Year = WarehouseInputDetailBarcode.DateScan.Value.Year;
                                        //            WarehouseInventory.Month = WarehouseInputDetailBarcode.DateScan.Value.Month;
                                        //            WarehouseInventory.Day = WarehouseInputDetailBarcode.DateScan.Value.Day;
                                        //            WarehouseInventory.QuantityInput00 = 0;
                                        //            WarehouseInventory.Active = true;
                                        //            WarehouseInventory.Action = 10;

                                        //            WarehouseInventory = await GetByCondition(o => o.Action == WarehouseInventory.Action && o.CompanyID == WarehouseInventory.CompanyID && o.ParentID == WarehouseInventory.ParentID && o.Year == WarehouseInventory.Year && o.Month == WarehouseInventory.Month).FirstOrDefaultAsync();
                                        //            if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                        //            {
                                        //                WarehouseInventory = new WarehouseInventory();
                                        //            }

                                        //            WarehouseInventory.ParentID = WarehouseInputDetailBarcode.MaterialID;
                                        //            WarehouseInventory.CompanyID = InvoiceInput.SupplierID;
                                        //            WarehouseInventory.Year = WarehouseInputDetailBarcode.DateScan.Value.Year;
                                        //            WarehouseInventory.Month = WarehouseInputDetailBarcode.DateScan.Value.Month;
                                        //            WarehouseInventory.Day = WarehouseInputDetailBarcode.DateScan.Value.Day;
                                        //            WarehouseInventory.QuantityInput00 = 0;
                                        //            WarehouseInventory.Active = true;
                                        //            WarehouseInventory.Action = 10;

                                        //            var ListInvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.Active == true && o.SupplierID == WarehouseInventory.CompanyID).ToListAsync();
                                        //            if (ListInvoiceInput.Count > 0)
                                        //            {
                                        //                var ListInvoiceInputID = ListInvoiceInput.Select(o => o.ID).ToList();
                                        //                ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.InvoiceInputID > 0 && ListInvoiceInputID.Contains(o.InvoiceInputID.Value)).ToListAsync();
                                        //                if (ListWarehouseInput.Count > 0)
                                        //                {
                                        //                    var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                        //                    var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.MaterialID == WarehouseInventory.ParentID && o.Active == true && o.DateScan != null && o.DateScan.Value.Date == WarehouseInputDetailBarcode.DateScan.Value.Date).ToListAsync();
                                        //                    if (ListWarehouseInputDetailBarcode.Count > 0)
                                        //                    {
                                        //                        WarehouseInventory.QuantityInput00 = ListWarehouseInputDetailBarcode.Sum(x => x.Quantity);
                                        //                    }
                                        //                }
                                        //            }
                                        //            await SaveFromSyncAsync(WarehouseInventory);
                                        //        }
                                        //    }
                                        //}
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncByWarehouseOutputDetailBarcodeAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    var WarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByIDAsync(BaseParameter.ID);
                    if (WarehouseOutputDetailBarcode != null)
                    {
                        if (WarehouseOutputDetailBarcode.ID > 0)
                        {
                            if (WarehouseOutputDetailBarcode.ParentID > 0)
                            {
                                if (WarehouseOutputDetailBarcode.MaterialID > 0)
                                {
                                    if (WarehouseOutputDetailBarcode.UpdateDate != null)
                                    {
                                        var WarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.ID == WarehouseOutputDetailBarcode.ParentID.Value && o.Active == true && o.SupplierID > 0).FirstOrDefaultAsync();
                                        if (WarehouseOutput != null && WarehouseOutput.ID > 0 && WarehouseOutput.Active == true && WarehouseOutput.SupplierID > 0)
                                        {
                                            var WarehouseInventory = new WarehouseInventory();
                                            WarehouseInventory.ParentID = WarehouseOutputDetailBarcode.MaterialID;
                                            WarehouseInventory.CategoryDepartmentID = WarehouseOutput.SupplierID;
                                            WarehouseInventory.CompanyID = WarehouseOutput.CompanyID;
                                            WarehouseInventory.Year = WarehouseOutputDetailBarcode.UpdateDate.Value.Year;
                                            WarehouseInventory.Month = WarehouseOutputDetailBarcode.UpdateDate.Value.Month;
                                            WarehouseInventory.Day = WarehouseOutputDetailBarcode.UpdateDate.Value.Day;
                                            WarehouseInventory.QuantityOutput00 = 0;
                                            WarehouseInventory.Active = true;
                                            WarehouseInventory.Action = 1;

                                            WarehouseInventory = await GetByCondition(o => o.Action == 1 && o.CategoryDepartmentID == WarehouseInventory.CategoryDepartmentID && o.ParentID == WarehouseInventory.ParentID && o.Year == WarehouseInventory.Year && o.Month == WarehouseInventory.Month).FirstOrDefaultAsync();
                                            if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                            {
                                                WarehouseInventory = new WarehouseInventory();
                                            }
                                            WarehouseInventory.ParentID = WarehouseOutputDetailBarcode.MaterialID;
                                            WarehouseInventory.CategoryDepartmentID = WarehouseOutput.SupplierID;
                                            WarehouseInventory.Year = WarehouseOutputDetailBarcode.UpdateDate.Value.Year;
                                            WarehouseInventory.Month = WarehouseOutputDetailBarcode.UpdateDate.Value.Month;
                                            WarehouseInventory.Day = WarehouseOutputDetailBarcode.UpdateDate.Value.Day;
                                            WarehouseInventory.QuantityOutput00 = 0;
                                            WarehouseInventory.Active = true;
                                            WarehouseInventory.Action = 1;

                                            var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == WarehouseInventory.CategoryDepartmentID && o.MaterialID == WarehouseInventory.ParentID && o.Active == true && o.UpdateDate != null && o.UpdateDate.Value.Date == WarehouseOutputDetailBarcode.UpdateDate.Value.Date).ToListAsync();
                                            if (ListWarehouseOutputDetailBarcode.Count > 0)
                                            {
                                                WarehouseInventory.QuantityOutput01 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 1).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput02 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 2).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput03 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 3).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput04 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 4).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput05 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 5).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput06 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 6).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput07 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 7).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput08 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 8).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput09 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 9).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput10 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 10).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput11 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 11).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput12 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 12).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput13 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 13).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput14 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 14).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput15 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 15).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput16 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 16).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput17 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 17).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput18 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 18).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput19 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 19).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput20 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 20).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput21 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 21).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput22 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 22).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput23 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 23).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput24 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 24).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput25 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 25).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput26 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 26).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput27 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 27).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput28 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 28).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput29 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 29).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput30 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 30).ToList().Sum(o => o.Quantity);
                                                WarehouseInventory.QuantityOutput31 = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null && o.UpdateDate.Value.Day == 31).ToList().Sum(o => o.Quantity);
                                            }

                                            var BaseParameterWarehouseInventory = new BaseParameter<WarehouseInventory>();
                                            BaseParameterWarehouseInventory.BaseModel = WarehouseInventory;
                                            await SaveAsync(BaseParameterWarehouseInventory);

                                            //if (WarehouseOutput.SupplierID > 0)
                                            //{
                                            //    var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.ID == WarehouseOutput.SupplierID.Value && o.IsSync == true).FirstOrDefaultAsync();
                                            //    if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                                            //    {
                                            //        var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.CustomerID == WarehouseOutput.SupplierID && o.Active == true && o.InvoiceInputID > 0).ToListAsync();
                                            //        if (ListWarehouseInput.Count > 0)
                                            //        {
                                            //            var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                            //            var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Barcode == WarehouseOutputDetailBarcode.Barcode && o.Active == true).FirstOrDefaultAsync();
                                            //            if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0 && WarehouseInputDetailBarcode.ParentID > 0)
                                            //            {
                                            //                var WarehouseInput = ListWarehouseInput.Where(o => o.ID == WarehouseInputDetailBarcode.ParentID.Value).FirstOrDefault();
                                            //                if (WarehouseInput != null && WarehouseInput.ID > 0 && WarehouseInput.InvoiceInputID > 0)
                                            //                {
                                            //                    var InvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.ID == WarehouseInput.InvoiceInputID.Value && o.Active == true).FirstOrDefaultAsync();
                                            //                    if (InvoiceInput != null && InvoiceInput.ID > 0 && InvoiceInput.Active == true)
                                            //                    {
                                            //                        WarehouseInventory = new WarehouseInventory();
                                            //                        WarehouseInventory.ParentID = WarehouseOutputDetailBarcode.MaterialID;
                                            //                        WarehouseInventory.CompanyID = InvoiceInput.SupplierID;
                                            //                        WarehouseInventory.Year = WarehouseOutputDetailBarcode.DateScan.Value.Year;
                                            //                        WarehouseInventory.Month = WarehouseOutputDetailBarcode.DateScan.Value.Month;
                                            //                        WarehouseInventory.Day = WarehouseOutputDetailBarcode.DateScan.Value.Day;
                                            //                        WarehouseInventory.QuantityOutput00 = 0;
                                            //                        WarehouseInventory.Active = true;
                                            //                        WarehouseInventory.Action = 10;

                                            //                        WarehouseInventory = await GetByCondition(o => o.Action == 10 && o.CompanyID == WarehouseInventory.CompanyID && o.ParentID == WarehouseInventory.ParentID && o.Year == WarehouseInventory.Year && o.Month == WarehouseInventory.Month).FirstOrDefaultAsync();
                                            //                        if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                            //                        {
                                            //                            WarehouseInventory = new WarehouseInventory();
                                            //                        }
                                            //                        WarehouseInventory.ParentID = WarehouseOutputDetailBarcode.MaterialID;
                                            //                        WarehouseInventory.CompanyID = InvoiceInput.SupplierID;
                                            //                        WarehouseInventory.Year = WarehouseOutputDetailBarcode.DateScan.Value.Year;
                                            //                        WarehouseInventory.Month = WarehouseOutputDetailBarcode.DateScan.Value.Month;
                                            //                        WarehouseInventory.Day = WarehouseOutputDetailBarcode.DateScan.Value.Day;
                                            //                        WarehouseInventory.QuantityOutput00 = 0;
                                            //                        WarehouseInventory.Active = true;
                                            //                        WarehouseInventory.Action = 10;

                                            //                        var ListProductionOrder = await _ProductionOrderRepository.GetByCondition(o => o.Active == true && o.SupplierID == WarehouseInventory.CompanyID).ToListAsync();
                                            //                        if (ListProductionOrder.Count > 0)
                                            //                        {
                                            //                            var ListProductionOrderID = ListProductionOrder.Select(o => o.ID).ToList();
                                            //                            ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListProductionOrderID.Contains(o.ParentID.Value)).OrderBy(o => o.Date).ToListAsync();
                                            //                            if (ListWarehouseOutput.Count > 0)
                                            //                            {
                                            //                                var ListWarehouseOutputID = ListWarehouseOutput.Select(x => x.ID).ToList();
                                            //                                var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID != null && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.Active == true && o.MaterialID == WarehouseInventory.ParentID && o.Active == true && o.DateScan != null && o.DateScan.Value.Date == WarehouseOutputDetailBarcode.DateScan.Value.Date).OrderBy(o => o.DateScan).ToListAsync();
                                            //                                if (ListWarehouseOutputDetailBarcode.Count > 0)
                                            //                                {
                                            //                                    WarehouseInventory.QuantityOutput00 = ListWarehouseOutputDetailBarcode.Sum(x => x.Quantity);
                                            //                                }
                                            //                            }
                                            //                        }

                                            //                        await SaveFromSyncAsync(WarehouseInventory);
                                            //                    }
                                            //                }
                                            //            }
                                            //        }
                                            //    }
                                            //}
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncByWarehouseOutputDetailBarcodeFromBaseParameterAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter != null)
            {
                var WarehouseOutputDetailBarcode = BaseParameter.BaseModel;
                if (WarehouseOutputDetailBarcode != null)
                {
                    if (WarehouseOutputDetailBarcode.ID > 0)
                    {
                        if (WarehouseOutputDetailBarcode.ParentID > 0)
                        {
                            if (WarehouseOutputDetailBarcode.DateScan != null)
                            {
                                var WarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.ID == WarehouseOutputDetailBarcode.ParentID.Value && o.Active == true && o.SupplierID > 0).FirstOrDefaultAsync();
                                if (WarehouseOutput != null && WarehouseOutput.ID > 0 && WarehouseOutput.Active == true && WarehouseOutput.SupplierID > 0)
                                {
                                    var WarehouseInventory = new WarehouseInventory();
                                    WarehouseInventory.ParentID = WarehouseOutputDetailBarcode.MaterialID;
                                    WarehouseInventory.CategoryDepartmentID = WarehouseOutput.SupplierID;
                                    WarehouseInventory.Year = WarehouseOutputDetailBarcode.DateScan.Value.Year;
                                    WarehouseInventory.Month = WarehouseOutputDetailBarcode.DateScan.Value.Month;
                                    WarehouseInventory.Day = WarehouseOutputDetailBarcode.DateScan.Value.Day;
                                    WarehouseInventory.QuantityOutput00 = 0;
                                    WarehouseInventory.Active = true;
                                    WarehouseInventory.Action = 1;

                                    WarehouseInventory = await GetByCondition(o => o.Action == 1 && o.CategoryDepartmentID == WarehouseInventory.CategoryDepartmentID && o.ParentID == WarehouseInventory.ParentID && o.Year == WarehouseInventory.Year && o.Month == WarehouseInventory.Month).FirstOrDefaultAsync();
                                    if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                    {
                                        WarehouseInventory = new WarehouseInventory();
                                    }
                                    WarehouseInventory.ParentID = WarehouseOutputDetailBarcode.MaterialID;
                                    WarehouseInventory.CategoryDepartmentID = WarehouseOutput.SupplierID;
                                    WarehouseInventory.Year = WarehouseOutputDetailBarcode.DateScan.Value.Year;
                                    WarehouseInventory.Month = WarehouseOutputDetailBarcode.DateScan.Value.Month;
                                    WarehouseInventory.Day = WarehouseOutputDetailBarcode.DateScan.Value.Day;
                                    WarehouseInventory.QuantityOutput00 = 0;
                                    WarehouseInventory.Active = true;
                                    WarehouseInventory.Action = 1;

                                    var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.SupplierID == WarehouseInventory.CategoryDepartmentID).ToListAsync();
                                    if (ListWarehouseOutput.Count > 0)
                                    {
                                        var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).ToList();
                                        var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.MaterialID == WarehouseInventory.ParentID && o.Active == true && o.DateScan != null && o.DateScan.Value.Date == WarehouseOutputDetailBarcode.DateScan.Value.Date).ToListAsync();
                                        if (ListWarehouseOutputDetailBarcode.Count > 0)
                                        {
                                            WarehouseInventory.QuantityOutput00 = ListWarehouseOutputDetailBarcode.Sum(x => x.Quantity);
                                        }
                                    }

                                    await SaveFromSyncAsync(WarehouseInventory);

                                    //if (WarehouseOutput.SupplierID > 0)
                                    //{
                                    //    var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.ID == WarehouseOutput.SupplierID.Value && o.IsSync == true).FirstOrDefaultAsync();
                                    //    if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                                    //    {
                                    //        var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.CustomerID == WarehouseOutput.SupplierID && o.Active == true && o.InvoiceInputID > 0).ToListAsync();
                                    //        if (ListWarehouseInput.Count > 0)
                                    //        {
                                    //            var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                    //            var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Barcode == WarehouseOutputDetailBarcode.Barcode && o.Active == true).FirstOrDefaultAsync();
                                    //            if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0 && WarehouseInputDetailBarcode.ParentID > 0)
                                    //            {
                                    //                var WarehouseInput = ListWarehouseInput.Where(o => o.ID == WarehouseInputDetailBarcode.ParentID.Value).FirstOrDefault();
                                    //                if (WarehouseInput != null && WarehouseInput.ID > 0 && WarehouseInput.InvoiceInputID > 0)
                                    //                {
                                    //                    var InvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.ID == WarehouseInput.InvoiceInputID.Value && o.Active == true).FirstOrDefaultAsync();
                                    //                    if (InvoiceInput != null && InvoiceInput.ID > 0 && InvoiceInput.Active == true)
                                    //                    {
                                    //                        WarehouseInventory = new WarehouseInventory();
                                    //                        WarehouseInventory.ParentID = WarehouseOutputDetailBarcode.MaterialID;
                                    //                        WarehouseInventory.CompanyID = InvoiceInput.SupplierID;
                                    //                        WarehouseInventory.Year = WarehouseOutputDetailBarcode.DateScan.Value.Year;
                                    //                        WarehouseInventory.Month = WarehouseOutputDetailBarcode.DateScan.Value.Month;
                                    //                        WarehouseInventory.Day = WarehouseOutputDetailBarcode.DateScan.Value.Day;
                                    //                        WarehouseInventory.QuantityOutput00 = 0;
                                    //                        WarehouseInventory.Active = true;
                                    //                        WarehouseInventory.Action = 10;

                                    //                        WarehouseInventory = await GetByCondition(o => o.Action == 10 && o.CompanyID == WarehouseInventory.CompanyID && o.ParentID == WarehouseInventory.ParentID && o.Year == WarehouseInventory.Year && o.Month == WarehouseInventory.Month).FirstOrDefaultAsync();
                                    //                        if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                    //                        {
                                    //                            WarehouseInventory = new WarehouseInventory();
                                    //                        }
                                    //                        WarehouseInventory.ParentID = WarehouseOutputDetailBarcode.MaterialID;
                                    //                        WarehouseInventory.CompanyID = InvoiceInput.SupplierID;
                                    //                        WarehouseInventory.Year = WarehouseOutputDetailBarcode.DateScan.Value.Year;
                                    //                        WarehouseInventory.Month = WarehouseOutputDetailBarcode.DateScan.Value.Month;
                                    //                        WarehouseInventory.Day = WarehouseOutputDetailBarcode.DateScan.Value.Day;
                                    //                        WarehouseInventory.QuantityOutput00 = 0;
                                    //                        WarehouseInventory.Active = true;
                                    //                        WarehouseInventory.Action = 10;

                                    //                        var ListProductionOrder = await _ProductionOrderRepository.GetByCondition(o => o.Active == true && o.SupplierID == WarehouseInventory.CompanyID).ToListAsync();
                                    //                        if (ListProductionOrder.Count > 0)
                                    //                        {
                                    //                            var ListProductionOrderID = ListProductionOrder.Select(o => o.ID).ToList();
                                    //                            ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListProductionOrderID.Contains(o.ParentID.Value)).OrderBy(o => o.Date).ToListAsync();
                                    //                            if (ListWarehouseOutput.Count > 0)
                                    //                            {
                                    //                                var ListWarehouseOutputID = ListWarehouseOutput.Select(x => x.ID).ToList();
                                    //                                var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID != null && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.Active == true && o.MaterialID == WarehouseInventory.ParentID && o.Active == true && o.DateScan != null && o.DateScan.Value.Date == WarehouseOutputDetailBarcode.DateScan.Value.Date).OrderBy(o => o.DateScan).ToListAsync();
                                    //                                if (ListWarehouseOutputDetailBarcode.Count > 0)
                                    //                                {
                                    //                                    WarehouseInventory.QuantityOutput00 = ListWarehouseOutputDetailBarcode.Sum(x => x.Quantity);
                                    //                                }
                                    //                            }
                                    //                        }

                                    //                        await SaveFromSyncAsync(WarehouseInventory);
                                    //                    }
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<int> SaveFromSyncAsync(WarehouseInventory WarehouseInventory)
        {
            int result = GlobalHelper.InitializationNumber;
            switch (WarehouseInventory.Day)
            {
                case 1:
                    WarehouseInventory.QuantityInput01 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput01 = WarehouseInventory.QuantityOutput00;
                    break;
                case 2:
                    WarehouseInventory.QuantityInput02 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput02 = WarehouseInventory.QuantityOutput00;
                    break;
                case 3:
                    WarehouseInventory.QuantityInput03 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput03 = WarehouseInventory.QuantityOutput00;
                    break;
                case 4:
                    WarehouseInventory.QuantityInput04 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput04 = WarehouseInventory.QuantityOutput00;
                    break;
                case 5:
                    WarehouseInventory.QuantityInput05 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput05 = WarehouseInventory.QuantityOutput00;
                    break;
                case 6:
                    WarehouseInventory.QuantityInput06 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput06 = WarehouseInventory.QuantityOutput00;
                    break;
                case 7:
                    WarehouseInventory.QuantityInput07 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput07 = WarehouseInventory.QuantityOutput00;
                    break;
                case 8:
                    WarehouseInventory.QuantityInput08 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput08 = WarehouseInventory.QuantityOutput00;
                    break;
                case 9:
                    WarehouseInventory.QuantityInput09 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput09 = WarehouseInventory.QuantityOutput00;
                    break;
                case 10:
                    WarehouseInventory.QuantityInput10 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput10 = WarehouseInventory.QuantityOutput00;
                    break;
                case 11:
                    WarehouseInventory.QuantityInput11 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput11 = WarehouseInventory.QuantityOutput00;
                    break;
                case 12:
                    WarehouseInventory.QuantityInput12 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput12 = WarehouseInventory.QuantityOutput00;
                    break;
                case 13:
                    WarehouseInventory.QuantityInput13 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput13 = WarehouseInventory.QuantityOutput00;
                    break;
                case 14:
                    WarehouseInventory.QuantityInput14 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput14 = WarehouseInventory.QuantityOutput00;
                    break;
                case 15:
                    WarehouseInventory.QuantityInput15 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput15 = WarehouseInventory.QuantityOutput00;
                    break;
                case 16:
                    WarehouseInventory.QuantityInput16 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput16 = WarehouseInventory.QuantityOutput00;
                    break;
                case 17:
                    WarehouseInventory.QuantityInput17 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput17 = WarehouseInventory.QuantityOutput00;
                    break;
                case 18:
                    WarehouseInventory.QuantityInput18 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput18 = WarehouseInventory.QuantityOutput00;
                    break;
                case 19:
                    WarehouseInventory.QuantityInput19 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput19 = WarehouseInventory.QuantityOutput00;
                    break;
                case 20:
                    WarehouseInventory.QuantityInput20 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput20 = WarehouseInventory.QuantityOutput00;
                    break;
                case 21:
                    WarehouseInventory.QuantityInput21 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput21 = WarehouseInventory.QuantityOutput00;
                    break;
                case 22:
                    WarehouseInventory.QuantityInput22 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput22 = WarehouseInventory.QuantityOutput00;
                    break;
                case 23:
                    WarehouseInventory.QuantityInput23 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput23 = WarehouseInventory.QuantityOutput00;
                    break;
                case 24:
                    WarehouseInventory.QuantityInput24 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput24 = WarehouseInventory.QuantityOutput00;
                    break;
                case 25:
                    WarehouseInventory.QuantityInput25 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput25 = WarehouseInventory.QuantityOutput00;
                    break;
                case 26:
                    WarehouseInventory.QuantityInput26 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput26 = WarehouseInventory.QuantityOutput00;
                    break;
                case 27:
                    WarehouseInventory.QuantityInput27 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput27 = WarehouseInventory.QuantityOutput00;
                    break;
                case 28:
                    WarehouseInventory.QuantityInput28 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput28 = WarehouseInventory.QuantityOutput00;
                    break;
                case 29:
                    WarehouseInventory.QuantityInput29 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput29 = WarehouseInventory.QuantityOutput00;
                    break;
                case 30:
                    WarehouseInventory.QuantityInput30 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput30 = WarehouseInventory.QuantityOutput00;
                    break;
                case 31:
                    WarehouseInventory.QuantityInput31 = WarehouseInventory.QuantityInput00;
                    WarehouseInventory.QuantityOutput31 = WarehouseInventory.QuantityOutput00;
                    break;
            }
            var BaseParameter = new BaseParameter<WarehouseInventory>();
            BaseParameter.BaseModel = WarehouseInventory;
            await SaveAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncByWarehouseAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter != null)
            {
                if (BaseParameter.ParentID > 0)
                {
                    if (BaseParameter.ID > 0)
                    {
                        var WarehouseInventory = new WarehouseInventory();

                        if (BaseParameter.Action == 1)
                        {
                            var WarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.ID == BaseParameter.ParentID.Value && o.Active == true).FirstOrDefaultAsync();
                            if (WarehouseInput != null)
                            {
                                WarehouseInventory = await GetByCondition(o => o.CategoryDepartmentID == WarehouseInput.CustomerID && o.ParentID == BaseParameter.ID && o.Action == 1 && o.Year == WarehouseInput.Year && o.Month == WarehouseInput.Month).FirstOrDefaultAsync();
                                if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                {
                                    WarehouseInventory = new WarehouseInventory();
                                }
                                WarehouseInventory.QuantityInput00 = 0;
                                WarehouseInventory.Action = 1;
                                WarehouseInventory.Active = true;
                                WarehouseInventory.ParentID = BaseParameter.ID;

                                WarehouseInventory.CategoryDepartmentID = WarehouseInput.CustomerID;
                                WarehouseInventory.Year = WarehouseInput.Year;
                                WarehouseInventory.Month = WarehouseInput.Month;
                                WarehouseInventory.Day = WarehouseInput.Day;
                                var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.CustomerID == WarehouseInput.CustomerID && o.Year == WarehouseInput.Year && o.Month == WarehouseInput.Month && o.Day == WarehouseInput.Day && o.Active == true).ToListAsync();
                                if (ListWarehouseInput != null && ListWarehouseInput.Count > 0)
                                {
                                    var ListWarehouseInputID = ListWarehouseInput.Select(x => x.ID).ToList();
                                    var ListWarehouseInputDetail = await _WarehouseInputDetailRepository.GetByCondition(o => ListWarehouseInputID.Contains(o.ParentID.Value) && o.MaterialID == BaseParameter.ID && o.Active == true).ToListAsync();
                                    if (ListWarehouseInputDetail != null && ListWarehouseInputDetail.Count > 0)
                                    {
                                        WarehouseInventory.QuantityInput00 = ListWarehouseInputDetail.Sum(x => x.QuantityActual);
                                    }
                                }
                            }
                        }

                        if (BaseParameter.Action == 2)
                        {
                            var WarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.ID == BaseParameter.ParentID.Value && o.Active == true).FirstOrDefaultAsync();
                            if (WarehouseOutput != null)
                            {
                                WarehouseInventory = await GetByCondition(o => o.CategoryDepartmentID == WarehouseOutput.SupplierID && o.ParentID == BaseParameter.ID && o.Action == 1 && o.Year == WarehouseOutput.Year && o.Month == WarehouseOutput.Month && o.Active == true).FirstOrDefaultAsync();
                                if ((WarehouseInventory == null) || (WarehouseInventory.ID == 0))
                                {
                                    WarehouseInventory = new WarehouseInventory();
                                }
                                WarehouseInventory.QuantityOutput00 = 0;
                                WarehouseInventory.CategoryDepartmentID = WarehouseOutput.SupplierID;
                                WarehouseInventory.Year = WarehouseOutput.Year;
                                WarehouseInventory.Month = WarehouseOutput.Month;
                                WarehouseInventory.Day = WarehouseOutput.Day;
                                var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.SupplierID == WarehouseOutput.SupplierID && o.Year == WarehouseOutput.Year && o.Month == WarehouseOutput.Month && o.Day == WarehouseOutput.Day && o.Active == true).ToListAsync();
                                if (ListWarehouseOutput != null && ListWarehouseOutput.Count > 0)
                                {
                                    var ListWarehouseOutputID = ListWarehouseOutput.Select(x => x.ID).ToList();
                                    var ListWarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => ListWarehouseOutputID.Contains(o.ParentID.Value) && o.MaterialID == BaseParameter.ID && o.Active == true).ToListAsync();
                                    if (ListWarehouseOutputDetail != null && ListWarehouseOutputDetail.Count > 0)
                                    {
                                        WarehouseInventory.QuantityOutput00 = ListWarehouseOutputDetail.Sum(x => x.QuantityActual);
                                    }
                                }
                            }
                        }

                        switch (WarehouseInventory.Day)
                        {
                            case 1:
                                WarehouseInventory.QuantityInput01 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput01 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 2:
                                WarehouseInventory.QuantityInput02 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput02 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 3:
                                WarehouseInventory.QuantityInput03 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput03 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 4:
                                WarehouseInventory.QuantityInput04 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput04 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 5:
                                WarehouseInventory.QuantityInput05 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput05 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 6:
                                WarehouseInventory.QuantityInput06 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput06 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 7:
                                WarehouseInventory.QuantityInput07 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput07 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 8:
                                WarehouseInventory.QuantityInput08 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput08 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 9:
                                WarehouseInventory.QuantityInput09 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput09 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 10:
                                WarehouseInventory.QuantityInput10 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput10 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 11:
                                WarehouseInventory.QuantityInput11 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput11 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 12:
                                WarehouseInventory.QuantityInput12 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput12 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 13:
                                WarehouseInventory.QuantityInput13 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput13 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 14:
                                WarehouseInventory.QuantityInput14 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput14 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 15:
                                WarehouseInventory.QuantityInput15 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput15 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 16:
                                WarehouseInventory.QuantityInput16 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput16 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 17:
                                WarehouseInventory.QuantityInput17 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput17 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 18:
                                WarehouseInventory.QuantityInput18 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput18 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 19:
                                WarehouseInventory.QuantityInput19 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput19 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 20:
                                WarehouseInventory.QuantityInput20 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput20 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 21:
                                WarehouseInventory.QuantityInput21 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput21 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 22:
                                WarehouseInventory.QuantityInput22 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput22 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 23:
                                WarehouseInventory.QuantityInput23 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput23 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 24:
                                WarehouseInventory.QuantityInput24 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput24 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 25:
                                WarehouseInventory.QuantityInput25 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput25 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 26:
                                WarehouseInventory.QuantityInput26 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput26 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 27:
                                WarehouseInventory.QuantityInput27 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput27 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 28:
                                WarehouseInventory.QuantityInput28 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput28 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 29:
                                WarehouseInventory.QuantityInput29 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput29 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 30:
                                WarehouseInventory.QuantityInput30 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput30 = WarehouseInventory.QuantityOutput00;
                                break;
                            case 31:
                                WarehouseInventory.QuantityInput31 = WarehouseInventory.QuantityInput00;
                                WarehouseInventory.QuantityOutput31 = WarehouseInventory.QuantityOutput00;
                                break;
                        }
                        BaseParameter.BaseModel = WarehouseInventory;
                        await SaveAsync(BaseParameter);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> GetByActionAndYearAndMonthAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            result.List = await GetByCondition(item => item.Action == BaseParameter.Action && item.Year == BaseParameter.Year && item.Month == BaseParameter.Month).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndActionAndYearAndMonthAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            result.List = await GetByCondition(item => item.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && item.Action == BaseParameter.Action && item.Year == BaseParameter.Year && item.Month == BaseParameter.Month).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthToListAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                result.List = await GetByCondition(item => item.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && item.Year == BaseParameter.Year && item.Month == BaseParameter.Month).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndActiveToListAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                if (BaseParameter.Active == true)
                {
                    result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.QuantityEnd > 0).ToListAsync();
                }
                else
                {
                    result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndSearchStringToListAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    var List = BaseParameter.SearchString.Split(' ').ToList();
                    if (BaseParameter.SearchString.Contains(";"))
                    {
                        List = BaseParameter.SearchString.Split(';').ToList();
                    }
                    if (List.Count == 1)
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.Code == BaseParameter.SearchString).ToListAsync();
                        if (result.List.Count == 0)
                        {
                            result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.FileName == BaseParameter.SearchString).ToListAsync();
                        }
                    }
                    else
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && !string.IsNullOrEmpty(o.Code) && List.Contains(o.Code)).ToListAsync();
                    }
                }
                else
                {
                    if (BaseParameter.Active == true)
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.QuantityEnd > 0).ToListAsync();
                    }
                    else
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month).ToListAsync();
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndActionAndSearchStringToListAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter.CategoryDepartmentID > 0 && BaseParameter.Action > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    var List = BaseParameter.SearchString.Split(' ').ToList();
                    if (BaseParameter.SearchString.Contains(";"))
                    {
                        List = BaseParameter.SearchString.Split(';').ToList();
                    }
                    if (List.Count == 1)
                    {
                        result.List = await GetByCondition(o => o.Action == BaseParameter.Action && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.FileName == BaseParameter.SearchString).ToListAsync();
                        if (result.List.Count == 0)
                        {
                            result.List = await GetByCondition(o => o.Action == BaseParameter.Action && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.Code == BaseParameter.SearchString).ToListAsync();
                        }
                    }
                    else
                    {
                        result.List = await GetByCondition(o => o.Action == BaseParameter.Action && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && !string.IsNullOrEmpty(o.Code) && List.Contains(o.Code)).ToListAsync();
                    }
                }
                else
                {
                    if (BaseParameter.Active == true)
                    {
                        result.List = await GetByCondition(o => o.Action == BaseParameter.Action && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && (o.QuantityEnd > 0 || o.QuantityBegin > 0)).ToListAsync();
                    }
                    else
                    {
                        result.List = await GetByCondition(o => o.Action == BaseParameter.Action && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month).ToListAsync();
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDViaCompanyIDAndYearAndMonthToListAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter.CompanyID > 0)
            {
                var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Active == true && o.IsSync == true).OrderBy(o => o.SortOrder).FirstOrDefaultAsync();
                if (CategoryDepartment != null && CategoryDepartment.ID > 0)
                {
                    result.List = await GetByCondition(item => item.CategoryDepartmentID == CategoryDepartment.ID && item.Year == BaseParameter.Year && item.Month == BaseParameter.Month).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCompanyIDAndYearAndMonthToListAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter.CompanyID > 0)
            {
                result.List = await GetByCondition(item => item.CompanyID == BaseParameter.CompanyID && item.Year == BaseParameter.Year && item.Month == BaseParameter.Month).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> CreateAutoAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                //for (int year = 2018; year < 2026; year++)
                //{
                //    for (int month = 1; month < 13; month++)
                //    {
                //        string sql = @"SELECT ParentID, COUNT(ID) AS CompanyID FROM WarehouseInventory WHERE CategoryDepartmentID=" + BaseParameter.CategoryDepartmentID + " AND ACTION=1 AND YEAR=" + year + " AND MONTH=" + month + " GROUP BY ParentID HAVING COUNT(ID)>1";
                //        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.ERP_MariaDBConectionString, sql);
                //        var ListWarehouseInventoryParent = new List<WarehouseInventory>();

                //        for (int i = 0; i < ds.Tables.Count; i++)
                //        {
                //            DataTable dt = ds.Tables[i];
                //            ListWarehouseInventoryParent.AddRange(SQLHelper.ToList<WarehouseInventory>(dt));
                //        }
                //        if (ListWarehouseInventoryParent.Count > 0)
                //        {
                //            var ListWarehouseInventoryParentID = ListWarehouseInventoryParent.Select(o => o.ParentID).ToList();
                //            var ListWarehouseInventory = await GetByCondition(o => o.Action == 1 && o.CompanyID == 16 && o.CategoryDepartmentID == 23 && o.ParentID > 0 && ListWarehouseInventoryParentID.Contains(o.ParentID.Value)).ToListAsync();
                //            foreach (long ParentID in ListWarehouseInventoryParentID)
                //            {
                //                var ListWarehouseInventorySub = ListWarehouseInventory.Where(o => o.ParentID == ParentID).OrderByDescending(o => o.UpdateDate).ToList();
                //                for (int i = 1; i < ListWarehouseInventorySub.Count; i++)
                //                {
                //                    await _WarehouseInventoryRepository.RemoveAsync(ListWarehouseInventorySub[i].ID);
                //                }
                //            }
                //        }
                //    }
                //}


                var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && ((BaseParameter.Year <= 0 && 1 == 1) || (BaseParameter.Year > 0 && BaseParameter.Year == o.Date.Value.Year)) && ((BaseParameter.Month <= 0 && 1 == 1) || (BaseParameter.Month > 0 && BaseParameter.Month == o.Date.Value.Month))).OrderBy(o => o.Date).ToListAsync();
                if (ListWarehouseInput.Count > 0)
                {
                    var ListWarehouseInputID = ListWarehouseInput.Select(x => x.ID).ToList();
                    var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Active == true && o.MaterialID > 0 && ((string.IsNullOrEmpty(BaseParameter.SearchString) && 1 == 1) || (!string.IsNullOrEmpty(BaseParameter.SearchString) && o.MaterialName == BaseParameter.SearchString)) && o.DateScan != null && ((BaseParameter.Year <= 0 && 1 == 1) || (BaseParameter.Year > 0 && BaseParameter.Year == o.DateScan.Value.Year)) && ((BaseParameter.Month <= 0 && 1 == 1) || (BaseParameter.Month > 0 && BaseParameter.Month == o.DateScan.Value.Month))).OrderBy(o => o.DateScan).ToListAsync();
                    if (ListWarehouseInputDetailBarcode.Count > 0)
                    {
                        //foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                        //{
                        //    BaseParameter.ID = WarehouseInputDetailBarcode.ID;
                        //    await SyncByWarehouseInputDetailBarcodeAsync(BaseParameter);
                        //}

                        var ListWarehouseInputDetailBarcodeMaterialID = ListWarehouseInputDetailBarcode.Where(o => o.MaterialID > 0).OrderBy(o => o.MaterialID).Select(o => o.MaterialID).Distinct().ToList();
                        var ListWarehouseInputDetailBarcodeYear = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null).OrderBy(o => o.DateScan).Select(o => o.DateScan.Value.Date.Year).Distinct().ToList();
                        var ListWarehouseInputDetailBarcodeMonth = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null).OrderBy(o => o.DateScan).Select(o => o.DateScan.Value.Date.Month).Distinct().ToList();
                        ListWarehouseInputDetailBarcodeMaterialID = ListWarehouseInputDetailBarcodeMaterialID.OrderBy(o => o).ToList();
                        ListWarehouseInputDetailBarcodeYear = ListWarehouseInputDetailBarcodeYear.OrderBy(o => o).ToList();
                        ListWarehouseInputDetailBarcodeMonth = ListWarehouseInputDetailBarcodeMonth.OrderBy(o => o).ToList();
                        foreach (var Year in ListWarehouseInputDetailBarcodeYear)
                        {
                            if (Year > 0)
                            {
                                foreach (var Month in ListWarehouseInputDetailBarcodeMonth)
                                {
                                    if (Month > 0)
                                    {
                                        foreach (var MaterialID in ListWarehouseInputDetailBarcodeMaterialID)
                                        {
                                            if (MaterialID > 0)
                                            {
                                                var WarehouseInputDetailBarcode = ListWarehouseInputDetailBarcode.Where(o => o.MaterialID == MaterialID && o.DateScan != null && o.DateScan.Value.Year == Year && o.DateScan.Value.Month == Month).OrderByDescending(o => o.DateScan).FirstOrDefault();
                                                if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                                                {
                                                    BaseParameter.ID = WarehouseInputDetailBarcode.ID;
                                                    await SyncByWarehouseInputDetailBarcodeAsync(BaseParameter);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.SupplierID == BaseParameter.CategoryDepartmentID && o.Date != null && ((BaseParameter.Year <= 0 && 1 == 1) || (BaseParameter.Year > 0 && BaseParameter.Year == o.Date.Value.Year)) && ((BaseParameter.Month <= 0 && 1 == 1) || (BaseParameter.Month > 0 && BaseParameter.Month == o.Date.Value.Month))).OrderBy(o => o.Date).ToListAsync();
                if (ListWarehouseOutput.Count > 0)
                {
                    var ListWarehouseOutputID = ListWarehouseOutput.Select(x => x.ID).ToList();
                    var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.Active == true && o.MaterialID > 0 && ((string.IsNullOrEmpty(BaseParameter.SearchString) && 1 == 1) || (!string.IsNullOrEmpty(BaseParameter.SearchString) && o.MaterialName == BaseParameter.SearchString)) && o.UpdateDate != null && ((BaseParameter.Year <= 0 && 1 == 1) || (BaseParameter.Year > 0 && BaseParameter.Year == o.UpdateDate.Value.Year)) && ((BaseParameter.Month <= 0 && 1 == 1) || (BaseParameter.Month > 0 && BaseParameter.Month == o.UpdateDate.Value.Month))).OrderBy(o => o.UpdateDate).ToListAsync();
                    if (ListWarehouseOutputDetailBarcode.Count > 0)
                    {
                        //foreach (var WarehouseOutputDetailBarcode in ListWarehouseOutputDetailBarcode)
                        //{
                        //    BaseParameter.ID = WarehouseOutputDetailBarcode.ID;
                        //    await SyncByWarehouseOutputDetailBarcodeAsync(BaseParameter);
                        //}

                        var ListWarehouseOutputDetailBarcodeMaterialID = ListWarehouseOutputDetailBarcode.Where(o => o.MaterialID > 0).OrderBy(o => o.MaterialID).Select(o => o.MaterialID).Distinct().ToList();
                        var ListWarehouseOutputDetailBarcodeYear = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null).OrderBy(o => o.UpdateDate).Select(o => o.UpdateDate.Value.Year).Distinct().ToList();
                        var ListWarehouseOutputDetailBarcodeMonth = ListWarehouseOutputDetailBarcode.Where(o => o.UpdateDate != null).OrderBy(o => o.UpdateDate).Select(o => o.UpdateDate.Value.Month).Distinct().ToList();
                        ListWarehouseOutputDetailBarcodeMaterialID = ListWarehouseOutputDetailBarcodeMaterialID.OrderBy(o => o).ToList();
                        ListWarehouseOutputDetailBarcodeYear = ListWarehouseOutputDetailBarcodeYear.OrderBy(o => o).ToList();
                        ListWarehouseOutputDetailBarcodeMonth = ListWarehouseOutputDetailBarcodeMonth.OrderBy(o => o).ToList();
                        foreach (var Year in ListWarehouseOutputDetailBarcodeYear)
                        {
                            if (Year > 0)
                            {
                                foreach (var Month in ListWarehouseOutputDetailBarcodeMonth)
                                {
                                    if (Month > 0)
                                    {
                                        foreach (var MaterialID in ListWarehouseOutputDetailBarcodeMaterialID)
                                        {
                                            if (MaterialID > 0)
                                            {
                                                var WarehouseOutputDetailBarcode = ListWarehouseOutputDetailBarcode.Where(o => o.MaterialID == MaterialID && o.UpdateDate != null && o.UpdateDate.Value.Year == Year && o.UpdateDate.Value.Month == Month).OrderByDescending(o => o.UpdateDate).FirstOrDefault();
                                                if (WarehouseOutputDetailBarcode != null && WarehouseOutputDetailBarcode.ID > 0)
                                                {
                                                    BaseParameter.ID = WarehouseOutputDetailBarcode.ID;
                                                    await SyncByWarehouseOutputDetailBarcodeAsync(BaseParameter);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //if (BaseParameter.CompanyID > 0)
            //{
            //    var ListInvoiceInput = await _InvoiceInputRepository.GetByCondition(o => o.Active == true && o.SupplierID == BaseParameter.CompanyID).ToListAsync();
            //    if (ListInvoiceInput.Count > 0)
            //    {
            //        var ListInvoiceInputID = ListInvoiceInput.Select(o => o.ID).ToList();
            //        var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.InvoiceInputID > 0 && ListInvoiceInputID.Contains(o.InvoiceInputID.Value)).ToListAsync();
            //        if (ListWarehouseInput.Count > 0)
            //        {
            //            var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
            //            var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Active == true && o.DateScan != null && o.MaterialID > 0 && o.ParentID > 0).ToListAsync();
            //            if (ListWarehouseInputDetailBarcode.Count > 0)
            //            {
            //                foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
            //                {
            //                    BaseParameter.ID = WarehouseInputDetailBarcode.ID;
            //                    await SyncByWarehouseInputDetailBarcodeAsync(BaseParameter);
            //                }
            //            }
            //        }
            //    }

            //    var ListProductionOrder = await _ProductionOrderRepository.GetByCondition(o => o.Active == true && o.SupplierID == BaseParameter.CompanyID).OrderByDescending(o => o.Date).ToListAsync();
            //    if (ListProductionOrder.Count > 0)
            //    {
            //        var ListProductionOrderID = ListProductionOrder.Select(o => o.ID).ToList();
            //        var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.ParentID > 0 && ListProductionOrderID.Contains(o.ParentID.Value)).OrderByDescending(o => o.Date).ToListAsync();
            //        if (ListWarehouseOutput.Count > 0)
            //        {
            //            var ListWarehouseOutputID = ListWarehouseOutput.Select(x => x.ID).ToList();
            //            var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.Active == true && o.DateScan != null && o.MaterialID > 0 && o.ParentID > 0).OrderByDescending(o => o.DateScan).ToListAsync();
            //            if (ListWarehouseOutputDetailBarcode.Count > 0)
            //            {
            //                foreach (var WarehouseOutputDetailBarcode in ListWarehouseOutputDetailBarcode)
            //                {
            //                    BaseParameter.ID = WarehouseOutputDetailBarcode.ID;
            //                    await SyncByWarehouseOutputDetailBarcodeAsync(BaseParameter);
            //                }
            //            }
            //        }
            //    }
            //}
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndMaterialSpecialToListAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            result.List = new List<WarehouseInventory>();
            var ListMaterial = await _MaterialRepository.GetByCondition(o => o.IsSpecial == BaseParameter.Active).ToListAsync();
            if (ListMaterial.Count > 0)
            {
                var ListMaterialID = ListMaterial.Select(x => x.ID).ToList();
                result.List = await GetByCondition(o => o.ParentID != null && ListMaterialID.Contains(o.ParentID.Value) && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncAutoAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            result = await GetAllToListAsync();
            if (result.List != null && result.List.Count > 0)
            {
                foreach (var item in result.List)
                {
                    await SaveAsync(new BaseParameter<WarehouseInventory>() { BaseModel = item });
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncByCategoryDepartmentIDAndYearAndMonthAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter.CategoryDepartmentID > 0 && BaseParameter.Year > 0 && BaseParameter.Month > 0)
            {
                await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                var ListWarehouseInputDetailBarcodeStock = new List<WarehouseInputDetailBarcode>();
                if (BaseParameter.CategoryDepartmentID == 23)
                {
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.IsStock == true && o.CustomerID == BaseParameter.CategoryDepartmentID).OrderBy(o => o.Date).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).Distinct().ToList();
                        ListWarehouseInputDetailBarcodeStock = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.IsStock == true && ListWarehouseInputID.Contains(o.ParentID ?? 0)).OrderBy(o => o.MaterialID).ToListAsync();
                    }
                }
                else
                {
                }
                if (ListWarehouseInputDetailBarcodeStock.Count > 0)
                {
                    var ListWarehouseInputDetailBarcodeStockMaterialID = new List<long?>();
                    ListWarehouseInputDetailBarcodeStockMaterialID = ListWarehouseInputDetailBarcodeStock.Select(o => o.MaterialID).Distinct().ToList();
                    var ListWarehouseInputDetailBarcodeERP = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsStock != true && ListWarehouseInputDetailBarcodeStockMaterialID.Contains(o.MaterialID ?? 0)).OrderBy(o => o.MaterialID).ToListAsync();
                    var ListWarehouseOutputDetailBarcodeERP = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && ListWarehouseInputDetailBarcodeStockMaterialID.Contains(o.MaterialID ?? 0)).OrderBy(o => o.MaterialID).ToListAsync();
                    foreach (var MaterialID in ListWarehouseInputDetailBarcodeStockMaterialID)
                    {
                        var WarehouseInventory = new WarehouseInventory();
                        WarehouseInventory.ParentID = MaterialID;
                        WarehouseInventory.CategoryDepartmentID = BaseParameter.CategoryDepartmentID;
                        WarehouseInventory.Year = BaseParameter.Year;
                        WarehouseInventory.Month = BaseParameter.Month;
                        WarehouseInventory.Action = 1;
                        var ListWarehouseInputDetailBarcodeStockSub = ListWarehouseInputDetailBarcodeStock.Where(o => o.MaterialID == MaterialID).ToList();
                        WarehouseInventory.FileName = string.Join(" | ", ListWarehouseInputDetailBarcodeStockSub.Select(o => o.FileName).Distinct().ToList());
                        if (BaseParameter.Year == GlobalHelper.YearStock && BaseParameter.Month == 1)
                        {
                            WarehouseInventory.QuantityBegin = ListWarehouseInputDetailBarcodeStockSub.Sum(o => o.QuantitySNP);
                        }
                        var ListWarehouseInputDetailBarcode = ListWarehouseInputDetailBarcodeERP.Where(o => o.MaterialID == MaterialID).ToList();
                        if (ListWarehouseInputDetailBarcode.Count > 0)
                        {
                            WarehouseInventory.QuantityInput01 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 1).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput02 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 2).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput03 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 3).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput04 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 4).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput05 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 5).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput06 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 6).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput07 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 7).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput08 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 8).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput09 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 9).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput10 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 10).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput11 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 11).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput12 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 12).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput13 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 13).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput14 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 14).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput15 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 15).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput16 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 16).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput17 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 17).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput18 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 18).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput19 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 19).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput20 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 20).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput21 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 21).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput22 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 22).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput23 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 23).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput24 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 24).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput25 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 25).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput26 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 26).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput27 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 27).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput28 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 28).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput29 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 29).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput30 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 30).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput31 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 31).ToList().Sum(o => o.Quantity);
                        }
                        var ListWarehouseOutputDetailBarcode = ListWarehouseOutputDetailBarcodeERP.Where(o => o.MaterialID == MaterialID).ToList();
                        if (ListWarehouseOutputDetailBarcode.Count > 0)
                        {
                            WarehouseInventory.QuantityOutput01 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 1).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput02 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 2).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput03 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 3).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput04 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 4).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput05 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 5).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput06 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 6).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput07 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 7).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput08 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 8).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput09 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 9).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput10 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 10).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput11 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 11).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput12 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 12).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput13 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 13).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput14 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 14).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput15 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 15).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput16 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 16).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput17 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 17).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput18 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 18).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput19 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 19).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput20 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 20).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput21 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 21).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput22 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 22).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput23 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 23).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput24 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 24).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput25 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 25).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput26 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 26).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput27 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 27).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput28 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 28).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput29 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 29).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput30 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 30).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput31 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 31).ToList().Sum(o => o.Quantity);
                        }
                        BaseParameter<WarehouseInventory> BaseParameterWarehouseInventory = new BaseParameter<WarehouseInventory>();
                        BaseParameterWarehouseInventory.BaseModel = WarehouseInventory;
                        await SaveAsync(BaseParameterWarehouseInventory);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncInvoiceByCategoryDepartmentIDAndYearAndMonthAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter.CategoryDepartmentID > 0 && BaseParameter.Year > 0 && BaseParameter.Month > 0)
            {
                await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                var ListWarehouseInputDetailBarcodeStock = new List<WarehouseInputDetailBarcode>();
                if (BaseParameter.CategoryDepartmentID == 23)
                {
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.IsStock == true && o.CustomerID == BaseParameter.CategoryDepartmentID).OrderBy(o => o.Date).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).Distinct().ToList();
                        ListWarehouseInputDetailBarcodeStock = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.IsStock == true && ListWarehouseInputID.Contains(o.ParentID ?? 0)).OrderBy(o => o.MaterialID).ToListAsync();
                    }
                }
                else
                {
                }
                if (ListWarehouseInputDetailBarcodeStock.Count > 0)
                {
                    var ListWarehouseInputDetailBarcodeStockMaterialID = new List<long?>();
                    ListWarehouseInputDetailBarcodeStockMaterialID = ListWarehouseInputDetailBarcodeStock.Select(o => o.MaterialID).Distinct().ToList();
                    var ListWarehouseInputDetailBarcodeERP = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsStock != true && ListWarehouseInputDetailBarcodeStockMaterialID.Contains(o.MaterialID ?? 0)).OrderBy(o => o.MaterialID).ToListAsync();
                    var ListWarehouseInputDetailBarcodeERPParentID = ListWarehouseInputDetailBarcodeERP.Select(o => o.ParentID).Distinct().ToList();
                    var ListWarehouseInputERP = await _WarehouseInputRepository.GetByCondition(o => ListWarehouseInputDetailBarcodeERPParentID.Contains(o.ID)).ToListAsync();
                    var ListWarehouseInputERPInvoiceInputID = ListWarehouseInputERP.Select(o => o.InvoiceInputID).Distinct().ToList();
                    var ListInvoiceInput = await _InvoiceInputRepository.GetByCondition(o => ListWarehouseInputERPInvoiceInputID.Contains(o.ID)).ToListAsync();
                    var ListWarehouseOutputDetailBarcodeERP = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && ListWarehouseInputDetailBarcodeStockMaterialID.Contains(o.MaterialID ?? 0)).OrderBy(o => o.MaterialID).ToListAsync();
                    foreach (var MaterialID in ListWarehouseInputDetailBarcodeStockMaterialID)
                    {
                        var WarehouseInventory = new WarehouseInventory();
                        WarehouseInventory.ParentID = MaterialID;
                        WarehouseInventory.CategoryDepartmentID = BaseParameter.CategoryDepartmentID;
                        WarehouseInventory.Year = BaseParameter.Year;
                        WarehouseInventory.Month = BaseParameter.Month;
                        WarehouseInventory.Action = 100;
                        var ListWarehouseInputDetailBarcodeStockSub = ListWarehouseInputDetailBarcodeStock.Where(o => o.MaterialID == MaterialID).ToList();
                        WarehouseInventory.FileName = string.Join(" | ", ListWarehouseInputDetailBarcodeStockSub.Select(o => o.FileName).Distinct().ToList());
                        if (BaseParameter.Year == GlobalHelper.YearStock && BaseParameter.Month == 1)
                        {
                            WarehouseInventory.QuantityBegin = ListWarehouseInputDetailBarcodeStockSub.Sum(o => o.QuantitySNP);
                        }
                        var ListWarehouseInputDetailBarcode = ListWarehouseInputDetailBarcodeERP.Where(o => o.MaterialID == MaterialID).ToList();
                        if (ListWarehouseInputDetailBarcode.Count > 0)
                        {
                            for (int i = 0; i < ListWarehouseInputDetailBarcode.Count; i++)
                            {
                                var WarehouseInput = ListWarehouseInputERP.Where(o => o.ID == ListWarehouseInputDetailBarcode[i].ParentID).FirstOrDefault();
                                if (WarehouseInput != null && WarehouseInput.ID > 0)
                                {
                                    var InvoiceInput = ListInvoiceInput.Where(o => o.ID == WarehouseInput.InvoiceInputID).FirstOrDefault();
                                    if (InvoiceInput != null && InvoiceInput.ID > 0 && InvoiceInput.DateETA != null)
                                    {
                                        ListWarehouseInputDetailBarcode[i].DateScan = InvoiceInput.DateETA;
                                    }
                                }
                            }
                            WarehouseInventory.QuantityInput01 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 1).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput02 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 2).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput03 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 3).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput04 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 4).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput05 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 5).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput06 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 6).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput07 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 7).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput08 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 8).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput09 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 9).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput10 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 10).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput11 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 11).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput12 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 12).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput13 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 13).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput14 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 14).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput15 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 15).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput16 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 16).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput17 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 17).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput18 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 18).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput19 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 19).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput20 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 20).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput21 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 21).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput22 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 22).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput23 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 23).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput24 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 24).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput25 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 25).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput26 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 26).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput27 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 27).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput28 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 28).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput29 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 29).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput30 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 30).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityInput31 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 31).ToList().Sum(o => o.Quantity);
                        }
                        var ListWarehouseOutputDetailBarcode = ListWarehouseOutputDetailBarcodeERP.Where(o => o.MaterialID == MaterialID).ToList();
                        if (ListWarehouseOutputDetailBarcode.Count > 0)
                        {
                            WarehouseInventory.QuantityOutput01 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 1).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput02 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 2).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput03 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 3).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput04 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 4).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput05 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 5).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput06 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 6).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput07 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 7).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput08 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 8).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput09 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 9).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput10 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 10).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput11 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 11).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput12 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 12).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput13 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 13).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput14 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 14).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput15 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 15).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput16 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 16).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput17 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 17).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput18 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 18).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput19 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 19).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput20 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 20).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput21 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 21).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput22 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 22).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput23 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 23).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput24 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 24).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput25 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 25).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput26 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 26).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput27 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 27).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput28 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 28).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput29 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 29).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput30 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 30).ToList().Sum(o => o.Quantity);
                            WarehouseInventory.QuantityOutput31 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 31).ToList().Sum(o => o.Quantity);
                        }
                        BaseParameter<WarehouseInventory> BaseParameterWarehouseInventory = new BaseParameter<WarehouseInventory>();
                        BaseParameterWarehouseInventory.BaseModel = WarehouseInventory;
                        await SaveAsync(BaseParameterWarehouseInventory);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInventory>> SyncValueByCategoryDepartmentIDAndYearAndMonthAsync(BaseParameter<WarehouseInventory> BaseParameter)
        {
            var result = new BaseResult<WarehouseInventory>();
            if (BaseParameter.CategoryDepartmentID > 0 && BaseParameter.Year > 0 && BaseParameter.Month > 0)
            {
                await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                var ListWarehouseInputDetailBarcodeStock = new List<WarehouseInputDetailBarcode>();
                if (BaseParameter.CategoryDepartmentID == 23)
                {
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.IsStock == true && o.CustomerID == BaseParameter.CategoryDepartmentID).OrderBy(o => o.Date).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).Distinct().ToList();
                        ListWarehouseInputDetailBarcodeStock = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.IsStock == true && ListWarehouseInputID.Contains(o.ParentID ?? 0)).OrderBy(o => o.MaterialID).ToListAsync();
                    }
                }
                else
                {
                }
                if (ListWarehouseInputDetailBarcodeStock.Count > 0)
                {
                    var ListWarehouseInputDetailBarcodeStockMaterialID = new List<long?>();
                    ListWarehouseInputDetailBarcodeStockMaterialID = ListWarehouseInputDetailBarcodeStock.Select(o => o.MaterialID).Distinct().ToList();
                    var ListWarehouseInputDetailBarcode2025 = new List<WarehouseInputDetailBarcode>();
                    if (BaseParameter.Year == GlobalHelper.YearStock && BaseParameter.Month == 1)
                    {
                        ListWarehouseInputDetailBarcode2025 = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Quantity > 0 && o.DateScan != null && o.DateScan.Value.Year == 2025 && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsStock != true && ListWarehouseInputDetailBarcodeStockMaterialID.Contains(o.MaterialID ?? 0)).OrderBy(o => o.MaterialID).ToListAsync();
                    }
                    var ListWarehouseInputDetailBarcodeERP = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Quantity > 0 && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsStock != true && ListWarehouseInputDetailBarcodeStockMaterialID.Contains(o.MaterialID ?? 0)).OrderBy(o => o.MaterialID).ToListAsync();
                    var ListWarehouseOutputDetailBarcodeERP = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.Quantity > 0 && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && ListWarehouseInputDetailBarcodeStockMaterialID.Contains(o.MaterialID ?? 0)).OrderBy(o => o.MaterialID).ToListAsync();
                    foreach (var MaterialID in ListWarehouseInputDetailBarcodeStockMaterialID)
                    {
                        var WarehouseInventory = new WarehouseInventory();
                        WarehouseInventory.ParentID = MaterialID;
                        WarehouseInventory.CategoryDepartmentID = BaseParameter.CategoryDepartmentID;
                        WarehouseInventory.Year = BaseParameter.Year;
                        WarehouseInventory.Month = BaseParameter.Month;
                        WarehouseInventory.Action = 1000;
                        var ListWarehouseInputDetailBarcodeStockSub = ListWarehouseInputDetailBarcodeStock.Where(o => o.MaterialID == MaterialID).ToList();
                        WarehouseInventory.FileName = string.Join(" | ", ListWarehouseInputDetailBarcodeStockSub.Select(o => o.FileName).Distinct().ToList());
                        if (BaseParameter.Year == GlobalHelper.YearStock && BaseParameter.Month == 1)
                        {
                            decimal? Price = 0;
                            var ListWarehouseInputDetailBarcode2025Sub = ListWarehouseInputDetailBarcode2025.Where(o => o.MaterialID == MaterialID && o.Price > 0).ToList();
                            if (ListWarehouseInputDetailBarcode2025Sub.Count > 0)
                            {
                                Price = ListWarehouseInputDetailBarcode2025Sub.Sum(o => o.Price);
                                Price = Price / ListWarehouseInputDetailBarcode2025Sub.Count;
                            }
                            decimal? QuantityBegin = 0;
                            var WarehouseInventory202601 = await _WarehouseInventoryRepository.GetByCondition(o => o.Action == 1 && o.ParentID == MaterialID && o.Year == BaseParameter.Year && o.Month == BaseParameter.Month).FirstOrDefaultAsync();
                            if (WarehouseInventory202601 != null && WarehouseInventory202601.ID > 0)
                            {
                                QuantityBegin = WarehouseInventory202601.QuantityBegin;
                            }
                            WarehouseInventory.QuantityBegin = Price * QuantityBegin;
                        }
                        var ListWarehouseInputDetailBarcode = ListWarehouseInputDetailBarcodeERP.Where(o => o.MaterialID == MaterialID).ToList();
                        if (ListWarehouseInputDetailBarcode.Count > 0)
                        {
                            WarehouseInventory.QuantityInput01 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 1).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput02 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 2).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput03 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 3).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput04 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 4).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput05 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 5).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput06 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 6).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput07 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 7).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput08 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 8).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput09 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 9).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput10 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 10).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput11 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 11).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput12 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 12).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput13 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 13).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput14 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 14).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput15 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 15).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput16 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 16).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput17 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 17).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput18 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 18).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput19 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 19).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput20 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 20).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput21 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 21).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput22 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 22).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput23 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 23).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput24 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 24).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput25 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 25).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput26 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 26).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput27 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 27).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput28 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 28).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput29 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 29).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput30 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 30).ToList().Sum(o => o.TotalInvoice);
                            WarehouseInventory.QuantityInput31 = ListWarehouseInputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 31).ToList().Sum(o => o.TotalInvoice);
                        }
                        var ListWarehouseOutputDetailBarcode = ListWarehouseOutputDetailBarcodeERP.Where(o => o.MaterialID == MaterialID).ToList();
                        if (ListWarehouseOutputDetailBarcode.Count > 0)
                        {
                            WarehouseInventory.QuantityOutput01 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 1).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput02 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 2).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput03 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 3).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput04 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 4).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput05 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 5).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput06 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 6).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput07 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 7).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput08 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 8).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput09 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 9).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput10 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 10).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput11 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 11).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput12 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 12).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput13 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 13).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput14 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 14).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput15 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 15).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput16 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 16).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput17 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 17).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput19 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 19).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput20 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 20).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput21 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 21).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput22 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 22).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput23 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 23).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput24 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 24).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput25 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 25).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput26 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 26).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput27 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 27).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput28 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 28).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput29 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 29).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput30 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 30).ToList().Sum(o => o.Total);
                            WarehouseInventory.QuantityOutput31 = ListWarehouseOutputDetailBarcode.Where(o => o.DateScan != null && o.DateScan.Value.Day == 31).ToList().Sum(o => o.Total);
                        }
                        BaseParameter<WarehouseInventory> BaseParameterWarehouseInventory = new BaseParameter<WarehouseInventory>();
                        BaseParameterWarehouseInventory.BaseModel = WarehouseInventory;
                        await SaveAsync(BaseParameterWarehouseInventory);
                    }
                }
            }
            return result;
        }
    }
}

