namespace MESService.Interface
{
    public interface IC02_REPRINTService : IBaseService<torderlist>
    {
        Task<BaseResult> C02_REPRINT_Load(BaseParameter BaseParameter);
        Task<BaseResult> PrintDocument1_PrintPage(BaseParameter BaseParameter);
    }
}


