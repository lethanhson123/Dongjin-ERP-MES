namespace MESRepository.Implement
{
    public class tscodeRepository : BaseRepository<tscode>
    , ItscodeRepository
    {
    private readonly Context _context;
    public tscodeRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

