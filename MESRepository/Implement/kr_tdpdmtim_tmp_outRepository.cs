namespace MESRepository.Implement
{
    public class kr_tdpdmtim_tmp_outRepository : BaseRepository<kr_tdpdmtim_tmp_out>
    , Ikr_tdpdmtim_tmp_outRepository
    {
    private readonly Context _context;
    public kr_tdpdmtim_tmp_outRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

