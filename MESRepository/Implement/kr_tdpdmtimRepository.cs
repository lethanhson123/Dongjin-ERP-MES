namespace MESRepository.Implement
{
    public class kr_tdpdmtimRepository : BaseRepository<kr_tdpdmtim>
    , Ikr_tdpdmtimRepository
    {
    private readonly Context _context;
    public kr_tdpdmtimRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

