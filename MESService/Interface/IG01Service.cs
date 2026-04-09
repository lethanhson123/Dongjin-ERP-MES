namespace MESService.Interface
{
    public interface IG01Service : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> Buttonfind_Click(BaseParameter baseParameter);
        Task<BaseResult> Buttonadd_Click(BaseParameter baseParameter);
        Task<BaseResult> Buttonsave_Click(BaseParameter baseParameter);
        Task<BaseResult> Buttondelete_Click(BaseParameter baseParameter);
        Task<BaseResult> Buttoncancel_Click(BaseParameter baseParameter);
        Task<BaseResult> Buttoninport_Click(BaseParameter baseParameter);
        Task<BaseResult> Buttonexport_Click(BaseParameter baseParameter);
        Task<BaseResult> Buttonprint_Click(BaseParameter baseParameter);
        Task<BaseResult> Buttonhelp_Click(BaseParameter baseParameter);
        Task<BaseResult> Buttonclose_Click(BaseParameter baseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button2_Click(BaseParameter BaseParameter);
    }
}