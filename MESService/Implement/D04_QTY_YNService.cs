namespace MESService.Implement
{
    public class D04_QTY_YNService : BaseService<torderlist, ItorderlistRepository>
    , ID04_QTY_YNService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D04_QTY_YNService(ItorderlistRepository torderlistRepository

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

