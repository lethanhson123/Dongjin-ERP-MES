namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BOMFileController : BaseController<BOMFile, IBOMFileService>
    {
        private readonly IBOMFileService _BOMFileService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public BOMFileController(IBOMFileService BOMFileService, IWebHostEnvironment WebHostEnvironment) : base(BOMFileService, WebHostEnvironment)
        {
            _BOMFileService = BOMFileService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

