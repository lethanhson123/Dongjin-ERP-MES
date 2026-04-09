namespace Repository.Implement
{
    public class CategoryParentheseRepository : BaseRepository<CategoryParenthese>
    , ICategoryParentheseRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryParentheseRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

