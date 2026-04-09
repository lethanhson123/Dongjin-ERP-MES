namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderSPSTOrderController : BaseController<ProductionOrderSPSTOrder, IProductionOrderSPSTOrderService>
    {
        private readonly IProductionOrderSPSTOrderService _ProductionOrderSPSTOrderService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductionOrderSPSTOrderController(IProductionOrderSPSTOrderService ProductionOrderSPSTOrderService, IWebHostEnvironment WebHostEnvironment) : base(ProductionOrderSPSTOrderService, WebHostEnvironment)
        {
            _ProductionOrderSPSTOrderService = ProductionOrderSPSTOrderService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByParentID_DateToListAsync")]
        public virtual async Task<BaseResult<ProductionOrderSPSTOrder>> GetByParentID_DateToListAsync()
        {
            var result = new BaseResult<ProductionOrderSPSTOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderSPSTOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderSPSTOrderService.GetByParentID_DateToListAsync(BaseParameter);
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
        public virtual async Task<BaseResult<ProductionOrderSPSTOrder>> ExportToExcelAsync()
        {
            var result = new BaseResult<ProductionOrderSPSTOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderSPSTOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderSPSTOrderService.ExportToExcelAsync(BaseParameter);
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
        public virtual async Task<BaseResult<ProductionOrderSPSTOrder>> SyncByParentID_DateToListAsync()
        {
            var result = new BaseResult<ProductionOrderSPSTOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderSPSTOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderSPSTOrderService.SyncByParentID_DateToListAsync(BaseParameter);
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
        public virtual async Task<BaseResult<ProductionOrderSPSTOrder>> SyncMESByParentID_DateToListAsync()
        {
            var result = new BaseResult<ProductionOrderSPSTOrder>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderSPSTOrder>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderSPSTOrderService.SyncMESByParentID_DateToListAsync(BaseParameter);
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