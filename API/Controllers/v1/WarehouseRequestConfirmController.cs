namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseRequestConfirmController : BaseController<WarehouseRequestConfirm, IWarehouseRequestConfirmService>
    {
    private readonly IWarehouseRequestConfirmService _WarehouseRequestConfirmService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public WarehouseRequestConfirmController(IWarehouseRequestConfirmService WarehouseRequestConfirmService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseRequestConfirmService, WebHostEnvironment)
    {
    _WarehouseRequestConfirmService = WarehouseRequestConfirmService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

