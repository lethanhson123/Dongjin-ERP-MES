namespace MESRepository.Implement
{
    public class DowntimeRecordsRepository : BaseRepository<DowntimeRecords>
    , IDowntimeRecordsRepository
    {
    private readonly Context _context;
    public DowntimeRecordsRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

