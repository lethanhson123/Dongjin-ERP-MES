namespace Repository.Implement
{
    public class ProjectTaskMembershipRepository : BaseRepository<ProjectTaskMembership>
    , IProjectTaskMembershipRepository
    {
    private readonly Context.Context.Context _context;
    public ProjectTaskMembershipRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

