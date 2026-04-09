namespace Repository.Implement
{
    public class CategoryCompanyRepository : BaseRepository<CategoryCompany>
    , ICategoryCompanyRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryCompanyRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

