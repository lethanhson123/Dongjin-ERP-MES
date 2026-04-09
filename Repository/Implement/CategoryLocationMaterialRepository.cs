namespace Repository.Implement
{
    public class CategoryLocationMaterialRepository : BaseRepository<CategoryLocationMaterial>
    , ICategoryLocationMaterialRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryLocationMaterialRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

