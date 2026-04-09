namespace MESService.Interface
{
    public interface IX02Service : IBaseService<torderlist>
    {
        Task<BaseResult> Load();
        Task<BaseResult> LoadScanInData();
        Task<BaseResult> LoadScanOutData();
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
        Task<BaseResult> ButtonScanIn_Click(BaseParameter BaseParameter);
        Task<BaseResult> ButtonScanOut_Click(BaseParameter BaseParameter);
       
    }
}


