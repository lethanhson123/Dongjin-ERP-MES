namespace MESRepository.Implement
{
    public class trackmasterRepository : BaseRepository<trackmaster>
    , ItrackmasterRepository
    {
    private readonly Context _context;
    public trackmasterRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

