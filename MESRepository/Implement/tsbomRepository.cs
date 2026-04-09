namespace MESRepository.Implement
{
    public class tsbomRepository : BaseRepository<tsbom>
    , ItsbomRepository
    {
    private readonly Context _context;
    public tsbomRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

