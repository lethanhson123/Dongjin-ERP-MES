namespace Service.Implement
{
    public class BaseService<T, TRepository> : Interface.IBaseService<T>
        where T : Data.Model.BaseModel
        where TRepository : Repository.Interface.IBaseRepository<T>
    {
        private readonly TRepository _repository;
        public BaseService(TRepository repository)
        {
            _repository = repository;
        }
        public virtual void InitializationSave(T model)
        {
            BaseInitialization(model);
        }
        public virtual void Initialization(T model)
        {
            BaseInitialization(model);
        }
        public virtual void BaseInitialization(T model)
        {
        }
        public virtual T SetModelByModelCheck(T Model, T ModelCheck)
        {
            if (ModelCheck != null)
            {
                if (ModelCheck.ID > 0)
                {
                    Model.ID = ModelCheck.ID;
                    Model.CreateUserID = ModelCheck.CreateUserID;
                    Model.CreateUserName = ModelCheck.CreateUserName;
                    Model.CreateUserCode = ModelCheck.CreateUserCode;
                    Model.CreateDate = ModelCheck.CreateDate;
                    Model.Active = Model.Active ?? ModelCheck.Active;
                    Model.SortOrder = Model.SortOrder ?? ModelCheck.SortOrder;
                }
            }
            return Model;
        }
        public virtual BaseResult<T> Save(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            InitializationSave(BaseParameter.BaseModel);
            if (BaseParameter.BaseModel.ID > 0)
            {
                result = Update(BaseParameter);
            }
            else
            {
                result = Add(BaseParameter);
            }
            return result;
        }
        public virtual async Task<BaseResult<T>> SaveAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            InitializationSave(BaseParameter.BaseModel);
            if (BaseParameter.BaseModel.ID > 0)
            {
                result = await UpdateAsync(BaseParameter);
            }
            else
            {
                result = await AddAsync(BaseParameter);
            }
            return result;
        }
        public virtual BaseResult<T> Copy(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            long IDOld = BaseParameter.BaseModel.ID;
            BaseParameter.BaseModel.ID = 0;
            result = Add(BaseParameter);
            if (result.BaseModel.ID > 0)
            {
            }
            return result;
        }
        public virtual async Task<BaseResult<T>> CopyAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            long IDOld = BaseParameter.BaseModel.ID;
            BaseParameter.BaseModel.ID = 0;
            result = await AddAsync(BaseParameter);
            if (result.BaseModel.ID > 0)
            {
            }
            return result;
        }
        public virtual BaseResult<T> Add(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            Initialization(BaseParameter.BaseModel);
            result.Count = _repository.Add(BaseParameter.BaseModel);
            result.BaseModel = BaseParameter.BaseModel;
            return result;
        }
        public virtual async Task<BaseResult<T>> AddAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            Initialization(BaseParameter.BaseModel);
            result.Count = await _repository.AddAsync(BaseParameter.BaseModel);
            result.BaseModel = BaseParameter.BaseModel;
            return result;
        }
        public virtual BaseResult<T> Update(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            Initialization(BaseParameter.BaseModel);
            result.Count = _repository.Update(BaseParameter.BaseModel);
            result.BaseModel = BaseParameter.BaseModel;
            return result;
        }
        public virtual async Task<BaseResult<T>> UpdateAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            Initialization(BaseParameter.BaseModel);
            result.Count = await _repository.UpdateAsync(BaseParameter.BaseModel);
            result.BaseModel = BaseParameter.BaseModel;
            return result;
        }
        public virtual BaseResult<T> Remove(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.Count = _repository.Remove(BaseParameter.ID);
            return result;
        }
        public virtual async Task<BaseResult<T>> RemoveAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.Count = await _repository.RemoveAsync(BaseParameter.ID);
            return result;
        }
        public virtual BaseResult<T> AddRange(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.Count = _repository.AddRange(BaseParameter.List);
            result.List = BaseParameter.List;
            return result;
        }
        public virtual async Task<BaseResult<T>> AddRangeAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.Count = await _repository.AddRangeAsync(BaseParameter.List);
            result.List = BaseParameter.List;
            return result;
        }
        public virtual BaseResult<T> UpdateRange(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.Count = _repository.UpdateRange(BaseParameter.List);
            result.List = BaseParameter.List;
            return result;
        }
        public virtual async Task<BaseResult<T>> UpdateRangeAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.Count = await _repository.UpdateRangeAsync(BaseParameter.List);
            result.List = BaseParameter.List;
            return result;
        }
        public virtual BaseResult<T> RemoveRange(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.Count = _repository.RemoveRange(BaseParameter.List);
            return result;
        }
        public virtual async Task<BaseResult<T>> RemoveRangeAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.Count = await _repository.RemoveRangeAsync(BaseParameter.List);
            return result;
        }
        public virtual IQueryable<T> GetByCondition(Expression<Func<T, bool>> whereCondition)
        {
            return _repository.GetByCondition(whereCondition).AsNoTracking();
        }
        public virtual BaseResult<T> GetByID(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.BaseModel = _repository.GetByID(BaseParameter.ID);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByIDAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.BaseModel = await _repository.GetByIDAsync(BaseParameter.ID);
            return result;
        }
        public virtual BaseResult<T> GetByName(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.BaseModel = _repository.GetByName(BaseParameter.Name);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByNameAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.BaseModel = await _repository.GetByNameAsync(BaseParameter.Name);
            return result;
        }
        public virtual BaseResult<T> GetByCode(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.BaseModel = _repository.GetByCode(BaseParameter.Name);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByCodeAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.BaseModel = await _repository.GetByCodeAsync(BaseParameter.Name);
            return result;
        }
        public virtual BaseResult<T> GetAllToList()
        {
            var result = new BaseResult<T>();
            result.List = _repository.GetAllToList();
            return result;
        }
        public virtual async Task<BaseResult<T>> GetAllToListAsync()
        {
            var result = new BaseResult<T>();
            result.List = await _repository.GetAllToListAsync();
            return result;
        }
        public virtual BaseResult<T> GetByIDToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = _repository.GetByIDToList(BaseParameter.ID);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByIDToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = await _repository.GetByIDToListAsync(BaseParameter.ID);
            return result;
        }
        public virtual BaseResult<T> GetByListIDToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = _repository.GetByListIDToList(BaseParameter.ListID);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByListIDToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = await _repository.GetByListIDToListAsync(BaseParameter.ListID);
            return result;
        }
        public virtual BaseResult<T> GetByCodeToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = _repository.GetByCodeToList(BaseParameter.Code);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByCodeToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = await _repository.GetByCodeToListAsync(BaseParameter.Code);
            return result;
        }
        public virtual BaseResult<T> GetByActiveToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = _repository.GetByActiveToList(BaseParameter.Active.Value);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByActiveToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = await _repository.GetByActiveToListAsync(BaseParameter.Active.Value);
            return result;
        }
        public virtual BaseResult<T> GetByParentIDToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = _repository.GetByParentIDToList(BaseParameter.ParentID.Value);
            }
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByParentIDToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await _repository.GetByParentIDToListAsync(BaseParameter.ParentID.Value);
            }
            return result;
        }
        public virtual BaseResult<T> GetByParentIDAndActiveToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = _repository.GetByParentIDAndActiveToList(BaseParameter.ParentID.Value, BaseParameter.Active.Value);
            }
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByParentIDAndActiveToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            if (BaseParameter.ParentID > 0)
            {
                result.List = await _repository.GetByParentIDAndActiveToListAsync(BaseParameter.ParentID.Value, BaseParameter.Active.Value);
            }
            return result;
        }
        public virtual BaseResult<T> GetByCompanyIDToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            if (BaseParameter.CompanyID > 0)
            {
                result.List = _repository.GetByCompanyIDToList(BaseParameter.CompanyID.Value);
            }
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByCompanyIDToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            if (BaseParameter.CompanyID > 0)
            {
                result.List = await _repository.GetByCompanyIDToListAsync(BaseParameter.CompanyID.Value);
            }
            return result;
        }
        public virtual BaseResult<T> GetByCompanyIDAndActiveToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            if (BaseParameter.CompanyID > 0)
            {
                result.List = _repository.GetByCompanyIDAndActiveToList(BaseParameter.CompanyID.Value, BaseParameter.Active.Value);
            }
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByCompanyIDAndActiveToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            if (BaseParameter.CompanyID > 0)
            {
                result.List = await _repository.GetByCompanyIDAndActiveToListAsync(BaseParameter.CompanyID.Value, BaseParameter.Active.Value);
            }
            return result;
        }
        public virtual BaseResult<T> GetBySearchStringToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = _repository.GetBySearchStringToList(BaseParameter.SearchString);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetBySearchStringToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = await _repository.GetBySearchStringToListAsync(BaseParameter.SearchString);
            return result;
        }
        public virtual BaseResult<T> GetByPageAndPageSizeToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = _repository.GetByPageAndPageSizeToList(BaseParameter.Page.Value, BaseParameter.PageSize.Value);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByPageAndPageSizeToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result.List = await _repository.GetByPageAndPageSizeToListAsync(BaseParameter.Page.Value, BaseParameter.PageSize.Value);
            return result;
        }
        public virtual BaseResult<T> ExecuteNonQueryByStoredProcedure(string storedProcedureName, params SqlParameter[] parameters)
        {
            var result = new BaseResult<T>();
            result.Message = _repository.ExecuteNonQueryByStoredProcedure(storedProcedureName, parameters);
            return result;
        }
        public virtual async Task<BaseResult<T>> ExecuteNonQueryByStoredProcedureAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            var result = new BaseResult<T>();
            result.Message = await _repository.ExecuteNonQueryByStoredProcedureAsync(storedProcedureName, parameters);
            return result;
        }
        public virtual BaseResult<T> GetByStoredProcedureToList(string storedProcedureName, params SqlParameter[] parameters)
        {
            var result = new BaseResult<T>();
            result.List = _repository.GetByStoredProcedureToList(storedProcedureName, parameters);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByStoredProcedureToListAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            var result = new BaseResult<T>();
            result.List = await _repository.GetByStoredProcedureToListAsync(storedProcedureName, parameters);
            return result;
        }
        public virtual BaseResult<T> GetAllAndEmptyToList()
        {
            var result = new BaseResult<T>();
            result = GetAllToList();
            T empty = (T)Activator.CreateInstance(typeof(T));
            result.List.Add(empty);
            return result;
        }
        public virtual async Task<BaseResult<T>> GetAllAndEmptyToListAsync()
        {
            var result = new BaseResult<T>();
            result = await GetAllToListAsync();
            T empty = (T)Activator.CreateInstance(typeof(T));
            result.List.Add(empty);
            return result;
        }
        public virtual BaseResult<T> GetByParentIDAndEmptyToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result = GetByParentIDToList(BaseParameter);
            if (BaseParameter.ParentID > 0)
            {
                T empty = (T)Activator.CreateInstance(typeof(T));
                result.List.Add(empty);
            }
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByParentIDAndEmptyToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result = await GetByParentIDToListAsync(BaseParameter);
            if (BaseParameter.ParentID > 0)
            {
                T empty = (T)Activator.CreateInstance(typeof(T));
                result.List.Add(empty);
            }
            return result;
        }
        public virtual BaseResult<T> GetByCompanyIDAndEmptyToList(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result = GetByCompanyIDToList(BaseParameter);
            if (BaseParameter.CompanyID > 0)
            {
                T empty = (T)Activator.CreateInstance(typeof(T));
                result.List.Add(empty);
            }
            return result;
        }
        public virtual async Task<BaseResult<T>> GetByCompanyIDAndEmptyToListAsync(BaseParameter<T> BaseParameter)
        {
            var result = new BaseResult<T>();
            result = await GetByCompanyIDToListAsync(BaseParameter);
            if (BaseParameter.CompanyID > 0)
            {
                T empty = (T)Activator.CreateInstance(typeof(T));
                result.List.Add(empty);
            }
            return result;
        }
    }
}
