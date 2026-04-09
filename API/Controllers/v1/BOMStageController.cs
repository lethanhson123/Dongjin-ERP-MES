namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BOMStageController : BaseController<BOMStage, IBOMStageService>
    {
        private readonly IBOMStageService _BOMStageService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public BOMStageController(IBOMStageService BOMStageService, IWebHostEnvironment WebHostEnvironment) : base(BOMStageService, WebHostEnvironment)
        {
            _BOMStageService = BOMStageService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

