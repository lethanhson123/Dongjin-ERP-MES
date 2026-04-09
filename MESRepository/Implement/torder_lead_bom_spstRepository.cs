namespace MESRepository.Implement
{
    public class torder_lead_bom_spstRepository : BaseRepository<torder_lead_bom_spst>
    , Itorder_lead_bom_spstRepository
    {
    private readonly Context _context;
    public torder_lead_bom_spstRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

