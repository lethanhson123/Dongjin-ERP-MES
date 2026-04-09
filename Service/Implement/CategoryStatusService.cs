namespace Service.Implement
{
    public class CategoryStatusService : BaseService<CategoryStatus, ICategoryStatusRepository>
    , ICategoryStatusService
    {
        private readonly ICategoryStatusRepository _CategoryStatusRepository;
        public CategoryStatusService(ICategoryStatusRepository CategoryStatusRepository) : base(CategoryStatusRepository)
        {
            _CategoryStatusRepository = CategoryStatusRepository;
        }
    }
}

