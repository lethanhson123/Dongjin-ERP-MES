namespace MESRepository.Implement
{
    public class twwkar_lpRepository : BaseRepository<twwkar_lp>
    , Itwwkar_lpRepository
    {
    private readonly Context _context;
    public twwkar_lpRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

