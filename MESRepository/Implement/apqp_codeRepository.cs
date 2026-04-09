namespace MESRepository.Implement
{
    public class apqp_codeRepository : BaseRepository<apqp_code>
    , Iapqp_codeRepository
    {
    private readonly Context _context;
    public apqp_codeRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

