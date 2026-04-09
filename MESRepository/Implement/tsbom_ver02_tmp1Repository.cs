namespace MESRepository.Implement
{
    public class tsbom_ver02_tmp1Repository : BaseRepository<tsbom_ver02_tmp1>
    , Itsbom_ver02_tmp1Repository
    {
    private readonly Context _context;
    public tsbom_ver02_tmp1Repository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

