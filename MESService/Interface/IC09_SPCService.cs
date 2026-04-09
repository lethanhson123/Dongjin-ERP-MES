namespace MESService.Interface
{
    public interface IC09_SPCService : IBaseService<torderlist>
    {
        Task<BaseResult> C09_SPC_Load(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
    }
}


