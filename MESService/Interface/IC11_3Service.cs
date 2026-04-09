namespace MESService.Interface
{
    public interface IC11_3Service : IBaseService<torderlist>
    {
        Task<BaseResult> ORDER_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> ORDER_COUNT(BaseParameter BaseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);
    }
}


