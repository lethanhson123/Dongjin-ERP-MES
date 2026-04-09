namespace Service.Implement
{
    public class WarehouseOutputDetailBarcodeService : BaseService<WarehouseOutputDetailBarcode, IWarehouseOutputDetailBarcodeRepository>
    , IWarehouseOutputDetailBarcodeService
    {
        private readonly IWarehouseOutputDetailBarcodeRepository _WarehouseOutputDetailBarcodeRepository;
        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly IWarehouseOutputDetailService _WarehouseOutputDetailService;
        private readonly IWarehouseOutputDetailRepository _WarehouseOutputDetailRepository;
        private readonly IWarehouseInputDetailBarcodeService _WarehouseInputDetailBarcodeService;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryLocationRepository _CategoryLocationRepository;
        private readonly IWarehouseInputService _WarehouseInputService;
        private readonly IWarehouseInventoryService _WarehourseInventoryService;
        private readonly IWarehouseStockService _WarehouseStockService;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IWarehouseOutputDetailBarcodeMaterialRepository _WarehouseOutputDetailBarcodeMaterialRepository;
        private readonly IWarehouseInputDetailBarcodeMaterialRepository _WarehouseInputDetailBarcodeMaterialRepository;
        private readonly IWarehouseOutputMaterialRepository _WarehouseOutputMaterialRepository;
        public WarehouseOutputDetailBarcodeService(IWarehouseOutputDetailBarcodeRepository WarehouseOutputDetailBarcodeRepository
            , IWarehouseOutputRepository WarehouseOutputRepository
            , IWarehouseOutputDetailService WarehouseOutputDetailService
            , IWarehouseOutputDetailRepository WarehouseOutputDetailRepository
            , IWarehouseInputDetailBarcodeService WarehouseInputDetailBarcodeService
            , IWarehouseInputDetailBarcodeRepository WarehouseInputDetailBarcodeRepository
            , IWarehouseInputRepository WarehouseInputRepository
            , IMaterialRepository MaterialRepository
            , ICategoryUnitRepository CategoryUnitRepository
            , ICategoryLocationRepository CategoryLocationRepository
            , IWarehouseInputService WarehouseInputService
            , IWarehouseInventoryService warehourseInventoryService
            , IWarehouseStockService WarehouseStockService
            , ICategoryDepartmentRepository CategoryDepartmentRepository
            , IWarehouseOutputDetailBarcodeMaterialRepository WarehouseOutputDetailBarcodeMaterialRepository
            , IWarehouseInputDetailBarcodeMaterialRepository WarehouseInputDetailBarcodeMaterialRepository
            , IWarehouseOutputMaterialRepository WarehouseOutputMaterialRepository

            ) : base(WarehouseOutputDetailBarcodeRepository)
        {
            _WarehouseOutputDetailBarcodeRepository = WarehouseOutputDetailBarcodeRepository;
            _WarehouseOutputRepository = WarehouseOutputRepository;
            _WarehouseOutputDetailService = WarehouseOutputDetailService;
            _WarehouseOutputDetailRepository = WarehouseOutputDetailRepository;
            _WarehouseInputDetailBarcodeService = WarehouseInputDetailBarcodeService;
            _WarehouseInputDetailBarcodeRepository = WarehouseInputDetailBarcodeRepository;
            _WarehouseInputRepository = WarehouseInputRepository;
            _MaterialRepository = MaterialRepository;
            _CategoryUnitRepository = CategoryUnitRepository;
            _CategoryLocationRepository = CategoryLocationRepository;
            _WarehouseInputService = WarehouseInputService;
            _WarehourseInventoryService = warehourseInventoryService;
            _WarehouseStockService = WarehouseStockService;
            _CategoryDepartmentRepository = CategoryDepartmentRepository;
            _WarehouseOutputDetailBarcodeMaterialRepository = WarehouseOutputDetailBarcodeMaterialRepository;
            _WarehouseInputDetailBarcodeMaterialRepository = WarehouseInputDetailBarcodeMaterialRepository;
            _WarehouseOutputMaterialRepository = WarehouseOutputMaterialRepository;
        }
        public override void InitializationSave(WarehouseOutputDetailBarcode model)
        {
            if (model.IsScan == true)
            {
                model.IsScan = false;
            }
            else
            {
                BaseInitialization(model);
                if (model.WarehouseInputID == null)
                {
                    string sql = @"UPDATE WarehouseOutputDetailBarcode JOIN WarehouseInputDetailBarcode ON WarehouseOutputDetailBarcode.Barcode=WarehouseInputDetailBarcode.Barcode SET WarehouseOutputDetailBarcode.WarehouseInputID=WarehouseInputDetailBarcode.ParentID, WarehouseOutputDetailBarcode.WarehouseInputName=WarehouseInputDetailBarcode.ParentName WHERE WarehouseOutputDetailBarcode.WarehouseInputID IS NULL AND WarehouseOutputDetailBarcode.CategoryDepartmentID=" + model.CategoryDepartmentID + " AND WarehouseInputDetailBarcode.CategoryDepartmentID=" + model.CategoryDepartmentID + " AND WarehouseOutputDetailBarcode.Barcode='" + model.Barcode + "'";
                    MySQLHelper.ExecuteNonQuery(GlobalHelper.ERP_MariaDBConectionString, sql);
                }
                if (model.Active == true)
                {
                    if (model.DateScan == null)
                    {
                        model.DateScan = GlobalHelper.InitializationDateTime;
                    }
                }
                if (model.ParentID > 0)
                {
                    var Parent = _WarehouseOutputRepository.GetByID(model.ParentID.Value);
                    model.ParentName = Parent.Code;
                    model.Date = Parent.Date;
                    model.Code = model.Code ?? Parent.Code;
                    model.CompanyID = Parent.CompanyID;
                    model.CompanyName = Parent.CompanyName;
                    model.CategoryDepartmentID = Parent.SupplierID;
                    model.CategoryDepartmentName = Parent.SupplierName;
                    var WarehouseInputDetailBarcode = _WarehouseInputDetailBarcodeService.GetByCondition(o => o.CategoryDepartmentID == model.CategoryDepartmentID && o.Barcode == model.Barcode && o.Active == true).FirstOrDefault();
                    if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                    {
                        model.Price = WarehouseInputDetailBarcode.Price;
                        model.QuantityInventory = WarehouseInputDetailBarcode.QuantityInventory;
                        if (model.Active == true)
                        {
                            if (model.Quantity > model.QuantityInventory)
                            {
                                model.Quantity = model.QuantityInventory;
                            }
                        }
                    }
                }

                model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
                model.UpdateDate = GlobalHelper.InitializationDateTime;
                if (model.UpdateDate != null)
                {
                    model.Year = model.Year ?? model.UpdateDate.Value.Year;
                    model.Month = model.Month ?? model.UpdateDate.Value.Month;
                    model.Day = model.Day ?? model.UpdateDate.Value.Day;
                    model.Week = model.Week ?? GlobalHelper.GetWeekByDateTime(model.UpdateDate.Value);
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
                        var CategoryLocation = _CategoryLocationRepository.GetByName(model.CategoryLocationName);
                        if (CategoryLocation.ID == 0)
                        {
                            CategoryLocation.Active = true;
                            CategoryLocation.Name = model.CategoryLocationName;
                            _CategoryLocationRepository.Add(CategoryLocation);
                        }
                        model.CategoryLocationID = CategoryLocation.ID;
                    }
                }
                if (!string.IsNullOrEmpty(model.Barcode))
                {
                    if (string.IsNullOrEmpty(model.MaterialName))
                    {
                        model.MaterialName = model.Barcode.Split('$')[0];
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
                model.MaterialName = Material.Code;
                model.QuantitySNP = Material.QuantitySNP;
                model.Display = Material.Name;
                model.IsSNP = model.IsSNP ?? Material.IsSNP;
                model.FileName = Material.CategoryLineName;
                model.CategoryFamilyID = model.CategoryFamilyID ?? Material.CategoryFamilyID;
                model.CategoryFamilyName = model.CategoryFamilyName ?? Material.CategoryFamilyName;
                model.CategoryCompanyName = Material.OriginalEquipmentManufacturer;
                if (model.WarehouseOutputDetailID > 0)
                {

                }
                else
                {
                    var WarehouseOutputDetail = _WarehouseOutputDetailRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialID == model.MaterialID).FirstOrDefault();
                    if (WarehouseOutputDetail == null)
                    {
                        WarehouseOutputDetail = _WarehouseOutputDetailRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialName == model.MaterialName && o.QuantitySNP == model.QuantitySNP && o.Description == model.Description).FirstOrDefault();
                    }
                    if (WarehouseOutputDetail == null)
                    {
                        WarehouseOutputDetail = new WarehouseOutputDetail();
                        WarehouseOutputDetail.Active = true;
                        WarehouseOutputDetail.ParentID = model.ParentID;
                        WarehouseOutputDetail.ParentName = model.ParentName;
                        WarehouseOutputDetail.Code = model.Code;
                        WarehouseOutputDetail.MaterialID = model.MaterialID;
                        WarehouseOutputDetail.MaterialName = model.MaterialName;
                        WarehouseOutputDetail.Display = model.Display;
                        WarehouseOutputDetail.CategoryUnitID = model.CategoryUnitID;
                        WarehouseOutputDetail.CategoryUnitName = model.CategoryUnitName;
                        WarehouseOutputDetail.CategoryLocationID = model.CategoryLocationID;
                        WarehouseOutputDetail.CategoryLocationName = model.CategoryLocationName;
                        _WarehouseOutputDetailRepository.Add(WarehouseOutputDetail);
                        model.WarehouseOutputDetailID = WarehouseOutputDetail.ID;
                    }
                }
                model.Quantity = model.Quantity ?? GlobalHelper.InitializationNumber;
                model.Description = model.Description ?? model.Barcode;
                model.QuantityInput = model.QuantityInput ?? GlobalHelper.InitializationNumber;
                model.QuantityReturn = model.QuantityReturn ?? GlobalHelper.InitializationNumber;
                model.Quantity = model.Quantity - model.QuantityReturn;
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
                model.TotalTax = model.Total * model.Tax / 100;
                model.TotalDiscount = model.Total * model.Discount / 100;
                model.TotalFinal = model.Total + model.TotalTax - model.TotalDiscount;


            }
        }
        public virtual void InitializationSaveHookRack(WarehouseOutputDetailBarcode model)
        {
            if (model.ParentID > 0)
            {
                var Parent = _WarehouseOutputRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Code;
                model.Date = Parent.Date;
                model.Code = model.Code ?? Parent.Code;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
                model.CategoryDepartmentID = Parent.SupplierID;
                model.CategoryDepartmentName = Parent.SupplierName;
            }
            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.DateInput = model.Date;
            model.DateScan = model.Date;
            if (model.DateScan != null)
            {
                model.Year = model.Year ?? model.DateScan.Value.Year;
                model.Month = model.Month ?? model.DateScan.Value.Month;
                model.Day = model.Day ?? model.DateScan.Value.Day;
                model.Week = model.Week ?? GlobalHelper.GetWeekByDateTime(model.DateScan.Value);
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
            model.MaterialName = Material.Code;
            model.QuantitySNP = Material.QuantitySNP;
            model.Display = Material.Name;
            model.IsSNP = model.IsSNP ?? Material.IsSNP;
            model.FileName = Material.CategoryLineName;
            model.CategoryFamilyID = model.CategoryFamilyID ?? Material.CategoryFamilyID;
            model.CategoryFamilyName = model.CategoryFamilyName ?? Material.CategoryFamilyName;
            model.CategoryCompanyName = Material.OriginalEquipmentManufacturer;
            if (model.WarehouseOutputDetailID > 0)
            {

            }
            else
            {
                var WarehouseOutputDetail = _WarehouseOutputDetailRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialID == model.MaterialID).FirstOrDefault();
                if (WarehouseOutputDetail == null)
                {
                    WarehouseOutputDetail = _WarehouseOutputDetailRepository.GetByCondition(o => o.ParentID == model.ParentID && o.MaterialName == model.MaterialName && o.QuantitySNP == model.QuantitySNP && o.Description == model.Description).FirstOrDefault();
                }
                if (WarehouseOutputDetail == null)
                {
                    WarehouseOutputDetail = new WarehouseOutputDetail();
                    WarehouseOutputDetail.Active = true;
                    WarehouseOutputDetail.ParentID = model.ParentID;
                    WarehouseOutputDetail.ParentName = model.ParentName;
                    WarehouseOutputDetail.Code = model.Code;
                    WarehouseOutputDetail.MaterialID = model.MaterialID;
                    WarehouseOutputDetail.MaterialName = model.MaterialName;
                    WarehouseOutputDetail.Display = model.Display;
                    WarehouseOutputDetail.CategoryUnitID = model.CategoryUnitID;
                    WarehouseOutputDetail.CategoryUnitName = model.CategoryUnitName;
                    WarehouseOutputDetail.CategoryLocationID = model.CategoryLocationID;
                    WarehouseOutputDetail.CategoryLocationName = model.CategoryLocationName;
                    _WarehouseOutputDetailRepository.Add(WarehouseOutputDetail);                    
                }
                model.WarehouseOutputDetailID = WarehouseOutputDetail.ID;
            }
            model.Quantity = model.Quantity ?? GlobalHelper.InitializationNumber;
            model.Description = model.Description ?? model.Barcode;
            model.QuantityInput = model.QuantityInput ?? GlobalHelper.InitializationNumber;
            model.QuantityReturn = model.QuantityReturn ?? GlobalHelper.InitializationNumber;
            model.Quantity = model.Quantity - model.QuantityReturn;


        }
        public override async Task<BaseResult<WarehouseOutputDetailBarcode>> SaveAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode))
                {
                    BaseParameter.BaseModel.Barcode = BaseParameter.BaseModel.Barcode.Trim();
                    InitializationSave(BaseParameter.BaseModel);
                    var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && !string.IsNullOrEmpty(o.Barcode) && !string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode) && o.Barcode.Trim().ToLower() == BaseParameter.BaseModel.Barcode.Trim().ToLower()).FirstOrDefaultAsync();
                    SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                    bool IsSave = true;
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        var Parent = _WarehouseOutputRepository.GetByID(BaseParameter.BaseModel.ParentID.Value);
                        if (Parent.WarehouseRequestID > 0)
                        {
                            var WarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
                            if (WarehouseOutputDetail == null)
                            {
                                WarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialName == BaseParameter.BaseModel.MaterialName).FirstOrDefaultAsync();
                            }
                            if (WarehouseOutputDetail == null)
                            {
                                IsSave = false;
                            }
                        }
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
                            try
                            {
                                BaseParameter.BaseModel = result.BaseModel;
                                await SyncAsync(BaseParameter);
                                result.BaseModel = await _WarehouseOutputDetailBarcodeRepository.GetByIDAsync(result.BaseModel.ID);
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
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SaveHookRackAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode))
                {
                    BaseParameter.BaseModel.Barcode = BaseParameter.BaseModel.Barcode.Trim();
                    InitializationSaveHookRack(BaseParameter.BaseModel);
                    var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && !string.IsNullOrEmpty(o.Barcode) && !string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode) && o.Barcode.Trim().ToLower() == BaseParameter.BaseModel.Barcode.Trim().ToLower()).FirstOrDefaultAsync();
                    SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                    bool IsSave = true;
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
                            try
                            {
                                BaseParameter.BaseModel = result.BaseModel;
                                await SyncHookRackAsync(BaseParameter);
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
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> Save2026Async(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter != null && BaseParameter.BaseModel != null)
            {
                if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode))
                {
                    BaseParameter.BaseModel.Barcode = BaseParameter.BaseModel.Barcode.Trim();
                    InitializationSave(BaseParameter.BaseModel);
                    var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && !string.IsNullOrEmpty(o.Barcode) && !string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode) && o.Barcode.Trim().ToLower() == BaseParameter.BaseModel.Barcode.Trim().ToLower()).FirstOrDefaultAsync();
                    SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                    bool IsSave = true;
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        var Parent = _WarehouseOutputRepository.GetByID(BaseParameter.BaseModel.ParentID.Value);
                        if (Parent.WarehouseRequestID > 0)
                        {
                            var WarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
                            if (WarehouseOutputDetail == null)
                            {
                                WarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialName == BaseParameter.BaseModel.MaterialName).FirstOrDefaultAsync();
                            }
                            if (WarehouseOutputDetail == null)
                            {
                                IsSave = false;
                            }
                        }
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
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            await SyncWarehouseInputDetailBarcodeQuantityAsync(BaseParameter);
            await SyncWarehouseOutputDetailAsync(BaseParameter);
            await SyncWarehouseOutputAsync(BaseParameter);
            await SyncWarehouseOutputDetailBarcodeAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncHookRackAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            await SyncWarehouseInputDetailBarcodeQuantityHoocRackAsync(BaseParameter);
            await SyncWarehouseOutputDetailAsync(BaseParameter);
            await SyncWarehouseOutputDetailBarcodeAsync(BaseParameter);
            return result;
        }
        public override async Task<BaseResult<WarehouseOutputDetailBarcode>> RemoveAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            result.BaseModel = await _WarehouseOutputDetailBarcodeRepository.GetByIDAsync(BaseParameter.ID);
            if (result.BaseModel.ID > 0)
            {
                result.Count = await _WarehouseOutputDetailBarcodeRepository.RemoveAsync(BaseParameter.ID);
                if (result.Count > 0)
                {
                    BaseParameter.BaseModel = result.BaseModel;
                    await SyncAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncRemoveAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            await SyncWarehouseOutputDetailAsync(BaseParameter);
            await SyncWarehouseInputDetailBarcodeAsync(BaseParameter);
            await SyncWarehouseOutputAsync(BaseParameter);
            //await SyncWarehouseInventoryAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncWarehouseOutputAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        var ListWarehouseOutputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID.Value).ToListAsync();
                        if (ListWarehouseOutputDetailBarcode.Count > 0)
                        {
                            var WarehouseOutput = await _WarehouseOutputRepository.GetByIDAsync(BaseParameter.BaseModel.ParentID.Value);
                            if (WarehouseOutput.ID > 0)
                            {
                                WarehouseOutput.Total = ListWarehouseOutputDetailBarcode.Where(o => o.Active == true).Sum(o => o.Total);
                                await _WarehouseOutputRepository.UpdateAsync(WarehouseOutput);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncWarehouseOutputDetailAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.WarehouseOutputDetailID > 0 || BaseParameter.BaseModel.MaterialID > 0 || !string.IsNullOrEmpty(BaseParameter.BaseModel.MaterialName))
                        {
                            var WarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.ID == BaseParameter.BaseModel.WarehouseOutputDetailID).FirstOrDefaultAsync();
                            if (WarehouseOutputDetail == null)
                            {
                                WarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
                            }
                            if (WarehouseOutputDetail == null)
                            {
                                WarehouseOutputDetail = await _WarehouseOutputDetailService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialName == BaseParameter.BaseModel.MaterialName).FirstOrDefaultAsync();
                            }
                            if (WarehouseOutputDetail != null)
                            {
                                if (WarehouseOutputDetail.ID > 0)
                                {
                                    var BaseParameterWarehouseOutputDetail = new BaseParameter<WarehouseOutputDetail>();
                                    BaseParameterWarehouseOutputDetail.BaseModel = WarehouseOutputDetail;
                                    await _WarehouseOutputDetailService.SaveAsync(BaseParameterWarehouseOutputDetail);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncWarehouseInputDetailBarcodeQuantityAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.CategoryDepartmentID > 0)
                            {
                            }
                            else
                            {
                                await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                                var WarehouseOutputDetailBarcode = await GetByCondition(o => o.ID == BaseParameter.BaseModel.ID).FirstOrDefaultAsync();
                                if (WarehouseOutputDetailBarcode != null && WarehouseOutputDetailBarcode.ID > 0)
                                {
                                    BaseParameter.BaseModel.CategoryDepartmentID = WarehouseOutputDetailBarcode.CategoryDepartmentID;
                                }
                            }
                            if (BaseParameter.BaseModel.CategoryDepartmentID > 0)
                            {
                                var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID && o.Barcode == BaseParameter.BaseModel.Barcode && o.Active == true).FirstOrDefaultAsync();
                                if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                                {
                                    var ListWarehouseOutputDetailBarcode = await GetByCondition(o => o.CategoryDepartmentID == WarehouseInputDetailBarcode.CategoryDepartmentID && o.Barcode == WarehouseInputDetailBarcode.Barcode && o.Active == true).ToListAsync();
                                    WarehouseInputDetailBarcode.QuantityOutput = ListWarehouseOutputDetailBarcode.Sum(o => o.Quantity);
                                    WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityOutput;
                                    await _WarehouseInputDetailBarcodeRepository.UpdateAsync(WarehouseInputDetailBarcode);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncWarehouseInputDetailBarcodeQuantityHoocRackAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.ParentID > 0)
                        {
                            if (BaseParameter.BaseModel.CategoryDepartmentID > 0)
                            {
                            }
                            else
                            {
                                await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                                var WarehouseOutputDetailBarcode = await GetByCondition(o => o.ID == BaseParameter.BaseModel.ID).FirstOrDefaultAsync();
                                if (WarehouseOutputDetailBarcode != null && WarehouseOutputDetailBarcode.ID > 0)
                                {
                                    BaseParameter.BaseModel.CategoryDepartmentID = WarehouseOutputDetailBarcode.CategoryDepartmentID;
                                }
                            }
                            if (BaseParameter.BaseModel.CategoryDepartmentID > 0)
                            {
                                var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID && o.Barcode == BaseParameter.BaseModel.Barcode && o.Active == true).FirstOrDefaultAsync();
                                if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                                {
                                    var ListWarehouseOutputDetailBarcode = await GetByCondition(o => o.CategoryDepartmentID == WarehouseInputDetailBarcode.CategoryDepartmentID && o.Barcode == WarehouseInputDetailBarcode.Barcode && o.Active == true).ToListAsync();
                                    WarehouseInputDetailBarcode.QuantityOutput = ListWarehouseOutputDetailBarcode.Sum(o => o.Quantity);
                                    WarehouseInputDetailBarcode.QuantityInventory = WarehouseInputDetailBarcode.Quantity - WarehouseInputDetailBarcode.QuantityOutput;
                                    await _WarehouseInputDetailBarcodeRepository.UpdateAsync(WarehouseInputDetailBarcode);

                                    BaseParameter.BaseModel.BOMID = WarehouseInputDetailBarcode.BOMID;
                                    BaseParameter.BaseModel.ECN = WarehouseInputDetailBarcode.ECN;
                                    BaseParameter.BaseModel.BOMECNVersion = WarehouseInputDetailBarcode.BOMECNVersion;
                                    BaseParameter.BaseModel.BOMDate = WarehouseInputDetailBarcode.BOMDate;
                                    BaseParameter.BaseModel.ListCode = WarehouseInputDetailBarcode.ListCode;
                                    await _WarehouseOutputDetailBarcodeRepository.UpdateAsync(BaseParameter.BaseModel);

                                    var ListWarehouseInputDetailBarcodeMaterial = await _WarehouseInputDetailBarcodeMaterialRepository.GetByCondition(o => o.WarehouseInputDetailBarcodeID == WarehouseInputDetailBarcode.ID).ToListAsync();
                                    if (ListWarehouseInputDetailBarcodeMaterial.Count > 0)
                                    {
                                        var ListWarehouseOutputDetailBarcodeMaterial = new List<WarehouseOutputDetailBarcodeMaterial>();
                                        foreach (var WarehouseInputDetailBarcodeMaterial in ListWarehouseInputDetailBarcodeMaterial)
                                        {
                                            var WarehouseOutputDetailBarcodeMaterial = new WarehouseOutputDetailBarcodeMaterial();
                                            WarehouseOutputDetailBarcodeMaterial.ParentID = BaseParameter.BaseModel.ParentID;
                                            WarehouseOutputDetailBarcodeMaterial.ParentName = BaseParameter.BaseModel.ParentName;
                                            WarehouseOutputDetailBarcodeMaterial.WarehouseOutputDetailBarcodeID = BaseParameter.BaseModel.ID;
                                            WarehouseOutputDetailBarcodeMaterial.Active = WarehouseInputDetailBarcodeMaterial.Active ?? true;
                                            WarehouseOutputDetailBarcodeMaterial.CategoryMaterialID = WarehouseInputDetailBarcodeMaterial.CategoryMaterialID;
                                            WarehouseOutputDetailBarcodeMaterial.CategoryMaterialName = WarehouseInputDetailBarcodeMaterial.CategoryMaterialName;
                                            WarehouseOutputDetailBarcodeMaterial.MaterialID01 = WarehouseInputDetailBarcodeMaterial.MaterialID01;
                                            WarehouseOutputDetailBarcodeMaterial.MaterialName01 = WarehouseInputDetailBarcodeMaterial.MaterialName01;
                                            WarehouseOutputDetailBarcodeMaterial.MaterialID = WarehouseInputDetailBarcodeMaterial.MaterialID;
                                            WarehouseOutputDetailBarcodeMaterial.MaterialName = WarehouseInputDetailBarcodeMaterial.MaterialName;
                                            WarehouseOutputDetailBarcodeMaterial.CategoryUnitID = WarehouseInputDetailBarcodeMaterial.CategoryUnitID;
                                            WarehouseOutputDetailBarcodeMaterial.CategoryUnitName = WarehouseInputDetailBarcodeMaterial.CategoryUnitName;
                                            WarehouseOutputDetailBarcodeMaterial.Quantity = WarehouseInputDetailBarcodeMaterial.Quantity;
                                            WarehouseOutputDetailBarcodeMaterial.BOMID = WarehouseInputDetailBarcodeMaterial.BOMID;
                                            WarehouseOutputDetailBarcodeMaterial.ECN = WarehouseInputDetailBarcodeMaterial.ECN;
                                            WarehouseOutputDetailBarcodeMaterial.BOMECNVersion = WarehouseInputDetailBarcodeMaterial.BOMECNVersion;
                                            WarehouseOutputDetailBarcodeMaterial.BOMDate = WarehouseInputDetailBarcodeMaterial.BOMDate;
                                            ListWarehouseOutputDetailBarcodeMaterial.Add(WarehouseOutputDetailBarcodeMaterial);
                                        }
                                        await _WarehouseOutputDetailBarcodeMaterialRepository.AddRangeAsync(ListWarehouseOutputDetailBarcodeMaterial);
                                        var ListMaterialID = ListWarehouseOutputDetailBarcodeMaterial.Select(o => o.MaterialID).Distinct().ToList();
                                        var ListWarehouseOutputMaterial = new List<WarehouseOutputMaterial>();
                                        foreach (var MaterialID in ListMaterialID)
                                        {
                                            var WarehouseOutputMaterial = new WarehouseOutputMaterial();
                                            WarehouseOutputMaterial.Active = true;
                                            WarehouseOutputMaterial.Quantity = ListWarehouseOutputDetailBarcodeMaterial.Where(o => o.MaterialID == MaterialID).Sum(o => o.Quantity);
                                            WarehouseOutputMaterial.MaterialID = MaterialID;
                                            WarehouseOutputMaterial.MaterialName = ListWarehouseOutputDetailBarcodeMaterial.Where(o => o.MaterialID == MaterialID).Select(o => o.MaterialName).FirstOrDefault();
                                            ListWarehouseOutputMaterial.Add(WarehouseOutputMaterial);
                                        }
                                        await _WarehouseOutputMaterialRepository.AddRangeAsync(ListWarehouseOutputMaterial);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncWarehouseInputDetailBarcodeAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode))
                        {
                            if (BaseParameter.BaseModel.CategoryDepartmentID > 0)
                            {
                            }
                            else
                            {
                                await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                                var WarehouseOutputDetailBarcode = await GetByCondition(o => o.ID == BaseParameter.BaseModel.ID).FirstOrDefaultAsync();
                                if (WarehouseOutputDetailBarcode != null && WarehouseOutputDetailBarcode.ID > 0)
                                {
                                    BaseParameter.BaseModel.CategoryDepartmentID = WarehouseOutputDetailBarcode.CategoryDepartmentID;
                                }
                            }
                            if (BaseParameter.BaseModel.CategoryDepartmentID > 0)
                            {
                                var WarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == BaseParameter.BaseModel.CategoryDepartmentID && o.Barcode == BaseParameter.BaseModel.Barcode && o.Active == true).FirstOrDefaultAsync();
                                if (WarehouseInputDetailBarcode != null && WarehouseInputDetailBarcode.ID > 0)
                                {
                                    var BaseParameterWarehouseInputDetailBarcode = new BaseParameter<WarehouseInputDetailBarcode>();
                                    BaseParameterWarehouseInputDetailBarcode.BaseModel = WarehouseInputDetailBarcode;
                                    await _WarehouseInputDetailBarcodeService.SaveAsync(BaseParameterWarehouseInputDetailBarcode);
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncWarehouseOutputDetailBarcodeAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (!string.IsNullOrEmpty(BaseParameter.BaseModel.Barcode))
                        {
                            if (BaseParameter.BaseModel.Active == true)
                            {
                                var ListWarehouseOutputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.Barcode == BaseParameter.BaseModel.Barcode).ToListAsync();
                                if (ListWarehouseOutputDetailBarcode.Count > 1)
                                {
                                    foreach (var item in ListWarehouseOutputDetailBarcode)
                                    {
                                        if (item.ID != BaseParameter.BaseModel.ID)
                                        {
                                            var BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                                            BaseParameterWarehouseOutputDetailBarcode.ID = item.ID;
                                            await RemoveAsync(BaseParameterWarehouseOutputDetailBarcode);
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
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncWarehouseInventoryAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    BaseParameter<WarehouseInventory> BaseParameterWarehourseInventory = new BaseParameter<WarehouseInventory>();
                    BaseParameterWarehourseInventory.ID = BaseParameter.BaseModel.ID;
                    await _WarehourseInventoryService.SyncByWarehouseOutputDetailBarcodeAsync(BaseParameterWarehourseInventory);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncWarehouseInventoryFromBaseParameterAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    await _WarehourseInventoryService.SyncByWarehouseOutputDetailBarcodeFromBaseParameterAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SyncWarehouseStockAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
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
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByWarehouseOutputDetailIDToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            result.List = await GetByCondition(o => o.WarehouseOutputDetailID == BaseParameter.GeneralID).ToListAsync();
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            result.List = new List<WarehouseOutputDetailBarcode>();
            await MySQLHelper.ERPSyncAsync(GlobalHelper.ERP_MariaDBConectionString);
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                var ListWarehouseOutput = new List<WarehouseOutput>();
                ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.SupplierID == BaseParameter.CategoryDepartmentID).ToListAsync();
                //if (BaseParameter.Year == 0)
                //{
                //    ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.SupplierID == BaseParameter.CategoryDepartmentID).ToListAsync();
                //}
                //else
                //{
                //    if (BaseParameter.Month == 0)
                //    {
                //        ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.SupplierID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Year == BaseParameter.Year).ToListAsync();
                //    }
                //    else
                //    {
                //        ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.SupplierID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Year == BaseParameter.Year && o.Date.Value.Month == BaseParameter.Month).ToListAsync();
                //    }
                //}
                if (ListWarehouseOutput.Count > 0)
                {
                    var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).ToList();
                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                        if (BaseParameter.Year == 0)
                        {
                            result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.Barcode == BaseParameter.SearchString).ToListAsync();
                            if (result.List.Count == 0)
                            {
                                result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.MaterialName == BaseParameter.SearchString).ToListAsync();
                            }
                            if (result.List.Count == 0)
                            {
                                result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.FileName == BaseParameter.SearchString).ToListAsync();
                            }
                            if (result.List.Count == 0)
                            {
                                result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.ParentName == BaseParameter.SearchString).ToListAsync();
                            }
                        }
                        else
                        {
                            if (BaseParameter.Month == 0)
                            {
                                result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.Barcode == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                if (result.List.Count == 0)
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.MaterialName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                }
                                if (result.List.Count == 0)
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.FileName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                }
                                if (result.List.Count == 0)
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.ParentName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                                }
                            }
                            else
                            {
                                result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.Barcode == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                if (result.List.Count == 0)
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.MaterialName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                }
                                if (result.List.Count == 0)
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.FileName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                }
                                if (result.List.Count == 0)
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.ParentName == BaseParameter.SearchString && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
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
                                        result.List = await GetByCondition(o => o.ParentID > 0 && o.Active == true && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.DateScan.Value.Day == BaseParameter.Day).ToListAsync();
                                    }
                                    else
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && o.Active == true && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                }
                                else
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && o.Active == true && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
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
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.UpdateDate != null && o.UpdateDate.Value.Year == BaseParameter.Year && o.UpdateDate.Value.Month == BaseParameter.Month && o.UpdateDate.Value.Day == BaseParameter.Day).ToListAsync();
                                    }
                                    else
                                    {
                                        result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.UpdateDate != null && o.UpdateDate.Value.Year == BaseParameter.Year && o.UpdateDate.Value.Month == BaseParameter.Month).ToListAsync();
                                    }
                                }
                                else
                                {
                                    result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID ?? 0) && o.UpdateDate != null && o.UpdateDate.Value.Year == BaseParameter.Year).ToListAsync();
                                }
                            }
                        }
                    }
                }
            }

            if (result.List.Count > 0)
            {
                if (BaseParameter.Active == true)
                {
                    result.List = result.List.Where(o => o.Active == true).ToList();
                }
                result.List = result.List.OrderByDescending(o => o.DateScan).ThenBy(o => o.ParentName).ThenBy(o => o.MaterialName).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_ActiveToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            result.List = new List<WarehouseOutputDetailBarcode>();

            if (BaseParameter.CategoryDepartmentID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();

                    result.List = await GetByCondition(o => o.ParentID.ToString() == BaseParameter.SearchString).ToListAsync();
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.ID.ToString() == BaseParameter.SearchString).ToListAsync();
                    }
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.ParentName == BaseParameter.SearchString).ToListAsync();
                    }
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Barcode == BaseParameter.SearchString).ToListAsync();
                    }
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.MaterialName == BaseParameter.SearchString).ToListAsync();
                    }
                    if (result.List.Count == 0)
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.FileName == BaseParameter.SearchString).ToListAsync();
                    }
                }
                else
                {
                    if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.DateScan != null && o.DateScan.Value.Date >= BaseParameter.DateBegin.Value.Date && o.DateScan.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                    }
                }
            }
            if (result.List.Count > 0)
            {
                if (BaseParameter.Active == true)
                {
                    result.List = result.List.Where(o => o.Active == true).ToList();
                }
                result.List = result.List.OrderByDescending(o => o.DateScan).ThenBy(o => o.ParentName).ThenBy(o => o.MaterialName).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> AutoSyncAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            //await AutoSync001Async(BaseParameter);
            await MySQLHelper.ERPSyncAsync(GlobalHelper.ERP_MariaDBConectionString);
            await MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                {
                    BaseParameter.DateBegin = new DateTime(BaseParameter.DateBegin.Value.Year, BaseParameter.DateBegin.Value.Month, BaseParameter.DateBegin.Value.Day, 0, 0, 0);
                    BaseParameter.DateEnd = new DateTime(BaseParameter.DateEnd.Value.Year, BaseParameter.DateEnd.Value.Month, BaseParameter.DateEnd.Value.Day, 23, 59, 59);

                    TimeSpan timeDifference = BaseParameter.DateEnd.Value - BaseParameter.DateBegin.Value;
                    DateTime? DateBeginRoot = BaseParameter.DateBegin;
                    for (int i = 0; i <= timeDifference.Days; i++)
                    {
                        DateBeginRoot = DateBeginRoot.Value.AddDays(i);
                        for (int j = 0; j < 24; j++)
                        {
                            DateTime? DateBeginSub = new DateTime(DateBeginRoot.Value.Year, DateBeginRoot.Value.Month, DateBeginRoot.Value.Day, j, 0, 0);
                            DateTime? DateEndSub = new DateTime(DateBeginRoot.Value.Year, DateBeginRoot.Value.Month, DateBeginRoot.Value.Day, j, 59, 59);
                            string DateBegin = DateBeginSub.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            string DateEnd = DateEndSub.Value.ToString("yyyy-MM-dd HH:mm:ss");
                            string sql = @"UPDATE WarehouseOutputDetailBarcode JOIN WarehouseInputDetailBarcode ON WarehouseOutputDetailBarcode.Barcode=WarehouseInputDetailBarcode.Barcode SET WarehouseOutputDetailBarcode.WarehouseInputID=WarehouseInputDetailBarcode.ParentID, WarehouseOutputDetailBarcode.WarehouseInputName=WarehouseInputDetailBarcode.ParentName WHERE WarehouseOutputDetailBarcode.WarehouseInputID IS NULL AND WarehouseOutputDetailBarcode.CategoryDepartmentID=" + BaseParameter.CategoryDepartmentID + " AND WarehouseInputDetailBarcode.CategoryDepartmentID=" + BaseParameter.CategoryDepartmentID + " AND (WarehouseOutputDetailBarcode.DateScan BETWEEN '" + DateBegin + "' AND '" + DateEnd + "')";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);
                        }
                    }
                }
            }

            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> AutoSync001Async(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            string sql = @"SELECT ParentID, Barcode, COUNT(ID) AS SortOrder FROM WarehouseOutputDetailBarcode GROUP BY ParentID, Barcode HAVING COUNT(ID)>1";
            List<WarehouseOutputDetailBarcode> ListWarehouseOutputDetailBarcode = new List<WarehouseOutputDetailBarcode>();
            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.ERP_MariaDBConectionString, sql);
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                ListWarehouseOutputDetailBarcode.AddRange(SQLHelper.ToList<WarehouseOutputDetailBarcode>(dt));
            }
            if (ListWarehouseOutputDetailBarcode.Count > 0)
            {
                foreach (var WarehouseOutputDetailBarcode in ListWarehouseOutputDetailBarcode)
                {
                    var ListWarehouseOutputDetailBarcodeActiveNot = await GetByCondition(o => o.Active != true && o.ParentID == WarehouseOutputDetailBarcode.ParentID && o.Barcode == WarehouseOutputDetailBarcode.Barcode).ToListAsync();
                    if (ListWarehouseOutputDetailBarcodeActiveNot.Count > 0)
                    {
                        await _WarehouseOutputDetailBarcodeRepository.RemoveRangeAsync(ListWarehouseOutputDetailBarcodeActiveNot);
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            result.List = new List<WarehouseOutputDetailBarcode>();
            var CategoryDepartment = await _CategoryDepartmentRepository.GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.Active == true && o.IsSync == true).OrderBy(o => o.SortOrder).FirstOrDefaultAsync();
            if (CategoryDepartment != null && CategoryDepartment.ID > 0)
            {
                Expression<Func<WarehouseOutput, bool>> ConditionWarehouseOutput = o => o.Active == true && o.SupplierID == BaseParameter.CategoryDepartmentID;
                var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(ConditionWarehouseOutput).ToListAsync();
                if ((ListWarehouseOutput != null) && (ListWarehouseOutput.Count > 0))
                {
                    var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).ToList();
                    Expression<Func<WarehouseOutputDetailBarcode, bool>> Condition = o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value);
                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                        Condition = o => (o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value))
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
                                    Condition = o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month && o.DateScan.Value.Day == BaseParameter.Day;
                                }
                                else
                                {
                                    Condition = o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month;
                                }
                            }
                            else
                            {
                                Condition = o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year;
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
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByBarcode_ActiveToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.Barcode))
                {
                    BaseParameter.Barcode = BaseParameter.Barcode.Trim();
                    var WarehouseInput = await _WarehouseInputRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                    if (WarehouseInput.ID > 0)
                    {
                        var ListWarehouseOutput = await _WarehouseOutputRepository.GetByCondition(o => o.Active == true && o.SupplierID == WarehouseInput.CustomerID).ToListAsync();
                        if (ListWarehouseOutput.Count > 0)
                        {
                            var ListWarehouseOutputID = ListWarehouseOutput.Select(o => o.ID).ToList();
                            result.List = await GetByCondition(o => o.ParentID > 0 && ListWarehouseOutputID.Contains(o.ParentID.Value) && o.Barcode == BaseParameter.Barcode && o.Active == BaseParameter.Active).OrderBy(o => o.DateScan).ToListAsync();
                        }
                    }

                }
            }
            return result;
        }
        public override async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByParentIDToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).OrderByDescending(o => o.DateScan).ThenByDescending(o => o.Active).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentID_Active_FIFOToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            result.List = new List<WarehouseOutputDetailBarcode>();
            MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                if (BaseParameter.Year == 0)
                {
                    result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsFIFO == true).ToListAsync();
                }
                else
                {
                    if (BaseParameter.Month == 0)
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsFIFO == true && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year).ToListAsync();
                    }
                    else
                    {
                        result.List = await GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsFIFO == true && o.DateScan != null && o.DateScan.Value.Year == BaseParameter.Year && o.DateScan.Value.Month == BaseParameter.Month).ToListAsync();
                    }
                }
            }
            if (result.List.Count > 0)
            {
                result.List = result.List.OrderByDescending(o => o.DateScan).ThenBy(o => o.ParentName).ThenBy(o => o.MaterialName).ToList();
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SaveList2026Async(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            result.List = new List<WarehouseOutputDetailBarcode>();
            if (BaseParameter.ParentID > 0 && BaseParameter.List != null && BaseParameter.List.Count > 0)
            {


                var ListWarehouseOutputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                foreach (var item in BaseParameter.List)
                {
                    if (item.ParentID > 0)
                    {
                        var BaseParameterWarehouseOutputDetailBarcode = new BaseParameter<WarehouseOutputDetailBarcode>();
                        var WarehouseOutputDetailBarcode = ListWarehouseOutputDetailBarcode.Where(o => o.ParentID == item.ParentID && o.Barcode == item.Barcode).FirstOrDefault();
                        if (WarehouseOutputDetailBarcode != null && WarehouseOutputDetailBarcode.ID > 0)
                        {
                            WarehouseOutputDetailBarcode.Active = true;
                            WarehouseOutputDetailBarcode.IsScan = false;
                            if (WarehouseOutputDetailBarcode.CategoryDepartmentID > 0)
                            {
                                WarehouseOutputDetailBarcode.IsScan = true;
                            }
                            WarehouseOutputDetailBarcode.DateScan = GlobalHelper.InitializationDateTime;
                            WarehouseOutputDetailBarcode.Quantity = item.Quantity;
                            BaseParameterWarehouseOutputDetailBarcode.BaseModel = WarehouseOutputDetailBarcode;
                        }
                        else
                        {
                            item.Active = true;
                            BaseParameterWarehouseOutputDetailBarcode.BaseModel = item;
                        }
                        await Save2026Async(BaseParameterWarehouseOutputDetailBarcode);
                    }
                }
                ListWarehouseOutputDetailBarcode = await GetByCondition(o => o.ParentID == BaseParameter.ParentID).ToListAsync();
                if (ListWarehouseOutputDetailBarcode.Count > 0)
                {
                    var WarehouseOutput = await _WarehouseOutputRepository.GetByIDAsync(BaseParameter.ParentID.Value);
                    if (WarehouseOutput.ID > 0)
                    {
                        WarehouseOutput.Total = ListWarehouseOutputDetailBarcode.Where(o => o.Active == true).Sum(o => o.Total);
                        await _WarehouseOutputRepository.UpdateAsync(WarehouseOutput);
                    }

                    var ListWarehouseOutputDetailBarcodeMaterialID = ListWarehouseOutputDetailBarcode.Select(o => o.MaterialID).Distinct().ToList();
                    var ListWarehouseOutputDetail = await _WarehouseOutputDetailRepository.GetByCondition(o => o.ParentID == BaseParameter.ParentID && ListWarehouseOutputDetailBarcodeMaterialID.Contains(o.MaterialID)).ToListAsync();
                    if (ListWarehouseOutputDetail.Count > 0)
                    {
                        for (int i = 0; i < ListWarehouseOutputDetail.Count; i++)
                        {
                            var ListWarehouseOutputDetailBarcodeSub = ListWarehouseOutputDetailBarcode.Where(o => o.ParentID == BaseParameter.ParentID && o.MaterialID == ListWarehouseOutputDetail[i].MaterialID && o.Active == true).ToList();
                            ListWarehouseOutputDetail[i].QuantityActual = ListWarehouseOutputDetailBarcodeSub.Sum(o => o.Quantity);
                            ListWarehouseOutputDetail[i].Quantity = ListWarehouseOutputDetail[i].Quantity ?? GlobalHelper.InitializationNumber;
                            ListWarehouseOutputDetail[i].QuantityActual = ListWarehouseOutputDetail[i].QuantityActual ?? GlobalHelper.InitializationNumber;
                            ListWarehouseOutputDetail[i].QuantityGAP = ListWarehouseOutputDetail[i].QuantityGAP ?? GlobalHelper.InitializationNumber;
                            ListWarehouseOutputDetail[i].Price = ListWarehouseOutputDetail[i].Price ?? GlobalHelper.InitializationNumber;
                            ListWarehouseOutputDetail[i].Tax = ListWarehouseOutputDetail[i].Tax ?? GlobalHelper.InitializationNumber;
                            ListWarehouseOutputDetail[i].Discount = ListWarehouseOutputDetail[i].Discount ?? GlobalHelper.InitializationNumber;
                            ListWarehouseOutputDetail[i].Total = ListWarehouseOutputDetail[i].Quantity * ListWarehouseOutputDetail[i].Price;
                            ListWarehouseOutputDetail[i].QuantityGAP = ListWarehouseOutputDetail[i].Quantity - ListWarehouseOutputDetail[i].QuantityActual;
                            ListWarehouseOutputDetail[i].TotalTax = ListWarehouseOutputDetail[i].Total * ListWarehouseOutputDetail[i].Tax / 100;
                            ListWarehouseOutputDetail[i].TotalDiscount = ListWarehouseOutputDetail[i].Total * ListWarehouseOutputDetail[i].Discount / 100;
                            ListWarehouseOutputDetail[i].TotalFinal = ListWarehouseOutputDetail[i].Total + ListWarehouseOutputDetail[i].TotalTax - ListWarehouseOutputDetail[i].TotalDiscount;

                        }
                        await _WarehouseOutputDetailRepository.UpdateRangeAsync(ListWarehouseOutputDetail);
                    }

                    var ListWarehouseOutputDetailBarcodeBarcode = BaseParameter.List.Select(o => o.Barcode).ToList();
                    var ListWarehouseOutputDetailBarcode001 = ListWarehouseOutputDetailBarcode.Where(o => ListWarehouseOutputDetailBarcodeBarcode.Contains(o.Barcode)).ToList();
                    var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == WarehouseOutput.SupplierID && o.Active == true && ListWarehouseOutputDetailBarcodeBarcode.Contains(o.Barcode)).ToListAsync();
                    if (ListWarehouseInputDetailBarcode.Count > 0)
                    {
                        for (int i = 0; i < ListWarehouseInputDetailBarcode.Count; i++)
                        {
                            var ListWarehouseOutputDetailBarcodeSub = await _WarehouseOutputDetailBarcodeRepository.GetByCondition(o => o.Active == true && o.CategoryDepartmentID == ListWarehouseInputDetailBarcode[i].CategoryDepartmentID && o.Barcode == ListWarehouseInputDetailBarcode[i].Barcode).ToListAsync();
                            var QuantityOutput = ListWarehouseOutputDetailBarcodeSub.Sum(o => o.Quantity);
                            ListWarehouseInputDetailBarcode[i].QuantityOutput = ListWarehouseOutputDetailBarcodeSub.Sum(o => o.Quantity);
                            ListWarehouseInputDetailBarcode[i].QuantityInventory = ListWarehouseInputDetailBarcode[i].Quantity - ListWarehouseInputDetailBarcode[i].QuantityOutput;
                            ListWarehouseInputDetailBarcode[i].Total = ListWarehouseInputDetailBarcode[i].Price * ListWarehouseInputDetailBarcode[i].QuantityInventory;
                        }
                        await _WarehouseInputDetailBarcodeRepository.UpdateRangeAsync(ListWarehouseInputDetailBarcode);
                    }
                }
            }
            return result;
        }
    }
}

