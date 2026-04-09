namespace MESService.Interface
{
    public interface IC11_SPC_LService : IBaseService<torderlist>
    {
        Task<BaseResult> C02_SPC_Load(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
    }
}


