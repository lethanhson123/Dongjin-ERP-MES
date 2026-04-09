namespace MESRepository.Implement
{
    public class tsbom_listRepository : BaseRepository<tsbom_list>
    , Itsbom_listRepository
    {
    private readonly Context _context;
    public tsbom_listRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

