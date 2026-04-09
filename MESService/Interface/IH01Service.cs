namespace MESService.Interface
{
    public interface IH01Service : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> RELOAD(BaseParameter BaseParameter);
        Task<BaseResult> MC_STOPN(BaseParameter BaseParameter);
        Task<BaseResult> MC_STOP_RUN(BaseParameter BaseParameter);
        Task<BaseResult> NON_WORKER_AUTO(BaseParameter BaseParameter);
        Task<BaseResult> GetMonthlyProduction(BaseParameter BaseParameter);


    }
}