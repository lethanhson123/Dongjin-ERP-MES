namespace MESRepository.Implement
{
    public class tdpdmtimRepository : BaseRepository<tdpdmtim>
    , ItdpdmtimRepository
    {
    private readonly Context _context;
    public tdpdmtimRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

