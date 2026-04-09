namespace MESRepository.Implement
{
    public class torderinspection_swRepository : BaseRepository<torderinspection_sw>
    , Itorderinspection_swRepository
    {
    private readonly Context _context;
    public torderinspection_swRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

