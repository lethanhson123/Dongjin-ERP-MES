namespace Service.Implement
{
    public class CategoryDeliveryService : BaseService<CategoryDelivery, ICategoryDeliveryRepository>
    , ICategoryDeliveryService
    {
        private readonly ICategoryDeliveryRepository _CategoryDeliveryRepository;
        public CategoryDeliveryService(ICategoryDeliveryRepository CategoryDeliveryRepository) : base(CategoryDeliveryRepository)
        {
            _CategoryDeliveryRepository = CategoryDeliveryRepository;
        }
    }
}

