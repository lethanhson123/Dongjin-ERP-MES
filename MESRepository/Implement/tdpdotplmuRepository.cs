namespace MESRepository.Implement
{
    public class tdpdotplmuRepository : BaseRepository<tdpdotplmu>
    , ItdpdotplmuRepository
    {
    private readonly Context _context;
    public tdpdotplmuRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

