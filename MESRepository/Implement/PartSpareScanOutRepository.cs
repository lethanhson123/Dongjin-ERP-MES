namespace MESRepository.Implement
{
    public class PartSpareScanOutRepository : BaseRepository<PartSpareScanOut>
    , IPartSpareScanOutRepository
    {
    private readonly Context _context;
    public PartSpareScanOutRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

