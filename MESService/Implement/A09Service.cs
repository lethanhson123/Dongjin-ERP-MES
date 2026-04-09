
namespace MESService.Implement
{
    public class A09Service : BaseService<torderlist, ItorderlistRepository>
    , IA09Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public A09Service(ItorderlistRepository torderlistRepository

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
        public async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ListSearchString != null)
                {
                    string suchTB1 = BaseParameter.ListSearchString[0] ?? "";
                    string suchTB2 = BaseParameter.ListSearchString[1] ?? "";
                    string suchTB3 = BaseParameter.ListSearchString[2] ?? "";

                    if (BaseParameter.Action == 1) // Tab1
                    {
                        suchTB1 = "%" + suchTB1 + "%";
                        suchTB2 = "%" + suchTB2 + "%";
                        suchTB3 = "%" + suchTB3 + "%";

                        string sql = @"SELECT 
                    (SELECT `PART_NO` FROM tspart WHERE tspart.PART_IDX = `T`.`TSPSRT_IDX`) AS `PART_NO`, 
                    (SELECT `PART_NM` FROM tspart WHERE tspart.PART_IDX = `T`.`TSPSRT_IDX`) AS `PART_NM`, 
                    (SELECT `PART_CAR` FROM tspart WHERE tspart.PART_IDX = `T`.`TSPSRT_IDX`) AS `PART_CAR`, 
                    (SELECT `PART_FML` FROM tspart WHERE tspart.PART_IDX = `T`.`TSPSRT_IDX`) AS `PART_FML`, 
                    `T`.`TSCOST_DT`, `T`.`TSCOST_VAL`, `T`.`TSCOST_IDX` 
                    FROM (SELECT `TSCOST_IDX`, `TSPSRT_IDX`, `TSCOST_DT`, `TSCOST_VAL` 
                          FROM TSCOST 
                          WHERE (`TSPSRT_IDX`, `TSCOST_DT`) 
                          IN (SELECT `TSPSRT_IDX`, MAX(`TSCOST_DT`) AS `TSCOST_DT` 
                              FROM TSCOST 
                              GROUP BY `TSPSRT_IDX`) 
                          ORDER BY `TSCOST_DT` DESC) `T`
                    GROUP BY `T`.`TSPSRT_IDX`
                    HAVING `PART_NO` LIKE '" + suchTB1 + @"' 
                        AND `PART_NM` LIKE '" + suchTB2 + @"' 
                        AND `PART_CAR` LIKE '" + suchTB3 + @"'";

                        sql += " LIMIT 200";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView1 = new List<SuperResultTranfer>();
                        foreach (DataTable dt in ds.Tables)
                        {
                            result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    else if (BaseParameter.Action == 2) // Tab2
                    {
                        suchTB1 = "%" + suchTB1 + "%";
                        suchTB2 = "%" + suchTB2 + "%";
                        suchTB3 = "%" + suchTB3 + "%";

                        string sql = @"SELECT 
                    (SELECT `PART_NO` FROM tspart WHERE tspart.PART_IDX = `T`.`TSPSRT_IDX`) AS `PART_NO`, 
                    (SELECT `PART_NM` FROM tspart WHERE tspart.PART_IDX = `T`.`TSPSRT_IDX`) AS `PART_NM`, 
                    (SELECT `PART_CAR` FROM tspart WHERE tspart.PART_IDX = `T`.`TSPSRT_IDX`) AS `PART_CAR`, 
                    (SELECT `PART_FML` FROM tspart WHERE tspart.PART_IDX = `T`.`TSPSRT_IDX`) AS `PART_FML`, 
                    `T`.`TSCOST_DT`, `T`.`TSCOST_VAL`, `T`.`TSCOST_IDX` 
                    FROM (SELECT `TSCOST_IDX`, `TSPSRT_IDX`, `TSCOST_DT`, `TSCOST_VAL` 
                          FROM TSCOST 
                          WHERE (`TSPSRT_IDX`, `TSCOST_DT`) 
                          IN (SELECT `TSPSRT_IDX`, MAX(`TSCOST_DT`) AS `TSCOST_DT` 
                              FROM TSCOST 
                              GROUP BY `TSPSRT_IDX`) 
                          ORDER BY `TSCOST_DT` DESC) `T`
                    GROUP BY `T`.`TSPSRT_IDX`
                    HAVING `PART_NO` LIKE '" + suchTB1 + @"' 
                        AND `PART_NM` LIKE '" + suchTB2 + @"' 
                        AND `PART_CAR` LIKE '" + suchTB3 + @"'";

                        sql += " LIMIT 200";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        foreach (DataTable dt in ds.Tables)
                        {
                            result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public async Task<BaseResult> LoadDataGridView3(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string TextBox21_Text = BaseParameter.ListSearchString[0] ?? "";

                string sql = @"SELECT `TSCOST_DT`, `TSCOST_VAL` FROM TSCOST
            WHERE `TSPSRT_IDX` = (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = '" + TextBox21_Text + @"') 
            ORDER BY `TSCOST_DT` DESC";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DataGridView3 = new List<SuperResultTranfer>();
                foreach (DataTable dt in ds.Tables)
                {
                    result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                if (BaseParameter != null && BaseParameter.Action == 1) // Tab1
                {
                    string sql = @"INSERT INTO TSCOST (`TSPSRT_IDX`, `TSCOST_DT`, `TSCOST_VAL`, `CREATE_DTM`, `CREATE_USER`)
                        SELECT tspart.PART_IDX, '2020-01-01', '0', NOW(), 'MES'
                        FROM tspart LEFT JOIN TSCOST ON TSCOST.TSPSRT_IDX = tspart.PART_IDX 
                        WHERE tspart.PART_SCN='6' AND TSCOST.TSCOST_IDX IS NULL";

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }
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
        public async Task<BaseResult> Buttonsave_Click_A09_1(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string TextBox1_Text = BaseParameter.ListSearchString[0] ?? ""; 
                string COST_DATE = BaseParameter.ListSearchString[1] ?? "";
                string TextBox7_Text = BaseParameter.ListSearchString[2] ?? "";
                string Main_Tooluser_Text = BaseParameter.USER_IDX ?? "MES";

                string sql = @"INSERT INTO TSCOST (`TSPSRT_IDX`, `TSCOST_DT`, `TSCOST_VAL`, `CREATE_DTM`, `CREATE_USER`)
            SELECT `PART_IDX`, '" + COST_DATE + "' , '" + TextBox7_Text + "', NOW(), '" + Main_Tooluser_Text + "'   FROM   tspart   WHERE `PART_SCN` = '6'   AND   `PART_NO` = '" + TextBox1_Text + "'  ON DUPLICATE KEY UPDATE  `TSCOST_VAL` = '" + TextBox7_Text + "', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + Main_Tooluser_Text + "'";
        
        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                try
                {
                    sql = @"ALTER TABLE `TSCOST` AUTO_INCREMENT = 1";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }
                catch { }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public async Task<BaseResult> Buttonsave_Click_A09_2(BaseParameter baseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string Main_Tooluser_Text = baseParameter.USER_IDX ?? "MES";
                string SQL_TOTAL = "";

                for (int i = 0; i < baseParameter.ListSearchString.Count; i += 3)
                {
                    string Column1 = baseParameter.ListSearchString[i] ?? ""; 
                    string Column2 = baseParameter.ListSearchString[i + 1] ?? ""; 
                    string Column3 = baseParameter.ListSearchString[i + 2] ?? ""; 

                    string SQLD = $"('{Column1}', '{Column2}', '{Column3}', NOW(), '{Main_Tooluser_Text}')";
                    SQL_TOTAL += string.IsNullOrEmpty(SQL_TOTAL) ? SQLD : ", " + SQLD;
                }

                string sql = @"INSERT INTO TSCOST_LIST (`TSPSRT_IDX`, `TSCOST_DT`, `TSCOST_VAL`, `CREATE_DTM`, `CREATE_USER`) VALUES " + SQL_TOTAL;

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                sql = @"INSERT INTO TSCOST (`TSPSRT_IDX`, `TSCOST_DT`, `TSCOST_VAL`, `CREATE_DTM`, `CREATE_USER`)
            (SELECT (SELECT `PART_IDX` FROM tspart WHERE `PART_NO` = `TSPSRT_IDX`), `TSCOST_DT`, `TSCOST_VAL`, NOW(), 'MES' 
            FROM TSCOST_LIST WHERE `CREATE_DTM` = NOW())
            ON DUPLICATE KEY UPDATE 
            `TSCOST_VAL` = TSCOST.`TSCOST_VAL`, `UPDATE_DTM` = NOW(), `UPDATE_USER` = 'MES'";

                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                try
                {
                    sql = @"ALTER TABLE `TSCOST` AUTO_INCREMENT = 1";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }
                catch { }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}

