namespace Repository.Interface
{
    public interface ICategoryDepartmentRepository : IBaseRepository<CategoryDepartment>
    {
        Task<List<CategoryDepartment>> GetByMembershipID_CompanyID_ActiveToListAsync(long? MembershipID, long? CompanyID, bool? Active);
        Task<List<CategoryDepartment>> GetByMembershipID_ActiveToListAsync(long? MembershipID, long? CompanyID, bool? Active);
    }
}

