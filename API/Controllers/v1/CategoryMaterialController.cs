namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryMaterialController : BaseController<CategoryMaterial, ICategoryMaterialService>
    {
    private readonly ICategoryMaterialService _CategoryMaterialService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategoryMaterialController(ICategoryMaterialService CategoryMaterialService, IWebHostEnvironment WebHostEnvironment) : base(CategoryMaterialService, WebHostEnvironment)
    {
    _CategoryMaterialService = CategoryMaterialService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

