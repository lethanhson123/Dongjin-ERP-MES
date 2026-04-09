
namespace MESService.Implement
{
    public class V03Service : BaseService<torderlist, ItorderlistRepository>
    , IV03Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public V03Service(ItorderlistRepository torderlistRepository
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
                await Task.Run(() => { });
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> COMB_CHG(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string mode = "ORDER";
                if (BaseParameter?.ListSearchString != null && BaseParameter.ListSearchString.Count > 0)
                {
                    mode = BaseParameter.ListSearchString[0];
                }

                string sql = "";

                if (mode == "ORDER")
                {
                    sql = @"SELECT `PDP_NO` FROM PDPUSCH 
                    WHERE `PDP_REC_YN` ='Y' AND `PDP_CONF` = 'Report' AND `PDP_CNF_YN` = 'N'
                    GROUP BY `PDP_NO` ORDER BY `CREATE_DTM` DESC";
                }
                else if (mode == "COMPANY")
                {
                    sql = @"SELECT `PDP_COMPY_NO` AS `PDP_NO` FROM PDPUSCH 
                    WHERE `PDP_REC_YN` ='Y' AND `PDP_CONF` = 'Report' 
                    AND `PDP_CNF_YN` = 'N' AND `PDP_PRIENT` = 'C'
                    AND `PDP_COMPY_NO` IS NOT NULL
                    GROUP BY `PDP_COMPY_NO` ORDER BY `CREATE_DTM` DESC";
                }

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.ComboBox1 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public virtual async Task<BaseResult> LoadComboBox1(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"
            SELECT 'ALL' AS CD_IDX, 'ALL' AS CD_NM
            UNION ALL
            SELECT '0' AS CD_IDX, '-' AS CD_NM
            UNION ALL
            SELECT '1' AS CD_IDX, 'Waiting' AS CD_NM
            UNION ALL
            SELECT '2' AS CD_IDX, 'Shipping' AS CD_NM
            UNION ALL
            SELECT '3' AS CD_IDX, 'Complete' AS CD_NM
            UNION ALL
            SELECT '4' AS CD_IDX, 'Cancel' AS CD_NM
            UNION ALL
            SELECT '5' AS CD_IDX, 'Ing...' AS CD_NM
            ORDER BY CD_IDX";

                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                result.ComboBox1 = new List<SuperResultTranfer>();
                if (ds.Tables.Count > 0)
                {
                    result.ComboBox1 = SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]);
                }
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> LoadV03_4Modal(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ListSearchString != null)
                {
                    string P_CODE = BaseParameter.ListSearchString[0];

                    string sql = @"SELECT 
                                       PDPUSCH.PDP_NO,
                                       PDPUSCH.PDP_DATE1,
                                       (SELECT pdpart.PN_V FROM pdpart WHERE pdpart.pdpart_IDX = PDPUSCH.PDP_PART) AS PN_V,
                                       (SELECT pdpart.PN_K FROM pdpart WHERE pdpart.pdpart_IDX = PDPUSCH.PDP_PART) AS PN_K,
                                       PDPUSCH.PDP_COST,
                                       PDPUSCH.PDP_BE_COST,
                                       (SELECT PDCMPNY.CMPNY_NM FROM PDCMPNY WHERE PDCMPNY.CMPNY_IDX = PDPUSCH.PDP_CMPY) AS Company
                                   FROM PDPUSCH
                                   WHERE (SELECT pdpart.pdpart_IDX FROM pdpart WHERE pdpart.PN_NM = @P_CODE) = PDPUSCH.PDP_PART
                                   AND PDPUSCH.PDP_CMPY != '134'
                                   ORDER BY PDPUSCH.PDP_COST ASC, PDPUSCH.PDP_DATE1 DESC
                                   LIMIT 200";

                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
               new MySqlParameter("@P_CODE", P_CODE)
                    };

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters);
                    result.DataGridView9 = new List<SuperResultTranfer>();

                    if (ds.Tables.Count > 0)
                    {
                        result.DataGridView9 = SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]);
                    }
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
                            string TextBox1 = BaseParameter.ListSearchString[0];

                            string sql = @"SELECT 
                                            `A`.`PDP_CONF`,`A`.PDP_NO, `A`.`PDP_DATE1`, (SELECT `CD_SYS_NOTE` FROM PDCDNM WHERE `CD_IDX` = `A`.`PDP_DEPA`) AS `DEPARTMENT`,
                                            `B`.`PN_NM`, `B`.`PN_V`, `B`.`PSPEC_V`, (SELECT `CD_NM_VN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_VN`,
                                            `B`.`PN_K`, `B`.`PSPEC_K`, (SELECT `CD_NM_HAN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_KR`, `B`.`PQTY`, `A`.`PDP_QTY`,
                                            `A`.`PDP_MEMO`, `A`.`CREATE_DTM`, `A`.`CREATE_USER`, (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `A`.`CREATE_USER`) AS `USER_NAME`, `A`.`PDP_PART`,
                                            IFNULL((SELECT `QTY` FROM pd_tiivtr WHERE `PART_IDX` = `A`.`PDP_PART` AND `A`.`PDPUSCH_IDX` = `ORDER_IDX`),0) AS `STOCK`, `A`.`PDPUSCH_IDX`, 
                                            IFNULL(`A`.`PDP_COST`, 0) AS `PDP_COST`, 
                                            IFNULL((`A`.`PDP_COST` * `A`.`PDP_QTY`), 0) AS `SUM_COST`,
                                            IFNULL(`A`.`PDP_VAT`, 0) AS `PDP_VAT`,
                                            IFNULL(`A`.`PDP_ECTCOST`, 0) AS `PDP_ECTCOST`, 
                                            IFNULL(`A`.`PDP_TOTCOST`, 0) AS `PDP_TOTCOST`, 
                                            IFNULL(`A`.`PDP_BE_COST`, 0) AS `PDP_BE_COST`,
                                            IFNULL(`A`.`PDP_CMPY`, '') AS `PDP_CMPY`, IFNULL((SELECT CMPNY_NM FROM PDCMPNY WHERE PDCMPNY.CMPNY_IDX = `A`.`PDP_CMPY`),'') AS `COMP_NM`,
                                            `PDP_PRIENT`
                                            FROM PDPUSCH `A` LEFT JOIN pdpart `B` ON `A`.`PDP_PART` = `B`.`pdpart_IDX`
                                            WHERE `A`.`PDP_REC_YN` ='Y' AND `A`.`PDP_CONF` = 'Report' AND `PDP_CNF_YN` = 'N' AND `A`.`PDP_NO` = '" + TextBox1 + "'";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView3 = new List<SuperResultTranfer>();
                            if (ds.Tables.Count > 0)
                            {
                                result.DataGridView3 = SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]);
                            }
                        }
                    }
                    else if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string OR_NO_CHK = BaseParameter.ListSearchString[0];
                            string CMPY_NO_CHK = BaseParameter.ListSearchString[1];
                            string value = BaseParameter.ListSearchString[2];

                            string whereClause = "";

                            if (OR_NO_CHK == "true")
                            {
                                whereClause = "AND `A`.`PDP_NO` = @value AND NOT(`A`.`PDP_PRIENT` = 'C')";
                            }
                            else if (CMPY_NO_CHK == "true")
                            {
                                whereClause = "AND `A`.`PDP_COMPY_NO` = @value AND NOT(`A`.`PDP_PRIENT` = 'Y')";
                            }

                            string sql = @"SELECT 
                       `A`.`PDP_CONF`,`A`.`PDP_NO`, `A`.`PDP_DATE1`, 
                       (SELECT `CD_SYS_NOTE` FROM PDCDNM WHERE `CD_IDX` = `A`.`PDP_DEPA`) AS `DEPARTMENT`,
                       `B`.`PN_NM`, `B`.`PN_V`, `B`.`PSPEC_V`, 
                       (SELECT `CD_NM_VN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_VN`,
                       `B`.`PN_K`, `B`.`PSPEC_K`, 
                       (SELECT `CD_NM_HAN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_KR`, 
                       `B`.`PQTY`, `A`.`PDP_QTY`,
                       `A`.`PDP_MEMO`, `A`.`CREATE_DTM`, `A`.`CREATE_USER`, 
                       (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `A`.`CREATE_USER`) AS `USER_NAME`, 
                       IFNULL(`A`.`PDP_REMARK`, '') AS `PDP_REMARK`, `A`.`PDP_PART`,
                       IFNULL((SELECT `QTY` FROM pd_tiivtr WHERE `PART_IDX` = `A`.`PDP_PART` AND `A`.`PDPUSCH_IDX` = `ORDER_IDX`),0) AS `STOCK`, 
                       `A`.`PDPUSCH_IDX`, 
                       `A`.`PDP_COST`,(`A`.`PDP_COST` * `A`.`PDP_QTY`) AS `SUM_COST`,  
                       `A`.`PDP_VAT`, `A`.`PDP_ECTCOST`, `A`.`PDP_TOTCOST`, `A`.`PDP_BE_COST`,
                       IFNULL(`A`.`PDP_CMPY`, '') AS `PDP_CMPY`, 
                       IFNULL((SELECT CMPNY_NM FROM PDCMPNY WHERE PDCMPNY.CMPNY_IDX = `A`.`PDP_CMPY`),'') AS `COMP_NM`
                       FROM PDPUSCH `A` LEFT JOIN pdpart `B` ON `A`.`PDP_PART` = `B`.`pdpart_IDX`
                       WHERE `A`.`PDP_REC_YN` ='Y' AND `A`.`PDP_CONF` = 'Report' AND `A`.`PDP_CNF_YN` = 'N' "
                                           + whereClause;

                            MySqlParameter[] parameters = new MySqlParameter[]
                            {
            new MySqlParameter("@value", value)
                            };

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql, parameters);
                            result.DataGridView2 = new List<SuperResultTranfer>();
                            if (ds.Tables.Count > 0)
                            {
                                result.DataGridView2 = SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]);
                            }
                        }
                    }
                    else if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string SU01 = BaseParameter.ListSearchString[0];
                            string SU02 = BaseParameter.ListSearchString[1];
                            string SU03 = BaseParameter.ListSearchString[2];
                            string SU04 = BaseParameter.ListSearchString[3];
                            string SU05 = BaseParameter.ListSearchString[4];
                            string SU06 = BaseParameter.ListSearchString[5];
                            string SU07 = BaseParameter.ListSearchString[6];
                            string SU08 = BaseParameter.ListSearchString[7];
                            string SU09 = BaseParameter.ListSearchString[8];
                            string SU10 = BaseParameter.ListSearchString[9];

                            string OS_DATE = "";

                            if (SU10 == "order")
                                OS_DATE = " AND `A`.`PDP_DATE1` >= '" + SU01 + "' AND `A`.`PDP_DATE1` <= '" + SU02 + "' ";
                            else if (SU10 == "confirm")
                                OS_DATE = " AND `A`.`PDP_CNF_DATE` >= '" + SU01 + "' AND `A`.`PDP_CNF_DATE` <= '" + SU02 + "' ";
                            else if (SU10 == "create")
                                OS_DATE = " AND DATE(`A`.`CREATE_DTM`) >= '" + SU01 + "' AND DATE(`A`.`CREATE_DTM`) <= '" + SU02 + "' ";

                            string sql = @"SELECT `A`.`PDP_CONF`,`A`.`PDP_NO`, IFNULL(`A`.`PDP_REMARK`, '') AS `PDP_REMARK`, `A`.`PDP_DATE1`, (SELECT `CD_SYS_NOTE` FROM PDCDNM WHERE `CD_IDX` = `A`.`PDP_DEPA`) AS `DEP`,
                                        `B`.`PN_NM`, `B`.`PQTY`, IFNULL((SELECT `QTY` FROM pd_tiivtr WHERE `PART_IDX` = `A`.`PDP_PART` AND `A`.`PDPUSCH_IDX` = `ORDER_IDX`),0) AS `STOCK`, `A`.`PDP_BE_COST`,
                                        `A`.`PDP_QTY`, `A`.`PDP_COST`,(`A`.`PDP_COST` * `A`.`PDP_QTY`) AS `SUM_COST`, `A`.`PDP_ECTCOST`, `A`.`PDP_VAT`, `A`.`PDP_TOTCOST`,  
                                        IFNULL(DATE_FORMAT(`A`.`PDP_CNF_DATE`, '%Y-%m-%d'), '----') AS `PDP_CNF_DATE`,   `A`.`PDP_MEMO`, 
                                        IFNULL(`A`.`PDP_CMPY`, '') AS `PDP_CMPY`, IFNULL((SELECT CMPNY_NM FROM PDCMPNY WHERE PDCMPNY.CMPNY_IDX = `A`.`PDP_CMPY`),'') AS `COMP_NM`,  `A`.`CREATE_DTM`, 
                                        `A`.`CREATE_USER`, (SELECT `USER_NM` FROM tsuser WHERE `USER_ID` = `A`.`CREATE_USER`) AS `USER_NAME`, `A`.`PDP_PART`, `A`.`PDPUSCH_IDX`, 
                                         `B`.`PN_V`, `B`.`PSPEC_V`, (SELECT `CD_NM_VN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_VN`,
                                        `B`.`PN_K`, `B`.`PSPEC_K`, (SELECT `CD_NM_HAN` FROM PDCDNM WHERE `CD_IDX` = `B`.`PUNIT_IDX`) AS `UNIT_KR`, 
                                        IFNULL(`PDP_ORD_ST`, '0') AS `ORDER_ST`, IFNULL(`PDP_IN_QTY`, 0) AS `PDP_IN_QTY`, IFNULL(`PDP_FIFO`, 'N') AS `PDP_FIFO`
                                        FROM PDPUSCH `A` LEFT JOIN pdpart `B` ON `A`.`PDP_PART` = `B`.`pdpart_IDX`
                                        WHERE
                                        `A`.`PDP_REC_YN` ='Y' AND `A`.`PDP_CONF` = 'Order' AND `PDP_CNF_YN` = 'Y' AND 
                                        `B`.`PN_NM` LIKE '%" + SU03 + @"%' AND (`B`.`PN_V` LIKE '%" + SU04 + @"%'  OR `B`.`PN_K` LIKE '%" + SU04 + @"%') AND 
                                        (`B`.`PSPEC_V`  LIKE '%" + SU05 + @"%' OR `B`.`PSPEC_K` LIKE '%" + SU05 + @"%') AND 
                                        (SELECT `CD_SYS_NOTE` FROM PDCDNM WHERE `CD_IDX` = `A`.`PDP_DEPA`) LIKE '%" + SU07 + @"%' AND
                                        `A`.`PDP_NO` LIKE '%" + SU08 + @"%'  " + OS_DATE + @"
                                        HAVING `COMP_NM` LIKE '%" + SU06 + @"%'   AND `ORDER_ST` LIKE '" + SU09 + @"' ";

                            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                            result.DataGridView4 = new List<SuperResultTranfer>();
                            if (ds.Tables.Count > 0)
                                result.DataGridView4 = SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]);
                        }
                    }
                    else if (BaseParameter.Action == 4)
                    {
                        // Tab 4
                        if (BaseParameter.ListSearchString != null)
                        {
                            string DATE_D1 = BaseParameter.ListSearchString[0];
                            string DATE_D2 = BaseParameter.ListSearchString[1];

                            string departmentQuery = "SELECT `CD_IDX`, `CD_SYS_NOTE` FROM PDCDNM WHERE `CDGR_IDX` ='2' ORDER BY `CD_SYS_NOTE`";
                            DataSet dsDepartments = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, departmentQuery);

                            StringBuilder departmentColumns = new StringBuilder();
                            for (int i = 0; i < dsDepartments.Tables[0].Rows.Count; i++)
                            {
                                string deptCode = dsDepartments.Tables[0].Rows[i]["CD_IDX"].ToString();
                                string deptName = dsDepartments.Tables[0].Rows[i]["CD_SYS_NOTE"].ToString();

                                string departmentColumn = $"IFNULL(SUM(CASE WHEN `PDP_DEPA` = '{deptCode}' THEN ROUND(((IFNULL(PDPUSCH.PDP_QTY, 0) * IFNULL(PDPUSCH.PDP_COST, 0)) + IFNULL(PDPUSCH.PDP_VAT, 0) + IFNULL(PDPUSCH.PDP_ECTCOST, 0)), 2) END), 0) AS '{deptName}'";

                                if (departmentColumns.Length > 0)
                                    departmentColumns.Append(", ");

                                departmentColumns.Append(departmentColumn);
                            }

                            string year = DateTime.Parse(DATE_D1).ToString("yyyy");
                            string yearStart = $"{year}-01-01 00:00:00";
                            string yearEnd = $"{year}-12-31 23:59:59";

                            string query = $@"
                                            SELECT '{year}' AS `DATE`, IFNULL(SUM(PDP_TOTCOST), 0) AS `TOTAL`, {departmentColumns}
                                            FROM PDPUSCH 
                                            WHERE PDP_DATE1 >= '{yearStart}' AND PDP_DATE1 <= '{yearEnd}' 
                                            AND PDP_CNF_YN = 'Y' AND NOT(IFNULL(PDP_ORD_ST, 0) ='4')

                                            UNION

                                            SELECT '{DATE_D1} ~ {DATE_D2}' AS `DATE`, IFNULL(SUM(PDP_TOTCOST), 0) AS `TOTAL`, {departmentColumns}
                                            FROM PDPUSCH 
                                            WHERE PDP_DATE1 >= '{DATE_D1} 00:00:00' AND PDP_DATE1 <= '{DATE_D2} 23:59:59' 
                                            AND PDP_CNF_YN = 'Y' AND NOT(IFNULL(PDP_ORD_ST, 0) ='4')";

                            DataSet dsResult = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, query);

                           
                            if (dsResult.Tables.Count > 0 && dsResult.Tables[0].Rows.Count > 0)
                            {
                                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

                                foreach (DataRow row in dsResult.Tables[0].Rows)
                                {
                                    Dictionary<string, object> item = new Dictionary<string, object>();

                                    foreach (DataColumn column in dsResult.Tables[0].Columns)
                                    {
                                        if (row[column] != DBNull.Value)
                                        {
                                            item[column.ColumnName] = row[column];
                                        }
                                        else
                                        {
                                            item[column.ColumnName] = null;
                                        }
                                    }

                                    data.Add(item);
                                }

                                result.DataGridView5Raw = data;
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
        public virtual async Task<BaseResult> DataGridView3_SelectionChanged(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        string PART_IDX = BaseParameter.ListSearchString[0];


                        string sql = @"SELECT `C11`.`STATE`, `C11`.`PDP_PART`, `C11`.`PDP_CMPY`, `C11`.`PDP_COST`, `C11`.`PDP_VAT`, `C11`.`PDP_ECTCOST`, `C11`.`PDP_TOTCOST`, `C11`.`PDPUSCH_IDX`
                                        FROM (SELECT  'LAST' AS `STATE`, `C1`.`PDP_PART`, `C1`.`PDP_CMPY`, IFNULL(`C1`.`PDP_COST`, 0) AS `PDP_COST`, IFNULL(`C1`.`PDP_VAT`, 0) AS `PDP_VAT`, 
                                        IFNULL(`C1`.`PDP_ECTCOST`,0) AS `PDP_ECTCOST`, IFNULL(`C1`.`PDP_TOTCOST`,0) AS `PDP_TOTCOST`, (`C1`.`PDPUSCH_IDX`) AS `PDPUSCH_IDX`
                                        FROM PDPUSCH `C1` WHERE `C1`.`PDP_PART` = '" + PART_IDX + @"' AND  `C1`.`PDP_CNF_YN` = 'Y'  AND `C1`.`CREATE_DTM` > DATE_ADD(NOW(), INTERVAL -5 MONTH)   AND NOT(`C1`.`PDP_CONF`='DEL')
                                        AND NOT(IFNULL(`C1`.`PDP_COST`, 0) = 0)  ORDER BY `C1`.`CREATE_DTM` DESC LIMIT 1) `C11`

                                        UNION
                                        SELECT 'MAX' AS `STATE`, `C2`.`PDP_PART`, `C2`.`PDP_CMPY`, IFNULL(MAX(`C2`.`PDP_COST`), 0) AS `PDP_COST`, IFNULL(`C2`.`PDP_VAT`, 0) AS `PDP_VAT`,  
                                        IFNULL(`C2`.`PDP_ECTCOST`,0) AS `PDP_ECTCOST`, IFNULL(`C2`.`PDP_TOTCOST`,0) AS `PDP_TOTCOST`, `C2`.PDPUSCH_IDX
                                        FROM PDPUSCH `C2` WHERE `C2`.`PDP_PART` = '" + PART_IDX + @"' AND  `C2`.`PDP_CNF_YN` = 'Y'AND `C2`.`CREATE_DTM` > DATE_ADD(NOW(), INTERVAL -5 MONTH)
                                        AND NOT(`C2`.`PDP_CONF`='DEL')
                                        AND NOT(IFNULL(`C2`.`PDP_COST`, 0) = 0)

                                        UNION
                                        SELECT 'MIN' AS `STATE`, `C3`.`PDP_PART`, `C3`.`PDP_CMPY`, IFNULL(MIN(`C3`.`PDP_COST`),0) AS `PDP_COST`, IFNULL(`C3`.`PDP_VAT`, 0) AS `PDP_VAT`,  
                                        IFNULL(`C3`.`PDP_ECTCOST`,0) AS `PDP_ECTCOST`, IFNULL(`C3`.`PDP_TOTCOST`,0) AS `PDP_TOTCOST`, `C3`.PDPUSCH_IDX
                                        FROM PDPUSCH `C3` WHERE `C3`.`PDP_PART` = '" + PART_IDX + @"' AND  `C3`.`PDP_CNF_YN` = 'Y' AND `C3`.`CREATE_DTM` > DATE_ADD(NOW(), INTERVAL -5 MONTH)  
                                        AND NOT(`C3`.`PDP_CONF`='DEL')
                                        AND NOT(IFNULL(`C3`.`PDP_COST`, 0) = 0)";

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
                    if (BaseParameter.ListSearchString != null)
                    {
                        string TextBox7 = BaseParameter.ListSearchString[0];
                        string TextBox_C1 = BaseParameter.ListSearchString[1];
                        string TextBox_C3 = BaseParameter.ListSearchString[2];
                        string TextBox_C4 = BaseParameter.ListSearchString[3];
                        string TextBox_C5 = BaseParameter.ListSearchString[4];
                        string PDP_BE_COST = BaseParameter.ListSearchString[5];
                        string PDPUSCH_IDX = BaseParameter.ListSearchString[6];
                        string USER_ID = BaseParameter.USER_IDX;


                        string sql = @"UPDATE `PDPUSCH` SET 
                                        `PDP_CMPY`='" + TextBox7 + @"', 
                                        `PDP_COST`='" + TextBox_C1 + @"', 
                                        `PDP_VAT`='" + TextBox_C3 + @"', 
                                        `PDP_ECTCOST`='" + TextBox_C4 + @"', 
                                        `PDP_TOTCOST`='" + TextBox_C5 + @"', 
                                        `PDP_BE_COST`='" + PDP_BE_COST + @"', 
                                        `UPDATE_DTM`= NOW(), 
                                        `UPDATE_USER`='" + USER_ID + @"' 
                                        WHERE `PDPUSCH_IDX`='" + PDPUSCH_IDX + "'";

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);


                        string TextBox1 = BaseParameter.ListSearchString[7];
                        if (TextBox1.StartsWith("99"))
                        {
                            string TextBox8 = BaseParameter.ListSearchString[8];
                            string Label8 = BaseParameter.ListSearchString[9];
                            string DGV_PINDEX = BaseParameter.ListSearchString[10];


                            double TOT_C = double.Parse(TextBox_C5) / double.Parse(Label8);
                            string TOT_CMPY = TextBox8;

                            string mcOrderSql = @"UPDATE `PD_MC_ORDERLIST` SET 
                                                `PD_MC_UNIT` = 'VND', 
                                                `PD_MC_COST`= '" + TOT_C + @"', 
                                                `PD_MC_CMPYNM`= '" + TOT_CMPY + @"', 
                                                `PD_MC_STAY`= 'Quote reception' 
                                                WHERE `PD_ORDER_NO`= '" + TextBox1 + "' AND `PD_PART_IDX` = '" + DGV_PINDEX + "'";

                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, mcOrderSql);
                        }

                        result.Success = true;
                    }


                }
            }
            catch (Exception ex)
            {
                result.Success = false;
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
        // ⭐ THAY THẾ TOÀN BỘ METHOD NÀY:

        public virtual async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.Action == 2 && BaseParameter.ListSearchString != null)
                    {
                        string OR_NO_CHK = BaseParameter.ListSearchString[0];
                        string CMPY_NO_CHK = BaseParameter.ListSearchString[1];
                        string value = BaseParameter.ListSearchString[2];
                        string USER_ID = BaseParameter.USER_IDX;

                        string sql = "";

                        if (OR_NO_CHK == "true")
                        {
                            sql = @"UPDATE `PDPUSCH` SET 
                           `PDP_CONF` = 'Order', 
                           `PDP_CNF_YN` = 'Y', 
                           `PDP_DATE2` = NOW(), 
                           `PDP_CNF_DATE` = NOW(),
                           `UPDATE_DTM` = NOW(), 
                           `UPDATE_USER` = @userId 
                           WHERE `PDP_NO` = @value 
                           AND `PDP_CONF` = 'Report' 
                           AND NOT(`PDP_PRIENT` = 'C')";
                        }
                        else if (CMPY_NO_CHK == "true")
                        {
                            sql = @"UPDATE `PDPUSCH` SET 
                           `PDP_CONF` = 'Order', 
                           `PDP_CNF_YN` = 'Y', 
                           `PDP_DATE2` = NOW(), 
                           `PDP_CNF_DATE` = NOW(),
                           `UPDATE_DTM` = NOW(), 
                           `UPDATE_USER` = @userId 
                           WHERE `PDP_COMPY_NO` = @value 
                           AND `PDP_CONF` = 'Report' 
                           AND NOT(`PDP_PRIENT` = 'Y')";
                        }
                        else
                        {
                            result.Success = false;
                            result.Error = "Please select mode.";
                            return result;
                        }

                        MySqlParameter[] parameters = new MySqlParameter[]
                        {
                    new MySqlParameter("@userId", USER_ID),
                    new MySqlParameter("@value", value)
                        };

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql, parameters);
                        result.Success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
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
                    if (BaseParameter.Action == 2 && BaseParameter.ListSearchString != null)
                    {
                        string orderNo = BaseParameter.ListSearchString[0];
                        string USER_ID = BaseParameter.USER_IDX;

                        string sql = @"UPDATE `PDPUSCH` SET 
                                       `PDP_CONF` = 'DEL', 
                                       `PDP_CNF_YN` = 'N', 
                                       `PDP_DATE2` = NOW(), 
                                       `UPDATE_DTM` = NOW(), 
                                       `UPDATE_USER` = '" + USER_ID + @"' 
                                       WHERE `PDP_NO` = '" + orderNo + "'";

                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Success = true;
                    }
                    if (BaseParameter.Action == 3)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string IDX = BaseParameter.ListSearchString[0];
                            string PUR_IDX = BaseParameter.ListSearchString[1];
                            string ORDER_ST = BaseParameter.ListSearchString[2];
                            string USER_ID = BaseParameter.USER_IDX;

                            if (ORDER_ST == "3")
                            {
                                result.Success = false;
                                return result;
                            }

                            if (ORDER_ST == "5")
                            {
                                string SQL1 = "UPDATE `PDPUSCH` SET `PDP_ORD_ST`= '3', `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + USER_ID + "'  WHERE  `PDPUSCH_IDX`='" + IDX + "'";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);
                            }
                            else
                            {
                                string SQL1 = "UPDATE `PDPUSCH` SET `PDP_CONF`= 'DEL', `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + USER_ID + "'  WHERE  `PDPUSCH_IDX`='" + IDX + "'";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);

                                SQL1 = "UPDATE `pd_tiivtr` SET `QTY`='0', `UPDATE_DTM`= NOW(), `UPDATE_USER`='" + USER_ID + "' WHERE `ORDER_IDX` = '" + PUR_IDX + "'";
                                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);
                            }

                            result.Success = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> DataGridView4_CellClick(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ListSearchString != null)
                {
                    string OR_NO = BaseParameter.ListSearchString[0];
                    string ColumnIndex = BaseParameter.ListSearchString[1];
                    string PDP_IN_QTY = BaseParameter.ListSearchString[2];
                    string USER_ID = BaseParameter.USER_IDX;

                    string SQL1 = "";

                    if (ColumnIndex == "1") // Waiting
                    {
                        if (int.Parse(PDP_IN_QTY) <= 0)
                        {
                            SQL1 = "UPDATE `PDPUSCH` SET `PDP_ORD_ST`='1', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "'  WHERE  `PDPUSCH_IDX`='" + OR_NO + "'";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);
                        }
                    }
                    else if (ColumnIndex == "2") // Cancel
                    {
                        SQL1 = "UPDATE `PDPUSCH` SET `PDP_ORD_ST`='4', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `PDPUSCH_IDX`='" + OR_NO + "'";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);
                    }
                    else if (ColumnIndex == "3") // Shipping or In Progress
                    {
                        if (int.Parse(PDP_IN_QTY) > 0)
                        {
                            SQL1 = "UPDATE `PDPUSCH` SET `PDP_ORD_ST`='5', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `PDPUSCH_IDX`='" + OR_NO + "'";
                        }
                        else
                        {
                            SQL1 = "UPDATE `PDPUSCH` SET `PDP_ORD_ST`='2', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `PDPUSCH_IDX`='" + OR_NO + "'";
                        }
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);
                    }

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = ex.Message;
            }
            return result;
        }

        // Hàm xử lý All In
        public virtual async Task<BaseResult> Button4_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null && BaseParameter.ListSearchString != null)
                {
                    string AAA = BaseParameter.ListSearchString[0];
                    string BBB = BaseParameter.ListSearchString[1];
                    string CCC = BaseParameter.ListSearchString[2];
                    string DDD = BaseParameter.ListSearchString[3];
                    string IN_QTY = BaseParameter.ListSearchString[4];
                    string USER_ID = BaseParameter.USER_IDX;

                    string SQL1 = "INSERT INTO `pd_inout_part` (`ORDER_IDX`, `pdpart_IDX`, `DSC_INOUT`, `PUR_QTY`, `PUR_TIME`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + AAA + "', '" + BBB + "', 'IN', '" + IN_QTY + "', NOW(), NOW(), '" + USER_ID + "')";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);

                    SQL1 = "INSERT INTO `pd_tiivtr` (`PART_IDX`, `ORDER_IDX`, `LOC_IDX`, `QTY`, `IMP_YN`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + BBB + "', '" + AAA + "', '0', '" + CCC + "', 'N', NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `QTY` = VALUES(`QTY`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);

                    if (int.Parse(CCC) >= int.Parse(DDD))
                    {
                        SQL1 = "UPDATE `PDPUSCH` SET `PDP_ORD_ST`='3', `PDP_IN_QTY`='" + DDD + "', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `PDPUSCH_IDX`='" + AAA + "'";
                    }
                    else
                    {
                        SQL1 = "UPDATE `PDPUSCH` SET `PDP_ORD_ST`='5', `PDP_IN_QTY`='" + DDD + "', `UPDATE_DTM` = NOW(), `UPDATE_USER` = '" + USER_ID + "' WHERE  `PDPUSCH_IDX`='" + AAA + "'";
                    }
                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);

                    try
                    {
                        SQL1 = "ALTER TABLE `pd_inout_part` AUTO_INCREMENT=1";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);
                    }
                    catch { }

                    try
                    {
                        SQL1 = "ALTER TABLE `pd_tiivtr` AUTO_INCREMENT=1";
                        await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, SQL1);
                    }
                    catch { }

                    result.Success = true;
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Error = ex.Message;
            }
            return result;
        }
        // Trong V03Service
        public virtual async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
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
                if (BaseParameter != null && BaseParameter.Action == 1 &&
                    BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count >= 22)
                {
                    StringBuilder SearchString = new StringBuilder();
                    string SheetName = "V03";

                    // Lấy thông tin factory
                    string factorySelected = BaseParameter.ListSearchString[21];

                    // Các tham số chính
                    string DGV_NO = BaseParameter.ListSearchString[0];
                    string DGV_DATE = BaseParameter.ListSearchString[3];
                    string DGV_SUM_COST = BaseParameter.ListSearchString[14];
                    string DGV_VAT = BaseParameter.ListSearchString[15];
                    string DGV_ECT_COST = BaseParameter.ListSearchString[16];
                    string DGV_TOT_COST = BaseParameter.ListSearchString[17];
                    string USER_IDX = BaseParameter.ListSearchString[19];
                    string orderNo = BaseParameter.ListSearchString[20];

                    // Nội dung dòng
                    string contentRows = "";
                    if (BaseParameter.ListSearchString.Count > 22)
                    {
                        contentRows = BaseParameter.ListSearchString[22];
                    }

                    // Chọn template theo factory
                    string templateFileName = factorySelected == "1" ? "V03Tab1.html" : "V03Tab1F2.html";

                    // Đọc template
                    string templateContent = GlobalHelper.InitializationString;
                    string folderPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, templateFileName);
                    using (FileStream fs = new FileStream(folderPath, FileMode.Open))
                    {
                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        {
                            templateContent = r.ReadToEnd();
                        }
                    }

                    // Thay thế placeholders
                    var replacements = new Dictionary<string, string>
                    {
                        ["[DGV_NO]"] = string.IsNullOrEmpty(DGV_NO) ? "-" : DGV_NO,
                        ["[DGV_DATE]"] = string.IsNullOrEmpty(DGV_DATE) ? "-" : DGV_DATE,
                        ["[DGV_PRINT_DATE]"] = DateTime.Now.ToString("yyyy-MM-dd"),
                        ["[DGV_CONTENT]"] = contentRows,
                        ["[DGV_SUM_COST]"] = string.IsNullOrEmpty(DGV_SUM_COST) ? "0" : DGV_SUM_COST,
                        ["[DGV_VAT]"] = string.IsNullOrEmpty(DGV_VAT) ? "0" : DGV_VAT,
                        ["[DGV_ECT_COST]"] = string.IsNullOrEmpty(DGV_ECT_COST) ? "0" : DGV_ECT_COST,
                        ["[DGV_TOT_COST]"] = string.IsNullOrEmpty(DGV_TOT_COST) ? "0" : DGV_TOT_COST
                    };

                    foreach (var replacement in replacements)
                    {
                        templateContent = templateContent.Replace(replacement.Key, replacement.Value);
                    }

                    SearchString.AppendLine(templateContent);

                    // Tạo trang HTML hoàn chỉnh
                    string contentHTML = GlobalHelper.InitializationString;
                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "EmptyPortrait.html");
                    using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                    {
                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        {
                            contentHTML = r.ReadToEnd();
                        }
                    }
                    contentHTML = contentHTML.Replace(@"[Content]", SearchString.ToString());

                    // Tạo file và lưu
                    string fileName = "V03Tab1_" + DateTime.Now.Ticks + ".html";
                    string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                    Directory.CreateDirectory(physicalPathCreate);
                    GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                    string filePath = Path.Combine(physicalPathCreate, fileName);

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            await w.WriteLineAsync(contentHTML);
                        }
                    }

                    // Cập nhật trạng thái in
                    string updateSql = @"UPDATE `PDPUSCH` SET `PDP_PRIENT`='Y', `UPDATE_DTM` = NOW(), 
                  `UPDATE_USER` = @userId WHERE `PDP_NO`= @orderNo";

                    MySqlParameter[] updateParams = new MySqlParameter[]
                    {
                new MySqlParameter("@userId", USER_IDX),
                new MySqlParameter("@orderNo", orderNo)
                    };

                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, updateSql, updateParams);

                    // Trả về URL
                    result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                    result.Success = true;
                }
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

