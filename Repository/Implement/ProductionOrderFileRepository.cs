namespace Repository.Implement
{
    public class ProductionOrderFileRepository : BaseRepository<ProductionOrderFile>
    , IProductionOrderFileRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderFileRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

