namespace MESRepository.Implement
{
    public class tsnon_oper_andon_listRepository : BaseRepository<tsnon_oper_andon_list>
    , Itsnon_oper_andon_listRepository
    {
    private readonly Context _context;
    public tsnon_oper_andon_listRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

