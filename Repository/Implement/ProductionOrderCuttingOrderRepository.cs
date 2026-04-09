namespace Repository.Implement
{
    public class ProductionOrderCuttingOrderRepository : BaseRepository<ProductionOrderCuttingOrder>
    , IProductionOrderCuttingOrderRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderCuttingOrderRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

