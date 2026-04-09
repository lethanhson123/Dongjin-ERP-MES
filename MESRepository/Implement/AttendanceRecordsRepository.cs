namespace MESRepository.Implement
{
    public class AttendanceRecordsRepository : BaseRepository<AttendanceRecords>, IAttendanceRecordsRepository
    {
        private readonly Context _context;

        public AttendanceRecordsRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}