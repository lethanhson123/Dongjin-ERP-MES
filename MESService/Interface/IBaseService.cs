using MySqlConnector;

namespace MESService.Interface
{
    public interface IBaseService<T>
        where T : class
    {
        T Save(T model);
        Task<T> SaveAsync(T model);       
        int Add(T model);
        Task<int> AddAsync(T model);
        int Update(T model);
        Task<int> UpdateAsync(T model);
        int Remove(T model);
        Task<int> RemoveAsync(T model);
        IQueryable<T> GetByCondition(Expression<Func<T, bool>> whereCondition);
        T GetByID(int ID);
        Task<T> GetByIDAsync(int ID);
        T GetByCode(string Code);
        Task<T> GetByCodeAsync(string Code);
        List<T> GetAllToList();
        Task<List<T>> GetAllToListAsync();      
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
        List<T> GetByMySQLStoredProcedureToList(string storedProcedureName, params MySqlParameter[] parameters);
        Task<List<T>> GetByMySQLStoredProcedureToListAsync(string storedProcedureName, params MySqlParameter[] parameters);
        List<T> GetByMySQLStoredProcedureToList(string ConnectionString, string storedProcedureName, params MySqlParameter[] parameters);
        Task<List<T>> GetByMySQLStoredProcedureToListAsync(string ConnectionString, string storedProcedureName, params MySqlParameter[] parameters);
        List<T> GetByMySQLToList(string ConnectionString, string sql, params MySqlParameter[] parameters);
        Task<List<T>> GetByMySQLToListAsync(string ConnectionString, string sql, params MySqlParameter[] parameters);
        List<T> GetAllAndEmptyToList();
        Task<List<T>> GetAllAndEmptyToListAsync();
        List<T> GetBySearchStringAndEmptyToList(string SearchString);
        Task<List<T>> GetBySearchStringAndEmptyToListAsync(string SearchString);
        Task<string> InsertItemsByDataTableAsync(DataTable table, string storedProcedureName);

    }
}
