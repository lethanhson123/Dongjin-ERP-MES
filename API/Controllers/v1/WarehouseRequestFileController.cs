namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseRequestFileController : BaseController<WarehouseRequestFile, IWarehouseRequestFileService>
    {
        private readonly IWarehouseRequestFileService _WarehouseRequestFileService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseRequestFileController(IWarehouseRequestFileService WarehouseRequestFileService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseRequestFileService, WebHostEnvironment)
        {
            _WarehouseRequestFileService = WarehouseRequestFileService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

