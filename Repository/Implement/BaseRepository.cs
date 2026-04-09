namespace Repository.Implement
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
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
            model.UpdateDate = DateTime.Now;
            model.CreateUserID = model.CreateUserID ?? model.UpdateUserID;
            model.UpdateUserID = model.UpdateUserID ?? model.CreateUserID;
            if (model.CreateUserID > 0)
            {
                var Membership = _context.Set<Membership>().AsNoTracking().Where(item => item.ID == model.CreateUserID).FirstOrDefault();
                if (Membership != null)
                {
                    model.CreateUserCode = Membership.UserName;
                    model.CreateUserName = Membership.Name;
                }
            }
            if (model.UpdateUserID > 0)
            {
                var Membership = _context.Set<Membership>().AsNoTracking().Where(item => item.ID == model.UpdateUserID).FirstOrDefault();
                if (Membership != null)
                {
                    model.UpdateUserCode = Membership.UserName;
                    model.UpdateUserName = Membership.Name;
                }
            }
            model.CompanyID = model.CompanyID ?? GlobalHelper.CompanyID;
            if (model.CompanyID > 0)
            {
                var Company = _context.Set<Company>().AsNoTracking().Where(item => item.ID == model.CompanyID).FirstOrDefault();
                if (Company != null)
                {
                    model.CompanyName = Company.Name;
                }
            }
            model.CreateDate = model.CreateDate ?? DateTime.Now;
            model.Active = model.Active ?? false;
            model.RowVersion = model.RowVersion ?? 1;
            if (model.SortOrder == null || model.SortOrder == 0)
            {
                model.SortOrder = 10;
                try
                {
                    int maxSortOrder = GlobalHelper.InitializationNumber;
                    //if (model.ParentID > 0)
                    //{
                    //    maxSortOrder = _context.Set<T>().AsNoTracking().Where(item => item.ParentID == model.ParentID).Select(x => x.SortOrder).Max().Value;
                    //}
                    //else
                    //{
                    //    maxSortOrder = _context.Set<T>().AsNoTracking().Select(x => x.SortOrder).Max().Value;
                    //}
                    maxSortOrder = _context.Set<T>().AsNoTracking().Select(x => x.SortOrder).Max().Value;
                    model.SortOrder = maxSortOrder + 10;
                }
                catch (Exception ex)
                {
                    string mes = ex.Message;
                }
            }
            if (!string.IsNullOrEmpty(model.CompanyName))
            {
                model.CompanyName = model.CompanyName.Trim();
            }
            if (!string.IsNullOrEmpty(model.ParentName))
            {
                model.ParentName = model.ParentName.Trim();
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                model.Name = model.Name.Trim();
            }
            if (!string.IsNullOrEmpty(model.Code))
            {
                model.Code = model.Code.Trim();
            }
            if (!string.IsNullOrEmpty(model.Note))
            {
                model.Note = model.Note.Trim();
            }
            if (!string.IsNullOrEmpty(model.Display))
            {
                model.Display = model.Display.Trim();
            }
            if (!string.IsNullOrEmpty(model.FileName))
            {
                model.FileName = model.FileName.Trim();
            }
            if (!string.IsNullOrEmpty(model.Description))
            {
                model.Description = model.Description.Trim();
            }
            foreach (PropertyInfo PropertyInfo in model.GetType().GetProperties())
            {
                try
                {
                    Type Type = Nullable.GetUnderlyingType(PropertyInfo.PropertyType) ?? PropertyInfo.PropertyType;
                    if (Type == typeof(string))
                    {
                        if (PropertyInfo.Name == "Barcode" && PropertyInfo.GetValue(model) != null)
                        {
                            PropertyInfo.SetValue(model, PropertyInfo.GetValue(model).ToString().Trim(), null);
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        public virtual int Add(T model)
        {
            int result = 0;
            try
            {
                Initialization(model);
                _context.ChangeTracker.Clear();
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
                _context.ChangeTracker.Clear();
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
                var existModel = GetByID(model.ID);
                if (existModel != null)
                {
                    existModel = model;
                    Initialization(existModel);
                    existModel.RowVersion = existModel.RowVersion + 1;
                    _context.ChangeTracker.Clear();
                    _context.Set<T>().Update(existModel);
                }
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
                var existModel = await GetByIDAsync(model.ID);
                if (existModel != null)
                {
                    existModel = model;
                    Initialization(existModel);
                    existModel.RowVersion = existModel.RowVersion + 1;
                    _context.ChangeTracker.Clear();
                    _context.Set<T>().Update(existModel);
                    result = await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual int Remove(long ID)
        {
            int result = 0;
            try
            {
                var existModel = GetByID(ID);
                if (existModel != null)
                {
                    _context.ChangeTracker.Clear();
                    _context.Set<T>().Remove(existModel);
                    result = _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual async Task<int> RemoveAsync(long ID)
        {
            int result = 0;
            try
            {
                var existModel = await GetByIDAsync(ID);
                if (existModel != null)
                {
                    _context.ChangeTracker.Clear();
                    _context.Set<T>().Remove(existModel);
                    result = await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            return result;
        }
        public virtual int AddRange(List<T> list)
        {
            int result = 0;
            if (list.Count > 0)
            {
                _context.Set<T>().AddRange(list);
                result = _context.SaveChanges();
            }
            return result;
        }
        public virtual async Task<int> AddRangeAsync(List<T> list)
        {
            int result = 0;
            if (list.Count > 0)
            {
                _context.Set<T>().AddRange(list);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public virtual int UpdateRange(List<T> list)
        {
            int result = 0;
            if (list.Count > 0)
            {
                _context.Set<T>().UpdateRange(list);
                result = _context.SaveChanges();
            }
            return result;
        }
        public virtual async Task<int> UpdateRangeAsync(List<T> list)
        {
            int result = 0;
            if (list.Count > 0)
            {
                _context.Set<T>().UpdateRange(list);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }
        public virtual int RemoveRange(List<T> list)
        {
            int result = 0;
            if (list.Count > 0)
            {
                _context.Set<T>().RemoveRange(list);
                result = _context.SaveChanges();
            }
            return result;
        }
        public virtual async Task<int> RemoveRangeAsync(List<T> list)
        {
            int result = 0;
            if (list.Count > 0)
            {
                _context.Set<T>().RemoveRange(list);
                result = await _context.SaveChangesAsync();
            }
            return result;
        }        
        public virtual IQueryable<T> GetByCondition(Expression<Func<T, bool>> whereCondition)
        {
            var result = _context.Set<T>().AsNoTracking().Where(whereCondition);
            return result;
        }
        public virtual T GetByID(long ID)
        {
            var result = (T)Activator.CreateInstance(typeof(T));
            if (ID > 0)
            {
                result = _context.Set<T>().AsNoTracking().FirstOrDefault(model => model.ID == ID);
                if (result == null)
                {
                    result = (T)Activator.CreateInstance(typeof(T));
                }
            }
            return result;
        }
        public virtual async Task<T> GetByIDAsync(long ID)
        {
            var result = (T)Activator.CreateInstance(typeof(T));
            if (ID > 0)
            {
                result = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(model => model.ID == ID);
                if (result == null)
                {
                    result = (T)Activator.CreateInstance(typeof(T));
                }
            }
            return result;
        }
        public virtual T GetByName(string name)
        {
            var result = (T)Activator.CreateInstance(typeof(T));
            if (!string.IsNullOrEmpty(name))
            {
                result = _context.Set<T>().AsNoTracking().FirstOrDefault(model => model.Name.ToLower() == name.ToLower());
                if (result == null)
                {
                    result = (T)Activator.CreateInstance(typeof(T));
                }
            }
            return result;
        }
        public virtual async Task<T> GetByNameAsync(string name)
        {
            var result = (T)Activator.CreateInstance(typeof(T));
            if (!string.IsNullOrEmpty(name))
            {
                result = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(model => model.Name.ToLower() == name.ToLower());
                if (result == null)
                {
                    result = (T)Activator.CreateInstance(typeof(T));
                }
            }
            return result;
        }
        public virtual T GetByCode(string code)
        {
            var result = _context.Set<T>().AsNoTracking().FirstOrDefault(model => model.Code == code);
            if (result == null)
            {
                result = (T)Activator.CreateInstance(typeof(T));
            }
            return result;
        }
        public virtual async Task<T> GetByCodeAsync(string code)
        {
            var result = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(model => model.Code == code);
            if (result == null)
            {
                result = (T)Activator.CreateInstance(typeof(T));
            }
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
        public virtual List<T> GetByIDToList(long ID)
        {
            var result = _context.Set<T>().AsNoTracking().Where(item => item.ID == ID).ToList();
            return result ?? new List<T>();
        }
        public virtual async Task<List<T>> GetByIDToListAsync(long ID)
        {
            var result = await _context.Set<T>().AsNoTracking().Where(item => item.ID == ID).ToListAsync();
            return result ?? new List<T>();
        }
        public virtual List<T> GetByListIDToList(List<long> ListID)
        {
            var result = _context.Set<T>().AsNoTracking().Where(item => ListID.Contains(item.ID)).ToList();
            return result ?? new List<T>();
        }
        public virtual async Task<List<T>> GetByListIDToListAsync(List<long> ListID)
        {
            var result = await _context.Set<T>().AsNoTracking().Where(item => ListID.Contains(item.ID)).ToListAsync();
            return result ?? new List<T>();
        }
        public virtual List<T> GetByCodeToList(string Code)
        {
            var result = _context.Set<T>().AsNoTracking().Where(item => item.Code == Code).ToList();
            return result ?? new List<T>();
        }
        public virtual async Task<List<T>> GetByCodeToListAsync(string Code)
        {
            var result = await _context.Set<T>().AsNoTracking().Where(item => item.Code == Code).ToListAsync();
            return result ?? new List<T>();
        }
        public virtual List<T> GetByActiveToList(bool active)
        {
            var result = _context.Set<T>().AsNoTracking().Where(item => item.Active == active).ToList();
            return result ?? new List<T>();
        }
        public virtual async Task<List<T>> GetByActiveToListAsync(bool active)
        {
            var result = await _context.Set<T>().AsNoTracking().Where(item => item.Active == active).ToListAsync();
            return result ?? new List<T>();
        }
        public virtual List<T> GetByParentIDToList(long parentID)
        {
            var result = _context.Set<T>().AsNoTracking().Where(item => item.ParentID == parentID).ToList();
            return result ?? new List<T>();
        }
        public virtual async Task<List<T>> GetByParentIDToListAsync(long parentID)
        {
            var result = await _context.Set<T>().AsNoTracking().Where(item => item.ParentID == parentID).ToListAsync();
            return result ?? new List<T>();
        }
        public virtual List<T> GetByParentIDAndActiveToList(long parentID, bool active)
        {
            var result = _context.Set<T>().AsNoTracking().Where(item => item.ParentID == parentID && item.Active == active).ToList();
            return result ?? new List<T>();
        }
        public virtual async Task<List<T>> GetByParentIDAndActiveToListAsync(long parentID, bool active)
        {
            var result = await _context.Set<T>().AsNoTracking().Where(item => item.ParentID == parentID && item.Active == active).ToListAsync();
            return result ?? new List<T>();
        }
        public virtual List<T> GetByCompanyIDToList(long CompanyID)
        {
            var result = _context.Set<T>().AsNoTracking().Where(item => item.CompanyID == CompanyID).ToList();
            return result ?? new List<T>();
        }
        public virtual async Task<List<T>> GetByCompanyIDToListAsync(long CompanyID)
        {
            var result = await _context.Set<T>().AsNoTracking().Where(item => item.CompanyID == CompanyID).ToListAsync();
            return result ?? new List<T>();
        }
        public virtual List<T> GetByCompanyIDAndActiveToList(long CompanyID, bool Active)
        {
            var result = _context.Set<T>().AsNoTracking().Where(item => item.CompanyID == CompanyID && item.Active == Active).ToList();
            return result ?? new List<T>();
        }
        public virtual async Task<List<T>> GetByCompanyIDAndActiveToListAsync(long CompanyID, bool Active)
        {
            var result = await _context.Set<T>().AsNoTracking().Where(item => item.CompanyID == CompanyID && item.Active == Active).ToListAsync();
            return result ?? new List<T>();
        }
        public virtual List<T> GetBySearchStringToList(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                return _context.Set<T>().AsNoTracking().Where(item => item.ID.ToString().Contains(searchString) || item.Name.Contains(searchString) || item.Code.Contains(searchString)).ToList();
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
                return await _context.Set<T>().AsNoTracking().Where(item => item.ID.ToString().Contains(searchString) || item.Name.Contains(searchString) || item.Code.Contains(searchString)).ToListAsync();
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

    }
}
