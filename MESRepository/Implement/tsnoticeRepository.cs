namespace MESRepository.Implement
{
    public class tsnoticeRepository : BaseRepository<tsnotice>
    , ItsnoticeRepository
    {
    private readonly Context _context;
    public tsnoticeRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

