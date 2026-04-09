namespace MESService.Interface
{
    public interface IH06Service : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
        Task<BaseResult> Dgv_reload(string startDate = null, string endDate = null);
        Task<BaseResult> DB_STOP();
        Task<BaseResult> MC_STOP_RUN(BaseParameter BaseParameter);
        Task<BaseResult> MC_STOPN(BaseParameter BaseParameter);
        Task<BaseResult> GetMonthlyChart(BaseParameter BaseParameter);
    }
}