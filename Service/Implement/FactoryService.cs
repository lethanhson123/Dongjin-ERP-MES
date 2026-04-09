namespace Service.Implement
{
    public class FactoryService : BaseService<Factory, IFactoryRepository>
    , IFactoryService
    {
        private readonly IFactoryRepository _FactoryRepository;
        public FactoryService(IFactoryRepository FactoryRepository) : base(FactoryRepository)
        {
            _FactoryRepository = FactoryRepository;
        }
    }
}

