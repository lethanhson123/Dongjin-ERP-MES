namespace Repository.Implement
{
    public class ProductionOrderBOMRepository : BaseRepository<ProductionOrderBOM>
    , IProductionOrderBOMRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderBOMRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

