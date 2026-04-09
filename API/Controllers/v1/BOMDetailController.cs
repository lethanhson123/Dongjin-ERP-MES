namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BOMDetailController : BaseController<BOMDetail, IBOMDetailService>
    {
        private readonly IBOMDetailService _BOMDetailService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public BOMDetailController(IBOMDetailService BOMDetailService, IWebHostEnvironment WebHostEnvironment) : base(BOMDetailService, WebHostEnvironment)
        {
            _BOMDetailService = BOMDetailService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync")]
        public virtual async Task<BaseResult<BOMDetail>> GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync()
        {
            var result = new BaseResult<BOMDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOMDetail>>(Request.Form["BaseParameter"]);
                result = await _BOMDetailService.GettrackmtimByCompanyID_PARTNO_ECN_QuantityToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SyncFinishGoodsListOftrackmtimAsync")]
        public virtual async Task<BaseResult<BOMDetail>> SyncFinishGoodsListOftrackmtimAsync()
        {
            var result = new BaseResult<BOMDetail>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<BOMDetail>>(Request.Form["BaseParameter"]);
                result = await _BOMDetailService.SyncFinishGoodsListOftrackmtimAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

