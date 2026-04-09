namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseInputMaterialController : BaseController<WarehouseInputMaterial, IWarehouseInputMaterialService>
    {
        private readonly IWarehouseInputMaterialService _WarehouseInputMaterialService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseInputMaterialController(IWarehouseInputMaterialService WarehouseInputMaterialService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseInputMaterialService, WebHostEnvironment)
        {
            _WarehouseInputMaterialService = WarehouseInputMaterialService;
            _WebHostEnvironment = WebHostEnvironment;
        }   
    }
}