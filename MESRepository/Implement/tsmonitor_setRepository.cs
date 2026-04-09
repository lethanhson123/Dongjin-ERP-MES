namespace MESRepository.Implement
{
    public class tsmonitor_setRepository : BaseRepository<tsmonitor_set>
    , Itsmonitor_setRepository
    {
    private readonly Context _context;
    public tsmonitor_setRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

