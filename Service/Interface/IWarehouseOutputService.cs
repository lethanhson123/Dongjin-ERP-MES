namespace Service.Interface
{
    public interface IWarehouseOutputService : IBaseService<WarehouseOutput>
    {
        Task<BaseResult<WarehouseOutput>> SyncByWarehouseRequestAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetBySupplierID_ActiveToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetBySupplierID_Active_IsCompleteToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetBySupplierID_Active_IsComplete_ActionToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByMembershipIDToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByMembershipID_DateBegin_DateEndToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByMembershipID_Active_SearchStringToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByMembershipID_Active_IsCompleteToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByMembershipID_Active_IsComplete_ActionToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByBarcodeFreedomNoFIFOAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByBarcodeFreedomAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByBarcodeAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByBarcodeNoFIFOAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> PrintAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> PrintGroupAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> PrintGroup2025Async(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> PrintGroup2026Async(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> PrintGroupMobileAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> PrintSumAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> CreateAutoAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByCompanyID_CategoryDepartmentID_Action_SearchStringToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_MembershipToListAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> SyncReInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> SyncOutputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> SyncOutputInputFromMES_C03_trackmtimByTRACK_IDXAndUserNameAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> SyncFromMESByCompanyID_CategoryDepartmentIDAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> SyncFromMES_C03ByCompanyID_CategoryDepartmentID_ID_ActionAsync(BaseParameter<WarehouseOutput> BaseParameter);
        Task<BaseResult<WarehouseOutput>> SyncFromMES_C03ByCompanyID_CategoryDepartmentID_SearchString_ActionAsync(BaseParameter<WarehouseOutput> BaseParameter);
    }
}

