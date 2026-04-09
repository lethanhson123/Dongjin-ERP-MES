namespace MESRepository.Implement
{
    public class apqp_cdgrRepository : BaseRepository<apqp_cdgr>
    , Iapqp_cdgrRepository
    {
    private readonly Context _context;
    public apqp_cdgrRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

