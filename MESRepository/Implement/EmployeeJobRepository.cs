namespace MESRepository.Implement
{
    public class EmployeeJobRepository : BaseRepository<EmployeeJob>
    , IEmployeeJobRepository
    {
    private readonly Context _context;
    public EmployeeJobRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

