namespace MESRepository.Implement
{
    public class torderlist_lpRepository : BaseRepository<torderlist_lp>
    , Itorderlist_lpRepository
    {
    private readonly Context _context;
    public torderlist_lpRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

