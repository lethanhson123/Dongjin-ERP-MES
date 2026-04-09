namespace MESRepository.Implement
{
    public class twwkarRepository : BaseRepository<twwkar>
    , ItwwkarRepository
    {
    private readonly Context _context;
    public twwkarRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

