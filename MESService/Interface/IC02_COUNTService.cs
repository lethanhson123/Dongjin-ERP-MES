namespace MESService.Interface
{
    public interface IC02_COUNTService : IBaseService<torderlist>
    {
        Task<BaseResult> C02_COUNT_Load(BaseParameter BaseParameter);
        Task<BaseResult> RadioButton2_Click(BaseParameter BaseParameter);
    }
}


