namespace MESRepository.Implement
{
    public class NGListRepository : BaseRepository<NGList>
    , INGListRepository
    {
        private readonly Context _context;
        public NGListRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}

