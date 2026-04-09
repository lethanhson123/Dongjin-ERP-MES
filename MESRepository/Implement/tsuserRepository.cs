namespace MESRepository.Implement
{
    public class tsuserRepository : BaseRepository<tsuser>
    , ItsuserRepository
    {
    private readonly Context _context;
    public tsuserRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

