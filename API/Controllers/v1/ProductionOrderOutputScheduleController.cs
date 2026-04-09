namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderOutputScheduleController : BaseController<ProductionOrderOutputSchedule, IProductionOrderOutputScheduleService>
    {
    private readonly IProductionOrderOutputScheduleService _CategoryCustomerService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public ProductionOrderOutputScheduleController(IProductionOrderOutputScheduleService CategoryCustomerService, IWebHostEnvironment WebHostEnvironment) : base(CategoryCustomerService, WebHostEnvironment)
    {
    _CategoryCustomerService = CategoryCustomerService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

