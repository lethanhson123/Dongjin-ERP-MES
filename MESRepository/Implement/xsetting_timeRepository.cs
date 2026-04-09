namespace MESRepository.Implement
{
    public class xsetting_timeRepository : BaseRepository<xsetting_time>
    , Ixsetting_timeRepository
    {
    private readonly Context _context;
    public xsetting_timeRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

