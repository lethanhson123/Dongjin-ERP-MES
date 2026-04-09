namespace MESRepository.Implement
{
    public class pd_inout_partRepository : BaseRepository<pd_inout_part>
    , Ipd_inout_partRepository
    {
    private readonly Context _context;
    public pd_inout_partRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

