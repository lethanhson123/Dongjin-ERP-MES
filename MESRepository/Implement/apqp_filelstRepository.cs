namespace MESRepository.Implement
{
    public class apqp_filelstRepository : BaseRepository<apqp_filelst>
    , Iapqp_filelstRepository
    {
    private readonly Context _context;
    public apqp_filelstRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

