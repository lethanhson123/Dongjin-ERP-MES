namespace Service.Interface
{
    public interface IWarehouseRequestService : IBaseService<WarehouseRequest>
    {
        Task<BaseResult<WarehouseRequest>> GetByConfirmToListAsync(BaseParameter<WarehouseRequest> BaseParameter);
        Task<BaseResult<WarehouseRequest>> GetByMembershipIDToListAsync(BaseParameter<WarehouseRequest> BaseParameter);
        Task<BaseResult<WarehouseRequest>> GetByMembershipID_DateBegin_DateEndToListAsync(BaseParameter<WarehouseRequest> BaseParameter);
        Task<BaseResult<WarehouseRequest>> GetByCompanyID_CategoryDepartmentID_Action_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<WarehouseRequest> BaseParameter);
        Task<BaseResult<WarehouseRequest>> GetByMembershipID_ConfirmToListAsync(BaseParameter<WarehouseRequest> BaseParameter);
        Task<BaseResult<WarehouseRequest>> GetByCategoryDepartmentID_SearchStringToListAsync(BaseParameter<WarehouseRequest> BaseParameter);
        Task<BaseResult<WarehouseRequest>> ExportToExcelAsync(BaseParameter<WarehouseRequest> BaseParameter);
    }
}

