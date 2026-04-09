namespace MESRepository.Implement
{
    public class ROTestLogRepository : BaseRepository<ROTestLog>
   , IROTestLogRepository
    {
        private readonly Context _context;
        public ROTestLogRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}
