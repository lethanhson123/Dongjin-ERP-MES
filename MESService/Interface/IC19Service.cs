namespace MESService.Interface
{
    public interface IC19Service : IBaseService<torderlist>
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
        Task<BaseResult> MAG_BACODE(BaseParameter BaseParameter);
        Task<BaseResult> DGV_LOAD(BaseParameter BaseParameter);
        Task<BaseResult> SaveLead(BaseParameter BaseParameter);
        Task<BaseResult> SaveSubPart(BaseParameter BaseParameter);
        Task<BaseResult> CheckPart(BaseParameter BaseParameter);
        Task<BaseResult> AddSubPart(BaseParameter BaseParameter);
    }
}


