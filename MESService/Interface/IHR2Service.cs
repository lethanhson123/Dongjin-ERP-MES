namespace MESService.Interface
{
    public interface IHR2Service : IBaseService<PersonalInfo>
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
        Task<BaseResult> GetPersonalInfo(BaseParameter BaseParameter);
        Task<BaseResult> SubmitSelfRegistration(BaseParameter BaseParameter);
        Task<BaseResult> GetEmployeeJob(BaseParameter BaseParameter);
        Task<BaseResult> SaveEmployeeJob(BaseParameter BaseParameter);
        Task<BaseResult> DeleteEmployeeJob(BaseParameter BaseParameter);
        Task<BaseResult> GetEmployeeContract(BaseParameter BaseParameter);
        Task<BaseResult> SaveEmployeeContract(BaseParameter BaseParameter);
        Task<BaseResult> DeleteEmployeeContract(BaseParameter BaseParameter);
        Task<BaseResult> GetEmployeeFinance(BaseParameter BaseParameter);
        Task<BaseResult> SaveEmployeeFinance(BaseParameter BaseParameter);
        Task<BaseResult> DeleteEmployeeFinance(BaseParameter BaseParameter);
        Task<BaseResult> GetEmployeeFiles(BaseParameter BaseParameter);
        Task<BaseResult> SaveEmployeeFile(BaseParameter BaseParameter);
        Task<BaseResult> DeleteEmployeeFile(BaseParameter BaseParameter);
    }
}