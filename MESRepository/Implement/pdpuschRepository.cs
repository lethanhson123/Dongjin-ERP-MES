namespace MESRepository.Implement
{
    public class pdpuschRepository : BaseRepository<pdpusch>
    , IpdpuschRepository
    {
    private readonly Context _context;
    public pdpuschRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

