namespace Repository.Implement
{
    public class ProductionOrderSPSTOrderRepository : BaseRepository<ProductionOrderSPSTOrder>
    , IProductionOrderSPSTOrderRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderSPSTOrderRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

