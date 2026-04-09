namespace Repository.Implement
{
    public class MembershipTokenRepository : BaseRepository<MembershipToken>
    , IMembershipTokenRepository
    {
    private readonly Context.Context.Context _context;
    public MembershipTokenRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

