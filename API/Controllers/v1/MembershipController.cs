using Service.Model;

namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MembershipController : BaseController<Membership, IMembershipService>
    {
        private readonly IMembershipService _MembershipService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembershipController(IMembershipService MembershipService, IWebHostEnvironment WebHostEnvironment) : base(MembershipService, WebHostEnvironment)
        {
            _MembershipService = MembershipService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("GetAuthenticationAsync")]
        //public virtual async Task<BaseResult<Membership>> GetAuthenticationAsync(string UserName, string Password)
        //{
        //    var result = new BaseResult<Membership>();
        //    try
        //    {
        //        var BaseParameter = new BaseParameter<Membership>();
        //        BaseParameter.UserName = UserName;
        //        BaseParameter.Password = Password;
        //        result = await _MembershipService.AuthenticationAsync(BaseParameter);
        //    }
        //    catch (Exception ex)
        //    {
        //        string mes = ex.Message;
        //    }
        //    return result;
        //}
        [AllowAnonymous]
        [HttpPost]
        [Route("AuthenticationAsync")]
        public virtual async Task<BaseResult<Membership>> AuthenticationAsync()
        {
            var result = new BaseResult<Membership>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Membership>>(Request.Form["BaseParameter"]);
                result = await _MembershipService.AuthenticationAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("CreateAutoAsync")]
        public virtual async Task<BaseResult<Membership>> CreateAutoAsync()
        {
            var result = new BaseResult<Membership>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Membership>>(Request.Form["BaseParameter"]);
                result = await _MembershipService.CreateAutoAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_ActiveToListAsync")]
        public virtual async Task<BaseResult<Membership>> GetByCategoryDepartmentID_ActiveToListAsync()
        {
            var result = new BaseResult<Membership>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Membership>>(Request.Form["BaseParameter"]);
                result = await _MembershipService.GetByCategoryDepartmentID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("GetByCategoryDepartmentID_CategoryPositionID_ActiveToListAsync")]
        public virtual async Task<BaseResult<Membership>> GetByCategoryDepartmentID_CategoryPositionID_ActiveToListAsync()
        {
            var result = new BaseResult<Membership>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Membership>>(Request.Form["BaseParameter"]);
                result = await _MembershipService.GetByCategoryDepartmentID_CategoryPositionID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        [HttpPost]
        [Route("IsPasswordValidWithRegex")]
        public virtual async Task<BaseResult<Membership>> IsPasswordValidWithRegex()
        {
            var result = new BaseResult<Membership>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Membership>>(Request.Form["BaseParameter"]);
                result = await _MembershipService.IsPasswordValidWithRegex(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

