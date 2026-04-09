namespace MESService.Interface
{
    public interface ID04_PLT_PRNTService : IBaseService<torderlist>
    {
        Task<BaseResult> D04_PLT_PRNT_Load(BaseParameter BaseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);
    }
}


