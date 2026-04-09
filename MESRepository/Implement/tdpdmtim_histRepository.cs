namespace MESRepository.Implement
{
    public class tdpdmtim_histRepository : BaseRepository<tdpdmtim_hist>
    , Itdpdmtim_histRepository
    {
    private readonly Context _context;
    public tdpdmtim_histRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

