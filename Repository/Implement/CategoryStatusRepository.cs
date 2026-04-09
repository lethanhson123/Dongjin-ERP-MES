namespace Repository.Implement
{
    public class CategoryStatusRepository : BaseRepository<CategoryStatus>
    , ICategoryStatusRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryStatusRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

