namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReportController : BaseController<Report, IReportService>
    {
        private readonly IReportService _ReportService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ReportController(IReportService ReportService, IWebHostEnvironment WebHostEnvironment) : base(ReportService, WebHostEnvironment)
        {
            _ReportService = ReportService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetWarehouse001_001ToListAsync")]
        public virtual async Task<BaseResult<Report>> GetWarehouse001_001ToListAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.GetWarehouse001_001ToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetWarehouse001_002ToListAsync")]
        public virtual async Task<BaseResult<Report>> GetWarehouse001_002ToListAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.GetWarehouse001_002ToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetWarehouse001_003ToListAsync")]
        public virtual async Task<BaseResult<Report>> GetWarehouse001_003ToListAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.GetWarehouse001_003ToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetWarehouse001_004ToListAsync")]
        public virtual async Task<BaseResult<Report>> GetWarehouse001_004ToListAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.GetWarehouse001_004ToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetWarehouse001_005ToListAsync")]
        public virtual async Task<BaseResult<Report>> GetWarehouse001_005ToListAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.GetWarehouse001_005ToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetWarehouse001_006ToListAsync")]
        public virtual async Task<BaseResult<Report>> GetWarehouse001_006ToListAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.GetWarehouse001_006ToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetWarehouse001_007ToListAsync")]
        public virtual async Task<BaseResult<Report>> GetWarehouse001_007ToListAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.GetWarehouse001_007ToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByProductionTrackingToListAsync")]
        public virtual async Task<BaseResult<Report>> GetByProductionTrackingToListAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.GetByProductionTrackingToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByProductionTracking2026Async")]
        public virtual async Task<BaseResult<Report>> GetByProductionTracking2026Async()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.GetByProductionTracking2026Async(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncByProductionTracking2026Async")]
        public virtual async Task<BaseResult<Report>> SyncByProductionTracking2026Async()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.SyncByProductionTracking2026Async(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByWarehouseStockLongTermAsync")]
        public virtual async Task<BaseResult<Report>> GetByWarehouseStockLongTermAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.GetByWarehouseStockLongTermAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncByWarehouseStockLongTermAsync")]
        public virtual async Task<BaseResult<Report>> SyncByWarehouseStockLongTermAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.SyncByWarehouseStockLongTermAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                result.StatusCode = 500;
                result.Message = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("HookRackGetByCompanyID_Begin_End_SearchStringAsync")]
        public virtual async Task<BaseResult<Report>> HookRackGetByCompanyID_Begin_End_SearchStringAsync()
        {
            var result = new BaseResult<Report>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Report>>(Request.Form["BaseParameter"]);
                result = await _ReportService.HookRackGetByCompanyID_Begin_End_SearchStringAsync(BaseParameter);
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

