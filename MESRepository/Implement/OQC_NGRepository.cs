namespace MESRepository.Implement
{
    public class OQC_NGRepository : BaseRepository<OQC_NG>
   , IOQC_NGRepository
    {
        private readonly Context _context;
        public OQC_NGRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}
