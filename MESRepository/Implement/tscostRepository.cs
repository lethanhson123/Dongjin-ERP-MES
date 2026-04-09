namespace MESRepository.Implement
{
    public class tscostRepository : BaseRepository<tscost>
    , ItscostRepository
    {
    private readonly Context _context;
    public tscostRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

