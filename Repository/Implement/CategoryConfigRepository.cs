namespace Repository.Implement
{
    public class CategoryConfigRepository : BaseRepository<CategoryConfig>
    , ICategoryConfigRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryConfigRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

