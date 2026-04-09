namespace MESService.Interface
{
    public interface IC11_ER_LService : IBaseService<torderlist>
    {
        Task<BaseResult> C11_ERROR_Load(BaseParameter BaseParameter);
        Task<BaseResult> C11_ERROR_FormClosed(BaseParameter BaseParameter);
        Task<BaseResult> Button1_ClickSub(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button2_Click(BaseParameter BaseParameter);
    }
}


