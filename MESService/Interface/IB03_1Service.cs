namespace MESService.Interface
{
    public interface IB03_1Service : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);
    }
}


