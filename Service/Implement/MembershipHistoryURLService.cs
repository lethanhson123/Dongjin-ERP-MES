namespace Service.Implement
{
    public class MembershipHistoryURLService : BaseService<MembershipHistoryURL, IMembershipHistoryURLRepository>
    , IMembershipHistoryURLService
    {
        private readonly IMembershipHistoryURLRepository _MembershipHistoryURLRepository;
        private readonly IMembershipRepository _MembershipRepository;
        public MembershipHistoryURLService(IMembershipHistoryURLRepository MembershipHistoryURLRepository, IMembershipRepository membershipRepository) : base(MembershipHistoryURLRepository)
        {
            _MembershipHistoryURLRepository = MembershipHistoryURLRepository;
            _MembershipRepository = membershipRepository;
        }
        public override void Initialization(MembershipHistoryURL model)
        {
            BaseInitialization(model);
            if (model.ParentID > 0)
            {
                var Membership = _MembershipRepository.GetByID(model.ParentID.Value);
                model.ParentName = Membership.UserName;
            }
            model.Date = GlobalHelper.InitializationDateTime;
            model.Code = model.Code ?? "ERP";
            switch (model.Code)
            {
                case "MES":
                    var Membership = _MembershipRepository.GetByCondition(o => o.USER_IDX == model.ParentID).FirstOrDefault();
                    if (Membership != null && Membership.ID > 0)
                    {
                        model.ParentID = Membership.ID;
                    }
                    break;
            }
            model.CreateUserID = model.CreateUserID ?? model.ParentID;
            model.UpdateUserID = model.UpdateUserID ?? model.ParentID;
        }
        public virtual async Task<BaseResult<MembershipHistoryURL>> GetByParentName_DateToListAsync(BaseParameter<MembershipHistoryURL> BaseParameter)
        {
            var result = new BaseResult<MembershipHistoryURL>();
            if (BaseParameter.Date == null)
            {
                BaseParameter.Date = DateTime.Now;
            }
            if (!string.IsNullOrEmpty(BaseParameter.ParentName))
            {

            }
            else
            {
                result.List = await GetByCondition(o => o.Date != null && BaseParameter.Date != null && o.Date.Value.Date == BaseParameter.Date.Value.Date).ToListAsync();
            }
            return result;
        }
        public virtual async Task<BaseResult<MembershipHistoryURL>> GetByParentName_DateBegin_DateEndToListAsync(BaseParameter<MembershipHistoryURL> BaseParameter)
        {
            var result = new BaseResult<MembershipHistoryURL>();
            if (BaseParameter.DateBegin == null)
            {
                BaseParameter.DateBegin = DateTime.Now;
            }
            if (BaseParameter.DateEnd == null)
            {
                BaseParameter.DateEnd = DateTime.Now;
            }
            if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
            {
                if (!string.IsNullOrEmpty(BaseParameter.ParentName))
                {
                    BaseParameter.ParentName = BaseParameter.ParentName.Trim();
                    result.List = await GetByCondition(o => o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date && !string.IsNullOrEmpty(o.ParentName) && o.ParentName.Contains(BaseParameter.ParentName)).ToListAsync();
                }
                else
                {
                    result.List = await GetByCondition(o => o.Date != null && o.Date.Value.Date >= BaseParameter.DateBegin.Value.Date && o.Date.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                }
            }
            return result;
        }
    }
}

