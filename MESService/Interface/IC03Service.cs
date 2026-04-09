namespace MESService.Interface
{
    public interface IC03Service : IBaseService<torderlist>
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
        Task<BaseResult> DATA_ADD(BaseParameter BaseParameter);
        Task<BaseResult> DATA_ADD_IN(BaseParameter BaseParameter);
        Task<BaseResult> Buttonfind_Click_IN(BaseParameter BaseParameter);
        Task<BaseResult> ReInputBarcode(BaseParameter BaseParameter);
        Task<BaseResult> DGV_CellSEL(BaseParameter BaseParameter);
        Task<BaseResult> GET_LONG_TERM_DETAIL(BaseParameter BaseParameter);
        Task<BaseResult> MAG_BACODE(BaseParameter BaseParameter);
        Task<BaseResult> DGV_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> CheckPartExists(BaseParameter BaseParameter);
        Task<BaseResult> DATA_ADD_RE_IN(BaseParameter BaseParameter);
        Task<BaseResult> GetBarcodesByTrolley(BaseParameter BaseParameter);
        Task<BaseResult> InputTrolleyBarcodes(BaseParameter BaseParameter);
        Task<BaseResult> CategoryDepartmentGetByCompanyID_ActiveToListAsync(BaseParameter BaseParameter);

    }
}


