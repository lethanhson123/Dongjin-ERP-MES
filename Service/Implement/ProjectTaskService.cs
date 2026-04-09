namespace Service.Implement
{
    public class ProjectTaskService : BaseService<ProjectTask, IProjectTaskRepository>
    , IProjectTaskService
    {
        private readonly IProjectTaskRepository _ProjectTaskRepository;
        private readonly ICategoryDepartmentRepository _CategoryDepartmentRepository;
        private readonly ICategorySystemRepository _CategorySystemRepository;
        private readonly IModuleRepository _ModuleRepository;
        private readonly ICategoryTypeRepository _CategoryTypeRepository;
        private readonly ICategoryLevelRepository _CategoryLevelRepository;
        private readonly ICategoryStatusRepository _CategoryStatusRepository;

        public ProjectTaskService(IProjectTaskRepository ProjectTaskRepository
            , ICategoryDepartmentRepository categoryDepartmentRepository
            , ICategorySystemRepository categorySystemRepository
            , IModuleRepository moduleRepository
            , ICategoryTypeRepository categoryTypeRepository
            , ICategoryLevelRepository categoryLevelRepository
            , ICategoryStatusRepository categoryStatusRepository
            ) : base(ProjectTaskRepository)
        {
            _ProjectTaskRepository = ProjectTaskRepository;
            _CategoryDepartmentRepository = categoryDepartmentRepository;
            _CategorySystemRepository = categorySystemRepository;
            _ModuleRepository = moduleRepository;
            _CategoryTypeRepository = categoryTypeRepository;
            _CategoryLevelRepository = categoryLevelRepository;
            _CategoryStatusRepository = categoryStatusRepository;
        }
        public override void Initialization(ProjectTask model)
        {
            BaseInitialization(model);

            if (model.CategoryDepartmentID > 0)
            {
                var CategoryDepartment = _CategoryDepartmentRepository.GetByID(model.CategoryDepartmentID.Value);
                model.CategoryDepartmentName = CategoryDepartment.Name;
            }
            if (model.CategorySystemID > 0)
            {
                var CategorySystem = _CategorySystemRepository.GetByID(model.CategorySystemID.Value);
                model.CategorySystemName = CategorySystem.Code;
            }
            if (model.ModuleID > 0)
            {
                var Module = _ModuleRepository.GetByID(model.ModuleID.Value);
                model.ModuleName = Module.Code;
            }
            if (model.CategoryTypeID > 0)
            {
                var CategoryType = _CategoryTypeRepository.GetByID(model.CategoryTypeID.Value);
                model.CategoryTypeName = CategoryType.Code;
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

