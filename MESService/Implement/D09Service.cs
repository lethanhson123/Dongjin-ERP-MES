namespace MESService.Implement
{
    public class D09Service : BaseService<torderlist, ItorderlistRepository>
    , ID09Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D09Service(ItorderlistRepository torderlistRepository

        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
        }
        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }
        public virtual async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string fromDateStr = BaseParameter.DateTimePicker1;
                    string searchText = BaseParameter.TextBox1 ?? string.Empty;

                    DateTime fromDate = DateTime.Now;
                    if (!string.IsNullOrEmpty(fromDateStr))
                    {
                        DateTime.TryParse(fromDateStr, out fromDate);
                    }

                    DateTime toDate = fromDate.AddDays(14);

                    string sqlTop = @"
                SELECT 
                    p.ID, p.POCode, p.Vehicle, p.Family, p.ProductCode, p.ECN, p.SNP, p.Quantity, 
                    p.WODate, p.CreateDate, p.CreateUserName, p.UpdateDate, p.UpdateUserName,
                    (SELECT COUNT(*) FROM PODetail WHERE POConfirmID = p.ID) as LeadCount
                FROM 
                    POConfirm p
                WHERE 
                    p.WODate BETWEEN @FromDate AND @ToDate
                    AND (
                        @SearchText = '' 
                        OR p.POCode LIKE CONCAT('%', @SearchText, '%') 
                        OR p.ECN LIKE CONCAT('%', @SearchText, '%')
                        OR p.ProductCode LIKE CONCAT('%', @SearchText, '%')
                    )
                ORDER BY 
                    p.POCode, p.WODate";

                    MySqlParameter[] parametersTop = new MySqlParameter[] {
                new MySqlParameter("@FromDate", fromDate),
                new MySqlParameter("@ToDate", toDate),
                new MySqlParameter("@SearchText", searchText)
            };

                    var dsTop = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlTop, parametersTop);

                    result.DataGridView1 = new List<SuperResultTranfer>();
                    if (dsTop.Tables.Count > 0 && dsTop.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in dsTop.Tables[0].Rows)
                        {
                            SuperResultTranfer item = new SuperResultTranfer
                            {
                                Coln1 = row["POCode"].ToString(),
                                Coln2 = row["Vehicle"].ToString(),
                                Coln3 = row["Family"].ToString(),
                                Coln4 = row["ProductCode"].ToString(),
                                Coln5 = row["ECN"].ToString(),
                                Coln6 = row["SNP"].ToString(),
                                Coln7 = row["Quantity"].ToString(),
                                DATEString = row["WODate"] != DBNull.Value ? Convert.ToDateTime(row["WODate"]).ToString("yyyy-MM-dd") : "",
                                CreateTime = row["CreateDate"] != DBNull.Value ? Convert.ToDateTime(row["CreateDate"]).ToString("yyyy-MM-dd HH:mm:ss") : "",
                                CreateBy = row["CreateUserName"].ToString(),
                                UpdateTime = row["UpdateDate"].ToString(),
                                UpdateBy = row["UpdateUserName"] != DBNull.Value ? Convert.ToDateTime(row["UpdateUserName"]).ToString("yyyy-MM-dd HH:mm:ss") : "",
                                COUNT = row["LeadCount"] != DBNull.Value ? Convert.ToInt32(row["LeadCount"]) : 0
                            };

                            result.DataGridView1.Add(item);
                        }
                    }

                    result.DataGridView = new List<SuperResultTranfer>();
                    SuperResultTranfer headerRow = new SuperResultTranfer();

                    for (int i = 0; i < 30; i++)
                    {
                        string propName = "D" + (i + 1).ToString("00");
                        DateTime date = fromDate.AddDays(i);
                        typeof(SuperResultTranfer).GetProperty(propName)?.SetValue(headerRow, date.ToString("yyyy-MM-dd"));
                    }

                    result.DataGridView.Add(headerRow);

                    string sqlBottom = @"
               SELECT 
                   POCode, Vehicle, Family, ProductCode, ECN, SNP, Quantity, WODate 
               FROM 
                   POConfirm
               WHERE 
                   WODate BETWEEN @FromDate AND @ToDate
                   AND (
                        @SearchText = '' 
                        OR POCode LIKE CONCAT('%', @SearchText, '%') 
                        OR ECN LIKE CONCAT('%', @SearchText, '%')
                        OR ProductCode LIKE CONCAT('%', @SearchText, '%')
                    )
               ORDER BY 
                   POCode, Vehicle, Family, ProductCode, ECN, SNP";

                    MySqlParameter[] parametersBottom = new MySqlParameter[] {
               new MySqlParameter("@FromDate", fromDate),
               new MySqlParameter("@ToDate", toDate),
               new MySqlParameter("@SearchText", searchText)
            };

                    var dsBottom = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sqlBottom, parametersBottom);

                    if (dsBottom.Tables.Count > 0 && dsBottom.Tables[0].Rows.Count > 0)
                    {
                        Dictionary<string, SuperResultTranfer> groupedData = new Dictionary<string, SuperResultTranfer>();

                        foreach (DataRow row in dsBottom.Tables[0].Rows)
                        {
                            string poCode = row["POCode"].ToString();
                            string vehicle = row["Vehicle"].ToString();
                            string family = row["Family"].ToString();
                            string productCode = row["ProductCode"].ToString();
                            string ecn = row["ECN"].ToString();
                            int snp = Convert.ToInt32(row["SNP"]);
                            int quantity = Convert.ToInt32(row["Quantity"]);
                            DateTime woDate = Convert.ToDateTime(row["WODate"]);

                            string key = $"{poCode}|{vehicle}|{family}|{productCode}|{ecn}|{snp}";

                            if (!groupedData.ContainsKey(key))
                            {
                                SuperResultTranfer item = new SuperResultTranfer
                                {
                                    Coln1 = poCode,
                                    Coln2 = vehicle,
                                    Coln3 = family,
                                    Coln4 = productCode,
                                    Coln5 = ecn,
                                    Coln6 = snp.ToString()
                                };

                                for (int i = 1; i <= 30; i++)
                                {
                                    string propName = "D" + i.ToString("00");
                                    typeof(SuperResultTranfer).GetProperty(propName)?.SetValue(item, "0");
                                }

                                groupedData.Add(key, item);
                            }

                            int dayDiff = (woDate.Date - fromDate.Date).Days;

                            if (dayDiff >= 0 && dayDiff < 30)
                            {
                                string propName = "D" + (dayDiff + 1).ToString("00");
                                PropertyInfo propInfo = typeof(SuperResultTranfer).GetProperty(propName);

                                if (propInfo != null)
                                {
                                    string currentValue = (string)propInfo.GetValue(groupedData[key]);
                                    int currentQuantity = int.TryParse(currentValue, out int value) ? value : 0;
                                    propInfo.SetValue(groupedData[key], (currentQuantity + quantity).ToString());
                                }
                            }
                        }

                        result.DataGridView.AddRange(groupedData.Values);
                    }

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                int successCount = 0;
                string userId = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 0
                                ? BaseParameter.ListSearchString[0] : "SYSTEM";

                foreach (var item in BaseParameter.ImportData)
                {
                    string checkSql = "SELECT ID, Quantity FROM POConfirm WHERE POCode = @POCode AND ProductCode = @ProductCode AND ECN = @ECN AND WODate = @WODate";

                    var checkParams = new MySqlParameter[]
                    {
                new MySqlParameter("@POCode", item.Coln1),
                new MySqlParameter("@ProductCode", item.Coln4),
                new MySqlParameter("@ECN", item.Coln5),
                new MySqlParameter("@WODate", DateTime.Parse(item.DATEString))
                    };

                    var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, checkSql, checkParams);

                    int newQuantity = Convert.ToInt32(item.Coln7);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        // Bản ghi đã tồn tại, cập nhật
                        long existingId = Convert.ToInt64(ds.Tables[0].Rows[0]["ID"]);

                        string updateSql = "UPDATE POConfirm SET Quantity = @Quantity, UpdateDate = @UpdateDate, UpdateUserName = @UpdateUserName WHERE ID = @ID";

                        var updateParams = new MySqlParameter[]
                        {
                    new MySqlParameter("@ID", existingId),
                    new MySqlParameter("@Quantity", newQuantity),
                    new MySqlParameter("@UpdateDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    new MySqlParameter("@UpdateUserName", userId)
                        };

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql, updateParams);

                        string insertDetailSql = "INSERT INTO PODetail (POConfirmID, Quantity, CreatedDate, CreatedBy) VALUES (@POConfirmID, @Quantity, @CreatedDate, @CreatedBy)";

                        var insertDetailParams = new MySqlParameter[]
                        {
                    new MySqlParameter("@POConfirmID", existingId),
                    new MySqlParameter("@Quantity", newQuantity),
                    new MySqlParameter("@CreatedDate", DateTime.Now),
                    new MySqlParameter("@CreatedBy", userId)
                        };

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertDetailSql, insertDetailParams);
                    }
                    else
                    {
                        // Bản ghi chưa tồn tại, thêm mới
                        string insertSql = @"INSERT INTO POConfirm 
                                  (POCode, Vehicle, Family, ProductCode, ECN, SNP, Quantity, WODate, CreateDate, CreateUserName) 
                                  VALUES 
                                  (@POCode, @Vehicle, @Family, @ProductCode, @ECN, @SNP, @Quantity, @WODate, @CreateDate, @CreateUserName);
                                  SELECT LAST_INSERT_ID()";

                        var insertParams = new MySqlParameter[]
                        {
                    new MySqlParameter("@POCode", item.Coln1),
                    new MySqlParameter("@Vehicle", item.Coln2),
                    new MySqlParameter("@Family", item.Coln3),
                    new MySqlParameter("@ProductCode", item.Coln4),
                    new MySqlParameter("@ECN", item.Coln5),
                    new MySqlParameter("@SNP", Convert.ToInt32(item.Coln6)),
                    new MySqlParameter("@Quantity", newQuantity),
                    new MySqlParameter("@WODate", DateTime.Parse(item.DATEString)),
                    new MySqlParameter("@CreateDate", DateTime.Now),
                    new MySqlParameter("@CreateUserName", userId)
                        };

                        string newIdStr = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, insertSql, insertParams);
                        long newId = Convert.ToInt64(newIdStr);

                        string insertDetailSql = "INSERT INTO PODetail (POConfirmID, Quantity, CreatedDate, CreatedBy) VALUES (@POConfirmID, @Quantity, @CreatedDate, @CreatedBy)";

                        var insertDetailParams = new MySqlParameter[]
                        {
                    new MySqlParameter("@POConfirmID", newId),
                    new MySqlParameter("@Quantity", newQuantity),
                    new MySqlParameter("@CreatedDate", DateTime.Now),
                    new MySqlParameter("@CreatedBy", userId)
                        };

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertDetailSql, insertDetailParams);
                    }

                    successCount++;
                }

                result.Success = true;
                result.Message = $"Đã lưu thành công {successCount} bản ghi";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> GetDetailGridData(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                DateTime today = DateTime.Today;

                result.DataGridView = new List<SuperResultTranfer>();

                SuperResultTranfer headerRow = new SuperResultTranfer();

                for (int i = 0; i < 30; i++)
                {
                    string propName = "D" + (i + 1).ToString("00");
                    DateTime date = today.AddDays(i);
                    typeof(SuperResultTranfer).GetProperty(propName)?.SetValue(headerRow, date.ToString("yyyy-MM-dd"));
                }

                result.DataGridView.Add(headerRow);

                string sql = @"
           SELECT 
               POCode, Vehicle, Family, ProductCode, ECN, SNP, Quantity, WODate 
           FROM 
               POConfirm
           WHERE 
               WODate BETWEEN @FromDate AND @ToDate
           ORDER BY 
               POCode, Vehicle, Family, ProductCode, ECN, SNP";

                MySqlParameter[] parameters = new MySqlParameter[] {
           new MySqlParameter("@FromDate", today),
           new MySqlParameter("@ToDate", today.AddDays(29))
       };

                var ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    Dictionary<string, SuperResultTranfer> groupedData = new Dictionary<string, SuperResultTranfer>();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string poCode = row["POCode"].ToString();
                        string vehicle = row["Vehicle"].ToString();
                        string family = row["Family"].ToString();
                        string productCode = row["ProductCode"].ToString();
                        string ecn = row["ECN"].ToString();
                        int snp = Convert.ToInt32(row["SNP"]);
                        int quantity = Convert.ToInt32(row["Quantity"]);
                        DateTime woDate = Convert.ToDateTime(row["WODate"]);

                        string key = $"{poCode}|{vehicle}|{family}|{productCode}|{ecn}|{snp}";

                        if (!groupedData.ContainsKey(key))
                        {
                            SuperResultTranfer item = new SuperResultTranfer
                            {
                                Coln1 = poCode,
                                Coln2 = vehicle,
                                Coln3 = family,
                                Coln4 = productCode,
                                Coln5 = ecn,
                                Coln6 = snp.ToString()
                            };

                            for (int i = 1; i <= 30; i++)
                            {
                                string propName = "D" + i.ToString("00");
                                typeof(SuperResultTranfer).GetProperty(propName)?.SetValue(item, "0");
                            }

                            groupedData.Add(key, item);
                        }

                        int dayDiff = (woDate.Date - today.Date).Days;

                        if (dayDiff >= 0 && dayDiff < 30)
                        {
                            string propName = "D" + (dayDiff + 1).ToString("00");
                            PropertyInfo propInfo = typeof(SuperResultTranfer).GetProperty(propName);

                            if (propInfo != null)
                            {
                                string currentValue = (string)propInfo.GetValue(groupedData[key]);
                                int currentQuantity = int.TryParse(currentValue, out int value) ? value : 0;
                                propInfo.SetValue(groupedData[key], (currentQuantity + quantity).ToString());
                            }
                        }
                    }

                    result.DataGridView.AddRange(groupedData.Values);
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }

            return result;
        }
        public virtual async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
      

    }
}

