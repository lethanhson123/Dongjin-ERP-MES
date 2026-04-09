namespace Repository.Implement
{
    public class WarehouseOutputFileRepository : BaseRepository<WarehouseOutputFile>
    , IWarehouseOutputFileRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseOutputFileRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

