namespace MESRepository.Implement
{
    public class torderinspection_lpRepository : BaseRepository<torderinspection_lp>
    , Itorderinspection_lpRepository
    {
    private readonly Context _context;
    public torderinspection_lpRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

