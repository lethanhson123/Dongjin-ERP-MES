namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MembershipTokenController : BaseController<MembershipToken, IMembershipTokenService>
    {
        private readonly IMembershipTokenService _MembershipTokenService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembershipTokenController(IMembershipTokenService MembershipTokenService, IWebHostEnvironment WebHostEnvironment) : base(MembershipTokenService, WebHostEnvironment)
        {
            _MembershipTokenService = MembershipTokenService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("GetAuthenticationByTokenAsync")]
        public virtual async Task<BaseResult<MembershipToken>> GetAuthenticationByTokenAsync(string Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJKV1RTZXJ2aWNlQWNjZXNzVG9rZW4iLCJqdGkiOiJhYTk3ZGRiMC1iOGE2LTRlMTAtYWYxZC0zZWFmY2U1NDI2NjYiLCJpYXQiOiIxLzkvMjAyNiA0OjUxOjUxIFBNIiwiSUQiOiI5IiwiZXhwIjoxNzcwNTY5NTExLCJpc3MiOiJKV1RBdXRoZW50aWNhdGlvblNlcnZlciIsImF1ZCI6IkpXVFNlcnZpY2VQb3N0bWFuQ2xpZW50In0.4OPEtKbXC2BtilvFSFbMQc3tZRLX15o1sG1MuutawOM")
        {
            var result = new BaseResult<MembershipToken>();
            try
            {
                var BaseParameter = new BaseParameter<MembershipToken>();
                BaseParameter.Token = Token;
                result = await _MembershipTokenService.AuthenticationByTokenAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("AuthenticationByTokenAsync")]
        public virtual async Task<BaseResult<MembershipToken>> AuthenticationByTokenAsync()
        {
            var result = new BaseResult<MembershipToken>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<MembershipToken>>(Request.Form["BaseParameter"]);
                result = await _MembershipTokenService.AuthenticationByTokenAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

