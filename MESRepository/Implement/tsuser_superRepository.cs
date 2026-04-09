namespace MESRepository.Implement
{
    public class tsuser_superRepository : BaseRepository<tsuser_super>
    , Itsuser_superRepository
    {
    private readonly Context _context;
    public tsuser_superRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

