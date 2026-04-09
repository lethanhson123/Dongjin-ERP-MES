namespace Service.Implement
{
    public class WarehouseOutputDetailService : BaseService<WarehouseOutputDetail, IWarehouseOutputDetailRepository>
    , IWarehouseOutputDetailService
    {
        private readonly IWarehouseOutputDetailRepository _WarehouseOutputDetailRepository;
        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryLocationRepository _CategoryLocationRepository;
        private readonly IWarehouseInventoryService _WarehourseInventoryService;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputDetailRepository _WarehouseInputDetailRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseOutputDetailBarcodeRepository _WarehouseOutputDetailBarcodeRepository;

        public WarehouseOutputDetailService(IWarehouseOutputDetailRepository WarehouseOutputDetailRepository
            , IWarehouseOutputRepository WarehouseOutputRepository
            , IMaterialRepository MaterialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            , ICategoryLocationRepository CategoryLocationRepository
            , IWarehouseInventoryService warehourseInventoryService
            , IWarehouseInputRepository WarehouseInputRepository
            , IWarehouseInputDetailRepository WarehouseInputDetailRepository
            , IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
            , IWarehouseOutputDetailBarcodeRepository WarehouseOutputDetailBarcodeRepository
            ) : base(WarehouseOutputDetailRepository)
        {
            _WarehouseOutputDetailRepository = WarehouseOutputDetailRepository;
            _WarehouseOutputRepository = WarehouseOutputRepository;
            _MaterialRepository = MaterialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
            _CategoryLocationRepository = CategoryLocationRepository;
            _WarehourseInventoryService = warehourseInventoryService;
            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseInputDetailRepository = WarehouseInputDetailRepository;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseOutputDetailBarcodeRepository = WarehouseOutputDetailBarcodeRepository;
        }
        public override void Initialization(WarehouseOutputDetail model)
        {
            BaseInitialization(model);

            if (model.ParentID > 0)
            {
                var Parent = _WarehouseOutputRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Date = Parent.Date;
                model.Code = model.Code ?? Parent.Code;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
            }
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Year = model.Year ?? model.Date.Value.Year;
            model.Month = model.Month ?? model.Date.Value.Month;
            model.Day = model.Day ?? model.Date.Value.Day;
            if (model.Week == null)
            {
                if (model.DateInput != null)
                {
                    model.Week = GlobalHelper.GetWeekByDateTime(model.DateInput.Value);
                }
            }

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
            var Material = new Material();
            if (model.MaterialID > 0)
            {

            }
            else
            {
                Material = _MaterialRepository.GetByDescription(model.MaterialName, model.CompanyID);
                model.MaterialID = Material.ID;
            }
            if (model.MaterialID > 0)
            {
                Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialName = Material.Code;
                model.QuantitySNP = Material.QuantitySNP;
                model.IsSNP = model.IsSNP ?? Material.IsSNP;
                model.FileName = Material.CategoryLineName;
                model.CategoryFamilyID = model.CategoryFamilyID ?? Material.CategoryFamilyID;
                model.CategoryFamilyName = model.CategoryFamilyName ?? Material.CategoryFamilyName;
                model.CategoryCompanyName = Material.OriginalEquipmentManufacturer;
                if (string.IsNullOrEmpty(model.Display))
                {
                    model.Display = Material.Name;
                }
            }

            var ListWarehouseOutputDetailBarcode = _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialID == model.MaterialID && o.Active == true).ToList();
            model.QuantityActual = ListWarehouseOutputDetailBarcode.Sum(o => o.Quantity);
            model.Description = model.Description ?? model.Barcode;
            model.Quantity = model.Quantity ?? GlobalHelper.InitializationNumber;
            model.QuantityActual = model.QuantityActual ?? GlobalHelper.InitializationNumber;
            model.QuantityGAP = model.QuantityGAP ?? GlobalHelper.InitializationNumber;
            model.Price = model.Price ?? GlobalHelper.InitializationNumber;
            model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
            model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
            if (model.ID > 0)
            {
                model.Total = model.Quantity * model.Price;
            }
            else
            {
                model.Total = model.Total ?? model.Quantity * model.Price;
            }
            model.QuantityGAP = model.Quantity - model.QuantityActual;
            model.TotalTax = model.Total * model.Tax / 100;
            model.TotalDiscount = model.Total * model.Discount / 100;
            model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;
        }
        public override BaseResult<WarehouseOutputDetail> Save(BaseParameter<WarehouseOutputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                Initialization(BaseParameter.BaseModel);
                //if (BaseParameter.BaseModel.ParentID > 0)
                //{
                //    var Parent = _WarehouseOutputRepository.GetByID(BaseParameter.BaseModel.ParentID.Value);
                //    BaseParameter.BaseModel.ParentName = Parent.Code;
                //    BaseParameter.BaseModel.Date = Parent.Date;
                //    BaseParameter.BaseModel.Code = BaseParameter.BaseModel.Code ?? Parent.Code;
                //    BaseParameter.BaseModel.CompanyID = Parent.CompanyID;
                //    BaseParameter.BaseModel.CompanyName = Parent.CompanyName;
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
        public override async Task<BaseResult<WarehouseOutputDetail>> SaveAsync(BaseParameter<WarehouseOutputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                //InitializationSave(BaseParameter.BaseModel);
                if (BaseParameter.BaseModel.ParentID > 0)
                {
                    var Parent = _WarehouseOutputRepository.GetByID(BaseParameter.BaseModel.ParentID.Value);
                    BaseParameter.BaseModel.ParentName = Parent.Code;
                    BaseParameter.BaseModel.Date = Parent.Date;
                    BaseParameter.BaseModel.Code = BaseParameter.BaseModel.Code ?? Parent.Code;
                    BaseParameter.BaseModel.CompanyID = Parent.CompanyID;
                    BaseParameter.BaseModel.CompanyName = Parent.CompanyName;
                }
                if (BaseParameter.BaseModel.MaterialID > 0)
                {

                }
                else
                {
                    var Material = _MaterialRepository.GetByDescription(BaseParameter.BaseModel.MaterialName, BaseParameter.BaseModel.CompanyID);
                    BaseParameter.BaseModel.MaterialID = Material.ID;
                }
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
                if (ModelCheck != null)
                {
                    if (ModelCheck.ID > 0)
                    {
                        BaseParameter.BaseModel.ID = ModelCheck.ID;
                        BaseParameter.BaseModel.CreateDate = ModelCheck.CreateDate;
                        BaseParameter.BaseModel.CreateUserID = ModelCheck.CreateUserID;
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
        public override async Task<BaseResult<WarehouseOutputDetail>> RemoveAsync(BaseParameter<WarehouseOutputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            result.Count = await _WarehouseOutputDetailRepository.RemoveAsync(BaseParameter.ID);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetail>> SyncAsync(BaseParameter<WarehouseOutputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            //await SyncWarehouseOutputAsync(BaseParameter);
            await SyncWarehouseOutputDetailBarcodeAsync(BaseParameter);
            //await SyncWarehouseInventoryAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetail>> SyncWarehouseOutputAsync(BaseParameter<WarehouseOutputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.Active == true)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            var Parent = await _WarehouseOutputRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                            if (Parent != null)
                            {
                                var List = await _WarehouseOutputDetailRepository.GetByParentIDToListAsync(Parent.ID);
                                if (List != null && List.Count > 0)
                                {
                                    Parent.Total = List.Sum(x => x.Total);
                                    await _WarehouseOutputRepository.UpdateAsync(Parent);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetail>> SyncWarehouseInventoryAsync(BaseParameter<WarehouseOutputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.MaterialID > 0)
                        {
                            BaseParameter<WarehouseInventory> BaseParameterWarehourseInventory = new BaseParameter<WarehouseInventory>();
                            BaseParameterWarehourseInventory.ID = BaseParameter.BaseModel.MaterialID.Value;
                            BaseParameterWarehourseInventory.ParentID = BaseParameter.BaseModel.ParentID;
                            BaseParameterWarehourseInventory.Action = 2;
                            await _WarehourseInventoryService.SyncByWarehouseAsync(BaseParameterWarehourseInventory);
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetail>> SyncWarehouseOutputDetailBarcodeAsync(BaseParameter<WarehouseOutputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.MaterialID > 0)
                        {
                            if (BaseParameter.BaseModel.QuantityGAP > 0)
                            {
                                var WarehouseOutput = await _WarehouseOutputRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                                if (WarehouseOutput.ID > 0)
                                {
                                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == WarehouseOutput.SupplierID).ToListAsync();
                                    if (ListWarehouseInput.Count > 0)
                                    {
                                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                        var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Active == true && o.IsStock != true && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.QuantityInventory > 0).OrderBy(o => o.DateScan).ToListAsync();
                                        if (ListWarehouseInputDetailBarcode.Count == 0)
                                        {
                                            ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Active == true && o.IsStock != true && o.MaterialName == BaseParameter.BaseModel.MaterialName && o.QuantityInventory > 0).OrderBy(o => o.DateScan).ToListAsync();
                                        }
                                        for (int i = 0; i < ListWarehouseInputDetailBarcode.Count; i++)
                                        {
                                            var WarehouseInputDetailBarcode = ListWarehouseInputDetailBarcode[i];

                                            var WarehouseOutputDetailBarcode = new WarehouseOutputDetailBarcode();
                                            WarehouseOutputDetailBarcode.ParentID = BaseParameter.BaseModel.ParentID;
                                            WarehouseOutputDetailBarcode.WarehouseOutputDetailID = BaseParameter.BaseModel.ID;
                                            WarehouseOutputDetailBarcode.QuantityRequest = BaseParameter.BaseModel.QuantityRequest;
                                            WarehouseOutputDetailBarcode.IsSNP = BaseParameter.BaseModel.IsSNP;
                                            WarehouseOutputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.QuantityInventory;
                                            WarehouseOutputDetailBarcode.DateInput = WarehouseInputDetailBarcode.DateScan;
                                            WarehouseOutputDetailBarcode.Year = WarehouseInputDetailBarcode.Year;
                                            WarehouseOutputDetailBarcode.Month = WarehouseInputDetailBarcode.Month;
                                            WarehouseOutputDetailBarcode.Day = WarehouseInputDetailBarcode.Day;
                                            WarehouseOutputDetailBarcode.Week = WarehouseInputDetailBarcode.Week;
                                            //if (WarehouseOutputDetailBarcode.DateInput != null)
                                            //{
                                            //    WarehouseOutputDetailBarcode.Year = WarehouseOutputDetailBarcode.DateInput.Value.Year;
                                            //    WarehouseOutputDetailBarcode.Month = WarehouseOutputDetailBarcode.DateInput.Value.Month;
                                            //    WarehouseOutputDetailBarcode.Day = WarehouseOutputDetailBarcode.DateInput.Value.Day;
                                            //    WarehouseOutputDetailBarcode.Week = GlobalHelper.GetWeekByDateTime(WarehouseOutputDetailBarcode.DateInput.Value);
                                            //}
                                            WarehouseOutputDetailBarcode.CategoryDepartmentID = WarehouseInputDetailBarcode.CategoryDepartmentID;
                                            WarehouseOutputDetailBarcode.CategoryDepartmentName = WarehouseInputDetailBarcode.CategoryDepartmentName;
                                            if (WarehouseOutputDetailBarcode.CategoryDepartmentID > 0)
                                            {
                                            }
                                            else
                                            {
                                                var WarehouseOutputSub = await _WarehouseOutputRepository.GetByIDAsync(WarehouseOutputDetailBarcode.ParentID.Value);
                                                WarehouseOutputDetailBarcode.CategoryDepartmentID = WarehouseOutputSub.SupplierID;
                                                WarehouseOutputDetailBarcode.CategoryDepartmentName = WarehouseOutputSub.SupplierName;
                                            }
                                            WarehouseOutputDetailBarcode.CategoryLocationID = WarehouseInputDetailBarcode.CategoryLocationID;
                                            WarehouseOutputDetailBarcode.CategoryLocationName = WarehouseInputDetailBarcode.CategoryLocationName;
                                            WarehouseOutputDetailBarcode.CategoryUnitID = WarehouseInputDetailBarcode.CategoryUnitID;
                                            WarehouseOutputDetailBarcode.CategoryUnitName = WarehouseInputDetailBarcode.CategoryUnitName;
                                            WarehouseOutputDetailBarcode.MaterialID = WarehouseInputDetailBarcode.MaterialID;
                                            WarehouseOutputDetailBarcode.MaterialName = WarehouseInputDetailBarcode.MaterialName;
                                            WarehouseOutputDetailBarcode.QuantitySNP = WarehouseInputDetailBarcode.QuantitySNP;
                                            WarehouseOutputDetailBarcode.Display = WarehouseInputDetailBarcode.Display;
                                            WarehouseOutputDetailBarcode.Barcode = WarehouseInputDetailBarcode.Barcode;
                                            if (BaseParameter.BaseModel.QuantityGAP <= 0)
                                            {
                                                i = ListWarehouseInputDetailBarcode.Count;
                                            }
                                            else
                                            {
                                                if (WarehouseInputDetailBarcode.Quantity > BaseParameter.BaseModel.QuantityGAP)
                                                {
                                                    WarehouseOutputDetailBarcode.Quantity = BaseParameter.BaseModel.QuantityGAP;
                                                    i = ListWarehouseInputDetailBarcode.Count;
                                                }
                                                else
                                                {
                                                    WarehouseOutputDetailBarcode.Quantity = WarehouseInputDetailBarcode.Quantity;
                                                }
                                            }
                                            BaseParameter.BaseModel.QuantityActual = BaseParameter.BaseModel.QuantityActual + WarehouseOutputDetailBarcode.Quantity;
                                            BaseParameter.BaseModel.QuantityGAP = BaseParameter.BaseModel.Quantity - BaseParameter.BaseModel.QuantityActual;
                                            if (WarehouseOutputDetailBarcode.Quantity > 0)
                                            {
                                                var WarehouseOutputDetailBarcodeCheck = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.ParentID == WarehouseOutputDetailBarcode.ParentID && !string.IsNullOrEmpty(o.Barcode) && !string.IsNullOrEmpty(WarehouseOutputDetailBarcode.Barcode) && o.Barcode.Trim().ToLower() == WarehouseOutputDetailBarcode.Barcode.Trim().ToLower()).FirstOrDefaultAsync();
                                                if (WarehouseOutputDetailBarcodeCheck == null)
                                                {
                                                    await _WarehouseOutputDetailBarcodeRepository.AddAsync(WarehouseOutputDetailBarcode);
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
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetail>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseOutputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            result.List = new List<WarehouseOutputDetail>();
            Expression<Func<WarehouseOutput, bool>> ConditionWarehouseOutput = o => o.SupplierID == BaseParameter.CategoryDepartmentID;
            if (BaseParameter.Year > 0)
            {
                if (BaseParameter.Month > 0)
                {
                    if (BaseParameter.Day > 0)
                    {
                        ConditionWarehouseOutput = o => (o.CustomerID == BaseParameter.CategoryDepartmentID) && (o.Year == BaseParameter.Year && o.Month == BaseParameter.Month && o.Day == BaseParameter.Day);
                    }
                    else
                    {
                        ConditionWarehouseOutput = o => (o.CustomerID == BaseParameter.CategoryDepartmentID) && (o.Year == BaseParameter.Year && o.Month == BaseParameter.Month);
                    }
                }
                else
                {
                    ConditionWarehouseOutput = o => (o.CustomerID == BaseParameter.CategoryDepartmentID) && (o.Year == BaseParameter.Year);
                }
            }
            var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(ConditionWarehouseOutput).ToListAsync();

            if ((ListWarehouseOutput != null) && (ListWarehouseOutput.Count > 0))
            {
                var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).ToList();
                Expression<Func<WarehouseInputDetail, bool>> Condition = o => o.ParentID != null && ListWarehouseOutputID.Contains(o.ParentID.Value);
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    Condition = o => (o.ParentID != null && ListWarehouseOutputID.Contains(o.ParentID.Value))
                    && ((o.MaterialName != null && o.MaterialName.Contains(BaseParameter.SearchString))
                    || (o.Display != null && o.Display.Contains(BaseParameter.SearchString))
                    || (o.Barcode != null && o.Barcode.Contains(BaseParameter.SearchString))
                    || (o.Code != null && o.Code.Contains(BaseParameter.SearchString)));
                }
            }
            if (result.List.Count > 0)
            {
                result.List = result.List.OrderBy(o => o.Date).ThenBy(o => o.ParentID).ToList();
            }
            return result;
        }
        public override async Task<BaseResult<WarehouseOutputDetail>> GetByParentIDAndActiveToListAsync(BaseParameter<WarehouseOutputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == BaseParameter.Active).OrderByDescending(o => o.UpdateDate).ThenBy(o => o.MaterialName).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetail>> GetByActive_IsComplete_MembershipAsync(BaseParameter<WarehouseOutputDetail> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetail>();
            var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.IsComplete != true && o.MembershipID > 0).OrderBy(o => o.Date).ToListAsync();
            if (ListWarehouseOutput.Count > 0)
            {
                var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).OrderBy(o => o).ToList();
                result.List = await GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.Quantity > 0 && o.QuantityGAP > 0).OrderBy(o => o.MaterialName).ToListAsync();
            }
            return result;
        }
    }
}

