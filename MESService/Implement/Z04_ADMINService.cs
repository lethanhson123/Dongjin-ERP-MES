namespace MESService.Implement
{
    public class Z04_ADMINService : BaseService<torderlist, ItorderlistRepository>
    , IZ04_ADMINService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public Z04_ADMINService(ItorderlistRepository torderlistRepository

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
                result = await CB_DATASET();
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
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var MES_NO = BaseParameter.ListSearchString[0] == null ? "0" : BaseParameter.ListSearchString[0];
                            var DEP = BaseParameter.ListSearchString[1];
                            var S1 = BaseParameter.ListSearchString[2] == null ? "0" : BaseParameter.ListSearchString[2];
                            var S2 = BaseParameter.ListSearchString[3] == null ? "0" : BaseParameter.ListSearchString[3];
                            var AA = BaseParameter.ListSearchString[4];

                            if (AA == "")
                            {
                                AA = DateTime.Now.ToString("yyyy");
                            }
                            if (AA == "ALL")
                            {
                                AA = "%%";
                            }
                            var S_Y_D = DateTime.Now.AddMonths(-3);

                            string sql = @"SELECT `TSYEAR_YEAR`, `TSYEAR_MESNO`, `TSYEAR_DEPART`, `TSYEAR_PKILOC`, `TSYEAR_INPUTER`, `TSYEAR_SERIAL_NO1`, `TSYEAR_SERIAL_NO2`, (`TSYEAR_SERIAL_NO2` - `TSYEAR_SERIAL_NO1` +1 ) AS `COUNT` 
                                FROM tsyear_group_inv    WHERE    `TSYEAR_YEAR` LIKE '" + AA + "' AND (`TSYEAR_MESNO` = '" + MES_NO + "' OR  `TSYEAR_DEPART` LIKE '%" + DEP + "%' OR  (`TSYEAR_SERIAL_NO1` >='" + S1 + "' AND `TSYEAR_SERIAL_NO1` <= '" + S2 + "'))";

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
                var S_Y_D = DateTime.Now.AddMonths(-3);
                var MES_CODE = "";
                var AA = S_Y_D.Year;
                var BB = S_Y_D.Month.ToString("00");

                string sql = @"SELECT `TSYEAR_YEAR`, `TSYEAR_MESNO`, `TSYEAR_DEPART`, `TSYEAR_PKILOC`, `TSYEAR_INPUTER`, `TSYEAR_SERIAL_NO1`, `TSYEAR_SERIAL_NO2` 
                    FROM tsyear_group_inv    WHERE    `TSYEAR_YEAR` = '" + MES_CODE + "'";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                var DGV_Z04_09 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    DGV_Z04_09.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }

                result.ErrorNumber = DGV_Z04_09.Count + 1;

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
                    if (BaseParameter.Action == 1)
                    {
                        var USER_ID = BaseParameter.USER_ID;
                        if (BaseParameter.ListSearchString != null)
                        {
                            var S_Y_D = DateTime.Now.AddMonths(-3);

                            var AA = BaseParameter.ListSearchString[0];
                            var BB = BaseParameter.ListSearchString[1];
                            var CC = BaseParameter.ListSearchString[2];
                            var DD = BaseParameter.ListSearchString[3];
                            var EE = BaseParameter.ListSearchString[4];
                            var FF = BaseParameter.ListSearchString[5];
                            var GG = BaseParameter.ListSearchString[6];

                            var SUBTEXT = "('" + AA + "', '" + BB + "', '" + CC + "', '" + DD + "', '" + EE + "', '" + FF + "', '" + GG + "', NOW(), '" + USER_ID + "')";

                            string sql = @"INSERT INTO `tsyear_group_inv` (`TSYEAR_YEAR`, `TSYEAR_MESNO`, `TSYEAR_DEPART`, `TSYEAR_PKILOC`, `TSYEAR_INPUTER`, `TSYEAR_SERIAL_NO1`, `TSYEAR_SERIAL_NO2`, `CREATE_DTM`, `CREATE_USER`) 
                                VALUES  " + SUBTEXT + " ON DUPLICATE KEY UPDATE `TSYEAR_DEPART` = VALUES(`TSYEAR_DEPART`),  `TSYEAR_PKILOC` = VALUES(`TSYEAR_PKILOC`),  `TSYEAR_INPUTER` = VALUES(`TSYEAR_INPUTER`),  `TSYEAR_SERIAL_NO1` = VALUES(`TSYEAR_SERIAL_NO1`), `TSYEAR_SERIAL_NO2` = VALUES(`TSYEAR_SERIAL_NO2`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`) ";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"ALTER TABLE     `tsyear_group_inv`     AUTO_INCREMENT= 1";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"INSERT INTO `tsyear_group_inv_HIST` (`TSYEAR_YEAR`, `TSYEAR_MESNO`, `TSYEAR_DEPART`, `TSYEAR_PKILOC`, `TSYEAR_INPUTER`, `TSYEAR_SERIAL_NO1`, `TSYEAR_SERIAL_NO2`, `CREATE_DTM`, `CREATE_USER`) 
                                VALUES  " + " ('" + AA + "', '" + BB + "', '" + CC + "', '" + DD + "', '" + EE + "', '" + FF + "', '" + GG + "', NOW(), '" + USER_ID + "')";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> CB_DATASET()
        {
            BaseResult result = new BaseResult();
            try
            {
                var YE_DT = DateTime.Now.Year - 3;
                string sql = @"SELECT DISTINCT(`TSYEAR_YEAR`)  FROM tsyear_group_inv    WHERE    `TSYEAR_YEAR` >= '" + YE_DT + "'";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.CB_01 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.CB_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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

