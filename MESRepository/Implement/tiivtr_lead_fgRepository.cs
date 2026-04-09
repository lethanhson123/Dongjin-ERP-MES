namespace MESRepository.Implement
{
    public class tiivtr_lead_fgRepository : BaseRepository<tiivtr_lead_fg>
    , Itiivtr_lead_fgRepository
    {
    private readonly Context _context;
    public tiivtr_lead_fgRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

