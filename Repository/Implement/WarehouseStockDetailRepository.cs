namespace Repository.Implement
{
    public class WarehouseStockDetailRepository : BaseRepository<WarehouseStockDetail>
    , IWarehouseStockDetailRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseStockDetailRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

