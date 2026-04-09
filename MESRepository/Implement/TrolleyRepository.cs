namespace MESRepository.Implement
{
    public class TrolleyRepository : BaseRepository<Trolley>
    , ITrolleyRepository
    {
    private readonly Context _context;
    public TrolleyRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

