namespace MESRepository.Implement
{
    public class LineAssignmentRepository : BaseRepository<LineAssignment>, ILineAssignmentRepository
    {
        private readonly Context _context;
        public LineAssignmentRepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}

