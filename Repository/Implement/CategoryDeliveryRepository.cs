namespace Repository.Implement
{
    public class CategoryDeliveryRepository : BaseRepository<CategoryDelivery>
    , ICategoryDeliveryRepository
    {
    private readonly Context.Context.Context _context;
    public CategoryDeliveryRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

