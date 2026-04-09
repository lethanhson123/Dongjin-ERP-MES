namespace MESRepository.Implement
{
    public class torderinspectionRepository : BaseRepository<torderinspection>
    , ItorderinspectionRepository
    {
    private readonly Context _context;
    public torderinspectionRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

