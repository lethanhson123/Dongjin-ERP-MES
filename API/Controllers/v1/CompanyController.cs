namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CompanyController : BaseController<Company, ICompanyService>
    {
        private readonly ICompanyService _CompanyService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public CompanyController(ICompanyService CompanyService, IWebHostEnvironment WebHostEnvironment) : base(CompanyService, WebHostEnvironment)
        {
            _CompanyService = CompanyService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByMembershipID_ActiveToListAsync")]
        public virtual async Task<BaseResult<Company>> GetByMembershipID_ActiveToListAsync()
        {
            var result = new BaseResult<Company>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Company>>(Request.Form["BaseParameter"]);
                result = await _CompanyService.GetByMembershipID_ActiveToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

