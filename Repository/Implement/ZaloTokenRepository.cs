namespace Repository.Implement
{
    public class ZaloTokenRepository : BaseRepository<ZaloToken>
    , IZaloTokenRepository
    {
        private readonly Context.Context.Context _context;
        public ZaloTokenRepository(Context.Context.Context context) : base(context)
        {
            _context = context;
        }
    }
}

