namespace MESService.Implement
{
    public class B08Service : BaseService<torderlist, ItorderlistRepository>
    , IB08Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public B08Service(ItorderlistRepository torderlistRepository
            , IWebHostEnvironment webHostEnvironment



        ) : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
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
                string sql = @"SELECT TTC_PART.`TC_MC` FROM TTC_PART GROUP BY `TC_MC`";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.Listttc_part = new List<ttc_part>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.Listttc_part.AddRange(SQLHelper.ToList<ttc_part>(dt));
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
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string AA = BaseParameter.ListSearchString[0];
                            string BB = BaseParameter.ListSearchString[1];
                            string DD = BaseParameter.ListSearchString[2];
                            string MC_TEXT = BaseParameter.ListSearchString[3];
                            string TextBox1 = BaseParameter.ListSearchString[4];

                            AA = DateTime.Parse(AA).ToString("yyyy-MM-dd");
                            BB = DateTime.Parse(BB).ToString("yyyy-MM-dd");

                            DD = "%" + DD + "%";
                            if (DD == "%ALL%")
                            {
                                DD = "%%";
                            }
                            await DB_LISECHK();

                            if (MC_TEXT == "ALL")
                            {
                                MC_TEXT = "";
                            }

                            string sql = @"SELECT FALSE AS `CHK`, TTC_ORDER.`CONDITION`, IFNULL(TTC_ORDER.`TTC_ENG`, 'N') AS `TTC_ENG`,  TTC_ORDER.`TTC_PO_DT`, TTC_PART.`TC_PART_NM`, TTC_PART.`TC_DESC`, TTC_ORDER.`TTC_PO`, TTC_ORDER.`PERFORMN`, 
                            (SELECT `PART_NO` FROM tspart WHERE `PART_IDX` = TTC_PART.`RAW_PART_IDX`) AS `RAW_PART_NO`, TTC_PART.`TC_SIZE`, TTC_PART.`TC_MC`, TTC_PART.`TC_PACKUNIT`, TTC_PART.`TC_LOC`, 
                            TTC_PART.`TTC_PART_IDX`, TTC_ORDER.`TTC_PO_INX`
                            FROM TTC_ORDER JOIN TTC_PART ON TTC_PART.TTC_PART_IDX = TTC_ORDER.TTC_PN_IDX WHERE  TTC_PART.`TC_MC` LIKE '%" + MC_TEXT + "%' AND TTC_PART.`TC_PART_NM` LIKE '%" + TextBox1 + "%' AND (NOT (`CONDITION` = 'Close'))  AND TTC_ORDER.`TTC_PO_DT` >= '" + BB + "' AND TTC_ORDER.`TTC_PO_DT` <= '" + AA + "' AND TTC_ORDER.`CONDITION` LIKE '" + DD + "' ORDER BY `CONDITION` DESC, TTC_ORDER.`TTC_PO_DT`";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        string sql = @"SELECT `RACKCODE`, (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`) AS `TCC_PARTNO`, `BARCODE_NM`, `RACKDTM`, IFNULL(`IN_USER`, 'No user') AS `IN_USER` 
                                FROM ttc_rackmtin ORDER BY `RACKDTM` DESC LIMIT 1000";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        string sql = @"SELECT `RACKCODE`, (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`) AS `TCC_PARTNO`, `BARCODE_NM`, `RACKDTM`, `IN_USER`, `RACKOUT_DTM`, `OUT_USER` FROM ttc_rackmtin WHERE `RACKOUT_YN` ='Y'
                                ORDER BY `RACKDTM` DESC LIMIT 1000";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView3 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    if (BaseParameter.Action == 4)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string AAA = BaseParameter.ListSearchString[0];
                            string CCC = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd") + " 00:00:00";

                            string sql = @"SELECT  
                                    ROW_NUMBER() OVER(ORDER BY `X`.`CREATE_DTM`  DESC) AS `NO`,
                                    DATE_FORMAT(`X`.`CREATE_DTM`, '%Y-%m-%d') AS `CREATE_DTM`, 
                                    (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = `X`.`TTC_PART_IDX`) AS `PART_NO`,
                                    (SELECT `TC_DESC` FROM TTC_PART WHERE `TTC_PART_IDX` = `X`.`TTC_PART_IDX`) AS `PART_NAME`,
                                    SUM(IF(`X`.`RACKIN_YN` = 'Y', `X`.`QTY`, 0)) AS `IN_STOCK`, 
                                    SUM(IF(`X`.`RACKOUT_YN` = 'Y', `X`.`QTY`, 0)) AS `OUT_STOCK`,
                                    (IF(`X`.`RACKIN_YN` = 'Y', SUM(SUM(`X`.`QTY`)) OVER (ORDER BY `X`.`CREATE_DTM` ASC) , 0) - IF(`X`.`RACKOUT_YN`='Y', SUM(SUM(`X`.`QTY`)) OVER (ORDER BY `X`.`CREATE_DTM` ASC), 0)) AS `STOCK`, 
                                    `X`.`TTC_PART_IDX`
                                    FROM ttc_rackmtin `X`
                                    WHERE `X`.`TTC_PART_IDX` = (SELECT `TTC_PART_IDX` FROM TTC_PART WHERE `TC_PART_NM` = '" + AAA + "')  AND `X`.`CREATE_DTM` >= '" + CCC + "' GROUP BY DATE_FORMAT(`X`.`CREATE_DTM`, '%Y-%m-%d'), `X`.`TTC_PART_IDX` ORDER BY `X`.`CREATE_DTM` DESC";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.T_DGV_01 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.T_DGV_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            result.T_DGV_02 = new List<SuperResultTranfer>();
                            if (result.T_DGV_01.Count > 0)
                            {
                                var T3_PART = result.T_DGV_01[0].STOCK;
                                sql = @"SELECT FALSE AS `CHK`, `C`.`RACKCODE`, `C`.`BARCODE_NM`, `C`.`QTY`, `C`.`RACKDTM`, `C`.`IN_USER`, `C`.`RACKOUT_DTM`, `C`.`OUT_USER`, `C`.`TRACK_IDX`
                                FROM ttc_rackmtin `C`   WHERE `C`.TTC_PART_IDX = '" + T3_PART + "' ORDER BY `C`.`CREATE_DTM` DESC LIMIT 700";

                                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);

                                for (int i = 0; i < ds.Tables.Count; i++)
                                {
                                    DataTable dt = ds.Tables[i];
                                    result.T_DGV_02.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 5)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string AAA = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd") + " 00:00:00";
                            string CCC = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd") + " 00:00:00";
                            string USER_NM = BaseParameter.ListSearchString[2];
                            if (USER_NM == "")
                            {
                                USER_NM = "%%";
                            }
                            string sql = @"SELECT  IFNULL(`IN_USER`, 'No user') AS `IN_USERNAME`,  DATE_FORMAT(`RACKDTM`, '%Y-%m-%d') AS `DATE`, DATE_FORMAT(`RACKDTM`, '%H') AS `TIME`, 
                                    (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`) AS `TC_PART_NM`,
                                    (SELECT `TC_DESC` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.TTC_PART_IDX) AS `TC_DESC`,
                                    SUM(`QTY`) AS `QTY`, (SELECT `TC_SIZE` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`) AS `TC_SIZE`, 
                                    (IFNULL(SUM(`QTY`), 0) * IFNULL((SELECT `TC_SIZE` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`), 0) / 1000) AS `Meter`,

                                    (SELECT `TC_W_S` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`) AS `TC_W_S`, 
                                    (SELECT `TC_W_MS` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`) AS `TC_W_MS`,  

                                    (IFNULL(SUM(`QTY`), 0) * (SELECT `TC_W_S` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`) + 
                                    (IFNULL(SUM(`QTY`), 0) * IFNULL((SELECT `TC_SIZE` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`), 0) / 1000) * 
                                    (SELECT `TC_W_MS` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`)) /60 AS `MIN` FROM ttc_rackmtin WHERE `RACKDTM` >= '" + AAA + "' AND `RACKDTM` <= '" + CCC + "' GROUP BY `IN_USER`, DATE_FORMAT(`RACKDTM`, '%Y-%m-%d %H'), `TTC_PART_IDX` HAVING `IN_USERNAME` LIKE '" + USER_NM + "' ORDER BY `IN_USER`, `DATE`";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.IN_LIST_DGV = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.IN_LIST_DGV.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                            sql = "SELECT IFNULL(SUM(`M`.`Meter`), 0) AS `MT`,  IFNULL(SUM(`M`.`MIN`), 0) AS `MIN`   FROM (" + sql + ") `M`";

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DGV_B08_09_1 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DGV_B08_09_1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 6)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string AAA = DateTime.Parse(BaseParameter.ListSearchString[0]).ToString("yyyy-MM-dd") + " 00:00:00";
                            string CCC = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd") + " 00:00:00";

                            string sql = @"SELECT  IFNULL(`OUT_USER`, 'No user') AS `OUT_USERNAME`,  DATE_FORMAT(`RACKOUT_DTM`, '%Y-%m-%d') AS `DATE`, DATE_FORMAT(`RACKOUT_DTM`, '%H') AS `TIME`, 
                                (SELECT `TC_PART_NM` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.`TTC_PART_IDX`) AS `TC_PART_NM`,
                                (SELECT `TC_DESC` FROM TTC_PART WHERE `TTC_PART_IDX` = ttc_rackmtin.TTC_PART_IDX) AS `TC_DESC`,
                                SUM(`QTY`) AS `QTY`  FROM ttc_rackmtin 
                                WHERE `RACKOUT_DTM` >= '" + AAA + "' AND `RACKOUT_DTM` <= '" + CCC + "' GROUP BY `OUT_USER`, DATE_FORMAT(`RACKOUT_DTM`, '%Y-%m-%d %H'), `TTC_PART_IDX` ORDER BY `OUT_USER`, `DATE`";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.OUT_LIST_DGV = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.OUT_LIST_DGV.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string TextBox2 = BaseParameter.ListSearchString[0];
                            string TextBox4 = BaseParameter.ListSearchString[1];
                            if (TextBox2.Length > 0)
                            {
                                if (TextBox4.Length > 0)
                                {
                                    string B_CODE = TextBox2.ToUpper().Trim();

                                    string sql = @"SELECT * FROM ttc_rackmtin WHERE ttc_rackmtin.BARCODE_NM = '" + B_CODE + "'";

                                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    result.DGV_B08_01 = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        result.DGV_B08_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }
                                    result.DGV_B08_02 = new List<SuperResultTranfer>();
                                    if (result.DGV_B08_01.Count <= 0)
                                    {
                                        sql = @"SELECT TTC_PART.TC_PART_NM, TTC_ORDER.TTC_PO_DT, TTC_ORDER.TTC_PO, TTC_ORDER.PERFORMN, TTC_ORDER.`CONDITION`, TTC_ORDER.ERROR_YN, TTC_BARCODE.TTC_BARCODENM, 
                                        TTC_BARCODE.Barcode_SEQ, TTC_BARCODE.DSCN_YN, (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = TTC_PART.RAW_PART_IDX) AS `TC_RAW_PN`, TTC_PART.TC_SIZE, TTC_PART.TC_PACKUNIT, TTC_PART.TC_MC
                                        FROM TTC_ORDER, TTC_BARCODE, TTC_PART
                                        WHERE TTC_ORDER.TTC_PO_INX = TTC_BARCODE.TTC_ORDER_IDX AND TTC_PART.TTC_PART_IDX = TTC_ORDER.TTC_PN_IDX AND TTC_BARCODE.`TTC_BARCODENM` = '" + B_CODE + "'";

                                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                        for (int i = 0; i < ds.Tables.Count; i++)
                                        {
                                            DataTable dt = ds.Tables[i];
                                            result.DGV_B08_02.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                        }

                                        if (result.DGV_B08_02.Count > 0)
                                        {
                                            sql = @"INSERT INTO `ttc_rackmtin` (`RACKCODE`, `TTC_PART_IDX`, `BARCODE_NM`, `RACKDTM`, `RACKOUT_DTM`, `QTY`, `CREATE_DTM`, `CREATE_USER`, `IN_USER`)
                                                (SELECT 'INPUT', TTC_ORDER.`TTC_PN_IDX`, TTC_BARCODE.`TTC_BARCODENM`, NOW(), NULL, SUBSTRING_INDEX(SUBSTRING_INDEX(TTC_BARCODE.`TTC_BARCODENM`, '$$', 2), '$$', -1), NOW(), '" + USER_IDX + "', '" + TextBox4 + "' From TTC_ORDER, TTC_BARCODE WHERE TTC_BARCODE.TTC_ORDER_IDX = TTC_ORDER.TTC_PO_INX AND TTC_BARCODE.TTC_BARCODENM = '" + B_CODE + "') ON DUPLICATE KEY UPDATE `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_IDX + "'";
                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                            sql = @"UPDATE TTC_BARCODE SET `WORK_END` = NOW(), `DSCN_YN` = 'Y', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_IDX + "'" + "WHERE TTC_BARCODE.`TTC_BARCODENM` = '" + B_CODE + "'";
                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                            sql = @"UPDATE TTC_ORDER SET TTC_ORDER.PERFORMN = PERFORMN+ (SELECT SUBSTRING_INDEX(SUBSTRING_INDEX(TTC_BARCODE.`TTC_BARCODENM`, '$$', 2), '$$', -1) FROM TTC_BARCODE WHERE TTC_BARCODE.`TTC_BARCODENM`= '" + B_CODE + "') WHERE TTC_ORDER.TTC_PO_INX = (SELECT TTC_BARCODE.TTC_ORDER_IDX FROM TTC_BARCODE WHERE TTC_BARCODE.`TTC_BARCODENM`= '" + B_CODE + "') ";
                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                            sql = @"UPDATE TTC_ORDER SET `CONDITION`='Complete' WHERE `TTC_PO` <= `PERFORMN` AND NOT (`CONDITION` = 'Complete')";
                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                            sql = @"ALTER TABLE     `ttc_rackmtin`     AUTO_INCREMENT= 1";
                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string TextBox3 = BaseParameter.ListSearchString[0];
                            string TextBox5 = BaseParameter.ListSearchString[1];
                            if (TextBox3.Length > 0)
                            {
                                if (TextBox5.Length > 0)
                                {
                                    string B_CODE = TextBox3.ToUpper().Trim();
                                    string sql = @"SELECT * FROM ttc_rackmtin WHERE ttc_rackmtin.RACKOUT_YN ='Y' AND ttc_rackmtin.BARCODE_NM = '" + B_CODE + "'";

                                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                    result.DGV_B08_02 = new List<SuperResultTranfer>();
                                    for (int i = 0; i < ds.Tables.Count; i++)
                                    {
                                        DataTable dt = ds.Tables[i];
                                        result.DGV_B08_02.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                    }

                                    if (result.DGV_B08_02.Count <= 0)
                                    {
                                        sql = @"SELECT * FROM ttc_rackmtin WHERE ttc_rackmtin.RACKOUT_YN ='N' AND ttc_rackmtin.BARCODE_NM = '" + B_CODE + "'";

                                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                                        result.DGV_B08_03 = new List<SuperResultTranfer>();
                                        for (int i = 0; i < ds.Tables.Count; i++)
                                        {
                                            DataTable dt = ds.Tables[i];
                                            result.DGV_B08_03.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                                        }

                                        if (result.DGV_B08_03.Count > 0)
                                        {
                                            sql = @"UPDATE `ttc_rackmtin` SET `RACKCODE`= 'OUTPUT', `RACKOUT_DTM` = NOW() , `OUT_USER` = '" + TextBox5 + "', `RACKOUT_YN` ='Y', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_IDX + "' WHERE ttc_rackmtin.BARCODE_NM = '" + B_CODE + "'";
                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                        }
                                    }
                                }
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
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.Action == 4)
                    {
                        if (BaseParameter.T_DGV_02 != null)
                        {
                            if (BaseParameter.T_DGV_02.Count > 0)
                            {
                                foreach (var item in BaseParameter.T_DGV_02)
                                {
                                    if (item.CHK == true)
                                    {
                                        string sql = @"UPDATE `ttc_rackmtin` SET `RACKCODE`= 'OUTPUT', `RACKOUT_DTM` = NOW() , `OUT_USER` = 'SYSTEM', `RACKOUT_YN` ='Y', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_IDX + "' WHERE ttc_rackmtin.`BARCODE_NM` = '" + item.BARCODE_NM + "'";
                                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                }
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
        public virtual async Task<BaseResult> DataGridView1_CellClick(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        var USER = BaseParameter.USER_IDX;
                        List<string> BARNM = new List<string>();
                        List<int> PACK_UNIT = new List<int>();
                        List<string> BARCODE_SEQ = new List<string>();
                        int PrintLine_total = 1;
                        int PrintLine = 0;
                        DateTime AAA = DateTime.Parse(BaseParameter.ListSearchString[0]);
                        string BBB = BaseParameter.ListSearchString[1];
                        string CCC = BaseParameter.ListSearchString[2];
                        int DDD = int.Parse(BaseParameter.ListSearchString[3]);
                        int FFF = int.Parse(BaseParameter.ListSearchString[4]);
                        string GGG = BaseParameter.ListSearchString[5];
                        int HHH = int.Parse(BaseParameter.ListSearchString[6]);
                        string LLL = BaseParameter.ListSearchString[7];
                        int KKK = int.Parse(BaseParameter.ListSearchString[8]);
                        string MMM = BaseParameter.ListSearchString[9];
                        int OOO = int.Parse(BaseParameter.ListSearchString[10]);
                        int PPP = int.Parse(BaseParameter.ListSearchString[11]);

                        string sql = @"SELECT  `TTC_BARCODE_IDX`, `TTC_BARCODENM`, `TTC_ORDER_IDX`  FROM  TTC_BARCODE    WHERE `TTC_ORDER_IDX` = '" + PPP + "'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        var DGV_C02_04 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            DGV_C02_04.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        var BARCODE_CNT = DGV_C02_04.Count;
                        if (BARCODE_CNT <= 0)
                        {
                            decimal DDDDecimal = DDD;
                            decimal KKKDecimal = KKK;
                            var AA = Math.Ceiling(DDDDecimal / KKKDecimal);
                            var DCO = DDD;
                            var VALUES = "";
                            var VALUESSUM = "";

                            PrintLine_total = 1;
                            for (int II = 1; II <= AA; II++)
                            {
                                if (DCO > KKK)
                                {
                                    BARNM.Add(BBB + "$$" + KKK + "$$" + AAA.ToString("yyyy-MM-dd") + "_" + PPP + "$$" + DDD + "$$" + II);
                                    PACK_UNIT.Add(KKK);
                                }
                                else
                                {
                                    BARNM.Add(BBB + "$$" + DCO + "$$" + AAA.ToString("yyyy-MM-dd") + "_" + PPP + "$$" + DDD + "$$" + II);
                                    PACK_UNIT.Add(DCO);
                                }
                                int index = II - 1;
                                VALUES = "('" + BARNM[index] + "', " + PPP + ", " + II + ", 'Y', 'N', NOW(), '" + USER + "')";

                                sql = @"INSERT INTO TTC_BARCODE (`TTC_BARCODENM`, `TTC_ORDER_IDX`, `Barcode_SEQ`, `TTC_BC_WORK`, `DSCN_YN`, `CREATE_DTM`, `CREATE_USER`) VALUES " + VALUES;
                                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                BARCODE_SEQ.Add(II.ToString());
                                DCO = DCO - KKK;
                            }
                            PrintLine = 1;
                            PrintLine_total = (int)AA;

                            if (BaseParameter.SuperResultTranfer != null)
                            {
                                try
                                {
                                    int index = PrintLine - 1;
                                    StringBuilder SearchString = new StringBuilder();

                                    for (PrintLine = 0; PrintLine < PrintLine_total; PrintLine++)
                                    {
                                        string TTC_BARCODENM = BARNM[PrintLine];
                                        try
                                        {
                                            double QTY = 0;
                                            try
                                            {
                                                QTY = BaseParameter.SuperResultTranfer.TTC_PO.Value;
                                            }
                                            catch (Exception ex)
                                            {
                                                string message = ex.Message;
                                            }
                                            int Barcode_SEQ = 1;
                                            try
                                            {
                                                Barcode_SEQ = int.Parse(BARCODE_SEQ[PrintLine]);
                                            }
                                            catch (Exception ex)
                                            {
                                                string message = ex.Message;
                                            }
                                            DateTime TTC_PO_DT = DateTime.Now;
                                            try
                                            {
                                                TTC_PO_DT = BaseParameter.SuperResultTranfer.TTC_PO_DT.Value;
                                            }
                                            catch (Exception ex)
                                            {
                                                string message = ex.Message;
                                            }
                                            SearchString.AppendLine(GlobalHelper.CreateHTMLB08(_WebHostEnvironment.WebRootPath, TTC_BARCODENM, BaseParameter.SuperResultTranfer.TC_PART_NM, BaseParameter.SuperResultTranfer.TC_DESC, QTY, Barcode_SEQ, BaseParameter.SuperResultTranfer.TC_LOC, TTC_PO_DT));
                                        }
                                        catch (Exception ex)
                                        {
                                            string message = ex.Message;
                                        }
                                    }
                                    string SheetName = this.GetType().Name;
                                    string contentHTML = GlobalHelper.InitializationString;
                                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "Empty.html");
                                    using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                                    {
                                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                                        {
                                            contentHTML = r.ReadToEnd();
                                        }
                                    }
                                    contentHTML = contentHTML.Replace(@"[Content]", SearchString.ToString());
                                    string fileName = "B08_" + GlobalHelper.InitializationDateTimeCode + ".html";
                                    string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                                    Directory.CreateDirectory(physicalPathCreate);
                                    GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                                    string filePath = Path.Combine(physicalPathCreate, fileName);
                                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                                    {
                                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                                        {
                                            w.WriteLine(contentHTML);
                                        }
                                    }
                                    result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;

                                    PrintLine = PrintLine + 1;
                                    if (PrintLine > PrintLine_total)
                                    {
                                    }
                                }
                                catch (Exception ex)
                                {
                                    string message = ex.Message;
                                }
                            }
                        }
                        sql = @"SELECT TTC_PART.`TC_PART_NM`, TTC_ORDER.`TTC_PO_DT`, TTC_ORDER.`TTC_PO`, TTC_ORDER.`PERFORMN`, TTC_ORDER.`CONDITION`, TTC_ORDER.`ERROR_YN`, TTC_BARCODE.`TTC_BARCODENM`, 
                            TTC_BARCODE.`Barcode_SEQ`, TTC_BARCODE.`DSCN_YN`, (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = TTC_PART.`RAW_PART_IDX`) AS `TC_RAW_PN`, TTC_PART.`TC_SIZE`, TTC_PART.`TC_PACKUNIT`, TTC_PART.TC_MC
                            FROM TTC_ORDER, TTC_BARCODE, TTC_PART
                            WHERE TTC_ORDER.`TTC_PO_INX` = TTC_BARCODE.`TTC_ORDER_IDX` AND TTC_PART.`TTC_PART_IDX` = TTC_ORDER.`TTC_PN_IDX` AND TTC_BARCODE.`DSCN_YN` = 'N' AND TTC_ORDER.`TTC_PO_INX` = '" + PPP + "'";
                        var DGV_B08_S01 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            DGV_B08_S01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        var BARCODE_CT = DGV_B08_S01.Count;
                        if (BARCODE_CT > 0)
                        {
                            sql = @"Update TTC_ORDER SET `CONDITION` = 'Working', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER + "' WHERE (`TTC_PO_INX` = '" + PPP + "')";
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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

        public virtual async Task<BaseResult> DB_LISECHK()
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"UPDATE TTC_ORDER SET `CONDITION`='Close' WHERE `TTC_PO_DT` < DATE_ADD(NOW(), INTERVAL -11 DAY) AND NOT (`CONDITION` = 'Complete')";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
    }
}

