namespace MESRepository.Implement
{
    public class tsnon_oper_andonRepository : BaseRepository<tsnon_oper_andon>
    , Itsnon_oper_andonRepository
    {
    private readonly Context _context;
    public tsnon_oper_andonRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

