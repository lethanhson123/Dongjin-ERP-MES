namespace MESService.Interface
{
    public interface IMAINService : IBaseService<tsmenu>
    {
        Task<BaseResult> Main_Shown(BaseParameter BaseParameter);
    }
}

