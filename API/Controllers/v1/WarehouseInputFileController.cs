namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseInputFileController : BaseController<WarehouseInputFile, IWarehouseInputFileService>
    {
        private readonly IWarehouseInputFileService _WarehouseInputFileService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseInputFileController(IWarehouseInputFileService WarehouseInputFileService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseInputFileService, WebHostEnvironment)
        {
            _WarehouseInputFileService = WarehouseInputFileService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

