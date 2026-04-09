namespace Service.Interface
{
    public interface IWarehouseOutputDetailBarcodeService : IBaseService<WarehouseOutputDetailBarcode>
    {
        Task<BaseResult<WarehouseOutputDetailBarcode>> SaveList2026Async(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseOutputDetailBarcode>> Save2026Async(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseOutputDetailBarcode>> SaveHookRackAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseOutputDetailBarcode>> GetByWarehouseOutputDetailIDToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_ActiveToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseOutputDetailBarcode>> GetByBarcode_ActiveToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentID_Active_FIFOToListAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
        Task<BaseResult<WarehouseOutputDetailBarcode>> AutoSyncAsync(BaseParameter<WarehouseOutputDetailBarcode> BaseParameter);
       
    }
}

