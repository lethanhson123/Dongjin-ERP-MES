namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryLevelController : BaseController<CategoryLevel, ICategoryLevelService>
    {
    private readonly ICategoryLevelService _CategoryLevelService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategoryLevelController(ICategoryLevelService CategoryLevelService, IWebHostEnvironment WebHostEnvironment) : base(CategoryLevelService, WebHostEnvironment)
    {
    _CategoryLevelService = CategoryLevelService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

