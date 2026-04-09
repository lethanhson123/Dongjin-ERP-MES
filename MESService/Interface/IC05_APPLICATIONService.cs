namespace MESService.Interface
{
    public interface IC05_APPLICATIONService : IBaseService<torderlist>
    {
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button2_Click(BaseParameter BaseParameter);
    }
}


