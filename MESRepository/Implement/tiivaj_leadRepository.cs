namespace MESRepository.Implement
{
    public class tiivaj_leadRepository : BaseRepository<tiivaj_lead>
    , Itiivaj_leadRepository
    {
    private readonly Context _context;
    public tiivaj_leadRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

