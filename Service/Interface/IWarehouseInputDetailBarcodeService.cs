namespace Service.Interface
{
    public interface IWarehouseInputDetailBarcodeService : IBaseService<WarehouseInputDetailBarcode>
    {
        Task<BaseResult<WarehouseInputDetailBarcode>> SaveHookRackAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByWarehouseInputDetailIDToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByParentID_MaterialIDToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_FIFO_StockToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_BarcodeAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeFromtmbrcdAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeFromtdpdmtimAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByBarcodeFromtrackmtimAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByMaterialID_CategoryLocationIDFromDiagramToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_MaterialID_CategoryLocationIDFromDiagramToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByCategoryDepartmentID_MaterialID_CategoryLocationNameFromDiagramToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByParentIDAndEmpty_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> PrintAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> PrintByBarcodeAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> PrintByParentIDAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> PrintByListWarehouseInputDetailID2025Async(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> PrintByListWarehouseInputDetailIDAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> PrintByWarehouseInputDetailIDAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> PrintBarcode_WarehouseOutputIDAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> PrintByListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> CreateAutoAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetCompareMESAndERPToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetPARTNOCompareMESAndERPToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> SyncByParrentIDAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByHOOKRACK_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByKOMAX_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetBySHIELDWIRE_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetByLP_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> GetBySPST_SearchStringToListAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> ExportToExcelAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> SyncStockAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseInputDetailBarcode>> AutoSyncAsync(BaseParameter<WarehouseInputDetailBarcode> BaseParameter);
        
    }
}

