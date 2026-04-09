namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ZaloTokenController : BaseController<ZaloToken, IZaloTokenService>
    {
        private readonly IZaloTokenService _ZaloTokenService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ZaloTokenController(IZaloTokenService ZaloTokenService, IWebHostEnvironment WebHostEnvironment) : base(ZaloTokenService, WebHostEnvironment)
        {
            _ZaloTokenService = ZaloTokenService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("SendTemplateAsync")]
        public virtual async Task<BaseResult<ZaloToken>> SendTemplateAsync(long ID)
        {
            var result = new BaseResult<ZaloToken>();
            try
            {
                var BaseParameter = new BaseParameter<ZaloToken>();
                BaseParameter.ID = ID;
                result = await _ZaloTokenService.SendTemplateAsync(BaseParameter);
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

