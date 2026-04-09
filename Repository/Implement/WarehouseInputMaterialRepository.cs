namespace Repository.Implement
{
    public class WarehouseInputMaterialRepository : BaseRepository<WarehouseInputMaterial>
    , IWarehouseInputMaterialRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseInputMaterialRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

