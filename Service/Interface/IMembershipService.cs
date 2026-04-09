namespace Service.Interface
{
    public interface IMembershipService : IBaseService<Membership>
    {
        Task<BaseResult<Membership>> AuthenticationAsync(BaseParameter<Membership> BaseParameter);
        Task<BaseResult<Membership>> CreateAutoAsync(BaseParameter<Membership> BaseParameter);
        Task<BaseResult<Membership>> GetByCategoryDepartmentID_ActiveToListAsync(BaseParameter<Membership> BaseParameter);
        Task<BaseResult<Membership>> GetByCategoryDepartmentID_CategoryPositionID_ActiveToListAsync(BaseParameter<Membership> BaseParameter);
        Task<BaseResult<Membership>> IsPasswordValidWithRegex(BaseParameter<Membership> BaseParameter);
    }
}

