namespace Service.Interface
{
    public interface IWarehouseInventoryService : IBaseService<WarehouseInventory>
    {
        Task<BaseResult<WarehouseInventory>> SyncByWarehouseInputDetailBarcodeAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> SyncByWarehouseInputDetailBarcodeFromBaseParameterAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInventory>> SyncByWarehouseOutputDetailBarcodeAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> SyncByWarehouseOutputDetailBarcodeFromBaseParameterAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInventory>> SyncByWarehouseAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> GetByActionAndYearAndMonthAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndActionAndYearAndMonthAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndActiveToListAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndSearchStringToListAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndActiveAndActionAndSearchStringToListAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthToListAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> GetByCompanyIDAndYearAndMonthToListAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> CreateAutoAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDAndYearAndMonthAndMaterialSpecialToListAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> GetByCategoryDepartmentIDViaCompanyIDAndYearAndMonthToListAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> SyncAutoAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> SyncByCategoryDepartmentIDAndYearAndMonthAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> SyncInvoiceByCategoryDepartmentIDAndYearAndMonthAsync(BaseParameter<WarehouseInventory> BaseParameter);
        Task<BaseResult<WarehouseInventory>> SyncValueByCategoryDepartmentIDAndYearAndMonthAsync(BaseParameter<WarehouseInventory> BaseParameter);
    }
}

