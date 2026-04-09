namespace Service.Implement
{
    public class ProjectTaskHistoryService : BaseService<ProjectTaskHistory, IProjectTaskHistoryRepository>
    , IProjectTaskHistoryService
    {
        private readonly IProjectTaskHistoryRepository _ProjectTaskHistoryRepository;
        private readonly IProjectTaskRepository _ProjectTaskRepository;
        private readonly IMembershipRepository _MembershipRepository;
        public ProjectTaskHistoryService(IProjectTaskHistoryRepository ProjectTaskHistoryRepository
            , IProjectTaskRepository projectTaskRepository
            , IMembershipRepository membershipRepository
            ) : base(ProjectTaskHistoryRepository)
        {
            _ProjectTaskHistoryRepository = ProjectTaskHistoryRepository;
            _ProjectTaskRepository = projectTaskRepository;
            _MembershipRepository = membershipRepository;
        }
        public override void Initialization(ProjectTaskHistory model)
        {
            BaseInitialization(model);
            if (!string.IsNullOrEmpty(model.MembershipCode))
            {
                var Membership = _MembershipRepository.GetByCondition(o => o.UserName == model.MembershipCode).FirstOrDefault();
                if (Membership != null && Membership.ID > 0)
                {
                    model.MembershipID = Membership.ID;
                    model.MembershipName = Membership.Name;
                }
            }
            if (model.MembershipID > 0)
            {
                var Membership = _MembershipRepository.GetByID(model.MembershipID.Value);
                model.MembershipCode = Membership.UserName;
                model.MembershipName = Membership.Name;
            }
            model.Hour = model.HourEnd - model.HourBegin;
        }
        public override async Task<BaseResult<ProjectTaskHistory>> SaveAsync(BaseParameter<ProjectTaskHistory> BaseParameter)
        {
            var result = new BaseResult<ProjectTaskHistory>();
            Initialization(BaseParameter.BaseModel);
            var ModelCheck = await GetByCondition(o => o.ParentID == BaseParameter.BaseModel.ParentID && o.MembershipID == BaseParameter.BaseModel.MembershipID && o.DateBegin != null && BaseParameter.BaseModel.DateBegin != null && o.DateBegin.Value.Date == BaseParameter.BaseModel.DateBegin.Value.Date).FirstOrDefaultAsync();
            SetModelByModelCheck(BaseParameter.BaseModel, ModelCheck);
            bool IsSave = true;
            if (IsSave == true)
            {
                if (BaseParameter.BaseModel.ID > 0)
                {
                    result = await UpdateAsync(BaseParameter);
                }
                else
                {
                    result = await AddAsync(BaseParameter);
                }
            }
            return result;
        }
    }
}

