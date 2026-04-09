namespace Service.Implement
{
    public class WarehouseInputFileService : BaseService<WarehouseInputFile, IWarehouseInputFileRepository>
    , IWarehouseInputFileService
    {
    private readonly IWarehouseInputFileRepository _WarehouseInputFileRepository;
    public WarehouseInputFileService(IWarehouseInputFileRepository WarehouseInputFileRepository) : base(WarehouseInputFileRepository)
    {
    _WarehouseInputFileRepository = WarehouseInputFileRepository;
    }
  
    }
    }

