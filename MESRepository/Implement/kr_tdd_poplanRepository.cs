namespace MESRepository.Implement
{
    public class kr_tdd_poplanRepository : BaseRepository<kr_tdd_poplan>
    , Ikr_tdd_poplanRepository
    {
    private readonly Context _context;
    public kr_tdd_poplanRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

