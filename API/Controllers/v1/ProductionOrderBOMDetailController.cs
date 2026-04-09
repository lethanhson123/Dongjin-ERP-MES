namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderBOMDetailController : BaseController<ProductionOrderBOMDetail, IProductionOrderBOMDetailService>
    {
    private readonly IProductionOrderBOMDetailService _ProductionOrderBOMDetailService;
    private readonly IWebHostEnvironment _WebHostEnvironment;
    public ProductionOrderBOMDetailController(IProductionOrderBOMDetailService ProductionOrderBOMDetailService, IWebHostEnvironment WebHostEnvironment) : base(ProductionOrderBOMDetailService, WebHostEnvironment)
    {
    _ProductionOrderBOMDetailService = ProductionOrderBOMDetailService;
    _WebHostEnvironment = WebHostEnvironment;
    }
    }
    }

