namespace Service.Implement
{
    public class ProjectFileService : BaseService<ProjectFile, IProjectFileRepository>
    , IProjectFileService
    {
        private readonly IProjectFileRepository _ProjectFileRepository;
        private readonly IProjectTaskRepository _ProjectTaskRepository;
        public ProjectFileService(IProjectFileRepository ProjectFileRepository
            , IProjectTaskRepository projectTaskRepository
            
            ) : base(ProjectFileRepository)
        {
            _ProjectFileRepository = ProjectFileRepository;
            _ProjectTaskRepository = projectTaskRepository;
        }
        public override void Initialization(ProjectFile model)
        {
            BaseInitialization(model);
           
            if (model.ProjectTaskID > 0)
            {
                var ProjectTask = _ProjectTaskRepository.GetByID(model.ProjectTaskID.Value);
                model.ProjectTaskName = ProjectTask.Code;
            }            
        }
    }
}

