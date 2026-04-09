namespace MESRepository.Implement
{
    public class torderlist_workRepository : BaseRepository<torderlist_work>
    , Itorderlist_workRepository
    {
    private readonly Context _context;
    public torderlist_workRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

