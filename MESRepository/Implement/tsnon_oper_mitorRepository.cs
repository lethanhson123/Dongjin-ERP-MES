namespace MESRepository.Implement
{
    public class tsnon_oper_mitorRepository : BaseRepository<tsnon_oper_mitor>
    , Itsnon_oper_mitorRepository
    {
    private readonly Context _context;
    public tsnon_oper_mitorRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

