namespace Repository.Implement
{
    public class WarehouseRequestRepository : BaseRepository<WarehouseRequest>
    , IWarehouseRequestRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseRequestRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

