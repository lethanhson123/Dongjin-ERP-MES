namespace MESRepository.Implement
{
    public class ttoolmasterRepository : BaseRepository<ttoolmaster>
    , IttoolmasterRepository
    {
    private readonly Context _context;
    public ttoolmasterRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

