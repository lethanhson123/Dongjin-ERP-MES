namespace MESRepository.Implement
{
    public class pdcmpnyRepository : BaseRepository<pdcmpny>
    , IpdcmpnyRepository
    {
    private readonly Context _context;
    public pdcmpnyRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

