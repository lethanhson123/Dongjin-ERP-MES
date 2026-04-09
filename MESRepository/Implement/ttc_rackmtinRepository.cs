namespace MESRepository.Implement
{
    public class ttc_rackmtinRepository : BaseRepository<ttc_rackmtin>
    , Ittc_rackmtinRepository
    {
    private readonly Context _context;
    public ttc_rackmtinRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

