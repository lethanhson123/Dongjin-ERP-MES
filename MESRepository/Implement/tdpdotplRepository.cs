namespace MESRepository.Implement
{
    public class tdpdotplRepository : BaseRepository<tdpdotpl>
    , ItdpdotplRepository
    {
    private readonly Context _context;
    public tdpdotplRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

