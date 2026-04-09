namespace MESRepository.Implement
{
    public class pd_mc_orderlistRepository : BaseRepository<pd_mc_orderlist>
    , Ipd_mc_orderlistRepository
    {
    private readonly Context _context;
    public pd_mc_orderlistRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

