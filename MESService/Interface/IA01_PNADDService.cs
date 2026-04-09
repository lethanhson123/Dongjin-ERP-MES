namespace MESService.Interface
{
    public interface IA01_PNADDService : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
    }
}


