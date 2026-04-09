namespace MESRepository.Implement
{
    public class tsmnauRepository : BaseRepository<tsmnau>
    , ItsmnauRepository
    {
    private readonly Context _context;
    public tsmnauRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

