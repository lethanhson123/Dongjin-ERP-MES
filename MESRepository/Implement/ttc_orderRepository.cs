namespace MESRepository.Implement
{
    public class ttc_orderRepository : BaseRepository<ttc_order>
    , Ittc_orderRepository
    {
    private readonly Context _context;
    public ttc_orderRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

