
namespace MESService.Implement
{
    public class C09_LISTService : BaseService<torderlist, ItorderlistRepository>
    , IC09_LISTService
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public C09_LISTService(ItorderlistRepository torderlistRepository

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
        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    string USER_IDX = BaseParameter.USER_IDX;
                    var AA_1 = BaseParameter.SearchString;
                    string sql = @"SELECT IF(LEFT(`OR_NO`, 1)='N','','E') AS `OR_NO`, `WORK_WEEK`, `ORDER_IDX`, `CONDITION`, IFNULL(TORDERLIST_SPST.TORDER_FG, '') AS `TORDER_FG`,  `LEAD_NO`, `PO_QTY`, `PERFORMN`, 
                    `MC`, `SAFTY_QTY`, `PO_DT`, derivedtbl_1.`LS_DATE`, `BUNDLE_SIZE`, 
                    (SELECT `HOOK_RACK` FROM  trackmaster  WHERE trackmaster.`LEAD_NO` = TORDERLIST_SPST.`LEAD_NO`) AS `HOOK_RACK`, 
                    `DSCN_YN`, `ORDER_IDX`, IFNULL((SELECT `QTY` FROM  tiivtr_lead  WHERE `PART_IDX` = (SELECT `LEAD_INDEX` FROM torder_lead_bom WHERE `LEAD_PN` = TORDERLIST_SPST.`LEAD_NO`)), 0) AS `QTY_STOCK`, `LEAD_COUNT`

                    FROM  TORDERLIST_SPST LEFT OUTER JOIN
                    (SELECT  `TORDER_IDX`, MAX(`CREATE_DTM`) AS `LS_DATE`  FROM   TWWKAR   GROUP BY `TORDER_IDX`) derivedtbl_1 ON TORDERLIST_SPST.`ORDER_IDX` = derivedtbl_1.`TORDER_IDX`

                    WHERE `DSCN_YN` = 'Y' AND   TORDERLIST_SPST.`LEAD_NO` = '" + AA_1 + "' AND  NOT(`CONDITION` = 'Close' OR  `CONDITION` = 'Complete')   ORDER BY `CONDITION` DESC, `PO_DT`, `LEAD_NO`";

                    sql = sql + " LIMIT " + GlobalHelper.ListCount;

                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.DataGridView1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.DataGridView1 != null)
                    {
                        if (BaseParameter.DataGridView1.Count > 0)
                        {
                            foreach (var item in BaseParameter.DataGridView1)
                            {
                                if (item.CHK == true)
                                {
                                    var VAL = "";
                                    var AA = item.ORDER_IDX;                                 
                                    var BB = item.MC;
                                    VAL = @"UPDATE   TORDERLIST_SPST    SET `MC`='" + BB + "' WHERE   `ORDER_IDX` = " + AA;
                                    string sql = VAL;
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                if (BaseParameter != null)
                {
                    string USER_ID = BaseParameter.USER_ID;
                    string USER_IDX = BaseParameter.USER_IDX;
                    if (BaseParameter.DataGridView1 != null)
                    {
                        if (BaseParameter.DataGridView1.Count > 0)
                        {
                            var CONT = 0;
                            List<int> CHK = new List<int>();

                            foreach (var item in BaseParameter.DataGridView1)
                            {
                                if (item.CHK == true)
                                {
                                    var ORDER_IDX = item.ORDER_IDX.Value;
                                    CHK.Add(ORDER_IDX);
                                    CONT = CONT + 1;
                                }
                            }
                            if (CONT > 0)
                            {
                                for (int GG = 0; GG < CONT; GG++)
                                {
                                    string sql = @"UPDATE TORDERLIST_SPST SET `CONDITION`='Close' WHERE  `ORDER_IDX`= " + CHK[GG];
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.DataGridView1 != null)
                    {
                        if (BaseParameter.DataGridView1.Count > 0)
                        {
                            foreach (var item in BaseParameter.DataGridView1)
                            {
                                if (item.CHK == true)
                                {
                                    var OO = item.ORDER_IDX;
                                    string sql = @"UPDATE TORDERLIST SET `PERFORMN` = (SELECT ((SELECT (COUNT(`DSCN_YN`)) FROM TORDER_BARCODE WHERE `ORDER_IDX` = '" + OO + "' AND `DSCN_YN`='Y') * TORDERLIST.`BUNDLE_SIZE`) FROM TORDERLIST WHERE((SELECT(COUNT(TORDER_BARCODE.`DSCN_YN`)) FROM TORDER_BARCODE WHERE `ORDER_IDX` = TORDERLIST.`ORDER_IDX` AND `DSCN_YN`= 'Y') * TORDERLIST.`BUNDLE_SIZE`) <> TORDERLIST.`PERFORMN`), `CONDITION`= 'Close' WHERE `ORDER_IDX` = '" + OO + "'";
                                    await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
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
        public virtual async Task<BaseResult> C09_LIST_Load(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var MCbox = BaseParameter.SearchString;
                    string sql = @"INSERT INTO tsnon_oper_mitor (`tsnon_oper_mitor_MCNM`, `tsnon_oper_mitor_NOIC`, `tsnon_oper_mitor_RUNYN`,`StopCode`) VALUES ('" + MCbox + "', '-----', 'N','-')  ON DUPLICATE KEY UPDATE `tsnon_oper_mitor_NOIC` = '-----', `tsnon_oper_mitor_RUNYN` = 'N',`StopCode`='-'";
                    string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                    sql = @"ALTER TABLE     `tsnon_oper_mitor`     AUTO_INCREMENT= 1";
                    sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                }

            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> MC_LIST(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var FCT = BaseParameter.SearchString;
                    string sql = @"SELECT DISTINCT `MC` FROM TORDERLIST_SPST WHERE  `DSCN_YN` = 'Y' AND (NOT `CONDITION` = 'Close')  AND  `PO_DT` > DATE_SUB(NOW(),INTERVAL 12 DAY)  AND TORDERLIST_SPST.`FCTRY_NM` = '" + FCT + "'  GROUP BY `MC`";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.ComboBox1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> DB_LISECHK(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string sql = @"UPDATE   TORDERLIST_SPST    SET `CONDITION` = 'Close', `UPDATE_DTM` = NOW(),  `UPDATE_USER` = 'MES'   WHERE `PO_DT` < DATE_ADD(NOW(), INTERVAL -11 DAY) AND NOT (`CONDITION` = 'Complete') AND NOT (`CONDITION` = 'Close') ";
                string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                sql = @"UPDATE   TORDERLIST_SPST   SET `CONDITION` = 'Close', `DSCN_YN`='N'  WHERE    `PO_QTY`  <= 0  ";
                sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> TS_USER(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    string USER_IDX = BaseParameter.USER_IDX;
                    var MCbox = BaseParameter.SearchString;
                    string sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_TIME_ST` > DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d') AND  `TS_USER` = '" + USER_IDX + "'";
                    DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    var USER_COUNT = result.Search.Count;
                    if (USER_COUNT <= 0)
                    {
                        sql = @"INSERT INTO `TUSER_LOG` (`TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER`) VALUES ('" + MCbox + "', DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), NOW(), CONCAT(DATE_ADD(DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d'), INTERVAL +1 DAY), ' 05:49:59'), '" + USER_IDX + "')";
                        string sqlResult = await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                        sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, `TS_TIME_ST`, `TS_TIME_END`, `TS_USER` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_TIME_ST` > DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d') AND  `TS_USER` = '" + USER_IDX + "'";
                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.Search = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.Search.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }
                    }
                    sql = @"SELECT `TUSER_IDX`, `TS_MC_NM`, `TS_DATE`, MIN(`TS_TIME_ST`) AS `MIN`, MAX(`TS_TIME_END`) AS `MAX` FROM TUSER_LOG WHERE `TS_MC_NM` = '" + MCbox + "' AND `TS_DATE` = DATE_FORMAT(IF(DATE_FORMAT(NOW(), '%H%i') > 0550, DATE_FORMAT(DATE_ADD(NOW(), INTERVAL 0 DAY), '%Y-%m-%d'), DATE_FORMAT(DATE_ADD(NOW(), INTERVAL -1 DAY), '%Y-%m-%d')), '%Y-%m-%d')";
                    ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                    result.Search1 = new List<SuperResultTranfer>();
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        DataTable dt = ds.Tables[i];
                        result.Search1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                    }
                    if (result.Search1.Count > 0)
                    {
                        var S_DATE = result.Search1[0].TS_DATE;
                        result.Search1[0].Name = S_DATE.Value.ToString("HH:mm:ss");
                        result.Search1[0].Description = S_DATE.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    }
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> DataGridView1_SelectionChanged(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        var SELECT_LN = BaseParameter.ListSearchString[0];
                        string sql = @"SELECT  (SELECT `LEAD_PN` FROM torder_lead_bom WHERE `LEAD_INDEX` = `S_PART_IDX`) AS `LEAD_NO1`,  `S_LR` FROM torder_lead_bom_spst   WHERE 
                            (SELECT `LEAD_INDEX` FROM torder_lead_bom WHERE `LEAD_PN` = '" + SELECT_LN + "') = torder_lead_bom_spst.`M_PART_IDX`    ORDER BY `S_LR`";
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
        public virtual async Task<BaseResult> GenerateBarcode_MT(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    var BARCODE_MT = BaseParameter.SearchString;
                    result.Code = Helper.QRCodeHelper.CreateQRCodeViaString(BARCODE_MT);

                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        public virtual async Task<BaseResult> Button1_Click_1(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {

                string sql = @"SELECT `LEAD_NO`, SUM(`PO_QTY`) AS `TOTAL_QTY`  FROM   TORDERLIST_SPST   WHERE `CONDITION` = 'STAY' AND (`MC` LIKE 'Z1%' OR `MC` LIKE 'PLAN')";
                DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                var DGV_C09_99 = new List<SuperResultTranfer>();
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    DataTable dt = ds.Tables[i];
                    DGV_C09_99.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                }
                if (DGV_C09_99.Count > 0)
                {
                    string SheetName = this.GetType().Name;
                    string D1 = GlobalHelper.InitializationDateTime.ToString("yyyyMMdd");
                    string fileName = @"ORDER_LIST(SPTP)" + D1 + "_LIST.xlsx";
                    var streamExport = new MemoryStream();
                    InitializationToExcelAsync(DGV_C09_99, streamExport, SheetName);
                    string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                    Directory.CreateDirectory(physicalPathCreate);
                    GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                    string filePath = Path.Combine(physicalPathCreate, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        streamExport.CopyTo(stream);
                    }
                    result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                }
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }
        private void InitializationToExcelAsync(List<SuperResultTranfer> list, MemoryStream streamExport, string SheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(SheetName);
                int row = 1;
                int column = 1;
                workSheet.Cells[1, column].Value = "LEAD_NO";
                column = column + 1;
                workSheet.Cells[1, column].Value = "TOTAL_QTY";
                column = column + 1;

                foreach (var item in list)
                {
                    try
                    {
                        workSheet.Cells[row, 1].Value = item.LEAD_NO;
                        workSheet.Cells[row, 2].Value = item.TOTAL_QTY;
                        for (int i = 1; i < column; i++)
                        {
                            workSheet.Cells[row, i].Style.Font.Name = "Arial";
                            workSheet.Cells[row, i].Style.Font.Size = 14;
                            workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                        row = row + 1;
                    }
                    catch (Exception ex)
                    {
                        string mes = ex.Message;
                    }
                }
                for (int i = 1; i <= column; i++)
                {
                    workSheet.Column(i).AutoFit();
                }
                package.Save();
            }
            streamExport.Position = 0;
        }
    }
}

