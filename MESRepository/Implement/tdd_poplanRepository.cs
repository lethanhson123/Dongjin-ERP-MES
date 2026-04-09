namespace MESRepository.Implement
{
    public class tdd_poplanRepository : BaseRepository<tdd_poplan>
    , Itdd_poplanRepository
    {
    private readonly Context _context;
    public tdd_poplanRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

