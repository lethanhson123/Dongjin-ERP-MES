namespace MESRepository.Implement
{
    public class torderlistRepository : BaseRepository<torderlist>
    , ItorderlistRepository
    {
    private readonly Context _context;
    public torderlistRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

