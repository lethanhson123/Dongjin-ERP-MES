namespace Repository.Implement
{
    public class WarehouseOutputRepository : BaseRepository<WarehouseOutput>
    , IWarehouseOutputRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseOutputRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

