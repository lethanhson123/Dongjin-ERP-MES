namespace Repository.Implement
{
    public class CategoryUnitRepository : BaseRepository<CategoryUnit>
    , ICategoryUnitRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryUnitRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

