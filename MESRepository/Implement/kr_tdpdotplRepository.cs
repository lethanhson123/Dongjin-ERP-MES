namespace MESRepository.Implement
{
    public class kr_tdpdotplRepository : BaseRepository<kr_tdpdotpl>
    , Ikr_tdpdotplRepository
    {
    private readonly Context _context;
    public kr_tdpdotplRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

