namespace MESRepository.Implement
{
    public class tiivtr_leadRepository : BaseRepository<tiivtr_lead>
    , Itiivtr_leadRepository
    {
    private readonly Context _context;
    public tiivtr_leadRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

