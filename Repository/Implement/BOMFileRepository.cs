namespace Repository.Implement
{
    public class BOMFileRepository : BaseRepository<BOMFile>
    , IBOMFileRepository
    {
    private readonly Context.Context.Context _context;
    public BOMFileRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

