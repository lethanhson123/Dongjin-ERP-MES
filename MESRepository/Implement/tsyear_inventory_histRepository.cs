namespace MESRepository.Implement
{
    public class tsyear_inventory_histRepository : BaseRepository<tsyear_inventory_hist>
    , Itsyear_inventory_histRepository
    {
    private readonly Context _context;
    public tsyear_inventory_histRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

