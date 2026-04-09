namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryPositionController : BaseController<CategoryPosition, ICategoryPositionService>
    {
    private readonly ICategoryPositionService _CategoryPositionService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategoryPositionController(ICategoryPositionService CategoryPositionService, IWebHostEnvironment WebHostEnvironment) : base(CategoryPositionService, WebHostEnvironment)
    {
    _CategoryPositionService = CategoryPositionService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

