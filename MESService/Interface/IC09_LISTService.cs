namespace MESService.Interface
{
    public interface IC09_LISTService : IBaseService<torderlist>
    {
        Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter);
        Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter);
        Task<BaseResult> C09_LIST_Load(BaseParameter BaseParameter);
        Task<BaseResult> MC_LIST(BaseParameter BaseParameter);
        Task<BaseResult> DB_LISECHK(BaseParameter BaseParameter);
        Task<BaseResult> TS_USER(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click_1(BaseParameter BaseParameter);
        Task<BaseResult> DataGridView1_SelectionChanged(BaseParameter BaseParameter);
        Task<BaseResult> GenerateBarcode_MT(BaseParameter BaseParameter);
    }
}


