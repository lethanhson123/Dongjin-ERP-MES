namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BOMCompareController : BaseController<BOMCompare, IBOMCompareService>
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly IBOMCompareService _BOMCompareService;      
        public BOMCompareController(IBOMCompareService BOMCompareService
            , IWebHostEnvironment WebHostEnvironment
           

            ) : base(BOMCompareService, WebHostEnvironment)
        {
            _BOMCompareService = BOMCompareService;
            _WebHostEnvironment = WebHostEnvironment;
          
        }
        [HttpPost]
        [Route("GetCompanyID_YearBegin_YearEndToListAsync")]
        public virtual async Task<BaseResult<BOMCompare>> GetCompanyID_YearBegin_YearEndToListAsync()
        {
            var result = new BaseResult<BOMCompare>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOMCompare>>(Request.Form["BaseParameter"]);
                result = await _BOMCompareService.GetCompanyID_YearBegin_YearEndToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncByCompanyID_YearBegin_YearEndToListAsync")]
        public virtual async Task<BaseResult<BOMCompare>> SyncByCompanyID_YearBegin_YearEndToListAsync()
        {
            var result = new BaseResult<BOMCompare>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOMCompare>>(Request.Form["BaseParameter"]);
                result = await _BOMCompareService.SyncByCompanyID_YearBegin_YearEndToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

