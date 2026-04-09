namespace MESRepository.Implement
{
    public class tdd_ct_stRepository : BaseRepository<tdd_ct_st>
    , Itdd_ct_stRepository
    {
    private readonly Context _context;
    public tdd_ct_stRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

