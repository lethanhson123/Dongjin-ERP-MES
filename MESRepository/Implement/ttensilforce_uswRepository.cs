namespace MESRepository.Implement
{
    public class ttensilforce_uswRepository : BaseRepository<ttensilforce_usw>
    , Ittensilforce_uswRepository
    {
    private readonly Context _context;
    public ttensilforce_uswRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

