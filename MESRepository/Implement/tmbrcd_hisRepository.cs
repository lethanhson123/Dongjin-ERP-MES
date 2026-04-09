namespace MESRepository.Implement
{
    public class tmbrcd_hisRepository : BaseRepository<tmbrcd_his>
    , Itmbrcd_hisRepository
    {
    private readonly Context _context;
    public tmbrcd_hisRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

