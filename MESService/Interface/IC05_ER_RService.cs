namespace MESService.Interface
{
    public interface IC05_ER_RService : IBaseService<torderlist>
    {
        Task<BaseResult> C11_ERROR_Load(BaseParameter BaseParameter);
        Task<BaseResult> C11_ERROR_FormClosed(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button2_Click(BaseParameter BaseParameter);
    }
}


