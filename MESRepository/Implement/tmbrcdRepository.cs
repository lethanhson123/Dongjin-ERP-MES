namespace MESRepository.Implement
{
    public class tmbrcdRepository : BaseRepository<tmbrcd>
    , ItmbrcdRepository
    {
    private readonly Context _context;
    public tmbrcdRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

