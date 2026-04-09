namespace MESService.Implement
{
    public class A03Service : IA03Service
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public A03Service(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<BaseResult> Load()
        {
            BaseResult result = new BaseResult();
            try
            {
                var today = DateTime.Today;
                result.Data = new
                {
                    MinDate = today.ToString("yyyy-MM-01"),
                    MaxDate = today.ToString("yyyy-MM-") + DateTime.DaysInMonth(today.Year, today.Month)
                };
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
                string searchString = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 0 ? BaseParameter.ListSearchString[0] : "";
                string startDate = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 1 ? BaseParameter.ListSearchString[1] : DateTime.Today.ToString("yyyy-MM-01");
                string endDate = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 2 ? BaseParameter.ListSearchString[2] : DateTime.Today.ToString("yyyy-MM-dd");

                string sql = @"SELECT tsnotice.`Notice_IDX` AS `NO`, tsnotice.`Title`, tsnotice.`CREATE_DTM` AS `DATE`, 
                              tsnotice.`Contents`, (SELECT tsuser.`USER_NM` FROM tsuser WHERE tsuser.`USER_ID` = tsnotice.`CREATE_USER`) AS `CREATE_USER`
                              FROM tsnotice
                              WHERE tsnotice.`CREATE_DTM` >= @StartDate AND tsnotice.`CREATE_DTM` <= @EndDate 
                              AND tsnotice.`Title` LIKE @SearchString
                              ORDER BY tsnotice.`Notice_IDX` DESC
                              LIMIT 1000";

                using (var connection = new MySqlConnection(GlobalHelper.MariaDBConectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate);
                        command.Parameters.AddWithValue("@EndDate", endDate);
                        command.Parameters.AddWithValue("@SearchString", "%" + searchString + "%");

                        using (var adapter = new MySqlDataAdapter(command))
                        {
                            DataSet ds = new DataSet();
                            adapter.Fill(ds);
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

        public async Task<BaseResult> Buttonadd_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result.Data = new
                {
                    Notice_IDX = 0,
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    Title = "",
                    Contents = ""
                };
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<BaseResult> Buttonsave_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string noticeIdx = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 0 ? BaseParameter.ListSearchString[0] : "0";
                string title = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 1 ? BaseParameter.ListSearchString[1] : "";
                string contents = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 2 ? BaseParameter.ListSearchString[2] : "";
                string createDtm = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 3 ? BaseParameter.ListSearchString[3] : DateTime.Now.ToString("yyyy-MM-dd");
                string createUser = BaseParameter.USER_IDX ?? "";

                string sql = noticeIdx == "0"
                    ? @"INSERT INTO tsnotice (`Title`, `Contents`, `CREATE_DTM`, `CREATE_USER`)
                        VALUES (@Title, @Contents, @CreateDtm, @CreateUser)"
                    : @"INSERT INTO tsnotice (`Notice_IDX`, `Title`, `Contents`, `CREATE_DTM`, `CREATE_USER`)
                        VALUES (@NoticeIdx, @Title, @Contents, @CreateDtm, @CreateUser)
                        ON DUPLICATE KEY UPDATE 
                        `Title` = @Title, `Contents` = @Contents, 
                        `CREATE_DTM` = @CreateDtm, `CREATE_USER` = @CreateUser";

                using (var connection = new MySqlConnection(GlobalHelper.MariaDBConectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NoticeIdx", noticeIdx);
                        command.Parameters.AddWithValue("@Title", title);
                        command.Parameters.AddWithValue("@Contents", contents);
                        command.Parameters.AddWithValue("@CreateDtm", createDtm);
                        command.Parameters.AddWithValue("@CreateUser", createUser);
                        await command.ExecuteNonQueryAsync();
                    }

                    using (var command = new MySqlCommand("ALTER TABLE tsnotice AUTO_INCREMENT = 1", connection))
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }

                result.Message = "Saving completed.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<BaseResult> Buttondelete_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                string noticeIdx = BaseParameter.ListSearchString != null && BaseParameter.ListSearchString.Count > 0 ? BaseParameter.ListSearchString[0] : "0";
                if (noticeIdx == "0")
                {
                    result.Message = "No notice selected.";
                    return result;
                }

                string sql = @"DELETE FROM tsnotice WHERE `Notice_IDX` = @NoticeIdx";

                using (var connection = new MySqlConnection(GlobalHelper.MariaDBConectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@NoticeIdx", noticeIdx);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                result.Message = "Done. The selected Notice data.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<BaseResult> Buttoncancel_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result.Data = new
                {
                    Notice_IDX = "",
                    Title = "",
                    Contents = ""
                };
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<BaseResult> Buttoninport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result.Error = "Excel import not implemented.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<BaseResult> Buttonexport_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result = await Buttonfind_Click(BaseParameter);
                string sheetName = this.GetType().Name;
                string fileName = sheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".xlsx";
                var streamExport = new MemoryStream();
                InitializationToExcelAsync(result.DataGridView1, streamExport, sheetName);
                string physicalPathCreate = Path.Combine(_webHostEnvironment.WebRootPath, GlobalHelper.Download, sheetName);
                Directory.CreateDirectory(physicalPathCreate);
                GlobalHelper.DeleteFilesByPath(physicalPathCreate);
                string filePath = Path.Combine(physicalPathCreate, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    streamExport.CopyTo(stream);
                }
                result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + sheetName + "/" + fileName;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<BaseResult> Buttonprint_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result = await Buttonfind_Click(BaseParameter);
                string sheetName = this.GetType().Name;
                string fileName = sheetName + "_" + GlobalHelper.InitializationDateTimeCode + ".html";
                string contentHTML = GlobalHelper.InitializationString;
                string physicalPathOpen = Path.Combine(_webHostEnvironment.WebRootPath, GlobalHelper.HTML, "HTML.html");
                using (FileStream fs = new FileStream(physicalPathOpen, FileMode.Open))
                {
                    using (StreamReader r = new StreamReader(fs, Encoding.UTF8))
                    {
                        contentHTML = r.ReadToEnd();
                    }
                }
                contentHTML = contentHTML.Replace("[Name]", sheetName);
                contentHTML = contentHTML.Replace("[Day]", GlobalHelper.InitializationDateTime.ToString("dd/MM/yyyy hh:mm:ss"));
                StringBuilder content = new StringBuilder();
                content.AppendLine(InitializationToHTMLAsync(result.DataGridView1));
                contentHTML = contentHTML.Replace("[Content]", content.ToString());
                string physicalPathCreate = Path.Combine(_webHostEnvironment.WebRootPath, GlobalHelper.Download, sheetName);
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
                result.Code = GlobalHelper.URLSite + "/" + GlobalHelper.Download + "/" + sheetName + "/" + fileName;
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<BaseResult> Buttonhelp_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result.Code = "/WMP_PLAY";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        public async Task<BaseResult> Buttonclose_Click(BaseParameter BaseParameter)
        {
            BaseResult result = new BaseResult();
            try
            {
                result.Message = "Form closed.";
            }
            catch (Exception ex)
            {
                result.Error = ex.Message;
            }
            return result;
        }

        private void InitializationToExcelAsync(List<SuperResultTranfer> list, MemoryStream streamExport, string sheetName)
        {
            using (var package = new ExcelPackage(streamExport))
            {
                var workSheet = package.Workbook.Worksheets.Add(sheetName);
                int row = 1;
                int column = 1;
                workSheet.Cells[row, column].Value = "No"; column++;
                workSheet.Cells[row, column].Value = "Title"; column++;
                workSheet.Cells[row, column].Value = "Date"; column++;
                workSheet.Cells[row, column].Value = "Contents"; column++;
                workSheet.Cells[row, column].Value = "Create User";

                for (int i = 1; i <= column; i++)
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

                row++;
                int stt = 1;
                foreach (var item in list)
                {
                    column = 1;
                    workSheet.Cells[row, column].Value = stt; column++;
                    workSheet.Cells[row, column].Value = item.Title; column++;
                    workSheet.Cells[row, column].Value = item.DATE?.ToString("yyyy-MM-dd"); column++;
                    workSheet.Cells[row, column].Value = item.Contents; column++;
                    workSheet.Cells[row, column].Value = item.CREATE_USER;

                    for (int i = 1; i <= column; i++)
                    {
                        workSheet.Cells[row, i].Style.Font.Name = "Times New Roman";
                        workSheet.Cells[row, i].Style.Font.Size = 14;
                        workSheet.Cells[row, i].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[row, i].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    }
                    stt++;
                    row++;
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
            StringBuilder content = new StringBuilder();
            content.AppendLine(@"<table class=""border"">");
            content.AppendLine(@"<thead>");
            content.AppendLine(@"<tr>");
            content.AppendLine(@"<th>No</th>");
            content.AppendLine(@"<th>Title</th>");
            content.AppendLine(@"<th>Date</th>");
            content.AppendLine(@"<th>Contents</th>");
            content.AppendLine(@"<th>Create User</th>");
            content.AppendLine(@"</tr>");
            content.AppendLine(@"</thead>");
            content.AppendLine(@"<tbody>");
            int stt = 0;
            foreach (var item in list)
            {
                stt++;
                content.AppendLine(@"<tr>");
                content.AppendLine(@"<td>" + stt + "</td>");
                content.AppendLine(@"<td>" + item.Title + "</td>");
                content.AppendLine(@"<td>" + item.DATE?.ToString("yyyy-MM-dd") + "</td>");
                content.AppendLine(@"<td>" + item.Contents + "</td>");
                content.AppendLine(@"<td>" + item.CREATE_USER + "</td>");
                content.AppendLine(@"</tr>");
            }
            content.AppendLine(@"</tbody>");
            content.AppendLine(@"</table>");
            return content.ToString();
        }
    }
}