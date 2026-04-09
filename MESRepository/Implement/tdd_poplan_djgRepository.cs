namespace MESRepository.Implement
{
    public class tdd_poplan_djgRepository : BaseRepository<tdd_poplan_djg>
    , Itdd_poplan_djgRepository
    {
    private readonly Context _context;
    public tdd_poplan_djgRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

