namespace MESRepository.Implement
{
    public class torderlist_swRepository : BaseRepository<torderlist_sw>
    , Itorderlist_swRepository
    {
    private readonly Context _context;
    public torderlist_swRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

