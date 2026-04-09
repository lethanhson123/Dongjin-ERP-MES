namespace MESRepository.Implement
{
    public class AttendanceRepository : BaseRepository<Attendance>, IAttendanceRepository
    {
        private readonly Context _context;

        public AttendanceRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}