namespace MESService.Interface
{
    public interface IH15Service
    {
        Task<BaseResult> Load();
        Task<BaseResult> GetDashboardSummary(BaseParameter BaseParameter);
        Task<BaseResult> GetHourlyProduction(BaseParameter BaseParameter);    
        Task<BaseResult> GetDailyProduction(BaseParameter BaseParameter);    
        Task<BaseResult> GetDailyLineProduction(BaseParameter BaseParameter);  
        Task<BaseResult> GetWeeklyProduction(BaseParameter BaseParameter);   
        Task<BaseResult> GetMonthlyProduction(BaseParameter BaseParameter);
        Task<BaseResult> GetYearlyProduction(BaseParameter BaseParameter);
        Task<BaseResult> GetLatestRecords(BaseParameter BaseParameter);
        Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter);
        Task<BaseResult> GetLinePlanByDate(BaseParameter BaseParameter);
        Task<BaseResult> GetLineProductivityMetrics(BaseParameter BaseParameter);
    }
}