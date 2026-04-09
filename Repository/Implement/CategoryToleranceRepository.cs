namespace Repository.Implement
{
    public class CategoryToleranceRepository : BaseRepository<CategoryTolerance>
    , ICategoryToleranceRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryToleranceRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

