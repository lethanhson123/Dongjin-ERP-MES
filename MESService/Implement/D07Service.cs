namespace MESService.Implement
{
    public class D07Service : BaseService<torderlist, ItorderlistRepository>
    , ID07Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        public D07Service(ItorderlistRepository torderlistRepository

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
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string AAA = BaseParameter.ListSearchString[0];
                            string BBB = BaseParameter.ListSearchString[1];
                            string CCC = BaseParameter.ListSearchString[2];
                            string DDD = BaseParameter.ListSearchString[3];
                            string EEE = BaseParameter.ListSearchString[4];



                            string sql = @"SELECT
                            tdd_poplan.TDD_POCODE, tdpdotpl.PO_CODE, 
                            (SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = tdpdotpl.PART_IDX)) AS `PART_NO`,
                            (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = tdpdotpl.PART_IDX)) AS `PART_GRP`,
                            (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = tdpdotpl.PART_IDX)) AS `PART_NM`,
                            tdpdotpl.PART_IDX_SNP, tdd_poplan.TDD_PP_DT, tdd_poplan.TDD_PP_QTY, tdd_poplan.TDD_PP_NTQTY,
                            IFNULL(`MZ`.`PN_QYT`, 0) AS `PACK_QTY`, IFNULL(`TDD_REMARK`, '') AS `TDD_REMARK`, 
                            tdpdotpl.PART_IDX

                            FROM tdd_poplan JOIN tdpdotpl
                            ON MID(tdd_poplan.TDD_POCODE, 1, LENGTH(tdd_poplan.TDD_POCODE) -2) = MID(tdpdotpl.PO_CODE, 1, LENGTH(tdpdotpl.PO_CODE) -3)

                            LEFT JOIN
                            (SELECT COUNT(`ZZ`.`VLID_PART_IDX`) AS `PN_QYT`,
                            `ZZ`.`VLID_DSCN_YN`, `ZZ`.`PDOTPL_IDX`
                            FROM tdpdmtim `ZZ`
                            WHERE `ZZ`.`VLID_DSCN_YN` ='Y' 
                            GROUP BY `ZZ`.`PDOTPL_IDX`, `ZZ`.`VLID_PART_IDX`) `MZ`

                            ON tdpdotpl.PDOTPL_IDX = `MZ`.`PDOTPL_IDX`

                            WHERE tdd_poplan.TDD_POCODE = '" + AAA + "'  HAVING `PO_CODE` LIKE '%" + BBB + "%' AND `PART_NO` LIKE '%" + CCC + "%' AND `PART_GRP` LIKE '%" + DDD + "%' AND `PART_NM` LIKE '%" + EEE + "%'    ";

                            //sql = sql + " LIMIT " + GlobalHelper.ListCount;

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
                    var CREATE_USER = BaseParameter.USER_ID;
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            if (BaseParameter.DataGridView5 != null)
                            {
                                if (BaseParameter.DataGridView5.Count > 0)
                                {
                                    var PO_COUNT = BaseParameter.ListSearchString[0];
                                    var PO_CODE = BaseParameter.ListSearchString[1].ToUpper();
                                    var DateTimePicker3 = DateTime.Parse(BaseParameter.ListSearchString[2]).ToString("yyyy-MM-dd HH:mm:ss");
                                    var M_POCD = PO_CODE.Replace(@"-M", @"");
                                    var CH_POCD = M_POCD + "-01";
                                    string sql = @"UPDATE `tdpdotpl` SET `PO_QTY` = 0, `NT_QTY` = 0, `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + CREATE_USER + "'    WHERE  `PO_CODE` = '" + PO_CODE + "' AND `PACK_QTY` = 0   ";
                                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    string SQL_PO = @"INSERT INTO `tdd_poplan` (`TDD_PO_TYPE`, `TDD_PP_PNIDX`, `TDD_PP_DT`, `TDD_PP_QTY`, `TDD_PP_NTQTY`, `TDD_DSCN_YN`, `TDD_POCODE`, `TDD_REMK_YN`, `CREATE_DTM`, `CREATE_USER`, `TDD_REMARK`) VALUES ";
                                    var VALUES = "";
                                    var SUMVALUES = "";
                                    var VALUES_PO = "";
                                    var SUMVALUES_PO = "";
                                    sql = "INSERT INTO `tdpdotpl` (`PO_CODE`, `PART_IDX`, `PART_IDX_SNP`, `PO_QTY`, `NT_QTY`, `PACK_QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES ";
                                    foreach (var item in BaseParameter.DataGridView5)
                                    {
                                        if (item.CHK == true)
                                        {
                                            var DGC1 = item.PO_CODE;
                                            var DGC2 = item.PART_IDX;
                                            var DGC3 = item.PO_QTY;
                                            var DGC6 = item.NT_QTY;
                                            var DGC4 = item.PART_SNP;
                                            var DGC5 = item.PDOTPL_IDX;

                                            if (SUMVALUES == "")
                                            {
                                                VALUES = "('" + CH_POCD + "', '" + DGC2 + "', '" + DGC4 + "', '" + DGC3 + "', '" + DGC6 + "', '0', NOW(), '" + CREATE_USER + "')";
                                                SUMVALUES = VALUES;
                                                VALUES_PO = "('N', '" + DGC2 + "', '" + DateTimePicker3 + "', '" + DGC3 + "', '" + DGC6 + "', 'Y', '" + PO_CODE + "', 'Y', NOW(), '" + CREATE_USER + "', '" + PO_COUNT + "')";
                                                SUMVALUES_PO = VALUES_PO;
                                            }
                                            else
                                            {
                                                VALUES = " ,('" + CH_POCD + "', '" + DGC2 + "', '" + DGC4 + "', '" + DGC3 + "', '" + DGC6 + "', '0', NOW(), '" + CREATE_USER + "')";
                                                SUMVALUES = SUMVALUES + VALUES;
                                                VALUES_PO = " ,('N', '" + DGC2 + "', '" + DateTimePicker3 + "', '" + DGC3 + "', '" + DGC6 + "', 'Y', '" + PO_CODE + "', 'Y', NOW(), '" + CREATE_USER + "', '" + PO_COUNT + "')";
                                                SUMVALUES_PO = SUMVALUES_PO + VALUES_PO;
                                            }
                                        }
                                    }

                                    if (VALUES.Length > 0)
                                    {
                                        sql = sql + SUMVALUES + "  ON DUPLICATE KEY UPDATE `PO_QTY`= VALUES(`PO_QTY`), `UPDATE_DTM` = NOW(),  `UPDATE_USER` = '" + CREATE_USER + "'";
                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        sql = @"UPDATE `tdd_poplan` SET `TDD_PP_QTY` = 0, `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + CREATE_USER + "'    WHERE  `TDD_POCODE` = '" + PO_CODE + "'    ";
                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        sql = SQL_PO + " " + SUMVALUES_PO + "  ON DUPLICATE KEY UPDATE `TDD_PP_QTY`= VALUES(`TDD_PP_QTY`), `UPDATE_DTM` = NOW(),  `UPDATE_USER` = '" + CREATE_USER + "'";
                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        sql = @"ALTER TABLE     `tdpdotpl`     AUTO_INCREMENT= 1";
                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        sql = @"ALTER TABLE     `tdd_poplan`     AUTO_INCREMENT= 1";
                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                                    }
                                }
                            }
                        }
                    }
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.DGV_OUT != null)
                        {
                            if (BaseParameter.DGV_OUT.Count > 0)
                            {
                                foreach (var item in BaseParameter.DGV_OUT)
                                {
                                    if (item.CHK == true)
                                    {
                                        var IDX_INTER = item.PDOTPL_IDX;
                                        var PO_QTY = item.PO_QTY;

                                        string sql = @"UPDATE `tdpdotpl` SET `PO_QTY` = " + PO_QTY + ", `NT_QTY` = 0, `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + CREATE_USER + "'    WHERE  `PDOTPL_IDX` = '" + IDX_INTER + "' ";
                                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                string sql = @"DELETE FROM `tdpdotpl_TMP`  ";
                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> PO_LIST_CB(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"SELECT   DISTINCT(tdd_poplan.TDD_POCODE) AS `PO_CODE`
                FROM   tdd_poplan   WHERE   tdd_poplan.TDD_DSCN_YN = 'Y' AND tdd_poplan.TDD_PP_DT >= DATE_ADD(NOW(),  INTERVAL -30 DAY)
                ORDER BY tdd_poplan.TDD_PP_DT DESC   ";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.ComboBox2 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.ComboBox2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button3_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        string PO_TXT = BaseParameter.ListSearchString[0];
                        string POCNT = BaseParameter.ListSearchString[1].Trim();
                        string sql = @"UPDATE  `tdd_poplan`  SET `TDD_REMARK`= '" + POCNT + "' WHERE  `TDD_POCODE` = '" + PO_TXT + "'";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> SUB_POCODE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string SUB_CD = BaseParameter.SearchString;
                    SUB_CD = SUB_CD.Replace(@"-M", @"");
                    string sql = @"SELECT DISTINCT(tdpdotpl.PO_CODE) AS `PO_CODE`, CAST(RIGHT(tdpdotpl.`PO_CODE`, 2) AS UNSIGNED) AS `NO`  FROM tdpdotpl
                        WHERE tdpdotpl.PO_CODE LIKE '" + SUB_CD + "%'";

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
        public virtual async Task<BaseResult> SUB_DATE(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string PO_CD = BaseParameter.SearchString;
                    string sql = @"SELECT 0 AS `CHK`, `A`.`PO_CODE`,(SELECT `PART_NO` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_NO`,
                        (SELECT `PART_CAR` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_GRP`,
                        (SELECT `PART_NM` FROM tspart WHERE (`PART_IDX` = `A`.`PART_IDX`)) AS `PART_NM`,
                        (`A`.`PART_IDX_SNP`) AS `PART_SNP`,    IFNULL(`PO_QTY`, 0) AS `PO_QTY`,  IFNULL(`NT_QTY`, 0) AS `NT_QTY`, 
                        IFNULL(`MZ`.`PN_QYT`, 0) AS `QTY`,     IFNULL((SELECT `QTY` FROM tiivtr WHERE tiivtr.`PART_IDX` = `A`.`PART_IDX` AND tiivtr.LOC_IDX = '2'), 0) AS `STOCK`,
                        IFNULL(CEILING(IFNULL(`PO_QTY`, 0) / (`A`.`PART_IDX_SNP`)), 0) AS `BOX_QTY`,
                        IFNULL(`MZ`.`PN_QYT`, 0 ) AS `PACK_QTY`,   (IFNULL(`PO_QTY`, 0) - IFNULL(`MZ`.`PN_QYT`, 0 )) AS `NEXT_PO`,
                        `A`.`PDOTPL_IDX`, `A`.PART_IDX
                        FROM tdpdotpl `A` LEFT JOIN

                        (SELECT COUNT(`ZZ`.`VLID_PART_IDX`) AS `PN_QYT`,
                        `ZZ`.`VLID_DSCN_YN`, `ZZ`.`PDOTPL_IDX`
                        FROM tdpdmtim `ZZ`
                        WHERE `ZZ`.`VLID_DSCN_YN` ='Y' 
                        GROUP BY `ZZ`.`PDOTPL_IDX`, `ZZ`.`VLID_PART_IDX`) `MZ`

                        ON `A`.PDOTPL_IDX = `MZ`.`PDOTPL_IDX`

                        WHERE `A`.`PO_CODE` = '" + PO_CD + "'  ";

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DGV_OUT = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DGV_OUT.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button2_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var CREATE_USER = BaseParameter.USER_ID;
                    if (BaseParameter.DGV_OUT != null)
                    {
                        if (BaseParameter.DGV_OUT.Count > 0)
                        {
                            if (BaseParameter.DataGridView2 != null)
                            {
                                if (BaseParameter.DataGridView2.Count > 0)
                                {
                                    string PO_CD = BaseParameter.SearchString;
                                    var DataGridView2Count = BaseParameter.DataGridView2.Count + 1;
                                    var NEXT_PO_CD_CNT = DataGridView2Count.ToString().PadLeft(2, '0');
                                    var NEXT_PO_CD = PO_CD.Substring(0, PO_CD.Length - 3) + "-" + NEXT_PO_CD_CNT;
                                    var VALUES = "";
                                    var SUMVALUES = "";
                                    string sql = @"INSERT INTO `tdpdotpl` (`PO_CODE`, `PART_IDX`, `PART_IDX_SNP`, `PO_QTY`, `NT_QTY`, `PACK_QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES ";

                                    foreach (var item in BaseParameter.DGV_OUT)
                                    {
                                        var DGC1 = item.PO_CODE;
                                        var DGC2 = item.PART_IDX;
                                        var DGC3 = item.NEXT_PO;
                                        var DGC6 = item.NT_QTY;
                                        var DGC4 = item.PART_SNP;
                                        if (SUMVALUES == "")
                                        {
                                            VALUES = "('" + NEXT_PO_CD + "', '" + DGC2 + "', '" + DGC4 + "', '" + DGC3 + "', '" + DGC6 + "', '0', NOW(), '" + CREATE_USER + "')";
                                            SUMVALUES = VALUES;
                                        }
                                        else
                                        {
                                            VALUES = " ,('" + NEXT_PO_CD + "', '" + DGC2 + "', '" + DGC4 + "', '" + DGC3 + "', '" + DGC6 + "', '0', NOW(), '" + CREATE_USER + "')";
                                            SUMVALUES = SUMVALUES + VALUES;
                                        }
                                    }
                                    if (SUMVALUES.Length > 0)
                                    {
                                        sql = sql + SUMVALUES + "  ON DUPLICATE KEY UPDATE `PO_QTY`= VALUES(`PO_QTY`), `UPDATE_DTM` = NOW(),  `UPDATE_USER` = '" + CREATE_USER + "'";
                                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                                        sql = @"ALTER TABLE     `tdpdotpl`     AUTO_INCREMENT= 1";
                                        sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var CREATE_USER = BaseParameter.USER_ID;
                    if (BaseParameter.DataGridView4 != null)
                    {
                        if (BaseParameter.DataGridView4.Count > 0)
                        {
                            string PO_CODE = BaseParameter.SearchString;
                            var COL1 = PO_CODE.ToUpper();

                            var VALUES = "";
                            var SUMVALUES = "";
                            string sql = @"INSERT INTO `tdpdotpl_TMP` (`PO_CODE`, `PART_NO`, `PO_QTY`, `NT_QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES ";

                            foreach (var item in BaseParameter.DataGridView4)
                            {
                                if (item.CHK == true)
                                {
                                    var COL2 = item.PART_NO;
                                    var COL3 = item.PO_QTY == null ? 0 : item.PO_QTY;
                                    var COL4 = item.NEXT_PO == null ? 0 : item.NEXT_PO;
                                    if (SUMVALUES == "")
                                    {
                                        VALUES = VALUES = "('" + COL1 + "', '" + COL2 + "', '" + COL3 + "', '" + COL4 + "', NOW(), '" + CREATE_USER + "')";
                                        SUMVALUES = VALUES;
                                    }
                                    else
                                    {
                                        VALUES = " ,('" + COL1 + "', '" + COL2 + "', '" + COL3 + "', '" + COL4 + "', NOW(), '" + CREATE_USER + "')";
                                        SUMVALUES = SUMVALUES + VALUES;
                                    }
                                }
                            }
                            sql = sql + SUMVALUES;
                            string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"SELECT 'FALSE' AS `CHK`,  IFNULL((SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = tdpdotpl_TMP.PART_NO)), 0) AS `PART_IDX`,tdpdotpl_TMP.`PO_CODE`, 
                            (SELECT `PART_CAR` FROM tspart WHERE (`PART_NO` = tdpdotpl_TMP.PART_NO)) AS `PART_GRP`,
                            (SELECT `PART_FML` FROM tspart WHERE (`PART_NO` = tdpdotpl_TMP.PART_NO)) AS `PART_NM`,`PART_NO`,
                            (SELECT `PART_SNP` FROM tspart WHERE (`PART_NO` = tdpdotpl_TMP.PART_NO)) AS `PART_SNP`,tdpdotpl_TMP.`PO_QTY`,  IFNULL(tdpdotpl_TMP.`NT_QTY`, 0) AS `NT_QTY`, 
                            COALESCE((SELECT `QTY` FROM tiivtr WHERE (`PART_IDX` = (SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = tdpdotpl_TMP.PART_NO))) AND `LOC_IDX` = '2'), 0) AS `Inventory`,
                            (COALESCE((SELECT `QTY` FROM tiivtr WHERE (`PART_IDX` = (SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = tdpdotpl_TMP.PART_NO))) AND `LOC_IDX` = '2'),0) - tdpdotpl_TMP.`PO_QTY`) AS `PO_status`,
                            tdpdotpl_TMP.`CREATE_DTM`, tdpdotpl_TMP.`CREATE_USER`, (SELECT `PART_USENY` FROM tspart WHERE (`PART_NO` = tdpdotpl_TMP.PART_NO)) AS `PART_USENY`,
                            IFNULL(`TB_AA`.`MAX_PO`, 0) AS `MAX_PO`, `TB_AA`.`PACK_QTY`, IFNULL(`TB_AA`.`PDOTPL_IDX`, 0) AS `PDOTPL_IDX`

                            FROM tdpdotpl_TMP LEFT JOIN  

                            (SELECT tdpdotpl.PDOTPL_IDX, tdpdotpl.PO_CODE, tdpdotpl.PART_IDX, tdpdotpl.PART_IDX_SNP, MAX(tdpdotpl.PO_QTY) AS `MAX_PO`, tdpdotpl.PACK_QTY
                            FROM tdpdotpl
                            WHERE tdpdotpl.PO_CODE ='" + COL1 + "' GROUP BY tdpdotpl.PO_CODE, tdpdotpl.PART_IDX) `TB_AA` ON tdpdotpl_TMP.PO_CODE =  `TB_AA`.PO_CODE AND(SELECT `PART_IDX` FROM tspart WHERE (`PART_NO` = tdpdotpl_TMP.PART_NO)) =  `TB_AA`.PART_IDX WHERE tdpdotpl_TMP.`PO_CODE` = '" + COL1 + "'";

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
    }
}

