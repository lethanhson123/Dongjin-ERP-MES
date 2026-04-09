namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class BOMTermController : BaseController<BOMTerm, IBOMTermService>
    {
    private readonly IBOMTermService _BOMTermService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public BOMTermController(IBOMTermService BOMTermService, IWebHostEnvironment WebHostEnvironment) : base(BOMTermService, WebHostEnvironment)
    {
    _BOMTermService = BOMTermService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

