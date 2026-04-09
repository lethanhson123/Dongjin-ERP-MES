namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseOutputMaterialController : BaseController<WarehouseOutputMaterial, IWarehouseOutputMaterialService>
    {
        private readonly IWarehouseOutputMaterialService _WarehouseOutputMaterialService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseOutputMaterialController(IWarehouseOutputMaterialService WarehouseOutputMaterialService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseOutputMaterialService, WebHostEnvironment)
        {
            _WarehouseOutputMaterialService = WarehouseOutputMaterialService;
            _WebHostEnvironment = WebHostEnvironment;
        }   
    }
}