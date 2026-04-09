namespace Service.Implement
{
    public class ModuleService : BaseService<Data.Model.Module, IModuleRepository>
    , IModuleService
    {
    private readonly IModuleRepository _ModuleRepository;
    public ModuleService(IModuleRepository ModuleRepository) : base(ModuleRepository)
    {
    _ModuleRepository = ModuleRepository;
    }
  
    }
    }

