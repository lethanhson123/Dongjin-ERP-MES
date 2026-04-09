namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderProductionPlanSemiController : BaseController<ProductionOrderProductionPlanSemi, IProductionOrderProductionPlanSemiService>
    {
        private readonly IProductionOrderProductionPlanSemiService _ProductionOrderProductionPlanSemiService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductionOrderProductionPlanSemiController(IProductionOrderProductionPlanSemiService ProductionOrderProductionPlanSemiService, IWebHostEnvironment WebHostEnvironment) : base(ProductionOrderProductionPlanSemiService, WebHostEnvironment)
        {
            _ProductionOrderProductionPlanSemiService = ProductionOrderProductionPlanSemiService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("SyncByQuantityActualAsync")]
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> SyncByQuantityActualAsync()
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderProductionPlanSemi>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderProductionPlanSemiService.SyncByQuantityActualAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("ExportByParentIDToExcelAsync")]
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> ExportByParentIDToExcelAsync()
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderProductionPlanSemi>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderProductionPlanSemiService.ExportByParentIDToExcelAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SortByListAsync")]
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> SortByListAsync()
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderProductionPlanSemi>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderProductionPlanSemiService.SortByListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentIDAndSearchStringToListAsync")]
        public virtual async Task<BaseResult<ProductionOrderProductionPlanSemi>> GetByParentIDAndSearchStringToListAsync()
        {
            var result = new BaseResult<ProductionOrderProductionPlanSemi>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ProductionOrderProductionPlanSemi>>(Request.Form["BaseParameter"]);
                result = await _ProductionOrderProductionPlanSemiService.GetByParentIDAndSearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

