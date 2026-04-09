namespace Repository.Implement
{
    public class BOMTermRepository : BaseRepository<BOMTerm>
    , IBOMTermRepository
    {
    private readonly Context.Context.Context _context;
    public BOMTermRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

