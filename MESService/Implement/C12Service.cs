

namespace MESService.Implement
{
    public class C12Service : BaseService<torderlist, ItorderlistRepository>, IC12Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;

        public C12Service(ItorderlistRepository torderlistRepository, IWebHostEnvironment webHostEnvironment)
            : base(torderlistRepository)
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

        public virtual async Task<BaseResult> Buttonfind_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        // Extract parameters
                        var TabSelection = BaseParameter.ListSearchString[0]; // "TabPage1" or "TabPage2"
                        var MIN_DATE = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd");
                        var MAX_DATE = DateTime.Parse(BaseParameter.ListSearchString[2]).AddDays(1).ToString("yyyy-MM-dd");

                        if (TabSelection == "TabPage1")
                        {
                            // TabPage1 Logic - Detail records + Summary
                            await HandleTabPage1Logic(result, MIN_DATE, MAX_DATE);
                        }
                        else if (TabSelection == "TabPage2")
                        {
                            // TabPage2 Logic - Grouped by MC_NO
                            await HandleTabPage2Logic(result, MIN_DATE, MAX_DATE);
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

        private async Task HandleTabPage1Logic(BaseResult result, string MIN_DATE, string MAX_DATE)
        {
            // Build common FROM clause used in both queries
            string DGV_FROM = BuildCommonFromClause(MIN_DATE, MAX_DATE);

            // Detail query for DataGridView1
            string DGV_DATE = @"SELECT 
`TORDER_IDX`, `LEAD_NO`, `MC_NO`, 
`DATE`, `NON_WORK_TIME`, `WORKG_TIME`,
((`SUM_QTY` / `WORKG_TIME`) / (`RUS` / 60)) * 100 AS `WG_RUS`,
 `WORK_TIME`, 
((`SUM_QTY` / `WORK_TIME`) / (`RUS` / 60)) * 100 AS `WG_RUS2`,
 `SUM_QTY`, `TOT_QTY`, `WIRE`, `WIRE_L`, `TERM1`, `TERM2`, `SEAL1`, `SEAL2`, `RUS`, `CREATE_USER` ";

            string sql1 = DGV_DATE + DGV_FROM;

            DataSet ds1 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql1);
            result.DataGridView1 = new List<SuperResultTranfer>();
            for (int i = 0; i < ds1.Tables.Count; i++)
            {
                DataTable dt = ds1.Tables[i];
                result.DataGridView1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }

            // Summary query for DataGridView3
            string DGV_SUM = @"SELECT SUM(`NON_WORK_TIME`) AS `NON_WORK_TIME`, SUM(`WORKG_TIME`) AS `WORKG_TIME`,  
AVG(((`SUM_QTY` / `WORKG_TIME`) / (`RUS` / 60)) * 100) AS `WG_RUS`,  
`WORK_TIME`, AVG(((`SUM_QTY` / `WORK_TIME`) / (`RUS` / 60)) * 100) AS `WG_RUS2`, SUM(`SUM_QTY`) AS `PO_QTY`, SUM(`TOT_QTY`) AS `ACT_QTY` ";

            string sql2 = DGV_SUM + DGV_FROM;

            DataSet ds2 = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql2);
            result.DataGridView3 = new List<SuperResultTranfer>();
            for (int i = 0; i < ds2.Tables.Count; i++)
            {
                DataTable dt = ds2.Tables[i];
                result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }
        }

        private async Task HandleTabPage2Logic(BaseResult result, string MIN_DATE, string MAX_DATE)
        {
            string sql = @"SELECT 
`DATE`, `MC_NO`, SUM(`TOT_QTY`) AS `PO_QTY`, SUM(`SUM_QTY`) AS `WK_QTY`,
SUM(`NON_WORK_TIME`) AS `NON_WORK_TIME`,
SUM(`WORKG_TIME`) AS `WORKG_TIME`,
AVG(`WG_RUS`) AS `WG_RUS`,
SUM(`WORK_TIME`) AS `WORK_TIME`,
AVG(`WG_RUS2`) AS `WG_RUS2`
 
FROM (

SELECT 
`TORDER_IDX`, `LEAD_NO`, `MC_NO`, 
`DATE`, `NON_WORK_TIME`, `WORKG_TIME`,
((`SUM_QTY` / `WORKG_TIME`) / (`RUS` / 60)) * 100 AS `WG_RUS`,
 `WORK_TIME`, 
((`SUM_QTY` / `WORK_TIME`) / (`RUS` / 60)) * 100 AS `WG_RUS2`,
 `SUM_QTY`, `TOT_QTY`, `WIRE`, `WIRE_L`, `TERM1`, `TERM2`, `SEAL1`, `SEAL2`, `RUS`, `CREATE_USER`
" + BuildCommonFromClause(MIN_DATE, MAX_DATE) + @" ) `ZZ`

GROUP BY `MC_NO` 
ORDER BY `MC_NO`";

            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.DataGridView2 = new List<SuperResultTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.DataGridView2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }
        }

        private string BuildCommonFromClause(string MIN_DATE, string MAX_DATE)
        {
            return @"
FROM
(SELECT `A`.`TORDER_IDX`, `B`.`LEAD_NO`, `A`.`MC_NO`, `A`.`CREATE_USER`, SUM(`A`.`WK_QTY`) AS `SUM_QTY`,
`B`.`TOT_QTY`, `B`.`WIRE`,
CAST(REPLACE(MID(`B`.`WIRE`, CHAR_LENGTH(SUBSTRING_INDEX(`B`.`WIRE`, '  ', 2))+2), ',' , '') AS UNSIGNED)  AS  `WIRE_L`, 
`B`.`TERM1`, `B`.`TERM2`,
(IF(SUBSTRING_INDEX(`B`.`TERM1`, '(' ,1) = '', 0 ,1) + IF(SUBSTRING_INDEX(`B`.`TERM2`, '(' ,1) = '', 0 ,1)) AS `TC_ST`,
`B`.`SEAL1`, `B`.`SEAL2`,
(IF(SUBSTRING_INDEX(`B`.`SEAL1`, '(' ,1) = '', 0 ,1) + IF(SUBSTRING_INDEX(`B`.`SEAL2`, '(' ,1) = '', 0 ,1)) AS `SC_ST`,

(SELECT `TSCUT_ST_UPH_RUS` FROM TSCUT_ST_UPH  
WHERE `TSCUT_ST_UPH_MAX` >= CAST(REPLACE(MID(`B`.`WIRE`, CHAR_LENGTH(SUBSTRING_INDEX(`B`.`WIRE`, '  ', 2))+2), ',' , '') AS UNSIGNED)
AND `TSCUT_ST_UPH_MIN` <= CAST(REPLACE(MID(`B`.`WIRE`, CHAR_LENGTH(SUBSTRING_INDEX(`B`.`WIRE`, '  ', 2))+2), ',' , '') AS UNSIGNED)
AND `TSCUT_ST_UPH_TCNT` = (IF(SUBSTRING_INDEX(`B`.`TERM1`, '(' ,1) = '', 0 ,1) + IF(SUBSTRING_INDEX(`B`.`TERM2`, '(' ,1) = '', 0 ,1))
AND `TSCUT_ST_UPH_SCNT` = (IF(SUBSTRING_INDEX(`B`.`SEAL1`, '(' ,1) = '', 0 ,1) + IF(SUBSTRING_INDEX(`B`.`SEAL2`, '(' ,1) = '', 0 ,1)) 
AND `TSCUT_ST_UPH_RUT` = 'KOMAX' LIMIT 1) AS `RUS`,
`C`.`DATE`, 
`C`.`USER_ID`, 
(IFNULL(`C`.`WORK_TIME`, 0) - IFNULL(`C`.`NON_WORK_TIME`, 0)) AS `WORKG_TIME`,
IFNULL(`C`.`WORK_TIME`, 0) AS `WORK_TIME`, 
IFNULL(`C`.`NON_WORK_TIME`, 0) AS `NON_WORK_TIME`

FROM (TWWKAR `A` LEFT JOIN TORDERLIST `B` ON `A`.`TORDER_IDX` = `B`.`ORDER_IDX`) 

LEFT JOIN (SELECT `WT`.`ORDER_IDX`, 
IF(CAST(DATE_FORMAT(`CREATE_DTM`,'%H') AS UNSIGNED) < 6, DATE_FORMAT(DATE_ADD(`CREATE_DTM`, INTERVAL -1 DAY),'%Y-%m-%d'), DATE_FORMAT(`CREATE_DTM`,'%Y-%m-%d')) AS `DATE`, 
IF(`MENU_TEXT` = 'KOMAX_WORK', 'WORK', 'NON') AS `MENU`,
`USER_ID`, `USER_MC`, `S_TIME`, `E_TIME`,
SUM(IF(`WT`.`MENU_TEXT` =  'KOMAX_WORK', TIMESTAMPDIFF(second, S_TIME, E_TIME) /60, 0)) AS `WORK_TIME`,
SUM(IF(`WT`.`MENU_TEXT` =  'KOMAX_WORK', 0, TIMESTAMPDIFF(second, S_TIME, E_TIME) /60 * -1)) AS `NON_WORK_TIME`

FROM TORDERLIST_WTIME `WT`
WHERE `WT`.`CREATE_DTM` >= '" + MIN_DATE + @" 06:00:00' AND `WT`.`CREATE_DTM` < '" + MAX_DATE + @" 06:00:00' 
GROUP BY `WT`.`USER_ID`, `WT`.`ORDER_IDX`, DATE_FORMAT(`CREATE_DTM`,'%Y-%m-%d')) `C` 
ON `A`.`TORDER_IDX` =`C`.`ORDER_IDX` AND `C`.`USER_ID` = `A`.`CREATE_USER`

WHERE `A`.`CREATE_DTM` >= '" + MIN_DATE + @" 06:00:00' AND `A`.`CREATE_DTM` < '" + MAX_DATE + @" 06:00:00' 
GROUP BY `A`.`TORDER_IDX`, `A`.`CREATE_USER`) `BB`";
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
                if (BaseParameter != null)
                {
                    if (BaseParameter.ListSearchString != null)
                    {
                        // Extract parameters for export
                        var TabSelection = BaseParameter.ListSearchString[0];
                        var MIN_DATE = DateTime.Parse(BaseParameter.ListSearchString[1]).ToString("yyyy-MM-dd");
                        var MAX_DATE = DateTime.Parse(BaseParameter.ListSearchString[2]).AddDays(1).ToString("yyyy-MM-dd");

                        if (TabSelection == "TabPage1")
                        {
                            await HandleTabPage1Logic(result, MIN_DATE, MAX_DATE);
                            string SheetName = "C12_Detail";
                            string fileName = SheetName + @"_" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                            var streamExport = new MemoryStream();
                            InitializationToExcelTabPage1(result.DataGridView1, streamExport, SheetName);

                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                streamExport.CopyTo(stream);
                            }
                            result.DataGridView1 = new List<SuperResultTranfer>();
                            result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
                        }
                        else if (TabSelection == "TabPage2")
                        {
                            await HandleTabPage2Logic(result, MIN_DATE, MAX_DATE);
                            string SheetName = "C12_Summary";
                            string fileName = SheetName + @"_" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                            var streamExport = new MemoryStream();
                            InitializationToExcelTabPage2(result.DataGridView2, streamExport, SheetName);

                            string physicalPathCreate = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.Download, SheetName);
                            Directory.CreateDirectory(physicalPathCreate);
                            GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                            string filePath = Path.Combine(physicalPathCreate, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                streamExport.CopyTo(stream);
                            }
                            result.DataGridView2 = new List<SuperResultTranfer>();
                            result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
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

        private void InitializationToExcelTabPage1(List<SuperResultTranfer> list, MemoryStream streamExport, string SheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(SheetName);
                int row = 1;
                int column = 1;

                // Headers for TabPage1 Detail
                workSheet.Cells[row, column++].Value = "No";
                workSheet.Cells[row, column++].Value = "TORDER_IDX";
                workSheet.Cells[row, column++].Value = "LEAD_NO";
                workSheet.Cells[row, column++].Value = "MC_NO";
                workSheet.Cells[row, column++].Value = "DATE";
                workSheet.Cells[row, column++].Value = "NON_WORK_TIME";
                workSheet.Cells[row, column++].Value = "WORKG_TIME";
                workSheet.Cells[row, column++].Value = "WG_RUS";
                workSheet.Cells[row, column++].Value = "WORK_TIME";
                workSheet.Cells[row, column++].Value = "WG_RUS2";
                workSheet.Cells[row, column++].Value = "SUM_QTY";
                workSheet.Cells[row, column++].Value = "TOT_QTY";
                workSheet.Cells[row, column++].Value = "WIRE";
                workSheet.Cells[row, column++].Value = "WIRE_L";
                workSheet.Cells[row, column++].Value = "TERM1";
                workSheet.Cells[row, column++].Value = "TERM2";
                workSheet.Cells[row, column++].Value = "SEAL1";
                workSheet.Cells[row, column++].Value = "SEAL2";
                workSheet.Cells[row, column++].Value = "RUS";
                workSheet.Cells[row, column++].Value = "CREATE_USER";

                // Style headers
                for (int i = 1; i < column; i++)
                {
                    workSheet.Cells[row, i].Style.Font.Bold = true;
                    workSheet.Cells[row, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[row, i].Style.Font.Size = 14;
                    workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                // Data rows
                int stt = 0;
                foreach (var item in list)
                {
                    row++;
                    stt++;
                    workSheet.Cells[row, 1].Value = stt;
                    try
                    {
                        workSheet.Cells[row, 2].Value = item.TORDER_IDX;
                        workSheet.Cells[row, 3].Value = item.LEAD_NO;
                        workSheet.Cells[row, 4].Value = item.MC_NO;
                        workSheet.Cells[row, 5].Value = item.DATE;
                        workSheet.Cells[row, 6].Value = item.NON_WORK_TIME;
                        workSheet.Cells[row, 7].Value = item.WORKG_TIME;
                        workSheet.Cells[row, 8].Value = item.WG_RUS;
                        workSheet.Cells[row, 9].Value = item.WORK_TIME;
                        workSheet.Cells[row, 10].Value = item.WG_RUS2;
                        workSheet.Cells[row, 11].Value = item.SUM_QTY;
                        workSheet.Cells[row, 12].Value = item.TOT_QTY;
                        workSheet.Cells[row, 13].Value = item.WIRE;
                        workSheet.Cells[row, 14].Value = item.WIRE_L;
                        workSheet.Cells[row, 15].Value = item.TERM1;
                        workSheet.Cells[row, 16].Value = item.TERM2;
                        workSheet.Cells[row, 17].Value = item.SEAL1;
                        workSheet.Cells[row, 18].Value = item.SEAL2;
                        workSheet.Cells[row, 19].Value = item.RUS;
                        workSheet.Cells[row, 20].Value = item.CREATE_USER;
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

        private void InitializationToExcelTabPage2(List<SuperResultTranfer> list, MemoryStream streamExport, string SheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(SheetName);
                int row = 1;
                int column = 1;

                // Headers for TabPage2 Summary
                workSheet.Cells[row, column++].Value = "No";
                workSheet.Cells[row, column++].Value = "DATE";
                workSheet.Cells[row, column++].Value = "MC_NO";
                workSheet.Cells[row, column++].Value = "PO_QTY";
                workSheet.Cells[row, column++].Value = "WK_QTY";
                workSheet.Cells[row, column++].Value = "NON_WORK_TIME";
                workSheet.Cells[row, column++].Value = "WORKG_TIME";
                workSheet.Cells[row, column++].Value = "WG_RUS";
                workSheet.Cells[row, column++].Value = "WORK_TIME";
                workSheet.Cells[row, column++].Value = "WG_RUS2";

                // Style headers
                for (int i = 1; i < column; i++)
                {
                    workSheet.Cells[row, i].Style.Font.Bold = true;
                    workSheet.Cells[row, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                    workSheet.Cells[row, i].Style.Font.Size = 14;
                    workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                }

                // Data rows
                int stt = 0;
                foreach (var item in list)
                {
                    row++;
                    stt++;
                    workSheet.Cells[row, 1].Value = stt;
                    try
                    {
                        workSheet.Cells[row, 2].Value = item.DATE;
                        workSheet.Cells[row, 3].Value = item.MC_NO;
                        workSheet.Cells[row, 4].Value = item.PO_QTY;
                        workSheet.Cells[row, 5].Value = item.WK_QTY;
                        workSheet.Cells[row, 6].Value = item.NON_WORK_TIME;
                        workSheet.Cells[row, 7].Value = item.WORKG_TIME;
                        workSheet.Cells[row, 8].Value = item.WG_RUS;
                        workSheet.Cells[row, 9].Value = item.WORK_TIME;
                        workSheet.Cells[row, 10].Value = item.WG_RUS2;
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