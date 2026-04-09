namespace MESRepository.Implement
{
    public class twwkar_spstRepository : BaseRepository<twwkar_spst>
    , Itwwkar_spstRepository
    {
    private readonly Context _context;
    public twwkar_spstRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

