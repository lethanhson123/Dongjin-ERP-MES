namespace MESRepository.Implement
{
    public class tmmtin_dmm_appRepository : BaseRepository<tmmtin_dmm_app>
    , Itmmtin_dmm_appRepository
    {
    private readonly Context _context;
    public tmmtin_dmm_appRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

