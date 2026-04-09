namespace Repository.Implement
{
    public class MembershipCompanyRepository : BaseRepository<MembershipCompany>
    , IMembershipCompanyRepository
    {
    private readonly Context.Context.Context _context;
    public MembershipCompanyRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

