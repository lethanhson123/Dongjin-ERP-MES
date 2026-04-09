namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MembershipHistoryURLController : BaseController<MembershipHistoryURL, IMembershipHistoryURLService>
    {
        private readonly IMembershipHistoryURLService _MembershipHistoryURLService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembershipHistoryURLController(IMembershipHistoryURLService MembershipHistoryURLService, IWebHostEnvironment WebHostEnvironment) : base(MembershipHistoryURLService, WebHostEnvironment)
        {
            _MembershipHistoryURLService = MembershipHistoryURLService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("SaveAsync")]
        //public virtual async Task<BaseResult<MembershipHistoryURL>> SaveByAllowAnonymousAsync(long UserID, string URL)
        //{
        //    var result = new BaseResult<MembershipHistoryURL>();
        //    try
        //    {
        //        var BaseParameter = new BaseParameter<MembershipHistoryURL>();
        //        BaseParameter.BaseModel = new MembershipHistoryURL();
        //        BaseParameter.BaseModel.Code = "MES";
        //        BaseParameter.BaseModel.ParentID = UserID;
        //        BaseParameter.BaseModel.URL = URL;
        //        result = await _MembershipHistoryURLService.SaveAsync(BaseParameter);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.StatusCode = 500;
        //        result.Message = ex.Message;
        //    }
        //    return result;
        //}

        [HttpPost]
        [Route("GetByParentName_DateToListAsync")]
        public virtual async Task<BaseResult<MembershipHistoryURL>> GetByParentName_DateToListAsync()
        {
            var result = new BaseResult<MembershipHistoryURL>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<MembershipHistoryURL>>(Request.Form["BaseParameter"]);
                result = await _MembershipHistoryURLService.GetByParentName_DateToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByParentName_DateBegin_DateEndToListAsync")]
        public virtual async Task<BaseResult<MembershipHistoryURL>> GetByParentName_DateBegin_DateEndToListAsync()
        {
            var result = new BaseResult<MembershipHistoryURL>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<MembershipHistoryURL>>(Request.Form["BaseParameter"]);
                result = await _MembershipHistoryURLService.GetByParentName_DateBegin_DateEndToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

