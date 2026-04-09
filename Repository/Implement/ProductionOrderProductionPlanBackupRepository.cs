namespace Repository.Implement
{
    public class ProductionOrderProductionPlanBackupRepository : BaseRepository<ProductionOrderProductionPlanBackup>
    , IProductionOrderProductionPlanBackupRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderProductionPlanBackupRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

