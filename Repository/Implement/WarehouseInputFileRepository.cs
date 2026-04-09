namespace Repository.Implement
{
    public class WarehouseInputFileRepository : BaseRepository<WarehouseInputFile>
    , IWarehouseInputFileRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseInputFileRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

