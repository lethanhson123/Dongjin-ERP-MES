namespace Repository.Implement
{
    public class CategoryVehicleRepository : BaseRepository<CategoryVehicle>
    , ICategoryVehicleRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryVehicleRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

