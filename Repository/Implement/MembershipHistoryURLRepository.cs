namespace Repository.Implement
{
    public class MembershipHistoryURLRepository : BaseRepository<MembershipHistoryURL>
    , IMembershipHistoryURLRepository
    {
    private readonly Context.Context.Context _context;
    public MembershipHistoryURLRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

