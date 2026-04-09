namespace Repository.Implement
{
    public class ZaloZNSRepository : BaseRepository<ZaloZNS>
    , IZaloZNSRepository
    {
        private readonly Context.Context.Context _context;
        public ZaloZNSRepository(Context.Context.Context context) : base(context)
        {
            _context = context;
        }
    }
}

