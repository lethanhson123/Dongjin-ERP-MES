namespace MESRepository.Implement
{
    public class KOMAXCheckErrorProofHistoryRepository : BaseRepository<KOMAXCheckErrorProofHistory>
    , IKOMAXCheckErrorProofHistoryRepository
    {
    private readonly Context _context;
    public KOMAXCheckErrorProofHistoryRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

