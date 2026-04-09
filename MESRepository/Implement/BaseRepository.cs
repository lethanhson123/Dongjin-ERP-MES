namespace MESRepository.Implement
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly DbContext _context;

        public BaseRepository(DbContext context)
        {
            _context = context;
        }
        public virtual DbSet<T> DbSet()
        {
            return _context.Set<T>();
        }
        public virtual void Initialization(T model)
        {
        }
        public virtual int Add(T model)
        {
            int result = 0;
            try
            {
                Initialization(model);
                _context.Set<T>().Add(model);
                result = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<int> AddAsync(T model)
        {
            int result = 0;
            try
            {
                Initialization(model);
                _context.Set<T>().Add(model);
                result = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual int Update(T model)
        {
            int result = 0;
            try
            {
                _context.Set<T>().Update(model);
                result = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<int> UpdateAsync(T model)
        {
            int result = 0;
            try
            {
                _context.Set<T>().Update(model);
                result = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual int Remove(T model)
        {
            int result = 0;
            try
            {
                _context.Set<T>().Remove(model);
                result = _context.SaveChanges();
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<int> RemoveAsync(T model)
        {
            int result = 0;
            try
            {
                _context.Set<T>().Remove(model);
                result = await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual IQueryable<T> GetByCondition(Expression<Func<T, bool>> whereCondition)
        {
            var result = _context.Set<T>().AsNoTracking().Where(whereCondition);
            return result;
        }
        public virtual T GetByID(int ID)
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            return result;
        }
        public virtual async Task<T> GetByIDAsync(int ID)
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            return result;
        }
        public virtual T GetByCode(string Code)
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            return result;
        }
        public virtual async Task<T> GetByCodeAsync(string Code)
        {
            T result = (T)Activator.CreateInstance(typeof(T));
            return result;
        }
        public virtual List<T> GetAllToList()
        {
            var result = _context.Set<T>().AsNoTracking().ToList();
            return result ?? new List<T>();
        }
        public virtual async Task<List<T>> GetAllToListAsync()
        {
            var result = await _context.Set<T>().AsNoTracking().ToListAsync();
            return result ?? new List<T>();
        }

        public virtual List<T> GetBySearchStringToList(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return _context.Set<T>().AsNoTracking().Where(item => 1 == 1).ToList();
            }
            else
            {
                return new List<T>();
            }
        }
        public virtual async Task<List<T>> GetBySearchStringToListAsync(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return await _context.Set<T>().AsNoTracking().Where(item => 1 == 1).ToListAsync();
            }
            else
            {
                return new List<T>();
            }
        }
        public virtual List<T> GetByPageAndPageSizeToList(int page, int pageSize)
        {
            var result = _context.Set<T>().AsNoTracking().Skip(page * pageSize).Take(pageSize).ToList();
            return result;
        }
        public virtual async Task<List<T>> GetByPageAndPageSizeToListAsync(int page, int pageSize)
        {
            var result = await _context.Set<T>().AsNoTracking().Skip(page * pageSize).Take(pageSize).ToListAsync();
            return result;
        }

        public virtual string ExecuteNonQueryByStoredProcedure(string storedProcedureName, params SqlParameter[] parameters)
        {
            var result = GlobalHelper.InitializationString;
            try
            {
                result = SQLHelper.ExecuteNonQuery(_context.Database.GetConnectionString(), storedProcedureName, parameters);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual async Task<string> ExecuteNonQueryByStoredProcedureAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            var result = GlobalHelper.InitializationString;
            try
            {
                result = await SQLHelper.ExecuteNonQueryAsync(_context.Database.GetConnectionString(), storedProcedureName, parameters);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual List<T> GetByStoredProcedureToList(string storedProcedureName, params SqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            try
            {
                DataSet ds = SQLHelper.FillDataSet(_context.Database.GetConnectionString(), storedProcedureName, parameters);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.AddRange(SQLHelper.ToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual async Task<List<T>> GetByStoredProcedureToListAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            try
            {
                DataSet ds = await SQLHelper.FillDataSetAsync(_context.Database.GetConnectionString(), storedProcedureName, parameters);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.AddRange(SQLHelper.ToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual List<T> GetByStoredProcedureToList(string ConnectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            try
            {
                DataSet ds = SQLHelper.FillDataSet(ConnectionString, storedProcedureName, parameters);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.AddRange(SQLHelper.ToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual async Task<List<T>> GetByStoredProcedureToListAsync(string ConnectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            try
            {
                DataSet ds = await SQLHelper.FillDataSetAsync(ConnectionString, storedProcedureName, parameters);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.AddRange(SQLHelper.ToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual List<T> GetByMySQLStoredProcedureToList(string storedProcedureName, params MySqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            try
            {
                DataSet ds = MySQLHelper.FillDataSet(_context.Database.GetConnectionString(), storedProcedureName, parameters);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.AddRange(SQLHelper.ToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual async Task<List<T>> GetByMySQLStoredProcedureToListAsync(string storedProcedureName, params MySqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            try
            {
                DataSet ds = await MySQLHelper.FillDataSetAsync(_context.Database.GetConnectionString(), storedProcedureName, parameters);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.AddRange(SQLHelper.ToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual List<T> GetByMySQLStoredProcedureToList(string ConnectionString, string storedProcedureName, params MySqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            try
            {
                DataSet ds = MySQLHelper.FillDataSet(ConnectionString, storedProcedureName, parameters);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.AddRange(SQLHelper.ToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual async Task<List<T>> GetByMySQLStoredProcedureToListAsync(string ConnectionString, string storedProcedureName, params MySqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            try
            {
                DataSet ds = await MySQLHelper.FillDataSetAsync(ConnectionString, storedProcedureName, parameters);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.AddRange(SQLHelper.ToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual List<T> GetByMySQLToList(string ConnectionString, string sql, params MySqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            try
            {
                DataSet ds = MySQLHelper.FillDataSetBySQL(ConnectionString, sql, parameters);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.AddRange(SQLHelper.ToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
        public virtual async Task<List<T>> GetByMySQLToListAsync(string ConnectionString, string sql, params MySqlParameter[] parameters)
        {
            List<T> result = new List<T>();
            try
            {
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(ConnectionString, sql, parameters);
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.AddRange(SQLHelper.ToList<T>(dt));
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return result;
        }
    }
}
