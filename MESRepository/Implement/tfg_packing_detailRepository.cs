namespace MESRepository.Implement
{
    public class tfg_packing_detailRepository : BaseRepository<tfg_packing_detail>
    , Itfg_packing_detailRepository
    {
    private readonly Context _context;
    public tfg_packing_detailRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

