namespace Repository.Implement
{
    public class ProjectTaskRepository : BaseRepository<ProjectTask>
    , IProjectTaskRepository
    {
    private readonly Context.Context.Context _context;
    public ProjectTaskRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

