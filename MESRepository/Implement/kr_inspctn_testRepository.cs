namespace MESRepository.Implement
{
    public class kr_inspctn_testRepository : BaseRepository<kr_inspctn_test>
    , Ikr_inspctn_testRepository
    {
    private readonly Context _context;
    public kr_inspctn_testRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

