namespace Repository.Implement
{
    public class InventoryDetailRepository : BaseRepository<InventoryDetail>
    , IInventoryDetailRepository
    {
    private readonly Context.Context.Context _context;
    public InventoryDetailRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

