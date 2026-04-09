namespace MESRepository.Implement
{
    public class torder_spcRepository : BaseRepository<torder_spc>
    , Itorder_spcRepository
    {
    private readonly Context _context;
    public torder_spcRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

