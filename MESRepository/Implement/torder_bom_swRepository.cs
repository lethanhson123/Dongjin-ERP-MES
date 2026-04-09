namespace MESRepository.Implement
{
    public class torder_bom_swRepository : BaseRepository<torder_bom_sw>
    , Itorder_bom_swRepository
    {
    private readonly Context _context;
    public torder_bom_swRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

