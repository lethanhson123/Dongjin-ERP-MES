namespace MESRepository.Implement
{
    public class ShiftTimeRepository : BaseRepository<ShiftTime>, IShiftTimeRepository
    {
        private readonly Context _context;
        public ShiftTimeRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}

