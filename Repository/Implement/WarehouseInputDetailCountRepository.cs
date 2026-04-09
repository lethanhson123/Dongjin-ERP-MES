namespace Repository.Implement
{
    public class WarehouseInputDetailCountRepository : BaseRepository<WarehouseInputDetailCount>
    , IWarehouseInputDetailCountRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseInputDetailCountRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

