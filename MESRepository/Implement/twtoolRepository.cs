namespace MESRepository.Implement
{
    public class twtoolRepository : BaseRepository<twtool>
    , ItwtoolRepository
    {
    private readonly Context _context;
    public twtoolRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

