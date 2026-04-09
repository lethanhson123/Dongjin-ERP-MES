namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProjectFileController : BaseController<ProjectFile, IProjectFileService>
    {
    private readonly IProjectFileService _ProjectFileService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public ProjectFileController(IProjectFileService ProjectFileService, IWebHostEnvironment WebHostEnvironment) : base(ProjectFileService, WebHostEnvironment)
    {
    _ProjectFileService = ProjectFileService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

