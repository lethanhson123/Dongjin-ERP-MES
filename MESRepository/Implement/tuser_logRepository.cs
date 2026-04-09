namespace MESRepository.Implement
{
    public class tuser_logRepository : BaseRepository<tuser_log>
    , Ituser_logRepository
    {
    private readonly Context _context;
    public tuser_logRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

