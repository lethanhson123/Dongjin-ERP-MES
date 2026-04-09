namespace MESService.Interface
{
    public interface ID04_POADDService : IBaseService<torderlist>
    {
        Task<BaseResult> TextBoxA2_KeyDown(BaseParameter BaseParameter);
        Task<BaseResult> Button3_Click(BaseParameter BaseParameter);
    }
}


