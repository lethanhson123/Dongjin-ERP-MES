namespace MESRepository.Implement
{
    public class tdpdotpl_labelRepository : BaseRepository<tdpdotpl_label>
    , Itdpdotpl_labelRepository
    {
    private readonly Context _context;
    public tdpdotpl_labelRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

