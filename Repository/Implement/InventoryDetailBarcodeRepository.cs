namespace Repository.Implement
{
    public class InventoryDetailBarcodeRepository : BaseRepository<InventoryDetailBarcode>
    , IInventoryDetailBarcodeRepository
    {
    private readonly Context.Context.Context _context;
    public InventoryDetailBarcodeRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

