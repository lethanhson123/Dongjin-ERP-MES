namespace Repository.Implement
{
    public class MaterialReplaceRepository : BaseRepository<MaterialReplace>
    , IMaterialReplaceRepository
    {
    private readonly Context.Context.Context _context;
    public MaterialReplaceRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

