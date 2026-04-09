namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderBOMController : BaseController<ProductionOrderBOM, IProductionOrderBOMService>
    {
        private readonly IProductionOrderBOMService _ProductionOrderBOMService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductionOrderBOMController(IProductionOrderBOMService ProductionOrderBOMService, IWebHostEnvironment WebHostEnvironment) : base(ProductionOrderBOMService, WebHostEnvironment)
        {
            _ProductionOrderBOMService = ProductionOrderBOMService;
            _WebHostEnvironment = WebHostEnvironment;
        }
    }
}

