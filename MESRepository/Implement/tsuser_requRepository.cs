namespace MESRepository.Implement
{
    public class tsuser_requRepository : BaseRepository<tsuser_requ>
    , Itsuser_requRepository
    {
    private readonly Context _context;
    public tsuser_requRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

