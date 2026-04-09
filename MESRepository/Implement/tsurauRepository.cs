namespace MESRepository.Implement
{
    public class tsurauRepository : BaseRepository<tsurau>
    , ItsurauRepository
    {
    private readonly Context _context;
    public tsurauRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

