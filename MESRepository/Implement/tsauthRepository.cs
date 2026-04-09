namespace MESRepository.Implement
{
    public class tsauthRepository : BaseRepository<tsauth>
    , ItsauthRepository
    {
    private readonly Context _context;
    public tsauthRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

