namespace Repository.Implement
{
    public class CategoryMaterialRepository : BaseRepository<CategoryMaterial>
    , ICategoryMaterialRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryMaterialRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

