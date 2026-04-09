namespace MESService.Interface
{
    public interface IV01_4Service : IBaseService<torderlist>
    {
        Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter);
    }
}


