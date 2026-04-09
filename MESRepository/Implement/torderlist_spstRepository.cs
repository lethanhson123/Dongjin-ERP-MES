namespace MESRepository.Implement
{
    public class torderlist_spstRepository : BaseRepository<torderlist_spst>
    , Itorderlist_spstRepository
    {
    private readonly Context _context;
    public torderlist_spstRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

