namespace MESRepository.Implement
{
    public class ttc_bomRepository : BaseRepository<ttc_bom>
    , Ittc_bomRepository
    {
    private readonly Context _context;
    public ttc_bomRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

