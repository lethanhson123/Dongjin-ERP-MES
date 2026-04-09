namespace MESRepository.Implement
{
    public class tiivtrRepository : BaseRepository<tiivtr>
    , ItiivtrRepository
    {
    private readonly Context _context;
    public tiivtrRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

