namespace MESRepository.Implement
{
    public class tiivtr_excelRepository : BaseRepository<tiivtr_excel>
    , Itiivtr_excelRepository
    {
    private readonly Context _context;
    public tiivtr_excelRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

