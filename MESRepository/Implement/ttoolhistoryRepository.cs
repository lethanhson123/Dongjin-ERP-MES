namespace MESRepository.Implement
{
    public class ttoolhistoryRepository : BaseRepository<ttoolhistory>
    , IttoolhistoryRepository
    {
    private readonly Context _context;
    public ttoolhistoryRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

