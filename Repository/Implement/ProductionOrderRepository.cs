namespace Repository.Implement
{
    public class ProductionOrderRepository : BaseRepository<ProductionOrder>
    , IProductionOrderRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

