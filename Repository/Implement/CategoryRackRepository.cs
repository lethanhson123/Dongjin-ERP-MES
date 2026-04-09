namespace Repository.Implement
{
    public class CategoryRackRepository : BaseRepository<CategoryRack>
    , ICategoryRackRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryRackRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

