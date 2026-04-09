namespace MESRepository.Implement
{
    public class tsnon_oper_workerRepository : BaseRepository<tsnon_oper_worker>
    , Itsnon_oper_workerRepository
    {
    private readonly Context _context;
    public tsnon_oper_workerRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

