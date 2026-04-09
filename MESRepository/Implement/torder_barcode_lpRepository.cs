namespace MESRepository.Implement
{
    public class torder_barcode_lpRepository : BaseRepository<torder_barcode_lp>
    , Itorder_barcode_lpRepository
    {
    private readonly Context _context;
    public torder_barcode_lpRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

