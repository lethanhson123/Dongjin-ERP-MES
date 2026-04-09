namespace MESRepository.Implement
{
    public class ttensilforceRepository : BaseRepository<ttensilforce>
    , IttensilforceRepository
    {
    private readonly Context _context;
    public ttensilforceRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

