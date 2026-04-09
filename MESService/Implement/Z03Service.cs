namespace MESService.Implement
{
    public class Z03Service : BaseService<torderlist, ItorderlistRepository>
    , IZ03Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public Z03Service(ItorderlistRepository torderlistRepository

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
              
                string sql = @"SELECT `CD_IDX` AS `Value`, `CD_SYS_NOTE` AS `COMB_CODE` 
                              FROM TSCODE 
                              WHERE `CDGR_IDX` = '8'";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.ComboBox1 = new List<SuperResultTranfer>();
                if (ds.Tables.Count > 0)
                {
                    result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
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
                if (BaseParameter?.ListSearchString?.Count > 0)
                {
                    string filterDate = BaseParameter.ListSearchString[0]; 

                    string sql = $@"SELECT `TSNON_WT_IDX` AS `CODE`, `TSNON_DATE` AS `DATE`, 
                                  (SELECT `CD_SYS_NOTE` FROM TSCODE WHERE `CD_IDX` = `TSNON_MCIDX`) AS `MC_NAME`, 
                                  `TSNON_SHIF` AS `SHIFT`, `TSNON_ST` AS `START_TIME`, `TSNON_ET` AS `END_TIME`, 
                                  ROUND(TIMESTAMPDIFF(MINUTE, `TSNON_ST`, `TSNON_ET`) / 60, 2) AS `WK_TIME_H`, 
                                  TIMESTAMPDIFF(MINUTE, `TSNON_ST`, `TSNON_ET`) AS `WK_TIME` 
                                  FROM TSNON_WORKTIME 
                                  WHERE `TSNON_DATE` = '{filterDate}' 
                                  ORDER BY `MC_NAME`, `SHIFT`";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView1 = new List<SuperResultTranfer>();
                    if (ds.Tables.Count > 0)
                    {
                        result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
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
                if (BaseParameter?.ListSearchString?.Count >= 4)
                {
                    string mcIdx = BaseParameter.ListSearchString[0];
                    string shift = BaseParameter.ListSearchString[1]; 
                    string date = BaseParameter.ListSearchString[2]; 
                    string startTime = BaseParameter.ListSearchString[3];
                    string workHours = BaseParameter.ListSearchString[4]; 
                    string userIdx = BaseParameter.USER_IDX;

                    // Tính END_TIME
                    DateTime startDateTime = DateTime.Parse($"{date} {startTime}");
                    double hours = double.Parse(workHours);
                    string endTime = startDateTime.AddHours(hours).ToString("yyyy-MM-dd HH:mm:ss");
                    startTime = startDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                    string sql = $@"INSERT INTO `TSNON_WORKTIME` (`TSNON_MCIDX`, `TSNON_SHIF`, `TSNON_DATE`, `TSNON_ST`, `TSNON_ET`, `CREATE_DTM`, `CREATE_USER`) 
                                  VALUES ('{mcIdx}', '{shift}', '{date}', '{startTime}', '{endTime}', NOW(), '{userIdx}') 
                                  ON DUPLICATE KEY UPDATE 
                                  `TSNON_ST` = '{startTime}', `TSNON_ET` = '{endTime}', 
                                  `UPDATE_DTM` = NOW(), `UPDATE_USER` = '{userIdx}'";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Success = true;
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
                if (BaseParameter?.SearchString != null)
                {
                    string code = BaseParameter.SearchString; 

                    string sql = $@"DELETE FROM `TSNON_WORKTIME` WHERE `TSNON_WT_IDX` = '{code}'";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Success = true;
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

