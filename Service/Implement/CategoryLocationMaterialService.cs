namespace Service.Implement
{
    public class CategoryLocationMaterialService : BaseService<CategoryLocationMaterial, ICategoryLocationMaterialRepository>
    , ICategoryLocationMaterialService
    {
        private readonly ICategoryLocationMaterialRepository _CategoryLocationMaterialRepository;
        private readonly ICategoryLocationRepository _CategoryLocationRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        public CategoryLocationMaterialService(ICategoryLocationMaterialRepository CategoryLocationMaterialRepository
            , ICategoryLocationRepository CategoryLocationRepository
            , IMaterialRepository materialRepository
            , IWarehouseInputRepository WarehouseInputRepository
            , IWarehouseInputDetailBarcodeRepository warehouseInputDetailBarcodeRepository



            ) : base(CategoryLocationMaterialRepository)
        {
            _CategoryLocationMaterialRepository = CategoryLocationMaterialRepository;
            _CategoryLocationRepository = CategoryLocationRepository;
            _MaterialRepository = materialRepository;
            _WarehouseInputRepository = WarehouseInputRepository;
            _WarehouseInputDetailBarcodeRepository = warehouseInputDetailBarcodeRepository;
        }
        public override void Initialization(CategoryLocationMaterial model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Parent = _CategoryLocationRepository.GetByID(model.ParentID.Value);
                model.ParentName = Parent.Name;
                model.CompanyID = Parent.CompanyID;
                model.CompanyName = Parent.CompanyName;
            }
            if (model.MaterialID > 0)
            {
            }
            else
            {
                var Material = _MaterialRepository.GetByDescription(model.MaterialName, model.CompanyID);
                model.MaterialID = Material.ID;
            }
            if (model.MaterialID > 0)
            {
                var Material = _MaterialRepository.GetByID(model.MaterialID.Value);
                model.MaterialName = Material.Code;
                model.Name = Material.Code;
            }
            model.Name = model.Name ?? model.MaterialName;
        }
        public override async Task<BaseResult<CategoryLocationMaterial>> SaveAsync(BaseParameter<CategoryLocationMaterial> BaseParameter)
        {
            var result = new BaseResult<CategoryLocationMaterial>();
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
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
            }
            return result;
        }

        public virtual async Task<BaseResult<CategoryLocationMaterial>> CreateAutoAsync(BaseParameter<CategoryLocationMaterial> BaseParameter)
        {
            var result = new BaseResult<CategoryLocationMaterial>();
            if (BaseParameter.CategoryDepartmentID > 0)
            {
                MySQLHelper.ERPSyncAsync02(GlobalHelper.ERP_MariaDBConectionString);
                var ListWarehouseInputDetailBarcode = await _WarehouseInputDetailBarcodeRepository.GetByCondition(o => o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID && o.Active == true && !string.IsNullOrEmpty(o.CategoryLocationName) && o.QuantityInventory > 0).ToListAsync();
                if (ListWarehouseInputDetailBarcode.Count > 0)
                {
                    var ListCategoryLocation = await _CategoryLocationRepository.GetByCondition(o => o.Active == true && o.CategoryDepartmentID == BaseParameter.CategoryDepartmentID).ToListAsync();
                    foreach (var CategoryLocation in ListCategoryLocation)
                    {
                        string sql = @"delete from CategoryLocationMaterial where ParentID=" + CategoryLocation.ID;
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.ERP_MariaDBConectionString, sql);

                        var ListWarehouseInputDetailBarcodeCategoryLocation = ListWarehouseInputDetailBarcode.Where(o => o.CategoryLocationName == CategoryLocation.Name).ToList();
                        if (ListWarehouseInputDetailBarcodeCategoryLocation.Count > 0)
                        {
                            var ListWarehouseInputDetailBarcodeCategoryLocationMaterialName = ListWarehouseInputDetailBarcodeCategoryLocation.Select(o => o.MaterialName).Distinct().ToList();
                            foreach (var MaterialName in ListWarehouseInputDetailBarcodeCategoryLocationMaterialName)
                            {
                                var CategoryLocationMaterial = await GetByCondition(o => o.ParentName == CategoryLocation.Name && o.MaterialName == MaterialName).FirstOrDefaultAsync();
                                if (CategoryLocationMaterial == null)
                                {
                                    CategoryLocationMaterial = new CategoryLocationMaterial();
                                }
                                CategoryLocationMaterial.ParentID = CategoryLocation.ID;
                                CategoryLocationMaterial.ParentName = CategoryLocation.Name;
                                CategoryLocationMaterial.MaterialName = MaterialName;
                                CategoryLocationMaterial.Count = ListWarehouseInputDetailBarcode.Where(o => o.CategoryLocationName == CategoryLocation.Name && o.MaterialName == MaterialName).ToList().Count;
                                var BaseParameterCategoryLocationMaterial = new BaseParameter<CategoryLocationMaterial>();
                                BaseParameterCategoryLocationMaterial.BaseModel = CategoryLocationMaterial;
                                await SaveAsync(BaseParameterCategoryLocationMaterial);
                            }
                        }
                    }

                    //var ListWarehouseInputDetailBarcodeSub = ListWarehouseInputDetailBarcode.Where(o => !ListWarehouseInputDetailBarcodeID.Contains(o.ID)).ToList();
                    //var ListWarehouseInputDetailBarcodeCategoryLocationMaterialIDSub = ListWarehouseInputDetailBarcodeSub.Select(o => o.MaterialID).Distinct().ToList();
                    //foreach (var MaterialID in ListWarehouseInputDetailBarcodeCategoryLocationMaterialIDSub)
                    //{
                    //    var CategoryLocationMaterial = new CategoryLocationMaterial();
                    //    CategoryLocationMaterial.ParentID = GlobalHelper.CategoryLocationID;
                    //    CategoryLocationMaterial.MaterialID = MaterialID;
                    //    CategoryLocationMaterial.Count = ListWarehouseInputDetailBarcodeSub.Where(o => o.MaterialID == MaterialID).ToList().Count;
                    //    var BaseParameterCategoryLocationMaterial = new BaseParameter<CategoryLocationMaterial>();
                    //    BaseParameterCategoryLocationMaterial.BaseModel = CategoryLocationMaterial;
                    //    await SaveAsync(BaseParameterCategoryLocationMaterial);
                    //}
                }
            }
            return result;
        }
    }
}

