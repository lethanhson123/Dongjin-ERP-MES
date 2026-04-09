namespace Service.Interface
{
    public interface IWarehouseInputDetailBarcodeMaterialService : IBaseService<WarehouseInputDetailBarcodeMaterial>
    {
        Task<BaseResult<WarehouseInputDetailBarcodeMaterial>> GetByWarehouseInputDetailBarcodeIDToListAsync(BaseParameter<WarehouseInputDetailBarcodeMaterial> BaseParameter);
    }
}

