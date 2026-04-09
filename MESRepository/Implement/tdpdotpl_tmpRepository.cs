namespace MESRepository.Implement
{
    public class tdpdotpl_tmpRepository : BaseRepository<tdpdotpl_tmp>
    , Itdpdotpl_tmpRepository
    {
    private readonly Context _context;
    public tdpdotpl_tmpRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

