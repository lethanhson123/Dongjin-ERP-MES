namespace Repository.Implement
{
    public class MembershipDepartmentRepository : BaseRepository<MembershipDepartment>
    , IMembershipDepartmentRepository
    {
    private readonly Context.Context.Context _context;
    public MembershipDepartmentRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

