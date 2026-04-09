namespace MESRepository.Implement
{
    public class pd_part_costRepository : BaseRepository<pd_part_cost>
    , Ipd_part_costRepository
    {
    private readonly Context _context;
    public pd_part_costRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

