namespace MESService.Implement
{
    public class X04Service : BaseService<torderlist, ItorderlistRepository>
    , IX04Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public X04Service(ItorderlistRepository torderlistRepository

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
                    ECNNo AS Coln1, 
                    PartNo AS PART_CODE, 
                    Vehicle AS Coln2, 
                    Family AS FAMILY, 
                    Lead AS COUNT, 
                    CT AS MIN, 
                    LP AS UPH, 
                    FA AS MT, 
                    Total AS SUM, 
                    CreateDate AS CREATE_DTM, 
                    CreateUserName AS CREATE_USER, 
                    UpdateDate AS UPDATE_DTM, 
                    UpdateUserName AS UPDATE_USER
                FROM 
                    TaskTimeStandard
                WHERE 
                    1=1
                ORDER BY 
                    CreateDate DESC ";

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

                    // ✅ Lấy currentUser duy nhất 1 lần
                    string currentUser = !string.IsNullOrEmpty(BaseParameter.USER_IDX)
                        ? BaseParameter.USER_IDX
                        : (BaseParameter.ListSearchString.Count > 1 ? BaseParameter.ListSearchString[1] : "SYSTEM");
                    currentUser = currentUser.Replace("'", "''");

                    foreach (var item in BaseParameter.ImportData)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(item.Coln1)) continue;

                            string ecnNo = (item.Coln1 ?? "").Replace("'", "''");
                            string vehicle = (item.Coln2 ?? "").Replace("'", "''");
                            string family = (item.FAMILY ?? "").Replace("'", "''");
                            string partNo = (item.PART_CODE ?? "").Replace("'", "''");
                            int lead = item.COUNT ?? 0;
                            decimal ct = item.MIN ?? 0;
                            decimal lp = item.UPH ?? 0;
                            decimal fa = item.MT ?? 0;
                            decimal total = ct + lp + fa;

                            string insertSql = @"INSERT INTO TaskTimeStandard 
                        (ECNNo, PartNo, Vehicle, Family, Lead, CT, LP, FA, Total, CreateDate, CreateUserName) 
                        VALUES 
                        ('" + ecnNo + "', '" +
                                partNo + "', '" +
                                vehicle + "', '" +
                                family + "', " +
                                lead + ", " +
                                ct.ToString(CultureInfo.InvariantCulture) + ", " +
                                lp.ToString(CultureInfo.InvariantCulture) + ", " +
                                fa.ToString(CultureInfo.InvariantCulture) + ", " +
                                total.ToString(CultureInfo.InvariantCulture) + ", NOW(), '" +
                                currentUser + "')";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, insertSql);
                            totalCount++;
                        }
                        catch
                        {
                            continue;
                        }
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

