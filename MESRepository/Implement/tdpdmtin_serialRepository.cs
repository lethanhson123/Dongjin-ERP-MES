namespace MESRepository.Implement
{
    public class tdpdmtin_serialRepository : BaseRepository<tdpdmtin_serial>
    , Itdpdmtin_serialRepository
    {
    private readonly Context _context;
    public tdpdmtin_serialRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

