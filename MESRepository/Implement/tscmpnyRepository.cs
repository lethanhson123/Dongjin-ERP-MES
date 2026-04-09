namespace MESRepository.Implement
{
    public class tscmpnyRepository : BaseRepository<tscmpny>
    , ItscmpnyRepository
    {
    private readonly Context _context;
    public tscmpnyRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

