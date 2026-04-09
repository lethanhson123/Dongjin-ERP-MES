namespace Repository.Implement
{
    public class CategoryTermRepository : BaseRepository<CategoryTerm>
    , ICategoryTermRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryTermRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

