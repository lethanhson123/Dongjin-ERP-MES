namespace MESRepository.Implement
{
    public class tdpdmtim_delRepository : BaseRepository<tdpdmtim_del>
    , Itdpdmtim_delRepository
    {
    private readonly Context _context;
    public tdpdmtim_delRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

