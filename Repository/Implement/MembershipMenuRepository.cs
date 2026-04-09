namespace Repository.Implement
{
    public class MembershipMenuRepository : BaseRepository<MembershipMenu>
    , IMembershipMenuRepository
    {
    private readonly Context.Context.Context _context;
    public MembershipMenuRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

