namespace MESRepository.Implement
{
    public class AttendanceSessionRepository : BaseRepository<AttendanceSession>
    , IAttendanceSessionRepository
    {
    private readonly Context _context;
    public AttendanceSessionRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

