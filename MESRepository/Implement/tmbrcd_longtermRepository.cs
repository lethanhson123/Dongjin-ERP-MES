namespace MESRepository.Implement
{
    public class tmbrcd_longtermRepository : BaseRepository<tmbrcd_longterm>
    , Itmbrcd_longtermRepository
    {
    private readonly Context _context;
    public tmbrcd_longtermRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

