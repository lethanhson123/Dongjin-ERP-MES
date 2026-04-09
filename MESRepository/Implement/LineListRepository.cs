namespace MESRepository.Implement
{
    public class LineListRepository : BaseRepository<LineList>, ILineListRepository
    {
        private readonly Context _context;
        public LineListRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}

