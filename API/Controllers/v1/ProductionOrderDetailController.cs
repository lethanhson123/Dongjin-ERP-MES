namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderDetailController : BaseController<ProductionOrderDetail, IProductionOrderDetailService>
    {
        private readonly IProductionOrderDetailService _ProductionOrderDetailService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductionOrderDetailController(IProductionOrderDetailService ProductionOrderDetailService, IWebHostEnvironment WebHostEnvironment) : base(ProductionOrderDetailService, WebHostEnvironment)
        {
            _ProductionOrderDetailService = ProductionOrderDetailService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByParentIDAndSearchStringToListAsync")]
        public virtual async Task<BaseResult<ProductionOrderDetail>> GetByParentIDAndSearchStringToListAsync()
        {
            var result = new BaseResult<ProductionOrderDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderDetail>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderDetailService.GetByParentIDAndSearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

