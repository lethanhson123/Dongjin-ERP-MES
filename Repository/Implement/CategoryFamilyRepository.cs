namespace Repository.Implement
{
    public class CategoryFamilyRepository : BaseRepository<CategoryFamily>
    , ICategoryFamilyRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryFamilyRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

