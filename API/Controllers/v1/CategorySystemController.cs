namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategorySystemController : BaseController<CategorySystem, ICategorySystemService>
    {
    private readonly ICategorySystemService _CategorySystemService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategorySystemController(ICategorySystemService CategorySystemService, IWebHostEnvironment WebHostEnvironment) : base(CategorySystemService, WebHostEnvironment)
    {
    _CategorySystemService = CategorySystemService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

