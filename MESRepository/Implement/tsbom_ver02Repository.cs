namespace MESRepository.Implement
{
    public class tsbom_ver02Repository : BaseRepository<tsbom_ver02>
    , Itsbom_ver02Repository
    {
    private readonly Context _context;
    public tsbom_ver02Repository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

