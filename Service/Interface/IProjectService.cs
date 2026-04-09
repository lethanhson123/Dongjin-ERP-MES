namespace Service.Interface
{
    public interface IProjectService : IBaseService<Project>
    {
        Task<BaseResult<Project>> GetByCompanyID_DateBegin_DateEnd_SearchStringToListAsync(BaseParameter<Project> BaseParameter);
    }
}

