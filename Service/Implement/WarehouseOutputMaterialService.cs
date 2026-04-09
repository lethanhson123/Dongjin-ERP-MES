namespace Service.Implement
{
    public class WarehouseOutputMaterialService : BaseService<WarehouseOutputMaterial, IWarehouseOutputMaterialRepository>
    , IWarehouseOutputMaterialService
    {
        private readonly IWarehouseOutputMaterialRepository _WarehouseOutputMaterialRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IWarehouseOutputDetailBarcodeMaterialRepository _WarehouseOutputDetailBarcodeMaterialRepository;

        public WarehouseOutputMaterialService(IWarehouseOutputMaterialRepository WarehouseOutputMaterialRepository
            , IWebHostEnvironment webHostEnvironment
            , IWarehouseOutputDetailBarcodeMaterialRepository WarehouseOutputDetailBarcodeMaterialRepository



            ) : base(WarehouseOutputMaterialRepository)
        {
            _WarehouseOutputMaterialRepository = WarehouseOutputMaterialRepository;
            _WebHostEnvironment = webHostEnvironment;
            _WarehouseOutputDetailBarcodeMaterialRepository = WarehouseOutputDetailBarcodeMaterialRepository;
        }
        public override void Initialization(WarehouseOutputMaterial model)
        {
            model.Active = model.Active ?? true;
            var ListWarehouseOutputDetailBarcodeMaterial = _WarehouseOutputDetailBarcodeMaterialRepository.GetByCondition(o => o.Active == true && o.ParentID == model.ParentID && o.MaterialID == model.MaterialID).ToList();
            model.Quantity = ListWarehouseOutputDetailBarcodeMaterial.Sum(o => o.Quantity);
        }
        public override async Task<BaseResult<WarehouseOutputMaterial>> SaveAsync(BaseParameter<WarehouseOutputMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseOutputMaterial>();
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
            if (result.BaseModel != null && result.BaseModel.ID > 0)
            {
            }
            return result;
        }
    }
}

