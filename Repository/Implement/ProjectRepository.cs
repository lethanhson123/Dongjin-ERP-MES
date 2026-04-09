namespace Repository.Implement
{
    public class ProjectRepository : BaseRepository<Project>
    , IProjectRepository
    {
    private readonly Context.Context.Context _context;
    public ProjectRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

