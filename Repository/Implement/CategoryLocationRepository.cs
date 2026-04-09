namespace Repository.Implement
{
    public class CategoryLocationRepository : BaseRepository<CategoryLocation>
    , ICategoryLocationRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryLocationRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

