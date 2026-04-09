namespace MESRepository.Implement
{
    public class tiivajRepository : BaseRepository<tiivaj>
    , ItiivajRepository
    {
    private readonly Context _context;
    public tiivajRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

