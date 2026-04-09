namespace Service.Implement
{
    public class CategoryToleranceService : BaseService<CategoryTolerance, ICategoryToleranceRepository>
    , ICategoryToleranceService
    {
        private readonly ICategoryToleranceRepository _CategoryToleranceRepository;
        public CategoryToleranceService(ICategoryToleranceRepository CategoryToleranceRepository) : base(CategoryToleranceRepository)
        {
            _CategoryToleranceRepository = CategoryToleranceRepository;
        }
    }
}

