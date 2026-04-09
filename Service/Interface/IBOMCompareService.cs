namespace Service.Interface
{
    public interface IBOMCompareService : IBaseService<BOMCompare>
    {
        Task<BaseResult<BOMCompare>> GetCompanyID_YearBegin_YearEndToListAsync(BaseParameter<BOMCompare> BaseParameter);
        Task<BaseResult<BOMCompare>> SyncByCompanyID_YearBegin_YearEndToListAsync(BaseParameter<BOMCompare> BaseParameter);
    }
}

