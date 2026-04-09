namespace API.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("2.0")]
    public class trackmtimController : BaseController<BOM, ItrackmtimService>
    {        
        private readonly IWebHostEnvironment _WebHostEnvironment;
        private readonly ItrackmtimService _trackmtimService;

        public trackmtimController(ItrackmtimService trackmtimService
            , IWebHostEnvironment WebHostEnvironment

            ) : base(trackmtimService, WebHostEnvironment)
        {
            _trackmtimService = trackmtimService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByCompanyID_LeadNo_FinishGoodsToListAsync")]
        public virtual async Task<BaseResult<trackmtim>> GetByCompanyID_LeadNo_FinishGoodsToListAsync()
        {
            var result = new BaseResult<trackmtim>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<trackmtim>>(Request.Form["BaseParameter"]);
                result = await _trackmtimService.GetByCompanyID_LeadNo_FinishGoodsToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SaveByListID_PO_FinishGoods_ECNAsync")]
        public virtual async Task<BaseResult<trackmtim>> SaveByListID_PO_FinishGoods_ECNAsync()
        {
            var result = new BaseResult<trackmtim>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<trackmtim>>(Request.Form["BaseParameter"]);
                result = await _trackmtimService.SaveByListID_PO_FinishGoods_ECNAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCompanyID_LEADNM_Begin_EndToListAsync")]
        public virtual async Task<BaseResult<trackmtim>> GetByCompanyID_LEADNM_Begin_EndToListAsync()
        {
            var result = new BaseResult<trackmtim>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<trackmtim>>(Request.Form["BaseParameter"]);
                result = await _trackmtimService.GetByCompanyID_LEADNM_Begin_EndToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("SaveAsync")]
        public virtual async Task<BaseResult<trackmtim>> SaveAsync()
        {
            var result = new BaseResult<trackmtim>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<trackmtim>>(Request.Form["BaseParameter"]);
                result = await _trackmtimService.SaveAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

