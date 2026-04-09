namespace MESRepository.Implement
{
    public class SparePartScanInRepository : BaseRepository<SparePartScanIn>, ISparePartScanInRepository
    {
        private readonly Context _context;

        public SparePartScanInRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}
