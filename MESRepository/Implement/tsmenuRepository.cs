namespace MESRepository.Implement
{
    public class tsmenuRepository : BaseRepository<tsmenu>
    , ItsmenuRepository
    {
    private readonly Context _context;
    public tsmenuRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

