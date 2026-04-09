namespace Service.Implement
{
    public class ProductionOrderFileService : BaseService<ProductionOrderFile, IProductionOrderFileRepository>
    , IProductionOrderFileService
    {
    private readonly IProductionOrderFileRepository _ProductionOrderFileRepository;
    public ProductionOrderFileService(IProductionOrderFileRepository ProductionOrderFileRepository) : base(ProductionOrderFileRepository)
    {
    _ProductionOrderFileRepository = ProductionOrderFileRepository;
    }
  
    }
    }

