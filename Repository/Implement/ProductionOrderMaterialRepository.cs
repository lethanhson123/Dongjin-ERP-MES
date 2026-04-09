namespace Repository.Implement
{
    public class ProductionOrderMaterialRepository : BaseRepository<ProductionOrderMaterial>
    , IProductionOrderMaterialRepository
    {
    private readonly Context.Context.Context _context;
    public ProductionOrderMaterialRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

