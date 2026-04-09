namespace Service.Implement
{
    public class WarehouseOutputDetailBarcodeMaterialService : BaseService<WarehouseOutputDetailBarcodeMaterial, IWarehouseOutputDetailBarcodeMaterialRepository>
    , IWarehouseOutputDetailBarcodeMaterialService
    {
        private readonly IWarehouseOutputDetailBarcodeMaterialRepository _WarehouseOutputDetailBarcodeMaterialRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IBOMRepository _BOMRepository;
        private readonly IBOMDetailRepository _BOMDetailRepository;
        private readonly IMaterialRepository _MaterialRepository;
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        private readonly ICategoryMaterialRepository _CategoryMaterialRepository;
        private readonly IWarehouseOutputRepository _WarehouseOutputRepository;
        private readonly IWarehouseOutputDetailBarcodeRepository _WarehouseOutputDetailBarcodeRepository;
        private readonly IWarehouseOutputMaterialService _WarehouseOutputMaterialService;

        public WarehouseOutputDetailBarcodeMaterialService(IWarehouseOutputDetailBarcodeMaterialRepository WarehouseOutputDetailBarcodeMaterialRepository
            , IWebHostEnvironment webHostEnvironment
            , IBOMRepository bomRepository
            , IBOMDetailRepository BOMDetailRepository
            , IMaterialRepository materialRepository
            , ICategoryUnitRepository categoryUnitRepository
            , ICategoryMaterialRepository categoryMaterialRepository
            , IWarehouseOutputRepository WarehouseOutputRepository
            , IWarehouseOutputDetailBarcodeRepository WarehouseOutputDetailBarcodeRepository
            , IWarehouseOutputMaterialService WarehouseOutputMaterialService

            ) : base(WarehouseOutputDetailBarcodeMaterialRepository)
        {
            _WarehouseOutputDetailBarcodeMaterialRepository = WarehouseOutputDetailBarcodeMaterialRepository;
            _WebHostEnvironment = webHostEnvironment;
            _BOMRepository = bomRepository;
            _BOMDetailRepository = BOMDetailRepository;
            _MaterialRepository = materialRepository;
            _CategoryUnitRepository = categoryUnitRepository;
            _CategoryMaterialRepository = categoryMaterialRepository;
            _WarehouseOutputRepository = WarehouseOutputRepository;
            _WarehouseOutputDetailBarcodeRepository = WarehouseOutputDetailBarcodeRepository;
            _WarehouseOutputMaterialService = WarehouseOutputMaterialService;
        }
        public override void InitializationSave(WarehouseOutputDetailBarcodeMaterial model)
        {
            model.Active = model.Active ?? true;
        }
        public override async Task<BaseResult<WarehouseOutputDetailBarcodeMaterial>> SaveAsync(BaseParameter<WarehouseOutputDetailBarcodeMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcodeMaterial>();
            if (BaseParameter.BaseModel != null && BaseParameter.BaseModel.ParentID > 0)
            {
                InitializationSave(BaseParameter.BaseModel);
                var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.WarehouseOutputDetailBarcodeID == BaseParameter.BaseModel.WarehouseOutputDetailBarcodeID && o.MaterialID01 == BaseParameter.BaseModel.MaterialID01 && o.MaterialID == BaseParameter.BaseModel.MaterialID).FirstOrDefaultAsync();
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
        public override async Task<BaseResult<WarehouseOutputDetailBarcodeMaterial>> RemoveAsync(BaseParameter<WarehouseOutputDetailBarcodeMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcodeMaterial>();
            result.BaseModel = await _WarehouseOutputDetailBarcodeMaterialRepository.GetByIDAsync(BaseParameter.ID);
            if (result.BaseModel.ID > 0)
            {
                result.Count = await _WarehouseOutputDetailBarcodeMaterialRepository.RemoveAsync(BaseParameter.ID);
                if (result.Count > 0)
                {
                    BaseParameter.BaseModel = result.BaseModel;
                    await SyncAsync(BaseParameter);
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcodeMaterial>> SyncAsync(BaseParameter<WarehouseOutputDetailBarcodeMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcodeMaterial>();
            await SyncWarehouseOutputMaterialAsync(BaseParameter);
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcodeMaterial>> SyncWarehouseOutputMaterialAsync(BaseParameter<WarehouseOutputDetailBarcodeMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcodeMaterial>();
            if (BaseParameter.BaseModel != null)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    if (BaseParameter.BaseModel.ParentID > 0)
                    {
                        if (BaseParameter.BaseModel.MaterialID > 0)
                        {
                            var ListWarehouseOutputDetailBarcodeMaterial = await GetByCondition(o => o.Active == true && o.ParentID == BaseParameter.BaseModel.ParentID && o.MaterialID == BaseParameter.BaseModel.MaterialID).ToListAsync();
                            if (ListWarehouseOutputDetailBarcodeMaterial.Count > 0)
                            {
                                var WarehouseOutputMaterial = new WarehouseOutputMaterial();
                                WarehouseOutputMaterial.ParentID = ListWarehouseOutputDetailBarcodeMaterial[0].ParentID;
                                WarehouseOutputMaterial.ParentName = ListWarehouseOutputDetailBarcodeMaterial[0].ParentName;
                                WarehouseOutputMaterial.MaterialID = ListWarehouseOutputDetailBarcodeMaterial[0].MaterialID;
                                WarehouseOutputMaterial.MaterialName = ListWarehouseOutputDetailBarcodeMaterial[0].MaterialName;                                
                                var BaseParameterWarehouseOutputMaterial = new BaseParameter<WarehouseOutputMaterial>();
                                BaseParameterWarehouseOutputMaterial.BaseModel = WarehouseOutputMaterial;
                                await _WarehouseOutputMaterialService.SaveAsync(BaseParameterWarehouseOutputMaterial);
                            }
                        }
                    }
                }
            }
            return result;
        }
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcodeMaterial>> GetByWarehouseOutputDetailBarcodeIDToListAsync(BaseParameter<WarehouseOutputDetailBarcodeMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputDetailBarcodeMaterial>();
            if (BaseParameter.GeneralID > 0)
            {
                result.List = await GetByCondition(o => o.WarehouseOutputDetailBarcodeID == BaseParameter.GeneralID).OrderBy(o => o.MaterialName).ToListAsync();
            }
            return result;
        }
    }
}

