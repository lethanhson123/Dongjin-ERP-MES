namespace MESRepository.Implement
{
    public class tdpdmtim_locRepository : BaseRepository<tdpdmtim_loc>
    , Itdpdmtim_locRepository
    {
    private readonly Context _context;
    public tdpdmtim_locRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

