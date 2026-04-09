namespace MESRepository.Implement
{
    public class EmployeeContractRepository : BaseRepository<EmployeeContract>
    , IEmployeeContractRepository
    {
    private readonly Context _context;
    public EmployeeContractRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

