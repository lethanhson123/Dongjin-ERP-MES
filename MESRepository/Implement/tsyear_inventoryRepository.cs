namespace MESRepository.Implement
{
    public class tsyear_inventoryRepository : BaseRepository<tsyear_inventory>
    , Itsyear_inventoryRepository
    {
    private readonly Context _context;
    public tsyear_inventoryRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

