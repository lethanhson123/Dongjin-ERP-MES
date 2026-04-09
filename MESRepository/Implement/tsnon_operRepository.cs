namespace MESRepository.Implement
{
    public class tsnon_operRepository : BaseRepository<tsnon_oper>
    , Itsnon_operRepository
    {
    private readonly Context _context;
    public tsnon_operRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

