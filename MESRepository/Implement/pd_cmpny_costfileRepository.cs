namespace MESRepository.Implement
{
    public class pd_cmpny_costfileRepository : BaseRepository<pd_cmpny_costfile>
    , Ipd_cmpny_costfileRepository
    {
    private readonly Context _context;
    public pd_cmpny_costfileRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

