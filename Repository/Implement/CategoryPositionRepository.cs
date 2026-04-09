namespace Repository.Implement
{
    public class CategoryPositionRepository : BaseRepository<CategoryPosition>
    , ICategoryPositionRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryPositionRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

