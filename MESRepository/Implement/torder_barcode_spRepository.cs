namespace MESRepository.Implement
{
    public class torder_barcode_spRepository : BaseRepository<torder_barcode_sp>
    , Itorder_barcode_spRepository
    {
    private readonly Context _context;
    public torder_barcode_spRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

