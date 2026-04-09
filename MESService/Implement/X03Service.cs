namespace MESService.Implement
{
    public class X03Service : BaseService<torderlist, ItorderlistRepository>
    , IX03Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public X03Service(ItorderlistRepository torderlistRepository

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
                    `ECNNo` AS Coln1, 
                    `ProductionLineName` AS Coln2, 
                    `ProcessName` AS Coln3, 
                    `Code` AS PART_CODE, 
                    `Name` AS PART_NAME, 
                    `Family` AS FAMILY, 
                    `CircuitNumber` AS COUNT, 
                    `TaskTime` AS MIN, 
                    `CustomerTaskTime` AS UPH, 
                    `Gap` AS MT, 
                    `Note` AS Description, 
                    `ID`, 
                    `Active`, 
                    `CreateDate` AS CREATE_DTM, 
                    `CreateUserName` AS CREATE_USER
                FROM 
                    `TasktimeDetail`
                WHERE 
                    `Active` = 1
                ORDER BY 
                    `CreateDate` DESC 
                LIMIT 50;";

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
                await Task.Run(() => { });
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
                await Task.Run(() => { });
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
                if (BaseParameter != null && BaseParameter.ImportData != null && BaseParameter.ImportData.Count > 0)
                {
                    int totalCount = 0;
                    foreach (var item in BaseParameter.ImportData)
                    {
                        try
                        {
                            // Kiểm tra nếu ECNNo không có giá trị thì bỏ qua
                            if (string.IsNullOrEmpty(item.Coln1)) continue;

                            string ecnNo = item.Coln1.Replace("'", "''");
                            string productionLineName = (item.Coln2 ?? "").Replace("'", "''");
                            string processName = (item.Coln3 ?? "").Replace("'", "''");
                            string productCode = (item.PART_CODE ?? "").Replace("'", "''");
                            string productName = (item.PART_NAME ?? "").Replace("'", "''");
                            string family = (item.FAMILY ?? "").Replace("'", "''");

                            int circuitNumber = 0;
                            if (item.COUNT.HasValue) circuitNumber = item.COUNT.Value;

                            decimal taskTime = 0;
                            if (item.MIN.HasValue) taskTime = item.MIN.Value;

                            decimal customerTaskTime = 0;
                            if (item.UPH.HasValue) customerTaskTime = item.UPH.Value;

                            decimal gap = 0;
                            if (item.MT.HasValue) gap = item.MT.Value;

                            string note = (item.Description ?? "").Replace("'", "''");
                            string userIdx = (BaseParameter.USER_IDX ?? "SYSTEM").Replace("'", "''");
                            string userName = (BaseParameter.USER_NM ?? "SYSTEM").Replace("'", "''");

                            string insertSql = @"INSERT INTO `TasktimeDetail` 
                    (`CreateDate`, `CreateUserName`, `UpdateDate`, `UpdateUserName`, `Active`, 
                    `ECNNo`, `ProductionLineName`, `ProcessName`, `Code`, `Name`, `Family`, 
                    `CircuitNumber`, `TaskTime`, `CustomerTaskTime`, `Gap`, `Note`) 
                    VALUES 
                    (NOW(), '" + userName + "', NULL, NULL, 1, '" +
                                    ecnNo + "', '" +
                                    productionLineName + "', '" +
                                    processName + "', '" +
                                    productCode + "', '" +
                                    productName + "', '" +
                                    family + "', " +
                                    circuitNumber + ", " +
                                    taskTime.ToString(CultureInfo.InvariantCulture) + ", " +
                                    customerTaskTime.ToString(CultureInfo.InvariantCulture) + ", " +
                                    gap.ToString(CultureInfo.InvariantCulture) + ", '" +
                                    note + "')";

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
    }
}

