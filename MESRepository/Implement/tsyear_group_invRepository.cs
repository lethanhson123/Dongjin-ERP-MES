namespace MESRepository.Implement
{
    public class tsyear_group_invRepository : BaseRepository<tsyear_group_inv>
    , Itsyear_group_invRepository
    {
    private readonly Context _context;
    public tsyear_group_invRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

