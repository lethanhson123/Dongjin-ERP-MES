namespace MESRepository.Implement
{
    public class tsbom_part_infRepository : BaseRepository<tsbom_part_inf>
    , Itsbom_part_infRepository
    {
    private readonly Context _context;
    public tsbom_part_infRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

