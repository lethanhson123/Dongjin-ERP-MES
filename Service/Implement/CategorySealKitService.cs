namespace Service.Implement
{
    public class CategorySealKitService : BaseService<CategorySealKit, ICategorySealKitRepository>
    , ICategorySealKitService
    {
        private readonly ICategorySealKitRepository _CategorySealKitRepository;
        public CategorySealKitService(ICategorySealKitRepository CategorySealKitRepository) : base(CategorySealKitRepository)
        {
            _CategorySealKitRepository = CategorySealKitRepository;
        }
    }
}

