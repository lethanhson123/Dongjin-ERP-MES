namespace MESRepository.Implement
{
    public class tdpdmtim_tmpRepository : BaseRepository<tdpdmtim_tmp>
    , Itdpdmtim_tmpRepository
    {
    private readonly Context _context;
    public tdpdmtim_tmpRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

