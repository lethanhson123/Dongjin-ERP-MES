namespace MESRepository.Implement
{
    public class tscdgrRepository : BaseRepository<tscdgr>
    , ItscdgrRepository
    {
    private readonly Context _context;
    public tscdgrRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

