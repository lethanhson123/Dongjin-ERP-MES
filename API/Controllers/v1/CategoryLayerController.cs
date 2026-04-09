namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryLayerController : BaseController<CategoryLayer, ICategoryLayerService>
    {
    private readonly ICategoryLayerService _CategoryLayerService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategoryLayerController(ICategoryLayerService CategoryLayerService, IWebHostEnvironment WebHostEnvironment) : base(CategoryLayerService, WebHostEnvironment)
    {
    _CategoryLayerService = CategoryLayerService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

