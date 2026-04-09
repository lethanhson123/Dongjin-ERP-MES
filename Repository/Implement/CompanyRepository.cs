namespace Repository.Implement
{
    public class CompanyRepository : BaseRepository<Company>
    , ICompanyRepository
    {
    private readonly Context.Context.Context _context;
    public CompanyRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

