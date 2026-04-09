namespace Service.Interface
{
    public interface IInventoryDetailBarcodeService : IBaseService<InventoryDetailBarcode>
    {
        Task<BaseResult<InventoryDetailBarcode>> GetByCategoryDepartmentIDToListAsync(BaseParameter<InventoryDetailBarcode> BaseParameter);
        Task<BaseResult<InventoryDetailBarcode>> ExportToExcelAsync(BaseParameter<InventoryDetailBarcode> BaseParameter);
        Task<BaseResult<InventoryDetailBarcode>> CreateAutoAsync(BaseParameter<InventoryDetailBarcode> BaseParameter);
        Task<BaseResult<InventoryDetailBarcode>> ExportWithCategoryLocationNameToExcelAsync(BaseParameter<InventoryDetailBarcode> BaseParameter);
        Task<BaseResult<InventoryDetailBarcode>> ExportWithQuantityToExcelAsync(BaseParameter<InventoryDetailBarcode> BaseParameter);
        Task<BaseResult<InventoryDetailBarcode>> ExportWithNotExistToExcelAsync(BaseParameter<InventoryDetailBarcode> BaseParameter);
    }
}

