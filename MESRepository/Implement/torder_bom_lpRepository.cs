namespace MESRepository.Implement
{
    public class torder_bom_lpRepository : BaseRepository<torder_bom_lp>
    , Itorder_bom_lpRepository
    {
    private readonly Context _context;
    public torder_bom_lpRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

