namespace MESService.Interface
{
    public interface IWMP_PLAYService : IBaseService<tsmenu>
    {
        Task<BaseResult> Load(BaseParameter BaseParameter);
    }
}

