namespace MESRepository.Implement
{
    public class tspartRepository : BaseRepository<tspart>
    , ItspartRepository
    {
    private readonly Context _context;
    public tspartRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

