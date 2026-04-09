namespace Repository.Interface
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        DbSet<T> DbSet();
        int Add(T model);
        Task<int> AddAsync(T model);
        int Update(T model);
        Task<int> UpdateAsync(T model);
        int Remove(long ID);
        Task<int> RemoveAsync(long ID);     
        int AddRange(List<T> list);
        Task<int> AddRangeAsync(List<T> list);
        int UpdateRange(List<T> list);
        Task<int> UpdateRangeAsync(List<T> list);
        int RemoveRange(List<T> list);
        Task<int> RemoveRangeAsync(List<T> list);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> whereCondition);
        T GetByID(long ID);
        Task<T> GetByIDAsync(long ID);
        T GetByName(string name);
        Task<T> GetByNameAsync(string name);
        T GetByCode(string code);
        Task<T> GetByCodeAsync(string code);      
        List<T> GetAllToList();
        Task<List<T>> GetAllToListAsync();
        List<T> GetByIDToList(long ID);
        Task<List<T>> GetByIDToListAsync(long ID);
        List<T> GetByListIDToList(List<long> ListID);
        Task<List<T>> GetByListIDToListAsync(List<long> ListID);
        List<T> GetByCodeToList(string Code);
        Task<List<T>> GetByCodeToListAsync(string Code);      
        List<T> GetByActiveToList(bool active);
        Task<List<T>> GetByActiveToListAsync(bool active);
        List<T> GetByParentIDToList(long parentID);
        Task<List<T>> GetByParentIDToListAsync(long parentID);       
        List<T> GetByParentIDAndActiveToList(long parentID, bool active);
        Task<List<T>> GetByParentIDAndActiveToListAsync(long parentID, bool active);
        List<T> GetByCompanyIDToList(long CompanyID);
        Task<List<T>> GetByCompanyIDToListAsync(long CompanyID);
        List<T> GetByCompanyIDAndActiveToList(long CompanyID, bool active);
        Task<List<T>> GetByCompanyIDAndActiveToListAsync(long CompanyID, bool active);
        List<T> GetBySearchStringToList(string searchString);
        Task<List<T>> GetBySearchStringToListAsync(string searchString);
        List<T> GetByPageAndPageSizeToList(int page, int pageSize);
        Task<List<T>> GetByPageAndPageSizeToListAsync(int page, int pageSize);
        string ExecuteNonQueryByStoredProcedure(string storedProcedureName, params SqlParameter[] parameters);
        Task<string> ExecuteNonQueryByStoredProcedureAsync(string storedProcedureName, params SqlParameter[] parameters);
        List<T> GetByStoredProcedureToList(string storedProcedureName, params SqlParameter[] parameters);
        Task<List<T>> GetByStoredProcedureToListAsync(string storedProcedureName, params SqlParameter[] parameters);
        List<T> GetByStoredProcedureToList(string ConnectionString, string storedProcedureName, params SqlParameter[] parameters);
        Task<List<T>> GetByStoredProcedureToListAsync(string ConnectionString, string storedProcedureName, params SqlParameter[] parameters);       
    }
}
