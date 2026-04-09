namespace MESRepository.Implement
{
    public class pd_asset_mmRepository : BaseRepository<pd_asset_mm>
    , Ipd_asset_mmRepository
    {
    private readonly Context _context;
    public pd_asset_mmRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

