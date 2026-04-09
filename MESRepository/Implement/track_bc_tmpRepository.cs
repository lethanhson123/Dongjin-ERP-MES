namespace MESRepository.Implement
{
    public class track_bc_tmpRepository : BaseRepository<track_bc_tmp>
    , Itrack_bc_tmpRepository
    {
    private readonly Context _context;
    public track_bc_tmpRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

