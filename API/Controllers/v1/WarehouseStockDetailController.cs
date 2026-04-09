namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseStockDetailController : BaseController<WarehouseStockDetail, IWarehouseStockDetailService>
    {
        private readonly IWarehouseStockDetailService _WarehouseStockDetailService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseStockDetailController(IWarehouseStockDetailService WarehouseStockDetailService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseStockDetailService, WebHostEnvironment)
        {
            _WarehouseStockDetailService = WarehouseStockDetailService;
            _WebHostEnvironment = WebHostEnvironment;
        }       
    }
}

