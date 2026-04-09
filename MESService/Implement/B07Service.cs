namespace MESService.Implement
{
    public class B07Service : BaseService<torderlist, ItorderlistRepository>
    , IB07Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public B07Service(ItorderlistRepository torderlistRepository

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

                    if (BaseParameter.Action == 2)
                    {
                        string SUCHK = BaseParameter.SearchString;
                        string sql = @"SELECT `TC_PART_NM`, `TC_DESC`, (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = TTC_PART.RAW_PART_IDX) AS `RAW_PART_NO`, 
                        `TC_SIZE`, `TC_MC`, `TC_PACKUNIT`, `TC_LOC`, `TTC_PART_IDX`, `TC_W_S`, `TC_W_MS` 
                        FROM TTC_PART WHERE `TC_PART_NM` LIKE '%" + SUCHK + "%'";

                        sql = sql + " LIMIT " + GlobalHelper.ListCount;

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView3 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }


                    }
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string DGV_DATAT3_1 = GlobalHelper.InitializationString;
                            string AAA = BaseParameter.ListSearchString[0];
                            string BBB = BaseParameter.ListSearchString[1];
                            string CCC = BaseParameter.ListSearchString[2];
                            string DDD = BaseParameter.ListSearchString[3];
                            if (string.IsNullOrEmpty(AAA))
                            {
                                DGV_DATAT3_1 = @"SELECT tspart.`PART_IDX`, tspart.`PART_NO` AS `FG_PART_NO`, IFNULL(tspart.`PART_NM`, '') AS `FG_PART_NAME`, IFNULL(tspart.`PART_CAR`, '') AS `FG_PART_MODEL`, '' AS `TB_PART_NO` FROM tspart WHERE tspart.PART_SCN = '6' AND tspart.`PART_NO` LIKE '%" + BBB + "%' AND tspart.`PART_NM` LIKE '%" + CCC + "%' AND tspart.`PART_CAR` LIKE '%" + DDD + "%' ORDER BY `FG_PART_NO`   ";
                            }
                            else
                            {
                                DGV_DATAT3_1 = @"SELECT tspart.`PART_IDX`,
                                    tspart.`PART_NO` AS `FG_PART_NO`,
                                    IFNULL(tspart.`PART_NM`, '') AS `FG_PART_NAME`,
                                    IFNULL(tspart.`PART_CAR`, '') AS `FG_PART_MODEL`,
                                    IFNULL((SELECT TTC_PART.`TC_PART_NM` FROM TTC_PART WHERE TTC_PART.`TTC_PART_IDX` = TTC_BOM.`TTC_PART_IDX`), '') AS `TB_PART_NO`
                                    FROM tspart LEFT JOIN TTC_BOM
                                    ON tspart.`PART_IDX` = TTC_BOM.`PART_IDX`
                                    WHERE tspart.PART_SCN = '6' AND TTC_BOM.`TCC_DSVYN` = 'Y' AND
                                    IFNULL((SELECT TTC_PART.`TC_PART_NM` FROM TTC_PART WHERE TTC_PART.`TTC_PART_IDX` = TTC_BOM.`TTC_PART_IDX`), '') LIKE '%" + AAA + "%' AND tspart.`PART_NO` LIKE '%" + BBB + "%' AND tspart.`PART_NM` LIKE '%" + CCC + "%' AND tspart.`PART_CAR` LIKE '%" + DDD + "%' GROUP BY tspart.`PART_NO` ORDER BY `FG_PART_NO`   ";
                            }
                            string sql = DGV_DATAT3_1;
                            sql = sql + " LIMIT " + GlobalHelper.ListCount;
                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView5 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView5.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 1)
                    {
                        string TextBox1 = BaseParameter.SearchString;
                        string sql = @"SELECT `TTC_PART_IDX`, `TC_PART_NM`, `TC_DESC`, (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = TTC_PART.RAW_PART_IDX) AS `RAW_PART_NO`, `TC_SIZE`, `TC_MC`, `TC_PACKUNIT`, `TC_LOC`  FROM TTC_PART 
                            WHERE TTC_PART.`TC_PART_NM` = '" + TextBox1 + "'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
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
        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.DataGridView2 != null)
                        {
                            string SQL1 = GlobalHelper.InitializationString;
                            string SQL_SUM = GlobalHelper.InitializationString;
                            foreach (var item in BaseParameter.DataGridView2)
                            {
                                if (item.CHK == true)
                                {
                                    string AAA = item.ORDER_DATE;
                                    string BBB = item.TTC_PART_IDX.ToString();
                                    string CCC = item.CUT_ORDER;
                                    SQL1 = "('" + BBB + "', '" + AAA + "', '" + CCC + "', '0', 'Stay', 'Y', 'N',  NOW(), '" + USER_IDX + "')";
                                    if (SQL_SUM == "")
                                    {
                                        SQL_SUM = SQL1;
                                    }
                                    else
                                    {
                                        SQL_SUM = SQL_SUM + "," + SQL1;
                                    }
                                }
                            }
                            string sql = @"INSERT INTO `TTC_ORDER` (`TTC_PN_IDX`, `TTC_PO_DT`, `TTC_PO`, `PERFORMN`, `CONDITION`, `DSCN_YN`, `ERROR_YN`, `CREATE_DTM`, `CREATE_USER`) VALUES " + SQL_SUM;
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView2 = new List<SuperResultTranfer>();
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            if (!string.IsNullOrEmpty(BaseParameter.SearchString))
                            {
                                string Label4 = BaseParameter.SearchString;
                                string AAA = BaseParameter.ListSearchString[0];
                                string BBB = BaseParameter.ListSearchString[1];
                                string CCC = BaseParameter.ListSearchString[2];
                                string DDD = BaseParameter.ListSearchString[3];
                                string EEE = BaseParameter.ListSearchString[4];
                                string FFF = BaseParameter.ListSearchString[5];
                                string GGG = BaseParameter.ListSearchString[6];
                                string HHH = BaseParameter.ListSearchString[7];
                                string KKK = BaseParameter.ListSearchString[8];
                                string NNN = BaseParameter.ListSearchString[9];

                                if (Label4 == "New")
                                {
                                    string TextBox5 = CCC;
                                    if (!string.IsNullOrEmpty(TextBox5))
                                    {
                                        string sql = @"INSERT INTO `TTC_PART` (`TC_PART_NM`, `TC_DESC`, `RAW_PART_IDX`, `TC_SIZE`, `TC_MC`, `TC_PACKUNIT`, `TC_LOC`, `CREATE_DTM`, `CREATE_USER`,  `TC_W_S`, `TC_W_MS` ) VALUES 
                                        ('" + AAA + "', '" + BBB + "', (SELECT tspart.PART_IDX FROM tspart WHERE tspart.PART_NO = '" + CCC + "'), '" + DDD + "', '" + EEE + "', '" + FFF + "', '" + GGG + "', NOW(), '" + USER_IDX + "', '" + KKK + "', '" + NNN + "')";
                                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                }
                                else
                                {                                   
                                    if (int.Parse(Label4) > 0)
                                    {
                                        string sql = @"UPDATE `TTC_PART` SET `TC_DESC`= '" + BBB + "', `TC_SIZE`= '" + DDD + "', `TC_MC`= '" + EEE + "', `TC_PACKUNIT`= '" + FFF + "', `TC_LOC`= '" + GGG + "', `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + USER_IDX + "' WHERE `TC_PART_NM`= '" + AAA + "'";
                                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string BOM_IDX = BaseParameter.ListSearchString[0];
                            string Text2 = BaseParameter.ListSearchString[1];
                            string sql = @"UPDATE `TTC_BOM`  SET `TTC_BOMSNP` = '" + int.Parse(Text2) + "', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_IDX + "'  WHERE `TTC_BOM_IDX` = '" + BOM_IDX + "'";
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 3)
                    {
                        string BOM_IDX = BaseParameter.SearchString;
                        string USER_IDX = BaseParameter.USER_IDX;
                        string sql = @"UPDATE `TTC_BOM`  SET `TCC_DSVYN` = 'N', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_IDX + "'  WHERE `TTC_BOM_IDX` = '" + BOM_IDX + "'";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> DGV_BOM_LD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string P_IDX = BaseParameter.SearchString;
                    string sql = @"SELECT (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = TTC_BOM.`PART_IDX`) AS `FG_PART_NO`,
                    IFNULL((SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = TTC_BOM.`PART_IDX`), '') AS `FG_PART_NAME`,
                    IFNULL((SELECT tspart.`PART_CAR` FROM tspart WHERE tspart.`PART_IDX` = TTC_BOM.`PART_IDX`), '') AS `FG_PART_MODEL`,
                    (SELECT TTC_PART.`TC_PART_NM` FROM TTC_PART WHERE TTC_PART.`TTC_PART_IDX` = TTC_BOM.`TTC_PART_IDX`) AS `TB_PART_NO`,
                    `TTC_BOMSNP`, `TTC_BOM_IDX`, `PART_IDX`, `TTC_PART_IDX`   FROM TTC_BOM
                    WHERE TTC_BOM.`PART_IDX` = '" + P_IDX + "' AND  TTC_BOM.`TCC_DSVYN` = 'Y' ORDER BY `FG_PART_NO`";


                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView4 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.DataGridView1 != null)
                    {
                        string sql = @"SELECT `TTC_PART_IDX`, `TC_PART_NM`, `TC_DESC`, (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = TTC_PART.RAW_PART_IDX) AS `RAW_PART_NO`, `TC_SIZE`, `TC_MC`, `TC_PACKUNIT`, `TC_LOC`  FROM TTC_PART";


                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_C09_01 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_C09_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < result.DataGridView1.Count; i++)
                        {
                            string DATE_AA = BaseParameter.DataGridView1[i].DATE.Value.ToString("yyyy-MM-dd");
                            string PSRT_AA = BaseParameter.DataGridView1[i].Tube_Cutting_Part_No;
                            string ORDER_AA = BaseParameter.DataGridView1[i].CUT_ORDER;

                            var DGV_C09_01 = result.DGV_C09_01.Where(o => o.TC_PART_NM == PSRT_AA).FirstOrDefault();
                            if (DGV_C09_01 != null)
                            {
                                DGV_C09_01.CHK = true;
                                DGV_C09_01.DATE = BaseParameter.DataGridView1[i].DATE;
                                result.DataGridView2.Add(DGV_C09_01);
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
    }
}

