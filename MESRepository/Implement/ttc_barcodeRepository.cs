namespace MESRepository.Implement
{
    public class ttc_barcodeRepository : BaseRepository<ttc_barcode>
    , Ittc_barcodeRepository
    {
    private readonly Context _context;
    public ttc_barcodeRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

