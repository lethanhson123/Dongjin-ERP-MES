namespace Repository.Implement
{
    public class ProductionOrderBOMDetailRepository : BaseRepository<ProductionOrderBOMDetail>
    , IProductionOrderBOMDetailRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderBOMDetailRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

