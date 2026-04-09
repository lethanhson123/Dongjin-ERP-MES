namespace MESRepository.Implement
{
    public class PartSpareRepository : BaseRepository<PartSpare>
    , IPartSpareRepository
    {
    private readonly Context _context;
    public PartSpareRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

