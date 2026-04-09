namespace Repository.Implement
{
    public class ProductionOrderProductionPlanMaterialRepository : BaseRepository<ProductionOrderProductionPlanMaterial>
    , IProductionOrderProductionPlanMaterialRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderProductionPlanMaterialRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

