namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryFamilyController : BaseController<CategoryFamily, ICategoryFamilyService>
    {
    private readonly ICategoryFamilyService _CategoryFamilyService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategoryFamilyController(ICategoryFamilyService CategoryFamilyService, IWebHostEnvironment WebHostEnvironment) : base(CategoryFamilyService, WebHostEnvironment)
    {
    _CategoryFamilyService = CategoryFamilyService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

