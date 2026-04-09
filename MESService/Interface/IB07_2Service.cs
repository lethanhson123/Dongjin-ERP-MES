namespace MESService.Interface
{
    public interface IB07_2Service : IBaseService<torderlist>
    {
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
    }
}


