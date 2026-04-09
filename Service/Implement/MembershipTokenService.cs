namespace Service.Implement
{
    public class MembershipTokenService : BaseService<MembershipToken, IMembershipTokenRepository>
    , IMembershipTokenService
    {
        private readonly IMembershipTokenRepository _MembershipTokenRepository;
        public MembershipTokenService(IMembershipTokenRepository MembershipTokenRepository) : base(MembershipTokenRepository)
        {
            _MembershipTokenRepository = MembershipTokenRepository;
        }
        public virtual async Task<BaseResult<MembershipToken>> AuthenticationByTokenAsync(BaseParameter<MembershipToken> BaseParameter)
        {
            var result = new BaseResult<MembershipToken>();
            if (!string.IsNullOrEmpty(BaseParameter.Token))
            {
                result.BaseModel = await GetByCondition(item => item.Token == BaseParameter.Token && item.Active == true).FirstOrDefaultAsync();
            }
            return result;
        }
    }
}

