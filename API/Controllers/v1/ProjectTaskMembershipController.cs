namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProjectTaskMembershipController : BaseController<ProjectTaskMembership, IProjectTaskMembershipService>
    {
        private readonly IProjectTaskMembershipService _ProjectTaskMembershipService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProjectTaskMembershipController(IProjectTaskMembershipService ProjectTaskMembershipService, IWebHostEnvironment WebHostEnvironment) : base(ProjectTaskMembershipService, WebHostEnvironment)
        {
            _ProjectTaskMembershipService = ProjectTaskMembershipService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

