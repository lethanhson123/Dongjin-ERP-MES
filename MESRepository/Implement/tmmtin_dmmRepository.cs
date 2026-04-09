namespace MESRepository.Implement
{
    public class tmmtin_dmmRepository : BaseRepository<tmmtin_dmm>
    , Itmmtin_dmmRepository
    {
    private readonly Context _context;
    public tmmtin_dmmRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

