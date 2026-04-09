namespace MESService.Interface
{
    public interface IC11_2Service : IBaseService<torderlist>
    {
        Task<BaseResult> ORDER_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> SPC_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);
    }
}


