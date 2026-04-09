namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReportDetailController : BaseController<ReportDetail, IReportDetailService>
    {
        private readonly IReportDetailService _ReportDetailService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ReportDetailController(IReportDetailService ReportDetailService, IWebHostEnvironment WebHostEnvironment) : base(ReportDetailService, WebHostEnvironment)
        {
            _ReportDetailService = ReportDetailService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetProductionTracking2026ByParentIDToListAsync")]
        public virtual async Task<BaseResult<ReportDetail>> GetProductionTracking2026ByParentIDToListAsync()
        {
            var result = new BaseResult<ReportDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ReportDetail>>(Request.Form["BaseParameter"]);
                result = await _ReportDetailService.GetProductionTracking2026ByParentIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetWarehouseStockLongTermByParentIDToListAsync")]
        public virtual async Task<BaseResult<ReportDetail>> GetWarehouseStockLongTermByParentIDToListAsync()
        {
            var result = new BaseResult<ReportDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ReportDetail>>(Request.Form["BaseParameter"]);
                result = await _ReportDetailService.GetWarehouseStockLongTermByParentIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetWarehouseStockLongTerm1000ByParentIDToListAsync")]
        public virtual async Task<BaseResult<ReportDetail>> GetWarehouseStockLongTerm1000ByParentIDToListAsync()
        {
            var result = new BaseResult<ReportDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ReportDetail>>(Request.Form["BaseParameter"]);
                result = await _ReportDetailService.GetWarehouseStockLongTerm1000ByParentIDToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("HookRackByParentIDExportToExcelAsync")]
        public virtual async Task<BaseResult<ReportDetail>> HookRackByParentIDExportToExcelAsync()
        {
            var result = new BaseResult<ReportDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<ReportDetail>>(Request.Form["BaseParameter"]);
                result = await _ReportDetailService.HookRackByParentIDExportToExcelAsync(BaseParameter);
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

