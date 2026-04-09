namespace MESRepository.Implement
{
    public class torderlist_wtimeRepository : BaseRepository<torderlist_wtime>
    , Itorderlist_wtimeRepository
    {
    private readonly Context _context;
    public torderlist_wtimeRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

