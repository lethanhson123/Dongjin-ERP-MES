namespace MESRepository.Implement
{
    public class tuser_log_chk_listRepository : BaseRepository<tuser_log_chk_list>
    , Ituser_log_chk_listRepository
    {
    private readonly Context _context;
    public tuser_log_chk_listRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

