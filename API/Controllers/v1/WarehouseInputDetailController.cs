namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class WarehouseInputDetailController : BaseController<WarehouseInputDetail, IWarehouseInputDetailService>
    {
        private readonly IWarehouseInputDetailService _WarehouseInputDetailService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public WarehouseInputDetailController(IWarehouseInputDetailService WarehouseInputDetailService, IWebHostEnvironment WebHostEnvironment) : base(WarehouseInputDetailService, WebHostEnvironment)
        {
            _WarehouseInputDetailService = WarehouseInputDetailService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("WarehouseInputDetailCreateAutoAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetail>> WarehouseInputDetailCreateAutoAsync(long ParentID= 153)
        {
            var result = new BaseResult<WarehouseInputDetail>();
            try
            {
                var BaseParameter = new BaseParameter<WarehouseInputDetail>();
                BaseParameter.ParentID = 153;
                result = await _WarehouseInputDetailService.SaveListAndSyncWarehouseInputDetailBarcodeAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByYear_Month_Day_SearchString_InventoryToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetail>> GetByYear_Month_Day_SearchString_InventoryToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetail>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailService.GetByYear_Month_Day_SearchString_InventoryToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetail>> GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetail>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailService.GetByCategoryDepartmentID_Year_Month_Day_SearchString_InventoryToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentIDAndEmpty_SearchStringToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetail>> GetByParentIDAndEmpty_SearchStringToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetail>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailService.GetByParentIDAndEmpty_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SaveListAndSyncWarehouseInputDetailBarcodeAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetail>> SaveListAndSyncWarehouseInputDetailBarcodeAsync()
        {
            var result = new BaseResult<WarehouseInputDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetail>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailService.SaveListAndSyncWarehouseInputDetailBarcodeAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByQuantityGAPToListAsync")]
        public virtual async Task<BaseResult<WarehouseInputDetail>> GetByQuantityGAPToListAsync()
        {
            var result = new BaseResult<WarehouseInputDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<WarehouseInputDetail>>(Request.Form["BaseParameter"]);
                result = await _WarehouseInputDetailService.GetByQuantityGAPToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}

