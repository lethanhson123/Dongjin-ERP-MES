namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class MembershipDepartmentController : BaseController<MembershipDepartment, IMembershipDepartmentService>
    {
        private readonly IMembershipDepartmentService _MembershipDepartmentService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public MembershipDepartmentController(IMembershipDepartmentService MembershipDepartmentService, IWebHostEnvironment WebHostEnvironment) : base(MembershipDepartmentService, WebHostEnvironment)
        {
            _MembershipDepartmentService = MembershipDepartmentService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

