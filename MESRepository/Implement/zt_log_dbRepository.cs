namespace MESRepository.Implement
{
    public class zt_log_dbRepository : BaseRepository<zt_log_db>
    , Izt_log_dbRepository
    {
    private readonly Context _context;
    public zt_log_dbRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

