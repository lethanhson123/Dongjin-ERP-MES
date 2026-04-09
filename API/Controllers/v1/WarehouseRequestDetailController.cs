namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseRequestDetailController : BaseController<WarehouseRequestDetail, IWarehouseRequestDetailService>
    {
    private readonly IWarehouseRequestDetailService _WarehouseRequestDetailService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public WarehouseRequestDetailController(IWarehouseRequestDetailService WarehouseRequestDetailService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseRequestDetailService, WebHostEnvironment)
    {
    _WarehouseRequestDetailService = WarehouseRequestDetailService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

