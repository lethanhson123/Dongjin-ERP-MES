namespace Repository.Implement
{
    public class BOMRepository : BaseRepository<BOM>
    , IBOMRepository
    {
    private readonly Context.Context.Context _context;
    public BOMRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

