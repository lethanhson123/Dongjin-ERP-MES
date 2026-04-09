namespace MESService.Interface
{
    public interface ISETUP_FORMService : IBaseService<torderlist>
    {
        Task<BaseResult> DB_MC_LIST(BaseParameter BaseParameter);
    }
}


