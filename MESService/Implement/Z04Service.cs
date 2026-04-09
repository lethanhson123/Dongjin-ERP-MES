namespace MESService.Implement
{
    public class Z04Service : BaseService<torderlist, ItorderlistRepository>
    , IZ04Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public Z04Service(ItorderlistRepository torderlistRepository

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
                        var AA = BaseParameter.SearchString;

                        string sql = @"SELECT `TSYEAR_DEPART`, `TSYEAR_PKILOC`, `TSYEAR_INPUTER`, `TSYEAR_SERIAL_NO1`, `TSYEAR_SERIAL_NO2`, `TSYEAR_YEAR`, `TSYEAR_MESNO`
                    FROM tsyear_group_inv    WHERE   CONCAT(`TSYEAR_YEAR`,' ', tsyear_group_inv.`TSYEAR_MESNO`) = '" + AA + "'";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DGV_Z04_02 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DGV_Z04_02.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                        if (result.DGV_Z04_02.Count > 0)
                        {
                            var TextBox5 = result.DGV_Z04_02[0].TSYEAR_SERIAL_NO1;
                            var TextBox6 = result.DGV_Z04_02[0].TSYEAR_SERIAL_NO2;
                            var YEAR_CD = result.DGV_Z04_02[0].TSYEAR_YEAR;

                            sql = @"SELECT ROW_NUMBER() OVER (ORDER BY `TSYEAR_INV_SERIALNO` ASC) AS `NO`, 
                        `TSYEAR_INV_PKILOC`, 
                        `TSYEAR_INV_SERIALNO`, 
                        IF(`TSYEAR_INV_PART_TNM` = 'tspart', IFNULL((SELECT `PART_NO` FROM tspart WHERE `PART_IDX` =  `TSYEAR_INV_PART_IDX`), 'Cancel'), IFNULL((SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` =  `TSYEAR_INV_PART_IDX`), 'Cancel')) AS `PART_NO`, 
                        `TSYEAR_INV_QTY`, 
                        `TSYEAR_INV_DEPART`,  
                        IF(`TSYEAR_INV_PART_TNM` = 'tspart', IFNULL((SELECT PART_NM FROM tspart WHERE tspart.PART_IDX =  `TSYEAR_INV_PART_IDX`), 'Cencel'), IF(`TSYEAR_INV_PART_TNM` = 'Cancel', 'Cancel', 'LEAD_NO')) AS `PART_NM`,  
                        `TSYEAR_INV_PART_IDX`, 
                        (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = IFNULL(tsyear_inventory.`UPDATE_USER`, tsyear_inventory.`CREATE_USER`)) AS `Name`, 
                        `TSYEAR_INV_ANM`, 
                        `TSYEAR_INV_PART_TNM`, 
                        `TSYEAR_INV_DJGLOC`
                        FROM  tsyear_inventory    
                        WHERE `TSYEAR_INV_SERIALNO` >= '" + TextBox5 + "' AND `TSYEAR_INV_SERIALNO` <= '" + TextBox6 + "' AND   `TSYEAR_INV_YEAR` = '" + YEAR_CD + "' ORDER BY `TSYEAR_INV_SERIALNO` ASC  ";

                            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DGV_Z04_03 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DGV_Z04_03.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        var AA = BaseParameter.SearchString;

                        string sql = @"SELECT ROW_NUMBER() OVER (ORDER BY `TSYEAR_INV_SERIALNO` ASC) AS `NO`, `TSYEAR_INV_PKILOC`, `TSYEAR_INV_SERIALNO`, 
                            IF(`TSYEAR_INV_PART_TNM` = 'tspart', IFNULL((SELECT `PART_NO` FROM tspart WHERE `PART_IDX` =  `TSYEAR_INV_PART_IDX`), 'Cancel'), 
                            IFNULL((SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` =  `TSYEAR_INV_PART_IDX`), 'Cancel')) AS `PART_NO`, 
                            `TSYEAR_INV_QTY`, `TSYEAR_INV_DEPART`,  
                            IF(`TSYEAR_INV_PART_TNM` = 'tspart', IFNULL((SELECT PART_NM FROM tspart WHERE tspart.PART_IDX =  `TSYEAR_INV_PART_IDX`), 'Cencel'), IF(`TSYEAR_INV_PART_TNM` = 'Cancel', 'Cancel', 'LEAD_NO')) AS `PART_NM`,  
                            `TSYEAR_INV_PART_IDX`, (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = IFNULL(tsyear_inventory.`UPDATE_USER`, tsyear_inventory.`CREATE_USER`)) AS `Name`, `TSYEAR_INV_ANM`, `TSYEAR_INV_PART_TNM`, `TSYEAR_INV_DJGLOC`,
                            `TSYEAR_INV_YEAR`
                            FROM tsyear_inventory WHERE CONCAT(`TSYEAR_INV_YEAR`,' ', `TSYEAR_INV_MESNO`) = '" + AA + "'   AND `TSYEAR_INV_SERIALNO` >= (SELECT `TSYEAR_SERIAL_NO1` FROM tsyear_group_inv WHERE CONCAT(tsyear_group_inv.`TSYEAR_YEAR`,' ', tsyear_group_inv.TSYEAR_MESNO) = '" + AA + "') AND `TSYEAR_INV_SERIALNO` <= (SELECT `TSYEAR_SERIAL_NO2` FROM tsyear_group_inv WHERE CONCAT(tsyear_group_inv.`TSYEAR_YEAR`,' ', tsyear_group_inv.TSYEAR_MESNO) = '" + AA + "') ORDER BY `TSYEAR_INV_SERIALNO` ASC";

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
                            var TextBox14 = BaseParameter.ListSearchString[0];
                            var TextBox13 = BaseParameter.ListSearchString[1];
                            var TextBox15 = BaseParameter.ListSearchString[2];
                            var T3_CB01 = BaseParameter.ListSearchString[3];
                            var SS11 = 0;
                            var SS22 = 0;
                            if (TextBox14 == "") SS11 = 1;
                            if (TextBox13 == "") SS22 = 999999999;
                            var OP1 = "";
                            if (BaseParameter.RadioButton3 == true)
                            {
                                if (TextBox15 == "") OP1 = "";
                                else OP1 = "AND `TSYEAR_INV_MESNO` = '" + TextBox15 + "'  ";
                            }
                            if (BaseParameter.RadioButton4 == true)
                            {
                                OP1 = "AND `TSYEAR_INV_DEPART` LIKE '%" + TextBox15 + "%' AND (`TSYEAR_INV_SERIALNO` >= '" + SS11 + "' AND `TSYEAR_INV_SERIALNO` <= '" + SS22 + "')";
                            }
                            string sql = @"SELECT ROW_NUMBER() OVER (ORDER BY `TSYEAR_INV_SERIALNO` ASC) AS `NO`, `TSYEAR_INV_PKILOC`, `TSYEAR_INV_SERIALNO`, 
                            IF(`TSYEAR_INV_PART_TNM` = 'tspart', IFNULL((SELECT `PART_NO` FROM tspart WHERE `PART_IDX` =  `TSYEAR_INV_PART_IDX`), 'Cancel'), 
                            IFNULL((SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` =  `TSYEAR_INV_PART_IDX`), 'Cancel')) AS `PART_NO`, 
                            `TSYEAR_INV_QTY`, `TSYEAR_INV_DEPART`,  
                            IF(`TSYEAR_INV_PART_TNM` = 'tspart', IFNULL((SELECT PART_NM FROM tspart WHERE tspart.PART_IDX =  `TSYEAR_INV_PART_IDX`), 'Cencel'), 'LEAD_NO') AS `PART_NM`,  
                            `TSYEAR_INV_PART_IDX`,(SELECT `USER_NM` FROM tsuser WHERE `USER_ID` =  tsyear_inventory.`CREATE_USER`) AS `CreateBy`,CREATE_DTM AS CreateTime, (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = tsyear_inventory.`UPDATE_USER`) AS `UpdateBy`, `TSYEAR_INV_DJGLOC`, UPDATE_DTM AS UpdateTime
                            FROM  tsyear_inventory    
                            WHERE  `TSYEAR_INV_YEAR` = '" + T3_CB01 + "'" + OP1 + "   ORDER BY `TSYEAR_INV_SERIALNO` ASC  ";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView4 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView4.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 4)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var T4_CB01 = BaseParameter.ListSearchString[0];
                            var TextBox19 = BaseParameter.ListSearchString[1];
                            var DGV_DATA6 = "";
                            var DGV_DATA6_1 = @"SELECT ROW_NUMBER() OVER (ORDER BY `PART_NO` ASC) AS `NO`, 
                            IF(`TSYEAR_INV_PART_TNM` = 'tspart', IFNULL((SELECT `PART_NO` FROM tspart WHERE `PART_IDX` =  `TSYEAR_INV_PART_IDX`), 'Cancel'), 
                            IFNULL((SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` =  `TSYEAR_INV_PART_IDX`), 'Cancel')) AS `PART_NO`, 
                            IF(`TSYEAR_INV_PART_TNM` = 'tspart', IFNULL((SELECT PART_NM FROM tspart WHERE tspart.PART_IDX =  `TSYEAR_INV_PART_IDX`), 'Cencel'), 'LEAD_NO') AS `PART_NM`,  
                            COUNT(`TSYEAR_INV_SERIALNO`) AS `TSYEAR_INV_SERIALNO`, 
                            SUM(`TSYEAR_INV_QTY`) AS `TSYEAR_INV_QTY`, `TSYEAR_INV_PART_IDX`
                            FROM tsyear_inventory
                            WHERE `TSYEAR_INV_YEAR` = '" + T4_CB01 + "' GROUP BY `TSYEAR_INV_PART_IDX` HAVING `PART_NO` LIKE '%" + TextBox19 + "%' ORDER BY `PART_NO` ASC  ";

                            var DGV_DATA6_2 = @"SELECT ROW_NUMBER() OVER (ORDER BY `PART_NO` ASC) AS `NO`, 
                            IF(`TSYEAR_INV_PART_TNM` = 'tspart', IFNULL((SELECT `PART_NO` FROM tspart WHERE `PART_IDX` =  `TSYEAR_INV_PART_IDX`), 'Cancel'), 
                            IFNULL((SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` =  `TSYEAR_INV_PART_IDX`), 'Cancel')) AS `PART_NO`, 
                            IF(`TSYEAR_INV_PART_TNM` = 'tspart', IFNULL((SELECT PART_NM FROM tspart WHERE tspart.PART_IDX =  `TSYEAR_INV_PART_IDX`), 'Cencel'), 'LEAD_NO') AS `PART_NM`,  
                            (`TSYEAR_INV_SERIALNO`) AS `TSYEAR_INV_SERIALNO`, 
                            (`TSYEAR_INV_QTY`) AS `TSYEAR_INV_QTY`, `TSYEAR_INV_PART_IDX`
                            FROM  tsyear_inventory    
                            WHERE `TSYEAR_INV_YEAR` = '" + T4_CB01 + "' HAVING `PART_NO` LIKE '%" + TextBox19 + "%' ORDER BY `PART_NO` ASC  ";

                            if (BaseParameter.CheckBox1 == true) DGV_DATA6 = DGV_DATA6_1;
                            else DGV_DATA6 = DGV_DATA6_2;

                            string sql = DGV_DATA6 + " LIMIT " + GlobalHelper.ListCount;

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView5 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView5.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                            }
                        }
                    }
                    if (BaseParameter.Action == 5)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var TextBox1 = BaseParameter.ListSearchString[0];
                            var ComboBox1 = BaseParameter.ListSearchString[1];
                            var T5_CB01 = BaseParameter.ListSearchString[2];
                            var MES_NO = "";
                            if (!string.IsNullOrEmpty(TextBox1))
                                MES_NO = "  AND  `TSYEAR_MESNO` = '" + TextBox1 + "'";
                            var COMBO1 = ComboBox1;
                            if (COMBO1 == "ALL") COMBO1 = "%%";

                            string sql = @"SELECT `TSYEAR_YEAR`, `TSYEAR_MESNO`, `TSYEAR_DEPART`, `TSYEAR_PKILOC`, `TSYEAR_INPUTER`, `TSYEAR_SERIAL_NO1`, `TSYEAR_SERIAL_NO2`, (`TSYEAR_SERIAL_NO2` - `TSYEAR_SERIAL_NO1` +1 ) AS `COUNT`,
                            (SELECT COUNT(`TSYEAR_INV_PART_IDX`) FROM tsyear_inventory WHERE   `TSYEAR_INV_YEAR` = '" + T5_CB01 + "'  AND  `TSYEAR_INV_SERIALNO` >= `TSYEAR_SERIAL_NO1` AND `TSYEAR_INV_SERIALNO` <= `TSYEAR_SERIAL_NO2`) AS `INPUT_DATA`, IF(((`TSYEAR_SERIAL_NO2` - `TSYEAR_SERIAL_NO1` +1) - (SELECT COUNT(`TSYEAR_INV_PART_IDX`) FROM tsyear_inventory    WHERE    `TSYEAR_INV_YEAR` = '" + T5_CB01 + "'  AND   `TSYEAR_INV_SERIALNO` >= `TSYEAR_SERIAL_NO1` AND `TSYEAR_INV_SERIALNO` <= `TSYEAR_SERIAL_NO2`)) = 0, 'Complete', 'Not yet') AS `STATE`  FROM tsyear_group_inv    WHERE   `TSYEAR_YEAR` = '" + T5_CB01 + "' " + MES_NO + "HAVING  `State` LIKE '" + COMBO1 + "'     ORDER BY `INPUT_DATA` DESC";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView6 = new List<SuperResultTranfer>();
                            for (int i = 0; i < ds.Tables.Count; i++)
                            {
                                DataTable dt = ds.Tables[i];
                                result.DataGridView6.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    string USER_ID = BaseParameter.USER_ID;
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            if (BaseParameter.DataGridView1 != null)
                            {
                                if (BaseParameter.DataGridView1.Count > 0)
                                {
                                    var TextBox5 = BaseParameter.ListSearchString[0];
                                    var AA = BaseParameter.ListSearchString[1];
                                    var CC = BaseParameter.ListSearchString[2];
                                    var DD = BaseParameter.ListSearchString[3];
                                    var GG = BaseParameter.ListSearchString[4];

                                    if (!string.IsNullOrEmpty(TextBox5))
                                    {
                                        var VALTEXT = "";
                                        var SUBTEXT = "";

                                        foreach (var item in BaseParameter.DataGridView1)
                                        {
                                            if (item.NO != "New") continue;

                                            var BB = item.TSYEAR_INV_SERIALNO;
                                            var EE0 = item.TSYEAR_INV_PART_TNM;
                                            var EE = item.TSYEAR_INV_PART_IDX;
                                            var FF = item.TSYEAR_INV_QTY;
                                            var HH = item.TSYEAR_INV_ANM;
                                            var KK = item.TSYEAR_INV_DJGLOC;

                                            SUBTEXT = "('" + AA + "', '" + BB + "', '" + CC + "', '" + DD + "', '" + EE0 + "', '" + EE + "', '" + FF + "', 'Y', NOW(), '" + USER_ID + "', '" + GG + "', '" + HH + "', '" + KK + "')";

                                            if (VALTEXT == "")
                                                VALTEXT = SUBTEXT;
                                            else
                                                VALTEXT = VALTEXT + "," + SUBTEXT;
                                        }

                                        if (VALTEXT.Length > 0)
                                        {
                                            string sql = @"INSERT INTO `tsyear_inventory` (`TSYEAR_INV_YEAR`, `TSYEAR_INV_SERIALNO`, `TSYEAR_INV_DEPART`, `TSYEAR_INV_PKILOC`, `TSYEAR_INV_PART_TNM`, `TSYEAR_INV_PART_IDX`, `TSYEAR_INV_QTY`, `TSYEAR_INV_DSNY`, `CREATE_DTM`, `CREATE_USER`, `TSYEAR_INV_MESNO`, `TSYEAR_INV_ANM`, `TSYEAR_INV_DJGLOC`) 
                                    VALUES " + VALTEXT + @" ON DUPLICATE KEY UPDATE 
                                    `TSYEAR_INV_DEPART` = VALUES(`TSYEAR_INV_DEPART`), `TSYEAR_INV_PKILOC` = VALUES(`TSYEAR_INV_PKILOC`), `TSYEAR_INV_PART_TNM` = VALUES(`TSYEAR_INV_PART_TNM`), `TSYEAR_INV_PART_IDX` = VALUES(`TSYEAR_INV_PART_IDX`), `TSYEAR_INV_QTY` = VALUES(`TSYEAR_INV_QTY`), `TSYEAR_INV_DSNY` = VALUES(`TSYEAR_INV_DSNY`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`), `TSYEAR_INV_MESNO` = VALUES(`TSYEAR_INV_MESNO`), `TSYEAR_INV_ANM` = VALUES(`TSYEAR_INV_ANM`), `TSYEAR_INV_DJGLOC` = VALUES(`TSYEAR_INV_DJGLOC`)";
                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                            sql = @"INSERT INTO `tsyear_inventory_HIST` (`TSYEAR_INV_YEAR`, `TSYEAR_INV_SERIALNO`, `TSYEAR_INV_DEPART`, `TSYEAR_INV_PKILOC`, `TSYEAR_INV_PART_TNM`, `TSYEAR_INV_PART_IDX`, `TSYEAR_INV_QTY`, `TSYEAR_INV_DSNY`, `CREATE_DTM`, `CREATE_USER`, `TSYEAR_INV_MESNO`, `TSYEAR_INV_ANM`, `TSYEAR_INV_DJGLOC`) VALUES " + VALTEXT;
                                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                            try
                                            {
                                                sql = @"ALTER TABLE `tsyear_inventory` AUTO_INCREMENT= 1";
                                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                            }
                                            catch { }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var AAA = BaseParameter.ListSearchString[0];
                            var BBB = BaseParameter.ListSearchString[1];
                            var CCC = BaseParameter.ListSearchString[2];
                            var DDD = BaseParameter.ListSearchString[3];
                            var EEE = BaseParameter.ListSearchString[4];
                            var FFF = BaseParameter.ListSearchString[5];

                            string sql = @"UPDATE `tsyear_inventory` SET `TSYEAR_INV_PART_IDX`= '" + CCC + "', `TSYEAR_INV_PART_TNM` = '" + EEE + "',  `TSYEAR_INV_QTY`='" + DDD + "', `TSYEAR_INV_DJGLOC` = '" + FFF + "', UPDATE_DTM = NOW() , UPDATE_USER='" + USER_ID + "'  WHERE  `TSYEAR_INV_SERIALNO`= '" + BBB + "'  AND CONCAT(`TSYEAR_INV_YEAR`,' ', `TSYEAR_INV_MESNO`) = '" + AAA + "'";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            var SUBTEXT = "('" + AAA + "', '" + BBB + "', 'EDIT', 'EDIT', '" + EEE + "', '" + CCC + "', '" + DDD + "', 'Y', NOW(), '" + USER_ID + "', 'EDIT', 'E', '" + FFF + "')";
                            sql = @"INSERT INTO `tsyear_inventory_HIST` (`TSYEAR_INV_YEAR`, `TSYEAR_INV_SERIALNO`, `TSYEAR_INV_DEPART`, `TSYEAR_INV_PKILOC`, `TSYEAR_INV_PART_TNM`, `TSYEAR_INV_PART_IDX`, `TSYEAR_INV_QTY`, `TSYEAR_INV_DSNY`, `CREATE_DTM`, `CREATE_USER`, `TSYEAR_INV_MESNO`, `TSYEAR_INV_ANM`, `TSYEAR_INV_DJGLOC`) VALUES " + SUBTEXT;
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
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            var BBB = BaseParameter.ListSearchString[0];
                            var T2_CB01 = BaseParameter.ListSearchString[1];
                            var Label29 = BaseParameter.ListSearchString[2];

                            string sql = @"UPDATE `tsyear_inventory` SET `TSYEAR_INV_DSNY` ='N',  `TSYEAR_INV_PART_IDX`= '0', `TSYEAR_INV_QTY`='0', `TSYEAR_INV_PART_TNM` = 'Cancel' 
                            WHERE    CONCAT(`TSYEAR_INV_YEAR`,' ', `TSYEAR_INV_MESNO`) = '" + T2_CB01 + "'   AND   `TSYEAR_INV_SERIALNO`= '" + BBB + "'";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"INSERT INTO `tsyear_inventory_HIST` (`TSYEAR_INV_YEAR`, `TSYEAR_INV_SERIALNO`, `TSYEAR_INV_DEPART`, `TSYEAR_INV_PKILOC`, `TSYEAR_INV_PART_TNM`, `TSYEAR_INV_PART_IDX`, `TSYEAR_INV_QTY`, `TSYEAR_INV_DSNY`, `CREATE_DTM`, `CREATE_USER`, `TSYEAR_INV_MESNO`, `TSYEAR_INV_ANM`, `TSYEAR_INV_DJGLOC`)  
                            (SELECT `TSYEAR_INV_YEAR`, `TSYEAR_INV_SERIALNO`, `TSYEAR_INV_DEPART`,  '', 'Cancel',  '0',  '0', 'N', `CREATE_DTM`, `CREATE_USER`,
                            `TSYEAR_INV_MESNO`, `TSYEAR_INV_ANM`, `TSYEAR_INV_DJGLOC` FROM tsyear_inventory
                            WHERE `TSYEAR_INV_YEAR` = '" + Label29 + "' AND  `TSYEAR_INV_SERIALNO` = '" + BBB + "')";
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
                string sql = @"SELECT DISTINCT(CONCAT(`TSYEAR_YEAR`,' ', tsyear_group_inv.`TSYEAR_MESNO`)) AS `Description`  FROM tsyear_group_inv    
                WHERE    IFNULL(`UPDATE_DTM`, `CREATE_DTM`) >=  DATE_ADD(NOW(), INTERVAL -45 DAY)
                ORDER BY `TSYEAR_YEAR` DESC, `TSYEAR_MESNO`   ";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DGV_Z04_CB = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DGV_Z04_CB.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }

                var YE_DT_1 = DateTime.Now.Year - 2;
                sql = @"SELECT DISTINCT(`TSYEAR_YEAR`) FROM tsyear_group_inv WHERE `TSYEAR_YEAR` >= '" + YE_DT_1 + "' ORDER BY `TSYEAR_YEAR` DESC";

                ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.DGV_Z04_CB_1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.DGV_Z04_CB_1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> PART_LIST(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var AAA = "%" + BaseParameter.SearchString + "%";
                    string sql = @"SELECT * FROM (
                    SELECT  `PART_NO`, `PART_NM`, '' AS `QTY`, '' AS `DJG_LOC`, '>>>' AS `CHK`, `PART_IDX`, 'tspart' AS `TNM` FROM tspart 
                    UNION
                    SELECT  `LEAD_PN` AS `PART_NO`, 'LEAD' AS `PART_NM`, '' AS `QTY`, '' AS `DJG_LOC`, '>>>' AS `CHK`, `LEAD_INDEX` AS `PART_IDX`, 'torder_lead_bom' AS `TNM` FROM torder_lead_bom) `MM`
                    WHERE `MM`.`PART_NO` LIKE  '" + AAA + "'";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView2 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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