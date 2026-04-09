namespace Repository.Implement
{
    public class ProductionOrderProductionPlanRepository : BaseRepository<ProductionOrderProductionPlan>
    , IProductionOrderProductionPlanRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderProductionPlanRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

