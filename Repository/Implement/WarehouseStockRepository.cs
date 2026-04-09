namespace Repository.Implement
{
    public class WarehouseStockRepository : BaseRepository<WarehouseStock>
    , IWarehouseStockRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseStockRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

