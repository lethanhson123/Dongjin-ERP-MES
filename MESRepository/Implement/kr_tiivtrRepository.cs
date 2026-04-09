namespace MESRepository.Implement
{
    public class kr_tiivtrRepository : BaseRepository<kr_tiivtr>
    , Ikr_tiivtrRepository
    {
    private readonly Context _context;
    public kr_tiivtrRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

