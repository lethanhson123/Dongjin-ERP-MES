namespace MESRepository.Implement
{
    public class kr_tdpdmtim_tmpRepository : BaseRepository<kr_tdpdmtim_tmp>
    , Ikr_tdpdmtim_tmpRepository
    {
    private readonly Context _context;
    public kr_tdpdmtim_tmpRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

