namespace MESRepository.Implement
{
    public class trackmtimRepository : BaseRepository<trackmtim>
    , ItrackmtimRepository
    {
    private readonly Context _context;
    public trackmtimRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

