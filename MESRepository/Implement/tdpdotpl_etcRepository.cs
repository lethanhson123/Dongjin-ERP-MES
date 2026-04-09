namespace MESRepository.Implement
{
    public class tdpdotpl_etcRepository : BaseRepository<tdpdotpl_etc>
    , Itdpdotpl_etcRepository
    {
    private readonly Context _context;
    public tdpdotpl_etcRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

