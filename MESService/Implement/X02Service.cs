namespace MESService.Implement
{
    public class X02Service : BaseService<torderlist, ItorderlistRepository>
    , IX02Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public X02Service(ItorderlistRepository torderlistRepository

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
                string sql = @"SELECT 
                                    `ID`, 
                                    `ParentID`, 
                                    `Code` AS SPECIFICATION, 
                                    `Name` AS PART_NAME, 
                                    `Location`,
                                    `CurrentStock` AS MT,
                                    (`CurrentStock` - `SafetyStock`) AS MIN,
                                    `SafetyStock` AS RAT,
                                    `Description` AS W5,
                                    `Active` AS D00,
                                    `CreateDate` AS CREATE_DTM,
                                    `CreateUserName` AS CREATE_USER
                                FROM 
                                    `BladeList`
                                WHERE 
                                    `Active` = 1
                                ORDER BY 
                                    `CreateDate` DESC  LIMIT 50;";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
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
                    // Xử lý tìm kiếm dựa vào tab hiện tại
                    if (BaseParameter.Action == 1) // Tab Blade List
                    {
                        if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 3)
                        {
                            string PartName = BaseParameter.ListSearchString[0];
                            string Spec = BaseParameter.ListSearchString[1];
                            string Location = BaseParameter.ListSearchString[2];

                            // Xây dựng câu SQL với điều kiện tìm kiếm
                            string sqlWhere = "";
                            if (!string.IsNullOrEmpty(PartName))
                                sqlWhere += " AND `Name` LIKE '%" + PartName + "%'";
                            if (!string.IsNullOrEmpty(Spec))
                                sqlWhere += " AND `Code` LIKE '%" + Spec + "%'";
                            if (!string.IsNullOrEmpty(Location))
                                sqlWhere += " AND `Location` LIKE '%" + Location + "%'";

                            string sql = @"SELECT 
                                                `ID`, 
                                                `ParentID`, 
                                                `Code` AS SPECIFICATION, 
                                                `Name` AS PART_NAME, 
                                                `Location`,
                                                `CurrentStock` AS MT,
                                                (`CurrentStock` - `SafetyStock`) AS MIN,
                                                `SafetyStock` AS RAT,
                                                `Description` AS W5,
                                                `Active` AS D00,
                                                `CreateDate` AS CREATE_DTM,
                                                `CreateUserName` AS CREATE_USER
                                            FROM 
                                                `BladeList`
                                            WHERE 
                                                `Active` = 1" + sqlWhere + @"
                                            ORDER BY 
                                                `CreateDate` DESC  LIMIT 50;";

                            // Thực hiện truy vấn và trả về kết quả
                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1) // Tab Blade List
                    {
                        if (BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 5)
                        {
                            string PartName = BaseParameter.ListSearchString[0];
                            string Spec = BaseParameter.ListSearchString[1];
                            string Location = BaseParameter.ListSearchString[2];
                            string SafetyStock = BaseParameter.ListSearchString[3];
                            string Description = BaseParameter.ListSearchString[4];

                            // Sử dụng USER_IDX nếu có
                            string UserIDX = !string.IsNullOrEmpty(BaseParameter.USER_IDX) ?
                                           BaseParameter.USER_IDX : "SYSTEM";

                            // Kiểm tra tên lưỡi cắt đã tồn tại chưa
                            string checkSql = "SELECT COUNT(*) FROM `BladeList` WHERE `Name` = '" + PartName + "'";
                            object countObj = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, checkSql);
                            int count = Convert.ToInt32(countObj);

                            if (count > 0)
                            {
                                // Cập nhật lưỡi cắt hiện có
                                string updateSql = @"UPDATE `BladeList` SET 
                            `Code` = '" + Spec + @"',
                            `Location` = '" + Location + @"',
                            `SafetyStock` = " + SafetyStock + @",
                            `Description` = '" + Description + @"',
                            `UpdateDate` = NOW(),
                            `UpdateUserName` = '" + UserIDX + @"'
                            WHERE `Name` = '" + PartName + "'";

                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql);
                            }
                            else
                            {
                                // Thêm mới lưỡi cắt
                                string insertSql = @"INSERT INTO `BladeList` 
                            (`Code`, `Name`, `Location`, `SafetyStock`, `CurrentStock`, 
                             `Description`, `CreateDate`, `CreateUserName`, `Active`) 
                            VALUES 
                            ('" + Spec + "', '" + PartName + "', '" + Location + "', " + SafetyStock +
                                            ", 0, '" + Description + "', NOW(), '" + UserIDX + "', 1)";

                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql);
                            }

                            result.Success = true;
                        }
                    }
                }
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
                if (BaseParameter != null)
                {
                    if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                    {
                        string ID = BaseParameter.SearchString;

                        // Kiểm tra lưỡi cắt có đang được sử dụng không
                        string checkScanInSql = "SELECT COUNT(*) FROM `BladeListScanIn` WHERE `ParentID` = '" + ID + "'";
                        string checkScanOutSql = "SELECT COUNT(*) FROM `BladeListScanOut` WHERE `ParentID` = '" + ID + "'";

                        object scanInCountObj = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, checkScanInSql);
                        object scanOutCountObj = await MySQLHelper.ExecuteScalarAsync(GlobalHelper.MariaDBConectionString, checkScanOutSql);

                        int scanInCount = Convert.ToInt32(scanInCountObj);
                        int scanOutCount = Convert.ToInt32(scanOutCountObj);

                        if (scanInCount > 0 || scanOutCount > 0)
                        {
                            // Nếu lưỡi cắt đã được sử dụng, chỉ thực hiện soft delete (đặt Active = 0)
                            string updateSql = "UPDATE `BladeList` SET `Active` = 0 WHERE `ID` = '" + ID + "'";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql);
                        }
                        else
                        {
                            // Nếu lưỡi cắt chưa được sử dụng, có thể xóa hoàn toàn
                            string deleteSql = "DELETE FROM `BladeList` WHERE `ID` = '" + ID + "'";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, deleteSql);
                        }

                        result.Success = true;
                    }
                }
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
                if (BaseParameter != null && BaseParameter.ImportData != null && BaseParameter.ImportData.Count > 0)
                {
                    int totalCount = 0;
                    foreach (var item in BaseParameter.ImportData)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(item.PART_NAME)) continue;

                            string safetyStock = item.RAT.HasValue ? item.RAT.Value.ToString() : "0";
                            string specification = (item.SPECIFICATION ?? "").Replace("'", "''");
                            string partName = item.PART_NAME.Replace("'", "''");
                            string location = (item.Location ?? "").Replace("'", "''");
                            string description = (item.W5 ?? "").Replace("'", "''");
                            string userIdx = (BaseParameter.USER_IDX ?? "SYSTEM").Replace("'", "''");

                            string insertSql = @"INSERT INTO `BladeList` 
                        (`Code`, `Name`, `Location`, `SafetyStock`, `CurrentStock`, 
                         `Description`, `CreateDate`, `CreateUserName`, `Active`) 
                        VALUES 
                        ('" + specification + "', '" +
                                 partName + "', '" +
                                 location + "', " + safetyStock +
                                 ", 0, '" + description + "', NOW(), '" +
                                 userIdx + "', 1)";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql);
                            totalCount++;
                        }
                        catch { continue; }
                    }

                    result.TotalCount = totalCount;
                    result.Success = true;
                }
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
        public virtual async Task<BaseResult> ButtonScanIn_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 4)
                {
                    string PartName = BaseParameter.ListSearchString[0];
                    string Quantity = BaseParameter.ListSearchString[1];
                    string User = BaseParameter.ListSearchString[2];
                    string ReasonForUse = BaseParameter.ListSearchString[3];
                    string UserIDX = !string.IsNullOrEmpty(BaseParameter.USER_IDX) ? BaseParameter.USER_IDX : "SYSTEM";
                    int quantityToAdd;
                    if (!int.TryParse(Quantity, out quantityToAdd) || quantityToAdd <= 0)
                    {
                        result.Error = "Số lượng không hợp lệ";
                        return result;
                    }

                    string getInfoSql = @"SELECT `ID`, `Name`, `Code`, `Location`, `SafetyStock`, `CurrentStock`, `Description` 
                              FROM `BladeList` WHERE `Name` = '" + PartName + "' AND `Active` = 1";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, getInfoSql);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        long bladeID = Convert.ToInt64(row["ID"]);
                        string bladeName = row["Name"].ToString();
                        string bladeCode = row["Code"].ToString();
                        string location = row["Location"].ToString();
                        int safetyStock = Convert.ToInt32(row["SafetyStock"]);
                        int currentStock = Convert.ToInt32(row["CurrentStock"]);
                        string description = row["Description"].ToString();

                        int newStock = currentStock + quantityToAdd;
                        string updateSql = @"UPDATE `BladeList` SET `CurrentStock` = " + newStock + @",
                                  `UpdateDate` = NOW(), `UpdateUserName` = '" + UserIDX + @"'
                                  WHERE `ID` = " + bladeID;
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql);

                        string insertSql = @"INSERT INTO `BladeListScanIn` 
                                 (`ParentID`, `ParentName`, `CreateDate`, `CreateUserName`, `Active`,
                                  `Code`, `Name`, `Location`, `CurrentStock`, `SafetyStock`,
                                  `Quantity`, `ReasonForUse`, `Description`) 
                                 VALUES 
                                 (" + bladeID + ", '" + bladeName + "', NOW(), '" + UserIDX + "', 1, '" +
                                          bladeCode + "', '" + bladeName + "', '" + location + "', " +
                                          newStock + ", " + safetyStock + ", " + quantityToAdd + ", '" +
                                          ReasonForUse + "', '" + description + "')";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql);
                        result.Success = true;
                    }
                    else
                    {
                        result.Error = "Không tìm thấy lưỡi cắt";
                    }
                }
                else
                {
                    result.Error = "Thông tin không đầy đủ";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> LoadScanInData()
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT 
                    `ID`, 
                    `ParentID`,
                    `Code` AS W1, 
                    `Name` AS W2, 
                    `Quantity` AS QTY, 
                    `SafetyStock` AS RAT, 
                    `CurrentStock` AS MT, 
                    `ReasonForUse` AS W5, 
                    `CreateDate` AS CREATE_DTM, 
                    `CreateUserName` AS CREATE_USER
                 FROM 
                    `BladeListScanIn`
                 WHERE `Active` = 1
                 ORDER BY 
                     `CreateDate` DESC
                 LIMIT 100;";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView2 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> ButtonScanOut_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 4)
                {
                    string PartName = BaseParameter.ListSearchString[0];
                    string Quantity = BaseParameter.ListSearchString[1];
                    string User = BaseParameter.ListSearchString[2];
                    string ReasonForUse = BaseParameter.ListSearchString[3];
                    string UserIDX = !string.IsNullOrEmpty(BaseParameter.USER_IDX) ? BaseParameter.USER_IDX : "SYSTEM";

                    int quantityToRemove;
                    if (!int.TryParse(Quantity, out quantityToRemove) || quantityToRemove <= 0)
                    {
                        result.Error = "Số lượng không hợp lệ";
                        return result;
                    }

                    string getInfoSql = @"SELECT `ID`, `Name`, `Code`, `Location`, `SafetyStock`, `CurrentStock`, `Description` 
                               FROM `BladeList` WHERE `Name` = '" + PartName + "' AND `Active` = 1";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, getInfoSql);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = ds.Tables[0].Rows[0];
                        long bladeID = Convert.ToInt64(row["ID"]);
                        string bladeName = row["Name"].ToString();
                        string bladeCode = row["Code"].ToString();
                        string location = row["Location"].ToString();
                        int safetyStock = Convert.ToInt32(row["SafetyStock"]);
                        int currentStock = Convert.ToInt32(row["CurrentStock"]);
                        string description = row["Description"].ToString();

                      

                        int newStock = currentStock - quantityToRemove;
                        string updateSql = @"UPDATE `BladeList` SET `CurrentStock` = " + newStock + @",
                                   `UpdateDate` = NOW(), `UpdateUserName` = '" + UserIDX + @"'
                                   WHERE `ID` = " + bladeID;
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql);

                        string insertSql = @"INSERT INTO `BladeListScanOut` 
                                  (`ParentID`, `ParentName`, `CreateDate`, `CreateUserName`, `Active`,
                                   `Code`, `Name`, `Location`, `CurrentStock`, `SafetyStock`,
                                   `Quantity`, `ReasonForUse`, `Description`) 
                                  VALUES 
                                  (" + bladeID + ", '" + bladeName + "', NOW(), '" + UserIDX + "', 1, '" +
                                          bladeCode + "', '" + bladeName + "', '" + location + "', " +
                                          newStock + ", " + safetyStock + ", " + quantityToRemove + ", '" +
                                          ReasonForUse + "', '" + description + "')";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql);
                        result.Success = true;
                    }
                    else
                    {
                        result.Error = "Không tìm thấy lưỡi cắt";
                    }
                }
                else
                {
                    result.Error = "Thông tin không đầy đủ";
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> LoadScanOutData()
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT 
                     `ID`, 
                     `ParentID`,
                     `Code` AS W1, 
                     `Name` AS W2, 
                     `Quantity` AS QTY, 
                     `SafetyStock` AS RAT, 
                     `CurrentStock` AS MT, 
                     `ReasonForUse` AS W5, 
                     `CreateDate` AS CREATE_DTM, 
                     `CreateUserName` AS CREATE_USER
                  FROM 
                     `BladeListScanOut`
                  WHERE `Active` = 1
                  ORDER BY 
                      `CreateDate` DESC
                  LIMIT 100;";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView3 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}

