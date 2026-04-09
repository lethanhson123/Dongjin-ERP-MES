namespace MESService.Interface
{
    public interface IMES_REPORTService : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
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
        Task<BaseResult> T1_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> T2_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> T3_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> T4_LOAD(BaseParameter BaseParameter);
    }
}


