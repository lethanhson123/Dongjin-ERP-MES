namespace Service.Interface
{
    public interface IWarehouseInputDetailService : IBaseService<WarehouseInputDetail>
    {
        Task<BaseResult<WarehouseInputDetail>> GetByYear_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseInputDetail> BaseParameter);
        Task<BaseResult<WarehouseInputDetail>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter<WarehouseInputDetail> BaseParameter);
        Task<BaseResult<WarehouseInputDetail>> GetByParentIDAndEmpty_SearchStringToListAsync(BaseParameter<WarehouseInputDetail> BaseParameter);
        Task<BaseResult<WarehouseInputDetail>> SaveListAndSyncWarehouseInputDetailBarcodeAsync(BaseParameter<WarehouseInputDetail> BaseParameter);
        Task<BaseResult<WarehouseInputDetail>> GetByQuantityGAPToListAsync(BaseParameter<WarehouseInputDetail> BaseParameter);
    }
}

