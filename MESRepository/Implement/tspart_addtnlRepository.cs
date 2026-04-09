namespace MESRepository.Implement
{
    public class tspart_addtnlRepository : BaseRepository<tspart_addtnl>
    , Itspart_addtnlRepository
    {
    private readonly Context _context;
    public tspart_addtnlRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

