namespace Service.Interface
{
    public interface IInventoryService : IBaseService<Inventory>
    {
        Task<BaseResult<Inventory>> SyncByIDAsync(BaseParameter<Inventory> BaseParameter);
        Task<BaseResult<Inventory>> GetByCategoryDepartmentIDAsync(BaseParameter<Inventory> BaseParameter);
        Task<BaseResult<Inventory>> GetByCategoryDepartmentIDToListAsync(BaseParameter<Inventory> BaseParameter);
        Task<BaseResult<Inventory>> GetByMembershipIDToListAsync(BaseParameter<Inventory> BaseParameter);
        Task<BaseResult<Inventory>> GetByCompanyID_CategoryDepartmentID_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<Inventory> BaseParameter);
    }
}

