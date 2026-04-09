namespace MESRepository.Implement
{
    public class torder_bom_spst1Repository : BaseRepository<torder_bom_spst1>
    , Itorder_bom_spst1Repository
    {
    private readonly Context _context;
    public torder_bom_spst1Repository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

