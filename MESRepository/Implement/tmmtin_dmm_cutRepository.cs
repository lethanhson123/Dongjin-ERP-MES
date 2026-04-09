namespace MESRepository.Implement
{
    public class tmmtin_dmm_cutRepository : BaseRepository<tmmtin_dmm_cut>
    , Itmmtin_dmm_cutRepository
    {
    private readonly Context _context;
    public tmmtin_dmm_cutRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

