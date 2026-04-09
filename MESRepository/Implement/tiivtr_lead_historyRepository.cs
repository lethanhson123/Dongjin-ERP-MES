namespace MESRepository.Implement
{
    public class tiivtr_lead_historyRepository : BaseRepository<tiivtr_lead_history>
    , Itiivtr_lead_historyRepository
    {
    private readonly Context _context;
    public tiivtr_lead_historyRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

