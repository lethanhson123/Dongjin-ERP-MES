namespace Repository.Implement
{
    public class WarehouseInputDetailRepository : BaseRepository<WarehouseInputDetail>
    , IWarehouseInputDetailRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseInputDetailRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

