namespace MESRepository.Implement
{
    public class tiivaj_historyRepository : BaseRepository<tiivaj_history>
    , Itiivaj_historyRepository
    {
    private readonly Context _context;
    public tiivaj_historyRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

