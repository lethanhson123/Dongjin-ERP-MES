namespace MESRepository.Implement
{
    public class pdpartRepository : BaseRepository<pdpart>
    , IpdpartRepository
    {
    private readonly Context _context;
    public pdpartRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

