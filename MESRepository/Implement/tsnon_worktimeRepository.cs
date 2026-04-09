namespace MESRepository.Implement
{
    public class tsnon_worktimeRepository : BaseRepository<tsnon_worktime>
    , Itsnon_worktimeRepository
    {
    private readonly Context _context;
    public tsnon_worktimeRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

