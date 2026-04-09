namespace MESRepository.Implement
{
    public class torderlist_lplistRepository : BaseRepository<torderlist_lplist>
    , Itorderlist_lplistRepository
    {
    private readonly Context _context;
    public torderlist_lplistRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

