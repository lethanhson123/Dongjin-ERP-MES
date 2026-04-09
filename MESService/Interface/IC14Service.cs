namespace MESService.Interface
{
    public interface IC14Service : IBaseService<torderlist>
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
        Task<BaseResult> CB_FCTRY_LINE(BaseParameter BaseParameter);
        Task<BaseResult> COMLIST_LINE_1(BaseParameter BaseParameter);
        Task<BaseResult> COMLIST_LINE_2(BaseParameter BaseParameter);
        Task<BaseResult> COMLIST_LINE_3(BaseParameter BaseParameter);
        Task<BaseResult> DataT2DGV_LOAD(BaseParameter BaseParameter);
    }
}


