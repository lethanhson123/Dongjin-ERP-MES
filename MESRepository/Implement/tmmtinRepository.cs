namespace MESRepository.Implement
{
    public class tmmtinRepository : BaseRepository<tmmtin>
    , ItmmtinRepository
    {
    private readonly Context _context;
    public tmmtinRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

