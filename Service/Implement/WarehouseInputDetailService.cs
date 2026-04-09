namespace Service.Implement
{
    public class WarehouseInputDetailService : BaseService<WarehouseInputDetail, IWarehouseInputDetailRepository>
    , IWarehouseInputDetailService
    {
        private readonly IWarehouseInputDetailRepository _WarehouseInputDetailRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseOutputDetailBarcodeRepository _WarehouseOutputDetailBarcodeRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IMaterialConvertRepository _MaterialConvertRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryLocationRepository _CategoryLocationRepository;
        private readonly IWarehouseInventoryService _WarehourseInventoryService;


        private readonly IProductionOrderProductionPlanService _ProductionOrderProductionPlanService;
        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly IWarehouseRequestRepository _WarehouseRequestRepository;
        private readonly IInvoiceInputRepository _InvoiceInputRepository;

        public WarehouseInputDetailService(IWarehouseInputDetailRepository WarehouseInputDetailRepository
            , IWarehouseInputRepository WarehouseInputRepository
            , IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
            , IWarehouseOutputDetailBarcodeRepository WarehouseOutputDetailBarcodeRepository
            , IMaterialRepository MaterialRepository
            , IMaterialConvertRepository MaterialConvertRepository
            , ICategoryUnitRepository CategoryUnitRepository
            , ICategoryLocationRepository CategoryLocationRepository
            , IWarehouseInventoryService WarehourseInventoryService

            , IProductionOrderProductionPlanService ProductionOrderProductionPlanService
            , IWarehouseOutputRepository WarehouseOutputRepository
            , IWarehouseRequestRepository WarehouseRequestRepository
            , IInvoiceInputRepository InvoiceInputRepository
            ) : base(WarehouseInputDetailRepository)
        {
            _WarehouseInputDetailRepository = WarehouseInputDetailRepository;
            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseOutputDetailBarcodeRepository = WarehouseOutputDetailBarcodeRepository;
            _MaterialRepository = MaterialRepository;
            _MaterialConvertRepository = MaterialConvertRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
            _CategoryLocationRepository = CategoryLocationRepository;
            _WarehourseInventoryService = WarehourseInventoryService;

            _ProductionOrderProductionPlanService = ProductionOrderProductionPlanService;
            _WarehouseOutputRepository = WarehouseOutputRepository;
            _WarehouseRequestRepository = WarehouseRequestRepository;
            _InvoiceInputRepository = InvoiceInputRepository;
        }
        public override void InitializationSave(WarehouseInputDetail model)
        {
            BaseInitialization(model);

            long? CompanyID = 0;

            if (model.ParentID > 0)
            {
                var Parent = _WarehouseInputRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Date = Parent.Date;
                model.Code = model.Code ?? Parent.Code;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
                model.IsStock = Parent.IsStock;
                model.CategoryDepartmentID = Parent.CustomerID;
                if (Parent.InvoiceInputID > 0)
                {
                    var InvoiceInput = _InvoiceInputRepository.GetByID(Parent.InvoiceInputID.Value);
                    CompanyID = InvoiceInput.SupplierID;
                }

                if (Parent.Root == true)
                {
                    model.Quantity = model.QuantityActual;
                }
            }
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Year ?? model.Date.Value.Year;
            model.Month = model.Month ?? model.Date.Value.Month;
            model.Day = model.Day ?? model.Date.Value.Day;

            if (model.CategoryUnitID > 0)
            {
            }
            else
            {
                if (!string.IsNullOrEmpty(model.CategoryUnitName))
                {
                    var CategoryUnit = _CategoryUnitRepository.GetByName(model.CategoryUnitName);
                    if (CategoryUnit.ID == 0)
                    {
                        CategoryUnit.Active = true;
                        CategoryUnit.Name = model.CategoryUnitName;
                        _CategoryUnitRepository.Add(CategoryUnit);
                    }
                    model.CategoryUnitID = CategoryUnit.ID;
                }
            }
            if (model.CategoryUnitID > 0)
            {
                var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID.Value);
                model.CategoryUnitName = CategoryUnit.Name;
            }
            if (model.CategoryUnitID > 0)
            {
                var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID.Value);
                model.CategoryUnitName = CategoryUnit.Name;
            }
            var Material = _MaterialRepository.GetByDescription(model.MaterialName, model.CompanyID);
            model.MaterialID = Material.ID;
            if (model.MaterialID > 0)
            {
                Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialID = Material.ID;
                model.MaterialName = model.MaterialName ?? Material.Code;
                model.Display = Material.Name;
                model.QuantitySNP = model.QuantitySNP ?? Material.QuantitySNP;
                model.IsSNP = model.IsSNP ?? Material.IsSNP;
                model.FileName = Material.CategoryLineName;
                model.CategoryFamilyID = model.CategoryFamilyID ?? Material.CategoryFamilyID;
                model.CategoryFamilyName = model.CategoryFamilyName ?? Material.CategoryFamilyName;
                model.CategoryCompanyName = Material.OriginalEquipmentManufacturer;
                switch (CompanyID)
                {
                    case 19:
                        model.Name = Material.Code;
                        break;
                    default:
                        if (string.IsNullOrEmpty(model.Name))
                        {
                            var ListMaterialConvert = _MaterialConvertRepository.GetByParentIDAndActiveToList(Material.ID, true);
                            if (ListMaterialConvert.Count == 0)
                            {
                                ListMaterialConvert = _MaterialConvertRepository.GetByParentIDToList(Material.ID);
                            }
                            if (ListMaterialConvert.Count > 0)
                            {
                                var MaterialConvert = ListMaterialConvert.Where(o => o.Code != Material.Code).OrderBy(o => o.SortOrder).FirstOrDefault();
                                if (MaterialConvert != null && MaterialConvert.ID > 0)
                                {
                                    model.Name = MaterialConvert.Code;
                                }
                            }
                        }
                        break;
                }
                model.Name = model.Name ?? Material.Code;
            }

            var ListWarehouseInputDetailBarcode = _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == model.ParentID && o.WarehouseInputDetailID == model.ID && o.Active == true).ToList();
            model.QuantityOutput = ListWarehouseInputDetailBarcode.Sum(o => o.QuantityOutput);
            model.QuantityActual = ListWarehouseInputDetailBarcode.Sum(o => o.Quantity);

            //if (model.ParentID > 0)
            //{
            //    var Parent = _WarehouseInputRepository.GetByID(model.ParentID.Value);
            //    if (Parent.Root == true)
            //    {
            //        model.Quantity = model.QuantityActual;
            //    }
            //}

            if (model.IsStock == true)
            {
                model.Active = true;
                model.DateEnd = new DateTime(2025, 12, 31);
                if (model.DateBegin == null)
                {
                    model.DateBegin = model.DateEnd;
                }
                if (model.DateBegin.Value.Year < GlobalHelper.YearStock)
                {
                    var ListWarehouseInputDetailBarcodeStock = _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.MaterialID == model.MaterialID && o.ParentID > 0 && o.CategoryDepartmentID == model.CategoryDepartmentID && o.Active == true && o.DateScan != null && o.DateScan.Value.Date >= model.DateBegin.Value.Date && o.DateScan.Value.Date <= model.DateEnd.Value.Date).ToList();
                    var ListWarehouseOutputDetailBarcodeStock = _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.MaterialID == model.MaterialID && o.ParentID > 0 && o.CategoryDepartmentID == model.CategoryDepartmentID && o.Active == true && o.DateScan != null && o.DateScan.Value.Date >= model.DateBegin.Value.Date && o.DateScan.Value.Date <= model.DateEnd.Value.Date).ToList();
                    model.QuantityStockInput = ListWarehouseInputDetailBarcodeStock.Sum(o => o.Quantity);
                    model.QuantityStockOuput = ListWarehouseOutputDetailBarcodeStock.Sum(o => o.Quantity);
                    model.QuantityStockGAP = model.QuantityStockInput - model.QuantityStockOuput;
                    model.Quantity = model.QuantityStock + model.QuantityStockGAP;
                    model.QuantitySNP = model.Quantity;
                }
            }


            model.Quantity = model.Quantity ?? GlobalHelper.InitializationNumber;
            model.QuantityActual = model.QuantityActual ?? GlobalHelper.InitializationNumber;
            model.QuantityGAP = model.QuantityGAP ?? GlobalHelper.InitializationNumber;
            model.QuantityInvoiceGAP = model.QuantityInvoiceGAP ?? GlobalHelper.InitializationNumber;
            model.Price = model.Price ?? GlobalHelper.InitializationNumber;
            model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
            model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput = model.QuantityOutput ?? GlobalHelper.InitializationNumber;
            model.QuantityInvoice = model.QuantityInvoice ?? GlobalHelper.InitializationNumber;
            model.QuantityInventory = model.QuantityInventory ?? GlobalHelper.InitializationNumber;
            if (model.ID > 0)
            {
                model.Total = model.Quantity * model.Price;
            }
            else
            {
                model.Total = model.Total ?? model.Quantity * model.Price;
            }
            model.QuantityInventory = model.Quantity - model.QuantityOutput;
            model.QuantityGAP = model.QuantityActual - model.Quantity;
            model.QuantityInvoiceGAP = model.QuantityActual - model.QuantityInvoice;
            model.TotalTax = model.Total * model.Tax / 100;
            model.TotalDiscount = model.Total * model.Discount / 100;
            model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;
        }
        public override WarehouseInputDetail SetModelByModelCheck(WarehouseInputDetail Model, WarehouseInputDetail ModelCheck)
        {
            if (ModelCheck != null)
            {
                if (ModelCheck.ID > 0)
                {
                    if (Model.ID == 0)
                    {
                        Model.Quantity = Model.Quantity + ModelCheck.Quantity;
                    }
                    Model.ID = ModelCheck.ID;
                    Model.CreateUserID = ModelCheck.CreateUserID;
                    Model.CreateUserName = ModelCheck.CreateUserName;
                    Model.CreateUserCode = ModelCheck.CreateUserCode;
                    Model.CreateDate = ModelCheck.CreateDate;
                    if (Model.Active == null)
                    {
                        Model.Active = ModelCheck.Active;
                    }
                    if (Model.SortOrder == null)
                    {
                        Model.SortOrder = ModelCheck.SortOrder;
                    }
                }
            }
            return Model;
        }
        public override BaseResult<WarehouseInputDetail> Save(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                Initialization(BaseParameter.BaseModel);
                //if (BaseParameter.BaseModel.ParentID > 0)
                //{
                //    var Parent = _WarehouseInputRepository.GetByID(BaseParameter.BaseModel.ParentID.Value);
                //    BaseParameter.BaseModel.CompanyID = Parent.CompanyID;                    
                //}
                //if (BaseParameter.BaseModel.MaterialID > 0)
                //{

                //}
                //else
                //{
                //    var Material = _MaterialRepository.GetByDescription(BaseParameter.BaseModel.MaterialName, BaseParameter.BaseModel.CompanyID);
                //    BaseParameter.BaseModel.MaterialID = Material.ID;
                //}
                var ModelCheck = GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefault();
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = Update(BaseParameter);
                }
                else
                {
                    result = Add(BaseParameter);
                }
                if (result.BaseModel.ID > 0)
                {
                    try
                    {
                        BaseParameter.BaseModel = result.BaseModel;
                        SyncAsync(BaseParameter);
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
            }
            return result;
        }
        public override async Task<BaseResult<WarehouseInputDetail>> SaveAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                InitializationSave(BaseParameter.BaseModel);
                //if (BaseParameter.BaseModel.ParentID > 0)
                //{
                //    var Parent = _WarehouseInputRepository.GetByID(BaseParameter.BaseModel.ParentID.Value);
                //    BaseParameter.BaseModel.CompanyID = Parent.CompanyID;
                //}
                //if (BaseParameter.BaseModel.MaterialID > 0)
                //{

                //}
                //else
                //{
                //    var Material = _MaterialRepository.GetByDescription(BaseParameter.BaseModel.MaterialName, BaseParameter.BaseModel.CompanyID);
                //    BaseParameter.BaseModel.MaterialID = Material.ID;
                //}
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.QuantitySNP == BaseParameter.BaseModel.QuantitySNP && o.Description == BaseParameter.BaseModel.Description).FirstOrDefaultAsync();
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);

                if (ModelCheck != null && ModelCheck.ID > 0)
                {
                    if (ModelCheck.QuantitySNP != BaseParameter.BaseModel.QuantitySNP && ModelCheck.Quantity != BaseParameter.BaseModel.Quantity)
                    {
                        var ListWarehouseInputDetailBarcodeSub = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID && (o.QuantitySNP != BaseParameter.BaseModel.QuantitySNP || o.Quantity != BaseParameter.BaseModel.Quantity)).ToListAsync();
                        foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcodeSub)
                        {
                            if (WarehouseInputDetailBarcode.Active == true)
                            {
                                WarehouseInputDetailBarcode.Active = false;
                                await _WarehouseInputDetailBarcodeRepository.UpdateAsync(WarehouseInputDetailBarcode);
                            }
                            await _WarehouseInputDetailBarcodeRepository.RemoveAsync(WarehouseInputDetailBarcode.ID);
                        }
                    }
                }

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
                        await SyncAsync(BaseParameter);
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
            }
            return result;
        }
        public override async Task<BaseResult<WarehouseInputDetail>> RemoveAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            result.Count = await _WarehouseInputDetailRepository.RemoveAsync(BaseParameter.ID);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetail>> SyncAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            //await SyncWarehouseInputAsync(BaseParameter);
            //await SyncWarehouseInventoryAsync(BaseParameter);
            await SyncWarehouseInputDetailBarcodeAsync(BaseParameter);
            //await SyncProductionOrderProductionPlanAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetail>> SyncProductionOrderProductionPlanAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.Active == true)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.MaterialID > 0)
                            {
                                var WarehouseInput = await _WarehouseInputRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                                if (WarehouseInput.ID > 0)
                                {
                                    if (WarehouseInput.ParentID > 0)
                                    {
                                        var ListProductionOrderProductionPlan = await _ProductionOrderProductionPlanService.GetByCondition(o => o.ParentID == WarehouseInput.ParentID && o.Active == true && o.MaterialID == BaseParameter.BaseModel.MaterialID).ToListAsync();
                                        foreach (var ProductionOrderProductionPlan in ListProductionOrderProductionPlan)
                                        {
                                            var BaseParameterProductionOrderProductionPlan = new BaseParameter<ProductionOrderProductionPlan>();
                                            BaseParameterProductionOrderProductionPlan.BaseModel = ProductionOrderProductionPlan;
                                            await _ProductionOrderProductionPlanService.SaveAsync(BaseParameterProductionOrderProductionPlan);
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

        public virtual async Task<BaseResult<WarehouseInputDetail>> SyncWarehouseInputAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.Active == true)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            var Parent = await _WarehouseInputRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                            if (Parent != null)
                            {
                                if (Parent.Root == false)
                                {
                                    var List = await _WarehouseInputDetailRepository.GetByParentIDToListAsync(Parent.ID);
                                    if (List != null && List.Count > 0)
                                    {
                                        Parent.Total = List.Sum(x => x.Total);
                                        await _WarehouseInputRepository.UpdateAsync(Parent);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetail>> SyncWarehouseInventoryAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.Active == true)
                    {
                        if (BaseParameter.BaseModel.MaterialID > 0)
                        {
                            BaseParameter<WarehouseInventory> BaseParameterWarehourseInventory = new BaseParameter<WarehouseInventory>();
                            BaseParameterWarehourseInventory.ID = BaseParameter.BaseModel.MaterialID.Value;
                            BaseParameterWarehourseInventory.ParentID = BaseParameter.BaseModel.ParentID;
                            BaseParameterWarehourseInventory.Action = 1;
                            await _WarehourseInventoryService.SyncByWarehouseAsync(BaseParameterWarehourseInventory);
                        }
                    }
                }
            }
            return result;
        }
        public override async Task<BaseResult<WarehouseInputDetail>> GetBySearchStringToListAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                result.List = await GetByCondition(o => o.MaterialName.Contains(BaseParameter.SearchString)).ToListAsync();
                if (result.List.Count == 0)
                {
                    result.List = await GetByCondition(o => o.Display.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                if (result.List.Count == 0)
                {
                    result.List = await GetByCondition(o => o.Barcode.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                if (result.List.Count == 0)
                {
                    result.List = await GetByCondition(o => o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
            }
            return result;
        }

        public virtual async Task<BaseResult<WarehouseInputDetail>> GetByYear_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                result = await GetBySearchStringToListAsync(BaseParameter);
            }
            else
            {
                if (BaseParameter.Year > 0)
                {
                    if (BaseParameter.Month > 0)
                    {
                        if (BaseParameter.Day > 0)
                        {
                            result.List = await GetByCondition(o => o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.Day == BaseParameter.Day).ToListAsync();
                        }
                        else
                        {
                            result.List = await GetByCondition(o => o.Year == BaseParameter.Year && o.Month == BaseParameter.Month).ToListAsync();
                        }
                    }
                    else
                    {
                        result.List = await GetByCondition(o => o.Year == BaseParameter.Year).ToListAsync();
                    }
                }
                else
                {
                    result = await GetAllToListAsync();
                }
            }
            if (result.List.Count > 0)
            {
                if (BaseParameter.Active == true)
                {
                    result.List = result.List.Where(o => o.QuantityInventory > 0).ToList();
                }
                result.List = result.List.OrderBy(o => o.Date).ThenBy(o => o.ParentID).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetail>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            result.List = new List<WarehouseInputDetail>();
            Expression<Func<WarehouseInput, bool>> ConditionWarehouseInput = o => o.CustomerID == BaseParameter.CategoryDepartmentID;
            if (BaseParameter.Year > 0)
            {
                if (BaseParameter.Month > 0)
                {
                    if (BaseParameter.Day > 0)
                    {
                        ConditionWarehouseInput = o => (o.CustomerID == BaseParameter.CategoryDepartmentID) && (o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.Day == BaseParameter.Day);
                    }
                    else
                    {
                        ConditionWarehouseInput = o => (o.CustomerID == BaseParameter.CategoryDepartmentID) && (o.Year == BaseParameter.Year && o.Month == BaseParameter.Month);
                    }
                }
                else
                {
                    ConditionWarehouseInput = o => (o.CustomerID == BaseParameter.CategoryDepartmentID) && (o.Year == BaseParameter.Year);
                }
            }
            var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(ConditionWarehouseInput).ToListAsync();

            if ((ListWarehouseInput != null) && (ListWarehouseInput.Count > 0))
            {
                var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                Expression<Func<WarehouseInputDetail, bool>> Condition = o => o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value);
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    Condition = o => (o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value))
                    && ((o.MaterialName != null && o.MaterialName.Contains(BaseParameter.SearchString))
                    || (o.Display != null && o.Display.Contains(BaseParameter.SearchString))
                    || (o.Barcode != null && o.Barcode.Contains(BaseParameter.SearchString))
                    || (o.Code != null && o.Code.Contains(BaseParameter.SearchString)));
                }
                result.List = await GetByCondition(Condition).ToListAsync();
                if (BaseParameter.Active == true)
                {
                    result.List = result.List.Where(o => o.QuantityInventory > 0).ToList();
                }
            }
            if (result.List.Count > 0)
            {
                result.List = result.List.OrderBy(o => o.Date).ThenBy(o => o.ParentID).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetail>> GetByParentIDAndEmpty_SearchStringToListAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            result.List = new List<WarehouseInputDetail>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                result.List = await GetByCondition(o => o.MaterialName != null && o.MaterialName.Contains(BaseParameter.SearchString)).ToListAsync();
            }
            else
            {
                if (BaseParameter.ParentID > 0)
                {
                    result.List = await GetByCondition(o => o.ParentID != null && o.ParentID == BaseParameter.ParentID).Take(100).ToListAsync();
                    var empty = new WarehouseInputDetail();
                    result.List.Add(empty);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetail>> SyncWarehouseInputDetailBarcodeAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            try
            {
                if (BaseParameter.BaseModel != null)
                {
                    if (BaseParameter.BaseModel.ID > 0)
                    {
                        if (BaseParameter.BaseModel.Active == true)
                        {
                            if (BaseParameter.BaseModel.ParentID > 0)
                            {
                                if (BaseParameter.BaseModel.MaterialID > 0)
                                {
                                    if (BaseParameter.BaseModel.Quantity > 0)
                                    {
                                        await MySQLHelper.ERPSyncAsync(GlobalHelper.ERP_MariaDBConectionString);

                                        var WarehouseInput = await _WarehouseInputRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                                        if (WarehouseInput.ID > 0 && WarehouseInput.Active == true && WarehouseInput.Root != true && (WarehouseInput.WarehouseOutputID == null || WarehouseInput.WarehouseOutputID < 1))
                                        {
                                            var Material = await _MaterialRepository.GetByIDAsync(BaseParameter.BaseModel.MaterialID.Value);
                                            if (Material.ID > 0)
                                            {
                                                if (BaseParameter.BaseModel.QuantitySNP == 0 || BaseParameter.BaseModel.QuantitySNP == null)
                                                {
                                                    BaseParameter.BaseModel.QuantitySNP = Material.QuantitySNP;
                                                    if (BaseParameter.BaseModel.QuantitySNP == 0 || BaseParameter.BaseModel.QuantitySNP == null)
                                                    {
                                                        BaseParameter.BaseModel.QuantitySNP = BaseParameter.BaseModel.Quantity;
                                                    }
                                                }
                                                var LotNumber = (int)BaseParameter.BaseModel.Quantity / (int)BaseParameter.BaseModel.QuantitySNP;
                                                var LotNumberRemainder = (int)BaseParameter.BaseModel.Quantity % BaseParameter.BaseModel.QuantitySNP;
                                                if (LotNumberRemainder > 0)
                                                {
                                                    LotNumber = LotNumber + 1;
                                                }
                                                if (LotNumber == 0)
                                                {
                                                    LotNumber = 1;
                                                }
                                                int? QuantitySum = (int)BaseParameter.BaseModel.Quantity;
                                                int? Begin = 0;
                                                if (LotNumber == 1)
                                                {
                                                    Begin = LotNumber;
                                                }
                                                var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID).ToListAsync();
                                                var PARTNO = BaseParameter.BaseModel.Name ?? BaseParameter.BaseModel.MaterialName;
                                                var ListWarehouseInputDetailBarcodeAdd = new List<WarehouseInputDetailBarcode>();
                                                var ListWarehouseInputDetailBarcodeUpdate = new List<WarehouseInputDetailBarcode>();
                                                for (int? i = Begin; i <= LotNumber; i++)
                                                {
                                                    var LotNo = i.ToString();
                                                    if (i < 10)
                                                    {
                                                        LotNo = "0" + LotNo;
                                                    }
                                                    if (BaseParameter.BaseModel.Date == null)
                                                    {
                                                        BaseParameter.BaseModel.Date = GlobalHelper.InitializationDateTime;
                                                    }
                                                    var Barcode = PARTNO + "$$" + BaseParameter.BaseModel.QuantitySNP.Value.ToString("N0") + "$$" + BaseParameter.BaseModel.ParentID + "$$" + BaseParameter.BaseModel.ID + "$$" + BaseParameter.BaseModel.Date.Value.ToString("yyyyMMdd") + "$$" + LotNo;
                                                    if (WarehouseInput.IsStock == true)
                                                    {
                                                        Barcode = PARTNO + "$$" + BaseParameter.BaseModel.QuantitySNP.Value.ToString("N0") + "$$" + BaseParameter.BaseModel.ParentID + "$$" + BaseParameter.BaseModel.ID + "$$" + BaseParameter.BaseModel.Date.Value.ToString("yyyyMMdd") + "$$00$$StockBegin";
                                                    }
                                                    var WarehouseInputDetailBarcode = ListWarehouseInputDetailBarcode.Where(o => o.Barcode == Barcode).FirstOrDefault();
                                                    if (WarehouseInputDetailBarcode == null)
                                                    {
                                                        WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                                                        WarehouseInputDetailBarcode.DateScan = null;
                                                    }
                                                    if (WarehouseInput.IsStock == true)
                                                    {
                                                        WarehouseInputDetailBarcode.Active = true;
                                                        WarehouseInputDetailBarcode.DateScan = WarehouseInput.Date;
                                                    }
                                                    WarehouseInputDetailBarcode.IsStock = WarehouseInput.IsStock;
                                                    WarehouseInputDetailBarcode.ParentID = WarehouseInput.ID;
                                                    WarehouseInputDetailBarcode.CategoryDepartmentID = WarehouseInput.CustomerID;
                                                    WarehouseInputDetailBarcode.WarehouseInputDetailID = BaseParameter.BaseModel.ID;
                                                    WarehouseInputDetailBarcode.MaterialID = BaseParameter.BaseModel.MaterialID;
                                                    WarehouseInputDetailBarcode.Description = BaseParameter.BaseModel.Description;
                                                    WarehouseInputDetailBarcode.Barcode = Barcode;
                                                    WarehouseInputDetailBarcode.Price = BaseParameter.BaseModel.Price;
                                                    WarehouseInputDetailBarcode.QuantityInvoice = BaseParameter.BaseModel.Quantity;
                                                    WarehouseInputDetailBarcode.ProductID = i;
                                                    WarehouseInputDetailBarcode.ProductName = LotNumber.ToString();


                                                    WarehouseInputDetailBarcode.ParentName = WarehouseInput.Code;
                                                    WarehouseInputDetailBarcode.Date = WarehouseInput.Date;                                                    
                                                    WarehouseInputDetailBarcode.Code = WarehouseInput.Code;

                                                    WarehouseInputDetailBarcode.MaterialName = Material.Code;
                                                    WarehouseInputDetailBarcode.Name = PARTNO ?? Material.Code;
                                                    WarehouseInputDetailBarcode.QuantitySNP = BaseParameter.BaseModel.QuantitySNP;
                                                    WarehouseInputDetailBarcode.IsSNP = Material.IsSNP;
                                                    WarehouseInputDetailBarcode.Display = Material.Name;
                                                    WarehouseInputDetailBarcode.CategoryLocationName = Material.CategoryLocationName;

                                                    WarehouseInputDetailBarcode.CategoryUnitID = BaseParameter.BaseModel.CategoryUnitID;
                                                    WarehouseInputDetailBarcode.CategoryUnitName = BaseParameter.BaseModel.CategoryUnitName;

                                                    if (i == 0)
                                                    {
                                                        WarehouseInputDetailBarcode.Quantity = 0;
                                                        WarehouseInputDetailBarcode.ProductionCode = "Large";
                                                    }
                                                    else
                                                    {
                                                        WarehouseInputDetailBarcode.ProductionCode = "Small";
                                                        if (i == LotNumber)
                                                        {
                                                            WarehouseInputDetailBarcode.Quantity = QuantitySum;
                                                        }
                                                        else
                                                        {
                                                            WarehouseInputDetailBarcode.Quantity = WarehouseInputDetailBarcode.QuantitySNP;
                                                        }
                                                        QuantitySum = QuantitySum - (int)WarehouseInputDetailBarcode.QuantitySNP;
                                                    }

                                                    WarehouseInputDetailBarcodeInitialization(WarehouseInputDetailBarcode);

                                                    if (WarehouseInputDetailBarcode.ID > 0)
                                                    {
                                                        //await _WarehouseInputDetailBarcodeRepository.UpdateAsync(WarehouseInputDetailBarcode);
                                                        ListWarehouseInputDetailBarcodeUpdate.Add(WarehouseInputDetailBarcode);
                                                    }
                                                    else
                                                    {
                                                        //WarehouseInputDetailBarcode.Active = true;
                                                        //WarehouseInputDetailBarcode.Note = "SOHU";
                                                        //await _WarehouseInputDetailBarcodeRepository.AddAsync(WarehouseInputDetailBarcode);
                                                        ListWarehouseInputDetailBarcodeAdd.Add(WarehouseInputDetailBarcode);
                                                    }
                                                }

                                                await _WarehouseInputDetailBarcodeRepository.AddRangeAsync(ListWarehouseInputDetailBarcodeAdd);
                                                await _WarehouseInputDetailBarcodeRepository.UpdateRangeAsync(ListWarehouseInputDetailBarcodeUpdate);
                                            }
                                        }
                                    }
                                    if (BaseParameter.BaseModel.Quantity == 0 && BaseParameter.BaseModel.IsStock == true)
                                    {
                                        await MySQLHelper.ERPSyncAsync(GlobalHelper.ERP_MariaDBConectionString);

                                        var WarehouseInput = await _WarehouseInputRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                                        if (WarehouseInput.ID > 0 && WarehouseInput.Active == true && WarehouseInput.Root != true && (WarehouseInput.WarehouseOutputID == null || WarehouseInput.WarehouseOutputID < 1))
                                        {
                                            var Material = await _MaterialRepository.GetByIDAsync(BaseParameter.BaseModel.MaterialID.Value);
                                            if (Material.ID > 0)
                                            {
                                                var LotNumber = 0;
                                                var LotNumberRemainder = 0;
                                                if (LotNumberRemainder > 0)
                                                {
                                                    LotNumber = LotNumber + 1;
                                                }
                                                if (LotNumber == 0)
                                                {
                                                    LotNumber = 1;
                                                }
                                                LotNumber = 0;
                                                int? QuantitySum = (int)BaseParameter.BaseModel.Quantity;
                                                int? Begin = 0;
                                                if (LotNumber == 1)
                                                {
                                                    Begin = LotNumber;
                                                }
                                                Begin = 0;
                                                var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID).ToListAsync();
                                                var PARTNO = BaseParameter.BaseModel.Name ?? BaseParameter.BaseModel.MaterialName;
                                                var ListWarehouseInputDetailBarcodeAdd = new List<WarehouseInputDetailBarcode>();
                                                var ListWarehouseInputDetailBarcodeUpdate = new List<WarehouseInputDetailBarcode>();
                                                for (int? i = Begin; i <= LotNumber; i++)
                                                {
                                                    var LotNo = i.ToString();
                                                    if (i < 10)
                                                    {
                                                        LotNo = "0" + LotNo;
                                                    }
                                                    if (BaseParameter.BaseModel.Date == null)
                                                    {
                                                        BaseParameter.BaseModel.Date = GlobalHelper.InitializationDateTime;
                                                    }
                                                    var Barcode = PARTNO + "$$" + BaseParameter.BaseModel.QuantitySNP.Value.ToString("N0") + "$$" + BaseParameter.BaseModel.ParentID + "$$" + BaseParameter.BaseModel.ID + "$$" + BaseParameter.BaseModel.Date.Value.ToString("yyyyMMdd") + "$$" + LotNo;
                                                    if (WarehouseInput.IsStock == true)
                                                    {
                                                        Barcode = PARTNO + "$$" + BaseParameter.BaseModel.QuantitySNP.Value.ToString("N0") + "$$" + BaseParameter.BaseModel.ParentID + "$$" + BaseParameter.BaseModel.ID + "$$" + BaseParameter.BaseModel.Date.Value.ToString("yyyyMMdd") + "$$00$$StockBegin";
                                                    }
                                                    var WarehouseInputDetailBarcode = ListWarehouseInputDetailBarcode.Where(o => o.Barcode == Barcode).FirstOrDefault();
                                                    if (WarehouseInputDetailBarcode == null)
                                                    {
                                                        WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                                                        WarehouseInputDetailBarcode.DateScan = null;
                                                    }
                                                    if (WarehouseInput.IsStock == true)
                                                    {
                                                        WarehouseInputDetailBarcode.Active = true;
                                                        WarehouseInputDetailBarcode.DateScan = WarehouseInput.Date;
                                                    }
                                                    WarehouseInputDetailBarcode.IsStock = WarehouseInput.IsStock;
                                                    WarehouseInputDetailBarcode.ParentID = WarehouseInput.ID;
                                                    WarehouseInputDetailBarcode.CategoryDepartmentID = WarehouseInput.CustomerID;
                                                    WarehouseInputDetailBarcode.WarehouseInputDetailID = BaseParameter.BaseModel.ID;
                                                    WarehouseInputDetailBarcode.MaterialID = BaseParameter.BaseModel.MaterialID;
                                                    WarehouseInputDetailBarcode.Description = BaseParameter.BaseModel.Description;
                                                    WarehouseInputDetailBarcode.Barcode = Barcode;
                                                    WarehouseInputDetailBarcode.Price = BaseParameter.BaseModel.Price;
                                                    WarehouseInputDetailBarcode.TotalInvoice = BaseParameter.BaseModel.TotalInvoice;
                                                    WarehouseInputDetailBarcode.QuantityInvoice = BaseParameter.BaseModel.Quantity;
                                                    WarehouseInputDetailBarcode.ProductID = i;
                                                    WarehouseInputDetailBarcode.ProductName = LotNumber.ToString();


                                                    WarehouseInputDetailBarcode.ParentName = WarehouseInput.Code;
                                                    WarehouseInputDetailBarcode.Date = WarehouseInput.Date;
                                                    WarehouseInputDetailBarcode.Code = WarehouseInput.Code;

                                                    WarehouseInputDetailBarcode.MaterialName = Material.Code;
                                                    WarehouseInputDetailBarcode.Name = PARTNO ?? Material.Code;
                                                    WarehouseInputDetailBarcode.QuantitySNP = BaseParameter.BaseModel.QuantitySNP;
                                                    WarehouseInputDetailBarcode.IsSNP = Material.IsSNP;
                                                    WarehouseInputDetailBarcode.Display = Material.Name;
                                                    WarehouseInputDetailBarcode.CategoryLocationName = Material.CategoryLocationName;

                                                    WarehouseInputDetailBarcode.CategoryUnitID = BaseParameter.BaseModel.CategoryUnitID;
                                                    WarehouseInputDetailBarcode.CategoryUnitName = BaseParameter.BaseModel.CategoryUnitName;

                                                    if (i == 0)
                                                    {
                                                        WarehouseInputDetailBarcode.Quantity = 0;
                                                        WarehouseInputDetailBarcode.ProductionCode = "Large";
                                                    }
                                                    else
                                                    {
                                                        WarehouseInputDetailBarcode.ProductionCode = "Small";
                                                        if (i == LotNumber)
                                                        {
                                                            WarehouseInputDetailBarcode.Quantity = QuantitySum;
                                                        }
                                                        else
                                                        {
                                                            WarehouseInputDetailBarcode.Quantity = WarehouseInputDetailBarcode.QuantitySNP;
                                                        }
                                                        QuantitySum = QuantitySum - (int)WarehouseInputDetailBarcode.QuantitySNP;
                                                    }

                                                    WarehouseInputDetailBarcodeInitialization(WarehouseInputDetailBarcode);

                                                    if (WarehouseInputDetailBarcode.ID > 0)
                                                    {
                                                        //await _WarehouseInputDetailBarcodeRepository.UpdateAsync(WarehouseInputDetailBarcode);
                                                        ListWarehouseInputDetailBarcodeUpdate.Add(WarehouseInputDetailBarcode);
                                                    }
                                                    else
                                                    {
                                                        //WarehouseInputDetailBarcode.Active = true;
                                                        //WarehouseInputDetailBarcode.Note = "SOHU";
                                                        //await _WarehouseInputDetailBarcodeRepository.AddAsync(WarehouseInputDetailBarcode);
                                                        ListWarehouseInputDetailBarcodeAdd.Add(WarehouseInputDetailBarcode);
                                                    }
                                                }

                                                await _WarehouseInputDetailBarcodeRepository.AddRangeAsync(ListWarehouseInputDetailBarcodeAdd);
                                                await _WarehouseInputDetailBarcodeRepository.UpdateRangeAsync(ListWarehouseInputDetailBarcodeUpdate);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual void WarehouseInputDetailBarcodeInitialization(WarehouseInputDetailBarcode model)
        {
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Year ?? model.Date.Value.Year;
            model.Month = model.Month ?? model.Date.Value.Month;
            model.Day = model.Day ?? model.Date.Value.Day;
            if (model.Date != null)
            {
                model.Week = GlobalHelper.GetWeekByDateTime(model.Date.Value);
            }

            //model.Description = model.Description ?? model.Barcode;
            model.Quantity = model.Quantity ?? GlobalHelper.InitializationNumber;
            model.QuantityOutput = model.QuantityOutput ?? GlobalHelper.InitializationNumber;
            model.QuantityInventory = model.Quantity - model.QuantityOutput;
            model.Price = model.Price ?? GlobalHelper.InitializationNumber;
            model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
            model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;

            model.Total = model.Price * model.QuantityInventory;
            model.TotalInvoice = model.Price * model.Quantity;

            model.TotalTax = model.Total * model.Tax / 100;
            model.TotalDiscount = model.Total * model.Discount / 100;
            model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;
            if (!string.IsNullOrEmpty(model.Barcode))
            {
                var BarcodeSubString = model.Barcode.Split('$')[model.Barcode.Split('$').Length - 1];
                model.GroupCode = model.Barcode.Replace(BarcodeSubString, "");
            }
            model.DateScan = model.DateScan ?? model.Date;
        }
        public virtual async Task<BaseResult<WarehouseInputDetail>> SaveListAndSyncWarehouseInputDetailBarcodeAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            if (BaseParameter.ParentID > 0)
            {
                var List = await _WarehouseInputDetailRepository.GetByParentIDToListAsync(BaseParameter.ParentID.Value);
                foreach (var item in List)
                {
                    if (item.ID > 0)
                    {
                        item.Active = true;
                        await _WarehouseInputDetailRepository.UpdateAsync(item);
                        await SyncWarehouseInputDetailBarcodeAsync(new BaseParameter<WarehouseInputDetail> { BaseModel = item });
                    }
                }
            }
            return result;
        }
        public override BaseResult<WarehouseInputDetail> GetByParentIDToList(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = _WarehouseInputDetailRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID).OrderBy(o => o.MaterialName).ThenBy(o => o.QuantitySNP).ToList();
            }
            return result;
        }
        public override async Task<BaseResult<WarehouseInputDetail>> GetByParentIDToListAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await _WarehouseInputDetailRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID).OrderBy(o => o.MaterialName).ThenBy(o => o.QuantitySNP).ToListAsync();
            }
            return result;
        }

        public virtual async Task<BaseResult<WarehouseInputDetail>> GetByQuantityGAPToListAsync(BaseParameter<WarehouseInputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            result.List = new List<WarehouseInputDetail>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                if (BaseParameter.Active == true)
                {
                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.QuantityGAP > 0).ToListAsync();
                }
                else
                {
                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.QuantityGAP < 0).ToListAsync();
                }
            }
            result.List = result.List.OrderBy(o => o.ParentID).ThenBy(o => o.MaterialName).ToList();
            return result;
        }
    }
}

