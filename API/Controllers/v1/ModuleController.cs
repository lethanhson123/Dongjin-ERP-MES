namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ModuleController : BaseController<Data.Model.Module, IModuleService>
    {
    private readonly IModuleService _ModuleService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public ModuleController(IModuleService ModuleService, IWebHostEnvironment WebHostEnvironment) : base(ModuleService, WebHostEnvironment)
    {
    _ModuleService = ModuleService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

