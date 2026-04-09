namespace Repository.Implement
{
    public class WarehouseOutputDetailBarcodeRepository : BaseRepository<WarehouseOutputDetailBarcode>
    , IWarehouseOutputDetailBarcodeRepository
    {
    private readonly Context.Context.Context _context;
    public WarehouseOutputDetailBarcodeRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

