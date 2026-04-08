namespace Helper
{
    public static class SQLHelper
    {
        public static string ExecuteNonQuery(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            string result = "";
            try
            {

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters);
                        conn.Open();
                        result = cmd.ExecuteNonQuery().ToString();
                    }
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
        public static async Task<string> ExecuteNonQueryAsync(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            string result = "";
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters);
                        await conn.OpenAsync();
                        int result01 = await cmd.ExecuteNonQueryAsync();
                        result = result01.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        public static object ExecuteScalar(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    var result = cmd.ExecuteScalar();
                    return result;
                }
            }
        }

        public static async Task<object> ExecuteScalarAsync(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    await conn.OpenAsync();
                    var result = await cmd.ExecuteScalarAsync();
                    return result;
                }
            }
        }

        public static SqlDataReader ExecuteReader(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                conn.Open();
                SqlDataReader result = cmd.ExecuteReader();
                return result;

            }
        }

        public static async Task<SqlDataReader> ExecuteReaderAsync(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(parameters);
                await conn.OpenAsync();
                var result = await cmd.ExecuteReaderAsync();
                return result;
            }
        }

        public static DataTable FillDataTable(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    conn.Open();
                    adapter.Fill(dt);
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return dt;
        }

        public static async Task<DataTable> FillDataTableAsync(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            string result = "";
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    await conn.OpenAsync();
                    adapter.Fill(dt);
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return dt;
        }

        public static DataSet FillDataSet(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    conn.Open();
                    adapter.Fill(ds);
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return ds;
        }

        public static async Task<DataSet> FillDataSetAsync(string connectionString, string storedProcedureName, params SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    await conn.OpenAsync();
                    adapter.Fill(ds);
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return ds;
        }
        public static async Task<DataSet> FillDataSetBySQLAsync(string connectionString, string sql, params SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters);
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    await conn.OpenAsync();
                    adapter.Fill(ds);
                    await conn.CloseAsync();
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return ds;
        }
        public static List<T> ToList<T>(DataTable dataTable)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        /// <summary>
        /// hàm chuyển đổi thục hiện loại bỏ các ký tự đặc biệt của máy SCAN hàng tự động. những ký tự không in được sẽ được loại bỏ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <param name="clearUnPrintCharactor"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(DataTable dataTable, bool clearUnPrintCharactor)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dataTable.Rows)
            {
                T item = GetItem<T>(row);
                if (clearUnPrintCharactor)
                    CleanStringProperties(item);
                data.Add(item);
            }
            return data;
        }

        /// <summary>
        /// xóa bỏ các ký tự đặc biệt do máy Scan tạo ra khi lưu trữ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        private static void CleanStringProperties<T>(T obj)
        {
            if (obj == null) return;

            var stringProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                            .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

            foreach (var prop in stringProperties)
            {
                string val = (string)prop.GetValue(obj);
                if (!string.IsNullOrEmpty(val))
                {
                    // Loại bỏ ký tự control
                    string cleanVal = new string(val.Where(c => !char.IsControl(c)).ToArray());

                    // Trim khoảng trắng 2 đầu
                    cleanVal = cleanVal.Trim();

                    prop.SetValue(obj, cleanVal);
                }
            }
        }

        public static IEnumerable<T> ToEnumerable<T>(DataTable dataTable)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                T item = GetItem<T>(row);
                yield return item;
            }
        }

        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    try
                    {
                        if (pro.Name == column.ColumnName && dr[column.ColumnName] != DBNull.Value)
                        {
                            Type t = Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType;

                            pro.SetValue(obj, System.Convert.ChangeType(dr[column.ColumnName], t), null);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return obj;
        }

        public static T InitializationNumber<T>(T model)
        {
            if (model != null)
            {
                foreach (PropertyInfo pro in model.GetType().GetProperties())
                {
                    try
                    {
                        Type type = Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType;
                        if (type == typeof(decimal) || type == typeof(int))
                        {
                            if (pro.Name != "SortOrder")
                            {
                                if (pro.GetValue(model) == null)
                                {
                                    pro.SetValue(model, GlobalHelper.InitializationNumber, null);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return model;
        }
        public static T InitializationDateTimeName<T>(T model)
        {
            if (model != null)
            {
                foreach (PropertyInfo pro in model.GetType().GetProperties())
                {
                    try
                    {
                        Type type = Nullable.GetUnderlyingType(pro.PropertyType) ?? pro.PropertyType;
                        if (pro.Name.Contains("Date") && pro.Name.Contains("Name"))
                        {
                            if (type == typeof(string))
                            {
                                foreach (PropertyInfo proSub in model.GetType().GetProperties())
                                {
                                    try
                                    {
                                        Type typeSub = Nullable.GetUnderlyingType(proSub.PropertyType) ?? proSub.PropertyType;
                                        if (proSub.Name.Contains("Date"))
                                        {
                                            if (typeSub == typeof(DateTime))
                                            {
                                                if (pro.Name.Contains(proSub.Name))
                                                {
                                                    if (proSub.GetValue(model) != null)
                                                    {
                                                        pro.SetValue(model, GlobalHelper.GetDateNameByDateTime((DateTime)proSub.GetValue(model)), null);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return model;
        }
        public static T InitializationQuantityGAP<T>(T model)
        {
            if (model != null)
            {
                int Index = 1;
                foreach (PropertyInfo proQuantityGAP in model.GetType().GetProperties())
                {
                    string IndexName = Index.ToString();
                    if (Index < 10)
                    {
                        IndexName = "0" + IndexName;
                    }
                    string QuantityName = "Quantity" + IndexName;
                    string QuantityActualName = "QuantityActual" + IndexName;
                    string QuantityGAPName = "QuantityGAP" + IndexName;
                    if (proQuantityGAP.Name == QuantityGAPName)
                    {
                        int Quantity = 0;
                        int QuantityActual = 0;
                        int QuantityGAP = 0;
                        foreach (PropertyInfo proQuantity in model.GetType().GetProperties())
                        {
                            if (proQuantity.Name == QuantityName)
                            {
                                try
                                {
                                    if (proQuantity.GetValue(model) != null)
                                    {
                                        Quantity = (int)proQuantity.GetValue(model);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            }
                        }
                        foreach (PropertyInfo proQuantityActual in model.GetType().GetProperties())
                        {
                            if (proQuantityActual.Name == QuantityActualName)
                            {
                                try
                                {
                                    if (proQuantityActual.GetValue(model) != null)
                                    {
                                        QuantityActual = (int)proQuantityActual.GetValue(model);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                break;
                            }
                        }
                        QuantityGAP = Quantity - QuantityActual;
                        proQuantityGAP.SetValue(model, QuantityGAP, null);
                        Index = Index + 1;
                    }
                }              
            }
            return model;
        }

        public static T InitializationQuantity00<T>(T model)
        {
            if (model != null)
            {
                foreach (PropertyInfo proQuantity00 in model.GetType().GetProperties())
                {
                    if (proQuantity00.Name == "Quantity00")
                    {
                        int Index = 1;
                        int Quantity00 = 0;
                        foreach (PropertyInfo proQuantity in model.GetType().GetProperties())
                        {
                            string IndexName = Index.ToString();
                            if (Index < 10)
                            {
                                IndexName = "0" + IndexName;
                            }
                            string QuantityName = "Quantity" + IndexName;
                            if (proQuantity.Name == QuantityName)
                            {
                                try
                                {
                                    if (proQuantity.GetValue(model) != null)
                                    {
                                        Quantity00 = Quantity00 + (int)proQuantity.GetValue(model);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                Index = Index + 1;
                            }
                        }
                        proQuantity00.SetValue(model, Quantity00, null);
                    }
                }              
            }
            return model;
        }
        public static T InitializationQuantityActual00<T>(T model)
        {
            if (model != null)
            {
                foreach (PropertyInfo proQuantityActual00 in model.GetType().GetProperties())
                {
                    if (proQuantityActual00.Name == "QuantityActual00")
                    {
                        int Index = 1;
                        int QuantityActual00 = 0;
                        foreach (PropertyInfo proQuantityActual in model.GetType().GetProperties())
                        {
                            string IndexName = Index.ToString();
                            if (Index < 10)
                            {
                                IndexName = "0" + IndexName;
                            }
                            string QuantityActualName = "QuantityActual" + IndexName;
                            if (proQuantityActual.Name == QuantityActualName)
                            {
                                try
                                {
                                    if (proQuantityActual.GetValue(model) != null)
                                    {
                                        QuantityActual00 = QuantityActual00 + (int)proQuantityActual.GetValue(model);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                Index = Index + 1;
                            }
                        }
                        proQuantityActual00.SetValue(model, QuantityActual00, null);
                    }
                }              
            }
            return model;
        }
        public static T InitializationQuantityCut00<T>(T model)
        {
            if (model != null)
            {
                foreach (PropertyInfo proQuantityCut00 in model.GetType().GetProperties())
                {
                    if (proQuantityCut00.Name == "QuantityCut00")
                    {
                        int Index = 1;
                        int QuantityCut00 = 0;
                        foreach (PropertyInfo proQuantityCut in model.GetType().GetProperties())
                        {
                            string IndexName = Index.ToString();
                            if (Index < 10)
                            {
                                IndexName = "0" + IndexName;
                            }
                            string QuantityCutName = "QuantityCut" + IndexName;
                            if (proQuantityCut.Name == QuantityCutName)
                            {
                                try
                                {
                                    if (proQuantityCut.GetValue(model) != null)
                                    {
                                        QuantityCut00 = QuantityCut00 + (int)proQuantityCut.GetValue(model);
                                    }
                                }
                                catch (Exception)
                                {
                                }
                                Index = Index + 1;
                            }
                        }
                        proQuantityCut00.SetValue(model, QuantityCut00, null);
                    }
                }             
            }
            return model;
        }
    }
}
