namespace Repository.Implement
{
    public class WarehouseRequestFileRepository : BaseRepository<WarehouseRequestFile>
    , IWarehouseRequestFileRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseRequestFileRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

