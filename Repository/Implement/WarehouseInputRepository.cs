namespace Repository.Implement
{
    public class WarehouseInputRepository : BaseRepository<WarehouseInput>
    , IWarehouseInputRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseInputRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

