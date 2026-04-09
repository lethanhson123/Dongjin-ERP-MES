namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryStatusController : BaseController<CategoryStatus, ICategoryStatusService>
    {
    private readonly ICategoryStatusService _CategoryStatusService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategoryStatusController(ICategoryStatusService CategoryStatusService, IWebHostEnvironment WebHostEnvironment) : base(CategoryStatusService, WebHostEnvironment)
    {
    _CategoryStatusService = CategoryStatusService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

