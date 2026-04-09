namespace Repository.Implement
{
    public class WarehouseInputDetailBarcodeRepository : BaseRepository<WarehouseInputDetailBarcode>
    , IWarehouseInputDetailBarcodeRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseInputDetailBarcodeRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

