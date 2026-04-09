namespace MESRepository.Implement
{
    public class FAWorkOrderRepository : BaseRepository<FAWorkOrder>, IFAWorkOrderRepository
    {
        private readonly Context _context;

        public FAWorkOrderRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}