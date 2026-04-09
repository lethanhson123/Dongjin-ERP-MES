namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MembershipCompanyController : BaseController<MembershipCompany, IMembershipCompanyService>
    {
        private readonly IMembershipCompanyService _MembershipCompanyService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembershipCompanyController(IMembershipCompanyService MembershipCompanyService, IWebHostEnvironment WebHostEnvironment) : base(MembershipCompanyService, WebHostEnvironment)
        {
            _MembershipCompanyService = MembershipCompanyService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

