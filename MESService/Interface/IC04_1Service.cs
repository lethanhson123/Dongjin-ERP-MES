namespace MESService.Interface
{
    public interface IC04_1Service : IBaseService<torderlist>
    {
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
    }
}


