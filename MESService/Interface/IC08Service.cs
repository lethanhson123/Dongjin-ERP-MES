namespace MESService.Interface
{
    public interface IC08Service : IBaseService<torderlist>
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
        Task<BaseResult> SearchLead(BaseParameter BaseParameter);
        Task<BaseResult> PrintBarcode(BaseParameter BaseParameter);
        Task<BaseResult> PrintBarcodeOnly(BaseParameter BaseParameter);
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button4_Click(BaseParameter BaseParameter);
        Task<BaseResult> TextBox4_KeyDown(BaseParameter BaseParameter);
       

    }
}


