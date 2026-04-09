namespace Repository.Implement
{
    public class CategoryLevelRepository : BaseRepository<CategoryLevel>
    , ICategoryLevelRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryLevelRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

