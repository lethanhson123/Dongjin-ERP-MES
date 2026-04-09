namespace Service.Interface
{
    public interface ICategoryDepartmentService : IBaseService<CategoryDepartment>
    {
        Task<BaseResult<CategoryDepartment>> CreateAutoAsync(BaseParameter<CategoryDepartment> BaseParameter);
        Task<BaseResult<CategoryDepartment>> GetByMembershipID_ActiveToListAsync(BaseParameter<CategoryDepartment> BaseParameter);
        Task<BaseResult<CategoryDepartment>> GetByMembershipID_CompanyID_ActiveToListAsync(BaseParameter<CategoryDepartment> BaseParameter);
    }
}

