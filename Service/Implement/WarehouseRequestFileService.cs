namespace Service.Implement
{
    public class WarehouseRequestFileService : BaseService<WarehouseRequestFile, IWarehouseRequestFileRepository>
    , IWarehouseRequestFileService
    {
    private readonly IWarehouseRequestFileRepository _WarehouseRequestFileRepository;
    public WarehouseRequestFileService(IWarehouseRequestFileRepository WarehouseRequestFileRepository) : base(WarehouseRequestFileRepository)
    {
    _WarehouseRequestFileRepository = WarehouseRequestFileRepository;
    }
  
    }
    }

