namespace Service.Interface
{
    public interface IInventoryDetailService : IBaseService<InventoryDetail>
    {
        Task<BaseResult<InventoryDetail>> SyncByParentIDToListAsync(BaseParameter<InventoryDetail> BaseParameter);
        Task<BaseResult<InventoryDetail>> SyncByParentIDCategoryLocationNameToListAsync(BaseParameter<InventoryDetail> BaseParameter);
        Task<BaseResult<InventoryDetail>> PrintByIDAsync(BaseParameter<InventoryDetail> BaseParameter);
        Task<BaseResult<InventoryDetail>> Print2025ByIDAsync(BaseParameter<InventoryDetail> BaseParameter);
        Task<BaseResult<InventoryDetail>> PrintByListAsync(BaseParameter<InventoryDetail> BaseParameter);
        Task<BaseResult<InventoryDetail>> Print2025ByListAsync(BaseParameter<InventoryDetail> BaseParameter);
        Task<BaseResult<InventoryDetail>> ExportToExcelAsync(BaseParameter<InventoryDetail> BaseParameter);
    }
}

