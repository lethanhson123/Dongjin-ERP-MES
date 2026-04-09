namespace MESRepository.Implement
{
    public class tdpdmtim_reworkRepository : BaseRepository<tdpdmtim_rework>
    , Itdpdmtim_reworkRepository
    {
    private readonly Context _context;
    public tdpdmtim_reworkRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

