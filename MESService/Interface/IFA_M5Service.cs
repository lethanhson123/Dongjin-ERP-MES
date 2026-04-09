namespace MESService.Interface
{
    public interface IFA_M5Service : IBaseService<AttendanceRecords>
    {
        Task<BaseResult> Load();
        Task<BaseResult> ScanIn_Click(BaseParameter BaseParameter);
        Task<BaseResult> ScanOut_Click(BaseParameter BaseParameter);
        Task<BaseResult> GetStaffList(BaseParameter BaseParameter);
        Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter);
        Task<BaseResult> StartDowntime(BaseParameter BaseParameter);
        Task<BaseResult> EndDowntime(BaseParameter BaseParameter);
        Task<BaseResult> EditShift(BaseParameter BaseParameter);
    }
}