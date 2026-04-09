namespace Service.Interface
{
    public interface IWarehouseInputService : IBaseService<WarehouseInput>
    {

        Task<BaseResult<WarehouseInput>> GetByCustomerID_Active_IsCompleteToListAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> GetByCustomerID_Active_IsComplete_ActionToListAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> GetByMembershipIDToListAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> GetByMembershipID_Active_IsCompleteToListAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> GetByMembershipID_Active_IsComplete_ActionToListAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> GetByBarcodeAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> CreateAutoAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> SyncInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> SyncFromMES_C03ByCompanyID_Action_IDAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> SyncFromMESByCompanyID_CategoryDepartmentIDAsync(BaseParameter<WarehouseInput> BaseParameter);
        Task<BaseResult<WarehouseInput>> SaveHookRackAsync(BaseParameter<WarehouseInput> BaseParameter);
    }
}

