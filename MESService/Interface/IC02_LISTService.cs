namespace MESService.Interface
{
    public interface IC02_LISTService : IBaseService<torderlist>
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
        Task<BaseResult> DataGridView1_CellClick3(BaseParameter BaseParameter);
        Task<BaseResult> DB_LISECHK(BaseParameter BaseParameter);
        Task<BaseResult> TS_USER(BaseParameter BaseParameter);
        Task<BaseResult> PageLoad(BaseParameter BaseParameter);
        Task<BaseResult> SetMCLIST_CHK(BaseParameter BaseParameter);
        Task<BaseResult> RE_TIME(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> ValidateTrolley(string TrolleyCode);
    }
}


