namespace MESRepository.Implement
{
    public class apqp_dlylstRepository : BaseRepository<apqp_dlylst>
    , Iapqp_dlylstRepository
    {
    private readonly Context _context;
    public apqp_dlylstRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

