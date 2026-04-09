namespace Repository.Implement
{
    public class CategoryMenuRepository : BaseRepository<CategoryMenu>
    , ICategoryMenuRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryMenuRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

