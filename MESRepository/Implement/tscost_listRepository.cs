namespace MESRepository.Implement
{
    public class tscost_listRepository : BaseRepository<tscost_list>
    , Itscost_listRepository
    {
    private readonly Context _context;
    public tscost_listRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

