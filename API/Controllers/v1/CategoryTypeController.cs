namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryTypeController : BaseController<CategoryType, ICategoryTypeService>
    {
    private readonly ICategoryTypeService _CategoryTypeService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategoryTypeController(ICategoryTypeService CategoryTypeService, IWebHostEnvironment WebHostEnvironment) : base(CategoryTypeService, WebHostEnvironment)
    {
    _CategoryTypeService = CategoryTypeService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

