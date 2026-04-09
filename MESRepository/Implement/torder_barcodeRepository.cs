namespace MESRepository.Implement
{
    public class torder_barcodeRepository : BaseRepository<torder_barcode>
    , Itorder_barcodeRepository
    {
    private readonly Context _context;
    public torder_barcodeRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

