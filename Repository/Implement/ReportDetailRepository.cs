namespace Repository.Implement
{
    public class ReportDetailRepository : BaseRepository<ReportDetail>
    , IReportDetailRepository
    {
    private readonly Context.Context.Context _context;
    public ReportDetailRepository(Context.Context.Context context) : base(context)
    {
    _context = context;
    }
    }
    }

