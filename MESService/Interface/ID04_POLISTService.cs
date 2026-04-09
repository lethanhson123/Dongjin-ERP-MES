namespace MESService.Interface
{
    public interface ID04_POLISTService : IBaseService<torderlist>
    {
        Task<BaseResult> D04_POLIST_Load(BaseParameter BaseParameter);
    }
}


