namespace MESRepository.Implement
{
    public class apqp_mstlstRepository : BaseRepository<apqp_mstlst>
    , Iapqp_mstlstRepository
    {
    private readonly Context _context;
    public apqp_mstlstRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

