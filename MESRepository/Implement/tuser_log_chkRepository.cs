namespace MESRepository.Implement
{
    public class tuser_log_chkRepository : BaseRepository<tuser_log_chk>
    , Ituser_log_chkRepository
    {
    private readonly Context _context;
    public tuser_log_chkRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

