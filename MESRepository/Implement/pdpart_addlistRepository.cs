namespace MESRepository.Implement
{
    public class pdpart_addlistRepository : BaseRepository<pdpart_addlist>
    , Ipdpart_addlistRepository
    {
    private readonly Context _context;
    public pdpart_addlistRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

