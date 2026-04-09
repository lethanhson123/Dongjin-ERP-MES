namespace MESRepository.Implement
{
    public class torder_bom_spst2Repository : BaseRepository<torder_bom_spst2>
    , Itorder_bom_spst2Repository
    {
    private readonly Context _context;
    public torder_bom_spst2Repository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

