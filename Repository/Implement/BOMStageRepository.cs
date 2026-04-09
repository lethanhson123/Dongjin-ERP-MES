namespace Repository.Implement
{
    public class BOMStageRepository : BaseRepository<BOMStage>
    , IBOMStageRepository
    {
    private readonly Context.Context.Context _context;
    public BOMStageRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

