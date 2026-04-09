namespace MESService.Interface
{
    public interface IV03Service : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter);
        Task<BaseResult> DataGridView3_SelectionChanged(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter);
        Task<BaseResult> COMB_CHG(BaseParameter BaseParameter);
        Task<BaseResult> DataGridView4_CellClick(BaseParameter BaseParameter);
        Task<BaseResult> Button4_Click(BaseParameter BaseParameter);
        Task<BaseResult> LoadV03_4Modal(BaseParameter BaseParameter);
        Task<BaseResult> LoadComboBox1(BaseParameter BaseParameter);

    }
}