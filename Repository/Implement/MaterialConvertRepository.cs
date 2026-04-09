namespace Repository.Implement
{
    public class MaterialConvertRepository : BaseRepository<MaterialConvert>
    , IMaterialConvertRepository
    {
    private readonly Context.Context.Context _context;
    public MaterialConvertRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

