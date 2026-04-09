namespace MESRepository.Implement
{
    public class tworkresultRepository : BaseRepository<tworkresult>
    , ItworkresultRepository
    {
    private readonly Context _context;
    public tworkresultRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

