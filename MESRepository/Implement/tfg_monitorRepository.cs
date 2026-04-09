namespace MESRepository.Implement
{
    public class tfg_monitorRepository : BaseRepository<tfg_monitor>
    , Itfg_monitorRepository
    {
    private readonly Context _context;
    public tfg_monitorRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

