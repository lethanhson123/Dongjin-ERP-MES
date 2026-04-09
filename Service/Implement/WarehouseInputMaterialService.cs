namespace Service.Implement
{
    public class WarehouseInputMaterialService : BaseService<WarehouseInputMaterial, IWarehouseInputMaterialRepository>
    , IWarehouseInputMaterialService
    {
        private readonly IWarehouseInputMaterialRepository _WarehouseInputMaterialRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IWarehouseInputDetailBarcodeMaterialRepository _WarehouseInputDetailBarcodeMaterialRepository;

        public WarehouseInputMaterialService(IWarehouseInputMaterialRepository WarehouseInputMaterialRepository
            , IWebHostEnvironment webHostEnvironment
            , IWarehouseInputDetailBarcodeMaterialRepository warehouseInputDetailBarcodeMaterialRepository



            ) : base(WarehouseInputMaterialRepository)
        {
            _WarehouseInputMaterialRepository = WarehouseInputMaterialRepository;
            _WebHostEnvironment = webHostEnvironment;
            _WarehouseInputDetailBarcodeMaterialRepository = warehouseInputDetailBarcodeMaterialRepository;
        }
        public override void Initialization(WarehouseInputMaterial model)
        {
            model.Active = model.Active ?? true;
            var ListWarehouseInputDetailBarcodeMaterial = _WarehouseInputDetailBarcodeMaterialRepository.GetByCondition(o => o.Active == true && o.ParentID == model.ParentID && o.MaterialID == model.MaterialID).ToList();
            model.Quantity = ListWarehouseInputDetailBarcodeMaterial.Sum(o => o.Quantity);
        }
        public override async Task<BaseResult<WarehouseInputMaterial>> SaveAsync(BaseParameter<WarehouseInputMaterial> BaseParameter)
        {
            var result = new BaseResult<WarehouseInputMaterial>();
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

