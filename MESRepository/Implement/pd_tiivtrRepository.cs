namespace MESRepository.Implement
{
    public class pd_tiivtrRepository : BaseRepository<pd_tiivtr>
    , Ipd_tiivtrRepository
    {
    private readonly Context _context;
    public pd_tiivtrRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

