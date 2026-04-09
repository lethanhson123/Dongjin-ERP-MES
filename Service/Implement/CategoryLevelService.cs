namespace Service.Implement
{
    public class CategoryLevelService : BaseService<CategoryLevel, ICategoryLevelRepository>
    , ICategoryLevelService
    {
        private readonly ICategoryLevelRepository _CategoryLevelRepository;
        public CategoryLevelService(ICategoryLevelRepository CategoryLevelRepository) : base(CategoryLevelRepository)
        {
            _CategoryLevelRepository = CategoryLevelRepository;
        }
    }
}

