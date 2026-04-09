namespace MESRepository.Implement
{
    public class torder_bom_not_climpRepository : BaseRepository<torder_bom_not_climp>
    , Itorder_bom_not_climpRepository
    {
    private readonly Context _context;
    public torder_bom_not_climpRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

