namespace MESService.Interface
{
    public interface IC02_MTService : IBaseService<torderlist>
    {
        Task<BaseResult> C02_MT_Load(BaseParameter BaseParameter);
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
    }
}


