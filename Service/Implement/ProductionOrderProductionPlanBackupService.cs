
namespace Service.Implement
{
    public class ProductionOrderProductionPlanBackupService : BaseService<ProductionOrderProductionPlanBackup, IProductionOrderProductionPlanBackupRepository>
    , IProductionOrderProductionPlanBackupService
    {
        
        private readonly IProductionOrderProductionPlanBackupRepository _ProductionOrderProductionPlanBackupRepository;        
        public ProductionOrderProductionPlanBackupService(IProductionOrderProductionPlanBackupRepository ProductionOrderProductionPlanBackupRepository    
           

            ) : base(ProductionOrderProductionPlanBackupRepository)
        {            
            _ProductionOrderProductionPlanBackupRepository = ProductionOrderProductionPlanBackupRepository;
           
        }
       
    }
}

