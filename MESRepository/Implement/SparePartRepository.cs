namespace MESRepository.Implement
{
    public class SparePartRepository : BaseRepository<SparePart>, ISparePartRepository
    {
        private readonly Context _context;

        public SparePartRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}
