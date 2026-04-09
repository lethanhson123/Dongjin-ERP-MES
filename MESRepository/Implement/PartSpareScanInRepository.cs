namespace MESRepository.Implement
{
    public class PartSpareScanInRepository : BaseRepository<PartSpareScanIn>
    , IPartSpareScanInRepository
    {
    private readonly Context _context;
    public PartSpareScanInRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

