namespace Service.Implement
{
    public class ProjectService : BaseService<Project, IProjectRepository>
    , IProjectService
    {
        private readonly IProjectRepository _ProjectRepository;
        private readonly ICompanyRepository _CompanyRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        public ProjectService(IProjectRepository ProjectRepository
            , ICompanyRepository companyRepository
            , ICategoryDepartmentRepository categoryDepartmentRepository

            ) : base(ProjectRepository)
        {
            _ProjectRepository = ProjectRepository;
            _CompanyRepository = companyRepository;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
        }
        public override void Initialization(Project model)
        {
            BaseInitialization(model);

            if (!string.IsNullOrEmpty(model.ParentName))
            {
                var Project = _ProjectRepository.GetByCode(model.ParentName);
                model.ParentID = Project.ID;
            }
            if (model.CompanyID > 0)
            {
                var Company = _CompanyRepository.GetByID(model.CompanyID.Value);
                model.CompanyName = Company.Name;
            }
            if (model.CategoryDepartmentID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.CategoryDepartmentID.Value);
                model.CategoryDepartmentName = CategoryDepartment.Name;
            }
        }
        public override Project SetModelByModelCheck(Project Model, Project ModelCheck)
        {
            if (ModelCheck != null)
            {
                if (ModelCheck.ID > 0)
                {
                    Model.ID = ModelCheck.ID;
                    Model.CreateUserID = ModelCheck.CreateUserID;
                    Model.CreateUserName = ModelCheck.CreateUserName;
                    Model.CreateUserCode = ModelCheck.CreateUserCode;
                    Model.CreateDate = ModelCheck.CreateDate;
                    Model.DateBegin = Model.DateBegin ?? ModelCheck.DateBegin;
                    Model.DateEnd = Model.DateEnd ?? ModelCheck.DateEnd;
                    Model.Active = Model.Active ?? ModelCheck.Active;
                    Model.IsComplete = Model.IsComplete ?? ModelCheck.IsComplete;
                    Model.SortOrder = Model.SortOrder ?? ModelCheck.SortOrder;
                }
            }
            return Model;
        }
        public virtual async Task<BaseResult<Project>> GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<Project> BaseParameter)
        {
            var result = new BaseResult<Project>();
            result.List = new List<Project>();
            if (BaseParameter.CompanyID > 0)
            {
                if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                {
                    BaseParameter.SearchString = BaseParameter.SearchString.Trim();
                    result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && !string.IsNullOrEmpty(o.Code) && o.Code.Contains(BaseParameter.SearchString)).ToListAsync();
                }
                else
                {
                    if (BaseParameter.DateBegin != null && BaseParameter.DateEnd != null)
                    {
                        result.List = await GetByCondition(o => o.CompanyID == BaseParameter.CompanyID && o.DateBegin != null && o.DateBegin.Value.Date >= BaseParameter.DateBegin.Value.Date && o.DateBegin.Value.Date <= BaseParameter.DateEnd.Value.Date).ToListAsync();
                    }
                }
            }
            result.List = result.List.OrderByDescending(o => o.DateBegin).ThenBy(o => o.Code).ToList();
            return result;
        }
    }
}

