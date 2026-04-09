namespace MESRepository.Implement
{
    public class trackmtim_lt_inspRepository : BaseRepository<trackmtim_lt_insp>
    , Itrackmtim_lt_inspRepository
    {
    private readonly Context _context;
    public trackmtim_lt_inspRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

