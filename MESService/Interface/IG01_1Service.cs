namespace MESService.Interface
{
    public interface IG01_1Service : IBaseService<torderlist>
    {
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter);
    }
}