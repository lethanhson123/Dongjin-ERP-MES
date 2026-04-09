namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CategoryVehicleController : BaseController<CategoryVehicle, ICategoryVehicleService>
    {
    private readonly ICategoryVehicleService _CategoryVehicleService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public CategoryVehicleController(ICategoryVehicleService CategoryVehicleService, IWebHostEnvironment WebHostEnvironment) : base(CategoryVehicleService, WebHostEnvironment)
    {
    _CategoryVehicleService = CategoryVehicleService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

