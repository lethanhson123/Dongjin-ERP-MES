namespace MESService.Interface
{
    public interface IAdmin6UserService : IBaseService<torderlist>
    {
        Task<BaseResult> STK_BACODE(BaseParameter BaseParameter);
        Task<BaseResult> STK_BACODE_DATA(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button3_Click(BaseParameter BaseParameter);
        Task<BaseResult> BC_KeyDown(BaseParameter BaseParameter);
    }
}