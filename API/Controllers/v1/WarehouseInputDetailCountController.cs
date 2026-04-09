namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseInputDetailCountController : BaseController<WarehouseInputDetailCount, IWarehouseInputDetailCountService>
    {
        private readonly IWarehouseInputDetailCountService _WarehouseInputDetailCountService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseInputDetailCountController(IWarehouseInputDetailCountService WarehouseInputDetailCountService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseInputDetailCountService, WebHostEnvironment)
        {
            _WarehouseInputDetailCountService = WarehouseInputDetailCountService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        
    }
}