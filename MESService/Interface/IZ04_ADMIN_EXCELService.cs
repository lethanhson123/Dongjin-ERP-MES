namespace MESService.Interface
{
    public interface IZ04_ADMIN_EXCELService : IBaseService<torderlist>
    {
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
        Task<BaseResult> MES_CDD(BaseParameter BaseParameter);
    }
}


