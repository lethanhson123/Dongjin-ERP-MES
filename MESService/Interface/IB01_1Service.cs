namespace MESService.Interface
{
    public interface IB01_1Service : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);       
    }
}


