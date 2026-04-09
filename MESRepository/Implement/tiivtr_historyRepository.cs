namespace MESRepository.Implement
{
    public class tiivtr_historyRepository : BaseRepository<tiivtr_history>
    , Itiivtr_historyRepository
    {
    private readonly Context _context;
    public tiivtr_historyRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

