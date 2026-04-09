namespace MESService.Implement
{
    public class BaseService<T, TRepository> : MESService.Interface.IBaseService<T>
        where T : class
        where TRepository : MESRepository.Interface.IBaseRepository<T>
    {
        private readonly TRepository _repository;
        public BaseService(TRepository repository)
        {
            _repository = repository;
        }
        public virtual void Initialization(T model)
        {
            BaseInitialization(model);
        }
        public virtual void BaseInitialization(T model)
        {
        }
        public virtual T Save(T model)
        {
            return model;
        }
        public virtual async Task<T> SaveAsync(T model)
        {
            return model;
        }
        public virtual int Add(T model)
        {
            Initialization(model);
            int result = GlobalHelper.InitializationNumber;
            result = _repository.Add(model);
            return result;
        }
        public virtual async Task<int> AddAsync(T model)
        {
            Initialization(model);
            int result = GlobalHelper.InitializationNumber;
            result = await _repository.AddAsync(model);
            return result;
        }
        public virtual int Update(T model)
        {
            Initialization(model);
            return _repository.Update(model);
        }
        public virtual async Task<int> UpdateAsync(T model)
        {
            Initialization(model);
            return await _repository.UpdateAsync(model);
        }
        public virtual int Remove(T model)
        {
            return _repository.Remove(model);
        }
        public virtual async Task<int> RemoveAsync(T model)
        {
            return await _repository.RemoveAsync(model);
        }

        public virtual IQueryable<T> GetByCondition(Expression<Func<T, bool>> whereCondition)
        {
            return _repository.GetByCondition(whereCondition);
        }
        public virtual T GetByID(int ID)
        {
            return _repository.GetByID(ID);
        }
        public virtual async Task<T> GetByIDAsync(int ID)
        {
            return await _repository.GetByIDAsync(ID);
        }
        public virtual T GetByCode(string Code)
        {
            return _repository.GetByCode(Code);
        }
        public virtual async Task<T> GetByCodeAsync(string Code)
        {
            return await _repository.GetByCodeAsync(Code);
        }
        public virtual List<T> GetAllToList()
        {
            return _repository.GetAllToList();
        }
        public virtual async Task<List<T>> GetAllToListAsync()
        {
            return await _repository.GetAllToListAsync();
        }

        public virtual List<T> GetBySearchStringToList(string searchString)
        {
            return _repository.GetBySearchStringToList(searchString);
        }
        public virtual async Task<List<T>> GetBySearchStringToListAsync(string searchString)
        {
            return await _repository.GetBySearchStringToListAsync(searchString);
        }
        public virtual List<T> GetByPageAndPageSizeToList(int page, int pageSize)
        {
            return _repository.GetByPageAndPageSizeToList(page, pageSize);
        }
        public virtual async Task<List<T>> GetByPageAndPageSizeToListAsync(int page, int pageSize)
        {
            return await _repository.GetByPageAndPageSizeToListAsync(page, pageSize);
        }
        public virtual string ExecuteNonQueryByStoredProcedure(string storedProcedureName, params SqlParameter[] parameters)
        {
            return _repository.ExecuteNonQueryByStoredProcedure(storedProcedureName, parameters);
        }
        public virtual async Task<string> ExecuteNonQueryByStoredProcedureAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            return await _repository.ExecuteNonQueryByStoredProcedureAsync(storedProcedureName, parameters);
        }
        public virtual List<T> GetByStoredProcedureToList(string storedProcedureName, params SqlParameter[] parameters)
        {
            return _repository.GetByStoredProcedureToList(storedProcedureName, parameters);
        }
        public virtual async Task<List<T>> GetByStoredProcedureToListAsync(string storedProcedureName, params SqlParameter[] parameters)
        {
            return await _repository.GetByStoredProcedureToListAsync(storedProcedureName, parameters);
        }
        public virtual List<T> GetByStoredProcedureToList(string ConnectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            return _repository.GetByStoredProcedureToList(ConnectionString, storedProcedureName, parameters);
        }
        public virtual async Task<List<T>> GetByStoredProcedureToListAsync(string ConnectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            return await _repository.GetByStoredProcedureToListAsync(ConnectionString, storedProcedureName, parameters);
        }
        public virtual List<T> GetByMySQLStoredProcedureToList(string storedProcedureName, params MySqlParameter[] parameters)
        {
            return _repository.GetByMySQLStoredProcedureToList(storedProcedureName, parameters);
        }
        public virtual async Task<List<T>> GetByMySQLStoredProcedureToListAsync(string storedProcedureName, params MySqlParameter[] parameters)
        {
            return await _repository.GetByMySQLStoredProcedureToListAsync(storedProcedureName, parameters);
        }
        public virtual List<T> GetByMySQLStoredProcedureToList(string ConnectionString, string storedProcedureName, params MySqlParameter[] parameters)
        {
            return _repository.GetByMySQLStoredProcedureToList(ConnectionString, storedProcedureName, parameters);
        }
        public virtual async Task<List<T>> GetByMySQLStoredProcedureToListAsync(string ConnectionString, string storedProcedureName, params MySqlParameter[] parameters)
        {
            return await _repository.GetByMySQLStoredProcedureToListAsync(ConnectionString, storedProcedureName, parameters);
        }
        public virtual List<T> GetByMySQLToList(string ConnectionString, string sql, params MySqlParameter[] parameters)
        {
            return _repository.GetByMySQLToList(ConnectionString, sql, parameters);
        }
        public virtual async Task<List<T>> GetByMySQLToListAsync(string ConnectionString, string sql, params MySqlParameter[] parameters)
        {
            return await _repository.GetByMySQLToListAsync(ConnectionString, sql, parameters);
        }
        public virtual List<T> GetAllAndEmptyToList()
        {
            List<T> result = new List<T>();
            T empty = (T)Activator.CreateInstance(typeof(T));
            result.Add(empty);
            List<T> list = GetAllToList();
            if (list.Count > 0)
            {
                result.AddRange(list);
            }
            return result;
        }
        public virtual async Task<List<T>> GetAllAndEmptyToListAsync()
        {
            List<T> result = new List<T>();
            try
            {
                T empty = (T)Activator.CreateInstance(typeof(T));
                result.Add(empty);
                List<T> list = await GetAllToListAsync();
                if (list.Count > 0)
                {
                    result.AddRange(list);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return result;
        }
        public virtual List<T> GetBySearchStringAndEmptyToList(string SearchString)
        {
            List<T> result = new List<T>();
            T empty = (T)Activator.CreateInstance(typeof(T));
            result.Add(empty);
            List<T> list = GetBySearchStringToList(SearchString);
            if (list.Count > 0)
            {
                result.AddRange(list);
            }
            return result;
        }
        public virtual async Task<List<T>> GetBySearchStringAndEmptyToListAsync(string SearchString)
        {
            List<T> result = new List<T>();
            try
            {
                T empty = (T)Activator.CreateInstance(typeof(T));
                result.Add(empty);
                List<T> list = await GetBySearchStringToListAsync(SearchString);
                if (list.Count > 0)
                {
                    result.AddRange(list);
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return result;
        }

        public virtual async Task<string> InsertItemsByDataTableAsync(DataTable table, string storedProcedureName)
        {
            string result = GlobalHelper.InitializationString;
            if (table != null)
            {
                if (table.Rows.Count > 0)
                {
                    int rowCount = 100;
                    int rowFrom = 0;
                    int rowTo = rowCount;
                    try
                    {
                        while (rowTo < table.Rows.Count)
                        {
                            DataTable tableSub = table.Clone();
                            tableSub.TableName = "tableSub";
                            tableSub.Clear();
                            for (int i = rowFrom; i < rowTo; i++)
                            {
                                DataRow newRow = tableSub.NewRow();
                                newRow.ItemArray = table.Rows[i].ItemArray;
                                tableSub.Rows.Add(newRow);
                            }
                            SqlParameter[] parameters =
                            {
                            new SqlParameter("@Table",tableSub),
                            };
                            result = await ExecuteNonQueryByStoredProcedureAsync(storedProcedureName, parameters);
                            if (result != "-1")
                            {
                                try
                                {
                                    foreach (DataRow row in tableSub.Rows)
                                    {
                                        for (int i = 0; i < 1; i++)
                                        {
                                            string row0 = (string)row[i];
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    result = ex.Message;
                                }
                            }
                            rowFrom = rowTo;
                            rowTo = rowTo + rowCount;
                        }
                        DataTable tableSub001 = table.Clone();
                        tableSub001.TableName = "tableSub";
                        tableSub001.Clear();
                        for (int i = rowFrom; i < table.Rows.Count; i++)
                        {
                            DataRow newRow = tableSub001.NewRow();
                            newRow.ItemArray = table.Rows[i].ItemArray;
                            tableSub001.Rows.Add(newRow);
                        }
                        SqlParameter[] parameters001 =
                        {
                            new SqlParameter("@Table",tableSub001),
                            };
                        result = await ExecuteNonQueryByStoredProcedureAsync(storedProcedureName, parameters001);
                        if (result != "-1")
                        {
                            try
                            {
                                foreach (DataRow row in tableSub001.Rows)
                                {
                                    for (int i = 0; i < 1; i++)
                                    {
                                        string row0 = (string)row[i];
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                result = ex.Message;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                    }
                }
            }
            return result;
        }
        public virtual void InitializationToExcelAsync(List<T> list, MemoryStream streamExport, string SheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(SheetName);
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "No";
                column = column + 1;

                Type temp = typeof(T);
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    workSheet.Cells[row, column].Value = pro.Name;
                    column = column + 1;
                }

                for (int i = 1; i < column; i++)
                {
                    workSheet.Cells[row, i].Style.Font.Bold = true;
                    workSheet.Cells[row, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[row, i].Style.Font.Size = 14;
                    workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                row = row + 1;

                int stt = 1;
                foreach (T item in list)
                {
                    try
                    {
                        workSheet.Cells[row, 1].Value = stt;
                        workSheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        int columnSub = 2;
                        foreach (PropertyInfo pro in temp.GetProperties())
                        {
                            workSheet.Cells[row, columnSub].Value = pro.GetValue(item);
                            columnSub = columnSub + 1;
                        }

                        for (int i = 1; i < column; i++)
                        {
                            workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                            workSheet.Cells[row, i].Style.Font.Size = 14;
                            workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            if (i > 6)
                            {
                                workSheet.Cells[row, i].Style.Font.Bold = true;
                                workSheet.Cells[row, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                            }
                        }
                        stt = stt + 1;
                        row = row + 1;
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }

                }
                for (int i = 1; i <= column; i++)
                {
                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;
        }
        public virtual string InitializationToHTMLAsync(List<T> list)
        {
            Type temp = typeof(T);
            StringBuilder Content = new StringBuilder();
            Content.AppendLine(@"<table class=""border"">");
            Content.AppendLine(@"<thead>");
            Content.AppendLine(@"<tr>");
            Content.AppendLine(@"<th>No</th>");
            foreach (PropertyInfo pro in temp.GetProperties())
            {
                Content.AppendLine(@"<th>" + pro.Name + "</th>");
            }
            Content.AppendLine(@"</tr>");
            Content.AppendLine(@"</thead>");
            Content.AppendLine(@"<tbody>");
            int stt = 0;
            foreach (T item in list)
            {
                stt = stt + 1;
                Content.AppendLine(@"<tr>");
                Content.AppendLine(@"<td>" + stt + "</td>");
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    Content.AppendLine(@"<td>" + pro.GetValue(item) + "</td>");
                }
                Content.AppendLine(@"</tr>");
            }
            Content.AppendLine(@"</tbody>");
            Content.AppendLine(@"</table>");
            return Content.ToString();
        }
    }
}
