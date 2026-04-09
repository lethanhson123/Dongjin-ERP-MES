namespace MESRepository.Implement
{
    public class SparePartScanOutRepository : BaseRepository<SparePartScanOut>, ISparePartScanOutRepository
    {
        private readonly Context _context;

        public SparePartScanOutRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}
