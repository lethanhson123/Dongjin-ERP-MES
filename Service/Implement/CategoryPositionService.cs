namespace Service.Implement
{
    public class CategoryPositionService : BaseService<CategoryPosition, ICategoryPositionRepository>
    , ICategoryPositionService
    {
    private readonly ICategoryPositionRepository _CategoryPositionRepository;
    public CategoryPositionService(ICategoryPositionRepository CategoryPositionRepository) : base(CategoryPositionRepository)
    {
    _CategoryPositionRepository = CategoryPositionRepository;
    }
  
    }
    }

