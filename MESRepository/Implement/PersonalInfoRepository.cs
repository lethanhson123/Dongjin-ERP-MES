namespace MESRepository.Implement
{
    public class PersonalInfoRepository : BaseRepository<PersonalInfo>
        , IPersonalInfoRepository
    {
        private readonly Context _context;

        public PersonalInfoRepository(Context context) : base(context)
        {
            _context = context;
        }

        
    }
}