namespace MESRepository.Implement
{
    public class tsyear_group_inv_histRepository : BaseRepository<tsyear_group_inv_hist>
    , Itsyear_group_inv_histRepository
    {
    private readonly Context _context;
    public tsyear_group_inv_histRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

