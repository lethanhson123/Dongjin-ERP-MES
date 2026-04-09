namespace MESService.Interface
{
    public interface IA01_FILEService : IBaseService<torderlist>
    {
        Task<BaseResult> DGV_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
    }
}


