namespace MESService.Interface
{
    public interface IB04Service : IBaseService<torderlist>
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
        Task<BaseResult> BTBCCHK_Click(BaseParameter BaseParameter);
        Task<BaseResult> BTBCCHK_ClickSub(BaseParameter BaseParameter);
        Task<BaseResult> GetSHEETNOToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> GetFIFOReasonToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> GettmbrcdAndtmbrcd_hisToListAsync(BaseParameter BaseParameter);
        Task<BaseResult> Gettmbrcd_hisByBARCD_IDXToListAsync(BaseParameter BaseParameter);
    }
}


