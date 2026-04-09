namespace Repository.Implement
{
    public class BOMDetailRepository : BaseRepository<BOMDetail>
    , IBOMDetailRepository
    {
    private readonly Context.Context.Context _context;
    public BOMDetailRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

