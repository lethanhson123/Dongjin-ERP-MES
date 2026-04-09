namespace Repository.Implement
{
    public class ReportRepository : BaseRepository<Report>
    , IReportRepository
    {
    private readonly Context.Context.Context _context;
    public ReportRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

