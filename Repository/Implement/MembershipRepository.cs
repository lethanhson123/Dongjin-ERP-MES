namespace Repository.Implement
{
    public class MembershipRepository : BaseRepository<Membership>
    , IMembershipRepository
    {
    private readonly Context.Context.Context _context;
    public MembershipRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

