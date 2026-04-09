namespace MESRepository.Implement
{
    public class tfg_historyRepository : BaseRepository<tfg_history>
    , Itfg_historyRepository
    {
    private readonly Context _context;
    public tfg_historyRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

