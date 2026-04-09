namespace MESService.Interface
{
    public interface IH13Service : IBaseService<torderlist>
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
        Task<BaseResult> Button1_Click(BaseParameter BaseParameter);
        Task<BaseResult> Button2_Click(BaseParameter BaseParameter);
        Task<BaseResult> MachineStopRun(BaseParameter BaseParameter);
        Task<BaseResult> MachineStopNoWorker(BaseParameter BaseParameter);
        Task<BaseResult> NonWorkerAuto(BaseParameter BaseParameter);
    }
}


