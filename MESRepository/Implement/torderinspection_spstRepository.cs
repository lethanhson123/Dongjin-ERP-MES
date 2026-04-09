namespace MESRepository.Implement
{
    public class torderinspection_spstRepository : BaseRepository<torderinspection_spst>
    , Itorderinspection_spstRepository
    {
    private readonly Context _context;
    public torderinspection_spstRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

