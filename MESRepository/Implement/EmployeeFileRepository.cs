namespace MESRepository.Implement
{
    public class EmployeeFileRepository : BaseRepository<EmployeeFile>
    , IEmployeeFileRepository
    {
    private readonly Context _context;
    public EmployeeFileRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

