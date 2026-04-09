namespace Repository.Implement
{
    public class WarehouseInputDetailBarcodeMaterialRepository : BaseRepository<WarehouseInputDetailBarcodeMaterial>
    , IWarehouseInputDetailBarcodeMaterialRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseInputDetailBarcodeMaterialRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

