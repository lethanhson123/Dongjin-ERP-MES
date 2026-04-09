namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseOutputFileController : BaseController<WarehouseOutputFile, IWarehouseOutputFileService>
    {
        private readonly IWarehouseOutputFileService _WarehouseOutputFileService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseOutputFileController(IWarehouseOutputFileService WarehouseOutputFileService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseOutputFileService, WebHostEnvironment)
        {
            _WarehouseOutputFileService = WarehouseOutputFileService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

