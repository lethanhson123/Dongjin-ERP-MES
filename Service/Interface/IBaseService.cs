namespace Service.Interface
{
    public interface IBaseService<T>
        where T : class
    {
        BaseResult<T> Save(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> SaveAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> Copy(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> CopyAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> Add(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> AddAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> Update(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> UpdateAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> Remove(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> RemoveAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> AddRange(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> AddRangeAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> UpdateRange(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> UpdateRangeAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> RemoveRange(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> RemoveRangeAsync(BaseParameter<T> BaseParameter);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> whereCondition);
        BaseResult<T> GetByID(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByIDAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByName(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByNameAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByCode(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByCodeAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetAllToList();
        Task<BaseResult<T>> GetAllToListAsync();
        BaseResult<T> GetByIDToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByIDToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByListIDToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByListIDToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByCodeToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByCodeToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByActiveToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByActiveToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByParentIDToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByParentIDToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByParentIDAndActiveToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByParentIDAndActiveToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByCompanyIDToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByCompanyIDToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByCompanyIDAndActiveToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByCompanyIDAndActiveToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetBySearchStringToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetBySearchStringToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByPageAndPageSizeToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByPageAndPageSizeToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> ExecuteNonQueryByStoredProcedure(string storedProcedureName, params SqlParameter[] parameters);
        Task<BaseResult<T>> ExecuteNonQueryByStoredProcedureAsync(string storedProcedureName, params SqlParameter[] parameters);
        BaseResult<T> GetByStoredProcedureToList(string storedProcedureName, params SqlParameter[] parameters);
        Task<BaseResult<T>> GetByStoredProcedureToListAsync(string storedProcedureName, params SqlParameter[] parameters);
        BaseResult<T> GetAllAndEmptyToList();
        Task<BaseResult<T>> GetAllAndEmptyToListAsync();
        BaseResult<T> GetByParentIDAndEmptyToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByParentIDAndEmptyToListAsync(BaseParameter<T> BaseParameter);
        BaseResult<T> GetByCompanyIDAndEmptyToList(BaseParameter<T> BaseParameter);
        Task<BaseResult<T>> GetByCompanyIDAndEmptyToListAsync(BaseParameter<T> BaseParameter);

    }
}
