namespace MESRepository.Implement
{
    public class zadmin_functionRepository : BaseRepository<zadmin_function>
    , Izadmin_functionRepository
    {
    private readonly Context _context;
    public zadmin_functionRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

