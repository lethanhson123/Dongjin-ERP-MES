namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderCuttingOrderController : BaseController<ProductionOrderCuttingOrder, IProductionOrderCuttingOrderService>
    {
        private readonly IProductionOrderCuttingOrderService _ProductionOrderCuttingOrderService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductionOrderCuttingOrderController(IProductionOrderCuttingOrderService ProductionOrderCuttingOrderService, IWebHostEnvironment WebHostEnvironment) : base(ProductionOrderCuttingOrderService, WebHostEnvironment)
        {
            _ProductionOrderCuttingOrderService = ProductionOrderCuttingOrderService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByParentID_DateToListAsync")]
        public virtual async Task<BaseResult<ProductionOrderCuttingOrder>> GetByParentID_DateToListAsync()
        {
            var result = new BaseResult<ProductionOrderCuttingOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderCuttingOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderCuttingOrderService.GetByParentID_DateToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportToExcelAsync")]
        public virtual async Task<BaseResult<ProductionOrderCuttingOrder>> ExportToExcelAsync()
        {
            var result = new BaseResult<ProductionOrderCuttingOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderCuttingOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderCuttingOrderService.ExportToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncByParentID_DateToListAsync")]
        public virtual async Task<BaseResult<ProductionOrderCuttingOrder>> SyncByParentID_DateToListAsync()
        {
            var result = new BaseResult<ProductionOrderCuttingOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderCuttingOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderCuttingOrderService.SyncByParentID_DateToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncMESByParentID_DateToListAsync")]
        public virtual async Task<BaseResult<ProductionOrderCuttingOrder>> SyncMESByParentID_DateToListAsync()
        {
            var result = new BaseResult<ProductionOrderCuttingOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderCuttingOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderCuttingOrderService.SyncMESByParentID_DateToListAsync(BaseParameter);
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