namespace MESRepository.Implement
{
    public class tspart_fileRepository : BaseRepository<tspart_file>
    , Itspart_fileRepository
    {
    private readonly Context _context;
    public tspart_fileRepository(Context context) : base(context)
    {
    _context = context;
    }
    }
    }

