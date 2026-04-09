namespace Repository.Implement
{
    public class ProductionOrderProductionPlanSemiRepository : BaseRepository<ProductionOrderProductionPlanSemi>
    , IProductionOrderProductionPlanSemiRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderProductionPlanSemiRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

