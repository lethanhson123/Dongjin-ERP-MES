namespace Service.Implement
{
    public class BOMFileService : BaseService<BOMFile, IBOMFileRepository>
    , IBOMFileService
    {
    private readonly IBOMFileRepository _BOMFileRepository;
    public BOMFileService(IBOMFileRepository BOMFileRepository) : base(BOMFileRepository)
    {
    _BOMFileRepository = BOMFileRepository;
    }
  
    }
    }

