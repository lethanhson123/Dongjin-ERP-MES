namespace Service.Implement
{
    public class WarehouseInputDetailBarcodeMaterialService : BaseService<WarehouseInputDetailBarcodeMaterial, IWarehouseInputDetailBarcodeMaterialRepository>
    , IWarehouseInputDetailBarcodeMaterialService
    {
        private readonly IWarehouseInputDetailBarcodeMaterialRepository _WarehouseInputDetailBarcodeMaterialRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IBOMRepository _BOMRepository;
        private readonly IBOMDetailRepository _BOMDetailRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryMaterialRepository _CategoryMaterialRepository;
        private readonly IWarehouseInputRepository _WarehouseInputRepository;
        private readonly IWarehouseInputDetailBarcodeRepository _WarehouseInputDetailBarcodeRepository;
        private readonly IWarehouseInputMaterialService _WarehouseInputMaterialService;

        public WarehouseInputDetailBarcodeMaterialService(IWarehouseInputDetailBarcodeMaterialRepository WarehouseInputDetailBarcodeMaterialRepository
            , IWebHostEnvironment webHostEnvironment
            , IBOMRepository bomRepository
            , IBOMDetailRepository BOMDetailRepository
            , IMaterialRepository materialRepository
            , ICategoryUnitRepository categoryUnitRepository
            , ICategoryMaterialRepository categoryMaterialRepository
            , IWarehouseInputRepository warehouseInputRepository
            , IWarehouseInputDetailBarcodeRepository warehouseInputDetailBarcodeRepository
            , IWarehouseInputMaterialService WarehouseInputMaterialService

            ) : base(WarehouseInputDetailBarcodeMaterialRepository)
        {
            _WarehouseInputDetailBarcodeMaterialRepository = WarehouseInputDetailBarcodeMaterialRepository;
            _WebHostEnvironment = webHostEnvironment;
            _BOMRepository = bomRepository;
            _BOMDetailRepository = BOMDetailRepository;
            _MaterialRepository = materialRepository;
            _CategoryUnitRepository = categoryUnitRepository;
            _CategoryMaterialRepository = categoryMaterialRepository;
            _WarehouseInputRepository = warehouseInputRepository;
            _WarehouseInputDetailBarcodeRepository = warehouseInputDetailBarcodeRepository;
            _WarehouseInputMaterialService = WarehouseInputMaterialService;
        }
        public override void InitializationSave(WarehouseInputDetailBarcodeMaterial model)
        {
            model.Active = model.Active ?? true;
        }
        public override async Task<BaseResult<WarehouseInputDetailBarcodeMaterial>> SaveAsync(BaseParameter<WarehouseInputDetailBarcodeMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcodeMaterial>();
            if (BaseParameter.BaseModel != null && BaseParameter.BaseModel.ParentID > 0)
            {
                InitializationSave(BaseParameter.BaseModel);
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.WarehouseInputDetailBarcodeID == BaseParameter.BaseModel.WarehouseInputDetailBarcodeID && o.MaterialID01 == BaseParameter.BaseModel.MaterialID01 && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
                SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
                if (result.BaseModel != null && result.BaseModel.ID > 0)
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
        public override async Task<BaseResult<WarehouseInputDetailBarcodeMaterial>> RemoveAsync(BaseParameter<WarehouseInputDetailBarcodeMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcodeMaterial>();
            result.BaseModel = await _WarehouseInputDetailBarcodeMaterialRepository.GetByIDAsync(BaseParameter.ID);
            if (result.BaseModel.ID > 0)
            {
                result.Count = await _WarehouseInputDetailBarcodeMaterialRepository.RemoveAsync(BaseParameter.ID);
                if (result.Count > 0)
                {
                    BaseParameter.BaseModel = result.BaseModel;
                    await SyncAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcodeMaterial>> SyncAsync(BaseParameter<WarehouseInputDetailBarcodeMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcodeMaterial>();
            await SyncWarehouseInputMaterialAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcodeMaterial>> SyncWarehouseInputMaterialAsync(BaseParameter<WarehouseInputDetailBarcodeMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcodeMaterial>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.MaterialID > 0)
                        {
                            var ListWarehouseInputDetailBarcodeMaterial = await GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).ToListAsync();
                            if (ListWarehouseInputDetailBarcodeMaterial.Count > 0)
                            {
                                var WarehouseInputMaterial = new WarehouseInputMaterial();
                                WarehouseInputMaterial.ParentID = ListWarehouseInputDetailBarcodeMaterial[0].ParentID;
                                WarehouseInputMaterial.ParentName = ListWarehouseInputDetailBarcodeMaterial[0].ParentName;
                                WarehouseInputMaterial.MaterialID = ListWarehouseInputDetailBarcodeMaterial[0].MaterialID;
                                WarehouseInputMaterial.MaterialName = ListWarehouseInputDetailBarcodeMaterial[0].MaterialName;
                                //WarehouseInputMaterial.Quantity = ListWarehouseInputDetailBarcodeMaterial.Sum(o => o.Quantity);
                                var BaseParameterWarehouseInputMaterial = new BaseParameter<WarehouseInputMaterial>();
                                BaseParameterWarehouseInputMaterial.BaseModel = WarehouseInputMaterial;
                                await _WarehouseInputMaterialService.SaveAsync(BaseParameterWarehouseInputMaterial);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseInputDetailBarcodeMaterial>> GetByWarehouseInputDetailBarcodeIDToListAsync(BaseParameter<WarehouseInputDetailBarcodeMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputDetailBarcodeMaterial>();
            if (BaseParameter.GeneralID > 0)
            {
                result.List = await GetByCondition(o => o.WarehouseInputDetailBarcodeID == BaseParameter.GeneralID).OrderBy(o => o.MaterialName).ToListAsync();
            }
            return result;
        }
    }
}

