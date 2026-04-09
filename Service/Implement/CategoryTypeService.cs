namespace Service.Implement
{
    public class CategoryTypeService : BaseService<CategoryType, ICategoryTypeRepository>
    , ICategoryTypeService
    {
        private readonly ICategoryTypeRepository _CategoryTypeRepository;
        public CategoryTypeService(ICategoryTypeRepository CategoryTypeRepository) : base(CategoryTypeRepository)
        {
            _CategoryTypeRepository = CategoryTypeRepository;
        }
    }
}

