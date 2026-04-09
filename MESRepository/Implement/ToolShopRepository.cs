namespace MESRepository.Implement
{
    public class ToolShopRepository : BaseRepository<ToolShop>
    , IToolShopRepository
    {
        private readonly Context _context;
        public ToolShopRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}

