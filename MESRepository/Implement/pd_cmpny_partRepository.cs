namespace MESRepository.Implement
{
    public class pd_cmpny_partRepository : BaseRepository<pd_cmpny_part>
    , Ipd_cmpny_partRepository
    {
    private readonly Context _context;
    public pd_cmpny_partRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

