namespace Repository.Implement
{
    public class MaterialUnitCovertRepository : BaseRepository<MaterialUnitCovert>
    , IMaterialUnitCovertRepository
    {
    private readonly Context.Context.Context _context;
    public MaterialUnitCovertRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

