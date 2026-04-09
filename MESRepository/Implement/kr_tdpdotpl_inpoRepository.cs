namespace MESRepository.Implement
{
    public class kr_tdpdotpl_inpoRepository : BaseRepository<kr_tdpdotpl_inpo>
    , Ikr_tdpdotpl_inpoRepository
    {
    private readonly Context _context;
    public kr_tdpdotpl_inpoRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

