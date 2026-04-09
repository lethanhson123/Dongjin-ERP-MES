namespace MESRepository.Implement
{
    public class FAWorkOrderHistoryRepository : BaseRepository<FAWorkOrderHistory>, IFAWorkOrderHistoryRepository
    {
        private readonly Context _context;

        public FAWorkOrderHistoryRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}