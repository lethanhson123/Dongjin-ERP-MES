namespace MESRepository.Implement
{
    public class tmmtin_dmm_leadRepository : BaseRepository<tmmtin_dmm_lead>
    , Itmmtin_dmm_leadRepository
    {
    private readonly Context _context;
    public tmmtin_dmm_leadRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

