namespace MESRepository.Implement
{
    public class IQCNGCustomer2Repository : BaseRepository<IQCNGCustomer2>, IIQCNGCustomer2Repository
    {
        private readonly Context _context;

        public IQCNGCustomer2Repository(Context context) : base(context)
        {
            _context = context;
        }
    }
}