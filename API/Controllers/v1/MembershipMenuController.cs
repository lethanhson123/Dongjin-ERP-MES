namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MembershipMenuController : BaseController<MembershipMenu, IMembershipMenuService>
    {
        private readonly IMembershipMenuService _MembershipMenuService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembershipMenuController(IMembershipMenuService MembershipMenuService, IWebHostEnvironment WebHostEnvironment) : base(MembershipMenuService, WebHostEnvironment)
        {
            _MembershipMenuService = MembershipMenuService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

