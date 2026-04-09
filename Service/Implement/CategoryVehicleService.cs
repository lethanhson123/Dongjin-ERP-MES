namespace Service.Implement
{
    public class CategoryVehicleService : BaseService<CategoryVehicle, ICategoryVehicleRepository>
    , ICategoryVehicleService
    {
        private readonly ICategoryVehicleRepository _CategoryVehicleRepository;
        public CategoryVehicleService(ICategoryVehicleRepository CategoryVehicleRepository) : base(CategoryVehicleRepository)
        {
            _CategoryVehicleRepository = CategoryVehicleRepository;
        }
    }
}

