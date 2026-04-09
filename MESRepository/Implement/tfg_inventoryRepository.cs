namespace MESRepository.Implement
{
    public class tfg_inventoryRepository : BaseRepository<tfg_inventory>
    , Itfg_inventoryRepository
    {
    private readonly Context _context;
    public tfg_inventoryRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

