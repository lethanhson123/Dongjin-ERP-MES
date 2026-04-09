namespace MESService.Interface
{
    public interface IV01_1Service : IBaseService<torderlist>
    {
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
    }
}


