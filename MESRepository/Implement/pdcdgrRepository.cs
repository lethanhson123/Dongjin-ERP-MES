namespace MESRepository.Implement
{
    public class pdcdgrRepository : BaseRepository<pdcdgr>
    , IpdcdgrRepository
    {
    private readonly Context _context;
    public pdcdgrRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

