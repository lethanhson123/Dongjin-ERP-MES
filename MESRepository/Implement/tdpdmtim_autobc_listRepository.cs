namespace MESRepository.Implement
{
    public class tdpdmtim_autobc_listRepository : BaseRepository<tdpdmtim_autobc_list>
    , Itdpdmtim_autobc_listRepository
    {
    private readonly Context _context;
    public tdpdmtim_autobc_listRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

