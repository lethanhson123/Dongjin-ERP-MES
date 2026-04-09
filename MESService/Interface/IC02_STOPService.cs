namespace MESService.Interface
{
    public interface IC02_STOPService : IBaseService<torderlist>
    {
        Task<BaseResult> PageLoad(BaseParameter BaseParameter);
        Task<BaseResult> SW_TIME(BaseParameter BaseParameter);
        Task<BaseResult> EW_TIME(BaseParameter BaseParameter);
        Task<BaseResult> C02_STOP_FormClosed(BaseParameter BaseParameter);
        Task<BaseResult> OPER_TIME(BaseParameter BaseParameter);
        Task<BaseResult> Button2_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> SaveMaintenanceHistory(BaseParameter BaseParameter);
    }
}


