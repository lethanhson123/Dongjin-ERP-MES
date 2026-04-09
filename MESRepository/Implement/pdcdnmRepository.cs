namespace MESRepository.Implement
{
    public class pdcdnmRepository : BaseRepository<pdcdnm>
    , IpdcdnmRepository
    {
    private readonly Context _context;
    public pdcdnmRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

