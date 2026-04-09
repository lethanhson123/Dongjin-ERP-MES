namespace MESRepository.Implement
{
    public class MaintenanceHistoryRepository : BaseRepository<MaintenanceHistory>
    , IMaintenanceHistoryRepository
    {
    private readonly Context _context;
    public MaintenanceHistoryRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

