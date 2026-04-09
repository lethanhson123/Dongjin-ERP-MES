namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseInputDetailBarcodeMaterialController : BaseController<WarehouseInputDetailBarcodeMaterial, IWarehouseInputDetailBarcodeMaterialService>
    {
        private readonly IWarehouseInputDetailBarcodeMaterialService _WarehouseInputDetailBarcodeMaterialService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseInputDetailBarcodeMaterialController(IWarehouseInputDetailBarcodeMaterialService WarehouseInputDetailBarcodeMaterialService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseInputDetailBarcodeMaterialService, WebHostEnvironment)
        {
            _WarehouseInputDetailBarcodeMaterialService = WarehouseInputDetailBarcodeMaterialService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetByWarehouseInputDetailBarcodeIDToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcodeMaterial>> GetByWarehouseInputDetailBarcodeIDToListAsync(long GeneralID = 1841744)
        {
            var result = new BaseResult<WarehouseInputDetailBarcodeMaterial>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInputDetailBarcodeMaterial>();
                BaseParameter.GeneralID = GeneralID;
                result = await _WarehouseInputDetailBarcodeMaterialService.GetByWarehouseInputDetailBarcodeIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByWarehouseInputDetailBarcodeIDToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetailBarcodeMaterial>> GetByWarehouseInputDetailBarcodeIDToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetailBarcodeMaterial>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetailBarcodeMaterial>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailBarcodeMaterialService.GetByWarehouseInputDetailBarcodeIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}