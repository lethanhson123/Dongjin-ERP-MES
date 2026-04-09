namespace MESRepository.Implement
{
    public class ttoolmaster2Repository : BaseRepository<ttoolmaster2>
    , Ittoolmaster2Repository
    {
    private readonly Context _context;
    public ttoolmaster2Repository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

