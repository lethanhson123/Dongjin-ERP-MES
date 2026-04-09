namespace Repository.Implement
{
    public class CategoryLayerRepository : BaseRepository<CategoryLayer>
    , ICategoryLayerRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryLayerRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

