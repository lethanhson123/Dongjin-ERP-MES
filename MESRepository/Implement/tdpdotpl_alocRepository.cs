namespace MESRepository.Implement
{
    public class tdpdotpl_alocRepository : BaseRepository<tdpdotpl_aloc>
    , Itdpdotpl_alocRepository
    {
    private readonly Context _context;
    public tdpdotpl_alocRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

