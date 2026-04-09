namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderFileController : BaseController<ProductionOrderFile, IProductionOrderFileService>
    {
        private readonly IProductionOrderFileService _ProductionOrderFileService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductionOrderFileController(IProductionOrderFileService ProductionOrderFileService, IWebHostEnvironment WebHostEnvironment) : base(ProductionOrderFileService, WebHostEnvironment)
        {
            _ProductionOrderFileService = ProductionOrderFileService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

