namespace MESService.Interface
{
    public interface IB08_1Service : IBaseService<torderlist>
    {
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button2_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button5_Click(BaseParameter BaseParameter);
    }
}


