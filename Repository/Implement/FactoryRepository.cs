namespace Repository.Implement
{
    public class FactoryRepository : BaseRepository<Factory>
    , IFactoryRepository
    {
    private readonly Context.Context.Context _context;
    public FactoryRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

