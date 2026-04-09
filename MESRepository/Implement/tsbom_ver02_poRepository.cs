namespace MESRepository.Implement
{
    public class tsbom_ver02_poRepository : BaseRepository<tsbom_ver02_po>
    , Itsbom_ver02_poRepository
    {
    private readonly Context _context;
    public tsbom_ver02_poRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

