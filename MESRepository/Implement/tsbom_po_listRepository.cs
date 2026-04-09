namespace MESRepository.Implement
{
    public class tsbom_po_listRepository : BaseRepository<tsbom_po_list>
    , Itsbom_po_listRepository
    {
    private readonly Context _context;
    public tsbom_po_listRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

