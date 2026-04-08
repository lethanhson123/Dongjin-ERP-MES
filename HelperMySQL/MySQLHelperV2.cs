namespace HelperMySQL
{
    /// <summary>
    /// tối ưu truy vấn vói Database và giảm dung lượng RAM
    /// </summary>
    public static class MySQLHelperV2
    {
        // Cache PropertyInfo theo kiểu T để không phải gọi reflection nhiều lần
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _propCache = new();

        // Cache Column → Property mapping để tăng tốc cực mạnh
        private static readonly ConcurrentDictionary<string, Dictionary<string, PropertyInfo>> _mappingCache = new();


        // -----------------------------------------------------------
        // 1. ExecuteNonQueryAsync
        // -----------------------------------------------------------
        public static async Task<int> ExecuteNonQueryAsync(
            string connectionString,
            string sql,
            params MySqlParameter[] parameters)
        {
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddRange(parameters);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync();
        }


        // -----------------------------------------------------------
        // 2. ExecuteScalarAsync<T>
        // -----------------------------------------------------------
        public static async Task<T> ExecuteScalarAsync<T>(
            string connectionString,
            string sql,
            params MySqlParameter[] parameters)
        {
            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(sql, conn);

            cmd.Parameters.AddRange(parameters);

            await conn.OpenAsync();
            object result = await cmd.ExecuteScalarAsync();

            if (result == null || result == DBNull.Value)
                return default!;

            return (T)Convert.ChangeType(result, typeof(T));
        }


        // -----------------------------------------------------------
        // 3. QueryToListAsync<T> (cực nhanh + RAM rất thấp)
        // -----------------------------------------------------------
        public static async Task<List<T>> QueryToListAsync<T>(string connectionString, string sql, params MySqlParameter[] parameters) where T : new()
        {
            var list = new List<T>();

            using var conn = new MySqlConnection(connectionString);
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddRange(parameters);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);

            // Lấy cache PropertyInfo
            var props = _propCache.GetOrAdd(typeof(T),
                t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance));

            // Cache mapping column → property
            string mapKey = $"{typeof(T).FullName}_{sql.GetHashCode()}";
            var map = _mappingCache.GetOrAdd(mapKey, _ =>
            {
                var m = new Dictionary<string, PropertyInfo>(StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string col = reader.GetName(i);

                    var prop = props.FirstOrDefault(p => p.Name.Equals(col, StringComparison.OrdinalIgnoreCase));
                    if (prop != null)
                        m[col] = prop;
                }
                return m;
            });


            while (await reader.ReadAsync())
            {
                var obj = new T();

                foreach (var kv in map)
                {
                    string columnName = kv.Key;
                    PropertyInfo prop = kv.Value;

                    var val = reader[columnName];
                    if (val == DBNull.Value) continue;

                    // Lấy kiểu thực (nullable → underlying type)
                    Type targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

                    object safeValue;

                    // Tối ưu chuyển đổi theo kiểu để bỏ Convert.ChangeType nặng
                    if (targetType == typeof(int)) safeValue = Convert.ToInt32(val);
                    else if (targetType == typeof(long)) safeValue = Convert.ToInt64(val);
                    else if (targetType == typeof(decimal)) safeValue = Convert.ToDecimal(val);
                    else if (targetType == typeof(double)) safeValue = Convert.ToDouble(val);
                    else if (targetType == typeof(float)) safeValue = Convert.ToSingle(val);
                    else if (targetType == typeof(bool)) safeValue = Convert.ToBoolean(val);
                    else if (targetType == typeof(DateTime)) safeValue = Convert.ToDateTime(val);
                    else
                        safeValue = Convert.ChangeType(val, targetType); // fallback

                    prop.SetValue(obj, safeValue);
                }

                list.Add(obj);
            }

            return list;
        }


        // -----------------------------------------------------------
        // 4. QuerySingleAsync<T> thực hiện truy vấn 1 dòng dữ liệu đầu tiên cực nhanh và RAM Thấp
        // -----------------------------------------------------------
        public static async Task<T> QuerySingleAsync<T>(
            string connectionString,
            string sql,
            params MySqlParameter[] parameters) where T : new()
        {
            var list = await QueryToListAsync<T>(connectionString, sql, parameters);
            return list.FirstOrDefault()!;
        }


        // -----------------------------------------------------------
        // 5. Transaction hỗ trợ rollback
        // -----------------------------------------------------------
        public static async Task<bool> ExecuteTransactionAsync(
            string connectionString,
            List<(string sql, MySqlParameter[] param)> commands)
        {
            using var conn = new MySqlConnection(connectionString);
            await conn.OpenAsync();

            using var trans = await conn.BeginTransactionAsync();

            try
            {
                foreach (var cmdData in commands)
                {
                    using var cmd = new MySqlCommand(cmdData.sql, conn, (MySqlTransaction)trans);
                    cmd.Parameters.AddRange(cmdData.param);
                    await cmd.ExecuteNonQueryAsync();
                }

                await trans.CommitAsync();
                return true;
            }
            catch
            {
                await trans.RollbackAsync();
                return false;
            }
        }
    }
}
