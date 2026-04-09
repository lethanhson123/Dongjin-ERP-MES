namespace MESRepository.Implement
{
    public class torder_lead_bomRepository : BaseRepository<torder_lead_bom>
    , Itorder_lead_bomRepository
    {
    private readonly Context _context;
    public torder_lead_bomRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

