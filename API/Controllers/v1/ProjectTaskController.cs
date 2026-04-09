namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProjectTaskController : BaseController<ProjectTask, IProjectTaskService>
    {
    private readonly IProjectTaskService _ProjectTaskService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public ProjectTaskController(IProjectTaskService ProjectTaskService, IWebHostEnvironment WebHostEnvironment) : base(ProjectTaskService, WebHostEnvironment)
    {
    _ProjectTaskService = ProjectTaskService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

