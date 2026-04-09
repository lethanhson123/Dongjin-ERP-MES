namespace Repository.Implement
{
    public class ProductionOrderOutputScheduleRepository : BaseRepository<ProductionOrderOutputSchedule>
    , IProductionOrderOutputScheduleRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderOutputScheduleRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

