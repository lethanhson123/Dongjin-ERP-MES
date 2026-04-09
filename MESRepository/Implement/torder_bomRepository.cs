namespace MESRepository.Implement
{
    public class torder_bomRepository : BaseRepository<torder_bom>
    , Itorder_bomRepository
    {
    private readonly Context _context;
    public torder_bomRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

