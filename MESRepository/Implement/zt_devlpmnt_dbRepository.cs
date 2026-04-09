namespace MESRepository.Implement
{
    public class zt_devlpmnt_dbRepository : BaseRepository<zt_devlpmnt_db>
    , Izt_devlpmnt_dbRepository
    {
    private readonly Context _context;
    public zt_devlpmnt_dbRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

