namespace Service.Implement
{
    public class WarehouseOutputFileService : BaseService<WarehouseOutputFile, IWarehouseOutputFileRepository>
    , IWarehouseOutputFileService
    {
    private readonly IWarehouseOutputFileRepository _WarehouseOutputFileRepository;
    public WarehouseOutputFileService(IWarehouseOutputFileRepository WarehouseOutputFileRepository) : base(WarehouseOutputFileRepository)
    {
    _WarehouseOutputFileRepository = WarehouseOutputFileRepository;
    }
  
    }
    }

