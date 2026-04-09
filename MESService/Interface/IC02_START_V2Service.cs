namespace MESService.Interface
{
    public interface IC02_START_V2Service : IBaseService<torderlist>
    {
        Task<BaseResult> C02_start_Load(BaseParameter BaseParameter);
        Task<BaseResult> ORDER_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> DB_COUTN(BaseParameter BaseParameter);
        Task<BaseResult> OPER_TIME(BaseParameter BaseParameter);
        Task<BaseResult> SPC_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> SW_TIME(BaseParameter BaseParameter);
        Task<BaseResult> C02_FormClosed(BaseParameter BaseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);
        Task<BaseResult> PrintDocument1_PrintPage(BaseParameter BaseParameter);
        Task<BaseResult> EW_TIME(BaseParameter BaseParameter);
        Task<BaseResult> ScrapSave(BaseParameter BaseParameter);
    }
}


