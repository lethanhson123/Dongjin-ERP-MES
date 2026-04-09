namespace MESRepository.Implement
{
    public class tspart_ecnRepository : BaseRepository<tspart_ecn>
    , Itspart_ecnRepository
    {
    private readonly Context _context;
    public tspart_ecnRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

