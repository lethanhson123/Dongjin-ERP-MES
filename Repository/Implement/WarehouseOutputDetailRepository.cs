namespace Repository.Implement
{
    public class WarehouseOutputDetailRepository : BaseRepository<WarehouseOutputDetail>
    , IWarehouseOutputDetailRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseOutputDetailRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

