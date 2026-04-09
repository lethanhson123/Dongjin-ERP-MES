namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseOutputDetailBarcodeMaterialController : BaseController<WarehouseOutputDetailBarcodeMaterial, IWarehouseOutputDetailBarcodeMaterialService>
    {
        private readonly IWarehouseOutputDetailBarcodeMaterialService _WarehouseOutputDetailBarcodeMaterialService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseOutputDetailBarcodeMaterialController(IWarehouseOutputDetailBarcodeMaterialService WarehouseOutputDetailBarcodeMaterialService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseOutputDetailBarcodeMaterialService, WebHostEnvironment)
        {
            _WarehouseOutputDetailBarcodeMaterialService = WarehouseOutputDetailBarcodeMaterialService;
            _WebHostEnvironment = WebHostEnvironment;
        }       
        [HttpPost]
        [Route("GetByWarehouseOutputDetailBarcodeIDToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcodeMaterial>> GetByWarehouseOutputDetailBarcodeIDToListAsync()
        {
            var result = new BaseResult<WarehouseOutputDetailBarcodeMaterial>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetailBarcodeMaterial>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputDetailBarcodeMaterialService.GetByWarehouseOutputDetailBarcodeIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}