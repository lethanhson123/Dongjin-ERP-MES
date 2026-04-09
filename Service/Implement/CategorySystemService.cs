namespace Service.Implement
{
    public class CategorySystemService : BaseService<CategorySystem, ICategorySystemRepository>
    , ICategorySystemService
    {
        private readonly ICategorySystemRepository _CategorySystemRepository;
        public CategorySystemService(ICategorySystemRepository CategorySystemRepository) : base(CategorySystemRepository)
        {
            _CategorySystemRepository = CategorySystemRepository;
        }
    }
}

