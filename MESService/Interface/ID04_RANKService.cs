namespace MESService.Interface
{
    public interface ID04_RANKService : IBaseService<torderlist>
    {
        Task<BaseResult> SELECT_SAVE(BaseParameter BaseParameter);
    }
}


