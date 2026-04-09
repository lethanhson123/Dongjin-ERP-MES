namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProjectController : BaseController<Project, IProjectService>
    {
        private readonly IProjectService _ProjectService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProjectController(IProjectService ProjectService, IWebHostEnvironment WebHostEnvironment) : base(ProjectService, WebHostEnvironment)
        {
            _ProjectService = ProjectService;
            _WebHostEnvironment = WebHostEnvironment;
        }
        [HttpPost]
        [Route("GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync")]
        public virtual async Task<BaseResult<Project>> GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync()
        {
            var result = new BaseResult<Project>();
            try
            {
                var BaseParameter = JsonConvert.DeserializeObject<BaseParameter<Project>>(Request.Form["BaseParameter"]);
                result = await _ProjectService.GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter);
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
    }
}

