namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseOutputDetailBarcodeController : BaseController<WarehouseOutputDetailBarcode, IWarehouseOutputDetailBarcodeService>
    {
        private readonly IWarehouseOutputDetailBarcodeService _WarehouseOutputDetailBarcodeService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseOutputDetailBarcodeController(IWarehouseOutputDetailBarcodeService WarehouseOutputDetailBarcodeService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseOutputDetailBarcodeService, WebHostEnvironment)
        {
            _WarehouseOutputDetailBarcodeService = WarehouseOutputDetailBarcodeService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("SaveList2026Async")]
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> SaveList2026Async()
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            result.List = new List<WarehouseOutputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetailBarcode>>(Request.Form["BaseParameter"]);
                await _WarehouseOutputDetailBarcodeService.SaveList2026Async(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByWarehouseOutputDetailIDToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByWarehouseOutputDetailIDToListAsync()
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputDetailBarcodeService.GetByWarehouseOutputDetailIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync()
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputDetailBarcodeService.GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_ActiveToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_ActiveToListAsync()
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputDetailBarcodeService.GetByCategoryDepartmentID_DateBegin_DateEnd_SearchString_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync()
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputDetailBarcodeService.GetByCategoryDepartmentIDViaCompanyID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByBarcode_ActiveToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByBarcode_ActiveToListAsync()
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputDetailBarcodeService.GetByBarcode_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_Active_FIFOToListAsync")]
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> GetByCategoryDepartmentID_Active_FIFOToListAsync()
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputDetailBarcodeService.GetByCategoryDepartmentID_Active_FIFOToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("AutoSyncAsync")]
        public virtual async Task<BaseResult<WarehouseOutputDetailBarcode>> AutoSyncAsync()
        {
            var result = new BaseResult<WarehouseOutputDetailBarcode>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseOutputDetailBarcode>>(Request.Form["BaseParameter"]);
                result = await _WarehouseOutputDetailBarcodeService.AutoSyncAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

