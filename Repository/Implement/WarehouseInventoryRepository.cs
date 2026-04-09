namespace Repository.Implement
{
    public class WarehouseInventoryRepository : BaseRepository<WarehouseInventory>
    , IWarehouseInventoryRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseInventoryRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

