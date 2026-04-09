namespace MESRepository.Implement
{
    public class zt_help_dbRepository : BaseRepository<zt_help_db>
    , Izt_help_dbRepository
    {
    private readonly Context _context;
    public zt_help_dbRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

