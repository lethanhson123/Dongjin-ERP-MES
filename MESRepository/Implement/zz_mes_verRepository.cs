namespace MESRepository.Implement
{
    public class zz_mes_verRepository : BaseRepository<zz_mes_ver>
    , Izz_mes_verRepository
    {
    private readonly Context _context;
    public zz_mes_verRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

