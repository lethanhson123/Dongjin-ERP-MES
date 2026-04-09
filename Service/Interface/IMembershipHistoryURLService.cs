namespace Service.Interface
{
    public interface IMembershipHistoryURLService : IBaseService<MembershipHistoryURL>
    {
        Task<BaseResult<MembershipHistoryURL>> GetByParentName_DateToListAsync(BaseParameter<MembershipHistoryURL> BaseParameter);
        Task<BaseResult<MembershipHistoryURL>> GetByParentName_DateBegin_DateEndToListAsync(BaseParameter<MembershipHistoryURL> BaseParameter);
    }
}

