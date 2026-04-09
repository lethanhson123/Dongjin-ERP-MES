namespace Service.Interface
{
    public interface IMembershipTokenService : IBaseService<MembershipToken>
    {
        Task<BaseResult<MembershipToken>> AuthenticationByTokenAsync(BaseParameter<MembershipToken> BaseParameter);
    }
}

