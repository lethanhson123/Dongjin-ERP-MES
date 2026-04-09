namespace Repository.Implement
{
    public class ProjectTaskHistoryRepository : BaseRepository<ProjectTaskHistory>
    , IProjectTaskHistoryRepository
    {
    private readonly Context.Context.Context _context;
    public ProjectTaskHistoryRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

