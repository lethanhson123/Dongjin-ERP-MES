namespace Service.Interface
{
    public interface IWarehouseOutputDetailBarcodeMaterialService : IBaseService<WarehouseOutputDetailBarcodeMaterial>
    {
        Task<BaseResult<WarehouseOutputDetailBarcodeMaterial>> GetByWarehouseOutputDetailBarcodeIDToListAsync(BaseParameter<WarehouseOutputDetailBarcodeMaterial> BaseParameter);
    }
}

