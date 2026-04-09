namespace MESRepository.Implement
{
    public class tscut_st_uphRepository : BaseRepository<tscut_st_uph>
    , Itscut_st_uphRepository
    {
    private readonly Context _context;
    public tscut_st_uphRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

