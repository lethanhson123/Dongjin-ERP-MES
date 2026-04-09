namespace MESRepository.Implement
{
    public class kr_inspctn_stRepository : BaseRepository<kr_inspctn_st>
    , Ikr_inspctn_stRepository
    {
    private readonly Context _context;
    public kr_inspctn_stRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

