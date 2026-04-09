namespace MESRepository.Implement
{
    public class aatableRepository : BaseRepository<aatable>
    , IaatableRepository
    {
    private readonly Context _context;
    public aatableRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

