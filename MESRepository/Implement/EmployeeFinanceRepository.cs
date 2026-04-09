namespace MESRepository.Implement
{
    public class EmployeeFinanceRepository : BaseRepository<EmployeeFinance>
    , IEmployeeFinanceRepository
    {
    private readonly Context _context;
    public EmployeeFinanceRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

