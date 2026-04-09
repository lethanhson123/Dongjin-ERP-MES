namespace MESService.Interface
{
    public interface IC05_SPCService : IBaseService<torderlist>
    {
        Task<BaseResult> C02_SPC_Load(BaseParameter BaseParameter);
        Task<BaseResult> ORDER_CHG(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
    }
}


