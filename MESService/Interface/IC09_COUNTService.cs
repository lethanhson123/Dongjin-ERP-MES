namespace MESService.Interface
{
    public interface IC09_COUNTService : IBaseService<torderlist>
    {
        Task<BaseResult> C09_COUNT_Load(BaseParameter BaseParameter);
        Task<BaseResult> RadioButton2_Click(BaseParameter BaseParameter);
    }
}


