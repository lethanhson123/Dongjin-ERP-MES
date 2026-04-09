namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class FactoryController : BaseController<Factory, IFactoryService>
    {
        private readonly IFactoryService _FactoryService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public FactoryController(IFactoryService FactoryService, IWebHostEnvironment WebHostEnvironment) : base(FactoryService, WebHostEnvironment)
        {
            _FactoryService = FactoryService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

