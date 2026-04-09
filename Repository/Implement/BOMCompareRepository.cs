namespace Repository.Implement
{
    public class BOMCompareRepository : BaseRepository<BOMCompare>
    , IBOMCompareRepository
    {
    private readonly Context.Context.Context _context;
    public BOMCompareRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

