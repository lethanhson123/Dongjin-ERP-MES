namespace API.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ProductionOrderProductionPlanBackupController : BaseController<ProductionOrderProductionPlanBackup, IProductionOrderProductionPlanBackupService>
    {
        private readonly IProductionOrderProductionPlanBackupService _ProductionOrderProductionPlanBackupService;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public ProductionOrderProductionPlanBackupController(IProductionOrderProductionPlanBackupService ProductionOrderProductionPlanBackupService, IWebHostEnvironment WebHostEnvironment) : base(ProductionOrderProductionPlanBackupService, WebHostEnvironment)
        {
            _ProductionOrderProductionPlanBackupService = ProductionOrderProductionPlanBackupService;
            _WebHostEnvironment = WebHostEnvironment;
        }      
    }
}

