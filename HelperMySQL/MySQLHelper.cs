namespace HelperMySQL
{
    public static class MySQLHelper
    {

        public static string ExecuteNonQuery(string connectionString, string sql, params MySqlParameter[] parameters)
        {
            string result = "";
            try
            {

                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(parameters);
                        conn.Open();
                        result = cmd.ExecuteNonQuery().ToString();
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
        public static async Task<string> ExecuteNonQueryAsync(string connectionString, string sql, params MySqlParameter[] parameters)
        {
            string result = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(parameters);
                        await conn.OpenAsync();
                        int result01 = await cmd.ExecuteNonQueryAsync();
                        result = result01.ToString();
                        await conn.CloseAsync();
                    }
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
        public static DataSet FillDataSetBySQL(string connectionString, string sql, params MySqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters);
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    conn.Open();
                    adapter.Fill(ds);
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return ds;
        }
        public static async Task<DataSet> FillDataSetBySQLAsync(string connectionString, string sql, params MySqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddRange(parameters);
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
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
        public static DataSet FillDataSet(string connectionString, string storedProcedureName, params MySqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                using (MySqlCommand cmd = new MySqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    adapter.SelectCommand = cmd;
                    conn.Open();
                    adapter.Fill(ds);
                    conn.Close();
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return ds;
        }

        public static async Task<DataSet> FillDataSetAsync(string connectionString, string storedProcedureName, params MySqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            string result = "";
            try
            {
                MySqlConnection conn = new MySqlConnection(connectionString);
                using (MySqlCommand cmd = new MySqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddRange(parameters);
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
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
        public static async Task<string> ExecuteScalarAsync(string connectionString, string sql, params MySqlParameter[] parameters)
        {
            string result = "";
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddRange(parameters);
                        await conn.OpenAsync();
                        object resultObj = await cmd.ExecuteScalarAsync();
                        result = resultObj?.ToString() ?? "";
                        await conn.CloseAsync();
                    }
                }
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }

        /// <summary>
        /// hàm chạy sql hỗ trợ rollback nếu truy vấn thất bại
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqlCommands"></param>
        /// <returns></returns>
        public static async Task<string> ExecuteTransactionAsync(string connectionString, List<string> sqlCommands)
        {
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    await conn.OpenAsync();
                    using (var trans = await conn.BeginTransactionAsync())
                    {
                        using (var cmd = new MySqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.Transaction = (MySqlTransaction)trans;

                            foreach (var sql in sqlCommands)
                            {
                                cmd.CommandText = sql;
                                await cmd.ExecuteNonQueryAsync();
                            }
                        }
                        await trans.CommitAsync();
                    }
                }
                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public static async Task<string> ERPSyncAsync(string connectionString)
        {
            string result = "";
            try
            {
                string sql = @"UPDATE WarehouseInput JOIN WarehouseInputDetailBarcode ON WarehouseInput.ID=WarehouseInputDetailBarcode.ParentID SET WarehouseInputDetailBarcode.ParentName=WarehouseInput.Code WHERE WarehouseInputDetailBarcode.ParentID>0 AND WarehouseInputDetailBarcode.ParentName IS NULL AND WarehouseInput.Active=1 AND WarehouseInputDetailBarcode.Active=1";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseOutput JOIN WarehouseOutputDetailBarcode ON WarehouseOutput.ID=WarehouseOutputDetailBarcode.ParentID SET WarehouseOutputDetailBarcode.ParentName=WarehouseOutput.Code WHERE WarehouseOutputDetailBarcode.ParentID>0 AND WarehouseOutputDetailBarcode.ParentName IS NULL AND WarehouseOutput.Active=1 AND WarehouseOutputDetailBarcode.Active=1";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseInputDetail JOIN Material ON WarehouseInputDetail.MaterialID=Material.ID SET WarehouseInputDetail.FileName=Material.CategoryLineName WHERE WarehouseInputDetail.FileName IS NULL AND Material.CategoryLineName IS NOT NULL AND WarehouseInputDetail.Active=1";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseOutputDetail JOIN Material ON WarehouseOutputDetail.MaterialID=Material.ID SET WarehouseOutputDetail.FileName=Material.CategoryLineName WHERE WarehouseOutputDetail.FileName IS NULL AND Material.CategoryLineName IS NOT NULL AND WarehouseOutputDetail.Active=1";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseInputDetailBarcode JOIN Material ON WarehouseInputDetailBarcode.MaterialID=Material.ID SET WarehouseInputDetailBarcode.FileName=Material.CategoryLineName WHERE WarehouseInputDetailBarcode.FileName IS NULL AND Material.CategoryLineName IS NOT NULL AND WarehouseInputDetailBarcode.Active=1";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseOutputDetailBarcode JOIN Material ON WarehouseOutputDetailBarcode.MaterialID=Material.ID SET WarehouseOutputDetailBarcode.FileName=Material.CategoryLineName WHERE WarehouseOutputDetailBarcode.FileName IS NULL AND Material.CategoryLineName IS NOT NULL AND WarehouseOutputDetailBarcode.Active=1";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseInventory JOIN Material ON WarehouseInventory.ParentID=Material.ID SET WarehouseInventory.ParentName=Material.Code WHERE WarehouseInventory.ParentID>0 AND WarehouseInventory.ParentName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseStock JOIN Material ON WarehouseStock.ParentID=Material.ID SET WarehouseStock.ParentName=Material.Code WHERE WarehouseStock.ParentID>0 AND WarehouseStock.ParentName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseInputDetailBarcode SET MaterialName=SUBSTRING_INDEX(Barcode, '$', 1) WHERE MaterialName IS NULL AND Active=1";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseOutputDetailBarcode SET MaterialName=SUBSTRING_INDEX(Barcode, '$', 1) WHERE MaterialName IS NULL AND Active=1";
                await ExecuteNonQueryAsync(connectionString, sql);

            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
        public static async Task<string> ERPSyncAsync01(string connectionString)
        {
            string result = "";
            try
            {
                string sql = @"UPDATE WarehouseInputDetail JOIN MaterialConvert ON WarehouseInputDetail.MaterialName=MaterialConvert.Code SET WarehouseInputDetail.MaterialID=MaterialConvert.ParentID WHERE WarehouseInputDetail.MaterialID IS NULL OR WarehouseInputDetail.MaterialID=0";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseInputDetailBarcode JOIN MaterialConvert ON WarehouseInputDetailBarcode.MaterialName=MaterialConvert.Code SET WarehouseInputDetailBarcode.MaterialID=MaterialConvert.ParentID WHERE WarehouseInputDetailBarcode.MaterialID IS NULL OR WarehouseInputDetailBarcode.MaterialID=0";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseOutputDetail JOIN MaterialConvert ON WarehouseOutputDetail.MaterialName=MaterialConvert.Code SET WarehouseOutputDetail.MaterialID=MaterialConvert.ParentID WHERE WarehouseOutputDetail.MaterialID IS NULL OR WarehouseOutputDetail.MaterialID=0";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseOutputDetailBarcode JOIN MaterialConvert ON WarehouseOutputDetailBarcode.MaterialName=MaterialConvert.Code SET WarehouseOutputDetailBarcode.MaterialID=MaterialConvert.ParentID WHERE WarehouseOutputDetailBarcode.MaterialID IS NULL OR WarehouseOutputDetailBarcode.MaterialID=0";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseInputDetail JOIN Material ON WarehouseInputDetail.MaterialName=Material.Code SET WarehouseInputDetail.MaterialID=Material.ID WHERE WarehouseInputDetail.MaterialID IS NULL OR WarehouseInputDetail.MaterialID=0";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseInputDetailBarcode JOIN Material ON WarehouseInputDetailBarcode.MaterialName=Material.Code SET WarehouseInputDetailBarcode.MaterialID=Material.ID WHERE WarehouseInputDetailBarcode.MaterialID IS NULL OR WarehouseInputDetailBarcode.MaterialID=0";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseOutputDetail JOIN Material ON WarehouseOutputDetail.MaterialName=Material.Code SET WarehouseOutputDetail.MaterialID=Material.ID WHERE WarehouseOutputDetail.MaterialID IS NULL OR WarehouseOutputDetail.MaterialID=0";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseOutputDetailBarcode JOIN Material ON WarehouseOutputDetailBarcode.MaterialName=Material.Code SET WarehouseOutputDetailBarcode.MaterialID=Material.ID WHERE WarehouseOutputDetailBarcode.MaterialID IS NULL OR WarehouseOutputDetailBarcode.MaterialID=0";
                await ExecuteNonQueryAsync(connectionString, sql);
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
        public static async Task<string> ERPSyncAsync02(string connectionString)
        {
            string result = "";
            try
            {
                string sql = @"update WarehouseInputDetailBarcode JOIN WarehouseInput ON WarehouseInputDetailBarcode.ParentID=WarehouseInput.ID SET WarehouseInputDetailBarcode.CategoryDepartmentID=WarehouseInput.CustomerID WHERE WarehouseInputDetailBarcode.ParentID>0 AND WarehouseInputDetailBarcode.CategoryDepartmentID IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"update WarehouseOutputDetailBarcode JOIN WarehouseOutput ON WarehouseOutputDetailBarcode.ParentID=WarehouseOutput.ID SET WarehouseOutputDetailBarcode.CategoryDepartmentID=WarehouseOutput.SupplierID WHERE WarehouseOutputDetailBarcode.ParentID>0 AND WarehouseOutputDetailBarcode.CategoryDepartmentID IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"update WarehouseInputDetailBarcode set DateScan=UpdateDate WHERE DateScan IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
        public static string ERPSync02(string connectionString)
        {
            string result = "";
            try
            {
                string sql = @"update WarehouseInputDetailBarcode JOIN WarehouseInput ON WarehouseInputDetailBarcode.ParentID=WarehouseInput.ID SET WarehouseInputDetailBarcode.CategoryDepartmentID=WarehouseInput.CustomerID WHERE WarehouseInputDetailBarcode.ParentID>0 AND WarehouseInputDetailBarcode.CategoryDepartmentID IS NULL";
                ExecuteNonQuery(connectionString, sql);
                sql = @"update WarehouseOutputDetailBarcode JOIN WarehouseOutput ON WarehouseOutputDetailBarcode.ParentID=WarehouseOutput.ID SET WarehouseOutputDetailBarcode.CategoryDepartmentID=WarehouseOutput.SupplierID WHERE WarehouseOutputDetailBarcode.ParentID>0 AND WarehouseOutputDetailBarcode.CategoryDepartmentID IS NULL";
                ExecuteNonQuery(connectionString, sql);
                sql = @"update WarehouseInputDetailBarcode set DateScan=UpdateDate WHERE DateScan IS NULL";
                ExecuteNonQuery(connectionString, sql);
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
        public static async Task<string> ERPSyncAsync03(string connectionString)
        {
            string result = "";
            try
            {
                string sql = @"UPDATE WarehouseOutputDetail JOIN Material ON WarehouseOutputDetail.MaterialID=Material.ID SET WarehouseOutputDetail.CategoryFamilyName=Material.CategoryFamilyName WHERE WarehouseOutputDetail.CategoryFamilyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"UPDATE WarehouseOutputDetailBarcode JOIN Material ON WarehouseOutputDetailBarcode.MaterialID=Material.ID SET WarehouseOutputDetailBarcode.CategoryFamilyName=Material.CategoryFamilyName WHERE WarehouseOutputDetailBarcode.CategoryFamilyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"UPDATE WarehouseInputDetail JOIN Material ON WarehouseInputDetail.MaterialID=Material.ID SET WarehouseInputDetail.CategoryFamilyName=Material.CategoryFamilyName WHERE WarehouseInputDetail.CategoryFamilyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"UPDATE WarehouseInputDetailBarcode JOIN Material ON WarehouseInputDetailBarcode.MaterialID=Material.ID SET WarehouseInputDetailBarcode.CategoryFamilyName=Material.CategoryFamilyName WHERE WarehouseInputDetailBarcode.CategoryFamilyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"UPDATE WarehouseRequestDetail JOIN Material ON WarehouseRequestDetail.MaterialID=Material.ID SET WarehouseRequestDetail.CategoryFamilyName=Material.CategoryFamilyName WHERE WarehouseRequestDetail.CategoryFamilyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"UPDATE InvoiceInputDetail JOIN Material ON InvoiceInputDetail.MaterialID=Material.ID SET InvoiceInputDetail.CategoryFamilyName=Material.CategoryFamilyName WHERE InvoiceInputDetail.CategoryFamilyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);

                sql = @"UPDATE WarehouseOutputDetail JOIN Material ON WarehouseOutputDetail.MaterialID=Material.ID SET WarehouseOutputDetail.CategoryCompanyName=Material.OriginalEquipmentManufacturer WHERE WarehouseOutputDetail.CategoryCompanyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"UPDATE WarehouseOutputDetailBarcode JOIN Material ON WarehouseOutputDetailBarcode.MaterialID=Material.ID SET WarehouseOutputDetailBarcode.CategoryCompanyName=Material.OriginalEquipmentManufacturer WHERE WarehouseOutputDetailBarcode.CategoryCompanyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"UPDATE WarehouseInputDetail JOIN Material ON WarehouseInputDetail.MaterialID=Material.ID SET WarehouseInputDetail.CategoryCompanyName=Material.OriginalEquipmentManufacturer WHERE WarehouseInputDetail.CategoryCompanyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"UPDATE WarehouseInputDetailBarcode JOIN Material ON WarehouseInputDetailBarcode.MaterialID=Material.ID SET WarehouseInputDetailBarcode.CategoryCompanyName=Material.OriginalEquipmentManufacturer WHERE WarehouseInputDetailBarcode.CategoryCompanyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                //sql = @"UPDATE WarehouseRequestDetail JOIN Material ON WarehouseRequestDetail.MaterialID=Material.ID SET WarehouseRequestDetail.CategoryCompanyName=Material.OriginalEquipmentManufacturer WHERE WarehouseRequestDetail.CategoryCompanyName IS NULL";
                //await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"UPDATE InvoiceInputDetail JOIN Material ON InvoiceInputDetail.MaterialID=Material.ID SET InvoiceInputDetail.CategoryCompanyName=Material.OriginalEquipmentManufacturer WHERE InvoiceInputDetail.CategoryCompanyName IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
        public static async Task<string> MESSyncAsync(string connectionString)
        {
            string result = "";
            try
            {
                string sql = @"UPDATE torderlist SET TERM1='' WHERE TERM1 IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);
                sql = @"UPDATE torderlist SET TERM2='' WHERE TERM2 IS NULL";
                await ExecuteNonQueryAsync(connectionString, sql);                
            }
            catch (Exception e)
            {
                result = e.Message;
            }
            return result;
        }
    }
}
