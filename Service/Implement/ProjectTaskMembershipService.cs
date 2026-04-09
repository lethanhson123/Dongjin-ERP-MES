namespace Service.Implement
{
    public class ProjectTaskMembershipService : BaseService<ProjectTaskMembership, IProjectTaskMembershipRepository>
    , IProjectTaskMembershipService
    {
        private readonly IProjectTaskMembershipRepository _ProjectTaskMembershipRepository;
        private readonly IProjectTaskRepository _ProjectTaskRepository;
        private readonly ICategoryLevelRepository _CategoryLevelRepository;
        private readonly ICategoryStatusRepository _CategoryStatusRepository;
        private readonly IMembershipRepository _MembershipRepository;
        public ProjectTaskMembershipService(IProjectTaskMembershipRepository ProjectTaskMembershipRepository
            , IProjectTaskRepository projectTaskRepository
            , ICategoryLevelRepository categoryLevelRepository
            , ICategoryStatusRepository categoryStatusRepository
            , IMembershipRepository membershipRepository

            ) : base(ProjectTaskMembershipRepository)
        {
            _ProjectTaskMembershipRepository = ProjectTaskMembershipRepository;
            _ProjectTaskRepository = projectTaskRepository;
            _CategoryLevelRepository = categoryLevelRepository;
            _CategoryStatusRepository = categoryStatusRepository;
            _MembershipRepository = membershipRepository;

        }
        public override void Initialization(ProjectTaskMembership model)
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
            if (model.ProjectTaskID > 0)
            {
                var ProjectTask = _ProjectTaskRepository.GetByID(model.ProjectTaskID.Value);
                model.ProjectTaskName = ProjectTask.Code;
            }
            if (model.CategoryLevelID > 0)
            {
                var CategoryLevel = _CategoryLevelRepository.GetByID(model.CategoryLevelID.Value);
                model.CategoryLevelName = CategoryLevel.Code;
            }
            if (model.CategoryStatusID > 0)
            {
                var CategoryStatus = _CategoryStatusRepository.GetByID(model.CategoryStatusID.Value);
                model.CategoryStatusName = CategoryStatus.Code;
            }
        }
    }
}

