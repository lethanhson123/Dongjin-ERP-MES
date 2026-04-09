namespace MESRepository.Implement
{
    public class ttc_partRepository : BaseRepository<ttc_part>
    , Ittc_partRepository
    {
    private readonly Context _context;
    public ttc_partRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

