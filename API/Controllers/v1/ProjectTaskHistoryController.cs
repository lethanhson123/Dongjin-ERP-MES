namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProjectTaskHistoryController : BaseController<ProjectTaskHistory, IProjectTaskHistoryService>
    {
    private readonly IProjectTaskHistoryService _ProjectTaskHistoryService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public ProjectTaskHistoryController(IProjectTaskHistoryService ProjectTaskHistoryService, IWebHostEnvironment WebHostEnvironment) : base(ProjectTaskHistoryService, WebHostEnvironment)
    {
    _ProjectTaskHistoryService = ProjectTaskHistoryService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

