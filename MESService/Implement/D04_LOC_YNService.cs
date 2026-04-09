namespace MESService.Implement
{
    public class D04_LOC_YNService : BaseService<torderlist, ItorderlistRepository>
    , ID04_LOC_YNService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D04_LOC_YNService(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }       
       
    }
}

