namespace MESService.Interface
{
    public interface IB07_3Service : IBaseService<torderlist>
    {
        Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
    }
}


