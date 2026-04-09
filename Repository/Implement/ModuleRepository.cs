namespace Repository.Implement
{
    public class ModuleRepository : BaseRepository<Data.Model.Module>
    , IModuleRepository
    {
    private readonly Context.Context.Context _context;
    public ModuleRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

