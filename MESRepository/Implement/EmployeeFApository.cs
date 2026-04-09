namespace MESRepository.Implement
{
    public class EmployeeFARepository : BaseRepository<EmployeeFA>, IEmployeeFARepository
    {
        private readonly Context _context;
        public EmployeeFARepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}