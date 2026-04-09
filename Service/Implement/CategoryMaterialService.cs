namespace Service.Implement
{
    public class CategoryMaterialService : BaseService<CategoryMaterial, ICategoryMaterialRepository>
    , ICategoryMaterialService
    {
    private readonly ICategoryMaterialRepository _CategoryMaterialRepository;
    public CategoryMaterialService(ICategoryMaterialRepository CategoryMaterialRepository) : base(CategoryMaterialRepository)
    {
    _CategoryMaterialRepository = CategoryMaterialRepository;
    }
  
    }
    }

