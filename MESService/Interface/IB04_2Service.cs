namespace MESService.Interface
{
    public interface IB04_2Service : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
    }
}


