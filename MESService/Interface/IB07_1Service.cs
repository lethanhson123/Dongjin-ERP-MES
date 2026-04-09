namespace MESService.Interface
{
    public interface IB07_1Service : IBaseService<torderlist>
    {
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
    }
}


