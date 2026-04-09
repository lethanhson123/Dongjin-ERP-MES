namespace Repository.Implement
{
    public class CategorySystemRepository : BaseRepository<CategorySystem>
    , ICategorySystemRepository
    {
    private readonly Context.Context.Context _context;
    public CategorySystemRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

