namespace MESRepository.Implement
{
    public class TaskTimeFARepository : BaseRepository<TaskTimeFA>, ITaskTimeFARepository
    {
        private readonly Context _context;

        public TaskTimeFARepository(Context context) : base(context)
        {
            _context = context;
        }
    }
}