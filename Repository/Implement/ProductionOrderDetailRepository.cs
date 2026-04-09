namespace Repository.Implement
{
    public class ProductionOrderDetailRepository : BaseRepository<ProductionOrderDetail>
    , IProductionOrderDetailRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderDetailRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

