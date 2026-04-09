namespace Repository.Implement
{
    public class InventoryRepository : BaseRepository<Inventory>
    , IInventoryRepository
    {
    private readonly Context.Context.Context _context;
    public InventoryRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

