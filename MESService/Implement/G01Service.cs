
namespace MESService.Implement
{
    public class G01Service : BaseService<torderlist, ItorderlistRepository>, IG01Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public G01Service(ItorderlistRepository torderlistRepository, IWebHostEnvironment webHostEnvironment)
            : base(torderlistRepository)
        {
            _torderlistRepository = torderlistRepository;
            _WebHostEnvironment = webHostEnvironment;
        }

        public override void Initialization(torderlist model)
        {
            BaseInitialization(model);
        }

        public async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                await Task.Run(() => { });

                //BaseParameter defaultParam = new BaseParameter
                //{
                //    Action = 1,
                //    ListSearchString = new List<string> { "", "" }
                //};
                //result = await Buttonfind_Click(defaultParam);
                //result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = $"Error Code: 99990, Không thể kết nối MES: {ex.Message}";
            }
            return result;
        }

        public async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter.Action == 1)
                {
                    string AA = $"%{BaseParameter.ListSearchString[0] ?? ""}%";
                    string BB = $"%{BaseParameter.ListSearchString[1] ?? ""}%";

                    string sql = @"SELECT * FROM (
                        SELECT 
                            `PART_IDX` AS `PART_CODE`, 
                            (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX`) AS `PART_NO`,
                            (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX`) AS `PART_NAME`,
                            IFNULL(SUM(case when LOC_IDX = 1 then `QTY` END), 0) AS `Raw_Material`,
                            IFNULL(SUM(case when LOC_IDX = 2 then `QTY` END), 0) AS `Finish_Goods`,
                            IFNULL(SUM(case when LOC_IDX = 7 then `QTY` END), 0) AS `WIP`,
                            IFNULL(SUM(case when LOC_IDX = 1 OR LOC_IDX = 2 OR LOC_IDX = 7 then `QTY` END), 0) AS `SUM`
                        FROM tiivtr 
                        GROUP BY `PART_IDX`
                    ) AS `DBA` 
                    WHERE `DBA`.`PART_NO` LIKE @AA 
                    AND `DBA`.`PART_NAME` LIKE @BB 
                    ORDER BY `DBA`.`PART_NO` ASC";

                    if (BaseParameter.CheckBox1 == true)
                    {
                        sql = sql + " LIMIT " + GlobalHelper.ListCount;
                    }

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql,
                    new MySqlParameter("@AA", AA),
                    new MySqlParameter("@BB", BB));

                    result.G01Overview = new List<dynamic>();
                    if (ds.Tables.Count > 0)
                    {
                        result.G01Overview.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
                    }
                }
                else if (BaseParameter.Action == 2)
                {
                    string AA = $"%{BaseParameter.ListSearchString[0] ?? ""}%";
                    string BB = BaseParameter.ListSearchString[1] == "ALL" ? "%%" : $"%{BaseParameter.ListSearchString[1] ?? ""}%";
                    string CC = $"%{BaseParameter.ListSearchString[2] ?? ""}%";

                    string sql = @"SELECT 
                        tiivtr.PART_IDX, 
                        (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) AS `PART_NO`, 
                        (SELECT tspart.PART_NM FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) AS `PART_NM`, 
                        IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '1' AND tiivaj.PART_IDX = tiivtr.PART_IDX), 0) AS `Change_QTY`, 
                        IFNULL(`TB_B`.`Incoming_QTY`, 0) AS `Incoming_QTY`, 
                        IFNULL(`TB_B`.`OUT_QTY`, 0) AS `OUT_QTY`,
                        IFNULL(`TB_B`.`Incoming_QTY`, 0) - IFNULL(`TB_B`.`OUT_QTY`, 0) + IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '1' AND tiivaj.PART_IDX = tiivtr.PART_IDX), 0) - IFNULL(tiivtr.QTY, 0) AS `Difference_QTY`,
                        IFNULL(tiivtr.QTY, 0) AS `STOCK_QTY`, 
                        IFNULL((SELECT tiivtr_EXCEL.QTY FROM tiivtr_EXCEL WHERE tiivtr_EXCEL.`PART_NO` = (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX)), 0) AS `MT_EXCEL`,
                        IF((IFNULL(`TB_B`.`Incoming_QTY`, 0) - IFNULL(`TB_B`.`OUT_QTY`, 0) + IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '1' AND tiivaj.PART_IDX = tiivtr.PART_IDX), 0) - IFNULL(tiivtr.QTY, 0)) = 0, 'Good', 'Bad') AS `Verification`,
                        (SELECT tspart.PART_LOC FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) AS `Location` 
                        FROM tiivtr LEFT JOIN 
                        (SELECT `A`.`PART_CODE`, SUM(`A`.`Incoming_QTY`) AS `Incoming_QTY`, 
                            SUM(`A`.`OUT_QTY`) AS `OUT_QTY` 
                        FROM (
                            SELECT 
                                (SELECT TMMTIN.PART_IDX FROM TMMTIN WHERE TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX) AS `PART_CODE`, 
                                IF(TMBRCD.BBCO = 'Y', TMBRCD.`PKG_QTY`, 0) AS `Incoming_QTY`, 
                                IFNULL(TMBRCD.`PKG_OUTQTY`, 0) AS `OUT_QTY`
                            FROM TMBRCD
                        ) AS `A`
                        GROUP BY `A`.`PART_CODE`) `TB_B`
                        ON tiivtr.PART_IDX = `TB_B`.`PART_CODE`
                        WHERE tiivtr.LOC_IDX = '1' 
                        AND (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) LIKE @AA 
                        AND (SELECT tspart.PART_LOC FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) LIKE @CC 
                        HAVING `Verification` LIKE @BB 
                        ORDER BY `Verification` ASC, `Difference_QTY` ASC, `PART_NO` ASC";

                    //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql,
                        new MySqlParameter("@AA", AA),
                        new MySqlParameter("@BB", BB),
                        new MySqlParameter("@CC", CC));

                    result.G01RawMaterial = new List<dynamic>();
                    if (ds.Tables.Count > 0)
                    {
                        result.G01RawMaterial.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
                        int badCount = 0;
                        int goodCount = 0;
                        foreach (var row in result.G01RawMaterial)
                        {
                            if (row.Verification == "Bad")
                            {
                                badCount++;
                            }
                            else
                            {
                                goodCount++;
                            }
                        }
                        result.G01RawMaterialGood = goodCount;
                        result.G01RawMaterialBad = badCount;

                        result.Data = new Dictionary<string, string>
                        {
                            { "MT_CM07", "AliceBlue" },
                            { "MT_CM08", "LightGray" },
                            { "MT_CM10", "MistyRose" }
                        };
                    }

                    //result.G01RawMaterial = result.G01RawMaterial.Take(GlobalHelper.ListCount.Value).ToList();

                }
                else if (BaseParameter.Action == 3)
                {
                    string AA = $"%{BaseParameter.ListSearchString[0] ?? ""}%";
                    string BB = BaseParameter.ListSearchString[1] == "ALL" ? "%%" : $"%{BaseParameter.ListSearchString[1] ?? ""}%";
                    string CC = $"%{BaseParameter.ListSearchString[2] ?? ""}%";

                    string sql = @"SELECT `A`.`PART_IDX`, `A`.`PART_NO`, `A`.`PART_NM`, `A`.`Change_QTY`, `A`.`Incoming_QTY`, `A`.`OUT_QTY`,  
        `A`.`STOCK_QTY` AS `MES_STOCK`,
        IFNULL(`A`.`REWORK_QTY`, 0) AS `REWORK_QTY`,
        IFNULL((SELECT tiivtr_EXCEL.QTY FROM tiivtr_EXCEL WHERE tiivtr_EXCEL.`PART_NO` = `A`.`PART_NO`), 0) AS `FG_EXCEL`,
        (`A`.`Incoming_QTY` - `A`.`OUT_QTY` + IFNULL(`A`.`Change_QTY`, 0) - `A`.`STOCK_QTY`) AS `Difference_QTY`,
        IF((`A`.`Incoming_QTY` - `A`.`OUT_QTY` + IFNULL(`A`.`Change_QTY`, 0) - `A`.`STOCK_QTY`) = 0, 'Good', 'Bad') AS `Verification`, `A`.`Location`
        FROM 
        (SELECT tiivtr.`PART_IDX`,
            (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX`) AS `PART_NO`,
            (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX`) AS `PART_NM`,
            IFNULL(`TB_B`.`Incoming_QTY`, 0) AS `Incoming_QTY`, 
            IFNULL(`TB_B`.`OUT_QTY`, 0) AS `OUT_QTY`,
            IFNULL(tiivtr.`QTY`, 0) AS `MES_STOCK`,
            IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '2' AND tiivaj.PART_IDX = tiivtr.`PART_IDX`), 0) AS `Change_QTY`,
            IFNULL(tiivtr.QTY, 0) AS `STOCK_QTY`,
            IFNULL((SELECT t3.QTY FROM tiivtr t3 WHERE t3.PART_IDX = tiivtr.PART_IDX AND t3.LOC_IDX = '3'), 0) AS `REWORK_QTY`,
            (SELECT tspart.PART_LOC FROM tspart WHERE tspart.PART_IDX = tiivtr.`PART_IDX`) AS `Location`
        FROM tiivtr LEFT JOIN 
            (SELECT tdpdmtim.VLID_PART_IDX AS `PART_CODE`,  
                COUNT(tdpdmtim.VLID_PART_IDX) AS `Incoming_QTY`, 
                SUM(IF(tdpdmtim.VLID_DSCN_YN = 'Y', 1, 0)) AS `OUT_QTY`
            FROM tdpdmtim 
            GROUP BY `PART_CODE`) `TB_B`
        ON tiivtr.`PART_IDX` = `TB_B`.`PART_CODE`
        WHERE tiivtr.LOC_IDX = '2') `A`
        WHERE `PART_NO` LIKE @AA 
        AND `Location` LIKE @CC
        HAVING `Verification` LIKE @BB
        ORDER BY `Verification` ASC, `Difference_QTY` ASC, `PART_NO` ASC";

                    //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql,
                        new MySqlParameter("@AA", AA),
                        new MySqlParameter("@BB", BB),
                        new MySqlParameter("@CC", CC));

                    result.G01FinishedGood = new List<dynamic>();
                    if (ds.Tables.Count > 0)
                    {
                        result.G01FinishedGood.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
                        int badCount = 0;
                        int goodCount = 0;
                        foreach (var row in result.G01FinishedGood)
                        {
                            if (row.Verification == "Bad")
                            {
                                badCount++;
                            }
                            else
                            {
                                goodCount++;
                            }
                        }
                        result.G01FinishedGoodGood = goodCount;
                        result.G01FinishedGoodBad = badCount;

                        result.Data = new Dictionary<string, string>
                        {
                            { "FG_CM07", "AliceBlue" },
                            { "FG_CM08", "LightGray" },
                            { "FG_CM10", "MistyRose" }
                        };
                    }

                    //result.G01FinishedGood = result.G01FinishedGood.Take(GlobalHelper.ListCount.Value).ToList();
                }
                else if (BaseParameter.Action == 4)
                {
                    string AA = $"%{BaseParameter.ListSearchString[0] ?? ""}%";
                    string BB = BaseParameter.ListSearchString[1] == "ALL" ? "%%" : $"%{BaseParameter.ListSearchString[1] ?? ""}%";
                    string CC = $"%{BaseParameter.ListSearchString[2] ?? ""}%";

                    string sql = @"SELECT 
                        `DB_AA`.`LD_IDX` AS `PART_CODE`, 
                        `DB_AA`.`LEAD_NM` AS `PART_NO`, 
                        IFNULL(`STOCKDB`.`ADJ_QTY`, 0) AS `Change_QTY`,
                        `DB_AA`.`IN_QTY` AS `Incoming_QTY`, 
                        `DB_AA`.`OUT_QTY` AS `OUT_QTY`, 
                        ((`DB_AA`.`IN_QTY` - `DB_AA`.`OUT_QTY`) - (IFNULL(`STOCKDB`.`STOCK_QTY`, 0) - IFNULL(`STOCKDB`.`ADJ_QTY`, 0))) AS `MES_STOCK`,  
                        ((`DB_AA`.`IN_QTY` - `DB_AA`.`OUT_QTY`) - (IFNULL(`STOCKDB`.`STOCK_QTY`, 0) - IFNULL(`STOCKDB`.`ADJ_QTY`, 0))) AS `Difference_QTY`,
                        IFNULL(`STOCKDB`.`STOCK_QTY`, 0) AS `STOCK_QTY`,
                        IF(((`DB_AA`.`IN_QTY` - `DB_AA`.`OUT_QTY`) - (IFNULL(`STOCKDB`.`STOCK_QTY`, 0) - IFNULL(`STOCKDB`.`ADJ_QTY`, 0))) = 0, 'Good', 'Bad') AS `Verification`, 
                        `DB_AA`.`HOOK_RACK` AS `Location`
                        FROM (
                            SELECT 
                                (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `HOOK_RACK`, 
                                trackmtim.`LEAD_NM`, 
                                (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = trackmtim.`LEAD_NM`) AS `LD_IDX`,
                                SUM(IF(trackmtim.`RACKIN_YN` = 'Y', trackmtim.QTY, 0)) AS `IN_QTY`, 
                                SUM(IF(trackmtim.`RACKOUT_YN` = 'Y', trackmtim.QTY, 0)) AS `OUT_QTY`
                            FROM trackmtim 
                            GROUP BY trackmtim.`LEAD_NM`
                        ) `DB_AA` 
                        LEFT JOIN (
                            SELECT 
                                tiivtr_lead.`PART_IDX`, 
                                IFNULL(tiivtr_lead.`QTY`, 0) AS `STOCK_QTY`, 
                                IFNULL(tiivaj_LEAD.`ADJ_QTY`, 0) AS `ADJ_QTY`
                            FROM tiivtr_lead 
                            LEFT JOIN tiivaj_LEAD
                            ON tiivtr_lead.`PART_IDX` = tiivaj_LEAD.`PART_IDX` 
                            AND tiivtr_lead.`LOC_IDX` = tiivaj_LEAD.`ADJ_SCN`
                            WHERE tiivtr_lead.`LOC_IDX` = '3'
                        ) `STOCKDB`
                        ON `DB_AA`.`LD_IDX` = `STOCKDB`.`PART_IDX`
                        WHERE LENGTH(`DB_AA`.`LEAD_NM`) > 0 
                        HAVING `Verification` LIKE @BB 
                        AND `PART_NO` LIKE @AA 
                        AND `Location` LIKE @CC
                        ORDER BY `Verification` ASC, `Difference_QTY` ASC, `PART_NO` ASC";

                    //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql,
                        new MySqlParameter("@AA", AA),
                        new MySqlParameter("@BB", BB),
                        new MySqlParameter("@CC", CC));

                    result.G01HookRack = new List<dynamic>();
                    if (ds.Tables.Count > 0)
                    {
                        result.G01HookRack.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
                        int badCount = 0;
                        int goodCount = 0;
                        foreach (var row in result.G01HookRack)
                        {
                            if (row.Verification == "Bad")
                            {
                                badCount++;
                            }
                            else
                            {
                                goodCount++;
                            }
                        }
                        result.G01HookRackGood = goodCount;
                        result.G01HookRackBad = badCount;

                        result.Data = new Dictionary<string, string>
                        {
                            { "HR_CM07", "LightGray" },
                            { "HR_CM08", "AliceBlue" },
                            { "HR_CM09", "MistyRose" }
                        };
                    }

                    result.G01HookRack = result.G01HookRack.Take(GlobalHelper.ListCount.Value).ToList();
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = $"Error Code: 99990, Không thể kết nối MES: {ex.Message}";
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
                if (BaseParameter.Action == 1)
                {
                    string AA = $"%{BaseParameter.ListSearchString[0] ?? ""}%";
                    string BB = $"%{BaseParameter.ListSearchString[1] ?? ""}%";

                    string sql = @"SELECT * FROM (
                        SELECT 
                            `PART_IDX` AS `PART_CODE`, 
                            (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX`) AS `PART_NO`,
                            (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX`) AS `PART_NAME`,
                            IFNULL(SUM(case when LOC_IDX = 1 then `QTY` END), 0) AS `Raw_Material`,
                            IFNULL(SUM(case when LOC_IDX = 2 then `QTY` END), 0) AS `Finish_Goods`,
                            IFNULL(SUM(case when LOC_IDX = 7 then `QTY` END), 0) AS `WIP`,
                            IFNULL(SUM(case when LOC_IDX = 1 OR LOC_IDX = 2 OR LOC_IDX = 7 then `QTY` END), 0) AS `SUM`
                        FROM tiivtr 
                        GROUP BY `PART_IDX`
                    ) AS `DBA` 
                    WHERE `DBA`.`PART_NO` LIKE @AA 
                    AND `DBA`.`PART_NAME` LIKE @BB 
                    ORDER BY `DBA`.`PART_NO` ASC";

                    if (BaseParameter.CheckBox1 == true)
                    {
                        sql = sql + " LIMIT " + GlobalHelper.ListCount;
                    }

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql,
                    new MySqlParameter("@AA", AA),
                    new MySqlParameter("@BB", BB));

                    result.G01Overview = new List<dynamic>();
                    if (ds.Tables.Count > 0)
                    {
                        result.G01Overview.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
                    }
                }
                else if (BaseParameter.Action == 2)
                {
                    string AA = $"%{BaseParameter.ListSearchString[0] ?? ""}%";
                    string BB = BaseParameter.ListSearchString[1] == "ALL" ? "%%" : $"%{BaseParameter.ListSearchString[1] ?? ""}%";
                    string CC = $"%{BaseParameter.ListSearchString[2] ?? ""}%";

                    string sql = @"SELECT 
                        tiivtr.PART_IDX, 
                        (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) AS `PART_NO`, 
                        (SELECT tspart.PART_NM FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) AS `PART_NM`, 
                        IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '1' AND tiivaj.PART_IDX = tiivtr.PART_IDX), 0) AS `Change_QTY`, 
                        IFNULL(`TB_B`.`Incoming_QTY`, 0) AS `Incoming_QTY`, 
                        IFNULL(`TB_B`.`OUT_QTY`, 0) AS `OUT_QTY`,
                        IFNULL(`TB_B`.`Incoming_QTY`, 0) - IFNULL(`TB_B`.`OUT_QTY`, 0) + IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '1' AND tiivaj.PART_IDX = tiivtr.PART_IDX), 0) - IFNULL(tiivtr.QTY, 0) AS `Difference_QTY`,
                        IFNULL(tiivtr.QTY, 0) AS `STOCK_QTY`, 
                        IFNULL((SELECT tiivtr_EXCEL.QTY FROM tiivtr_EXCEL WHERE tiivtr_EXCEL.`PART_NO` = (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX)), 0) AS `MT_EXCEL`,
                        IF((IFNULL(`TB_B`.`Incoming_QTY`, 0) - IFNULL(`TB_B`.`OUT_QTY`, 0) + IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '1' AND tiivaj.PART_IDX = tiivtr.PART_IDX), 0) - IFNULL(tiivtr.QTY, 0)) = 0, 'Good', 'Bad') AS `Verification`,
                        (SELECT tspart.PART_LOC FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) AS `Location` 
                        FROM tiivtr LEFT JOIN 
                        (SELECT `A`.`PART_CODE`, SUM(`A`.`Incoming_QTY`) AS `Incoming_QTY`, 
                            SUM(`A`.`OUT_QTY`) AS `OUT_QTY` 
                        FROM (
                            SELECT 
                                (SELECT TMMTIN.PART_IDX FROM TMMTIN WHERE TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX) AS `PART_CODE`, 
                                IF(TMBRCD.BBCO = 'Y', TMBRCD.`PKG_QTY`, 0) AS `Incoming_QTY`, 
                                IFNULL(TMBRCD.`PKG_OUTQTY`, 0) AS `OUT_QTY`
                            FROM TMBRCD
                        ) AS `A`
                        GROUP BY `A`.`PART_CODE`) `TB_B`
                        ON tiivtr.PART_IDX = `TB_B`.`PART_CODE`
                        WHERE tiivtr.LOC_IDX = '1' 
                        AND (SELECT tspart.PART_NO FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) LIKE @AA 
                        AND (SELECT tspart.PART_LOC FROM tspart WHERE tspart.PART_IDX = tiivtr.PART_IDX) LIKE @CC 
                        HAVING `Verification` LIKE @BB 
                        ORDER BY `Verification` ASC, `Difference_QTY` ASC, `PART_NO` ASC";

                    //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql,
                        new MySqlParameter("@AA", AA),
                        new MySqlParameter("@BB", BB),
                        new MySqlParameter("@CC", CC));

                    result.G01RawMaterial = new List<dynamic>();
                    if (ds.Tables.Count > 0)
                    {
                        result.G01RawMaterial.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
                        int badCount = 0;
                        int goodCount = 0;
                        foreach (var row in result.G01RawMaterial)
                        {
                            if (row.Verification == "Bad")
                            {
                                badCount++;
                            }
                            else
                            {
                                goodCount++;
                            }
                        }
                        result.G01RawMaterialGood = goodCount;
                        result.G01RawMaterialBad = badCount;

                        result.Data = new Dictionary<string, string>
                        {
                            { "MT_CM07", "AliceBlue" },
                            { "MT_CM08", "LightGray" },
                            { "MT_CM10", "MistyRose" }
                        };
                    }

                    //result.G01RawMaterial = result.G01RawMaterial.Take(GlobalHelper.ListCount.Value).ToList();

                }
                else if (BaseParameter.Action == 3)
                {
                    string AA = $"%{BaseParameter.ListSearchString[0] ?? ""}%";
                    string BB = BaseParameter.ListSearchString[1] == "ALL" ? "%%" : $"%{BaseParameter.ListSearchString[1] ?? ""}%";
                    string CC = $"%{BaseParameter.ListSearchString[2] ?? ""}%";

                    string sql = @"SELECT `A`.`PART_IDX`, `A`.`PART_NO`, `A`.`PART_NM`, `A`.`Change_QTY`, `A`.`Incoming_QTY`, `A`.`OUT_QTY`,  
                        `A`.`STOCK_QTY` AS `MES_STOCK`,  
                        IFNULL((SELECT tiivtr_EXCEL.QTY FROM tiivtr_EXCEL WHERE tiivtr_EXCEL.`PART_NO` = `A`.`PART_NO`), 0) AS `FG_EXCEL`,
                        (`A`.`Incoming_QTY` - `A`.`OUT_QTY` + IFNULL(`A`.`Change_QTY`, 0) - `A`.`STOCK_QTY`) AS `Difference_QTY`,
                        IF((`A`.`Incoming_QTY` - `A`.`OUT_QTY` + IFNULL(`A`.`Change_QTY`, 0) - `A`.`STOCK_QTY`) = 0, 'Good', 'Bad') AS `Verification`, `A`.`Location`
                        FROM 
                        (SELECT tiivtr.`PART_IDX`,
                            (SELECT tspart.`PART_NO` FROM tspart WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX`) AS `PART_NO`,
                            (SELECT tspart.`PART_NM` FROM tspart WHERE tspart.`PART_IDX` = tiivtr.`PART_IDX`) AS `PART_NM`,
                            IFNULL(`TB_B`.`Incoming_QTY`, 0) AS `Incoming_QTY`, 
                            IFNULL(`TB_B`.`OUT_QTY`, 0) AS `OUT_QTY`,
                            IFNULL(tiivtr.`QTY`, 0) AS `MES_STOCK`,
                            IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '2' AND tiivaj.PART_IDX = tiivtr.`PART_IDX`), 0) AS `Change_QTY`,
                            IFNULL(tiivtr.QTY, 0) AS `STOCK_QTY`,
                            (SELECT tspart.PART_LOC FROM tspart WHERE tspart.PART_IDX = tiivtr.`PART_IDX`) AS `Location`
                        FROM tiivtr LEFT JOIN 
                            (SELECT tdpdmtim.VLID_PART_IDX AS `PART_CODE`,  
                                COUNT(tdpdmtim.VLID_PART_IDX) AS `Incoming_QTY`, 
                                SUM(IF(tdpdmtim.VLID_DSCN_YN = 'Y', 1, 0)) AS `OUT_QTY`
                            FROM tdpdmtim 
                            GROUP BY `PART_CODE`) `TB_B`
                        ON tiivtr.`PART_IDX` = `TB_B`.`PART_CODE`
                        WHERE tiivtr.LOC_IDX = '2') `A`
                        WHERE `PART_NO` LIKE @AA 
                        AND `Location` LIKE @CC
                        HAVING `Verification` LIKE @BB
                        ORDER BY `Verification` ASC, `Difference_QTY` ASC, `PART_NO` ASC";

                    //sql = sql + " LIMIT " + GlobalHelper.ListCount;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql,
                        new MySqlParameter("@AA", AA),
                        new MySqlParameter("@BB", BB),
                        new MySqlParameter("@CC", CC));

                    result.G01FinishedGood = new List<dynamic>();
                    if (ds.Tables.Count > 0)
                    {
                        result.G01FinishedGood.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));
                        int badCount = 0;
                        int goodCount = 0;
                        foreach (var row in result.G01FinishedGood)
                        {
                            if (row.Verification == "Bad")
                            {
                                badCount++;
                            }
                            else
                            {
                                goodCount++;
                            }
                        }
                        result.G01FinishedGoodGood = goodCount;
                        result.G01FinishedGoodBad = badCount;

                        result.Data = new Dictionary<string, string>
                        {
                            { "FG_CM07", "AliceBlue" },
                            { "FG_CM08", "LightGray" },
                            { "FG_CM10", "MistyRose" }
                        };
                    }

                    //result.G01FinishedGood = result.G01FinishedGood.Take(GlobalHelper.ListCount.Value).ToList();
                }
                else if (BaseParameter.Action == 4)
                {
                    string AA = $"%{BaseParameter.ListSearchString[0] ?? ""}%";
                    string BB = BaseParameter.ListSearchString[1] == "ALL" ? "%%" : $"%{BaseParameter.ListSearchString[1] ?? ""}%";
                    string CC = $"%{BaseParameter.ListSearchString[2] ?? ""}%";

                    string sql = @"SELECT 
                        `DB_AA`.`LD_IDX` AS `PART_CODE`, 
                        `DB_AA`.`LEAD_NM` AS `PART_NO`, 
                        IFNULL(`STOCKDB`.`ADJ_QTY`, 0) AS `Change_QTY`,
                        `DB_AA`.`IN_QTY` AS `Incoming_QTY`, 
                        `DB_AA`.`OUT_QTY` AS `OUT_QTY`, 
                        ((`DB_AA`.`IN_QTY` - `DB_AA`.`OUT_QTY`) - (IFNULL(`STOCKDB`.`STOCK_QTY`, 0) - IFNULL(`STOCKDB`.`ADJ_QTY`, 0))) AS `MES_STOCK`,  
                        ((`DB_AA`.`IN_QTY` - `DB_AA`.`OUT_QTY`) - (IFNULL(`STOCKDB`.`STOCK_QTY`, 0) - IFNULL(`STOCKDB`.`ADJ_QTY`, 0))) AS `Difference_QTY`,
                        IFNULL(`STOCKDB`.`STOCK_QTY`, 0) AS `STOCK_QTY`,
                        IF(((`DB_AA`.`IN_QTY` - `DB_AA`.`OUT_QTY`) - (IFNULL(`STOCKDB`.`STOCK_QTY`, 0) - IFNULL(`STOCKDB`.`ADJ_QTY`, 0))) = 0, 'Good', 'Bad') AS `Verification`, 
                        `DB_AA`.`HOOK_RACK` AS `Location`
                        FROM (
                            SELECT 
                                (SELECT trackmaster.`HOOK_RACK` FROM trackmaster WHERE trackmaster.`RACK_IDX` = trackmtim.`RACK_IDX`) AS `HOOK_RACK`, 
                                trackmtim.`LEAD_NM`, 
                                (SELECT torder_lead_bom.`LEAD_INDEX` FROM torder_lead_bom WHERE torder_lead_bom.`LEAD_PN` = trackmtim.`LEAD_NM`) AS `LD_IDX`,
                                SUM(IF(trackmtim.`RACKIN_YN` = 'Y', trackmtim.QTY, 0)) AS `IN_QTY`, 
                                SUM(IF(trackmtim.`RACKOUT_YN` = 'Y', trackmtim.QTY, 0)) AS `OUT_QTY`
                            FROM trackmtim 
                            GROUP BY trackmtim.`LEAD_NM`
                        ) `DB_AA` 
                        LEFT JOIN (
                            SELECT 
                                tiivtr_lead.`PART_IDX`, 
                                IFNULL(tiivtr_lead.`QTY`, 0) AS `STOCK_QTY`, 
                                IFNULL(tiivaj_LEAD.`ADJ_QTY`, 0) AS `ADJ_QTY`
                            FROM tiivtr_lead 
                            LEFT JOIN tiivaj_LEAD
                            ON tiivtr_lead.`PART_IDX` = tiivaj_LEAD.`PART_IDX` 
                            AND tiivtr_lead.`LOC_IDX` = tiivaj_LEAD.`ADJ_SCN`
                            WHERE tiivtr_lead.`LOC_IDX` = '3'
                        ) `STOCKDB`
                        ON `DB_AA`.`LD_IDX` = `STOCKDB`.`PART_IDX`
                        WHERE LENGTH(`DB_AA`.`LEAD_NM`) > 0 
                        HAVING `Verification` LIKE @BB 
                        AND `PART_NO` LIKE @AA 
                        AND `Location` LIKE @CC
                        ORDER BY `Verification` ASC, `Difference_QTY` ASC, `PART_NO` ASC";

                    //sql = sql + " LIMIT " + GlobalHelper.ListCount2000;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql,
                        new MySqlParameter("@AA", AA),
                        new MySqlParameter("@BB", BB),
                        new MySqlParameter("@CC", CC));

                    result.DataGridView = new List<SuperResultTranfer>();
                    if (ds.Tables.Count > 0)
                    {
                        result.DataGridView.AddRange(SQLHelper.ToList<SuperResultTranfer>(ds.Tables[0]));

                        string SheetName = this.GetType().Name;
                        string fileName = "G01_" + GlobalHelper.InitializationDateTimeCode0001 + ".xlsx";
                        using (var streamExport = new MemoryStream())
                        {
                            await InitializationToExcelAsync(result.DataGridView, streamExport, SheetName);
                            string physicalPath = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPath);
                            GlobalHelper.DeleteFilesByPath(physicalPath);

                            string filePath = Path.Combine(physicalPath, fileName);
                            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                            {
                                streamExport.Position = 0;
                                await streamExport.CopyToAsync(fileStream);
                            }
                            string downloadPath = Path.Combine(GlobalHelper.Download, SheetName, fileName).Replace("\\", "/");
                            result.Code = $"{GlobalHelper.URLSite}/{downloadPath}";
                        }
                    }
                    result.DataGridView = new List<SuperResultTranfer>();
                    //result.G01HookRack = result.G01HookRack.Take(GlobalHelper.ListCount.Value).ToList();
                }

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Error = $"Error Code: 99990, Không thể kết nối MES: {ex.Message}";
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
        public virtual async Task<BaseResult> Button1_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"INSERT INTO tiivaj (`PART_IDX`, `ADJ_SCN`, `ADJ_DTM`, `ADJ_QTY`, `ADJ_BF_QTY`, `ADJ_AF_QTY`, `ADJ_RSON`, `CREATE_DTM`, `CREATE_USER`)  
(SELECT 
tdpdmtim.VLID_PART_IDX, 2 AS `LOC`, NOW() AS `DATE`, 0, 0, 0, 'MES_AUTO', NOW(), 'MES_AUTO'
FROM tdpdmtim
WHERE tdpdmtim.VLID_DSCN_YN = 'N'  GROUP BY tdpdmtim.VLID_PART_IDX)
ON DUPLICATE KEY UPDATE `ADJ_QTY`= VALUES(`ADJ_QTY`), `UPDATE_DTM` = NOW(),  `UPDATE_USER` = 'MES_AUTO' ";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                sql = @" ALTER TABLE     `tiivaj`     AUTO_INCREMENT= 1  ";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                sql = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) 
                (SELECT   tspart.`PART_IDX`, 2 AS `LOC`, IFNULL(`FG_PT`.`QTY`, 0) AS `QTY`, NOW() AS `DATE`, 'MES_AUTO' AS `USER`
                FROM tspart LEFT JOIN 
                (SELECT  tdpdmtim.`VLID_PART_IDX`, 2 AS `LOC`, COUNT(tdpdmtim.`VLID_BARCODE`) AS `QTY`, NOW() AS `DATE`, 'MES_AUTO' AS `USER`
                FROM tdpdmtim  WHERE tdpdmtim.`VLID_DSCN_YN` = 'N'  GROUP BY tdpdmtim.`VLID_PART_IDX`) `FG_PT`
                ON tspart.PART_IDX = `FG_PT`.`VLID_PART_IDX`  WHERE tspart.PART_SCN = 6 )
                ON DUPLICATE KEY UPDATE `QTY`= VALUES(`QTY`), `UPDATE_DTM` = NOW(),  `UPDATE_USER` = 'MES_AUTO' ";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                sql = @"ALTER TABLE     `tiivtr`     AUTO_INCREMENT= 1 ";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                string sql = @"UPDATE tiivaj SET tiivaj.`ADJ_QTY` = 0, tiivaj.`ADJ_BF_QTY` = 0, tiivaj.`ADJ_AF_QTY` = 0, tiivaj.`ADJ_RSON` = 'MES',
                    tiivaj.`UPDATE_DTM` = NOW(), tiivaj.`UPDATE_USER` = 'MES_AUTO'   WHERE tiivaj.`ADJ_SCN` = 1    ";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                sql = @" ALTER TABLE     `tiivaj`     AUTO_INCREMENT= 1  ";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                sql = @"INSERT INTO `tiivtr` (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) 
                SELECT   tiivtr.PART_IDX, '1', IFNULL(`TB_B`.`Incoming_QTY`, 0) - IFNULL(`TB_B`.`OUT_QTY`, 0) +  IFNULL((SELECT tiivaj.ADJ_QTY FROM tiivaj WHERE tiivaj.ADJ_SCN = '1' AND tiivaj.PART_IDX = tiivtr.PART_IDX), 0)  AS `QTY`,
                NOW(), 'MES_AUTO'   FROM tiivtr LEFT JOIN 
                (SELECT `A`.`PART_CODE`, SUM(`A`.`Incoming_QTY`) AS `Incoming_QTY`, 
                SUM(`A`.`OUT_QTY`) AS `OUT_QTY` 
                FROM (SELECT  (SELECT TMMTIN.PART_IDX FROM TMMTIN WHERE TMMTIN.MTIN_IDX = TMBRCD.MTIN_IDX) AS `PART_CODE`, 
                IF(TMBRCD.BBCO = 'Y', TMBRCD.PKG_QTY, 0) AS `Incoming_QTY`,  IFNULL(TMBRCD.PKG_OUTQTY, 0) AS `OUT_QTY`
                FROM TMBRCD     ) AS `A` 
                GROUP BY  `A`.`PART_CODE` ) `TB_B`
                ON tiivtr.PART_IDX = `TB_B`.`PART_CODE`
                WHERE tiivtr.LOC_IDX = '1'

                ON DUPLICATE KEY UPDATE `QTY`= VALUES(`QTY`), `UPDATE_DTM` = NOW(),  `UPDATE_USER` = 'MES_AUTO' ";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                sql = @"ALTER TABLE     `tiivtr`     AUTO_INCREMENT= 1 ";
                await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private Task InitializationToExcelAsync(List<SuperResultTranfer> list, MemoryStream streamExport, string sheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(sheetName);

                string[] headers = { "PART_CODE", "PART_NO", "Change_QTY", "Incoming_QTY",
                             "OUT_QTY", "MES_STOCK", "Difference_QTY",
                             "STOCK_QTY", "Verification", "Location" };

                // Header
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = workSheet.Cells[1, i + 1];
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 14;
                    cell.Style.Font.Name = "Arial";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }

                // Data
                int row = 2;
                foreach (var item in list)
                {
                    workSheet.Cells[row, 1].Value = item.PART_CODE;
                    workSheet.Cells[row, 2].Value = item.PART_NO;
                    workSheet.Cells[row, 3].Value = Convert.ToInt32(item.Change_QTY ?? "0");
                    workSheet.Cells[row, 4].Value = Convert.ToInt32(item.Incoming_QTY ?? "0");
                    workSheet.Cells[row, 5].Value = Convert.ToInt32(item.OUT_QTY ?? "0"); ;
                    workSheet.Cells[row, 6].Value = Convert.ToInt32(item.MES_STOCK ?? "0");
                    workSheet.Cells[row, 7].Value = Convert.ToInt32(item.Difference_QTY ?? "0");
                    workSheet.Cells[row, 8].Value = Convert.ToInt32(item.STOCK_QTY ?? "0");
                    workSheet.Cells[row, 9].Value = item.Verification;
                    workSheet.Cells[row, 10].Value = item.Location;

                    for (int col = 1; col <= headers.Length; col++)
                    {
                        var cell = workSheet.Cells[row, col];
                        cell.Style.Font.Size = 11;
                        cell.Style.Font.Name = "Arial";
                        cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    }
                    row++;
                }

                // AutoFit toàn bộ vùng có data
                workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();

                package.Save();
            }
            streamExport.Position = 0;
            return Task.CompletedTask;
        }

        //private async Task InitializationToExcelAsync(List<SuperResultTranfer> list, MemoryStream streamExport, string sheetName)
        //{
        //    await Task.Run(() =>
        //    {
        //        using (var package = new ExcelPackage(streamExport))
        //        {
        //            var workSheet = package.Workbook.Worksheets.Add("G01_HOOKRACK");
        //            int row = 1;
        //            int column = 1;
        //            workSheet.Cells[row, column].Value = "PART_CODE";
        //            column++;
        //            workSheet.Cells[row, column].Value = "PART_NO";
        //            column++;
        //            workSheet.Cells[row, column].Value = "Change_QTY";
        //            column++;
        //            workSheet.Cells[row, column].Value = "Incoming_QTY";
        //            column++;
        //            workSheet.Cells[row, column].Value = "OUT_QTY";
        //            column++;
        //            workSheet.Cells[row, column].Value = "MES_STOCK";
        //            column++;
        //            workSheet.Cells[row, column].Value = "Difference_QTY";
        //            column++;
        //            workSheet.Cells[row, column].Value = "STOCK_QTY";
        //            column++;
        //            workSheet.Cells[row, column].Value = "Verification";
        //            column++;
        //            workSheet.Cells[row, column].Value = "Location";


        //            for (int i = 1; i <= column; i++)
        //            {
        //                workSheet.Cells[row, i].Style.Font.Bold = true;
        //                workSheet.Cells[row, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        //                workSheet.Cells[row, i].Style.Font.Name = "Arial";
        //                workSheet.Cells[row, i].Style.Font.Size = 14;
        //                workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //            }

        //            row++;
        //            foreach (var item in list)
        //            {
        //                column = 1;
        //                workSheet.Cells[row, column].Value = item.PART_CODE;
        //                column++;
        //                workSheet.Cells[row, column].Value = item.PART_NO;
        //                column++;
        //                workSheet.Cells[row, column].Value = item.Change_QTY;
        //                column++;
        //                workSheet.Cells[row, column].Value = item.Incoming_QTY;
        //                column++;
        //                workSheet.Cells[row, column].Value = item.OUT_QTY;
        //                column++;
        //                workSheet.Cells[row, column].Value = item.MES_STOCK;
        //                column++;
        //                workSheet.Cells[row, column].Value = item.Difference_QTY;
        //                column++;
        //                workSheet.Cells[row, column].Value = item.STOCK_QTY;
        //                column++;
        //                workSheet.Cells[row, column].Value = item.Verification;
        //                column++;
        //                workSheet.Cells[row, column].Value = item.Location;

        //                for (int i = 1; i <= column; i++)
        //                {
        //                    workSheet.Cells[row, i].Style.Font.Name = "Arial";
        //                    workSheet.Cells[row, i].Style.Font.Size = 11;
        //                    workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
        //                    workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
        //                    workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
        //                    workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        //                }
        //                row++;
        //            }
        //            for (int i = 1; i <= column; i++)
        //            {
        //                workSheet.Column(i).AutoFit();
        //            }

        //            package.Save();
        //        }
        //        streamExport.Position = 0;
        //    });
        //}

    }
}