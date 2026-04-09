namespace MESService.Interface
{
    public interface IC11_APPLICATIONService : IBaseService<torderlist>
    {
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button2_Click(BaseParameter BaseParameter);
    }
}


