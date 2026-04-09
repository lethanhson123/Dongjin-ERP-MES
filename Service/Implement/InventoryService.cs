namespace Service.Implement
{
    public class InventoryService : BaseService<Inventory, IInventoryRepository>
    , IInventoryService
    {
        private readonly IInventoryRepository _InventoryRepository;
        private readonly IInventoryDetailBarcodeService _InventoryDetailBarcodeService;
        private readonly IInventoryDetailBarcodeRepository _InventoryDetailBarcodeRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IMembershipDepartmentRepository _MembershipDepartmentRepository;
        public InventoryService(IInventoryRepository InventoryRepository
            , IInventoryDetailBarcodeService InventoryDetailBarcodeService
            , IInventoryDetailBarcodeRepository InventoryDetailBarcodeRepository
            , ICategoryDepartmentRepository CategoryDepartmentRepository
            , IWarehouseInputRepository warehouseInputRepository
            , IWarehouseInputDetailBarcodeRepository warehouseInputDetailBarcodeRepository
            , IMembershipDepartmentRepository MembershipDepartmentRepository

            ) : base(InventoryRepository)
        {
            _InventoryRepository = InventoryRepository;
            _InventoryDetailBarcodeService = InventoryDetailBarcodeService;
            _InventoryDetailBarcodeRepository = InventoryDetailBarcodeRepository;
            _CategoryDepartmentRepository = CategoryDepartmentRepository;
            _WarehouseInputRepository = warehouseInputRepository;
            _WarehouseInputDetailBarcodeRepository = warehouseInputDetailBarcodeRepository;
            _MembershipDepartmentRepository = MembershipDepartmentRepository;
        }
        public override void InitializationSave(Inventory model)
        {
            BaseInitialization(model);

            model.Date = model.Date ?? GlobalHelper.InitializationDateTime;
            model.Code = model.Code ?? model.Date.Value.ToString("yyyyMMddhhmmss");
            if (model.SupplierID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.SupplierID.Value);
                model.SupplierName = CategoryDepartment.Name;
            }

        }
        public override async Task<BaseResult<Inventory>> SaveAsync(BaseParameter<Inventory> BaseParameter)
        {
            var result = new BaseResult<Inventory>();
            InitializationSave(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.Active == true && o.Code == BaseParameter.BaseModel.Code && o.CompanyID == BaseParameter.BaseModel.CompanyID && o.SupplierID == BaseParameter.BaseModel.SupplierID).FirstOrDefaultAsync();
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
                    await SyncAsync(BaseParameter);
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<Inventory>> SyncAsync(BaseParameter<Inventory> BaseParameter)
        {
            var result = new BaseResult<Inventory>();
            await SyncWarehouseInputDetailBarcodeAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<Inventory>> SyncWarehouseInputDetailBarcodeAsync(BaseParameter<Inventory> BaseParameter)
        {
            var result = new BaseResult<Inventory>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.Active == true)
                    {
                        if (BaseParameter.BaseModel.IsSync == true)
                        {
                            if (BaseParameter.BaseModel.IsComplete != true)
                            {
                                if (BaseParameter.BaseModel.CompanyID > 0)
                                {
                                    if (BaseParameter.BaseModel.SupplierID > 0)
                                    {
                                        var ListInventoryDetailBarcodeUpdate = new List<InventoryDetailBarcode>();
                                        var ListInventoryDetailBarcodeAdd = new List<InventoryDetailBarcode>();
                                        var ListInventoryDetailBarcode = await _InventoryDetailBarcodeService.GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ID).ToListAsync();
                                        var ListWarehouseInput = await _WarehouseInputRepository.GetByCondition(o => o.CustomerID == BaseParameter.BaseModel.SupplierID && o.Active == true).OrderBy(o => o.Date).ToListAsync();
                                        if (ListWarehouseInput.Count > 0)
                                        {
                                            var ListWarehouseInputID = ListWarehouseInput.Select(o => o.ID).ToList();
                                            var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Active == true && o.QuantityInventory > 0).ToListAsync();
                                            //var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.Barcode == "9C9083$$4,000$$2522$$74113$$20251216$$01" && o.ParentID > 0 && ListWarehouseInputID.Contains(o.ParentID ?? 0) && o.Active == true && o.QuantityInventory > 0).ToListAsync();
                                            if (ListWarehouseInputDetailBarcode.Count > 0)
                                            {
                                                foreach (var WarehouseInputDetailBarcode in ListWarehouseInputDetailBarcode)
                                                {
                                                    var InventoryDetailBarcode = ListInventoryDetailBarcode.Where(o => o.Barcode == WarehouseInputDetailBarcode.Barcode).FirstOrDefault();
                                                    if (InventoryDetailBarcode == null)
                                                    {
                                                        InventoryDetailBarcode = new InventoryDetailBarcode();
                                                    }
                                                    InventoryDetailBarcode.MESID = WarehouseInputDetailBarcode.ID;
                                                    InventoryDetailBarcode.ParentID = BaseParameter.BaseModel.ID;
                                                    InventoryDetailBarcode.ParentName = BaseParameter.BaseModel.Code;
                                                    InventoryDetailBarcode.CompanyID = BaseParameter.BaseModel.CompanyID;
                                                    InventoryDetailBarcode.CompanyName = BaseParameter.BaseModel.CompanyName;
                                                    InventoryDetailBarcode.Active = false;
                                                    InventoryDetailBarcode.MaterialID = WarehouseInputDetailBarcode.MaterialID;
                                                    InventoryDetailBarcode.MaterialName = WarehouseInputDetailBarcode.MaterialName;
                                                    InventoryDetailBarcode.CategoryLocationName = WarehouseInputDetailBarcode.CategoryLocationName;
                                                    InventoryDetailBarcode.ProductName = WarehouseInputDetailBarcode.CategoryLocationName;
                                                    InventoryDetailBarcode.FileName = WarehouseInputDetailBarcode.FileName;
                                                    InventoryDetailBarcode.Barcode = WarehouseInputDetailBarcode.Barcode;
                                                    InventoryDetailBarcode.Description = WarehouseInputDetailBarcode.Description;
                                                    if (!string.IsNullOrEmpty(InventoryDetailBarcode.Barcode))
                                                    {
                                                        if (InventoryDetailBarcode.Barcode.Contains("$"))
                                                        {
                                                            InventoryDetailBarcode.MaterialName = InventoryDetailBarcode.Barcode.Split('$')[0];
                                                        }
                                                    }

                                                    bool IsSave = true;
                                                    if (InventoryDetailBarcode.ID > 0)
                                                    {
                                                        if (InventoryDetailBarcode.Quantity == WarehouseInputDetailBarcode.QuantityInventory)
                                                        {
                                                            IsSave = false;
                                                        }
                                                    }
                                                    if (IsSave == true)
                                                    {
                                                        InventoryDetailBarcode.Quantity = WarehouseInputDetailBarcode.QuantityInventory;
                                                        if (InventoryDetailBarcode.ID > 0)
                                                        {
                                                            ListInventoryDetailBarcodeUpdate.Add(InventoryDetailBarcode);
                                                            //await _InventoryDetailBarcodeRepository.UpdateAsync(InventoryDetailBarcode);
                                                        }
                                                        else
                                                        {
                                                            ListInventoryDetailBarcodeAdd.Add(InventoryDetailBarcode);
                                                            //await _InventoryDetailBarcodeRepository.AddAsync(InventoryDetailBarcode);
                                                        }
                                                    }
                                                }

                                                await _InventoryDetailBarcodeRepository.UpdateRangeAsync(ListInventoryDetailBarcodeUpdate);
                                                await _InventoryDetailBarcodeRepository.AddRangeAsync(ListInventoryDetailBarcodeAdd);
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
        public virtual async Task<BaseResult<Inventory>> SyncByIDAsync(BaseParameter<Inventory> BaseParameter)
        {
            var result = new BaseResult<Inventory>();
            if (BaseParameter.ID > 0)
            {
                BaseParameter.BaseModel = await _InventoryRepository.GetByIDAsync(BaseParameter.ID);
                await SyncWarehouseInputDetailBarcodeAsync(BaseParameter);
            }
            return result;
        }
        public virtual async Task<BaseResult<Inventory>> GetByCategoryDepartmentIDAsync(BaseParameter<Inventory> BaseParameter)
        {
            var result = new BaseResult<Inventory>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                result.BaseModel = await GetByCondition(o => o.SupplierID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsSync == true && o.IsComplete != true).OrderByDescending(o => o.Date).FirstOrDefaultAsync();
                if (result.BaseModel != null && result.BaseModel.ID > 0)
                {
                    BaseParameter.BaseModel = result.BaseModel;
                    await SaveAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<Inventory>> GetByCategoryDepartmentIDToListAsync(BaseParameter<Inventory> BaseParameter)
        {
            var result = new BaseResult<Inventory>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                result.List = await GetByCondition(o => o.SupplierID == BaseParameter.CategoryDepartmentID && o.Active == true && o.IsSync == true && o.IsComplete != true).OrderByDescending(o => o.Date).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<Inventory>> GetByMembershipIDToListAsync(BaseParameter<Inventory> BaseParameter)
        {
            var result = new BaseResult<Inventory>();
            if (BaseParameter.MembershipID > 0)
            {
                List<long?> ListMembershipCategoryDepartmentID = await _MembershipDepartmentRepository.GetByCondition(o => o.CategoryDepartmentID > 0 && o.ParentID == BaseParameter.MembershipID && o.Active == BaseParameter.Active).Select(o => o.CategoryDepartmentID).ToListAsync();
                if (ListMembershipCategoryDepartmentID.Count > 0)
                {
                    result.List = await GetByCondition(o => o.SupplierID > 0 && ListMembershipCategoryDepartmentID.Contains(o.SupplierID)).ToListAsync();
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<Inventory>> GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<Inventory> BaseParameter)
        {
            var result = new BaseResult<Inventory>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                else
                {
                    if (BaseParameter.CategoryDepartmentID > 0)
                    {
                        if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                        {
                            result.List = await GetByCondition(o => o.SupplierID == BaseParameter.CategoryDepartmentID && o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin && o.Date.Value.Date <= BaseParameter.DateEnd).ToListAsync();
                        }
                    }
                }
            }
            return result;
        }
    }
}

