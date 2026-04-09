namespace MESRepository.Implement
{
    public class torder_lead_bom_exclRepository : BaseRepository<torder_lead_bom_excl>
    , Itorder_lead_bom_exclRepository
    {
    private readonly Context _context;
    public torder_lead_bom_exclRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

