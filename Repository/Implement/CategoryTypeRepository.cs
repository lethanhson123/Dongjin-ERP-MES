namespace Repository.Implement
{
    public class CategoryTypeRepository : BaseRepository<CategoryType>
    , ICategoryTypeRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryTypeRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

