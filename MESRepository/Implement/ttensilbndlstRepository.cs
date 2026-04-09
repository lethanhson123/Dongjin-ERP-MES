namespace MESRepository.Implement
{
    public class ttensilbndlstRepository : BaseRepository<ttensilbndlst>
    , IttensilbndlstRepository
    {
    private readonly Context _context;
    public ttensilbndlstRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

