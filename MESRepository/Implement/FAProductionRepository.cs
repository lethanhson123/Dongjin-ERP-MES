namespace MESRepository.Implement
{
    public class FAProductionRepository : BaseRepository<FAProduction>
    , IFAProductionRepository
    {
    private readonly Context _context;
    public FAProductionRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

