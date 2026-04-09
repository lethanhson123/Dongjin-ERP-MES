using MESData.Model;

namespace MESService.Implement
{
    public class A01Service : BaseService<torderlist, ItorderlistRepository>
    , IA01Service
    {
        private readonly ItorderlistRepository _torderlistRepository;
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public A01Service(ItorderlistRepository torderlistRepository

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

            

            string sql = @"SELECT  `CD_IDX`, `CD_NM_HAN`, `CD_NM_EN`, `CDGR_IDX`, `CD_SYS_NOTE` FROM   TSCODE  WHERE  (`CDGR_IDX` = 4)";
            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.DGV_A01_01 = new List<SuperResultTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.DGV_A01_01.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }

            sql = @"SELECT  0 AS `CD_IDX`, 'SELECT' AS `CD_NM_HAN`, 'SELECT' AS `CD_NM_EN`, 0 AS `CDGR_IDX`, 'SELECT' AS `CD_SYS_NOTE` UNION SELECT  `CD_IDX`, `CD_NM_HAN`, `CD_NM_EN`, `CDGR_IDX`, `CD_SYS_NOTE` FROM   TSCODE  WHERE  (`CDGR_IDX` = 4)";
            ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            result.DGV_A01_01_1 = new List<SuperResultTranfer>();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.DGV_A01_01_1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
            }
            result.DGV_A01_CB1 = await BOM_CB_ADD();

            return result;
        }

        private async Task<List<SuperResultTranfer>> BOM_CB_ADD()
        {
            List<SuperResultTranfer> result = new List<SuperResultTranfer>();
            string sql = @"SELECT tspart.`BOM_GRP` FROM tspart     GROUP BY tspart.`BOM_GRP`";
            DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataTable dt = ds.Tables[i];
                result.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
                    string C_USER = BaseParameter.USER_ID;
                    if (BaseParameter.Action == 1)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string AA = BaseParameter.ListSearchString[0];
                            string BB = BaseParameter.ListSearchString[1];
                            string CC = BaseParameter.ListSearchString[2];
                            string DD = BaseParameter.ListSearchString[3];
                            string FF = BaseParameter.ListSearchString[4];
                            string GG = BaseParameter.ListSearchString[5];

                            AA = "%" + AA + "%";
                            BB = "%" + BB + "%";
                            FF = "%" + FF + "%";
                            GG = "%" + GG + "%";
                            if (DD == "ALL")
                            {
                                DD = "%%";
                            }
                            else
                            {
                                DD = "%" + DD + "%";
                            }
                            if (CC == "ALL")
                            {
                                CC = "";
                            }
                            else
                            {
                                if (BaseParameter.Language == GlobalHelper.LanguageCodeKR)
                                {
                                    CC = " AND `PART_SCN` = (SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_HAN` = '" + CC + "') ";
                                }
                                else
                                {
                                    CC = " AND `PART_SCN` = (SELECT `CD_IDX` FROM TSCODE WHERE `CD_NM_EN` = '" + CC + "') ";
                                }
                            }

                            string sql = @"SELECT  FALSE AS `CHK`, 
                        tspart.`PART_IDX` AS `CODE`, tspart.`PART_NO` AS `PART_NO`, IFNULL(tspart.`PART_SUPL`, '') AS `PART_SUPL`, 
                        tspart.`PART_NM` AS `PART_NAME`, tspart.`BOM_GRP` AS `BOM_GROUP`, 
                        tspart.`PART_CAR` AS `MODEL`, tspart.`PART_FML` AS `PART_FamilyPC`, tspart.`PART_SNP` AS `Packing_Unit`, 
                        (SELECT `CD_NM_HAN` FROM TSCODE WHERE TSCODE.`CD_IDX` = tspart.`PART_SCN`) AS `Item_TypeK`, 
                        (SELECT `CD_NM_EN` FROM TSCODE WHERE TSCODE.`CD_IDX` = tspart.`PART_SCN`) AS `Item_TypeE`, 
                        tspart.`PART_LOC` AS `Location`, tspart.`PART_USENY` AS `PART_USE`, 
                        `TM_A`.`PART_ENCNO`, `TM_A`.`PART_ECN_DATE`, 
                        tspart.`CREATE_DTM` AS `Creation_date`, tspart.`CREATE_USER` AS `Creation_User`, 
                        tspart.`UPDATE_DTM` AS `Update_Date`, tspart.`UPDATE_USER` AS `UPDATE_USER` 
                        FROM tspart LEFT JOIN 
                        (SELECT `TB_A`.`NO`, `TB_A`.`PART_IDX`, `TB_A`.`PART_ENCNO`, `TB_A`.`PART_ECN_DATE` 
                        FROM (SELECT ROW_NUMBER() OVER (PARTITION BY tspart_ecn.`PART_IDX` ORDER BY  tspart_ecn.`PART_ECN_DATE` DESC) AS `NO`, 
                        tspart_ecn.`PART_IDX`, tspart_ecn.`PART_ENCNO`, tspart_ecn.`PART_ECN_DATE`, tspart_ecn.`PART_ECN_USENY`  FROM tspart_ecn 
                        WHERE tspart_ecn.`PART_ECN_USENY` = 'Y' ) `TB_A` WHERE `TB_A`.`NO` = '1') `TM_A` ON tspart.`PART_IDX` = `TM_A`.`PART_IDX` 
                        WHERE tspart.`PART_NO` LIKE '" + AA + "' AND tspart.`PART_NM` LIKE '" + BB + "'  AND tspart.`PART_CAR` LIKE  '" + FF + "' AND tspart.`PART_FML` LIKE '" + GG + "' AND tspart.`BOM_GRP` LIKE '" + DD + "'      " + CC + " ORDER BY CODE DESC";

                            //if (BaseParameter.ID != 0)
                            //{
                            //    sql = sql + " LIMIT " + GlobalHelper.ListCount;
                            //}
                            //else
                            //{
                            //    sql = sql + " LIMIT 2000";
                            //}

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
                        if (BaseParameter.ListSearchString != null)
                        {
                            string ComboBox3 = BaseParameter.ListSearchString[0];
                            string TextBox10 = BaseParameter.ListSearchString[1];
                            string sql = @"SELECT tspart.`PART_IDX`, tspart.`PART_NO`  FROM  tspart  WHERE tspart.`PART_USENY` ='Y' AND   tspart.`PART_SCN` LIKE '%" + ComboBox3 + "%'  AND tspart.`PART_NO` LIKE '%" + TextBox10 + "%'  ";
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
                            string AAA = BaseParameter.ListSearchString[0].Replace("'", "");
                            string BBB = BaseParameter.ListSearchString[1].Replace("'", "");
                            string CCC = BaseParameter.ListSearchString[2];
                            string DDD = BaseParameter.ListSearchString[3];
                            string EEE = BaseParameter.ListSearchString[4];
                            string FFF = BaseParameter.ListSearchString[5];
                            string GGG = BaseParameter.ListSearchString[6];
                            string KKK = BaseParameter.ListSearchString[7];
                            string HHH = BaseParameter.ListSearchString[8];
                            string MMM = BaseParameter.ListSearchString[9];

                            if (string.IsNullOrEmpty(HHH))
                            {
                                HHH = "0";
                            }

                            if (KKK == "-00-")
                            {
                                KKK = " -  -";
                            }
                            int LOC_IDX_CODE = GlobalHelper.InitializationNumber;
                            if (FFF == "5")
                            {
                                LOC_IDX_CODE = 1;
                            }
                            if (FFF == "6")
                            {
                                LOC_IDX_CODE = 2;
                            }

                            string sql = @"INSERT INTO tspart (`PART_NO`, `PART_NM`, `PART_CAR`, `PART_FML`, `PART_SNP`, `PART_SCN`, `PART_USENY`, `BOM_GRP`, `PART_SUPL`, `CREATE_DTM`, `CREATE_USER`, `PART_LOC`) VALUES ('" + AAA + "', '" + BBB + "', '" + CCC + "', '" + DDD + "', " + EEE + ", " + FFF + ", '" + GGG + "', '" + HHH + "', '" + MMM + "', NOW(), '" + USER_ID + "', '" + KKK + "') ON DUPLICATE KEY UPDATE `PART_NM`= '" + BBB + "', `PART_CAR`= '" + CCC + "', `PART_FML`= '" + DDD + "', `PART_SNP`= " + EEE + ", `PART_SCN`= " + FFF + ",  `BOM_GRP`= '" + HHH + "', `PART_SUPL`= '" + MMM + "', `PART_USENY`= '" + GGG + "', `UPDATE_DTM`= VALUES(`CREATE_DTM`), `UPDATE_USER`= VALUES(`CREATE_USER`), `PART_LOC`= '" + KKK + "'";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"ALTER TABLE     `tspart`     AUTO_INCREMENT= 1";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"INSERT INTO tiivtr (`PART_IDX`, `LOC_IDX`, `QTY`, `CREATE_DTM`, `CREATE_USER`) VALUES ((SELECT tspart.`PART_IDX` FROM tspart WHERE tspart.`PART_NO` = '" + AAA + "'), " + LOC_IDX_CODE + ", 0, NOW(), '" + USER_ID + "')  ON DUPLICATE KEY UPDATE `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);

                            sql = @"ALTER TABLE     `tiivtr`     AUTO_INCREMENT= 1";
                            await MySQLHelper.ExecuteNonQueryAsync(GlobalHelper.MariaDBConectionString, sql);
                        }
                    }
                    if (BaseParameter.Action == 2)
                    {
                        if (BaseParameter.ListSearchString != null)
                        {
                            string Label25 = BaseParameter.ListSearchString[0];
                            string AAA = BaseParameter.ListSearchString[1].Replace("'", "");
                            string BBB = DateTime.Parse(BaseParameter.ListSearchString[2]).ToString("yyyy-MM-dd");
                            string CCC = BaseParameter.ListSearchString[3].Trim();
                            string sql = @"INSERT INTO `tspart_ecn` (`PART_IDX`, `PART_ENCNO`, `PART_ECN_DATE`, `DWG_NO`, `PART_ECN_USENY`, `CREATE_DTM`, `CREATE_USER`) VALUES ('" + Label25 + "', '" + AAA + "', '" + BBB + "', '" + CCC + "', 'Y', NOW(), '" + USER_ID + "') ON DUPLICATE KEY UPDATE `PART_ECN_DATE` = VALUES(`PART_ECN_DATE`), `DWG_NO` = VALUES(`DWG_NO`), `UPDATE_DTM` = VALUES(`CREATE_DTM`), `UPDATE_USER` = VALUES(`CREATE_USER`)";
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
                        string USER_ID = BaseParameter.USER_ID;
                        if (BaseParameter.ListSearchString != null)
                        {
                            var P_INDEX = BaseParameter.ListSearchString[0];
                            var FULL_FILE_PATH = BaseParameter.ListSearchString[1];
                            var FULL_FILE = BaseParameter.ListSearchString[2];
                            var ftpUser = "ysj4947";
                            var ftpPassword = "Kh0ngx0a@";
                            string sql = @"UPDATE `tspart_ecn` SET `DWG_FILE_GRP`= '', `DWG_FILE_EXPOR` ='', `UPDATE_DTM`= NOW(), `UPDATE_USER`= '" + USER_ID + "' WHERE  `PARTECN_IDX`= '" + P_INDEX + "'   ";
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
                BaseParameter.ID = 0;
                result = await Buttonfind_Click(BaseParameter);
                string SheetName = this.GetType().Name;
                string fileName = SheetName + @"_" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                var streamExport = new MemoryStream();
                InitializationToExcelAsync(result.DataGridView1, streamExport, SheetName);
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
                if (BaseParameter != null)
                {
                    result = await Buttonfind_Click(BaseParameter);
                    string SheetName = this.GetType().Name;
                    string fileName = SheetName + @"_" + GlobalHelper.InitializationDateTimeCode + ".html";
                    string contentHTML = GlobalHelper.InitializationString;
                    string physicalPathOpen = Path.Combine(_WebHostEnvironment.WebRootPath, GlobalHelper.HTML, "HTML.html");
                    using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                    {
                        using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                        {
                            contentHTML = r.ReadToEnd();
                        }
                    }
                    contentHTML = contentHTML.Replace("[Name]", "품목리스트 [" + SheetName + "]");
                    contentHTML = contentHTML.Replace("[Day]", GlobalHelper.InitializationDateTime.ToString("dd/MM/yyyy hh:mm:ss"));
                    StringBuilder Content = new StringBuilder();
                    Content.AppendLine(InitializationToHTMLAsync(result.DataGridView1));
                    contentHTML = contentHTML.Replace("[Content]", Content.ToString());
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
                    result = new BaseResult();
                    result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + SheetName + "/" + fileName;
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
        private void InitializationToExcelAsync(List<SuperResultTranfer> list, MemoryStream streamExport, string SheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(SheetName);
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "No";
                column = column + 1;

                Type temp = typeof(tspartTranfer);

                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    workSheet.Cells[row, column].Value = pro.Name;
                    column = column + 1;
                }

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

                row = row + 1;

                int stt = 1;
                foreach (var item in list)
                {
                    try
                    {
                        workSheet.Cells[row, 1].Value = stt;
                        workSheet.Cells[row, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        int columnSub = 2;
                        foreach (PropertyInfo pro in temp.GetProperties())
                        {
                            workSheet.Cells[row, columnSub].Value = pro.GetValue(item);
                            columnSub = columnSub + 1;
                        }

                        for (int i = 1; i < column; i++)
                        {
                            workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                            workSheet.Cells[row, i].Style.Font.Size = 14;
                            workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        }
                        stt = stt + 1;
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


        private string InitializationToHTMLAsync(List<SuperResultTranfer> list)
        {
            Type temp = typeof(SuperResultTranfer);
            StringBuilder Content = new StringBuilder();
            Content.AppendLine(@"<table class=""border"">");
            Content.AppendLine(@"<thead>");
            Content.AppendLine(@"<tr>");
            Content.AppendLine(@"<th>Code</th>");
            Content.AppendLine(@"<th>Part NO</th>");
            Content.AppendLine(@"<th>Part Name</th>");
            Content.AppendLine(@"<th>Car</th>");
            Content.AppendLine(@"<th>Family</th>");
            Content.AppendLine(@"<th>Unit</th>");
            Content.AppendLine(@"<th>Type</th>");
            Content.AppendLine(@"<th>Location</th>");
            Content.AppendLine(@"</tr>");
            Content.AppendLine(@"</thead>");
            Content.AppendLine(@"<tbody>");
            foreach (var item in list)
            {
                Content.AppendLine(@"<tr>");
                Content.AppendLine(@"<td>" + item.CODE + "</td>");
                Content.AppendLine(@"<td>" + item.PART_NO + "</td>");
                Content.AppendLine(@"<td>" + item.PART_NAME + "</td>");
                Content.AppendLine(@"<td>" + item.MODEL + "</td>");
                Content.AppendLine(@"<td>" + item.PART_FamilyPC + "</td>");
                Content.AppendLine(@"<td>" + item.Packing_Unit + "</td>");
                Content.AppendLine(@"<td>" + item.Item_TypeK + "</td>");
                Content.AppendLine(@"<td>" + item.Location + "</td>");
                Content.AppendLine(@"</tr>");
            }
            Content.AppendLine(@"</tbody>");
            Content.AppendLine(@"</table>");
            return Content.ToString();
        }

        public virtual async Task<BaseResult> DGV2_LOAD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.SearchString != null)
                    {
                        string P_IDX = BaseParameter.SearchString;
                        string sql = @"SELECT 
                    (SELECT `PART_NO` FROM tspart WHERE tspart.PART_IDX = tspart_ecn.`PART_IDX`) AS `PART_NO`, 
                    (SELECT `PART_NM` FROM tspart WHERE tspart.PART_IDX = tspart_ecn.`PART_IDX`) AS `PART_NAME`, 
                    tspart_ecn.`PART_ECN_DATE`, tspart_ecn.`PART_ENCNO`, IFNULL(tspart_ecn.`DWG_NO`, '') AS `DWG_NO`, 
                    'PDF Viewer' AS `FVWR`,
                    IFNULL(tspart_ecn.`DWG_FILE_GRP`, '') AS `DWG_FILE_GRP`,
                    'FILE_Down' AS `ADDFILE`,
                    tspart_ecn.`CREATE_DTM`, tspart_ecn.`CREATE_USER`, tspart_ecn.`UPDATE_DTM`, tspart_ecn.`UPDATE_USER`, tspart_ecn.`PARTECN_IDX`,
                    IFNULL(tspart_ecn.`DWG_FILE_EXPOR`, '') AS `DWG_FILE_EXPOR`
                    FROM tspart_ecn
                    WHERE tspart_ecn.`PART_ECN_USENY` = 'Y' AND tspart_ecn.`PART_IDX` = '" + P_IDX + "' ORDER BY tspart_ecn.`PART_ECN_DATE` DESC , tspart_ecn.`PART_ENCNO`   DESC    ";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.DataGridView3 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.DataGridView3.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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
        public virtual async Task<BaseResult> CB_ADD(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                if (BaseParameter != null)
                {
                    if (BaseParameter.SearchString != null)
                    {
                        string SELECT_V = BaseParameter.SearchString;
                        string sql = @"SELECT tspart.`PART_CAR` FROM tspart   WHERE tspart.PART_SCN = '" + SELECT_V + "'    GROUP BY tspart.`PART_CAR`";

                        DataSet ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.ComboBox1 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.ComboBox1.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
                        }

                        sql = @"SELECT tspart.`PART_FML` FROM tspart   WHERE tspart.PART_SCN = '" + SELECT_V + "'    GROUP BY tspart.`PART_FML`";

                        ds = await MySQLHelper.FillDataSetBySQLAsync(GlobalHelper.MariaDBConectionString, sql);
                        result.ComboBox2 = new List<SuperResultTranfer>();
                        for (int i = 0; i < ds.Tables.Count; i++)
                        {
                            DataTable dt = ds.Tables[i];
                            result.ComboBox2.AddRange(SQLHelper.ToList<SuperResultTranfer>(dt));
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

