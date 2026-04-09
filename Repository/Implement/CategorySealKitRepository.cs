namespace Repository.Implement
{
    public class CategorySealKitRepository : BaseRepository<CategorySealKit>
    , ICategorySealKitRepository
    {
    private readonly Context.Context.Context _context;
    public CategorySealKitRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

