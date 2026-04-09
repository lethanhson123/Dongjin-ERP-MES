namespace Repository.Implement
{
    public class ProjectFileRepository : BaseRepository<ProjectFile>
    , IProjectFileRepository
    {
    private readonly Context.Context.Context _context;
    public ProjectFileRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

