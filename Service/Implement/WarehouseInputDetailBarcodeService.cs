namespace Service.Implement
{
    public class WarehouseInputDetailBarcodeService : BaseService<WarehouseInputDetailBarcode, IWarehouseInputDetailBarcodeRepository>
    , IWarehouseInputDetailBarcodeService
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;

        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputDetailService _WarehouseInputDetailService;
        private readonly IWarehouseInputDetailRepository _WarehouseInputDetailRepository;
        private readonly IWarehouseInputDetailBarcodeMaterialService _WarehouseInputDetailBarcodeMaterialService;
        private readonly IWarehouseInputDetailCountRepository _WarehouseInputDetailCountRepository;

        private readonly IMaterialRepository _MaterialRepository;
        private readonly IMaterialConvertRepository _MaterialConvertRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryLocationRepository _CategoryLocationRepository;
        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly IWarehouseOutputDetailBarcodeRepository _WarehouseOutputDetailBarcodeRepository;
        private readonly IWarehouseInventoryService _WarehourseInventoryService;
        private readonly IWarehouseStockService _WarehouseStockService;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IMembershipRepository _MembershipRepository;
        private readonly IMembershipDepartmentRepository _MembershipDepartmentRepository;
        private readonly IBOMRepository _BOMRepository;
        private readonly IBOMDetailRepository _BOMDetailRepository;


        public WarehouseInputDetailBarcodeService(IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
            , IWebHostEnvironment webHostEnvironment
            , IWarehouseInputRepository WarehouseInputRepository
            , IWarehouseInputDetailService WarehouseInputDetailService
            , IWarehouseInputDetailRepository WarehouseInputDetailRepository
            , IWarehouseInputDetailBarcodeMaterialService WarehouseInputDetailBarcodeMaterialService
            , IWarehouseInputDetailCountRepository WarehouseInputDetailCountRepository
            , IMaterialRepository MaterialRepository
            , IMaterialConvertRepository MaterialConvertRepository
            , ICategoryUnitRepository CategoryUnitRepository
            , ICategoryLocationRepository CategoryLocationRepository
            , IWarehouseOutputRepository warehouseOutputRepository
            , IWarehouseOutputDetailBarcodeRepository warehouseOutputDetailBarcodeRepository
            , IWarehouseInventoryService WarehouseInventoryService
            , IWarehouseStockService WarehouseStockService
            , ICategoryDepartmentRepository CategoryDepartmentRepository
            , IMembershipRepository membershipRepository
            , IMembershipDepartmentRepository memberDepartmentRepository
            , IBOMRepository bomRepository
            , IBOMDetailRepository BOMDetailRepository

            ) : base(WarehouseInputDetailBarcodeRepository)
        {
            _WebHostEnvironment = webHostEnvironment;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseInputDetailService = WarehouseInputDetailService;
            _WarehouseInputDetailRepository = WarehouseInputDetailRepository;
            _WarehouseInputDetailBarcodeMaterialService = WarehouseInputDetailBarcodeMaterialService;
            _WarehouseInputDetailCountRepository = WarehouseInputDetailCountRepository;
            _MaterialRepository = MaterialRepository;
            _MaterialConvertRepository = MaterialConvertRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
            _CategoryLocationRepository = CategoryLocationRepository;
            _WarehouseOutputRepository = warehouseOutputRepository;
            _WarehouseOutputDetailBarcodeRepository = warehouseOutputDetailBarcodeRepository;
            _WarehourseInventoryService = WarehouseInventoryService;
            _WarehouseStockService = WarehouseStockService;
            _CategoryDepartmentRepository = CategoryDepartmentRepository;
            _MembershipRepository = membershipRepository;
            _MembershipDepartmentRepository = memberDepartmentRepository;
            _BOMRepository = bomRepository;
            _BOMDetailRepository = BOMDetailRepository;
        }
        public override void Initialization(WarehouseInputDetailBarcode model)
        {
            if (model.IsScan == true)
            {
                model.IsScan = false;
            }
            else
            {
                BaseInitialization(model);

                long CustomerID = 0;
                CategoryDepartment CategoryDepartment = new CategoryDepartment();
                if (model.ParentID > 0)
                {
                    var Parent = _WarehouseInputRepository.GetByID(model.ParentID.Value);
                    model.ParentName = Parent.Code;
                    model.InvoiceInputName = Parent.InvoiceInputName;
                    model.Date = Parent.Date;
                    model.Code = model.Code ?? Parent.Code;
                    model.CompanyID = Parent.CompanyID;
                    model.CompanyName = Parent.CompanyName;
                    CustomerID = Parent.CustomerID ?? 0;
                    CategoryDepartment = _CategoryDepartmentRepository.GetByID(CustomerID);
                    model.CategoryDepartmentID = Parent.CustomerID;
                    model.CategoryDepartmentName = Parent.CustomerName;
                }
                model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
                model.DateScan = model.DateScan ?? model.Date;

                if (model.DateScan != null)
                {
                    model.Year = model.DateScan.Value.Year;
                    model.Month = model.DateScan.Value.Month;
                    model.Day = model.DateScan.Value.Day;
                    model.Week = GlobalHelper.GetWeekByDateTime(model.DateScan.Value);
                }

                if (model.CategoryUnitID > 0)
                {
                    var CategoryUnit = _CategoryUnitRepository.GetByID(model.CategoryUnitID.Value);
                    model.CategoryUnitName = CategoryUnit.Name;
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
                if (model.CategoryLocationID > 0)
                {
                    var CategoryLocation = _CategoryLocationRepository.GetByID(model.CategoryLocationID.Value);
                    model.CategoryLocationName = CategoryLocation.Name;
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.CategoryLocationName))
                    {
                        var CategoryLocation = _CategoryLocationRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Name == model.CategoryLocationName).FirstOrDefault();
                        if (CategoryLocation == null)
                        {
                            CategoryLocation = new CategoryLocation();
                            CategoryLocation.Name = model.CategoryLocationName;
                            CategoryLocation.CompanyID = model.CompanyID;
                            _CategoryLocationRepository.Add(CategoryLocation);
                        }
                        model.CategoryLocationID = CategoryLocation.ID;
                    }
                }

                if (!string.IsNullOrEmpty(model.Barcode))
                {
                    if (model.Barcode.Contains("$"))
                    {
                        model.MaterialName = model.Barcode.Split('$')[0];
                    }
                }
                var Material = _MaterialRepository.GetByDescription(model.MaterialName, model.CompanyID);
                model.MaterialID = Material.ID;
                model.MaterialName = model.MaterialName ?? Material.Code;
                model.QuantitySNP = model.QuantitySNP ?? Material.QuantitySNP;
                model.Display = Material.Name;
                model.IsSNP = model.IsSNP ?? Material.IsSNP;
                model.CategoryLocationName = model.CategoryLocationName ?? Material.CategoryLocationName;
                model.CategoryFamilyID = model.CategoryFamilyID ?? Material.CategoryFamilyID;
                model.CategoryFamilyName = model.CategoryFamilyName ?? Material.CategoryFamilyName;
                model.FileName = Material.CategoryLineName;
                model.CategoryCompanyName = Material.OriginalEquipmentManufacturer;
                model.Name = model.Name ?? model.MaterialName;
                if (string.IsNullOrEmpty(model.Barcode))
                {
                    var PARTNO = model.Name ?? model.MaterialName;
                    model.Barcode = PARTNO + "$$" + model.QuantitySNP + "$$" + model.ParentID + "$$" + model.Date.Value.ToString("yyyyMMdd") + "$$" + model.Date.Value.Ticks;
                }
                var BarcodeSubString = model.Barcode.Split('$')[model.Barcode.Split('$').Length - 1];
                model.GroupCode = model.Barcode.Replace(BarcodeSubString, "");

                if (model.WarehouseInputDetailID > 0)
                {

                }
                else
                {
                    var WarehouseInputDetail = _WarehouseInputDetailRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialID == model.MaterialID && o.QuantitySNP == model.QuantitySNP && o.Description == model.Description).FirstOrDefault();
                    if (WarehouseInputDetail == null)
                    {
                        WarehouseInputDetail = _WarehouseInputDetailRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialName == model.MaterialName && o.QuantitySNP == model.QuantitySNP && o.Description == model.Description).FirstOrDefault();
                    }
                    if (WarehouseInputDetail == null)
                    {
                        WarehouseInputDetail = new WarehouseInputDetail();
                        WarehouseInputDetail.Active = true;
                        WarehouseInputDetail.ParentID = model.ParentID;
                        WarehouseInputDetail.ParentName = model.ParentName;
                        WarehouseInputDetail.QuantitySNP = model.QuantitySNP;
                        WarehouseInputDetail.Description = model.Description;
                        WarehouseInputDetail.Code = model.Code;
                        WarehouseInputDetail.MaterialID = model.MaterialID;
                        WarehouseInputDetail.MaterialName = model.MaterialName;
                        WarehouseInputDetail.Display = model.Display;
                        WarehouseInputDetail.CategoryUnitID = model.CategoryUnitID;
                        WarehouseInputDetail.CategoryUnitName = model.CategoryUnitName;
                        WarehouseInputDetail.CategoryLocationID = model.CategoryLocationID;
                        WarehouseInputDetail.CategoryLocationName = model.CategoryLocationName;
                        _WarehouseInputDetailRepository.Add(WarehouseInputDetail);                      
                    }
                    model.WarehouseInputDetailID = WarehouseInputDetail.ID;
                }

                if (!string.IsNullOrEmpty(model.Barcode) && model.ParentID > 0 && model.Active == true)
                {
                    MySQLHelper.ERPSync02(GlobalHelper.ERP_MariaDBConectionString);
                    var ListWarehouseOutputDetailBarcode = _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == model.CategoryDepartmentID && o.Barcode == model.Barcode && o.Active == true).ToList();
                    model.QuantityOutput = ListWarehouseOutputDetailBarcode.Sum(o => o.Quantity);
                }
                model.QuantityInvoice = model.QuantityInvoice ?? 1;
                model.QuantityMES = model.QuantityMES ?? 1;
                model.Quantity = model.Quantity ?? model.QuantityInvoice;
                model.PKG_QTYActual = (double)(model.Quantity - model.QuantityMES);
                model.QuantityOutput = model.QuantityOutput ?? GlobalHelper.InitializationNumber;
                model.QuantityInventory = model.Quantity - model.QuantityOutput;
                model.Price = model.Price ?? GlobalHelper.InitializationNumber;
                model.Tax = model.Tax ?? GlobalHelper.InitializationNumber;
                model.Discount = model.Discount ?? GlobalHelper.InitializationNumber;
                if (model.ID > 0)
                {
                    var ModelCheck = _WarehouseInputDetailBarcodeRepository.GetByID(model.ID);
                    model.Page = (model.Page + ModelCheck.PageSize) ?? 0;
                }
                model.Total = model.Price * model.QuantityInventory;
                model.TotalInvoice = model.TotalInvoice ?? model.Price * model.Quantity;

                model.TotalTax = model.Total * model.Tax / 100;
                model.TotalDiscount = model.Total * model.Discount / 100;
                model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;

            }
            model.IsSync = null;
        }
        public virtual void InitializationSaveHookRack(WarehouseInputDetailBarcode model)
        {
            if (model.ParentID > 0)
            {
                var Parent = _WarehouseInputRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.InvoiceInputName = Parent.InvoiceInputName;
                model.Date = Parent.Date;
                model.Code = model.Code ?? Parent.Code;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
                model.CategoryDepartmentID = Parent.CustomerID;
                model.CategoryDepartmentName = Parent.CustomerName;
            }
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.DateScan = model.DateScan ?? model.Date;

            if (model.DateScan != null)
            {
                model.Year = model.DateScan.Value.Year;
                model.Month = model.DateScan.Value.Month;
                model.Day = model.DateScan.Value.Day;
                model.Week = GlobalHelper.GetWeekByDateTime(model.DateScan.Value);
            }

            if (!string.IsNullOrEmpty(model.Barcode))
            {
                if (model.Barcode.Contains("$"))
                {
                    model.MaterialName = model.Barcode.Split('$')[0];
                }
            }
            var Material = _MaterialRepository.GetByDescription(model.MaterialName, model.CompanyID);
            model.MaterialID = Material.ID;
            model.MaterialName = model.MaterialName ?? Material.Code;
            model.QuantitySNP = model.QuantitySNP ?? Material.QuantitySNP;
            model.Display = Material.Name;
            model.IsSNP = model.IsSNP ?? Material.IsSNP;
            model.CategoryLocationName = model.CategoryLocationName ?? Material.CategoryLocationName;
            model.CategoryFamilyID = model.CategoryFamilyID ?? Material.CategoryFamilyID;
            model.CategoryFamilyName = model.CategoryFamilyName ?? Material.CategoryFamilyName;
            model.FileName = Material.CategoryLineName;
            model.CategoryCompanyName = Material.OriginalEquipmentManufacturer;

            model.ECN = model.ECN ?? "HOOKRACK";
            if (!string.IsNullOrEmpty(model.ECN) && model.CompanyID > 0)
            {
                model.BOMECNVersion = model.BOMECNVersion ?? "1.0";
                var ListBOM = _BOMRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.MaterialCode == model.MaterialName && o.Code == model.ECN).OrderByDescending(o => o.Date).ToList();
                if (ListBOM.Count == 0)
                {
                    ListBOM = _BOMRepository.GetByCondition(o => o.CompanyID == model.CompanyID && o.Active == true && o.MaterialCode == model.MaterialName).OrderByDescending(o => o.Date).ToList();
                }
                var BOM = new BOM();
                if (ListBOM.Count > 0)
                {
                    model.ListCode = string.Join(";", ListBOM.Select(o => o.Code));
                    BOM = ListBOM[0];
                }
                if (BOM != null && BOM.ID > 0)
                {
                    model.BOMID = BOM.ID;
                    model.ECN = BOM.Code;
                    model.BOMECNVersion = BOM.Version;
                    model.BOMDate = BOM.Date;
                    model.MaterialID01 = BOM.MaterialID;
                    model.MaterialName01 = BOM.MaterialCode;
                }
            }

            if (model.WarehouseInputDetailID > 0)
            {

            }
            else
            {
                var WarehouseInputDetail = _WarehouseInputDetailRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialID == model.MaterialID && o.QuantitySNP == model.QuantitySNP && o.Description == model.Description).FirstOrDefault();
                if (WarehouseInputDetail == null)
                {
                    WarehouseInputDetail = _WarehouseInputDetailRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialName == model.MaterialName && o.QuantitySNP == model.QuantitySNP && o.Description == model.Description).FirstOrDefault();
                }
                if (WarehouseInputDetail == null)
                {
                    WarehouseInputDetail = new WarehouseInputDetail();
                    WarehouseInputDetail.Active = true;
                    WarehouseInputDetail.ParentID = model.ParentID;
                    WarehouseInputDetail.ParentName = model.ParentName;
                    WarehouseInputDetail.QuantitySNP = model.QuantitySNP;
                    WarehouseInputDetail.Description = model.Description;
                    WarehouseInputDetail.Code = model.Code;
                    WarehouseInputDetail.MaterialID = model.MaterialID;
                    WarehouseInputDetail.MaterialName = model.MaterialName;
                    WarehouseInputDetail.Display = model.Display;
                    WarehouseInputDetail.CategoryUnitID = model.CategoryUnitID;
                    WarehouseInputDetail.CategoryUnitName = model.CategoryUnitName;
                    WarehouseInputDetail.CategoryLocationID = model.CategoryLocationID;
                    WarehouseInputDetail.CategoryLocationName = model.CategoryLocationName;
                    _WarehouseInputDetailRepository.Add(WarehouseInputDetail);                    
                }
                model.WarehouseInputDetailID = WarehouseInputDetail.ID;
            }

            model.QuantityInvoice = model.QuantityInvoice ?? 1;
            model.QuantityMES = model.QuantityMES ?? 1;
            model.Quantity = model.Quantity ?? model.QuantityInvoice;
            model.PKG_QTYActual = (double)(model.Quantity - model.QuantityMES);
            model.QuantityOutput = model.QuantityOutput ?? GlobalHelper.InitializationNumber;
            model.QuantityInventory = model.Quantity - model.QuantityOutput;

        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> AddHookRackAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.Count = await _WarehouseInputDetailBarcodeRepository.AddAsync(BaseParameter.BaseModel);
            result.BaseModel = BaseParameter.BaseModel;
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> UpdateHookRackAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.Count = await _WarehouseInputDetailBarcodeRepository.UpdateAsync(BaseParameter.BaseModel);
            result.BaseModel = BaseParameter.BaseModel;
            return result;
        }
        public override async Task<BaseResult<WarehouseInputDetailBarcode>> SaveAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();

            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                bool? IsScan = BaseParameter.BaseModel.IsScan;
                if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode))
                {
                    BaseParameter.BaseModel.Barcode = BaseParameter.BaseModel.Barcode.Trim();
                    if (BaseParameter.BaseModel.ParentID == null || BaseParameter.BaseModel.ParentID == 0)
                    {
                        var WarehouseInputDetailBarcodeCheck = BaseParameter.BaseModel;
                        if (BaseParameter.CategoryDepartmentID > 0)
                        {
                            var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).OrderBy(o => o.Date).ToListAsync();
                            if (ListWarehouseInput.Count > 0)
                            {
                                var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                var WarehouseInputDetailBarcodeExist = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Barcode == BaseParameter.BaseModel.Barcode).FirstOrDefaultAsync();
                                if (WarehouseInputDetailBarcodeExist != null && WarehouseInputDetailBarcodeExist.ID > 0)
                                {
                                    BaseParameter.BaseModel = WarehouseInputDetailBarcodeExist;
                                    BaseParameter.BaseModel.IsScan = WarehouseInputDetailBarcodeCheck.IsScan;
                                    BaseParameter.BaseModel.CategoryLocationName = WarehouseInputDetailBarcodeCheck.CategoryLocationName;
                                    BaseParameter.BaseModel.Active = WarehouseInputDetailBarcodeCheck.Active;
                                    BaseParameter.BaseModel.PageSize = WarehouseInputDetailBarcodeCheck.PageSize;
                                    BaseParameter.BaseModel.UpdateUserID = WarehouseInputDetailBarcodeCheck.UpdateUserID;
                                }
                            }
                        }
                    }
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && !string.IsNullOrEmpty(o.Barcode) && !string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode) && o.Barcode.Trim() == BaseParameter.BaseModel.Barcode.Trim()).FirstOrDefaultAsync();
                        SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                        if (BaseParameter.BaseModel.ID > 0)
                        {
                            result = await UpdateAsync(BaseParameter);
                        }
                        else
                        {
                            result = await AddAsync(BaseParameter);
                        }
                        result.BaseModel = BaseParameter.BaseModel;
                        if (result.BaseModel.ID > 0)
                        {
                            try
                            {
                                BaseParameter.BaseModel = result.BaseModel;
                                result = await SyncAsync(BaseParameter);
                                //if (IsScan == true)
                                //{
                                //}
                                //else
                                //{
                                //    BaseParameter.BaseModel = result.BaseModel;
                                //    result = await SyncAsync(BaseParameter);
                                //}
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SaveHookRackAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();

            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode))
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        InitializationSaveHookRack(BaseParameter.BaseModel);
                        var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && !string.IsNullOrEmpty(o.Barcode) && o.Barcode.Trim() == BaseParameter.BaseModel.Barcode.Trim()).FirstOrDefaultAsync();
                        SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                        if (BaseParameter.BaseModel.ID > 0)
                        {
                            result = await UpdateHookRackAsync(BaseParameter);
                        }
                        else
                        {
                            result = await AddHookRackAsync(BaseParameter);
                        }
                        result.BaseModel = BaseParameter.BaseModel;
                        if (result.BaseModel.ID > 0)
                        {
                            try
                            {
                                BaseParameter.BaseModel = result.BaseModel;
                                result = await SyncHookRackAsync(BaseParameter);
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ProductID == 0)
                {
                    await SyncWarehouseInputDetailBarcode0Async(BaseParameter);
                }
                else
                {
                    await SyncWarehouseInputDetailBarcodeAsync(BaseParameter);
                    await SyncWarehouseInputDetailBarcodeWithLocationAsync(BaseParameter);
                    await SyncWarehouseInputAsync(BaseParameter);
                    await SyncWarehouseInputDetailAsync(BaseParameter);
                    await SyncWarehouseInputDetailBarcodeMaterialAsync(BaseParameter);
                    //await SyncWarehouseInputDetailCountAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncHookRackAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                await SyncWarehouseInputDetailAsync(BaseParameter);
                await SyncWarehouseInputDetailBarcodeMaterialAsync(BaseParameter);
            }
            return result;
        }
        public override async Task<BaseResult<WarehouseInputDetailBarcode>> RemoveAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.BaseModel = await _WarehouseInputDetailBarcodeRepository.GetByIDAsync(BaseParameter.ID);
            if (result.BaseModel.ID > 0)
            {
                result.Count = await _WarehouseInputDetailBarcodeRepository.RemoveAsync(BaseParameter.ID);
                if (result.Count > 0)
                {
                    BaseParameter.BaseModel = result.BaseModel;
                    await SyncRemoveAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncRemoveAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            //await SyncWarehouseInputDetailBarcodeStockAsync(BaseParameter);
            await SyncWarehouseInputAsync(BaseParameter);
            await SyncWarehouseInputDetailAsync(BaseParameter);
            //await SyncWarehouseInventoryFromBaseParameterAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseInputDetailBarcodeStockAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.MaterialID > 0)
                        {
                            if (BaseParameter.BaseModel.CategoryDepartmentID > 0)
                            {
                                if (BaseParameter.BaseModel.IsStock != true)
                                {
                                    var WarehouseInputDetailBarcode = await GetByCondition(o => o.IsStock == true && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Active == true && o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID).OrderByDescending(o => o.DateScan).FirstOrDefaultAsync();
                                    if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                                    {
                                        var ListWarehouseInputDetailBarcode = await GetByCondition(o => o.UpdateDate != null && o.UpdateDate.Value.Year >= GlobalHelper.YearStock && o.IsStock != true && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.Active == true && o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID).OrderByDescending(o => o.DateScan).ToListAsync();
                                        WarehouseInputDetailBarcode.QuantityMES = ListWarehouseInputDetailBarcode.Sum(o => o.Quantity);
                                        WarehouseInputDetailBarcode.QuantityOutput = ListWarehouseInputDetailBarcode.Sum(o => o.QuantityOutput);
                                        WarehouseInputDetailBarcode.Quantity = WarehouseInputDetailBarcode.QuantitySNP + WarehouseInputDetailBarcode.QuantityMES;
                                        WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityOutput;
                                        await _WarehouseInputDetailBarcodeRepository.UpdateAsync(WarehouseInputDetailBarcode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseInputDetailBarcodeMaterialAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.MaterialID > 0)
                        {
                            if (BaseParameter.BaseModel.BOMID > 0)
                            {
                                var ListBOMDetail = await _BOMDetailRepository.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.BOMID).ToListAsync();
                                foreach (var BOMDetail in ListBOMDetail)
                                {

                                    var WarehouseInputDetailBarcodeMaterial = new WarehouseInputDetailBarcodeMaterial();
                                    WarehouseInputDetailBarcodeMaterial.Active = true;
                                    WarehouseInputDetailBarcodeMaterial.Display = BaseParameter.BaseModel.Barcode;
                                    WarehouseInputDetailBarcodeMaterial.WarehouseInputDetailBarcodeID = BaseParameter.BaseModel.ID;
                                    WarehouseInputDetailBarcodeMaterial.CompanyID = BaseParameter.BaseModel.CompanyID;
                                    WarehouseInputDetailBarcodeMaterial.CompanyName = BaseParameter.BaseModel.CompanyName;
                                    WarehouseInputDetailBarcodeMaterial.ParentID = BaseParameter.BaseModel.ParentID;
                                    WarehouseInputDetailBarcodeMaterial.ParentName = BaseParameter.BaseModel.ParentName;
                                    WarehouseInputDetailBarcodeMaterial.MaterialID01 = BaseParameter.BaseModel.MaterialID;
                                    WarehouseInputDetailBarcodeMaterial.MaterialName01 = BaseParameter.BaseModel.MaterialName;
                                    WarehouseInputDetailBarcodeMaterial.BOMID = BaseParameter.BaseModel.BOMID;
                                    WarehouseInputDetailBarcodeMaterial.ECN = BaseParameter.BaseModel.ECN;
                                    WarehouseInputDetailBarcodeMaterial.BOMECNVersion = BaseParameter.BaseModel.BOMECNVersion;
                                    WarehouseInputDetailBarcodeMaterial.BOMDate = BaseParameter.BaseModel.BOMDate;
                                    WarehouseInputDetailBarcodeMaterial.CategoryMaterialID = BOMDetail.ID;
                                    WarehouseInputDetailBarcodeMaterial.MaterialID = BOMDetail.MaterialID02;
                                    WarehouseInputDetailBarcodeMaterial.MaterialName = BOMDetail.MaterialCode02;
                                    WarehouseInputDetailBarcodeMaterial.CategoryUnitID = BOMDetail.CategoryUnitID02;
                                    WarehouseInputDetailBarcodeMaterial.CategoryUnitName = BOMDetail.CategoryUnitName02;
                                    WarehouseInputDetailBarcodeMaterial.Quantity = BOMDetail.Quantity02;
                                    var BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcodeMaterial>();
                                    BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcodeMaterial;
                                    await _WarehouseInputDetailBarcodeMaterialService.SaveAsync(BaseParameterWarehouseInputDetailBarcode);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseInputDetailCountAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.MaterialID > 0)
                        {
                            if (BaseParameter.BaseModel.BOMID > 0)
                            {
                                if (!string.IsNullOrEmpty(BaseParameter.BaseModel.ECN))
                                {
                                    var WarehouseInputDetailCount = new WarehouseInputDetailCount();
                                    WarehouseInputDetailCount.ParentID = BaseParameter.BaseModel.ParentID;
                                    WarehouseInputDetailCount.ParentName = BaseParameter.BaseModel.ParentName;
                                    WarehouseInputDetailCount.CompanyID = BaseParameter.BaseModel.CompanyID;
                                    WarehouseInputDetailCount.CompanyName = BaseParameter.BaseModel.CompanyName;
                                    WarehouseInputDetailCount.MaterialID = BaseParameter.BaseModel.MaterialID;
                                    WarehouseInputDetailCount.MaterialName = BaseParameter.BaseModel.MaterialName;
                                    WarehouseInputDetailCount.BOMID = BaseParameter.BaseModel.BOMID;
                                    WarehouseInputDetailCount.ECN = BaseParameter.BaseModel.ECN;
                                    WarehouseInputDetailCount.Date = BaseParameter.BaseModel.DateScan ?? GlobalHelper.InitializationDateTime;
                                    var ListWarehouseInputDetailBarcode = _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID == WarehouseInputDetailCount.ParentID && o.MaterialID == WarehouseInputDetailCount.MaterialID && o.BOMID == WarehouseInputDetailCount.BOMID && o.DateScan != null && o.DateScan.Value.Date == WarehouseInputDetailCount.Date.Value.Date).ToList();
                                    WarehouseInputDetailCount.Count = ListWarehouseInputDetailBarcode.Count;
                                    var WarehouseInputDetailCountCheck = await _WarehouseInputDetailCountRepository.GetByCondition(o => o.ParentID == WarehouseInputDetailCount.ParentID && o.MaterialID == WarehouseInputDetailCount.MaterialID && o.BOMID == WarehouseInputDetailCount.BOMID && o.Date != null && WarehouseInputDetailCount.Date != null && o.Date.Value.Date == WarehouseInputDetailCount.Date.Value.Date).FirstOrDefaultAsync();
                                    if (WarehouseInputDetailCountCheck == null)
                                    {
                                        await _WarehouseInputDetailCountRepository.AddAsync(WarehouseInputDetailCount);
                                    }
                                    else
                                    {
                                        WarehouseInputDetailCount.ID = WarehouseInputDetailCountCheck.ID;
                                        await _WarehouseInputDetailCountRepository.UpdateAsync(WarehouseInputDetailCount);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseInputDetailBarcode0Async(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.ProductID == 0)
                        {
                            var ListWarehouseInputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.WarehouseInputDetailID == BaseParameter.BaseModel.WarehouseInputDetailID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.QuantitySNP == BaseParameter.BaseModel.QuantitySNP && o.Description == BaseParameter.BaseModel.Description && o.ProductID > 0).ToListAsync();
                            if (ListWarehouseInputDetailBarcode.Count == 0)
                            {
                                ListWarehouseInputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.WarehouseInputDetailID == BaseParameter.BaseModel.WarehouseInputDetailID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.QuantitySNP == BaseParameter.BaseModel.QuantitySNP && o.ProductID > 0).ToListAsync();
                            }
                            if (ListWarehouseInputDetailBarcode.Count > 0)
                            {
                                for (int i = 0; i < ListWarehouseInputDetailBarcode.Count; i++)
                                {
                                    ListWarehouseInputDetailBarcode[i].Active = BaseParameter.BaseModel.Active;
                                    ListWarehouseInputDetailBarcode[i].DateScan = BaseParameter.BaseModel.DateScan;
                                    ListWarehouseInputDetailBarcode[i].UpdateDate = BaseParameter.BaseModel.UpdateDate;
                                    ListWarehouseInputDetailBarcode[i].UpdateUserID = BaseParameter.BaseModel.UpdateUserID;
                                    ListWarehouseInputDetailBarcode[i].UpdateUserCode = BaseParameter.BaseModel.UpdateUserCode;
                                    ListWarehouseInputDetailBarcode[i].UpdateUserName = BaseParameter.BaseModel.UpdateUserName;
                                    ListWarehouseInputDetailBarcode[i].CategoryLocationID = BaseParameter.BaseModel.CategoryLocationID;
                                    ListWarehouseInputDetailBarcode[i].CategoryLocationName = BaseParameter.BaseModel.CategoryLocationName;
                                }
                                await _WarehouseInputDetailBarcodeRepository.UpdateRangeAsync(ListWarehouseInputDetailBarcode);


                            }

                            var WarehouseInputDetail = await _WarehouseInputDetailService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.ID == BaseParameter.BaseModel.WarehouseInputDetailID).FirstOrDefaultAsync();
                            if (WarehouseInputDetail == null)
                            {
                                WarehouseInputDetail = await _WarehouseInputDetailService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
                            }
                            if (WarehouseInputDetail == null)
                            {
                                WarehouseInputDetail = await _WarehouseInputDetailService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialName == BaseParameter.BaseModel.MaterialName).FirstOrDefaultAsync();
                            }
                            if (WarehouseInputDetail != null)
                            {
                                if (WarehouseInputDetail.ID > 0)
                                {
                                    var BaseParameterWarehouseInputDetail = new BaseParameter<WarehouseInputDetail>();
                                    BaseParameterWarehouseInputDetail.BaseModel = WarehouseInputDetail;
                                    await _WarehouseInputDetailService.SaveAsync(BaseParameterWarehouseInputDetail);
                                }
                            }

                            ListWarehouseInputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID.Value).ToListAsync();
                            if (ListWarehouseInputDetailBarcode.Count > 0)
                            {
                                var WarehouseInput = await _WarehouseInputRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                                if (WarehouseInput.ID > 0)
                                {
                                    WarehouseInput.Total = ListWarehouseInputDetailBarcode.Where(o => o.Active == true).Sum(o => o.Total);
                                    await _WarehouseInputRepository.UpdateAsync(WarehouseInput);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseInputDetailBarcodeAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.ProductID != null)
                        {
                            var ListWarehouseInputDetailBarcode = new List<WarehouseInputDetailBarcode>();
                            if (BaseParameter.BaseModel.ProductID == 0)
                            {
                                ListWarehouseInputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.WarehouseInputDetailID == BaseParameter.BaseModel.WarehouseInputDetailID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.QuantitySNP == BaseParameter.BaseModel.QuantitySNP && o.Description == BaseParameter.BaseModel.Description && o.ProductID > 0).ToListAsync();
                                if (ListWarehouseInputDetailBarcode.Count == 0)
                                {
                                    ListWarehouseInputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.WarehouseInputDetailID == BaseParameter.BaseModel.WarehouseInputDetailID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.QuantitySNP == BaseParameter.BaseModel.QuantitySNP && o.ProductID > 0).ToListAsync();
                                }
                                if (ListWarehouseInputDetailBarcode.Count > 0)
                                {
                                    foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                                    {
                                        WarehouseInputDetailBarcode.Active = BaseParameter.BaseModel.Active;
                                        WarehouseInputDetailBarcode.DateScan = BaseParameter.BaseModel.DateScan;
                                        WarehouseInputDetailBarcode.UpdateDate = BaseParameter.BaseModel.UpdateDate;
                                        WarehouseInputDetailBarcode.UpdateUserID = BaseParameter.BaseModel.UpdateUserID;
                                        WarehouseInputDetailBarcode.UpdateUserCode = BaseParameter.BaseModel.UpdateUserCode;
                                        WarehouseInputDetailBarcode.UpdateUserName = BaseParameter.BaseModel.UpdateUserName;
                                        var BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                                        BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                                        await SaveAsync(BaseParameterWarehouseInputDetailBarcode);
                                    }
                                }
                                else
                                {
                                    ListWarehouseInputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.WarehouseInputDetailID == BaseParameter.BaseModel.WarehouseInputDetailID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.QuantitySNP == BaseParameter.BaseModel.QuantitySNP && o.Description == BaseParameter.BaseModel.Description && o.ProductID == 0).ToListAsync();
                                    if (ListWarehouseInputDetailBarcode.Count == 0)
                                    {
                                        ListWarehouseInputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.WarehouseInputDetailID == BaseParameter.BaseModel.WarehouseInputDetailID && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.QuantitySNP == BaseParameter.BaseModel.QuantitySNP && o.ProductID == 0).ToListAsync();
                                    }
                                    if (ListWarehouseInputDetailBarcode.Count > 0)
                                    {
                                        foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                                        {
                                            WarehouseInputDetailBarcode.Active = BaseParameter.BaseModel.Active;
                                            WarehouseInputDetailBarcode.DateScan = BaseParameter.BaseModel.DateScan;
                                            WarehouseInputDetailBarcode.UpdateDate = BaseParameter.BaseModel.UpdateDate;
                                            WarehouseInputDetailBarcode.UpdateUserID = BaseParameter.BaseModel.UpdateUserID;
                                            WarehouseInputDetailBarcode.UpdateUserCode = BaseParameter.BaseModel.UpdateUserCode;
                                            WarehouseInputDetailBarcode.UpdateUserName = BaseParameter.BaseModel.UpdateUserName;
                                            await _WarehouseInputDetailBarcodeRepository.UpdateAsync(WarehouseInputDetailBarcode);
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
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseInputDetailBarcodeWithLocationAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.MaterialID > 0)
                        {
                            if (BaseParameter.BaseModel.ProductID == 0)
                            {
                                var ListWarehouseInputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID.Value && o.MaterialID == BaseParameter.BaseModel.MaterialID && o.ProductID > 0).ToListAsync();
                                if (ListWarehouseInputDetailBarcode.Count > 0)
                                {
                                    if (BaseParameter.BaseModel.PageSize > 0)
                                    {
                                        BaseParameter.BaseModel.Page = BaseParameter.BaseModel.Page ?? 0;
                                        ListWarehouseInputDetailBarcode = ListWarehouseInputDetailBarcode.Skip(BaseParameter.BaseModel.Page.Value).Take(BaseParameter.BaseModel.PageSize.Value).ToList();
                                    }
                                    foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                                    {
                                        WarehouseInputDetailBarcode.CategoryLocationID = BaseParameter.BaseModel.CategoryLocationID;
                                        WarehouseInputDetailBarcode.CategoryLocationName = BaseParameter.BaseModel.CategoryLocationName;
                                        await _WarehouseInputDetailBarcodeRepository.UpdateAsync(WarehouseInputDetailBarcode);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseInputAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        var ListWarehouseInputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID.Value).ToListAsync();
                        if (ListWarehouseInputDetailBarcode.Count > 0)
                        {
                            var WarehouseInput = await _WarehouseInputRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                            if (WarehouseInput.ID > 0)
                            {
                                WarehouseInput.Total = ListWarehouseInputDetailBarcode.Where(o => o.Active == true).Sum(o => o.Total);
                                await _WarehouseInputRepository.UpdateAsync(WarehouseInput);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseInputDetailAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.ProductID != 0)
                        {
                            if (BaseParameter.BaseModel.WarehouseInputDetailID > 0 || BaseParameter.BaseModel.MaterialID > 0 || !string.IsNullOrEmpty(BaseParameter.BaseModel.MaterialName))
                            {
                                var WarehouseInputDetail = await _WarehouseInputDetailService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.ID == BaseParameter.BaseModel.WarehouseInputDetailID).FirstOrDefaultAsync();
                                if (WarehouseInputDetail == null)
                                {
                                    WarehouseInputDetail = await _WarehouseInputDetailService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
                                }
                                if (WarehouseInputDetail == null)
                                {
                                    WarehouseInputDetail = await _WarehouseInputDetailService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialName == BaseParameter.BaseModel.MaterialName).FirstOrDefaultAsync();
                                }
                                if (WarehouseInputDetail != null)
                                {
                                    if (WarehouseInputDetail.ID > 0)
                                    {
                                        var BaseParameterWarehouseInputDetail = new BaseParameter<WarehouseInputDetail>();
                                        BaseParameterWarehouseInputDetail.BaseModel = WarehouseInputDetail;
                                        await _WarehouseInputDetailService.SaveAsync(BaseParameterWarehouseInputDetail);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseInventoryAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    BaseParameter<WarehouseInventory> BaseParameterWarehourseInventory = new BaseParameter<WarehouseInventory>();
                    BaseParameterWarehourseInventory.ID = BaseParameter.BaseModel.ID;
                    await _WarehourseInventoryService.SyncByWarehouseInputDetailBarcodeAsync(BaseParameterWarehourseInventory);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseInventoryFromBaseParameterAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    await _WarehourseInventoryService.SyncByWarehouseInputDetailBarcodeFromBaseParameterAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncWarehouseStockAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.DateScan != null)
                        {
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByWarehouseInputDetailIDToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = await GetByCondition(o => o.WarehouseInputDetailID == BaseParameter.GeneralID).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByParentID_MaterialIDToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.MaterialID == BaseParameter.GeneralID).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            await MySQLHelper.ERPSyncAsync(GlobalHelper.ERP_MariaDBConectionString);
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                var ListWarehouseInput = new List<WarehouseInput>();
                ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                //if (BaseParameter.Year == 0)
                //{
                //    ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                //}
                //else
                //{
                //    if (BaseParameter.Month == 0)
                //    {
                //        ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Year == BaseParameter.Year).ToListAsync();
                //    }
                //    else
                //    {
                //        ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Year == BaseParameter.Year && o.Date.Value.Month == BaseParameter.Month).ToListAsync();
                //    }
                //}
                if (ListWarehouseInput.Count > 0)
                {
                    var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                        var MaterialConvert = await _MaterialConvertRepository.GetByCondition(o => o.Active == true && (o.ParentName == BaseParameter.SearchString || o.Code == BaseParameter.SearchString)).FirstOrDefaultAsync();
                        var ListMaterialConvert = new List<MaterialConvert>();
                        if (MaterialConvert != null && MaterialConvert.ID > 0 && MaterialConvert.ParentID > 0)
                        {
                            ListMaterialConvert = await _MaterialConvertRepository.GetByCondition(o => o.Active == true && o.ParentID == MaterialConvert.ParentID).ToListAsync();
                            if (ListMaterialConvert.Count > 0)
                            {
                                var ListMaterialConvertCode = ListMaterialConvert.Select(o => o.Code).Distinct().ToList();
                                ListMaterialConvertCode.Add(MaterialConvert.ParentName);
                            }
                        }
                        if (BaseParameter.Year == 0)
                        {
                            if (MaterialConvert != null && MaterialConvert.ID > 0 && MaterialConvert.ParentID > 0)
                            {
                                if (ListMaterialConvert.Count > 0)
                                {
                                    var ListMaterialConvertCode = ListMaterialConvert.Select(o => o.Code).Distinct().ToList();
                                    ListMaterialConvertCode.Add(MaterialConvert.ParentName);
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && ListMaterialConvertCode.Contains(o.MaterialName)).ToListAsync();
                                }
                            }
                            else
                            {
                                result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.MaterialName == BaseParameter.SearchString).ToListAsync();
                                if (result.List.Count == 0)
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Barcode == BaseParameter.SearchString).ToListAsync();
                                }
                                if (result.List.Count == 0)
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.ParentName == BaseParameter.SearchString).ToListAsync();
                                }
                                if (result.List.Count == 0)
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.CategoryLocationName == BaseParameter.SearchString).ToListAsync();
                                }
                                if (result.List.Count == 0)
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.FileName == BaseParameter.SearchString).ToListAsync();
                                }
                            }
                        }
                        else
                        {
                            if (BaseParameter.Month == 0)
                            {
                                if (MaterialConvert != null && MaterialConvert.ID > 0 && MaterialConvert.ParentID > 0)
                                {
                                    if (ListMaterialConvert.Count > 0)
                                    {
                                        var ListMaterialConvertCode = ListMaterialConvert.Select(o => o.Code).Distinct().ToList();
                                        ListMaterialConvertCode.Add(MaterialConvert.ParentName);
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && ListMaterialConvertCode.Contains(o.MaterialName) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                    }
                                }
                                else
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.MaterialName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                    if (result.List.Count == 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Barcode == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                    }
                                    if (result.List.Count == 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.ParentName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                    }
                                    if (result.List.Count == 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.CategoryLocationName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                    }
                                    if (result.List.Count == 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.FileName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                    }
                                }
                            }
                            else
                            {
                                if (MaterialConvert != null && MaterialConvert.ID > 0 && MaterialConvert.ParentID > 0)
                                {
                                    if (ListMaterialConvert.Count > 0)
                                    {
                                        var ListMaterialConvertCode = ListMaterialConvert.Select(o => o.Code).Distinct().ToList();
                                        ListMaterialConvertCode.Add(MaterialConvert.ParentName);
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && ListMaterialConvertCode.Contains(o.MaterialName) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                }
                                else
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.MaterialName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                    if (result.List.Count == 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Barcode == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                    if (result.List.Count == 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.ParentName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                    if (result.List.Count == 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.CategoryLocationName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                    if (result.List.Count == 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.FileName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (BaseParameter.Active == true)
                        {
                            if (BaseParameter.Year > 0)
                            {
                                if (BaseParameter.Month > 0)
                                {
                                    if (BaseParameter.Day > 0)
                                    {
                                        result.List = await GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.DateScan.Value.Day == BaseParameter.Day).ToListAsync();
                                    }
                                    else
                                    {
                                        result.List = await GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                }
                                else
                                {
                                    result.List = await GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                }
                            }
                        }
                        else
                        {
                            if (BaseParameter.Year > 0)
                            {
                                if (BaseParameter.Month > 0)
                                {
                                    if (BaseParameter.Day > 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.UpdateDate != null && o.UpdateDate.Value.Year == BaseParameter.Year && o.UpdateDate.Value.Month == BaseParameter.Month && o.UpdateDate.Value.Day == BaseParameter.Day).ToListAsync();
                                    }
                                    else
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.UpdateDate != null && o.UpdateDate.Value.Year == BaseParameter.Year && o.UpdateDate.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                }
                                else
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.UpdateDate != null && o.UpdateDate.Value.Year == BaseParameter.Year).ToListAsync();
                                }
                            }
                        }
                    }
                }
            }
            if (result.List.Count > 0)
            {
                if (BaseParameter.IsComplete == true)
                {
                    result.List = result.List.Where(o => o.QuantityInventory > 0).ToList();
                }
                result.List = result.List.OrderByDescending(o => o.DateScan).ThenBy(o => o.ParentName).ThenBy(o => o.MaterialName).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_FIFO_StockToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                {
                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                        var MaterialConvert = await _MaterialConvertRepository.GetByCondition(o => o.Active == true && (o.ParentName == BaseParameter.SearchString || o.Code == BaseParameter.SearchString)).FirstOrDefaultAsync();
                        var ListMaterialConvert = new List<MaterialConvert>();
                        if (MaterialConvert != null && MaterialConvert.ID > 0 && MaterialConvert.ParentID > 0)
                        {
                            ListMaterialConvert = await _MaterialConvertRepository.GetByCondition(o => o.Active == true && o.ParentID == MaterialConvert.ParentID).ToListAsync();
                        }
                        if (MaterialConvert != null && MaterialConvert.ID > 0 && MaterialConvert.ParentID > 0)
                        {
                            if (ListMaterialConvert.Count > 0)
                            {
                                var ListMaterialConvertCode = ListMaterialConvert.Select(o => o.Code).Distinct().ToList();
                                ListMaterialConvertCode.Add(MaterialConvert.ParentName);
                                result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && ListMaterialConvertCode.Contains(o.MaterialName) && o.DateScan != null && o.DateScan.Value.Date >= BaseParameter.DateBegin.Value.Date && o.DateScan.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                            }
                        }
                        else
                        {
                            result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.MaterialName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Date >= BaseParameter.DateBegin.Value.Date && o.DateScan.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                            if (result.List.Count == 0)
                            {
                                result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Barcode == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Date >= BaseParameter.DateBegin.Value.Date && o.DateScan.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                            }
                            if (result.List.Count == 0)
                            {
                                result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.ParentName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Date >= BaseParameter.DateBegin.Value.Date && o.DateScan.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                            }
                            if (result.List.Count == 0)
                            {
                                result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.CategoryLocationName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Date >= BaseParameter.DateBegin.Value.Date && o.DateScan.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                            }
                            if (result.List.Count == 0)
                            {
                                result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.FileName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Date >= BaseParameter.DateBegin.Value.Date && o.DateScan.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                            }
                        }
                    }
                    else
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.DateScan != null && o.DateScan.Value.Date >= BaseParameter.DateBegin.Value.Date && o.DateScan.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                    }
                }
            }
            if (result.List.Count > 0)
            {
                if (BaseParameter.Active == true)
                {
                    result.List = result.List.Where(o => o.Active == BaseParameter.Active).ToList();
                }
                if (BaseParameter.IsComplete == true)
                {
                    result.List = result.List.Where(o => o.QuantityInventory > 0).ToList();
                }
                result.List = result.List.OrderByDescending(o => o.DateScan).ThenBy(o => o.ParentName).ThenBy(o => o.MaterialName).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> AutoSyncAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            //var List = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == 188 && o.Active == true && o.QuantityInventory > 0).OrderBy(o => o.DateScan).ToListAsync();
            //for (int i = 0; i < List.Count; i++)
            //{
            //    if (List[i].DateScan == null)
            //    {
            //        List[i].DateScan = GlobalHelper.InitializationDateTime;
            //    }
            //    List[i].Year = List[i].DateScan.Value.Year;
            //    List[i].Month = List[i].DateScan.Value.Month;
            //    List[i].Day = List[i].DateScan.Value.Day;
            //    List[i].Week = GlobalHelper.GetWeekByDateTime(List[i].DateScan.Value);
            //}
            //await _WarehouseInputDetailBarcodeRepository.UpdateRangeAsync(List);
            await MySQLHelper.ERPSyncAsync(GlobalHelper.ERP_MariaDBConectionString);
            await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_Year_Month_Day_Active_SearchString_InventoryToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                if (ListWarehouseInput.Count > 0)
                {
                    var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Barcode == BaseParameter.SearchString).ToListAsync();
                        if (result.List.Count == 0)
                        {
                            result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.MaterialName == BaseParameter.SearchString).ToListAsync();
                        }
                        if (result.List.Count == 0)
                        {
                            result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.ParentName == BaseParameter.SearchString).ToListAsync();
                        }
                    }
                    else
                    {
                        if (BaseParameter.Active == true)
                        {
                            if (BaseParameter.Year > 0)
                            {
                                if (BaseParameter.Month > 0)
                                {
                                    if (BaseParameter.Day > 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.DateScan.Value.Day == BaseParameter.Day).ToListAsync();
                                    }
                                    else
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                }
                                else
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                }
                            }
                        }
                        else
                        {
                            if (BaseParameter.Year > 0)
                            {
                                if (BaseParameter.Month > 0)
                                {
                                    if (BaseParameter.Day > 0)
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.UpdateDate != null && o.UpdateDate.Value.Year == BaseParameter.Year && o.UpdateDate.Value.Month == BaseParameter.Month && o.UpdateDate.Value.Day == BaseParameter.Day).ToListAsync();
                                    }
                                    else
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.UpdateDate != null && o.UpdateDate.Value.Year == BaseParameter.Year && o.UpdateDate.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                }
                                else
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.UpdateDate != null && o.UpdateDate.Value.Year == BaseParameter.Year).ToListAsync();
                                }
                            }
                        }
                    }
                }
            }
            if (result.List.Count > 0)
            {
                result.List = result.List.OrderByDescending(o => o.UpdateDate).ThenBy(o => o.ParentID).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                result.List = await GetByCondition(o => !string.IsNullOrEmpty(o.Barcode) && o.Barcode.Contains(BaseParameter.SearchString)).ToListAsync();
            }
            if (result.List.Count > 0)
            {
                result.List = result.List.OrderByDescending(o => o.DateScan).ThenBy(o => o.ParentID).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_BarcodeAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.BaseModel = new WarehouseInputDetailBarcode();
            if (BaseParameter.CategoryDepartmentID > 0 && !string.IsNullOrEmpty(BaseParameter.Barcode))
            {
                BaseParameter.Barcode = BaseParameter.Barcode.Trim();
                result.BaseModel = await GetByCondition(o => o.Active == true && o.ParentID > 0 && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Barcode == BaseParameter.Barcode).OrderBy(o => o.DateScan).FirstOrDefaultAsync();
                if (result.BaseModel == null)
                {
                    await MySQLHelper.ERPSyncAsync02(GlobalHelper.MariaDBConectionString);
                    await MySQLHelper.ERPSyncAsync02(GlobalHelper.MariaDBConectionStringDJM);
                    result.BaseModel = await GetByCondition(o => o.Active == true && o.ParentID > 0 && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Barcode == BaseParameter.Barcode).OrderBy(o => o.DateScan).FirstOrDefaultAsync();
                }
                if (result.BaseModel == null)
                {
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                        result.BaseModel = await GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Barcode == BaseParameter.Barcode).FirstOrDefaultAsync();
                    }
                }
            }
            if (result.BaseModel == null)
            {
                result.BaseModel = new WarehouseInputDetailBarcode();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeFromtmbrcdAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.BaseModel = new WarehouseInputDetailBarcode();
            if (BaseParameter.CompanyID > 0 && !string.IsNullOrEmpty(BaseParameter.Barcode))
            {
                decimal Quantity = -1;
                BaseParameter.Barcode = BaseParameter.Barcode.Trim();

                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                string sql = @"select * from tmbrcd where BARCD_ID='" + BaseParameter.Barcode + "'";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                var tmbrcd = new tmbrcd();

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    var Listtmbrcd = new List<tmbrcd>();
                    Listtmbrcd.AddRange(SQLHelper.ToList<tmbrcd>(dt));
                    if (Listtmbrcd.Count > 0)
                    {
                        tmbrcd = Listtmbrcd[0];
                    }
                }
                //var tmbrcd = await _tmbrcdRepository.GetByCondition(o => !string.IsNullOrEmpty(o.BARCD_ID) && o.BARCD_ID == BaseParameter.Barcode).OrderByDescending(o => o.UPDATE_DTM).FirstOrDefaultAsync();
                if (tmbrcd != null && tmbrcd.BARCD_IDX > 0)
                {
                    Quantity = (decimal)tmbrcd.PKG_QTY - (decimal)tmbrcd.PKG_OUTQTY;
                    result.BaseModel.Barcode = tmbrcd.BARCD_ID;
                    result.BaseModel.Quantity = Quantity;
                    result.BaseModel.MESID = tmbrcd.BARCD_IDX;

                    //var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 39 || o.ParentID == 66).ToListAsync();
                    //result.BaseModel = ListWarehouseInputDetailBarcode.Where(o => o.Barcode == BaseParameter.Barcode).FirstOrDefault();
                    //if (result.BaseModel != null && result.BaseModel.ID > 0)
                    //{
                    //    result.BaseModel.Quantity = Quantity;
                    //}

                    //var ListCategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.Active == true && o.IsSync == true).ToListAsync();
                    //if (ListCategoryDepartment.Count > 0)
                    //{
                    //    var ListCategoryDepartmentID = ListCategoryDepartment.Select(x => x.ID).ToList();
                    //    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID > 0 && ListCategoryDepartmentID.Contains(o.CustomerID.Value)).OrderBy(o => o.Date).ToListAsync();
                    //    var ListWarehouseInputID = ListWarehouseInput.Select(x => x.ID).ToList();
                    //    var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value)).ToListAsync();
                    //    result.BaseModel = ListWarehouseInputDetailBarcode.Where(o => o.Barcode == BaseParameter.Barcode).FirstOrDefault();
                    //    if (result.BaseModel != null && result.BaseModel.ID > 0)
                    //    {
                    //        result.BaseModel.Quantity = Quantity;
                    //    }
                    //}
                }
                else
                {
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == 23 && o.ID != 39).OrderBy(o => o.Date).ToListAsync();
                    var ListWarehouseInputID = ListWarehouseInput.Select(x => x.ID).ToList();
                    var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value)).ToListAsync();
                    result.BaseModel = ListWarehouseInputDetailBarcode.Where(o => o.Barcode == BaseParameter.Barcode).FirstOrDefault();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeFromtdpdmtimAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.BaseModel = new WarehouseInputDetailBarcode();
            if (BaseParameter.CompanyID > 0 && !string.IsNullOrEmpty(BaseParameter.Barcode))
            {
                BaseParameter.Barcode = BaseParameter.Barcode.Trim();

                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                string sql = @"select * from tdpdmtim where VLID_DSCN_YN!='Y' AND (VLID_BARCODE='" + BaseParameter.Barcode + "' OR VLID_GRP='" + BaseParameter.Barcode + "')";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                var Listtdpdmtim = new List<tdpdmtim>();

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    Listtdpdmtim.AddRange(SQLHelper.ToList<tdpdmtim>(dt));
                }

                //var Listtdpdmtim = await _tdpdmtimRepository.GetByCondition(o => o.VLID_DSCN_YN != "Y" && ((!string.IsNullOrEmpty(o.VLID_BARCODE) && o.VLID_BARCODE == BaseParameter.Barcode) || (!string.IsNullOrEmpty(o.VLID_GRP) && o.VLID_GRP == BaseParameter.Barcode))).OrderByDescending(o => o.VLID_DTM).ToListAsync();
                if (Listtdpdmtim.Count == 0)
                {
                    var tdpdmtim = new tdpdmtim();
                    tdpdmtim.VLID_DTM = GlobalHelper.InitializationDateTime;
                    tdpdmtim.VLID_BARCODE = BaseParameter.Barcode;
                    //var BarcodeSub = BaseParameter.Barcode.Replace("DJ", "#");
                    //BarcodeSub = BaseParameter.Barcode.Replace("dj", "#");
                    //BarcodeSub = BarcodeSub.Split('#')[0];
                    //if (BarcodeSub.Contains(" ") == true)
                    //{
                    //    BarcodeSub = BarcodeSub.Split(' ')[BarcodeSub.Split(' ').Length - 1];
                    //}
                    //else
                    //{
                    //    int Begin = BarcodeSub.Length - 4;
                    //    BarcodeSub = BarcodeSub.Substring(Begin);
                    //}
                    switch (BaseParameter.CompanyID)
                    {
                        case 16:
                            var BarcodeSub = BaseParameter.Barcode.Substring(13, 4);
                            var YearString = BarcodeSub.Substring(0, 1);
                            var MonthString = BarcodeSub.Substring(1, 1);
                            var DayString = BarcodeSub.Substring(2, 2);
                            var Month = 1;
                            var Year = 1;
                            switch (YearString)
                            {
                                case "0":
                                    Year = 2020;
                                    break;
                                case "1":
                                    Year = 2021;
                                    break;
                                case "2":
                                    Year = 2022;
                                    break;
                                case "3":
                                    Year = 2023;
                                    break;
                                case "4":
                                    Year = 2024;
                                    break;
                                case "5":
                                    Year = 2025;
                                    break;
                                case "6":
                                    Year = 2016;
                                    break;
                                case "7":
                                    Year = 2017;
                                    break;
                                case "8":
                                    Year = 2018;
                                    break;
                                case "9":
                                    Year = 2019;
                                    break;
                            }
                            switch (MonthString)
                            {
                                case "A":
                                    Month = 1;
                                    break;
                                case "B":
                                    Month = 2;
                                    break;
                                case "C":
                                    Month = 3;
                                    break;
                                case "D":
                                    Month = 4;
                                    break;
                                case "E":
                                    Month = 5;
                                    break;
                                case "F":
                                    Month = 6;
                                    break;
                                case "G":
                                    Month = 7;
                                    break;
                                case "H":
                                    Month = 8;
                                    break;
                                case "J":
                                    Month = 9;
                                    break;
                                case "K":
                                    Month = 10;
                                    break;
                                case "L":
                                    Month = 11;
                                    break;
                                case "M":
                                    Month = 12;
                                    break;
                            }
                            try
                            {
                                tdpdmtim.VLID_DTM = new DateTime(Year, Month, int.Parse(DayString));
                            }
                            catch
                            {
                                tdpdmtim.VLID_DTM = GlobalHelper.InitializationDateTime;
                            }
                            break;
                        case 17:
                            try
                            {
                                var BarcodeSub17 = BaseParameter.Barcode.Split(' ')[BaseParameter.Barcode.Split(' ').Length - 1];
                                BarcodeSub17 = BarcodeSub17.Trim();
                                var YearString17 = "20" + BarcodeSub17.Substring(0, 2);
                                var MonthString17 = BarcodeSub17.Substring(2, 2);
                                var DayString17 = BarcodeSub17.Substring(4, 2);
                                var ProductName = BarcodeSub17.Substring(6, 4);
                                tdpdmtim.VLID_DTM = new DateTime(int.Parse(YearString17), int.Parse(MonthString17), int.Parse(DayString17));
                            }
                            catch
                            {
                                tdpdmtim.VLID_DTM = GlobalHelper.InitializationDateTime;
                            }
                            break;
                    }

                    Listtdpdmtim.Add(tdpdmtim);
                }
                result.List = new List<WarehouseInputDetailBarcode>();
                if (Listtdpdmtim.Count > 0)
                {
                    //var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == 113 || o.ParentID == 122).ToListAsync();
                    foreach (var tdpdmtim in Listtdpdmtim)
                    {
                        //var WarehouseInputDetailBarcode = ListWarehouseInputDetailBarcode.Where(o => o.Barcode == BaseParameter.Barcode).FirstOrDefault();
                        //if (WarehouseInputDetailBarcode == null)
                        //{
                        //    WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                        //    WarehouseInputDetailBarcode.Quantity = 0;
                        //}

                        var WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                        WarehouseInputDetailBarcode.MESID = tdpdmtim.PDMTIN_IDX;
                        WarehouseInputDetailBarcode.Description = tdpdmtim.VLID_GRP;
                        WarehouseInputDetailBarcode.Barcode = tdpdmtim.VLID_BARCODE;
                        WarehouseInputDetailBarcode.DateScan = tdpdmtim.VLID_DTM;
                        result.List.Add(WarehouseInputDetailBarcode);
                    }

                    //var ListCategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.Active == true && o.Code == "FinishGoods").ToListAsync();
                    //if (ListCategoryDepartment.Count > 0)
                    //{
                    //    var ListCategoryDepartmentID = ListCategoryDepartment.Select(x => x.ID).ToList();
                    //    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID > 0 && ListCategoryDepartmentID.Contains(o.CustomerID.Value)).OrderBy(o => o.Date).ToListAsync();
                    //    var ListWarehouseInputID = ListWarehouseInput.Select(x => x.ID).ToList();
                    //    var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value)).ToListAsync();
                    //    foreach (var tdpdmtim in Listtdpdmtim)
                    //    {
                    //        var WarehouseInputDetailBarcode = ListWarehouseInputDetailBarcode.Where(o => o.Barcode == BaseParameter.Barcode).FirstOrDefault();
                    //        if (WarehouseInputDetailBarcode == null)
                    //        {
                    //            WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                    //            WarehouseInputDetailBarcode.Quantity = 0;
                    //        }
                    //        WarehouseInputDetailBarcode.Description = tdpdmtim.VLID_GRP;
                    //        WarehouseInputDetailBarcode.Barcode = tdpdmtim.VLID_BARCODE;
                    //        WarehouseInputDetailBarcode.DateScan = tdpdmtim.VLID_DTM;
                    //        result.List.Add(WarehouseInputDetailBarcode);
                    //    }
                    //}
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeFromtrackmtimAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.BaseModel = new WarehouseInputDetailBarcode();
            if (BaseParameter.CompanyID > 0 && !string.IsNullOrEmpty(BaseParameter.Barcode))
            {
                BaseParameter.Barcode = BaseParameter.Barcode.Trim();

                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                string sql = @"select * from trackmtim where BARCODE_NM='" + BaseParameter.Barcode + "'";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                var trackmtim = new trackmtim();

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    var Listtrackmtim = new List<trackmtim>();
                    Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                    if (Listtrackmtim.Count > 0)
                    {
                        trackmtim = Listtrackmtim[0];
                    }
                }

                //var trackmtim = await _trackmtimRepository.GetByCondition(o => !string.IsNullOrEmpty(o.BARCODE_NM) && o.BARCODE_NM == BaseParameter.Barcode).FirstOrDefaultAsync();
                result.BaseModel = await GetByCondition(o => o.ParentID == 124 && o.Barcode == BaseParameter.Barcode).FirstOrDefaultAsync();
                if (result.BaseModel == null)
                {
                    result.BaseModel = new WarehouseInputDetailBarcode();
                }
                result.BaseModel.Barcode = BaseParameter.Barcode;
                if (trackmtim != null && trackmtim.TRACK_IDX > 0)
                {
                    result.BaseModel.MESID = trackmtim.TRACK_IDX;
                    result.BaseModel.Quantity = (decimal)(trackmtim.QTY ?? 0);
                    result.BaseModel.DateScan = trackmtim.RACKDTM;
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Active == true && o.IsSync == true).OrderBy(o => o.SortOrder).FirstOrDefaultAsync();
            if (CategoryDepartment != null && CategoryDepartment.ID > 0)
            {
                Expression<Func<WarehouseInput, bool>> ConditionWarehouseInput = o => o.CustomerID == CategoryDepartment.ID;
                var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(ConditionWarehouseInput).ToListAsync();
                if ((ListWarehouseInput != null) && (ListWarehouseInput.Count > 0))
                {
                    var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                    Expression<Func<WarehouseInputDetailBarcode, bool>> Condition = o => o.Active == true && o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value);
                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                        Condition = o => (o.Active == true && o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value))
                        && ((o.MaterialName != null && o.MaterialName.Contains(BaseParameter.SearchString))
                        || (o.Barcode != null && o.Barcode.Contains(BaseParameter.SearchString)));
                    }
                    else
                    {
                        if (BaseParameter.Year > 0)
                        {
                            if (BaseParameter.Month > 0)
                            {
                                if (BaseParameter.Day > 0)
                                {
                                    Condition = o => o.Active == true && o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.DateScan.Value.Day == BaseParameter.Day;
                                }
                                else
                                {
                                    Condition = o => o.Active == true && o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month;
                                }
                            }
                            else
                            {
                                Condition = o => o.Active == true && o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year;
                            }
                        }
                    }
                    result.List = await GetByCondition(Condition).ToListAsync();

                }
            }
            if (result.List.Count > 0)
            {
                result.List = result.List.OrderByDescending(o => o.DateScan).ThenBy(o => o.ParentID).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByMaterialID_CategoryLocationIDFromDiagramToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                if (BaseParameter.GeneralID > 0)
                {
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.IsComplete == true && o.CustomerID == GlobalHelper.DepartmentID).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                        if (BaseParameter.ParentID == GlobalHelper.CategoryLocationID)
                        {
                            result.List = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Active == true && o.QuantityInventory > 0 && o.MaterialID == BaseParameter.GeneralID).ToListAsync();
                        }
                        else
                        {
                            result.List = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID != null && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Active == true && o.QuantityInventory > 0 && o.MaterialID == BaseParameter.GeneralID && o.CategoryLocationID == BaseParameter.ParentID).ToListAsync();
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_MaterialID_CategoryLocationIDFromDiagramToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.CategoryDepartmentID > 0 && BaseParameter.ParentID > 0 && BaseParameter.GeneralID > 0)
            {
                var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.IsComplete == true && o.CustomerID == BaseParameter.CategoryDepartmentID).ToListAsync();
                if (ListWarehouseInput.Count > 0)
                {
                    var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                    result.List = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && o.Active == true && o.QuantityInventory > 0 && o.MaterialID == BaseParameter.GeneralID && o.CategoryLocationID == BaseParameter.ParentID).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_MaterialID_CategoryLocationNameFromDiagramToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                result.List = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && o.QuantityInventory > 0 && o.MaterialName == BaseParameter.Name && o.CategoryLocationName == BaseParameter.Code).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByParentIDAndEmpty_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
            {
                result.List = await GetByCondition(o => o.Barcode != null && o.Barcode == BaseParameter.SearchString).ToListAsync();
                if (result.List.Count == 0)
                {
                    result.List = await GetByCondition(o => o.MaterialName != null && o.MaterialName.Contains(BaseParameter.SearchString)).ToListAsync();
                }
            }
            else
            {
                if (BaseParameter.ParentID > 0)
                {
                    result.List = await GetByCondition(o => o.ParentID != null && o.ParentID == BaseParameter.ParentID).Take(100).ToListAsync();
                    var empty = new WarehouseInputDetailBarcode();
                    result.List.Add(empty);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    result = await GetByIDAsync(BaseParameter);
                    if (result.BaseModel != null)
                    {
                        if (result.BaseModel.ID > 0)
                        {
                            StringBuilder HTMLContent = new StringBuilder();
                            var Display = result.BaseModel.Display ?? GlobalHelper.InitializationString;
                            var MaterialName = result.BaseModel.Name ?? result.BaseModel.MaterialName;
                            var ProductionCode = result.BaseModel.ProductionCode ?? GlobalHelper.InitializationString;
                            var ProductID = result.BaseModel.ProductID ?? GlobalHelper.InitializationNumber;
                            var QuantityInvoice = result.BaseModel.QuantityInvoice ?? GlobalHelper.InitializationNumber;
                            var Quantity = result.BaseModel.QuantityInventory ?? GlobalHelper.InitializationNumber;
                            var ProductName = result.BaseModel.ProductName ?? GlobalHelper.InitializationString;
                            var CategoryLocationName = result.BaseModel.CategoryLocationName ?? GlobalHelper.InitializationString;
                            var Note = result.BaseModel.Note ?? GlobalHelper.InitializationString;
                            var Barcode = result.BaseModel.Barcode ?? GlobalHelper.InitializationString;
                            var Invoice = result.BaseModel.InvoiceInputName ?? result.BaseModel.ParentName;
                            var Date = result.BaseModel.Date ?? GlobalHelper.InitializationDateTime;
                            var Week = result.BaseModel.Week ?? GlobalHelper.InitializationNumber;
                            HTMLContent.AppendLine(GlobalHelper.CreateHTMLWarehouseInputBarcode(SheetName, _WebHostEnvironment.WebRootPath, Display, MaterialName, ProductionCode, ProductID.ToString(), QuantityInvoice.ToString("N0"), Quantity.ToString("N0"), ProductName, CategoryLocationName, Note, Barcode, Date.ToString("yyyy-MM-dd"), Week.ToString(), Invoice));

                            string HTMLEmpty = GlobalHelper.InitializationString;
                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    HTMLEmpty = r.ReadToEnd();
                                }
                            }
                            HTMLEmpty = HTMLEmpty.Replace(@"[Content]", HTMLContent.ToString());

                            string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    await w.WriteLineAsync(HTMLEmpty);
                                }
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByBarcodeAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    result.BaseModel = await GetByCondition(o => o.Barcode == BaseParameter.SearchString).OrderBy(o => o.CreateDate).FirstOrDefaultAsync();
                    if (result.BaseModel != null)
                    {
                        if (result.BaseModel.ID > 0)
                        {
                            StringBuilder HTMLContent = new StringBuilder();
                            var Display = result.BaseModel.Display ?? GlobalHelper.InitializationString;
                            var MaterialName = result.BaseModel.Name ?? result.BaseModel.MaterialName;
                            var ProductionCode = result.BaseModel.ProductionCode ?? GlobalHelper.InitializationString;
                            var ProductID = result.BaseModel.ProductID ?? GlobalHelper.InitializationNumber;
                            var QuantityInvoice = result.BaseModel.QuantityInvoice ?? GlobalHelper.InitializationNumber;
                            var Quantity = result.BaseModel.QuantityInventory ?? GlobalHelper.InitializationNumber;
                            var ProductName = result.BaseModel.ProductName ?? GlobalHelper.InitializationString;
                            var CategoryLocationName = result.BaseModel.CategoryLocationName ?? GlobalHelper.InitializationString;
                            var Note = result.BaseModel.Note ?? GlobalHelper.InitializationString;
                            var Barcode = result.BaseModel.Barcode ?? GlobalHelper.InitializationString;
                            var Invoice = result.BaseModel.InvoiceInputName ?? result.BaseModel.ParentName;
                            var Date = result.BaseModel.Date ?? GlobalHelper.InitializationDateTime;
                            var Week = result.BaseModel.Week ?? GlobalHelper.InitializationNumber;
                            HTMLContent.AppendLine(GlobalHelper.CreateHTMLWarehouseInputBarcode(SheetName, _WebHostEnvironment.WebRootPath, Display, MaterialName, ProductionCode, ProductID.ToString(), QuantityInvoice.ToString("N0"), Quantity.ToString("N0"), ProductName, CategoryLocationName, Note, Barcode, Date.ToString("yyyy-MM-dd"), Week.ToString(), Invoice));

                            string HTMLEmpty = GlobalHelper.InitializationString;
                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    HTMLEmpty = r.ReadToEnd();
                                }
                            }
                            HTMLEmpty = HTMLEmpty.Replace(@"[Content]", HTMLContent.ToString());

                            string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    await w.WriteLineAsync(HTMLEmpty);
                                }
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByParentIDAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ParentID > 0)
                {
                    //var ListWarehouseInputDetail = await _WarehouseInputDetailRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                    //if (ListWarehouseInputDetail.Count > 0)
                    //{
                    //    foreach (var WarehouseInputDetail in ListWarehouseInputDetail)
                    //    {
                    //        result.List.AddRange(await GetByCondition(o => o.WarehouseInputDetailID == WarehouseInputDetail.ID && o.QuantitySNP == WarehouseInputDetail.QuantitySNP).ToListAsync());
                    //    }
                    //}
                    //else
                    //{
                    //    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                    //}
                    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                    if (result.List != null)
                    {
                        if (result.List.Count > 0)
                        {
                            StringBuilder HTMLContent = new StringBuilder();
                            var ListSub = result.List.Where(o => o.ProductID == 0).OrderBy(o => o.Barcode).ToList();
                            foreach (var item in ListSub)
                            {
                                result.BaseModel = item;
                                var Display = result.BaseModel.Display ?? GlobalHelper.InitializationString;
                                var MaterialName = result.BaseModel.Name ?? result.BaseModel.MaterialName;
                                var ProductionCode = result.BaseModel.ProductionCode ?? GlobalHelper.InitializationString;
                                var ProductID = result.BaseModel.ProductID ?? GlobalHelper.InitializationNumber;
                                var QuantityInvoice = result.BaseModel.QuantityInvoice ?? GlobalHelper.InitializationNumber;
                                var Quantity = result.BaseModel.QuantityInventory ?? GlobalHelper.InitializationNumber;
                                var ProductName = result.BaseModel.ProductName ?? GlobalHelper.InitializationString;
                                var CategoryLocationName = result.BaseModel.CategoryLocationName ?? GlobalHelper.InitializationString;
                                var Note = result.BaseModel.Note ?? GlobalHelper.InitializationString;
                                var Barcode = result.BaseModel.Barcode ?? GlobalHelper.InitializationString;
                                var Invoice = result.BaseModel.InvoiceInputName ?? result.BaseModel.ParentName;
                                var Date = result.BaseModel.Date ?? GlobalHelper.InitializationDateTime;
                                var Week = result.BaseModel.Week ?? GlobalHelper.InitializationNumber;
                                HTMLContent.AppendLine(GlobalHelper.CreateHTMLWarehouseInputBarcode(SheetName, _WebHostEnvironment.WebRootPath, Display, MaterialName, ProductionCode, ProductID.ToString(), QuantityInvoice.ToString("N0"), Quantity.ToString("N0"), ProductName, CategoryLocationName, Note, Barcode, Date.ToString("yyyy-MM-dd"), Week.ToString(), Invoice));
                            }
                            ListSub = result.List.Where(o => o.ProductID > 0).OrderBy(o => o.Barcode).ToList();
                            foreach (var item in ListSub)
                            {
                                result.BaseModel = item;
                                var Display = result.BaseModel.Display ?? GlobalHelper.InitializationString;
                                var MaterialName = result.BaseModel.MaterialName ?? GlobalHelper.InitializationString;
                                var ProductionCode = result.BaseModel.ProductionCode ?? GlobalHelper.InitializationString;
                                var ProductID = result.BaseModel.ProductID ?? GlobalHelper.InitializationNumber;
                                var QuantityInvoice = result.BaseModel.QuantityInvoice ?? GlobalHelper.InitializationNumber;
                                var Quantity = result.BaseModel.QuantityInventory ?? GlobalHelper.InitializationNumber;
                                var ProductName = result.BaseModel.ProductName ?? GlobalHelper.InitializationString;
                                var CategoryLocationName = result.BaseModel.CategoryLocationName ?? GlobalHelper.InitializationString;
                                var Note = result.BaseModel.Note ?? GlobalHelper.InitializationString;
                                var Barcode = result.BaseModel.Barcode ?? GlobalHelper.InitializationString;
                                var Invoice = result.BaseModel.InvoiceInputName ?? result.BaseModel.ParentName;
                                var Date = result.BaseModel.Date ?? GlobalHelper.InitializationDateTime;
                                var Week = result.BaseModel.Week ?? GlobalHelper.InitializationNumber;
                                HTMLContent.AppendLine(GlobalHelper.CreateHTMLWarehouseInputBarcode(SheetName, _WebHostEnvironment.WebRootPath, Display, MaterialName, ProductionCode, ProductID.ToString(), QuantityInvoice.ToString("N0"), Quantity.ToString("N0"), ProductName, CategoryLocationName, Note, Barcode, Date.ToString("yyyy-MM-dd"), Week.ToString(), Invoice));
                            }

                            string HTMLEmpty = GlobalHelper.InitializationString;
                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    HTMLEmpty = r.ReadToEnd();
                                }
                            }
                            HTMLEmpty = HTMLEmpty.Replace(@"[Content]", HTMLContent.ToString());

                            string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    await w.WriteLineAsync(HTMLEmpty);
                                }
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByListWarehouseInputDetailID2025Async(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ListID != null && BaseParameter.ListID.Count > 0)
                {
                    //var ListWarehouseInputDetail = await _WarehouseInputDetailRepository.GetByCondition(o => BaseParameter.ListID.Contains(o.ID)).ToListAsync();
                    //if (ListWarehouseInputDetail.Count > 0)
                    //{
                    //    foreach (var WarehouseInputDetail in ListWarehouseInputDetail)
                    //    {
                    //        result.List.AddRange(await GetByCondition(o => o.WarehouseInputDetailID == WarehouseInputDetail.ID && o.QuantitySNP == WarehouseInputDetail.QuantitySNP).ToListAsync());
                    //    }
                    //}
                    //else
                    //{
                    //    result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                    //}
                    result.List.AddRange(await GetByCondition(o => BaseParameter.ListID.Contains(o.WarehouseInputDetailID ?? 0)).ToListAsync());
                    if (result.List != null)
                    {
                        if (result.List.Count > 0)
                        {
                            StringBuilder HTMLContent = new StringBuilder();
                            var ListSub = result.List.Where(o => o.ProductID == 0).OrderBy(o => o.Barcode).ToList();
                            foreach (var item in ListSub)
                            {
                                result.BaseModel = item;
                                var Display = result.BaseModel.Display ?? GlobalHelper.InitializationString;
                                var MaterialName = result.BaseModel.Name ?? result.BaseModel.MaterialName;
                                var ProductionCode = result.BaseModel.ProductionCode ?? GlobalHelper.InitializationString;
                                var ProductID = result.BaseModel.ProductID ?? GlobalHelper.InitializationNumber;
                                var QuantityInvoice = result.BaseModel.QuantityInvoice ?? GlobalHelper.InitializationNumber;
                                var Quantity = result.BaseModel.QuantityInventory ?? GlobalHelper.InitializationNumber;
                                var ProductName = result.BaseModel.ProductName ?? GlobalHelper.InitializationString;
                                var CategoryLocationName = result.BaseModel.CategoryLocationName ?? GlobalHelper.InitializationString;
                                var Note = result.BaseModel.Note ?? GlobalHelper.InitializationString;
                                var Barcode = result.BaseModel.Barcode ?? GlobalHelper.InitializationString;
                                var Invoice = result.BaseModel.InvoiceInputName ?? result.BaseModel.ParentName;
                                var Date = result.BaseModel.Date ?? GlobalHelper.InitializationDateTime;
                                var Week = result.BaseModel.Week ?? GlobalHelper.InitializationNumber;
                                HTMLContent.AppendLine(GlobalHelper.CreateHTMLWarehouseInputBarcode(SheetName, _WebHostEnvironment.WebRootPath, Display, MaterialName, ProductionCode, ProductID.ToString(), QuantityInvoice.ToString("N0"), Quantity.ToString("N0"), ProductName, CategoryLocationName, Note, Barcode, Date.ToString("yyyy-MM-dd"), Week.ToString(), Invoice));
                            }
                            ListSub = result.List.Where(o => o.ProductID > 0).OrderBy(o => o.Barcode).ToList();
                            foreach (var item in ListSub)
                            {
                                result.BaseModel = item;
                                var Display = result.BaseModel.Display ?? GlobalHelper.InitializationString;
                                var MaterialName = result.BaseModel.MaterialName ?? GlobalHelper.InitializationString;
                                var ProductionCode = result.BaseModel.ProductionCode ?? GlobalHelper.InitializationString;
                                var ProductID = result.BaseModel.ProductID ?? GlobalHelper.InitializationNumber;
                                var QuantityInvoice = result.BaseModel.QuantityInvoice ?? GlobalHelper.InitializationNumber;
                                var Quantity = result.BaseModel.QuantityInventory ?? GlobalHelper.InitializationNumber;
                                var ProductName = result.BaseModel.ProductName ?? GlobalHelper.InitializationString;
                                var CategoryLocationName = result.BaseModel.CategoryLocationName ?? GlobalHelper.InitializationString;
                                var Note = result.BaseModel.Note ?? GlobalHelper.InitializationString;
                                var Barcode = result.BaseModel.Barcode ?? GlobalHelper.InitializationString;
                                var Invoice = result.BaseModel.InvoiceInputName ?? result.BaseModel.ParentName;
                                var Date = result.BaseModel.Date ?? GlobalHelper.InitializationDateTime;
                                var Week = result.BaseModel.Week ?? GlobalHelper.InitializationNumber;
                                HTMLContent.AppendLine(GlobalHelper.CreateHTMLWarehouseInputBarcode(SheetName, _WebHostEnvironment.WebRootPath, Display, MaterialName, ProductionCode, ProductID.ToString(), QuantityInvoice.ToString("N0"), Quantity.ToString("N0"), ProductName, CategoryLocationName, Note, Barcode, Date.ToString("yyyy-MM-dd"), Week.ToString(), Invoice));
                            }

                            string HTMLEmpty = GlobalHelper.InitializationString;
                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    HTMLEmpty = r.ReadToEnd();
                                }
                            }
                            HTMLEmpty = HTMLEmpty.Replace(@"[Content]", HTMLContent.ToString());

                            string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    await w.WriteLineAsync(HTMLEmpty);
                                }
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByListWarehouseInputDetailIDAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ListID != null && BaseParameter.ListID.Count > 0)
                {
                    result.List = await GetByCondition(o => o.WarehouseInputDetailID > 0 && BaseParameter.ListID.Contains(o.WarehouseInputDetailID.Value)).OrderBy(o => o.WarehouseInputDetailID).ThenByDescending(o => o.UpdateDate).ToListAsync();
                    if (result.List != null && result.List.Count > 0)
                    {

                        StringBuilder HTMLContent = new StringBuilder();
                        foreach (var item in result.List)
                        {
                            result.BaseModel = item;
                            var Display = result.BaseModel.Display ?? GlobalHelper.InitializationString;
                            var MaterialName = result.BaseModel.Name ?? result.BaseModel.MaterialName;
                            var ProductionCode = result.BaseModel.ProductionCode ?? GlobalHelper.InitializationString;
                            var ProductID = result.BaseModel.ProductID ?? GlobalHelper.InitializationNumber;
                            var QuantityInvoice = result.BaseModel.QuantityInvoice ?? GlobalHelper.InitializationNumber;
                            var Quantity = result.BaseModel.QuantityInventory ?? GlobalHelper.InitializationNumber;
                            var ProductName = result.BaseModel.ProductName ?? GlobalHelper.InitializationString;
                            var CategoryLocationName = result.BaseModel.CategoryLocationName ?? GlobalHelper.InitializationString;
                            var Note = result.BaseModel.Note ?? GlobalHelper.InitializationString;
                            var Barcode = result.BaseModel.Barcode ?? GlobalHelper.InitializationString;
                            var Invoice = result.BaseModel.InvoiceInputName ?? result.BaseModel.ParentName;
                            var Date = result.BaseModel.Date ?? GlobalHelper.InitializationDateTime;
                            var Week = result.BaseModel.Week ?? GlobalHelper.InitializationNumber;
                            HTMLContent.AppendLine(GlobalHelper.CreateHTMLWarehouseInputBarcode(SheetName, _WebHostEnvironment.WebRootPath, Display, MaterialName, ProductionCode, ProductID.ToString(), QuantityInvoice.ToString("N0"), Quantity.ToString("N0"), ProductName, CategoryLocationName, Note, Barcode, Date.ToString("yyyy-MM-dd"), Week.ToString(), Invoice));
                        }

                        string HTMLEmpty = GlobalHelper.InitializationString;
                        string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                        using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                        {
                            using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                            {
                                HTMLEmpty = r.ReadToEnd();
                            }
                        }
                        HTMLEmpty = HTMLEmpty.Replace(@"[Content]", HTMLContent.ToString());

                        string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                        string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                        Directory.CreateDirectory(physicalPathCreate);
                        GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                        string filePath = Path.Combine(physicalPathCreate, fileName);
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                await w.WriteLineAsync(HTMLEmpty);
                            }
                        }
                        result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByWarehouseInputDetailIDAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ID > 0)
                {
                    var WarehouseInputDetail = await _WarehouseInputDetailRepository.GetByIDAsync(BaseParameter.ID);
                    if (WarehouseInputDetail.ID > 0)
                    {
                        result.List = await GetByCondition(o => o.WarehouseInputDetailID == WarehouseInputDetail.ID && o.QuantitySNP == WarehouseInputDetail.QuantitySNP).OrderBy(o => o.ProductID).ThenBy(o => o.Barcode).ToListAsync();
                        if (result.List != null && result.List.Count > 0)
                        {
                            StringBuilder HTMLContent = new StringBuilder();
                            foreach (var item in result.List)
                            {
                                result.BaseModel = item;
                                var Display = result.BaseModel.Display ?? GlobalHelper.InitializationString;
                                var MaterialName = result.BaseModel.Name ?? result.BaseModel.MaterialName;
                                var ProductionCode = result.BaseModel.ProductionCode ?? GlobalHelper.InitializationString;
                                var ProductID = result.BaseModel.ProductID ?? GlobalHelper.InitializationNumber;
                                var QuantityInvoice = result.BaseModel.QuantityInvoice ?? GlobalHelper.InitializationNumber;
                                var Quantity = result.BaseModel.QuantityInventory ?? GlobalHelper.InitializationNumber;
                                var ProductName = result.BaseModel.ProductName ?? GlobalHelper.InitializationString;
                                var CategoryLocationName = result.BaseModel.CategoryLocationName ?? GlobalHelper.InitializationString;
                                var Note = result.BaseModel.Note ?? GlobalHelper.InitializationString;
                                var Barcode = result.BaseModel.Barcode ?? GlobalHelper.InitializationString;
                                var Invoice = result.BaseModel.InvoiceInputName ?? result.BaseModel.ParentName;
                                var Date = result.BaseModel.Date ?? GlobalHelper.InitializationDateTime;
                                var Week = result.BaseModel.Week ?? GlobalHelper.InitializationNumber;
                                HTMLContent.AppendLine(GlobalHelper.CreateHTMLWarehouseInputBarcode(SheetName, _WebHostEnvironment.WebRootPath, Display, MaterialName, ProductionCode, ProductID.ToString(), QuantityInvoice.ToString("N0"), Quantity.ToString("N0"), ProductName, CategoryLocationName, Note, Barcode, Date.ToString("yyyy-MM-dd"), Week.ToString(), Invoice));
                            }

                            string HTMLEmpty = GlobalHelper.InitializationString;
                            string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                            using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                            {
                                using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                {
                                    HTMLEmpty = r.ReadToEnd();
                                }
                            }
                            HTMLEmpty = HTMLEmpty.Replace(@"[Content]", HTMLContent.ToString());

                            string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (FileStream fs = new FileStream(filePath, FileMode.Create))
                            {
                                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                {
                                    await w.WriteLineAsync(HTMLEmpty);
                                }
                            }
                            result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintBarcode_WarehouseOutputIDAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.ParentID > 0 && !string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    var WarehouseOutput = await _WarehouseOutputRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                    if (WarehouseOutput != null && WarehouseOutput.ID > 0)
                    {
                        var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == WarehouseOutput.SupplierID).ToListAsync();
                        if (ListWarehouseInput.Count > 0)
                        {
                            var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                            BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                            var WarehouseInputDetailBarcode = await GetByCondition(o => o.Active == true && o.Barcode == BaseParameter.SearchString && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value)).FirstOrDefaultAsync();
                            if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                            {
                                result.BaseModel = WarehouseInputDetailBarcode;
                                StringBuilder HTMLContent = new StringBuilder();
                                var Display = result.BaseModel.Display ?? GlobalHelper.InitializationString;
                                var MaterialName = result.BaseModel.Name ?? result.BaseModel.MaterialName;
                                var ProductionCode = result.BaseModel.ProductionCode ?? GlobalHelper.InitializationString;
                                var ProductID = result.BaseModel.ProductID ?? GlobalHelper.InitializationNumber;
                                var QuantityInvoice = result.BaseModel.QuantityInvoice ?? GlobalHelper.InitializationNumber;
                                var Quantity = result.BaseModel.QuantityInventory ?? GlobalHelper.InitializationNumber;
                                var ProductName = result.BaseModel.ProductName ?? GlobalHelper.InitializationString;
                                var CategoryLocationName = result.BaseModel.CategoryLocationName ?? GlobalHelper.InitializationString;
                                var Note = result.BaseModel.Note ?? GlobalHelper.InitializationString;
                                var Barcode = result.BaseModel.Barcode ?? GlobalHelper.InitializationString;
                                var Invoice = result.BaseModel.InvoiceInputName ?? result.BaseModel.ParentName;
                                var Date = result.BaseModel.Date ?? GlobalHelper.InitializationDateTime;
                                var Week = result.BaseModel.Week ?? GlobalHelper.InitializationNumber;
                                HTMLContent.AppendLine(GlobalHelper.CreateHTMLWarehouseInputBarcode(SheetName, _WebHostEnvironment.WebRootPath, Display, MaterialName, ProductionCode, ProductID.ToString(), QuantityInvoice.ToString("N0"), Quantity.ToString("N0"), ProductName, CategoryLocationName, Note, Barcode, Date.ToString("yyyy-MM-dd"), Week.ToString(), Invoice));

                                string HTMLEmpty = GlobalHelper.InitializationString;
                                string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                {
                                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                    {
                                        HTMLEmpty = r.ReadToEnd();
                                    }
                                }
                                HTMLEmpty = HTMLEmpty.Replace(@"[Content]", HTMLContent.ToString());

                                string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                                string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                                Directory.CreateDirectory(physicalPathCreate);
                                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                                string filePath = Path.Combine(physicalPathCreate, fileName);
                                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                                {
                                    using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                    {
                                        await w.WriteLineAsync(HTMLEmpty);
                                    }
                                }
                                result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> PrintByListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            string SheetName = this.GetType().Name;
            if (BaseParameter != null)
            {
                if (BaseParameter.List != null && BaseParameter.List.Count > 0)
                {
                    var ListID = BaseParameter.List.Select(o => o.ID).ToList();
                    result.List = await GetByCondition(o => ListID.Contains(o.ID) && o.QuantityInventory > 0).OrderBy(o => o.ID).ToListAsync();
                    if (result.List != null && result.List.Count > 0)
                    {
                        StringBuilder HTMLContent = new StringBuilder();
                        foreach (var item in result.List)
                        {
                            result.BaseModel = item;
                            var Display = result.BaseModel.Display ?? GlobalHelper.InitializationString;
                            var MaterialName = result.BaseModel.Name ?? result.BaseModel.MaterialName;
                            var ProductionCode = result.BaseModel.ProductionCode ?? GlobalHelper.InitializationString;
                            var ProductID = result.BaseModel.ProductID ?? GlobalHelper.InitializationNumber;
                            var QuantityInvoice = result.BaseModel.QuantityInvoice ?? GlobalHelper.InitializationNumber;
                            var Quantity = result.BaseModel.QuantityInventory ?? GlobalHelper.InitializationNumber;
                            var ProductName = result.BaseModel.ProductName ?? GlobalHelper.InitializationString;
                            var CategoryLocationName = result.BaseModel.CategoryLocationName ?? GlobalHelper.InitializationString;
                            var Note = result.BaseModel.Note ?? GlobalHelper.InitializationString;
                            var Barcode = result.BaseModel.Barcode ?? GlobalHelper.InitializationString;
                            var Invoice = result.BaseModel.InvoiceInputName ?? result.BaseModel.ParentName;
                            var Date = result.BaseModel.Date ?? GlobalHelper.InitializationDateTime;
                            var Week = result.BaseModel.Week ?? GlobalHelper.InitializationNumber;
                            HTMLContent.AppendLine(GlobalHelper.CreateHTMLWarehouseInputBarcode(SheetName, _WebHostEnvironment.WebRootPath, Display, MaterialName, ProductionCode, ProductID.ToString(), QuantityInvoice.ToString("N0"), Quantity.ToString("N0"), ProductName, CategoryLocationName, Note, Barcode, Date.ToString("yyyy-MM-dd"), Week.ToString(), Invoice));
                        }

                        string HTMLEmpty = GlobalHelper.InitializationString;
                        string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                        using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                        {
                            using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                            {
                                HTMLEmpty = r.ReadToEnd();
                            }
                        }
                        HTMLEmpty = HTMLEmpty.Replace(@"[Content]", HTMLContent.ToString());

                        string fileName = SheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                        string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                        Directory.CreateDirectory(physicalPathCreate);
                        GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                        string filePath = Path.Combine(physicalPathCreate, fileName);
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                await w.WriteLineAsync(HTMLEmpty);
                            }
                        }
                        result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> CreateAutoAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.DateScan != null).OrderBy(o => o.DateScan).ToListAsync();
            foreach (var model in ListWarehouseInputDetailBarcode)
            {
                if (model.DateScan != null)
                {
                    model.Year = model.DateScan.Value.Year;
                    model.Month = model.DateScan.Value.Month;
                    model.Day = model.DateScan.Value.Day;
                    model.Week = GlobalHelper.GetWeekByDateTime(model.DateScan.Value);
                    await _WarehouseInputDetailBarcodeRepository.UpdateAsync(model);
                }
            }
            return result;
        }
        public override BaseResult<WarehouseInputDetailBarcode> GetByParentIDToList(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID).OrderBy(o => o.MaterialName).ThenBy(o => o.Barcode).ToList();
            }
            return result;
        }
        public override async Task<BaseResult<WarehouseInputDetailBarcode>> GetByParentIDToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID).OrderBy(o => o.MaterialName).ThenBy(o => o.Barcode).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetCompareMESAndERPToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (BaseParameter.CategoryDepartmentID > 0 && BaseParameter.CompanyID > 0)
            {
                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);

                //var Listtsuser = await _tsuserRepository.GetAllToListAsync();
                //var Listtmbrcd_his = await _tmbrcd_hisRepository.GetAllToListAsync();

                var Listtsuser = new List<tsuser>();
                var Listtmbrcd_his = new List<tmbrcd_his>();

                string sql = @"select * from tsuser";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    Listtsuser.AddRange(SQLHelper.ToList<tsuser>(dt));
                }

                sql = @"select * from tmbrcd_his";
                ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    Listtmbrcd_his.AddRange(SQLHelper.ToList<tmbrcd_his>(dt));
                }

                BaseParameter.ParentID = BaseParameter.CompanyID;

                var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).OrderBy(o => o.Date).ToListAsync();
                var ListWarehouseInputID = ListWarehouseInput.Select(x => x.ID).ToList();
                var ListWarehouseInputDetailBarcode = new List<WarehouseInputDetailBarcode>();
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && !string.IsNullOrEmpty(o.Barcode) && o.Barcode.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                else
                {
                    ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value)).ToListAsync();
                }
                switch (BaseParameter.CategoryDepartmentID)
                {
                    case 23:
                    case 188:
                        //var Listtmbrcd = await _tmbrcdRepository.GetByCondition(o => o.BBCO == "Y" && o.DSCN_YN != "Y" && o.PKG_QTY - o.PKG_OUTQTY > 0).ToListAsync();

                        var Listtmbrcd = new List<tmbrcd>();
                        sql = @"select * from tmbrcd where BBCO='Y' and DSCN_YN!='Y' and (PKG_QTY-PKG_OUTQTY>0)";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            Listtmbrcd.AddRange(SQLHelper.ToList<tmbrcd>(dt));
                        }

                        foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                        {
                            var tmbrcd = Listtmbrcd.Where(o => o.BARCD_ID == WarehouseInputDetailBarcode.Barcode).FirstOrDefault();
                            if (tmbrcd != null && tmbrcd.BARCD_IDX > 0)
                            {
                                WarehouseInputDetailBarcode.MESID = tmbrcd.BARCD_IDX;
                                WarehouseInputDetailBarcode.PKG_QTY = tmbrcd.PKG_QTY;
                                WarehouseInputDetailBarcode.PKG_OUTQTY = tmbrcd.PKG_OUTQTY;
                                WarehouseInputDetailBarcode.BARCD_LOC = tmbrcd.BARCD_LOC;
                                WarehouseInputDetailBarcode.PKG_GRP = tmbrcd.PKG_GRP;
                                WarehouseInputDetailBarcode.OUT_DTM = tmbrcd.OUT_DTM;
                                WarehouseInputDetailBarcode.CREATE_DTM = tmbrcd.CREATE_DTM;
                                WarehouseInputDetailBarcode.QuantityMES = 0;
                                WarehouseInputDetailBarcode.QuantityOutputMES = 0;
                                WarehouseInputDetailBarcode.PKG_QTYActual = 0;
                                try
                                {
                                    WarehouseInputDetailBarcode.QuantityMES = (decimal)(tmbrcd.PKG_QTY - tmbrcd.PKG_OUTQTY);
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                                try
                                {
                                    var Listtmbrcd_hisSub = Listtmbrcd_his.Where(o => o.BARCD_ID == WarehouseInputDetailBarcode.Barcode && o.OUT_DTM != null && WarehouseInputDetailBarcode.UpdateDate != null && o.OUT_DTM >= WarehouseInputDetailBarcode.UpdateDate).ToList();
                                    WarehouseInputDetailBarcode.QuantityOutputMES = Listtmbrcd_hisSub.Sum(o => o.PKG_OUTQTY);
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                                try
                                {

                                    var tsuser = Listtsuser.Where(o => o.USER_IDX == int.Parse(tmbrcd.CREATE_USER)).FirstOrDefault();
                                    if (tsuser != null && tsuser.USER_IDX > 0)
                                    {
                                        try
                                        {
                                            var USER_ID = int.Parse(tsuser.USER_ID);
                                            BaseParameter.ParentID = 16;
                                        }
                                        catch (Exception ex)
                                        {
                                            string mes = ex.Message;
                                            BaseParameter.ParentID = 17;
                                        }
                                    }
                                    else
                                    {
                                        BaseParameter.ParentID = 16;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                            }
                            try
                            {
                                WarehouseInputDetailBarcode.QuantityOutputMES = WarehouseInputDetailBarcode.QuantityOutputMES ?? 0;
                                WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityOutputMES;
                                WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.QuantityInventory - WarehouseInputDetailBarcode.QuantityMES);

                                WarehouseInputDetailBarcode.QuantityGAP01 = (decimal)WarehouseInputDetailBarcode.PKG_QTYActual;
                                WarehouseInputDetailBarcode.QuantityGAP02 = WarehouseInputDetailBarcode.Quantity02 - WarehouseInputDetailBarcode.QuantityMES;
                                WarehouseInputDetailBarcode.QuantityGAP03 = WarehouseInputDetailBarcode.Quantity03 - WarehouseInputDetailBarcode.QuantityMES;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            if (BaseParameter.CompanyID == BaseParameter.ParentID)
                            {
                                result.List.Add(WarehouseInputDetailBarcode);
                            }
                        }
                        break;
                    case 108:
                    case 199:
                        //var Listtdpdmtim = await _tdpdmtimRepository.GetByCondition(o => o.VLID_DSCN_YN != "Y").ToListAsync();

                        var Listtdpdmtim = new List<tdpdmtim>();
                        sql = @"select * from tdpdmtim where VLID_DSCN_YN != 'Y'";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            Listtdpdmtim.AddRange(SQLHelper.ToList<tdpdmtim>(dt));
                        }
                        foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                        {
                            var tdpdmtim = Listtdpdmtim.Where(o => o.VLID_BARCODE == WarehouseInputDetailBarcode.Barcode).FirstOrDefault();
                            if (tdpdmtim != null && tdpdmtim.VLID_PART_IDX > 0)
                            {
                                WarehouseInputDetailBarcode.MESID = tdpdmtim.PDMTIN_IDX;
                                WarehouseInputDetailBarcode.Description = tdpdmtim.VLID_GRP;
                                WarehouseInputDetailBarcode.BARCD_LOC = tdpdmtim.BARCD_LOC;
                                WarehouseInputDetailBarcode.QuantityMES = 1;
                                WarehouseInputDetailBarcode.PKG_QTYActual = 0;
                                try
                                {
                                    WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityMES);
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                                try
                                {

                                    var tsuser = Listtsuser.Where(o => o.USER_IDX == int.Parse(tdpdmtim.CREATE_USER)).FirstOrDefault();
                                    if (tsuser != null && tsuser.USER_IDX > 0)
                                    {
                                        try
                                        {
                                            var USER_ID = int.Parse(tsuser.USER_ID);
                                            if (BaseParameter.CompanyID == 16)
                                            {
                                                result.List.Add(WarehouseInputDetailBarcode);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            string mes = ex.Message;
                                            if (BaseParameter.CompanyID == 17)
                                            {
                                                result.List.Add(WarehouseInputDetailBarcode);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (BaseParameter.CompanyID == 16)
                                        {
                                            result.List.Add(WarehouseInputDetailBarcode);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                            }
                            try
                            {
                                WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity;
                                WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityMES);

                                WarehouseInputDetailBarcode.QuantityGAP01 = (decimal)WarehouseInputDetailBarcode.PKG_QTYActual;
                                WarehouseInputDetailBarcode.QuantityGAP02 = WarehouseInputDetailBarcode.Quantity02 - WarehouseInputDetailBarcode.QuantityMES;
                                WarehouseInputDetailBarcode.QuantityGAP03 = WarehouseInputDetailBarcode.Quantity03 - WarehouseInputDetailBarcode.QuantityMES;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            if (BaseParameter.CompanyID == BaseParameter.ParentID)
                            {
                                result.List.Add(WarehouseInputDetailBarcode);
                            }
                        }
                        break;
                    case 86:
                    case 195:
                        //var Listtrackmtim = await _trackmtimRepository.GetByCondition(o => o.RACKOUT_DTM == null).ToListAsync();

                        var Listtrackmtim = new List<trackmtim>();
                        sql = @"select * from trackmtim where RACKOUT_DTM is null";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                        }
                        foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                        {
                            var trackmtim = Listtrackmtim.Where(o => o.BARCODE_NM == WarehouseInputDetailBarcode.Barcode).FirstOrDefault();
                            if (trackmtim != null && trackmtim.TRACK_IDX > 0)
                            {
                                WarehouseInputDetailBarcode.MESID = trackmtim.TRACK_IDX;
                                WarehouseInputDetailBarcode.QuantityMES = (decimal)trackmtim.QTY;
                                WarehouseInputDetailBarcode.PKG_QTYActual = 0;
                                try
                                {
                                    WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityMES);
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                                try
                                {

                                    var tsuser = Listtsuser.Where(o => o.USER_IDX == int.Parse(trackmtim.CREATE_USER)).FirstOrDefault();
                                    if (tsuser != null && tsuser.USER_IDX > 0)
                                    {
                                        try
                                        {
                                            var USER_ID = int.Parse(tsuser.USER_ID);
                                            if (BaseParameter.CompanyID == 16)
                                            {
                                                result.List.Add(WarehouseInputDetailBarcode);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            string mes = ex.Message;
                                            if (BaseParameter.CompanyID == 17)
                                            {
                                                result.List.Add(WarehouseInputDetailBarcode);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (BaseParameter.CompanyID == 16)
                                        {
                                            result.List.Add(WarehouseInputDetailBarcode);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                            }
                            try
                            {
                                WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity;
                                WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityMES);

                                WarehouseInputDetailBarcode.QuantityGAP01 = (decimal)WarehouseInputDetailBarcode.PKG_QTYActual;
                                WarehouseInputDetailBarcode.QuantityGAP02 = WarehouseInputDetailBarcode.Quantity02 - WarehouseInputDetailBarcode.QuantityMES;
                                WarehouseInputDetailBarcode.QuantityGAP03 = WarehouseInputDetailBarcode.Quantity03 - WarehouseInputDetailBarcode.QuantityMES;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            if (BaseParameter.CompanyID == BaseParameter.ParentID)
                            {
                                result.List.Add(WarehouseInputDetailBarcode);
                            }
                        }
                        break;

                }
            }
            switch (BaseParameter.Action)
            {
                case 1:
                    result.List = result.List.Where(o => o.CategoryLocationID > 0).ToList();
                    break;
                case 2:
                    result.List = result.List.Where(o => o.CategoryLocationID == null || o.CategoryLocationID == 0).ToList();
                    break;
            }
            result.List = result.List.OrderBy(o => o.MaterialName).ThenByDescending(o => o.UpdateDate).ToList();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetPARTNOCompareMESAndERPToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (BaseParameter.CategoryDepartmentID > 0 && BaseParameter.CompanyID > 0)
            {
                string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);

                //var Listtsuser = await _tsuserRepository.GetAllToListAsync();
                //var Listtmbrcd_his = await _tmbrcd_hisRepository.GetAllToListAsync();

                var Listtsuser = new List<tsuser>();
                var Listtmbrcd_his = new List<tmbrcd_his>();

                string sql = @"select * from tsuser";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    Listtsuser.AddRange(SQLHelper.ToList<tsuser>(dt));
                }

                sql = @"select * from tmbrcd_his";
                ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    Listtmbrcd_his.AddRange(SQLHelper.ToList<tmbrcd_his>(dt));
                }

                BaseParameter.ParentID = BaseParameter.CompanyID;

                var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CustomerID == BaseParameter.CategoryDepartmentID).OrderBy(o => o.Date).ToListAsync();
                var ListWarehouseInputID = ListWarehouseInput.Select(x => x.ID).ToList();
                var ListWarehouseInputDetailBarcode = new List<WarehouseInputDetailBarcode>();
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value) && !string.IsNullOrEmpty(o.Barcode) && o.Barcode.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                else
                {
                    ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID.Value)).ToListAsync();
                }
                switch (BaseParameter.CategoryDepartmentID)
                {
                    case 23:
                    case 188:
                        //var Listtmbrcd = await _tmbrcdRepository.GetByCondition(o => o.BBCO == "Y" && o.DSCN_YN != "Y" && o.PKG_QTY - o.PKG_OUTQTY > 0).ToListAsync();

                        var Listtmbrcd = new List<tmbrcd>();
                        sql = @"select * from tmbrcd where BBCO='Y' and DSCN_YN!='Y' and (PKG_QTY-PKG_OUTQTY>0)";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            Listtmbrcd.AddRange(SQLHelper.ToList<tmbrcd>(dt));
                        }

                        foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                        {
                            var tmbrcd = Listtmbrcd.Where(o => o.BARCD_ID == WarehouseInputDetailBarcode.Barcode).FirstOrDefault();
                            if (tmbrcd != null && tmbrcd.BARCD_IDX > 0)
                            {
                                WarehouseInputDetailBarcode.MESID = tmbrcd.BARCD_IDX;
                                WarehouseInputDetailBarcode.PKG_QTY = tmbrcd.PKG_QTY;
                                WarehouseInputDetailBarcode.PKG_OUTQTY = tmbrcd.PKG_OUTQTY;
                                WarehouseInputDetailBarcode.BARCD_LOC = tmbrcd.BARCD_LOC;
                                WarehouseInputDetailBarcode.PKG_GRP = tmbrcd.PKG_GRP;
                                WarehouseInputDetailBarcode.OUT_DTM = tmbrcd.OUT_DTM;
                                WarehouseInputDetailBarcode.CREATE_DTM = tmbrcd.CREATE_DTM;
                                WarehouseInputDetailBarcode.QuantityMES = 0;
                                WarehouseInputDetailBarcode.QuantityOutputMES = 0;
                                WarehouseInputDetailBarcode.PKG_QTYActual = 0;
                                try
                                {
                                    WarehouseInputDetailBarcode.QuantityMES = (decimal)(tmbrcd.PKG_QTY - tmbrcd.PKG_OUTQTY);
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                                try
                                {
                                    var Listtmbrcd_hisSub = Listtmbrcd_his.Where(o => o.BARCD_ID == WarehouseInputDetailBarcode.Barcode && o.OUT_DTM != null && WarehouseInputDetailBarcode.UpdateDate != null && o.OUT_DTM >= WarehouseInputDetailBarcode.UpdateDate).ToList();
                                    WarehouseInputDetailBarcode.QuantityOutputMES = Listtmbrcd_hisSub.Sum(o => o.PKG_OUTQTY);
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                                try
                                {

                                    var tsuser = Listtsuser.Where(o => o.USER_IDX == int.Parse(tmbrcd.CREATE_USER)).FirstOrDefault();
                                    if (tsuser != null && tsuser.USER_IDX > 0)
                                    {
                                        try
                                        {
                                            var USER_ID = int.Parse(tsuser.USER_ID);
                                            BaseParameter.ParentID = 16;
                                        }
                                        catch (Exception ex)
                                        {
                                            string mes = ex.Message;
                                            BaseParameter.ParentID = 17;
                                        }
                                    }
                                    else
                                    {
                                        BaseParameter.ParentID = 16;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                            }
                            try
                            {
                                WarehouseInputDetailBarcode.QuantityOutputMES = WarehouseInputDetailBarcode.QuantityOutputMES ?? 0;
                                WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityOutputMES;
                                WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.QuantityInventory - WarehouseInputDetailBarcode.QuantityMES);

                                WarehouseInputDetailBarcode.QuantityGAP01 = (decimal)WarehouseInputDetailBarcode.PKG_QTYActual;
                                WarehouseInputDetailBarcode.QuantityGAP02 = WarehouseInputDetailBarcode.Quantity02 - WarehouseInputDetailBarcode.QuantityMES;
                                WarehouseInputDetailBarcode.QuantityGAP03 = WarehouseInputDetailBarcode.Quantity03 - WarehouseInputDetailBarcode.QuantityMES;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            if (BaseParameter.CompanyID == BaseParameter.ParentID)
                            {
                                result.List.Add(WarehouseInputDetailBarcode);
                            }
                        }
                        break;
                    case 108:
                    case 199:
                        //var Listtdpdmtim = await _tdpdmtimRepository.GetByCondition(o => o.VLID_DSCN_YN != "Y").ToListAsync();

                        var Listtdpdmtim = new List<tdpdmtim>();
                        sql = @"select * from tdpdmtim where VLID_DSCN_YN != 'Y'";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            Listtdpdmtim.AddRange(SQLHelper.ToList<tdpdmtim>(dt));
                        }
                        foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                        {
                            var tdpdmtim = Listtdpdmtim.Where(o => o.VLID_BARCODE == WarehouseInputDetailBarcode.Barcode).FirstOrDefault();
                            if (tdpdmtim != null && tdpdmtim.VLID_PART_IDX > 0)
                            {
                                WarehouseInputDetailBarcode.MESID = tdpdmtim.PDMTIN_IDX;
                                WarehouseInputDetailBarcode.Description = tdpdmtim.VLID_GRP;
                                WarehouseInputDetailBarcode.BARCD_LOC = tdpdmtim.BARCD_LOC;
                                WarehouseInputDetailBarcode.QuantityMES = 1;
                                WarehouseInputDetailBarcode.PKG_QTYActual = 0;
                                try
                                {
                                    WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityMES);
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                                try
                                {

                                    var tsuser = Listtsuser.Where(o => o.USER_IDX == int.Parse(tdpdmtim.CREATE_USER)).FirstOrDefault();
                                    if (tsuser != null && tsuser.USER_IDX > 0)
                                    {
                                        try
                                        {
                                            var USER_ID = int.Parse(tsuser.USER_ID);
                                            if (BaseParameter.CompanyID == 16)
                                            {
                                                result.List.Add(WarehouseInputDetailBarcode);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            string mes = ex.Message;
                                            if (BaseParameter.CompanyID == 17)
                                            {
                                                result.List.Add(WarehouseInputDetailBarcode);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (BaseParameter.CompanyID == 16)
                                        {
                                            result.List.Add(WarehouseInputDetailBarcode);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                            }
                            try
                            {
                                WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity;
                                WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityMES);

                                WarehouseInputDetailBarcode.QuantityGAP01 = (decimal)WarehouseInputDetailBarcode.PKG_QTYActual;
                                WarehouseInputDetailBarcode.QuantityGAP02 = WarehouseInputDetailBarcode.Quantity02 - WarehouseInputDetailBarcode.QuantityMES;
                                WarehouseInputDetailBarcode.QuantityGAP03 = WarehouseInputDetailBarcode.Quantity03 - WarehouseInputDetailBarcode.QuantityMES;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            if (BaseParameter.CompanyID == BaseParameter.ParentID)
                            {
                                result.List.Add(WarehouseInputDetailBarcode);
                            }
                        }
                        break;
                    case 86:
                    case 195:
                        //var Listtrackmtim = await _trackmtimRepository.GetByCondition(o => o.RACKOUT_DTM == null).ToListAsync();

                        var Listtrackmtim = new List<trackmtim>();
                        sql = @"select * from trackmtim where RACKOUT_DTM is null";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            Listtrackmtim.AddRange(SQLHelper.ToList<trackmtim>(dt));
                        }
                        foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                        {
                            var trackmtim = Listtrackmtim.Where(o => o.BARCODE_NM == WarehouseInputDetailBarcode.Barcode).FirstOrDefault();
                            if (trackmtim != null && trackmtim.TRACK_IDX > 0)
                            {
                                WarehouseInputDetailBarcode.MESID = trackmtim.TRACK_IDX;
                                WarehouseInputDetailBarcode.QuantityMES = (decimal)trackmtim.QTY;
                                WarehouseInputDetailBarcode.PKG_QTYActual = 0;
                                try
                                {
                                    WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityMES);
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                                try
                                {

                                    var tsuser = Listtsuser.Where(o => o.USER_IDX == int.Parse(trackmtim.CREATE_USER)).FirstOrDefault();
                                    if (tsuser != null && tsuser.USER_IDX > 0)
                                    {
                                        try
                                        {
                                            var USER_ID = int.Parse(tsuser.USER_ID);
                                            if (BaseParameter.CompanyID == 16)
                                            {
                                                result.List.Add(WarehouseInputDetailBarcode);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            string mes = ex.Message;
                                            if (BaseParameter.CompanyID == 17)
                                            {
                                                result.List.Add(WarehouseInputDetailBarcode);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (BaseParameter.CompanyID == 16)
                                        {
                                            result.List.Add(WarehouseInputDetailBarcode);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string mes = ex.Message;
                                }
                            }
                            try
                            {
                                WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity;
                                WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityMES);

                                WarehouseInputDetailBarcode.QuantityGAP01 = (decimal)WarehouseInputDetailBarcode.PKG_QTYActual;
                                WarehouseInputDetailBarcode.QuantityGAP02 = WarehouseInputDetailBarcode.Quantity02 - WarehouseInputDetailBarcode.QuantityMES;
                                WarehouseInputDetailBarcode.QuantityGAP03 = WarehouseInputDetailBarcode.Quantity03 - WarehouseInputDetailBarcode.QuantityMES;
                            }
                            catch (Exception ex)
                            {
                                string mes = ex.Message;
                            }
                            if (BaseParameter.CompanyID == BaseParameter.ParentID)
                            {
                                result.List.Add(WarehouseInputDetailBarcode);
                            }
                        }
                        break;

                }
            }
            switch (BaseParameter.Action)
            {
                case 1:
                    result.List = result.List.Where(o => o.CategoryLocationID > 0).ToList();
                    break;
                case 2:
                    result.List = result.List.Where(o => o.CategoryLocationID == null || o.CategoryLocationID == 0).ToList();
                    break;
            }
            var ListMaterialID = result.List.Select(o => o.MaterialID).Distinct().ToList();
            var ListResult = new List<WarehouseInputDetailBarcode>();
            foreach (var MaterialID in ListMaterialID)
            {
                var ListSub = result.List.Where(o => o.MaterialID == MaterialID).ToList();
                if (ListSub.Count > 0)
                {
                    try
                    {
                        var WarehouseInputDetailBarcode = ListSub[0];
                        WarehouseInputDetailBarcode.PKG_QTY = ListSub.Sum(o => o.PKG_QTY);
                        WarehouseInputDetailBarcode.PKG_OUTQTY = ListSub.Sum(o => o.PKG_OUTQTY);
                        WarehouseInputDetailBarcode.QuantityMES = (decimal)(WarehouseInputDetailBarcode.PKG_QTY - WarehouseInputDetailBarcode.PKG_OUTQTY);
                        WarehouseInputDetailBarcode.QuantityInvoice = ListSub.Sum(o => o.QuantityInvoice);
                        WarehouseInputDetailBarcode.Quantity = ListSub.Sum(o => o.Quantity);
                        WarehouseInputDetailBarcode.QuantityOutput = ListSub.Sum(o => o.QuantityOutput);
                        WarehouseInputDetailBarcode.QuantityOutputMES = ListSub.Sum(o => o.QuantityOutputMES);
                        WarehouseInputDetailBarcode.QuantityInventory = ListSub.Sum(o => o.QuantityInventory);
                        WarehouseInputDetailBarcode.Quantity01 = ListSub.Sum(o => o.Quantity01);
                        WarehouseInputDetailBarcode.Quantity02 = ListSub.Sum(o => o.Quantity02);
                        WarehouseInputDetailBarcode.Quantity03 = ListSub.Sum(o => o.Quantity03);
                        WarehouseInputDetailBarcode.PKG_QTYActual = (double)(WarehouseInputDetailBarcode.QuantityInventory - WarehouseInputDetailBarcode.QuantityMES);
                        WarehouseInputDetailBarcode.QuantityGAP01 = WarehouseInputDetailBarcode.Quantity01 - WarehouseInputDetailBarcode.QuantityMES;
                        WarehouseInputDetailBarcode.QuantityGAP02 = WarehouseInputDetailBarcode.Quantity02 - WarehouseInputDetailBarcode.QuantityMES;
                        WarehouseInputDetailBarcode.QuantityGAP03 = WarehouseInputDetailBarcode.Quantity03 - WarehouseInputDetailBarcode.QuantityMES;
                        ListResult.Add(WarehouseInputDetailBarcode);
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }

                }
            }
            result.List = ListResult;
            result.List = result.List.OrderBy(o => o.MaterialName).ToList();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncByParrentIDAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                string sql = @"delete from WarehouseInputDetail where ParentID=" + BaseParameter.ParentID;
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);

                sql = @"update WarehouseInputDetailBarcode set MaterialID=NULL, MaterialName=NULL, WarehouseInputDetailID=NULL where ParentID=" + BaseParameter.ParentID;
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);

                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                foreach (var WarehouseInputDetailBarcode in result.List)
                {
                    var BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                    BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                    await SaveAsync(BaseParameterWarehouseInputDetailBarcode);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> SyncStockAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                var WarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.ID == BaseParameter.ParentID).OrderByDescending(o => o.Date).FirstOrDefaultAsync();
                if (WarehouseInput != null && WarehouseInput.ID > 0)
                {
                    if (WarehouseInput.Active == true && WarehouseInput.IsStock == true)
                    {
                        int? Year = GlobalHelper.YearStock;
                        if (WarehouseInput.Date != null)
                        {
                            Year = WarehouseInput.Date.Value.Year;
                        }
                        var ListWarehouseInputDetailBarcodeStock = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.ParentID == WarehouseInput.ID).OrderByDescending(o => o.DateScan).ToListAsync();
                        var ListWarehouseInputDetailBarcodeStockMaterialID = ListWarehouseInputDetailBarcodeStock.Select(o => o.MaterialID).Distinct().ToList();
                        var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.DateScan != null && o.DateScan.Value.Year >= Year && o.Active == true && o.IsStock != true && o.CategoryDepartmentID == WarehouseInput.CustomerID && ListWarehouseInputDetailBarcodeStockMaterialID.Contains(o.MaterialID)).OrderByDescending(o => o.DateScan).ToListAsync();
                        var ListWarehouseOutputDetailBarcode = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.DateScan != null && o.DateScan.Value.Year >= Year && o.Active == true && o.CategoryDepartmentID == WarehouseInput.CustomerID && ListWarehouseInputDetailBarcodeStockMaterialID.Contains(o.MaterialID)).OrderByDescending(o => o.DateScan).ToListAsync();
                        for (int i = 0; i < ListWarehouseInputDetailBarcodeStock.Count; i++)
                        {
                            ListWarehouseInputDetailBarcodeStock[i].Active = true;
                            ListWarehouseInputDetailBarcodeStock[i].IsStock = true;
                            ListWarehouseInputDetailBarcodeStock[i].QuantityMES = ListWarehouseInputDetailBarcode.Where(o => o.MaterialID == ListWarehouseInputDetailBarcodeStock[i].MaterialID).ToList().Sum(o => o.Quantity);
                            ListWarehouseInputDetailBarcodeStock[i].QuantityOutput = ListWarehouseOutputDetailBarcode.Where(o => o.MaterialID == ListWarehouseInputDetailBarcodeStock[i].MaterialID).ToList().Sum(o => o.Quantity);
                            ListWarehouseInputDetailBarcodeStock[i].Quantity = ListWarehouseInputDetailBarcodeStock[i].QuantitySNP + ListWarehouseInputDetailBarcodeStock[i].QuantityMES;
                            ListWarehouseInputDetailBarcodeStock[i].QuantityInventory = ListWarehouseInputDetailBarcodeStock[i].Quantity - ListWarehouseInputDetailBarcodeStock[i].QuantityOutput;
                        }
                        await _WarehouseInputDetailBarcodeRepository.UpdateRangeAsync(ListWarehouseInputDetailBarcodeStock);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByHOOKRACK_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.Active == true && o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.CustomerName) && o.CustomerName.Contains("HOOK_RACK")).ToListAsync();
                    if (ListWarehouseInput.Count > 0)
                    {
                        var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                        result.List = await GetByCondition(o => o.Active == true && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.MaterialName == BaseParameter.SearchString && o.QuantityInventory > 0).ToListAsync();
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByKOMAX_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    string DateEnd = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd 23:59:59");
                    string DateBegin = GlobalHelper.InitializationDateTime.AddDays(0).ToString("yyyy-MM-dd 00:00:00");
                    string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                    List<torderlist> Listtorderlist = new List<torderlist>();
                    List<torder_barcode> Listtorder_barcode = new List<torder_barcode>();
                    string sql = @"select * from torderlist WHERE LEAD_NO='" + BaseParameter.SearchString + "' AND UPDATE_DTM >= '" + DateBegin + "' AND UPDATE_DTM <= '" + DateEnd + "' AND `CONDITION` IN ('Complete', 'Working') AND (MC NOT IN ('SHIELD WIRE') OR MC NOT IN ('SHIELD WIRE'))";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorderlist.AddRange(SQLHelper.ToList<torderlist>(dt));
                    }
                    string ListtorderlistORDER_IDX = string.Join(",", Listtorderlist.Select(x => x.ORDER_IDX));
                    sql = @"SELECT * FROM TORDER_BARCODE WHERE DSCN_YN='Y' AND ORDER_IDX in (" + ListtorderlistORDER_IDX + ")";
                    ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorder_barcode.AddRange(SQLHelper.ToList<torder_barcode>(dt));
                    }
                    var ListMembership = await _MembershipRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID).ToListAsync();
                    foreach (var torder_barcode in Listtorder_barcode)
                    {
                        WarehouseInputDetailBarcode WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                        var torderlist = Listtorderlist.Where(o => o.ORDER_IDX == torder_barcode.ORDER_IDX).FirstOrDefault();
                        if (torderlist != null && torderlist.ORDER_IDX > 0)
                        {
                            //WarehouseInputDetailBarcode.Quantity = (decimal?)torderlist.BUNDLE_SIZE;
                            WarehouseInputDetailBarcode.CategoryLocationName = torderlist.MC2 ?? torderlist.MC;
                        }
                        WarehouseInputDetailBarcode.Active = true;
                        WarehouseInputDetailBarcode.ID = (long)torder_barcode.TORDER_BARCODE_IDX;
                        WarehouseInputDetailBarcode.ParentID = (long?)torder_barcode.ORDER_IDX;
                        WarehouseInputDetailBarcode.Barcode = torder_barcode.TORDER_BARCODENM;
                        WarehouseInputDetailBarcode.DateScan = torder_barcode.WORK_END;
                        WarehouseInputDetailBarcode.UpdateUserCode = torder_barcode.UPDATE_USER;
                        var Membership = ListMembership.Where(o => o.UserName == WarehouseInputDetailBarcode.UpdateUserCode).FirstOrDefault();
                        if (Membership != null && Membership.ID > 0)
                        {
                            WarehouseInputDetailBarcode.UpdateUserName = Membership.Name;
                        }
                        result.List.Add(WarehouseInputDetailBarcode);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetBySHIELDWIRE_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    string DateEnd = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd 23:59:59");
                    string DateBegin = GlobalHelper.InitializationDateTime.AddDays(0).ToString("yyyy-MM-dd 00:00:00");
                    string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                    List<torderlist> Listtorderlist = new List<torderlist>();
                    List<torder_barcode> Listtorder_barcode = new List<torder_barcode>();
                    string sql = @"select * from torderlist WHERE LEAD_NO='" + BaseParameter.SearchString + "' AND UPDATE_DTM >= '" + DateBegin + "' AND UPDATE_DTM <= '" + DateEnd + "' AND `CONDITION` IN ('Complete', 'Working') AND (MC IN ('SHIELD WIRE') OR MC IN ('SHIELD WIRE'))";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorderlist.AddRange(SQLHelper.ToList<torderlist>(dt));
                    }
                    string ListtorderlistORDER_IDX = string.Join(",", Listtorderlist.Select(x => x.ORDER_IDX));
                    sql = @"SELECT * FROM TORDER_BARCODE WHERE DSCN_YN='Y' AND ORDER_IDX in (" + ListtorderlistORDER_IDX + ")";
                    ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorder_barcode.AddRange(SQLHelper.ToList<torder_barcode>(dt));
                    }
                    var ListMembership = await _MembershipRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID).ToListAsync();
                    foreach (var torder_barcode in Listtorder_barcode)
                    {
                        WarehouseInputDetailBarcode WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                        var torderlist = Listtorderlist.Where(o => o.ORDER_IDX == torder_barcode.ORDER_IDX).FirstOrDefault();
                        if (torderlist != null && torderlist.ORDER_IDX > 0)
                        {
                            //WarehouseInputDetailBarcode.Quantity = (decimal?)torderlist.BUNDLE_SIZE;
                            WarehouseInputDetailBarcode.CategoryLocationName = torderlist.MC2 ?? torderlist.MC;
                        }
                        WarehouseInputDetailBarcode.Active = true;
                        WarehouseInputDetailBarcode.ID = (long)torder_barcode.TORDER_BARCODE_IDX;
                        WarehouseInputDetailBarcode.ParentID = (long?)torder_barcode.ORDER_IDX;
                        WarehouseInputDetailBarcode.Barcode = torder_barcode.TORDER_BARCODENM;
                        WarehouseInputDetailBarcode.DateScan = torder_barcode.WORK_END;
                        var Membership = ListMembership.Where(o => o.UserName == WarehouseInputDetailBarcode.UpdateUserCode).FirstOrDefault();
                        if (Membership != null && Membership.ID > 0)
                        {
                            WarehouseInputDetailBarcode.UpdateUserName = Membership.Name;
                        }
                        result.List.Add(WarehouseInputDetailBarcode);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetByLP_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    string DateEnd = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd 23:59:59");
                    string DateBegin = GlobalHelper.InitializationDateTime.AddDays(0).ToString("yyyy-MM-dd 00:00:00");
                    string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                    List<torderlist> Listtorderlist = new List<torderlist>();
                    List<torder_barcode_lp> Listtorder_barcode_lp = new List<torder_barcode_lp>();
                    string sql = @"SELECT TORDERLIST.* from TORDERLIST_LP JOIN TORDERLIST ON TORDERLIST_LP.ORDER_IDX=TORDERLIST.ORDER_IDX WHERE TORDERLIST.LEAD_NO='" + BaseParameter.SearchString + "' AND TORDERLIST_LP.UPDATE_DTM >= '" + DateBegin + "' AND TORDERLIST_LP.UPDATE_DTM <= '" + DateEnd + "' AND TORDERLIST_LP.`CONDITION` IN ('Complete', 'Working')";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorderlist.AddRange(SQLHelper.ToList<torderlist>(dt));
                    }
                    string ListtorderlistORDER_IDX = string.Join(",", Listtorderlist.Select(x => x.ORDER_IDX));
                    sql = @"SELECT * FROM torder_barcode_lp WHERE DSCN_YN='Y' AND ORDER_IDX in (" + ListtorderlistORDER_IDX + ")";
                    ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorder_barcode_lp.AddRange(SQLHelper.ToList<torder_barcode_lp>(dt));
                    }
                    var ListMembership = await _MembershipRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID).ToListAsync();
                    foreach (var torder_barcode_lp in Listtorder_barcode_lp)
                    {
                        WarehouseInputDetailBarcode WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                        var torderlist = Listtorderlist.Where(o => o.ORDER_IDX == torder_barcode_lp.ORDER_IDX).FirstOrDefault();
                        if (torderlist != null && torderlist.ORDER_IDX > 0)
                        {
                            //WarehouseInputDetailBarcode.Quantity = (decimal?)torderlist.BUNDLE_SIZE;
                            WarehouseInputDetailBarcode.CategoryLocationName = torderlist.MC2 ?? torderlist.MC;
                        }
                        WarehouseInputDetailBarcode.Active = true;
                        WarehouseInputDetailBarcode.ID = (long)torder_barcode_lp.TORDER_BARCODE_IDX;
                        WarehouseInputDetailBarcode.ParentID = (long?)torder_barcode_lp.ORDER_IDX;
                        WarehouseInputDetailBarcode.Barcode = torder_barcode_lp.TORDER_BARCODENM;
                        WarehouseInputDetailBarcode.DateScan = torder_barcode_lp.WORK_END;
                        var Membership = ListMembership.Where(o => o.UserName == WarehouseInputDetailBarcode.UpdateUserCode).FirstOrDefault();
                        if (Membership != null && Membership.ID > 0)
                        {
                            WarehouseInputDetailBarcode.UpdateUserName = Membership.Name;
                        }
                        result.List.Add(WarehouseInputDetailBarcode);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> GetBySPST_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            result.List = new List<WarehouseInputDetailBarcode>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    string DateEnd = GlobalHelper.InitializationDateTime.ToString("yyyy-MM-dd 23:59:59");
                    string DateBegin = GlobalHelper.InitializationDateTime.AddDays(0).ToString("yyyy-MM-dd 00:00:00");
                    string MariaDBConectionString = GlobalHelper.MESConectionStringByCompanyID(BaseParameter.CompanyID.Value);
                    List<torderlist_spst> Listtorderlist_spst = new List<torderlist_spst>();
                    List<torder_barcode_sp> Listtorder_barcode_sp = new List<torder_barcode_sp>();
                    string sql = @"SELECT * from torderlist_spst WHERE LEAD_NO='" + BaseParameter.SearchString + "' AND UPDATE_DTM >= '" + DateBegin + "' AND UPDATE_DTM <= '" + DateEnd + "' AND `CONDITION` IN ('Complete', 'Working')";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorderlist_spst.AddRange(SQLHelper.ToList<torderlist_spst>(dt));
                    }
                    string ListtorderlistORDER_IDX = string.Join(",", Listtorderlist_spst.Select(x => x.ORDER_IDX));
                    sql = @"SELECT * FROM torder_barcode_sp WHERE DSCN_YN='Y' AND ORDER_IDX in (" + ListtorderlistORDER_IDX + ")";
                    ds = await MySQLHelper.FillDataSetBySQLAsync(MariaDBConectionString, sql);
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        Listtorder_barcode_sp.AddRange(SQLHelper.ToList<torder_barcode_sp>(dt));
                    }
                    var ListMembership = await _MembershipRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID).ToListAsync();
                    foreach (var torder_barcode_sp in Listtorder_barcode_sp)
                    {
                        WarehouseInputDetailBarcode WarehouseInputDetailBarcode = new WarehouseInputDetailBarcode();
                        var torderlist_spst = Listtorderlist_spst.Where(o => o.ORDER_IDX == torder_barcode_sp.ORDER_IDX).FirstOrDefault();
                        if (torderlist_spst != null && torderlist_spst.ORDER_IDX > 0)
                        {
                            //WarehouseInputDetailBarcode.Quantity = (decimal?)torderlist.BUNDLE_SIZE;
                            WarehouseInputDetailBarcode.CategoryLocationName = torderlist_spst.MC;
                        }
                        WarehouseInputDetailBarcode.Active = true;
                        WarehouseInputDetailBarcode.ID = (long)torder_barcode_sp.TORDER_BARCODE_IDX;
                        WarehouseInputDetailBarcode.ParentID = (long?)torder_barcode_sp.ORDER_IDX;
                        WarehouseInputDetailBarcode.Barcode = torder_barcode_sp.TORDER_BARCODENM;
                        WarehouseInputDetailBarcode.DateScan = torder_barcode_sp.WORK_END;
                        WarehouseInputDetailBarcode.UpdateUserCode = torder_barcode_sp.UPDATE_USER;
                        var Membership = ListMembership.Where(o => o.UserName == WarehouseInputDetailBarcode.UpdateUserCode).FirstOrDefault();
                        if (Membership != null && Membership.ID > 0)
                        {
                            WarehouseInputDetailBarcode.UpdateUserName = Membership.Name;
                        }
                        result.List.Add(WarehouseInputDetailBarcode);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcode>> ExportToExcelAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID && o.Active == true).OrderByDescending(o => o.FileName).ThenBy(o => o.MaterialName).ToListAsync();
                if (result.List.Count > 0)
                {
                    string fileName = BaseParameter.ParentID + "-" + GlobalHelper.InitializationDateTimeCode0001 + ".xlsx";
                    var streamExport = new MemoryStream();
                    InitializationExcel(result.List, streamExport);
                    var physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, fileName);
                    using (var stream = new FileStream(physicalPathCreate, FileMode.Create))
                    {
                        streamExport.CopyTo(stream);
                    }
                    result.Message = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + fileName;
                }
            }
            return result;
        }
        private void InitializationExcel(List<WarehouseInputDetailBarcode> list, MemoryStream streamExport)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "No";
                column = column + 1;
                workSheet.Cells[row, column].Value = "ID";
                column = column + 1;
                workSheet.Cells[row, column].Value = "MaterialID";
                column = column + 1;
                workSheet.Cells[row, column].Value = "OEM";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Family";
                column = column + 1;
                workSheet.Cells[row, column].Value = "PART NO";
                column = column + 1;
                workSheet.Cells[row, column].Value = "2026 - Stock Begin";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Input";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Quantity";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Output";
                column = column + 1;
                workSheet.Cells[row, column].Value = "Stock End";

                for (int i = 1; i <= column; i++)
                {
                    workSheet.Cells[1, i].Style.Font.Bold = true;
                    workSheet.Cells[1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[1, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[1, i].Style.Font.Size = 16;
                    workSheet.Cells[1, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }
                row = row + 1;
                int no = 1;
                foreach (WarehouseInputDetailBarcode item in list)
                {
                    workSheet.Cells[row, 1].Value = no;
                    workSheet.Cells[row, 2].Value = item.ID;
                    workSheet.Cells[row, 3].Value = item.MaterialID;
                    workSheet.Cells[row, 4].Value = item.FileName;
                    workSheet.Cells[row, 5].Value = item.CategoryFamilyName;
                    workSheet.Cells[row, 6].Value = item.MaterialName;
                    workSheet.Cells[row, 7].Value = item.QuantityInvoice;
                    workSheet.Cells[row, 8].Value = item.QuantityMES;
                    workSheet.Cells[row, 9].Value = item.Quantity;
                    workSheet.Cells[row, 10].Value = item.QuantityOutput;
                    workSheet.Cells[row, 11].Value = item.QuantityInventory;


                    for (int i = 1; i <= column; i++)
                    {
                        workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[row, i].Style.Font.Size = 16;
                        workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }

                    row = row + 1;
                    no = no + 1;
                }
                for (int i = 1; i <= column; i++)
                {
                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;

        }
    }
}

