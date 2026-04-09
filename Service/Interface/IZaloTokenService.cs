namespace Service.Interface
{
    public interface IZaloTokenService : IBaseService<ZaloToken>
    {
        Task<BaseResult<ZaloToken>> SendTemplateAsync(BaseParameter<ZaloToken> BaseParameter);
        Task<BaseResult<ZaloToken>> GetLatestAsync(BaseParameter<ZaloToken> BaseParameter);
    }
}

