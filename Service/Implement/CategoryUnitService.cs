namespace Service.Implement
{
    public class CategoryUnitService : BaseService<CategoryUnit, ICategoryUnitRepository>
    , ICategoryUnitService
    {
        private readonly ICategoryUnitRepository _CategoryUnitRepository;
        public CategoryUnitService(ICategoryUnitRepository CategoryUnitRepository) : base(CategoryUnitRepository)
        {
            _CategoryUnitRepository = CategoryUnitRepository;
        }
    }
}

