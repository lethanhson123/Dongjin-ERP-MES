namespace MESService.Interface
{
    public interface IV01_3Service : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> ComboBox1_SelectedIndexChanged(BaseParameter BaseParameter);
    }
}


