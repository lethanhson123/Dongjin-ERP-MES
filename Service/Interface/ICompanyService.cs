namespace Service.Interface
{
    public interface ICompanyService : IBaseService<Company>
    {
        Task<BaseResult<Company>> GetByMembershipID_ActiveToListAsync(BaseParameter<Company> BaseParameter);
    }
}

